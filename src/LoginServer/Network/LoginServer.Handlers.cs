// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aura.Data;
using Aura.Login.Database;
using Aura.Login.Util;
using Aura.Shared.Const;
using Aura.Shared.Database;
using Aura.Shared.Network;
using Aura.Shared.Util;

namespace Aura.Login.Network
{
	public partial class LoginServer : BaseServer<LoginClient>
	{
		protected override void OnServerStartUp()
		{
			this.RegisterPacketHandler(Op.ClientIdent, HandleVersionCheck);
			this.RegisterPacketHandler(Op.Login, HandleLogin);
			this.RegisterPacketHandler(Op.EnterGame, HandleEnterGame);
			this.RegisterPacketHandler(Op.Disconnect, HandleDisconnect);
			this.RegisterPacketHandler(Op.NameCheck, HandleCheckName);
			this.RegisterPacketHandler(Op.AccountInfoRequest, HandleAccountInfoRequest);

			this.RegisterPacketHandler(Op.CharInfoRequest, HandleCharacterInfoRequest);
			this.RegisterPacketHandler(Op.CreateCharacter, HandleCreateCharacter);
			this.RegisterPacketHandler(Op.DeleteCharRequest, HandleDeletePC);
			this.RegisterPacketHandler(Op.DeleteChar, HandleDeletePC);
			this.RegisterPacketHandler(Op.RecoverChar, HandleDeletePC);

			// Partners are considered Pets, aside from CreatePartner.
			this.RegisterPacketHandler(Op.PetInfoRequest, HandleCharacterInfoRequest);
			this.RegisterPacketHandler(Op.CreatePet, HandleCreatePet);
			this.RegisterPacketHandler(Op.CreatePartner, HandleCreatePartner);
			this.RegisterPacketHandler(Op.DeletePetRequest, HandleDeletePC);
			this.RegisterPacketHandler(Op.DeletePet, HandleDeletePC);
			this.RegisterPacketHandler(Op.RecoverPet, HandleDeletePC);

			// Sent when entering Pet/Partner creation.
			this.RegisterPacketHandler(Op.CreatingPet, HandleEnterPetCreation);
			this.RegisterPacketHandler(Op.CreatingPartner, HandleEnterPetCreation);

			this.RegisterPacketHandler(Op.AcceptGift, HandleAcceptGift);
			this.RegisterPacketHandler(Op.RefuseGift, HandleRefuseGift);

			this.RegisterPacketHandler(Op.Internal.ChannelStatus, HandleChannelStatus);
			this.RegisterPacketHandler(Op.Internal.ServerIdentify, HandleServerIdentify);
		}

		private void HandleVersionCheck(LoginClient client, MabiPacket packet)
		{
			var response = new MabiPacket(Op.ClientIdentR, Id.Login);

			response.PutByte(1);
			response.PutLong(0x49534E4F47493A5D);

			client.Send(response);
		}

#pragma warning disable 0162
		private void HandleLogin(LoginClient client, MabiPacket packet)
		{
			var loginType = (LoginType)packet.GetByte(); // Known: 0x00 (KR), 0x0C, 0x12 (EU) (Normal), 0x5 (New), 0x2 (Coming from channel)
			var username = packet.GetString();
			var password = "";
			ulong sessionKey = 0;

			switch (loginType)
			{
				// Normal login, (MD5) password
				case LoginType.Normal:
				case LoginType.EU:
				case LoginType.KR:

					var passbin = packet.GetBin();
					password = System.Text.Encoding.UTF8.GetString(passbin);

					if (Op.Version == 140400)
					{
						// EU had no client side hashing. Let's make it compatible to NA.
						password = string.Empty;
						foreach (var chr in passbin.TakeWhile(a => a != 0))
							password += (char)chr;

						var md5 = System.Security.Cryptography.MD5.Create();
						password = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(password))).Replace("-", "");
					}
					break;

				// Logging in, comming from a channel
				case LoginType.FromChannel:

