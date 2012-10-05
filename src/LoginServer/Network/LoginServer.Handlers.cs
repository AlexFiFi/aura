// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Linq;
using System.Text;
using Common.Constants;
using Common.Data;
using Common.Database;
using Common.Network;
using Common.Tools;
using Common.World;
using Login.Tools;

namespace Login.Network
{
	public partial class LoginServer : Server<LoginClient>
	{
		protected override void InitPacketHandlers()
		{
			this.RegisterPacketHandler(Op.ClientIdent, HandleVersionCheck);
			this.RegisterPacketHandler(Op.Login, HandleLogin);
			this.RegisterPacketHandler(Op.EnterGame, HandleEnterGame);
			this.RegisterPacketHandler(Op.Disconnect, HandleDisconnect);
			this.RegisterPacketHandler(Op.NameCheck, HandleCheckName);

			this.RegisterPacketHandler(Op.CharInfoRequest, HandleCharacterInfoRequest);
			this.RegisterPacketHandler(Op.CreateCharacter, HandleCreateCharacter);
			this.RegisterPacketHandler(Op.DeleteCharRequest, HandleDeletePC);
			this.RegisterPacketHandler(Op.DeleteChar, HandleDeletePC);
			this.RegisterPacketHandler(Op.RecoverChar, HandleDeletePC);

			this.RegisterPacketHandler(Op.PetInfoRequest, HandlePetInfoRequest);
			this.RegisterPacketHandler(Op.CreatePet, HandleCreatePet);
			this.RegisterPacketHandler(Op.CreatePartner, HandleCreatePartner);
			this.RegisterPacketHandler(Op.DeletePetRequest, HandleDeletePC);
			this.RegisterPacketHandler(Op.DeletePet, HandleDeletePC);
			this.RegisterPacketHandler(Op.RecoverPet, HandleDeletePC);

			this.RegisterPacketHandler(Op.CreatingPet, HandleEnterPetCreation);
			this.RegisterPacketHandler(Op.CreatingPartner, HandleEnterPetCreation);
		}

