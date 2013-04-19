// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Aura.Shared.Const;
using Aura.Shared.Database;
using Aura.Shared.Network;
using Aura.World.Player;
using Aura.World.World;
using MySql.Data.MySqlClient;
using Aura.Shared.Util;

namespace Aura.World.Database
{
	public class WorldDb
	{
		public static readonly WorldDb Instance = new WorldDb();
		static WorldDb() { }
		private WorldDb() { }

		public bool AccountHasCharacter(string accountName, string charName)
		{
			var conn = MabiDb.Instance.GetConnection();
			try
			{
				var mc = new MySqlCommand("SELECT characterId FROM characters WHERE accountId = @acc AND name = @name", conn);
				mc.Parameters.AddWithValue("@acc", accountName);
				mc.Parameters.AddWithValue("@name", charName);

				var reader = mc.ExecuteReader();
				bool result = reader.Read();
				reader.Close();

				return result;
			}
			finally
			{
				conn.Close();
			}
		}

		/// <summary>
		/// Sets the  authority for the given account.
		/// </summary>
		/// <param name="accName"></param>
		/// <param name="level"></param>
		/// <returns></returns>
		public bool SetAuthority(string accName, byte level)
		{
			var conn = MabiDb.Instance.GetConnection();
			try
			{
				var mc = new MySqlCommand("UPDATE accounts SET authority = @auth WHERE accountId = @id", conn);
				mc.Parameters.AddWithValue("@id", accName);
				mc.Parameters.AddWithValue("@auth", level);
				var found = mc.ExecuteNonQuery();

				return found > 0;
			}
			finally
			{
				conn.Close();
			}
		}

		/// <summary>
		/// Checks if the name is okay, and if a character (or pet) with the given name exists.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public bool NameOkay(string name, string server)
		{
			if (!(new Regex(@"^[a-zA-Z0-9]{3,15}$")).IsMatch(name))
				return false;

			var conn = MabiDb.Instance.GetConnection();
			try
			{
				name = MySqlHelper.EscapeString(name);

				using (var reader = MabiDb.Instance.Query("SELECT characterId FROM characters WHERE name = '" + name + "' AND server = '" + server + "'", conn))
				{
					return !reader.HasRows;
				}
			}
			finally
			{
				conn.Close();
			}
		}

		/// <summary>
		/// Reads a full account, incl. characters, cards, etc. from the database and returns it.
		/// </summary>
		/// <param name="accName"></param>
		/// <returns></returns>
		public Account GetAccount(string accName)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				accName = MySqlHelper.EscapeString(accName);
				var account = new Account();

				using (var reader = MabiDb.Instance.Query("SELECT * FROM accounts WHERE accountId = '" + accName + "'", conn))
				{
					if (!reader.Read())
					{
						return null;
					}

					account.Name = accName;
					account.Password = reader.GetString("password");
					account.Authority = reader.GetByte("authority");
					account.LastIp = reader.GetString("lastip");
					account.LastLogin = reader["lastlogin"] as DateTime? ?? DateTime.MinValue;
					account.BannedReason = reader.GetString("bannedreason");
					account.BannedExpiration = reader["bannedexpiration"] as DateTime? ?? DateTime.MinValue;
				}

				this.GetCharacters(account);

