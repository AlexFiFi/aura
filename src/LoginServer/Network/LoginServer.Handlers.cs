// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Linq;
using Common.Constants;
using Common.Database;
using Common.Network;
using Common.Tools;
using Common.World;
using MabiNatives;
using Login.Tools;

namespace Login.Network
{
	public partial class LoginServer : Server<LoginClient>
	{
		protected override void InitPacketHandlers()
		{
			this.RegisterPacketHandler(0x0FD1020A, HandleVersionCheck);
			this.RegisterPacketHandler(0x0FD12002, HandleLogin);
			this.RegisterPacketHandler(0x00000029, HandleCharacterInfoRequest);
			this.RegisterPacketHandler(0x0000002B, HandleCreateCharacter);
			this.RegisterPacketHandler(0x0000002F, HandleEnterGame);
			this.RegisterPacketHandler(0x0000002D, HandleDeletePC);
			this.RegisterPacketHandler(0x00000035, HandleDeletePC);
			this.RegisterPacketHandler(0x00000037, HandleDeletePC);
			this.RegisterPacketHandler(0x00000039, HandleCheckName);
			this.RegisterPacketHandler(0x0000003B, HandlePetInfoRequest);
			this.RegisterPacketHandler(0x0000003D, HandleCreatePet);
			this.RegisterPacketHandler(0x0000003F, HandleDeletePC);
			this.RegisterPacketHandler(0x00000041, HandleDeletePC);
			this.RegisterPacketHandler(0x00000043, HandleDeletePC);
			this.RegisterPacketHandler(0x0000004D, HandleDisconnect);
		}

		private void HandleVersionCheck(LoginClient client, MabiPacket packet)
		{
			var response = new MabiPacket(0x1F, 0x1000000000000010);

			response.PutByte(1);
			response.PutLong(0x49534E4F47493A5D); // time?

			client.Send(response);
		}

		private void HandleLogin(LoginClient client, MabiPacket packet)
		{
			var loginInfoType = packet.GetByte(); // 5/12 = login?, 2 = log off?, 20 = second password?
			var username = packet.GetString();

			// Tmp fix, when logging off account name is sent twice.
			if (loginInfoType == 0x02)
				packet.GetString();

			string password = "";
			ulong sessionKey = 0;

			// If this is an ID, we have assigned the client an ID to use, we'll check that
			// instead of the password
			if (packet.GetElementType() == MabiPacket.ElementType.Long)
			{
				sessionKey = packet.GetLong();
			}
			// Its just a password if its not an ID
			else
			{
				var userpassbytes = packet.GetBin();
				password = System.Text.Encoding.UTF8.GetString(userpassbytes);
			}

			var loginResult = LoginResult.Fail;

			MabiAccount account = null;

			if (LoginConf.NewAccounts && username.StartsWith("new//"))
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
					else if (account.Banned != 0 && account.BannedExpiration.CompareTo(DateTime.Now) > 0)
					{
						Logger.Info("Banned! (" + account.Username + ")");
						loginResult = LoginResult.Fail;
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

			var response = new MabiPacket(0x23, 0x1000000000000010);

			response.PutByte((byte)loginResult);
			if (loginResult != LoginResult.Success)
			{
				client.Send(response);
				return;
			}

			// Account
			// --------------------------------------------------------------
			response.PutString(account.Username);
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

		private enum LoginResult { Fail = 0, Success = 1, Empty = 2, IdOrPassIncorrect = 3, /* IdOrPassIncorrect = 4, */ TooManyConnections = 6, AlreadyLoggedIn = 7, UnderAge = 33, }

		private void HandleCharacterInfoRequest(LoginClient client, MabiPacket packet)
		{
			var serverName = packet.GetString();
			var id = packet.GetLong();

			var response = new MabiPacket(0x2A, 0x1000000000000010);

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

			var response = new MabiPacket(0x2C, 0x1000000000000010);

			// Check if account has this card
			var card = client.Account.CharacterCards.FirstOrDefault(a => a.Id == cardId);
			PCCardInfo cardInfo = null;
			if (card != null)
			{
				// Check if this is a valid card and if this race can use it
				cardInfo = MabiData.PCCardDb.Find(card.Race);
				if (cardInfo == null || !cardInfo.Races.Contains(charRace))
				{
					// Reset card, so the request fails
					card = null;
				}
			}

			var faceItem = MabiData.ItemDb.Find(face);
			var hairItem = MabiData.ItemDb.Find(hair);

			if (card == null || !MabiDb.Instance.NameOkay(charName, serverName) || faceItem == null || faceItem.Type != (ushort)ItemType.Face || hairItem == null || hairItem.Type != (ushort)ItemType.Hair)
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
			var cardItems = MabiData.PCCardDb.FindSet(cardInfo.Id, charRace);

			cardItems.Add(new PCCardItem(face, 3, charRace, skinColor));
			cardItems.Add(new PCCardItem(hair, 4, charRace, (uint)hairColor + 0x10000000));

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

			var response = new MabiPacket(0x30, 0x1000000000000001);

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
			if (packet.Op < 0x3F)
				character = client.Account.Characters.FirstOrDefault(a => a.Id == charId);
			else
				character = client.Account.Pets.FirstOrDefault(a => a.Id == charId);

			// The response op is always +1.
			uint op = packet.Op + 1;

			var response = new MabiPacket(op, 0x1000000000000010);

			if (character == null || ((op == 0x36 || op == 0x42) && character.DeletionTime > DateTime.Now))
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

				switch (packet.Op)
				{
					// Set time at which the character can be deleted for good.
					default:
					case 0x2D:
					case 0x3F:
						// TODO: This should be configurable.
						character.DeletionTime = (DateTime.Now.AddDays(1).Date + new TimeSpan(7, 0, 0));
						break;

					// Reset time
					case 0x37:
					case 0x43:
						character.DeletionTime = DateTime.MinValue;
						break;

					// Mark for deletion
					case 0x35:
					case 0x41:
						character.DeletionTime = DateTime.MaxValue;
						break;
				}
			}

			client.Send(response);
		}

		private void HandleCheckName(LoginClient client, MabiPacket packet)
		{
			var server = packet.GetString();
			var name = packet.GetString();

			MabiPacket response = new MabiPacket(0x3A, 0x1000000000000010);

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

			var response = new MabiPacket(0x3C, 0x1000000000000010);

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
			response.PutByte(0);
			response.PutByte(0);
			response.PutByte(0);
			response.PutByte(0);
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

			// TODO: Can pets even have visible items?
			response.PutInt((uint)pet.Items.Count);
			foreach (MabiItem item in pet.Items)
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

			MabiPacket response = new MabiPacket(0x3E, 0x1000000000000010);

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

			Logger.Info("'" + accountName + "' is closing the connection. Saving...");

			//MabiDb.Instance.SaveAccount(client.Account);
		}
	}
}