		private void HandleVersionCheck(LoginClient client, MabiPacket packet)
		{
			var response = new MabiPacket(Op.ClientIdentR, 0x1000000000000010);

			response.PutByte(1);
			response.PutLong(0x49534E4F47493A5D); // time?

			client.Send(response);
		}

#pragma warning disable 0162
		private void HandleLogin(LoginClient client, MabiPacket packet)
		{
			var loginType = packet.GetByte(); // Known: 0x00 (KR), 0x0C, 0x12 (EU) (Normal), 0x5 (New), 0x2 (Coming from channel)
			var username = packet.GetString();
			var password = "";
			ulong sessionKey = 0;

			var response = new MabiPacket(Op.LoginR, 0x1000000000000010);

			// Normal login, MD5 password
			if (loginType == 0x0C || loginType == 0x12 || loginType == 0x00)
			{
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
			}
			// Logging in, comming from a channel
			else if (loginType == 0x02)
			{
				if (Op.Version > 160000)
					packet.GetString(); // Double acc name
				sessionKey = packet.GetLong();
			}
			// Type 5 uses the new Nexon hash thingy...
			// apart from that, equal to 0x0C.
			else if (loginType == 0x05)
			{
				response.PutByte(51);
				response.PutInt(14);
				response.PutInt(1);
				response.PutString("Sorry, but something seems to be wrong with your client.\nPlease report this, and try to login using \"new//\".");
				client.Send(response);
				return;
			}
			// Second password?
			else if (loginType == 0x14)
			{
				//sessionKey = packet.GetLong();
				//secPassword = packet.GetString(); ?

				response.PutByte(51);
				response.PutInt(14);
				response.PutInt(1);
				response.PutString("Second passwords aren't supported yet.");
				client.Send(response);
				return;
			}

			var loginResult = LoginResult.Fail;

			MabiAccount account = null;

			if (LoginConf.NewAccounts && (username.StartsWith("new//") || username.StartsWith("new__")))
			{
				username = username.Remove(0, 5);

				if (!MabiDb.Instance.AccountExists(username) && password != "")
				{
					var newAccount = new MabiAccount();
					newAccount.Username = username;
					newAccount.Userpass = BCrypt.HashPassword(password, BCrypt.GenerateSalt(12));
					newAccount.Creation = DateTime.Now;

					MabiDb.Instance.SaveAccount(newAccount);

					Logger.Info("New account '" + username + "' was created.");

					account = newAccount;
				}
			}

			// Load account
			if (account == null)
				account = MabiDb.Instance.GetAccount(username);

			if (account != null)
			{
				if (BCrypt.CheckPassword(password, account.Userpass) || MabiDb.Instance.IsSessionKey(account.Username, sessionKey))
				{
					if (account.LoggedIn)
					{
						Logger.Info("Account '" + account.Username + "' is logged in already.");
						loginResult = LoginResult.AlreadyLoggedIn;
					}
					else if (account.BannedExpiration.CompareTo(DateTime.Now) > 0)
					{
						Logger.Info("Banned! (" + account.Username + ")");
						loginResult = LoginResult.Banned;
					}
					else
					{
						Logger.Info("Logging in as '" + account.Username + "'.");
						account.LoggedIn = true;
						account.LastIp = client.Socket.RemoteEndPoint.ToString();
						account.LastLogin = DateTime.Now;

						// Add free cards if there are none.
						// If you don't have chars and char cards, you get a new free card,
						// if you don't have pets or pet cards either, you'll also get a 7-day horse.
						// TODO: Implement limited pets.
						if (account.CharacterCards.Count < 1 && account.Characters.Count < 1)
						{
							account.CharacterCards.Add(new MabiCard(147)); // Free card

							if (account.PetCards.Count < 1 && account.Pets.Count < 1)
							{
								account.PetCards.Add(new MabiCard(260016)); // Horse
							}
						}

						loginResult = LoginResult.Success;
					}
				}
				else
				{
					Logger.Info("Wrong password (" + account.Username + ")");
					loginResult = LoginResult.IdOrPassIncorrect;
				}
			}
			else
			{
				// Account doesn't exist
				Logger.Info("Account '" + username + "' not found.");
				loginResult = LoginResult.IdOrPassIncorrect;
			}

			if (loginResult == LoginResult.Banned)
			{
				response.PutByte(51);
				response.PutInt(14);
				response.PutInt(1);
				response.PutString("You've been banned, till " + account.BannedExpiration.ToString() + ".\r\nReason: " + account.BannedReason);
				client.Send(response);
				return;
			}

			response.PutByte((byte)loginResult);
			if (loginResult != LoginResult.Success)
			{
				client.Send(response);
				return;
			}

			// Account
			// --------------------------------------------------------------
			response.PutString(account.Username);
			if (Op.Version > 160000)
				response.PutString(account.Username);
			response.PutLong(MabiDb.Instance.CreateSession(account.Username));
			response.PutByte(0x00);

			// Servers
			// --------------------------------------------------------------
			var servers = MabiDb.Instance.GetServerList();
			response.PutByte((byte)servers.Count);
			foreach (var server in servers)
			{
				response.PutString(server.Name);
				response.PutShort(0); // Server type?
				response.PutShort(0);
				response.PutByte(1);

				// Channels
				// ----------------------------------------------------------
				response.PutInt((uint)server.Channels.Count());
				foreach (var channel in server.Channels)
				{
					response.PutString(channel.Name);
					response.PutInt((uint)channel.State);
					response.PutInt((uint)channel.Events);
					response.PutInt(0); // 1 for Housing? Hidden?
					response.PutShort(channel.Stress);
				}
			}

			// Premium services?
			// --------------------------------------------------------------
			response.PutLong(63461647475710);
			response.PutLong(63461647484213);
			response.PutInt(0);
			response.PutByte(1);
			response.PutByte(34);
			response.PutInt(0x800200FF);
			response.PutByte(1);
			response.PutByte(0);
			response.PutLong(0); //
			response.PutByte(0);
			response.PutLong(0);
			response.PutByte(0);
			response.PutLong(0);
			response.PutByte(0);
			response.PutByte(1);
			response.PutByte(0); // Inventory Plus Kit
			response.PutLong(63362367600000);
			response.PutByte(0); // Mabinogi Premium Pack
			response.PutLong(63362367600000);
			response.PutByte(0); // Mabinogi VIP
			response.PutLong(0); // till next week = (ulong)(DateTime.Now.AddDays(7).Ticks/10000)
			response.PutByte(0);
			response.PutByte(0);
			response.PutByte(0);

			// Characters
			// --------------------------------------------------------------
			response.PutShort((ushort)account.Characters.Count);
			foreach (var character in account.Characters)
			{
				response.PutString(character.Server);
				response.PutLong(character.Id);
				response.PutString(character.Name);
				response.PutByte(character.GetDeletionFlag());
				response.PutLong(0); // ??
				response.PutInt(0);
				response.PutByte(0); // 0 = Human. 1 = Elf. 2 = Giant.
				response.PutByte(0);
				response.PutByte(0);
			}

			// Pets
			// --------------------------------------------------------------
			response.PutShort((ushort)account.Pets.Count);
			foreach (var pet in account.Pets)
			{
				response.PutString(pet.Server);
				response.PutLong(pet.Id);
				response.PutString(pet.Name);
				response.PutByte(pet.GetDeletionFlag());
				response.PutLong(0);
				response.PutInt(pet.Race);
				response.PutLong(0);
				response.PutLong(0);
				response.PutInt(0);
				response.PutByte(0);
			}

			// Character cards
			// --------------------------------------------------------------
			response.PutShort((ushort)account.CharacterCards.Count);
			foreach (MabiCard card in account.CharacterCards)
			{
				response.PutByte(0x01);
				response.PutLong(card.Id);
				response.PutInt(card.Race);
				response.PutLong(0);
				response.PutLong(0);
				response.PutInt(0);
			}

			// Pet cards
			// --------------------------------------------------------------
			response.PutShort((ushort)account.PetCards.Count);
			foreach (MabiCard card in account.PetCards)
			{
				response.PutByte(0x01);
				response.PutLong(card.Id);
				response.PutInt(102);
				response.PutInt(card.Race);
				response.PutLong(0);
				response.PutLong(0);
				response.PutInt(0);
			}

			// Gifts
			// --------------------------------------------------------------
			response.PutShort(0);

			response.PutByte(0);

			client.Account = account;
			client.State = SessionState.LoggedIn;

			client.Send(response);
		}
#pragma warning restore 0162