					if (Op.Version > 160000)
						packet.GetString(); // Double acc name
					sessionKey = packet.GetLong();
					break;

				// Type 5 uses the new Nexon hash thingy...
				// apart from that, equal to 0x0C.
				case LoginType.NewHash:
				default:
					this.SendLoginResponse(client, Localization.Get("login.new_hash_error"), loginType);
					return;

				// Second password
				case LoginType.SecondPassword:

					//sessionKey = packet.GetLong();
					//secPassword = packet.GetString(); // SSH1

					this.SendLoginResponse(client, Localization.Get("login.second_error")); // Second passwords aren't supported yet.
					return;
			}

			// Create new account
			if (LoginConf.NewAccounts && (username.StartsWith("new//") || username.StartsWith("new__")))
			{
				username = username.Remove(0, 5);

				if (!MabiDb.Instance.AccountExists(username) && password != "")
				{
					LoginDb.Instance.CreateAccount(username, password);
					Logger.Info("New account '{0}' was created.", username);
				}
			}

			// Check account existence
			var account = LoginDb.Instance.GetAccount(username);
			if (account == null)
			{
				this.SendLoginResponse(client, LoginResult.IdOrPassIncorrect);
				return;
			}

			// Check bans
			if (account.BannedExpiration.CompareTo(DateTime.Now) > 0)
			{
				this.SendLoginResponse(client, Localization.Get("login.banned"), account.BannedExpiration, account.BannedReason); // You've been banned, till {0}.\r\nReason: {1}
				return;
			}

			// Check password/session
			if (!BCrypt.CheckPassword(password, account.Password) && !MabiDb.Instance.IsSessionKey(username, sessionKey))
			{
				this.SendLoginResponse(client, LoginResult.IdOrPassIncorrect);
				return;
			}

			// Check logged in already
			if (account.LoggedIn)
			{
				this.SendLoginResponse(client, LoginResult.AlreadyLoggedIn);
				return;
			}

			// Update account
			account.LastIp = client.IP;
			account.LastLogin = DateTime.Now;
			account.LoggedIn = true;
			LoginDb.Instance.UpdateAccount(account);

			// Req. Info
			account.CharacterCards = LoginDb.Instance.GetCharacterCards(username);
			account.PetCards = LoginDb.Instance.GetPetCards(username);
			account.Characters = LoginDb.Instance.GetCharacters(username);
			account.Pets = LoginDb.Instance.GetPets(username);
			account.Gifts = LoginDb.Instance.GetGifts(username);

			// Add free cards if there are none.
			// If you don't have chars and char cards, you get a new free card,
			// if you don't have pets or pet cards either, you'll also get a 7-day horse.
			// TODO: Implement limited pets.
			if (account.CharacterCards.Count < 1 && account.Characters.Count < 1)
			{
				// Free card
				var cardId = MabiDb.Instance.AddCard(username, 147, 0);
				account.CharacterCards.Add(new Card(cardId, 147, 0));

				if (account.PetCards.Count < 1 && account.Pets.Count < 1)
				{
					// 7-day Horse
					cardId = MabiDb.Instance.AddCard(username, Id.PetCardType, 260016);
					account.PetCards.Add(new Card(cardId, Id.PetCardType, 260016));
				}
			}

			// Success Response
			// --------------------------------------------------------------
			var response = new MabiPacket(Op.LoginR, Id.Login);
			response.PutByte((byte)LoginResult.Success);
			response.PutString(username);
			if (Op.Version > 160000)
				response.PutString(username);
			response.PutLong(LoginDb.Instance.CreateSession(username));
			response.PutByte(0);

			// Servers
			// --------------------------------------------------------------
			response.PutByte((byte)this.ServerList.Count);
			foreach (var server in this.ServerList.Values)
				server.AddToPacket(response);

			// Account Info
			// --------------------------------------------------------------
			account.AddToPacket(response);

			client.Account = account;
			client.State = ClientState.LoggedIn;

			client.Send(response);

			Logger.Info("Logging in as '{0}'.", username);
		}