				return account;
			}
		}

		/// <summary>
		/// Reads all characters in the given account from the database and adds them to it.
		/// </summary>
		/// <param name="account"></param>
		private void GetCharacters(Account account)
		{
			var conn = MabiDb.Instance.GetConnection();
			try
			{
				account.Characters = new List<MabiCharacter>();
				account.Pets = new List<MabiPet>();

				using (var reader = MabiDb.Instance.Query("SELECT characterId FROM characters WHERE accountId = '" + account.Name + "' AND type = 'CHARACTER'", conn))
				{
					while (reader.Read())
					{
						var character = this.GetCharacter<MabiCharacter>((ulong)reader.GetInt64("characterId"));

						// Add GM titles for the characters of authority 50+ accounts
						if (account.Authority >= Authority.GameMaster) character.Titles.Add(60000, true); // GM
						if (account.Authority >= Authority.Admin) character.Titles.Add(60001, true); // devCat

						account.Characters.Add(character);
					}
					reader.Close();
				}

				using (var reader = MabiDb.Instance.Query("SELECT characterId FROM characters WHERE accountId = '" + account.Name + "' AND type = 'PET'", conn))
				{
					while (reader.Read())
					{
						var p = this.GetCharacter<MabiPet>((ulong)reader.GetInt64("characterId"));
						account.Pets.Add(p);
					}
					reader.Close();
				}
			}
			finally
			{
				conn.Close();
			}
		}

		/// <summary>
		/// Reads the character with the given Id from the database and returns it.
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Returns null if character doesn't exist.</returns>
		private T GetCharacter<T>(ulong id) where T : MabiPC, new()
		{
			var conn = MabiDb.Instance.GetConnection();
			try
			{
				var character = new T();

				using (var reader = MabiDb.Instance.Query("SELECT * FROM characters WHERE characterId = " + id.ToString(), conn))
				{
					if (!reader.Read())
					{
						reader.Close();
						return null;
					}

					character.Id = reader.GetUInt64("characterId");
					character.Name = reader.GetString("name");
					character.Server = reader.GetString("server");
					character.Race = reader.GetUInt32("race");
					character.SkinColor = reader.GetByte("skinColor");
					character.EyeColor = reader.GetByte("eyeColor");
					character.Eye = reader.GetByte("eyeType");
					character.Lip = reader.GetByte("mouthType");
					character.Height = reader.GetFloat("height");
					character.Fat = reader.GetFloat("fatness");
					character.Upper = reader.GetFloat("upper");
					character.Lower = reader.GetFloat("lower");
					character.SetLocation(reader.GetUInt32("region"), reader.GetUInt32("x"), reader.GetUInt32("y"));
					character.Direction = reader.GetByte("direction");
					character.BattleState = reader.GetByte("battleState");
					character.WeaponSet = reader.GetByte("weaponSet");
					character.Injuries = reader.GetFloat("injuries");
					character.LifeMaxBase = reader.GetFloat("lifeMax");
					character.Life = reader.GetFloat("life");
					character.ManaMaxBase = reader.GetFloat("manaMax");
					character.Mana = reader.GetFloat("mana");
					character.StaminaMaxBase = reader.GetFloat("staminaMax");
					character.Hunger = reader.GetFloat("food");
					character.Stamina = reader.GetFloat("stamina");
					character.Level = reader.GetUInt16("level");
					character.LevelTotal = reader.GetUInt32("totalLevel");
					character.Experience = reader.GetUInt64("experience");
					character.Age = reader.GetByte("age");
					character.StrBase = reader.GetFloat("strength");
					character.DexBase = reader.GetFloat("dexterity");
					character.IntBase = reader.GetFloat("intelligence");
					character.WillBase = reader.GetFloat("will");
					character.LuckBase = reader.GetFloat("luck");
					character.AbilityPoints = reader.GetUInt16("abilityPoints");
					//character.StrMod = reader.GetFloat("strBoost");
					//character.DexMod = reader.GetFloat("dexBoost");
					//character.IntMod = reader.GetFloat("intBoost");
					//character.WillMod = reader.GetFloat("willBoost");
					//character.LuckMod = reader.GetFloat("luckBoost");
					character.Title = reader.GetUInt16("title");
					character.Color1 = reader.GetUInt32("color1");
					character.Color2 = reader.GetUInt32("color2");
					character.Color3 = reader.GetUInt32("color3");
					character.State = (CreatureStates)reader.GetUInt32("status") & ~CreatureStates.SitDown;

					character.LoadDefault();
				}

				this.GetQuests(character);
				this.GetItems(character);
				this.GetKeywords(character);
				this.GetSkills(character);

				character.Shamalas.Add(new ShamalaTransformation(1, 1, ShamalaState.Available));
				character.Shamalas.Add(new ShamalaTransformation(2, 1, ShamalaState.Available));
				character.Shamalas.Add(new ShamalaTransformation(3, 1, ShamalaState.Available));

				return character;
			}
			finally
			{
				conn.Close();
			}
		}

		private void GetQuests(MabiPC character)
		{
			var conn = MabiDb.Instance.GetConnection();
			try
			{
				var mc = new MySqlCommand("SELECT * FROM quests WHERE characterId = @characterId", conn);
				mc.Parameters.AddWithValue("@characterId", character.Id);

				using (var reader = mc.ExecuteReader())
				{
					while (reader.Read())
					{
						var id = reader.GetUInt64("questId");
						var cls = reader.GetUInt32("questClass");

						var quest = new MabiQuest(cls, id);
						quest.State = (MabiQuestState)reader.GetByte("state");

						character.Quests.Add(cls, quest);
					}
				}

				mc = new MySqlCommand("SELECT * FROM quest_progress WHERE characterId = @characterId ORDER BY progressId", conn);
				mc.Parameters.AddWithValue("@characterId", character.Id);

				using (var reader = mc.ExecuteReader())
				{
					while (reader.Read())
					{
						var id = reader.GetUInt64("questId");
						var cls = reader.GetUInt32("questClass");

						var quest = character.GetQuestOrNull(cls);
						if (quest == null)
							continue;

						var objective = reader.GetString("objective");
						var count = reader.GetUInt32("count");
						var done = reader.GetBoolean("done");
						var unlocked = reader.GetBoolean("unlocked");

						quest.Progresses.Add(objective, new MabiQuestProgress(objective, count, done, unlocked));
					}
				}
			}
			finally
			{
				conn.Close();
			}
		}

		private void GetKeywords(MabiPC character)
		{
			var conn = MabiDb.Instance.GetConnection();
			try
			{
				using (var reader = MabiDb.Instance.Query("SELECT keywordId FROM keywords WHERE characterId = " + character.Id.ToString(), conn))
				{
					while (reader.Read())
					{
						character.Keywords.Add(reader.GetUInt16("keywordId"));
					}
				}
			}
			finally
			{
				conn.Close();
			}
		}

		private void GetSkills(MabiPC character)
		{
			var conn = MabiDb.Instance.GetConnection();
			try
			{
				using (var reader = MabiDb.Instance.Query("SELECT skillId, rank, exp FROM skills WHERE characterId = " + character.Id.ToString(), conn))
				{
					while (reader.Read())
					{
						var skill = new MabiSkill(reader.GetUInt16("skillId"), reader.GetByte("rank"), character.Race);
						skill.Info.Experience = reader.GetInt32("exp");
						if (skill.IsRankable)
							skill.Info.Flag |= (ushort)SkillFlags.Rankable;
						character.AddSkill(skill);
					}
				}
			}
			finally
			{
				conn.Close();

				if (!character.HasSkill(SkillConst.MeleeCombatMastery))
					character.AddSkill(new MabiSkill(SkillConst.MeleeCombatMastery, SkillRank.RF, character.Race));
			}
		}

		/// <summary>
		/// Reads all items for the given character from the database, and adds them to it.
		/// </summary>
		/// <param name="character"></param>
		private void GetItems(MabiPC character)
		{
			var conn = MabiDb.Instance.GetConnection();
			try
			{
				using (var reader = MabiDb.Instance.Query("SELECT * FROM items WHERE characterId = " + character.Id.ToString() + " ORDER BY pocketId, pos_y, pos_x", conn))
				{
					while (reader.Read())
					{
						var item = GetItem(reader);

						character.Items.Add(item);
					}

					character.UpdateItemsFromPockets();

					reader.Close();
				}
			}
			finally
			{
				conn.Close();
			}
		}

		public MabiItem GetItem(ulong Id)
		{
			var conn = MabiDb.Instance.GetConnection();
			try
			{
				using (var reader = MabiDb.Instance.Query("SELECT * FROM items WHERE itemId = " + Id.ToString(), conn))
				{
					if (!reader.HasRows)
						return null;
					reader.Read();
					return GetItem(reader);
				}
			}
			finally
			{
				conn.Close();
			}
		}

		private MabiItem GetItem(MySqlDataReader reader)
		{
			var item = new MabiItem(reader.GetUInt32("class"), reader.GetUInt64("itemId"));
			item.Info.Pocket = reader.GetByte("pocketId");
			item.Info.X = reader.GetUInt32("pos_x");
			item.Info.Y = reader.GetUInt32("pos_y");
			item.Info.ColorA = reader.GetUInt32("color_01");
			item.Info.ColorB = reader.GetUInt32("color_02");
			item.Info.ColorC = reader.GetUInt32("color_03");
			item.OptionInfo.Price = reader.GetUInt32("price");
			item.Info.Amount = reader.GetUInt16("bundle");
			item.OptionInfo.LinkedPocketId = reader.GetUInt32("linked_pocket");
			item.Info.FigureA = (byte)reader.GetUInt32("figure");
			item.OptionInfo.Flag = reader.GetByte("flag");
			item.OptionInfo.Durability = reader.GetUInt32("durability");
			item.OptionInfo.DurabilityMax = reader.GetUInt32("durability_max");
			item.OptionInfo.DurabilityOriginal = reader.GetUInt32("origin_durability_max");
			item.OptionInfo.AttackMin = reader.GetUInt16("attack_min");
			item.OptionInfo.AttackMax = reader.GetUInt16("attack_max");
			item.OptionInfo.WAttackMin = reader.GetUInt16("wattack_min");
			item.OptionInfo.WAttackMax = reader.GetUInt16("wattack_max");
			item.OptionInfo.Balance = reader.GetByte("balance");
			item.OptionInfo.Critical = reader.GetByte("critical");
			item.OptionInfo.Defense = reader.GetUInt32("defence");
			item.OptionInfo.Protection = reader.GetInt16("protect");
			item.OptionInfo.EffectiveRange = reader.GetUInt16("effective_range");
			//item.ItemOptionInfo.AttackSpeed = reader.GetByte("attack_speed");
			//item.OptionInfo.DownHitCount = reader.GetByte("down_hit_count");
			item.OptionInfo.Experience = reader.GetUInt16("experience");
			//0 = reader.GetUInt32("exp_point"); // ?
			item.OptionInfo.Upgraded = reader.GetByte("upgraded");
			item.OptionInfo.UpgradeMax = reader.GetByte("upgraded");
			item.OptionInfo.Grade = reader.GetUInt32("grade");
			item.OptionInfo.Prefix = reader.GetUInt16("prefix");
			item.OptionInfo.Suffix = reader.GetUInt16("suffix");
			//mc.Parameters.AddWithValue("@data", "");
			//mc.Parameters.AddWithValue("@option", "");
			item.OptionInfo.SellingPrice = reader.GetUInt32("sellingprice");
			//DateTime.Now = reader.GetUInt32("update_time");
			item.Tags.Parse(reader.GetString("tags"));

			return item;
		}

		public List<MabiMail> GetSentMail(ulong senderId)
		{
			var conn = MabiDb.Instance.GetConnection();
			var messages = new List<MabiMail>();

			try
			{
				var reader = MabiDb.Instance.Query("SELECT * FROM mail WHERE senderId = " + senderId, conn);
				while (reader.Read())
					messages.Add(GetMail(reader));
			}
			finally
			{
				conn.Close();
			}

			return messages.Where(m => m.Type != (byte)MailTypes.Return).ToList();
		}

		public List<MabiMail> GetRecievedMail(ulong recipientId)
		{
			var conn = MabiDb.Instance.GetConnection();
			var messages = new List<MabiMail>();

			try
			{
				var reader = MabiDb.Instance.Query("SELECT * FROM mail WHERE recipientId = " + recipientId, conn);
				while (reader.Read())
					messages.Add(GetMail(reader));
			}
			finally
			{
				conn.Close();
			}

			return messages;
		}

		public MabiMail GetMail(ulong mailId)
		{
			var conn = MabiDb.Instance.GetConnection();
			try
			{
				var reader = MabiDb.Instance.Query("SELECT * FROM mail WHERE messageId = " + mailId, conn);
				if (!reader.Read())
					return null;
				return GetMail(reader);
			}
			finally
			{
				conn.Close();
			}
		}

		private MabiMail GetMail(MySqlDataReader reader)
		{
			MabiMail mail = new MabiMail();
			mail.MessageId = reader.GetUInt64("messageId");
			mail.SenderId = reader.GetUInt64("senderId");
			mail.SenderName = reader.GetString("senderName");
			mail.RecipientId = reader.GetUInt64("recipientId");
			mail.RecipientName = reader.GetString("recipientName");
			mail.Text = reader.GetString("text");
			mail.ItemId = reader.GetUInt64("itemId");
			mail.COD = reader.GetUInt32("cashOnDemand");
			mail.Sent = reader.GetDateTime("dateSent");
			mail.Read = reader.GetByte("read");
			mail.Type = reader.GetByte("type");

			return mail;
		}

		public void SaveMail(MabiMail mail)
		{
			var conn = MabiDb.Instance.GetConnection();
			try
			{
				if (mail.MessageId == 0)
				{
					var mc = new MySqlCommand(
						"INSERT INTO `mail`"
						+ " (`senderId`, `senderName`, `recipientId`, `recipientName`,"
						+ " `text`, `itemId`, `cashOnDemand`, `dateSent`, `read`, `type`) "
						+ " VALUES (@senderId, @senderName, @recipientId, @recipientName,"
						+ " @text, @itemId, @cashOnDemand, @dateSent, @read, @type) ", conn); //Only update read, because in all other instances, the mail is deleted

					mc.Parameters.AddWithValue("@messageId", mail.MessageId);
					mc.Parameters.AddWithValue("@senderId", mail.SenderId);
					mc.Parameters.AddWithValue("@senderName", mail.SenderName);
					mc.Parameters.AddWithValue("@recipientId", mail.RecipientId);
					mc.Parameters.AddWithValue("@recipientName", mail.RecipientName);
					mc.Parameters.AddWithValue("@text", mail.Text);
					mc.Parameters.AddWithValue("@itemId", mail.ItemId);
					mc.Parameters.AddWithValue("@cashOnDemand", mail.COD);
					mc.Parameters.AddWithValue("@dateSent", mail.Sent);
					mc.Parameters.AddWithValue("@read", mail.Read);
					mc.Parameters.AddWithValue("@type", mail.Type);

					mc.ExecuteNonQuery();

					var r = MabiDb.Instance.Query("SELECT LAST_INSERT_ID()", conn);
					try
					{
						r.Read();

						mail.MessageId = r.GetUInt64(0);
					}
					finally
					{
						r.Close();
					}
				}
				else
				{
					//Only update read because in all other instances, mail is deleted
					MabiDb.Instance.QueryN("UPDATE mail SET `read` = " + Convert.ToByte(mail.Read) + " WHERE `messageId` = " + mail.MessageId, conn);
				}
			}
			finally
			{
				conn.Close();
			}
		}

		public void DeleteMail(ulong mailId)
		{
			var conn = MabiDb.Instance.GetConnection();
			try
			{
				var mc = new MySqlCommand("DELETE FROM mail WHERE messageId = " + mailId, conn);
				mc.ExecuteNonQuery();
			}
			finally
			{
				conn.Close();
			}
		}

		public bool IsValidMailRecpient(string name, out ulong id) //TODO: Server
		{
			id = 0;
			if (!(new Regex(@"^[a-zA-Z0-9]{3,15}$")).IsMatch(name))
				return false;

			var conn = MabiDb.Instance.GetConnection();
			try
			{
				name = MySqlHelper.EscapeString(name);

				using (var reader = MabiDb.Instance.Query("SELECT characterId FROM characters WHERE name = '" + name + "'" /*AND server = '" + server + "'"*/, conn))
				{
					if (reader.HasRows)
					{
						reader.Read();
						id = reader.GetUInt64(0);
						return true;
					}
					else
						return false;
				}
			}
			finally
			{
				conn.Close();
			}
		}

		/// <summary>
		/// Writes a full account to the database, incl. characters, cards, etc.
		/// </summary>
		/// <param name="account"></param>
		public void SaveAccount(Account account)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				var mc = new MySqlCommand(
					"UPDATE `accounts` SET" +
					" `authority` = @authority, `lastlogin` = @lastlogin, `lastip` = @lastip," +
					" `bannedreason` = @bannedreason, `bannedexpiration` = @bannedexpiration" +
					" WHERE `accountId` = @accountId"
				, conn);

				mc.Parameters.AddWithValue("@accountId", account.Name);
				mc.Parameters.AddWithValue("@authority", account.Authority);
				mc.Parameters.AddWithValue("@lastlogin", account.LastLogin);
				mc.Parameters.AddWithValue("@lastip", account.LastIp);
				mc.Parameters.AddWithValue("@bannedreason", account.BannedReason);
				mc.Parameters.AddWithValue("@bannedexpiration", account.BannedExpiration);

				mc.ExecuteNonQuery();
			}

			// Save characters
			foreach (var character in account.Characters.Where(a => a.Save))
				this.SaveCharacter(account, character);

			foreach (var pet in account.Pets.Where(a => a.Save))
				this.SaveCharacter(account, pet);
		}

		public void SaveCharacter(Account account, MabiPC character)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				// Corrections
				// ----------------------------------------------------------
				// Inside dungeon, would make ppl stuck at loading.
				if (character.Region >= 10000 && character.Region <= 20000)
				{
					// TODO: Implement a "return to" location.
					character.SetLocation(13, 3329, 2948); // Alby altar
				}

				var characterLocation = character.GetPosition();

				var mc = new MySqlCommand(
					"UPDATE `characters` SET" +
					" `race` = @race, `status` = @status, `height` = @height, `fatness` = @fatness, `upper` = @upper, `lower` = @lower," +
					" `region` = @region, `x` = @x, `y` = @y, `direction` = @direction, `weaponSet` = @weaponSet, `title` = @title," +
					" `life` = @life, `injuries` = @injuries, `lifeMax` = @lifeMax," +
					" `mana` = @mana, `manaMax` = @manaMax," +
					" `stamina` = @stamina, `staminaMax` = @staminaMax, `food` = @food," +
					" `level` = @level, `totalLevel` = @totalLevel, `experience` = @experience, `age` = @age," +
					" `color1` = @color1, `color2` = @color2, `color3` = @color3," +
					" `strength` = @strength, `dexterity` = @dexterity, `intelligence` = @intelligence, `will` = @will, `luck` = @luck," +
					" `abilityPoints` = @abilityPoints, `lastTown` = @lastTown, `lastDungeon` = @lastDungeon" +
					" WHERE `characterId` = @characterId"
				, conn);

				mc.Parameters.AddWithValue("@characterId", character.Id);
				mc.Parameters.AddWithValue("@race", character.Race);
				mc.Parameters.AddWithValue("@status", (uint)character.State);
				mc.Parameters.AddWithValue("@height", character.Height);
				mc.Parameters.AddWithValue("@fatness", character.Fat);
				mc.Parameters.AddWithValue("@upper", character.Upper);
				mc.Parameters.AddWithValue("@lower", character.Lower);
				mc.Parameters.AddWithValue("@region", character.Region);
				mc.Parameters.AddWithValue("@x", characterLocation.X);
				mc.Parameters.AddWithValue("@y", characterLocation.Y);
				mc.Parameters.AddWithValue("@direction", character.Direction);
				mc.Parameters.AddWithValue("@battleState", character.BattleState);
				mc.Parameters.AddWithValue("@weaponSet", character.WeaponSet);
				mc.Parameters.AddWithValue("@life", character.Life);
				mc.Parameters.AddWithValue("@injuries", character.Injuries);
				mc.Parameters.AddWithValue("@lifeMax", character.LifeMaxBase);
				mc.Parameters.AddWithValue("@mana", character.Mana);
				mc.Parameters.AddWithValue("@manaMax", character.ManaMaxBase);
				mc.Parameters.AddWithValue("@stamina", character.Stamina);
				mc.Parameters.AddWithValue("@staminaMax", character.StaminaMaxBase);
				mc.Parameters.AddWithValue("@food", character.Hunger);
				mc.Parameters.AddWithValue("@level", character.Level);
				mc.Parameters.AddWithValue("@totalLevel", character.LevelTotal);
				mc.Parameters.AddWithValue("@experience", character.Experience);
				mc.Parameters.AddWithValue("@age", character.Age);
				mc.Parameters.AddWithValue("@strength", character.StrBase);
				mc.Parameters.AddWithValue("@dexterity", character.DexBase);
				mc.Parameters.AddWithValue("@intelligence", character.IntBase);
				mc.Parameters.AddWithValue("@will", character.WillBase);
				mc.Parameters.AddWithValue("@luck", character.LuckBase);
				mc.Parameters.AddWithValue("@abilityPoints", character.AbilityPoints);
				mc.Parameters.AddWithValue("@lastTown", "");
				mc.Parameters.AddWithValue("@lastDungeon", "");
				mc.Parameters.AddWithValue("@birthday", DateTime.MinValue);
				mc.Parameters.AddWithValue("@title", character.Title);
				mc.Parameters.AddWithValue("@maxLevel", 200);
				mc.Parameters.AddWithValue("@rebirthCount", character.RebirthCount);
				mc.Parameters.AddWithValue("@jobId", 0);
				mc.Parameters.AddWithValue("@color1", character.Color1);
				mc.Parameters.AddWithValue("@color2", character.Color2);
				mc.Parameters.AddWithValue("@color3", character.Color3);

				mc.ExecuteNonQuery();
			}

			this.SaveQuests(character);
			this.SaveItems(character);
			this.SaveKeywords(character);
			this.SaveSkills(character);
		}

		private void SaveQuests(MabiPC character)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				MySqlTransaction transaction = null;
				try
				{
					transaction = conn.BeginTransaction();

					var delmc = new MySqlCommand("DELETE FROM quests WHERE characterId = @characterId", conn, transaction);
					delmc.Parameters.AddWithValue("@characterId", character.Id);
					delmc.ExecuteNonQuery();

					//delmc = new MySqlCommand("DELETE FROM quest_progress WHERE characterId = @characterId", conn, transaction);
					//delmc.Parameters.AddWithValue("@characterId", character.Id);
					//delmc.ExecuteNonQuery();

					foreach (var q in character.Quests.Values)
					{
						if (q.Id >= Id.QuestsTmp)
							q.Id = MabiDb.Instance.GetNewPoolId("quests", Id.Quests);

						var mc = new MySqlCommand("INSERT INTO quests VALUES (@characterId, @questId, @questClass, @state)", conn, transaction);

						mc.Parameters.AddWithValue("@characterId", character.Id);
						mc.Parameters.AddWithValue("@questId", q.Id);
						mc.Parameters.AddWithValue("@questClass", q.Class);
						mc.Parameters.AddWithValue("@state", q.State);

						mc.ExecuteNonQuery();

						foreach (MabiQuestProgress p in q.Progresses.Values)
						{
							mc = new MySqlCommand("INSERT INTO quest_progress VALUES (NULL, @characterId, @questId, @questClass, @objective, @count, @done, @unlocked)", conn, transaction);

							mc.Parameters.AddWithValue("@characterId", character.Id);
							mc.Parameters.AddWithValue("@questId", q.Id);
							mc.Parameters.AddWithValue("@questClass", q.Class);
							mc.Parameters.AddWithValue("@objective", p.Objective);
							mc.Parameters.AddWithValue("@count", p.Count);
							mc.Parameters.AddWithValue("@done", p.Done);
							mc.Parameters.AddWithValue("@unlocked", p.Unlocked);

							mc.ExecuteNonQuery();
						}
					}

					transaction.Commit();
				}
				catch (Exception ex)
				{
					transaction.Rollback();
					throw ex;
				}
			}
		}

		private void SaveKeywords(MabiPC character)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				MySqlTransaction transaction = null;
				try
				{
					transaction = conn.BeginTransaction();

					var delmc = new MySqlCommand("DELETE FROM keywords WHERE characterId = @characterId", conn, transaction);
					delmc.Parameters.AddWithValue("@characterId", character.Id);
					delmc.ExecuteNonQuery();

					foreach (var keywordId in character.Keywords)
					{
						var mc = new MySqlCommand("INSERT INTO keywords VALUES (@keywordId, @characterId)", conn, transaction);
						mc.Parameters.AddWithValue("@keywordId", keywordId);
						mc.Parameters.AddWithValue("@characterId", character.Id);
						mc.ExecuteNonQuery();
					}

					transaction.Commit();
				}
				catch (Exception ex)
				{
					transaction.Rollback();
					throw ex;
				}
			}
		}

		private void SaveSkills(MabiPC character)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				MySqlTransaction transaction = null;
				try
				{
					transaction = conn.BeginTransaction();

					var delmc = new MySqlCommand("DELETE FROM skills WHERE characterId = @characterId", conn, transaction);
					delmc.Parameters.AddWithValue("@characterId", character.Id);
					delmc.ExecuteNonQuery();

					foreach (var skill in character.Skills.Values)
					{
						var mc = new MySqlCommand("INSERT INTO skills VALUES (@skillId, @characterId, @rank, @exp)", conn, transaction);
						mc.Parameters.AddWithValue("@skillId", skill.Info.Id);
						mc.Parameters.AddWithValue("@characterId", character.Id);
						mc.Parameters.AddWithValue("@rank", skill.Info.Rank);
						mc.Parameters.AddWithValue("@exp", skill.Info.Experience);
						mc.ExecuteNonQuery();
					}

					transaction.Commit();
				}
				catch (Exception ex)
				{
					transaction.Rollback();
					throw ex;
				}
			}
		}

		/// <summary>
		/// Handles transferring mailed items between mail system and players
		/// </summary>
		/// <param name="item">Item to transfer</param>
		/// <param name="giveTo">Player to give it to, or NULL to give it to the mail system.</param>
		public void SaveMailItem(MabiItem item, MabiCreature giveTo)
		{
			var conn = MabiDb.Instance.GetConnection();
			MySqlTransaction transaction = null;
			try
			{
				transaction = conn.BeginTransaction();

				var delmc = new MySqlCommand("DELETE FROM items WHERE `itemID` = @id", conn);
				delmc.Transaction = transaction;
				delmc.Parameters.AddWithValue("@id", item.Id);
				delmc.ExecuteNonQuery();

				var mc = new MySqlCommand(
					"INSERT INTO items"
					+ " (`characterId`, `itemID`, `class`, `pocketId`, `pos_x`, `pos_y`, `varint`, `color_01`, `color_02`, `color_03`, `price`, `bundle`,"
					+ " `linked_pocket`, `figure`, `flag`, `durability`, `durability_max`, `origin_durability_max`, `attack_min`, `attack_max`,"
					+ " `wattack_min`, `wattack_max`, `balance`, `critical`, `defence`, `protect`, `effective_range`, `attack_speed`,"
					+ " `experience`, `exp_point`, `upgraded`, `upgraded_max`, `grade`, `prefix`, `suffix`, `data`, `option`, `sellingprice`, `expiration`, `update_time`)"

					+ " VALUES (@characterId, @itemID, @class, @pocketId, @pos_x, @pos_y, @varint, @color_01, @color_02, @color_03, @price, @bundle,"
					+ " @linked_pocket, @figure, @flag, @durability, @durability_max, @origin_durability_max, @attack_min, @attack_max,"
					+ " @wattack_min, @wattack_max, @balance, @critical, @defence, @protect, @effective_range, @attack_speed,"
					+ " @experience, @exp_point, @upgraded, @upgraded_max, @grade, @prefix, @suffix, @data, @option, @sellingprice, @expiration, @update_time)"
				, conn);

				mc.Transaction = transaction;

				if (giveTo == null)
					mc.Parameters.AddWithValue("@characterId", 1); //Mail System ID
				else
					mc.Parameters.AddWithValue("@characterId", giveTo.Id);

				mc.Parameters.AddWithValue("@itemID", item.Id);
				mc.Parameters.AddWithValue("@class", item.Info.Class);
				mc.Parameters.AddWithValue("@pocketId", item.Info.Pocket);
				mc.Parameters.AddWithValue("@pos_x", item.Info.X);
				mc.Parameters.AddWithValue("@pos_y", item.Info.Y);
				mc.Parameters.AddWithValue("@varint", 0); // ???
				mc.Parameters.AddWithValue("@color_01", item.Info.ColorA);
				mc.Parameters.AddWithValue("@color_02", item.Info.ColorB);
				mc.Parameters.AddWithValue("@color_03", item.Info.ColorC);
				mc.Parameters.AddWithValue("@price", item.OptionInfo.Price);
				mc.Parameters.AddWithValue("@bundle", item.Info.Amount);
				mc.Parameters.AddWithValue("@linked_pocket", item.OptionInfo.LinkedPocketId);
				mc.Parameters.AddWithValue("@figure", item.Info.FigureA);
				mc.Parameters.AddWithValue("@flag", item.OptionInfo.Flag);
				mc.Parameters.AddWithValue("@durability", item.OptionInfo.Durability);
				mc.Parameters.AddWithValue("@durability_max", item.OptionInfo.DurabilityMax);
				mc.Parameters.AddWithValue("@origin_durability_max", item.OptionInfo.DurabilityOriginal);
				mc.Parameters.AddWithValue("@attack_min", item.OptionInfo.AttackMin);
				mc.Parameters.AddWithValue("@attack_max", item.OptionInfo.AttackMax);
				mc.Parameters.AddWithValue("@wattack_min", item.OptionInfo.WAttackMin);
				mc.Parameters.AddWithValue("@wattack_max", item.OptionInfo.WAttackMax);
				mc.Parameters.AddWithValue("@balance", item.OptionInfo.Balance);
				mc.Parameters.AddWithValue("@critical", item.OptionInfo.Critical);
				mc.Parameters.AddWithValue("@defence", item.OptionInfo.Defense);
				mc.Parameters.AddWithValue("@protect", item.OptionInfo.Protection);
				mc.Parameters.AddWithValue("@effective_range", item.OptionInfo.EffectiveRange);
				mc.Parameters.AddWithValue("@attack_speed", item.OptionInfo.AttackSpeed);
				mc.Parameters.AddWithValue("@experience", item.OptionInfo.Experience);
				mc.Parameters.AddWithValue("@exp_point", 0); // ?
				mc.Parameters.AddWithValue("@upgraded", item.OptionInfo.Upgraded);
				mc.Parameters.AddWithValue("@upgraded_max", item.OptionInfo.UpgradeMax);
				mc.Parameters.AddWithValue("@grade", item.OptionInfo.Grade);
				mc.Parameters.AddWithValue("@prefix", item.OptionInfo.Prefix);
				mc.Parameters.AddWithValue("@suffix", item.OptionInfo.Suffix);
				mc.Parameters.AddWithValue("@data", "");
				mc.Parameters.AddWithValue("@option", "");
				mc.Parameters.AddWithValue("@sellingprice", item.OptionInfo.SellingPrice);
				mc.Parameters.AddWithValue("@expiration", item.OptionInfo.ExpireTime);
				mc.Parameters.AddWithValue("@update_time", DateTime.Now);

				mc.ExecuteNonQuery();

				transaction.Commit();
			}
			finally
			{
				conn.Close();
			}
		}

		private void SaveItems(MabiPC character)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				MySqlTransaction transaction = null;
				try
				{
					transaction = conn.BeginTransaction();

					var delmc = new MySqlCommand("DELETE FROM items WHERE characterId = @id", conn, transaction);
					delmc.Parameters.AddWithValue("@id", character.Id);
					delmc.ExecuteNonQuery();

					var mc = new MySqlCommand(
						"INSERT INTO items"
						+ " (`characterId`, `itemID`, `class`, `pocketId`, `pos_x`, `pos_y`, `varint`, `color_01`, `color_02`, `color_03`, `price`, `bundle`,"
						+ " `linked_pocket`, `figure`, `flag`, `durability`, `durability_max`, `origin_durability_max`, `attack_min`, `attack_max`,"
						+ " `wattack_min`, `wattack_max`, `balance`, `critical`, `defence`, `protect`, `effective_range`, `attack_speed`,"
						+ " `experience`, `exp_point`, `upgraded`, `upgraded_max`, `grade`, `prefix`, `suffix`, `data`, `option`, `sellingprice`, `expiration`, `update_time`, `tags`)"

						+ " VALUES (@characterId, @itemID, @class, @pocketId, @pos_x, @pos_y, @varint, @color_01, @color_02, @color_03, @price, @bundle,"
						+ " @linked_pocket, @figure, @flag, @durability, @durability_max, @origin_durability_max, @attack_min, @attack_max,"
						+ " @wattack_min, @wattack_max, @balance, @critical, @defence, @protect, @effective_range, @attack_speed,"
						+ " @experience, @exp_point, @upgraded, @upgraded_max, @grade, @prefix, @suffix, @data, @option, @sellingprice, @expiration, @update_time, @tags)"
					, conn, transaction);

					foreach (var item in character.Items)
					{
						// If item has a temporary Id
						if (item.Id >= Id.TmpItems)
						{
							item.Id = MabiDb.Instance.GetNewItemId();
						}

						mc.Parameters.Clear();
						mc.Parameters.AddWithValue("@characterId", character.Id);
						mc.Parameters.AddWithValue("@itemID", item.Id);
						mc.Parameters.AddWithValue("@class", item.Info.Class);
						mc.Parameters.AddWithValue("@pocketId", item.Info.Pocket);
						mc.Parameters.AddWithValue("@pos_x", item.Info.X);
						mc.Parameters.AddWithValue("@pos_y", item.Info.Y);
						mc.Parameters.AddWithValue("@varint", 0); // ???
						mc.Parameters.AddWithValue("@color_01", item.Info.ColorA);
						mc.Parameters.AddWithValue("@color_02", item.Info.ColorB);
						mc.Parameters.AddWithValue("@color_03", item.Info.ColorC);
						mc.Parameters.AddWithValue("@price", item.OptionInfo.Price);
						mc.Parameters.AddWithValue("@bundle", item.Info.Amount);
						mc.Parameters.AddWithValue("@linked_pocket", item.OptionInfo.LinkedPocketId);
						mc.Parameters.AddWithValue("@figure", item.Info.FigureA);
						mc.Parameters.AddWithValue("@flag", item.OptionInfo.Flag);
						mc.Parameters.AddWithValue("@durability", item.OptionInfo.Durability);
						mc.Parameters.AddWithValue("@durability_max", item.OptionInfo.DurabilityMax);
						mc.Parameters.AddWithValue("@origin_durability_max", item.OptionInfo.DurabilityOriginal);
						mc.Parameters.AddWithValue("@attack_min", item.OptionInfo.AttackMin);
						mc.Parameters.AddWithValue("@attack_max", item.OptionInfo.AttackMax);
						mc.Parameters.AddWithValue("@wattack_min", item.OptionInfo.WAttackMin);
						mc.Parameters.AddWithValue("@wattack_max", item.OptionInfo.WAttackMax);
						mc.Parameters.AddWithValue("@balance", item.OptionInfo.Balance);
						mc.Parameters.AddWithValue("@critical", item.OptionInfo.Critical);
						mc.Parameters.AddWithValue("@defence", item.OptionInfo.Defense);
						mc.Parameters.AddWithValue("@protect", item.OptionInfo.Protection);
						mc.Parameters.AddWithValue("@effective_range", item.OptionInfo.EffectiveRange);
						mc.Parameters.AddWithValue("@attack_speed", item.OptionInfo.AttackSpeed);
						mc.Parameters.AddWithValue("@experience", item.OptionInfo.Experience);
						mc.Parameters.AddWithValue("@exp_point", 0); // ?
						mc.Parameters.AddWithValue("@upgraded", item.OptionInfo.Upgraded);
						mc.Parameters.AddWithValue("@upgraded_max", item.OptionInfo.UpgradeMax);
						mc.Parameters.AddWithValue("@grade", item.OptionInfo.Grade);
						mc.Parameters.AddWithValue("@prefix", item.OptionInfo.Prefix);
						mc.Parameters.AddWithValue("@suffix", item.OptionInfo.Suffix);
						mc.Parameters.AddWithValue("@data", "");
						mc.Parameters.AddWithValue("@option", "");
						mc.Parameters.AddWithValue("@sellingprice", item.OptionInfo.SellingPrice);
						mc.Parameters.AddWithValue("@expiration", item.OptionInfo.ExpireTime);
						mc.Parameters.AddWithValue("@update_time", DateTime.Now);
						mc.Parameters.AddWithValue("@tags", item.Tags.ToString());

						mc.ExecuteNonQuery();
					}

					transaction.Commit();
				}
				catch (Exception ex)
				{
					transaction.Rollback();
					throw ex;
				}
			}
		}

		/// <summary>
		/// Inserts a new channel and server to the database, or updates it, if it already exists.
		/// </summary>
		/// <param name="serverName">Name of the server this channel belongs to.</param>
		/// <param name="channelName">Name of the channel.</param>
		/// <param name="ipAddress"></param>
		/// <param name="port"></param>
		/// <param name="stress">Load of the server (0-75)</param>
		public void RegisterChannel(string serverName, string channelName, string ipAddress, ushort port, uint stress)
		{
			var conn = MabiDb.Instance.GetConnection();
			try
			{
				MabiDb.Instance.QueryN(string.Format(
					"INSERT INTO channels (server, name, ip, port, heartbeat, stress) VALUES ('{0}', '{1}', '{2}', {3}, NOW(), {4})" +
					"ON DUPLICATE KEY UPDATE heartbeat = NOW(), stress = {4}",
					serverName, channelName, ipAddress, port, stress
				), conn);
			}
			finally
			{
				conn.Close();
			}
		}

		/// <summary>
		/// Reads all servers, incl. channels, from the database.
		/// </summary>
		/// <returns></returns>
		public List<MabiServers> GetServerList()
		{
			var conn = MabiDb.Instance.GetConnection();
			try
			{
				var serverList = new List<MabiServers>();

				using (var reader = MabiDb.Instance.Query("SELECT * FROM channels ORDER BY name ASC", conn))
				{
					while (reader.Read())
					{
						var serverName = reader.GetString("server");

						// Channel servers will update "heartbeat" regularly.
						// If the value hasn't been updated recently, assume they died in a fire.
						var state = (ChannelState)(DateTime.Now.Subtract(reader.GetDateTime("heartbeat")).TotalMinutes >= 2 ? (byte)0 : reader.GetByte("state"));
						var events = (ChannelEvent)reader.GetByte("events");
						var stress = reader.GetByte("stress");

						var newChannel = new MabiChannel(reader.GetString("name"), reader.GetString("ip"), reader.GetUInt16("port"), state, events, stress);

						// Add server if it's new to us
						if (!serverList.Exists(a => a.Name == serverName))
							serverList.Add(new MabiServers(serverName));

						serverList.First(a => a.Name == serverName).Channels.Add(newChannel);
					}
				}

				return serverList;
			}
			finally
			{
				conn.Close();
			}
		}
	}
}