		private enum LoginResult { Fail = 0, Success = 1, Empty = 2, IdOrPassIncorrect = 3, /* IdOrPassIncorrect = 4, */ TooManyConnections = 6, AlreadyLoggedIn = 7, UnderAge = 33, Banned = 101 }

		private void HandleCharacterInfoRequest(LoginClient client, MabiPacket packet)
		{
			var serverName = packet.GetString();
			var id = packet.GetLong();

			var response = new MabiPacket(Op.CharInfo, 0x1000000000000010);

			var player = client.Account.Characters.FirstOrDefault(a => a.Id == id);
			if (player == null)
			{
				// Fail
				response.PutByte(0);
				client.Send(response);
				return;
			}

			// Info
			response.PutByte(1);
			response.PutString(player.Server);
			response.PutLong(player.Id);
			response.PutByte(0x01);
			response.PutString(player.Name);
			response.PutString("");
			response.PutString("");
			response.PutInt(player.Race);
			response.PutByte(player.SkinColor);
			response.PutByte(player.Eye);
			response.PutByte(player.EyeColor);
			response.PutByte(player.Lip);
			response.PutInt(0);
			response.PutFloat(player.Height);
			response.PutFloat(player.Fat);
			response.PutFloat(player.Upper);
			response.PutFloat(player.Lower);
			response.PutInt(0);
			response.PutInt(0);
			response.PutInt(0);
			response.PutByte(0);
			response.PutInt(0);
			response.PutByte(0);
			response.PutInt(player.ColorA);
			response.PutInt(player.ColorB);
			response.PutInt(player.ColorC);
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

			response.PutByte(3);

			response.PutInt((uint)player.Items.Count);
			foreach (var item in player.Items)
			{
				response.PutLong(item.Id);
				response.PutBin(item.Info);
			}

			response.PutInt(0);
			response.PutLong(0);
			response.PutLong(0);

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
			var lip = packet.GetByte();
			var face = packet.GetInt();

			var response = new MabiPacket(Op.CharacterCreated, 0x1000000000000010);

			// Check if account has this card
			var card = client.Account.CharacterCards.FirstOrDefault(a => a.Id == cardId);
			CharCardInfo cardInfo = null;
			if (card != null)
			{
				// Check if this is a valid card and if this race can use it
				cardInfo = MabiData.CharCardDb.Find(card.Race);
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

			// Remove card
			if (LoginConf.ConsumeCards)
			{
				client.Account.CharacterCards.Remove(card);
				MabiDb.Instance.SaveCards(client.Account);
			}

			// Create character
			var newChar = new MabiCharacter();
			newChar.Name = charName;
			newChar.Race = charRace;
			newChar.SkinColor = skinColor;
			newChar.Eye = eye;
			newChar.EyeColor = eyeColor;
			newChar.Lip = lip;
			newChar.Age = age;
			newChar.Server = serverName;
			newChar.Region = 1;
			newChar.SetPosition(12800, 38100);
			newChar.Level = 1;

			newChar.CalculateBaseStats();

			// Retrieve start items, add head, and add items to inventory
			//var cardItems = cardInfo.CardItems.FindAll(a => a.Race == charRace);
			var cardItems = MabiData.CharCardSetDb.Find(cardInfo.SetId, charRace);

			cardItems.Add(new CharCardSetInfo { ItemId = face, Pocket = 3, Race = charRace, Color1 = skinColor });
			cardItems.Add(new CharCardSetInfo { ItemId = hair, Pocket = 4, Race = charRace, Color1 = (uint)hairColor + 0x10000000 });

			foreach (var item in cardItems)
			{
				newChar.Items.Add(new MabiItem(item));
			}

			// Add beginner keywords
			newChar.Keywords.Add(1);
			newChar.Keywords.Add(2);
			newChar.Keywords.Add(3);
			newChar.Keywords.Add(37);
			newChar.Keywords.Add(38);

			// Skills
			newChar.Skills.Add(new MabiSkill(SkillConst.MeleeCombatMastery, SkillRank.RF, newChar.Race));

			// Item skills?
			newChar.Skills.Add(new MabiSkill(SkillConst.HiddenEnchant));
			newChar.Skills.Add(new MabiSkill(SkillConst.HiddenResurrection));
			newChar.Skills.Add(new MabiSkill(SkillConst.HiddenTownBack));
			newChar.Skills.Add(new MabiSkill(SkillConst.HiddenGuildstoneSetting));
			newChar.Skills.Add(new MabiSkill(SkillConst.HiddenBlessing));
			newChar.Skills.Add(new MabiSkill(SkillConst.CampfireKit));
			newChar.Skills.Add(new MabiSkill(SkillConst.SkillUntrainKit));
			newChar.Skills.Add(new MabiSkill(SkillConst.BigBlessingWaterKit));
			newChar.Skills.Add(new MabiSkill(SkillConst.Dye));
			newChar.Skills.Add(new MabiSkill(SkillConst.EnchantElementalAllSlot));
			newChar.Skills.Add(new MabiSkill(SkillConst.HiddenPoison));
			newChar.Skills.Add(new MabiSkill(SkillConst.HiddenBomb));
			newChar.Skills.Add(new MabiSkill(SkillConst.FossilRestoration));
			newChar.Skills.Add(new MabiSkill(SkillConst.SeesawJump));
			newChar.Skills.Add(new MabiSkill(SkillConst.SeesawCreate));
			newChar.Skills.Add(new MabiSkill(SkillConst.DragonSupport));
			newChar.Skills.Add(new MabiSkill(SkillConst.IceMineKit));
			newChar.Skills.Add(new MabiSkill(SkillConst.Scan));
			newChar.Skills.Add(new MabiSkill(SkillConst.UseSupportItem));
			newChar.Skills.Add(new MabiSkill(SkillConst.UseAntiMacroItem));
			newChar.Skills.Add(new MabiSkill(SkillConst.ItemSeal));
			newChar.Skills.Add(new MabiSkill(SkillConst.ItemUnseal));
			newChar.Skills.Add(new MabiSkill(SkillConst.ItemDungeonPass));
			newChar.Skills.Add(new MabiSkill(SkillConst.UseElathaItem));
			newChar.Skills.Add(new MabiSkill(SkillConst.UseMorrighansFeather));
			newChar.Skills.Add(new MabiSkill(SkillConst.PetBuffing));
			newChar.Skills.Add(new MabiSkill(SkillConst.CherryTreeKit));
			newChar.Skills.Add(new MabiSkill(SkillConst.Pollen));
			newChar.Skills.Add(new MabiSkill(SkillConst.Firecracker));
			newChar.Skills.Add(new MabiSkill(SkillConst.FeedFish));
			newChar.Skills.Add(new MabiSkill(SkillConst.HammerGame));
			newChar.Skills.Add(new MabiSkill(SkillConst.SoulStone));
			newChar.Skills.Add(new MabiSkill(SkillConst.UseItemBomb2));
			newChar.Skills.Add(new MabiSkill(SkillConst.NameColorChange));
			newChar.Skills.Add(new MabiSkill(SkillConst.HolyFire));
			newChar.Skills.Add(new MabiSkill(SkillConst.MakeFaliasPortal));
			newChar.Skills.Add(new MabiSkill(SkillConst.UseItemChattingColorChange));
			newChar.Skills.Add(new MabiSkill(SkillConst.InstallFacility));
			newChar.Skills.Add(new MabiSkill(SkillConst.RedesignFacility));
			newChar.Skills.Add(new MabiSkill(SkillConst.GachaponSynthesis));
			newChar.Skills.Add(new MabiSkill(SkillConst.MakeChocoStatue));
			newChar.Skills.Add(new MabiSkill(SkillConst.Painting));
			newChar.Skills.Add(new MabiSkill(SkillConst.PaintMixing));
			newChar.Skills.Add(new MabiSkill(SkillConst.PetSealToItem));
			newChar.Skills.Add(new MabiSkill(SkillConst.FlownHotAirBalloon));
			newChar.Skills.Add(new MabiSkill(SkillConst.ItemSeal2));
			newChar.Skills.Add(new MabiSkill(SkillConst.CureZombie));
			newChar.Skills.Add(new MabiSkill(SkillConst.WarpContinent));
			newChar.Skills.Add(new MabiSkill(SkillConst.AddSeasoning));

			MabiDb.Instance.SaveCharacter(client.Account, newChar);

			client.Account.Characters.Add(newChar);

			// Success
			response.PutByte(1);
			response.PutString(serverName);
			response.PutLong(newChar.Id);

			client.Send(response);
		}

		private void HandleEnterGame(LoginClient client, MabiPacket packet)
		{
			var serverName = packet.GetString();
			var channelName = packet.GetString();
			var unk1 = packet.GetByte();
			var unk2 = packet.GetInt();
			var characterId = packet.GetLong();

			var servers = MabiDb.Instance.GetServerList();
			MabiServers server = null;
			MabiChannel channel = null;

			if (servers.Count > 0)
			{
				server = servers.FirstOrDefault(a => a.Name == serverName);
				if (server != null)
				{
					channel = server.Channels.FirstOrDefault(a => a.Name == channelName);
				}
			}

			var response = new MabiPacket(Op.ChannelInfo, 0x1000000000000001);

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
			var charId = packet.GetLong();
			//var charName = packet.GetString();

			MabiPC character = null;
			if (packet.Op < Op.DeletePetRequest)
				character = client.Account.Characters.FirstOrDefault(a => a.Id == charId);
			else
				character = client.Account.Pets.FirstOrDefault(a => a.Id == charId);

			// The response op is always +1.
			uint op = packet.Op + 1;

			var response = new MabiPacket(op, 0x1000000000000010);

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
				response.PutLong(charId);
				response.PutLong(0);

				if (packet.Op == Op.RecoverChar || packet.Op == Op.RecoverPet)
				{
					// Reset time
					character.DeletionTime = DateTime.MinValue;
				}
				else if (packet.Op == Op.DeleteChar || packet.Op == Op.DeletePet)
				{
					// Mark for deletion
					character.DeletionTime = DateTime.MaxValue;
				}
				else // Op.DeleteCharRequest || Op.DeletePetRequest || Error?
				{
					// Set time at which the character can be deleted for good.
					// TODO: This should be configurable.
					character.DeletionTime = (DateTime.Now.AddDays(1).Date + new TimeSpan(7, 0, 0));
				}
			}

			client.Send(response);
		}