#pragma warning restore 0162

		private void SendLoginResponse(LoginClient client, string format, params object[] args)
		{
			var response = new MabiPacket(Op.LoginR, Id.Login);
			response.PutByte(51);
			response.PutInt(14);
			response.PutInt(1);
			response.PutString(format, args);
			client.Send(response);
		}

		private void SendLoginResponse(LoginClient client, LoginResult result)
		{
			var response = new MabiPacket(Op.LoginR, Id.Login);
			response.PutByte((byte)result);
			client.Send(response);
		}

		private enum LoginResult { Fail = 0, Success = 1, Empty = 2, IdOrPassIncorrect = 3, /* IdOrPassIncorrect = 4, */ TooManyConnections = 6, AlreadyLoggedIn = 7, UnderAge = 33, Banned = 101 }
		private enum LoginType { KR = 0x00, FromChannel = 0x02, NewHash = 0x05, Normal = 0x0C, EU = 0x12, SecondPassword = 0x14 }

		private void HandleCharacterInfoRequest(LoginClient client, MabiPacket packet)
		{
			var serverName = packet.GetString();
			var characterId = packet.GetLong();

			var response = new MabiPacket(packet.Op + 1, Id.Login);
			response.PutByte(1);
			response.PutString(serverName);
			response.PutLong(characterId);

			var character = (packet.Op != Op.PetInfoRequest ? client.Account.GetCharacter(characterId) : client.Account.GetPet(characterId));
			if (character == null)
			{
				// Fail ?
				response.PutByte(0);
				client.Send(response);
				return;
			}

			// Success ?
			response.PutByte(1);

			// Char Info
			response.PutString(character.Name);
			response.PutString("");
			response.PutString("");
			response.PutInt(character.Race);
			response.PutByte(character.SkinColor);
			response.PutByte(character.Eye);
			response.PutByte(character.EyeColor);
			response.PutByte(character.Mouth);
			response.PutInt(0);
			response.PutFloat(character.Height);
			response.PutFloat(character.Weight);
			response.PutFloat(character.Upper);
			response.PutFloat(character.Lower);
			response.PutInt(0);
			response.PutInt(0);
			response.PutInt(0);
			response.PutByte(0);
			response.PutInt(0);
			response.PutByte(0);
			response.PutInt(character.Color1);
			response.PutInt(character.Color2);
			response.PutInt(character.Color3);
			response.PutFloat(0.0f);
			response.PutString("");
			response.PutFloat(49.0f);
			response.PutFloat(49.0f);
			response.PutFloat(0.0f);
			response.PutFloat(49.0f);
			response.PutInt(0);
			response.PutInt(0);
			response.PutShort(0);
			response.PutLong(0);
			response.PutString("");
			response.PutByte(0);

			var items = LoginDb.Instance.GetEquipment(characterId);
			response.PutSInt(items.Count);
			foreach (var item in items)
			{
				response.PutLong(item.Id);
				response.PutBin(item.Info);
			}

			response.PutInt(0);		 // PetRemainingTime
			response.PutLong(0);	 // PetLastTime
			response.PutLong(0);	 // PetExpireTime

			client.Send(response);
		}

		private void HandleCreateCharacter(LoginClient client, MabiPacket packet)
		{
			var serverName = packet.GetString();
			var cardId = packet.GetLong();
			var charName = packet.GetString();
			var charRace = packet.GetInt();
			var skinColor = packet.GetByte();
			var hair = packet.GetInt();
			var hairColor = packet.GetByte();
			var age = packet.GetByte();
			var eye = packet.GetByte();
			var eyeColor = packet.GetByte();
			var mouth = packet.GetByte();
			var face = packet.GetInt();

			var response = new MabiPacket(Op.CharacterCreated, Id.Login);

			var card = client.Account.GetCharacterCard(cardId);
			CharCardInfo cardInfo = null;

			if (card != null)
			{
				// Check if this is a valid card and if this race can use it
				cardInfo = MabiData.CharCardDb.Find(card.Type);
				if (cardInfo == null || !cardInfo.Races.Contains(charRace))
				{
					// Reset card, so the request fails
					card = null;
				}
			}

			var faceItem = MabiData.ItemDb.Find(face);
			var hairItem = MabiData.ItemDb.Find(hair);

			if (card == null || !MabiDb.Instance.NameOkay(charName, serverName) || faceItem == null || hairItem == null || (faceItem.Type & ~1) != 100 || (hairItem.Type & ~1) != 100)
			{
				// Fail
				response.PutByte(0);
				client.Send(response);
				return;
			}

			// Create character
			var character = new Character(CharacterType.Character);
			character.Name = charName;
			character.Race = charRace;
			character.SkinColor = skinColor;
			character.Eye = eye;
			character.EyeColor = eyeColor;
			character.Mouth = mouth;
			character.Age = age;
			character.Server = serverName;
			character.Height = (1.0f / 7.0f * (age - 10.0f));

			character.Region = LoginConf.SpawnRegion;
			character.X = LoginConf.SpawnX;
			character.Y = LoginConf.SpawnY;

			var ageInfo = MabiData.StatsBaseDb.Find(character.Race, character.Age);
			if (ageInfo == null)
				Logger.Warning("Unable to find age info for race '{0}', age '{1}'.", character.Race, character.Age);
			else
			{
				character.Life = ageInfo.Life;
				character.Mana = ageInfo.Mana;
				character.Stamina = ageInfo.Stamina;
				character.Str = ageInfo.Str;
				character.Int = ageInfo.Int;
				character.Dex = ageInfo.Dex;
				character.Will = ageInfo.Will;
				character.Luck = ageInfo.Luck;
				character.AP = ageInfo.AP;
			}

			var characterId = LoginDb.Instance.CreateCharacter(client.Account.Name, character);
			client.Account.Characters.Add(character);

			// Retrieve start items and add head.
			var cardItems = MabiData.CharCardSetDb.Find(cardInfo.SetId, charRace);
			this.RandomizeItemColors(ref cardItems, (client.Account.Name + charRace + skinColor + hair + hairColor + age + eye + eyeColor + mouth + face));
			cardItems.Add(new CharCardSetInfo { Class = face, Pocket = 3, Race = charRace, Color1 = skinColor });
			cardItems.Add(new CharCardSetInfo { Class = hair, Pocket = 4, Race = charRace, Color1 = (uint)hairColor + 0x10000000 });
			LoginDb.Instance.AddItems(characterId, cardItems);

			// Add beginner keywords
			LoginDb.Instance.AddKeywords(characterId, 1, 2, 3, 37, 38);

			// Skills
			LoginDb.Instance.AddSkills(characterId,
				new Skill(SkillConst.MeleeCombatMastery, SkillRank.RF),
				new Skill(SkillConst.HiddenEnchant),
				new Skill(SkillConst.HiddenResurrection),
				new Skill(SkillConst.HiddenTownBack),
				new Skill(SkillConst.HiddenGuildstoneSetting),
				new Skill(SkillConst.HiddenBlessing),
				new Skill(SkillConst.CampfireKit),
				new Skill(SkillConst.SkillUntrainKit),
				new Skill(SkillConst.BigBlessingWaterKit),
				new Skill(SkillConst.Dye),
				new Skill(SkillConst.EnchantElementalAllSlot),
				new Skill(SkillConst.HiddenPoison),
				new Skill(SkillConst.HiddenBomb),
				new Skill(SkillConst.FossilRestoration),
				new Skill(SkillConst.SeesawJump),
				new Skill(SkillConst.SeesawCreate),
				new Skill(SkillConst.DragonSupport),
				new Skill(SkillConst.IceMineKit),
				new Skill(SkillConst.Scan),
				new Skill(SkillConst.UseSupportItem),
				new Skill(SkillConst.UseAntiMacroItem),
				new Skill(SkillConst.ItemSeal),
				new Skill(SkillConst.ItemUnseal),
				new Skill(SkillConst.ItemDungeonPass),
				new Skill(SkillConst.UseElathaItem),
				new Skill(SkillConst.UseMorrighansFeather),
				new Skill(SkillConst.PetBuffing),
				new Skill(SkillConst.CherryTreeKit),
				new Skill(SkillConst.Pollen),
				new Skill(SkillConst.Firecracker),
				new Skill(SkillConst.FeedFish),
				new Skill(SkillConst.HammerGame),
				new Skill(SkillConst.SoulStone),
				new Skill(SkillConst.UseItemBomb2),
				new Skill(SkillConst.NameColorChange),
				new Skill(SkillConst.HolyFire),
				new Skill(SkillConst.MakeFaliasPortal),
				new Skill(SkillConst.UseItemChattingColorChange),
				new Skill(SkillConst.InstallFacility),
				new Skill(SkillConst.RedesignFacility),
				new Skill(SkillConst.GachaponSynthesis),
				new Skill(SkillConst.MakeChocoStatue),
				new Skill(SkillConst.Painting),
				new Skill(SkillConst.PaintMixing),
				new Skill(SkillConst.PetSealToItem),
				new Skill(SkillConst.FlownHotAirBalloon),
				new Skill(SkillConst.ItemSeal2),
				new Skill(SkillConst.CureZombie),
				new Skill(SkillConst.WarpContinent),
				new Skill(SkillConst.AddSeasoning)
			);

			// Success
			response.PutByte(1);
			response.PutString(serverName);
			response.PutLong(characterId);

			client.Send(response);

			// Remove card
			if (LoginConf.ConsumeCards)
			{
				client.Account.CharacterCards.Remove(card);
				LoginDb.Instance.DeleteCard(client.Account.Name, cardId);
			}

			Logger.Info("New character: " + character.Name);
		}

		private void RandomizeItemColors(ref List<CharCardSetInfo> cardItems, string hash)
		{
			int ihash = 5381;
			foreach (var ch in hash)
				ihash = ihash * 33 + (int)ch;

			var rnd = new MTRandom(ihash);
			foreach (var item in cardItems)
			{
				var dataInfo = MabiData.ItemDb.Find(item.Class);
				if (dataInfo == null)
					continue;

				item.Color1 = (item.Color1 != 0 ? item.Color1 : MabiData.ColorMapDb.GetRandom(dataInfo.ColorMap1, rnd));
				item.Color2 = (item.Color2 != 0 ? item.Color2 : MabiData.ColorMapDb.GetRandom(dataInfo.ColorMap2, rnd));
				item.Color3 = (item.Color3 != 0 ? item.Color3 : MabiData.ColorMapDb.GetRandom(dataInfo.ColorMap3, rnd));
			}
		}

		private void HandleEnterGame(LoginClient client, MabiPacket packet)
		{
			var serverName = packet.GetString();
			var channelName = packet.GetString();
			var unk1 = packet.GetByte();
			var unk2 = packet.GetInt();
			var characterId = packet.GetLong();

			MabiServer server = null;
			MabiChannel channel = null;

			if (this.ServerList.Count > 0)
			{
				server = this.ServerList.Values.FirstOrDefault(a => a.Name == serverName);
				if (server != null)
				{
					channel = server.Channels.Values.FirstOrDefault(a => a.Name == channelName);
				}
			}

			var response = new MabiPacket(Op.ChannelInfo, Id.World);

			if (channel == null)
			{
				// Fail
				response.PutByte(0);
			}
			else
			{
				// Success
				response.PutByte(1);
				response.PutString(server.Name);
				response.PutString(channel.Name);
				response.PutShort(6);
				response.PutString(channel.IP);
				response.PutString(channel.IP);
				response.PutShort(channel.Port);
				response.PutShort((ushort)(channel.Port + 2));
				response.PutInt(unk2);
				response.PutLong(characterId);
			}

			client.Send(response);
		}

		private void HandleDeletePC(LoginClient client, MabiPacket packet)
		{
			var serverName = packet.GetString();
			var id = packet.GetLong();
			//var charName = packet.GetString();

			bool isPet = (packet.Op >= Op.DeletePetRequest);

			var character = (isPet ? client.Account.GetPet(id) : client.Account.GetCharacter(id));

			// The response op is always +1.
			uint op = packet.Op + 1;

			var response = new MabiPacket(op, Id.Login);

			if (character == null || ((op == Op.DeleteCharR || op == Op.DeletePetR) && character.DeletionTime > DateTime.Now))
			{
				// Fail
				response.PutByte(0);
			}
			else
			{
				// Success
				response.PutByte(1);
				response.PutString(serverName);
				response.PutLong(id);
				response.PutLong(0);

				if (
					(packet.Op == Op.DeleteChar || packet.Op == Op.DeletePet) ||
					((packet.Op == Op.DeleteCharRequest || packet.Op == Op.DeletePetRequest) && LoginConf.DeletionWait == 0)
				)
				{
					// Mark for deletion
					character.DeletionTime = DateTime.MaxValue;
					if (!isPet)
						client.Account.Characters.Remove(character);
					else
						client.Account.Pets.Remove(character);
				}
				else if (packet.Op == Op.RecoverChar || packet.Op == Op.RecoverPet)
				{
					// Reset time
					character.DeletionTime = DateTime.MinValue;
				}
				else // Op.DeleteCharRequest || Op.DeletePetRequest || Error?
				{
					// Set time at which the character can be deleted for good.
					// Below 100 means x hours, above 100 tomorrow at x.
					if (LoginConf.DeletionWait > 100)
						character.DeletionTime = (DateTime.Now.AddDays(1).Date + new TimeSpan(LoginConf.DeletionWait - 100, 0, 0));
					else
						character.DeletionTime = DateTime.Now.AddHours(LoginConf.DeletionWait);
				}

				LoginDb.Instance.SetDelete(character);
			}

			client.Send(response);
		}

		private void HandleCheckName(LoginClient client, MabiPacket packet)
		{
			var server = packet.GetString();
			var name = packet.GetString();

			MabiPacket response = new MabiPacket(Op.NameCheckR, Id.Login);

			if (MabiDb.Instance.NameOkay(name, server))
			{
				// Success
				response.PutByte(1);
				response.PutByte(0);
			}
			else
			{
				// Fail
				response.PutByte(0);
				response.PutByte(1);
			}

			client.Send(response);
		}

		private void HandleCreatePet(LoginClient client, MabiPacket packet)
		{
			var serverName = packet.GetString();
			var cardId = packet.GetLong();
			var name = packet.GetString();
			var color1 = packet.GetInt();
			var color2 = packet.GetInt();
			var color3 = packet.GetInt();

			var response = new MabiPacket(Op.PetCreated, Id.Login);

			// Check if the card, name, and race are valid
			var card = client.Account.GetPetCard(cardId);
			if (card == null || !MabiDb.Instance.NameOkay(name, serverName) || !MabiData.PetDb.Has(card.Race))
			{
				// Fail
				response.PutByte(0);
				client.Send(response);
				return;
			}

			// Create new pet
			var pet = new Character(CharacterType.Pet);
			pet.Name = name;
			pet.Race = card.Race;
			pet.Age = 1;
			pet.Server = serverName;
			pet.Region = LoginConf.SpawnRegion;
			pet.X = LoginConf.SpawnX;
			pet.Y = LoginConf.SpawnY;

			var petInfo = MabiData.PetDb.Find(pet.Race);

			pet.Height = petInfo.Height;
			pet.Upper = petInfo.Upper;
			pet.Lower = petInfo.Lower;
			pet.Life = petInfo.Life;
			pet.Mana = petInfo.Mana;
			pet.Stamina = petInfo.Stamina;
			pet.Str = petInfo.Str;
			pet.Int = petInfo.Int;
			pet.Dex = petInfo.Dex;
			pet.Will = petInfo.Will;
			pet.Luck = petInfo.Luck;
			pet.Defense = petInfo.Defense;
			pet.Protection = petInfo.Protection;
			if (color1 > 0 || color2 > 0 || color3 > 0)
			{
				pet.Color1 = color1;
				pet.Color2 = color2;
				pet.Color3 = color3;
			}
			else
			{
				pet.Color1 = petInfo.Color1;
				pet.Color2 = petInfo.Color2;
				pet.Color3 = petInfo.Color3;
			}

			var petId = LoginDb.Instance.CreateCharacter(client.Account.Name, pet);
			client.Account.Pets.Add(pet);

			// Skills
			LoginDb.Instance.AddSkills(petId, new Skill(SkillConst.MeleeCombatMastery, SkillRank.RF));

			// Success
			response.PutByte(1);
			response.PutString(serverName);
			response.PutLong(petId);

			client.Send(response);

			// Remove card
			if (LoginConf.ConsumeCards)
			{
				client.Account.PetCards.Remove(card);
				LoginDb.Instance.DeleteCard(client.Account.Name, cardId);
			}

			Logger.Info("New pet: " + pet.Name);
		}

		private void HandleCreatePartner(LoginClient client, MabiPacket packet)
		{
			var serverName = packet.GetString();
			var cardId = packet.GetLong();
			var name = packet.GetString();
			var unk = packet.GetInt(); // 730204
			var skinColor = packet.GetByte();
			var hair = packet.GetInt();
			var hairColor = packet.GetByte();
			var eye = packet.GetByte();
			var eyeColor = packet.GetByte();
			var mouth = packet.GetByte();
			var face = packet.GetInt();
			var height = packet.GetFloat();
			var weight = packet.GetFloat();
			var upper = packet.GetFloat();
			var lower = packet.GetFloat();
			var personality = packet.GetInt();

			var response = new MabiPacket(Op.PetCreated, Id.Login);

			// Check if the card is valid
			var card = client.Account.GetPetCard(cardId);
			if (card == null || !MabiDb.Instance.NameOkay(name, serverName))
			{
				// Fail
				response.PutByte(0);
				client.Send(response);
				return;
			}

			// Create new partner
			var partner = new Character(CharacterType.Partner);
			partner.Name = name;
			partner.Race = card.Race;
			partner.SkinColor = skinColor;
			partner.Eye = eye;
			partner.EyeColor = eyeColor;
			partner.Mouth = mouth;
			partner.Height = height;
			partner.Weight = weight;
			partner.Upper = upper;
			partner.Lower = lower;
			partner.Server = serverName;
			partner.Region = LoginConf.SpawnRegion;
			partner.X = LoginConf.SpawnX;
			partner.Y = LoginConf.SpawnY;

			var partnerId = LoginDb.Instance.CreateCharacter(client.Account.Name, partner);
			client.Account.Pets.Add(partner);

			uint setId = 0;
			if (card.Race == 730201 || card.Race == 730202 || card.Race == 730204 || card.Race == 730205)
				setId = 1000;
			else if (card.Race == 730203)
				setId = 1001;
			else if (card.Race == 730206)
				setId = 1002;

			// Add set items and head
			var cardItems = MabiData.CharCardSetDb.Find(setId, card.Race);
			this.RandomizeItemColors(ref cardItems, (client.Account.Name + partner.Race + skinColor + hair + hairColor + 1 + eye + eyeColor + mouth + face));
			cardItems.Add(new CharCardSetInfo { Class = face, Pocket = 3, Race = card.Race, Color1 = skinColor });
			cardItems.Add(new CharCardSetInfo { Class = hair, Pocket = 4, Race = card.Race, Color1 = (uint)hairColor + 0x10000000 });
			LoginDb.Instance.AddItems(partnerId, cardItems);

			// Skills
			LoginDb.Instance.AddSkills(partnerId, new Skill(SkillConst.MeleeCombatMastery, SkillRank.RF));

			// Success
			response.PutByte(1);
			response.PutString(serverName);
			response.PutLong(partner.Id);

			client.Send(response);

			// Remove card
			if (LoginConf.ConsumeCards)
			{
				client.Account.PetCards.Remove(card);
				LoginDb.Instance.DeleteCard(client.Account.Name, cardId);
			}

			Logger.Info("New partner: " + partner.Name);
		}

		private void HandleDisconnect(LoginClient client, MabiPacket packet)
		{
			var accountName = packet.GetString();

			if (accountName != client.Account.Name)
				return;

			client.Account.LoggedIn = false;
			LoginDb.Instance.UpdateAccount(client.Account);

			Logger.Info("'{0}' is closing the connection.", accountName);
		}

		private void HandleEnterPetCreation(LoginClient client, MabiPacket packet)
		{
			//Logger.Error("OMG! Someone is trying to create a pet/partner!!");
		}

		private void HandleAcceptGift(LoginClient client, MabiPacket packet)
		{
			var giftId = packet.GetLong();
			var success = false;

			var gift = client.Account.GetGift(giftId);
			if (gift != null)
			{
				client.Account.Gifts.Remove(gift);

				if (gift.IsCharacter)
					client.Account.CharacterCards.Add(gift);
				else
					client.Account.PetCards.Add(gift);

				LoginDb.Instance.ChangeGiftToCard(giftId);
				success = true;
			}

			var response = new MabiPacket(Op.AcceptGiftR, Id.Login);
			response.PutByte(success);
			if (success)
			{
				response.PutByte(gift.IsCharacter); // is char ?
				response.PutInt(0); // ???
				response.PutInt(0); // ???
				response.PutInt(gift.Type); // Type ?
				// ?
			}
			client.Send(response);
		}

		private void HandleRefuseGift(LoginClient client, MabiPacket packet)
		{
			var giftId = packet.GetLong();
			var success = false;

			var gift = client.Account.GetGift(giftId);
			if (gift != null)
			{
				LoginDb.Instance.DeleteCard(client.Account.Name, giftId);
				client.Account.Gifts.Remove(gift);
				success = true;
			}

			var response = new MabiPacket(Op.RefuseGiftR, Id.Login);
			response.PutByte(success);
			if (success)
			{
				// ?
			}
			client.Send(response);
		}

		private void HandleAccountInfoRequest(LoginClient client, MabiPacket packet)
		{
			var response = new MabiPacket(Op.AccountInfoRequestR, Id.Login);
			response.PutByte(true);
			client.Account.AddToPacket(response);

			client.Send(response);
		}

		private void HandleServerIdentify(LoginClient client, MabiPacket packet)
		{
			var pass = packet.GetString();

			if (!BCrypt.CheckPassword(LoginConf.Password, pass))
			{
				client.Send(new MabiPacket(Op.Internal.ServerIdentify).PutByte(false));

				Logger.Warning("Incorrect password from '{0}'.", client.IP);
				client.Kill();
				return;
			}

			client.State = ClientState.LoggedIn;
			lock (this.ChannelClients)
				this.ChannelClients.Add(client);

			client.Send(new MabiPacket(Op.Internal.ServerIdentify).PutByte(true));
		}

		private void HandleChannelStatus(LoginClient client, MabiPacket packet)
		{
			if (client.State != ClientState.LoggedIn)
				return;

			var serverName = packet.GetString();
			var channelName = packet.GetString();
			var host = packet.GetString();
			var port = packet.GetShort();
			var stress = packet.GetByte();

			if (!this.ServerList.ContainsKey(serverName))
			{
				this.ServerList[serverName] = new MabiServer(serverName);
				Logger.Info("Added available server: {0}", serverName);
			}

			if (!this.ServerList[serverName].Channels.ContainsKey(channelName))
			{
				this.ServerList[serverName].Channels[channelName] = new MabiChannel(channelName, host, port, ChannelState.Normal, ChannelEvent.Normal);
				Logger.Info("Added available channel: {0} @ {1}", channelName, serverName);
			}

			this.ServerList[serverName].Channels[channelName].Stress = stress;
			this.ServerList[serverName].Channels[channelName].LastUpdate = DateTime.Now;
		}
	}
}