		private void HandleCheckName(LoginClient client, MabiPacket packet)
		{
			var server = packet.GetString();
			var name = packet.GetString();

			MabiPacket response = new MabiPacket(Op.NameCheckR, 0x1000000000000010);

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

		private void HandlePetInfoRequest(LoginClient client, MabiPacket packet)
		{
			var serverName = packet.GetString();
			var id = packet.GetLong();

			var response = new MabiPacket(Op.PetInfo, 0x1000000000000010);

			var pet = client.Account.Pets.FirstOrDefault(a => a.Id == id);
			if (pet == null)
			{
				// Fail
				response.PutByte(1);
				client.Send(response);
				return;
			}

			// Success
			response.PutByte(1);
			response.PutString(pet.Server);
			response.PutLong(pet.Id);
			response.PutByte(1);
			response.PutString(pet.Name);
			response.PutString("");
			response.PutString("");
			response.PutInt(pet.Race);
			response.PutByte(pet.SkinColor);
			response.PutByte(pet.Eye);
			response.PutByte(pet.EyeColor);
			response.PutByte(pet.Lip);
			response.PutInt(0);
			response.PutFloat(pet.Height);
			response.PutFloat(pet.Fat);
			response.PutFloat(pet.Upper);
			response.PutFloat(pet.Lower);
			response.PutInt(0);
			response.PutInt(0);
			response.PutInt(0);
			response.PutByte(0);
			response.PutInt(0);
			response.PutByte(0);
			response.PutInt(pet.ColorA);
			response.PutInt(pet.ColorB);
			response.PutInt(pet.ColorC);
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

			response.PutByte(3);

			response.PutInt((uint)pet.Items.Count);
			foreach (var item in pet.Items)
			{
				response.PutLong(item.Id);
				response.PutBin(item.Info);
			}

			response.PutInt(0);
			response.PutLong(0);
			response.PutLong(0);

			client.Send(response);
		}

		private void HandleCreatePet(LoginClient client, MabiPacket packet)
		{
			var serverName = packet.GetString();
			var cardId = packet.GetLong();
			var name = packet.GetString();

			MabiPacket response = new MabiPacket(Op.PetCreated, 0x1000000000000010);

			// Check if the card is valid
			MabiCard card = null;
			if (client.Account.PetCards.Exists(a => a.Id == cardId))
			{
				card = client.Account.PetCards.First(a => a.Id == cardId);
			}

			if (card == null || !MabiDb.Instance.NameOkay(name, serverName))
			{
				// Fail
				response.PutByte(0);
				client.Send(response);
				return;
			}

			// Remove card
			if (LoginConf.ConsumeCards)
			{
				client.Account.PetCards.Remove(card);
				MabiDb.Instance.SaveCards(client.Account);
			}

			// Create new pet
			var newChar = new MabiPet();
			newChar.Name = name;
			newChar.Race = card.Race;
			newChar.Age = 1;
			newChar.Server = serverName;
			newChar.Region = 1;
			newChar.SetPosition(12800, 38100);

			newChar.CalculateBaseStats();

			// Skills
			newChar.Skills.Add(new MabiSkill(SkillConst.MeleeCombatMastery, SkillRank.RF, newChar.Race));

			MabiDb.Instance.SaveCharacter(client.Account, newChar);

			client.Account.Pets.Add(newChar);

			// Success
			response.PutByte(1);
			response.PutString(serverName);
			response.PutLong(newChar.Id);

			client.Send(response);
		}

		private void HandleCreatePartner(LoginClient client, MabiPacket packet)
		{
			var serverName = packet.GetString();
			var cardId = packet.GetLong();
			var name = packet.GetString();
			packet.GetInt();    // 730204
			var skinColor = packet.GetByte();
			var hair = packet.GetInt();
			var hairColor = packet.GetByte();
			var eye = packet.GetByte();
			var eyeColor = packet.GetByte();
			var lip = packet.GetByte();
			var face = packet.GetInt();
			var height = packet.GetFloat();
			var weight = packet.GetFloat();
			var upper = packet.GetFloat();
			var lower = packet.GetFloat();
			var personality = packet.GetInt();

			var response = new MabiPacket(Op.PetCreated, 0x1000000000000010);

			// Check if the card is valid
			var card = client.Account.PetCards.FirstOrDefault(a => a.Id == cardId);
			if (card == null || !MabiDb.Instance.NameOkay(name, serverName))
			{
				// Fail
				response.PutByte(0);
				client.Send(response);
				return;
			}

			// Remove card
			if (LoginConf.ConsumeCards)
			{
				client.Account.PetCards.Remove(card);
				MabiDb.Instance.SaveCards(client.Account);
			}

			// Create new pet
			var newChar = new MabiPet();
			newChar.Name = name;
			newChar.Race = card.Race;
			newChar.SkinColor = skinColor;
			newChar.Eye = eye;
			newChar.EyeColor = eyeColor;
			newChar.Lip = lip;
			newChar.Server = serverName;
			newChar.Region = 1;
			newChar.SetPosition(12800, 38100);
			newChar.Level = 1;

			newChar.CalculateBaseStats();
			newChar.Height = height;

			// Items
			var faceItem = new MabiItem(face); faceItem.Info.Pocket = (byte)Pocket.Face; faceItem.Info.ColorA = skinColor;
			var hairItem = new MabiItem(hair); hairItem.Info.Pocket = (byte)Pocket.Hair; hairItem.Info.ColorA = (uint)hairColor + 0x10000000;
			newChar.Items.Add(faceItem);
			newChar.Items.Add(hairItem);

			uint setId = 0;
			if(card.Race == 730201 ||card.Race == 730202 ||card.Race == 730204 ||card.Race == 730205)
				setId = 1000;
			else if(card.Race == 730203)
				setId = 1001;
			else if(card.Race == 730206)
				setId = 1002;

			if (setId > 0)
			{
				var setItems = MabiData.CharCardSetDb.Find(setId, card.Race);
				foreach (var setItem in setItems)
				{
					newChar.Items.Add(new MabiItem(setItem));
				}
			}

			// Skills
			newChar.Skills.Add(new MabiSkill(SkillConst.MeleeCombatMastery, SkillRank.RF, newChar.Race));

			MabiDb.Instance.SaveCharacter(client.Account, newChar);

			client.Account.Pets.Add(newChar);

			// Success
			response.PutByte(1);
			response.PutString(serverName);
			response.PutLong(newChar.Id);

			client.Send(response);
		}

		private void HandleDisconnect(LoginClient client, MabiPacket packet)
		{
			var accountName = packet.GetString();

			if (accountName != client.Account.Username)
				return;

			Logger.Info("'" + accountName + "' is closing the connection.");

			//MabiDb.Instance.SaveAccount(client.Account);
		}

		private void HandleEnterPetCreation(LoginClient client, MabiPacket packet)
		{
			//Logger.Error("OMG! Someone is trying to create a pet/partner!!");
		}
	}
}
