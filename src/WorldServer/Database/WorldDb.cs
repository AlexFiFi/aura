// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using Aura.Shared.Const;
using Aura.Shared.Database;
using Aura.Shared.Util;
using Aura.World.Player;
using Aura.World.Scripting;
using Aura.World.World;
using Aura.World.World.Guilds;
using MySql.Data.MySqlClient;

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

				account.Vars.Load(account.Name, 0);

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

						character.Vars.Load(account.Name, character.Id);

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
						var pet = this.GetCharacter<MabiPet>((ulong)reader.GetInt64("characterId"));

						pet.Vars.Load(account.Name, pet.Id);

						account.Pets.Add(pet);
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
		public T GetCharacter<T>(ulong id) where T : MabiPC, new()
		{
			var conn = MabiDb.Instance.GetConnection();
			try
			{
				var character = new T();
				float lifeDelta, manaDelta, stamDelta;

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
					character.EyeType = reader.GetByte("eyeType");
					character.Mouth = reader.GetByte("mouthType");
					character.Height = reader.GetFloat("height");
					character.Weight = reader.GetFloat("fatness");
					character.Upper = reader.GetFloat("upper");
					character.Lower = reader.GetFloat("lower");
					character.SetLocation(reader.GetUInt32("region"), reader.GetUInt32("x"), reader.GetUInt32("y"));
					character.Direction = reader.GetByte("direction");
					character.BattleState = reader.GetByte("battleState");
					character.Inventory.WeaponSet = (WeaponSet)reader.GetByte("weaponSet");
					character.Injuries = reader.GetFloat("injuries");
					character.Life = (character.LifeMaxBase = reader.GetFloat("lifeMax"));
					lifeDelta = reader.GetFloat("lifeDelta");
					character.Mana = (character.ManaMaxBase = reader.GetFloat("manaMax"));
					manaDelta = reader.GetFloat("manaDelta");
					character.Stamina = (character.StaminaMaxBase = reader.GetFloat("staminaMax"));
					character.Hunger = reader.GetFloat("food");
					stamDelta = reader.GetFloat("staminaDelta");
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
					character.Title = reader.GetUInt16("title");
					character.OptionTitle = reader.GetUInt16("optionTitle");
					character.Talents.SelectedTitle = (TalentTitle)reader.GetUInt16("talentTitle");
					character.Talents.Grandmaster = (TalentId)reader.GetByte("grandmasterTalent");
					character.Color1 = reader.GetUInt32("color1");
					character.Color2 = reader.GetUInt32("color2");
					character.Color3 = reader.GetUInt32("color3");
					character.PvPWins = reader.GetUInt64("pvpWins");
					character.PvPLosses = reader.GetUInt64("pvpLosses");
					character.EvGEnabled = reader.GetBoolean("evGEnabled");
					character.EvGSupportRace = reader.GetByte("evGSupportRace");
					character.TransPvPEnabled = reader.GetBoolean("transPvPEnabled");
					character.CauseOfDeath = (DeathCauses)reader.GetInt32("causeOfDeath");
					character.State = (CreatureStates)reader.GetUInt32("status") & ~CreatureStates.SitDown;

					character.LoadDefault();
				}

				this.GetQuests(character);
				this.GetItems(character);
				this.GetKeywords(character);
				this.GetSkills(character);
				this.GetTitles(character);
				this.GetCooldowns(character);

				character.Guild = this.GetGuildForChar(character.Id);
				character.GuildMember = this.GetGuildMember(character.Id);

				if (character.Guild != null && character.GuildMember.MemberRank < GuildMemberRank.Applied && character.Guild.Title != null)
					character.Titles.Add(50000, true);

				character.Shamalas.Add(new ShamalaTransformation(1, 1, ShamalaState.Available));
				character.Shamalas.Add(new ShamalaTransformation(2, 1, ShamalaState.Available));
				character.Shamalas.Add(new ShamalaTransformation(3, 1, ShamalaState.Available));

				character.Life -= lifeDelta;
				character.Mana -= manaDelta;
				character.Stamina -= stamDelta;

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

						character.Skills.Add(skill);
						character.Skills.AddBonuses(skill);
						character.Talents.UpdateExp(skill.Id, skill.Rank);
					}
				}
			}
			finally
			{
				conn.Close();

				if (!character.Skills.Has(SkillConst.MeleeCombatMastery))
				{
					var skill = new MabiSkill(SkillConst.MeleeCombatMastery, SkillRank.RF, character.Race);
					character.Skills.Add(skill);
					character.Skills.AddBonuses(skill);
					character.Talents.UpdateExp(skill.Id, skill.Rank);
				}

				character.Talents.UpdateStats();
			}
		}

		private void GetTitles(MabiPC character)
		{
			var conn = MabiDb.Instance.GetConnection();
			try
			{
				using (var reader = MabiDb.Instance.Query("SELECT titleId, usable FROM titles WHERE characterId = " + character.Id.ToString(), conn))
				{
					while (reader.Read())
					{
						character.Titles.Add(reader.GetUInt16("titleId"), reader.GetBoolean("usable"));
					}
				}
			}
			finally
			{
				conn.Close();
			}
		}

		/// <summary>
		/// Reads all items for the given character from the database, and adds them to it.
		/// </summary>
		/// <param name="character"></param>
		private void GetItems(MabiPC character)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				using (var reader = MabiDb.Instance.Query("SELECT * FROM items WHERE characterId = " + character.Id.ToString() + " ORDER BY pocketId, pos_y, pos_x", conn))
				{
					while (reader.Read())
					{
						var item = GetItem(reader);

						if (!character.Inventory.Has(item.Pocket))
							throw new Exception(string.Format("Character '{0}' failed to load, pocket '{1}' missing in inventory.", character.Name, item.Pocket));

						character.Inventory.ForcePutItem(item, item.Pocket);
					}
				}
			}
		}

		public MabiItem GetItem(ulong Id)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				using (var reader = MabiDb.Instance.Query("SELECT * FROM items WHERE itemId = " + Id.ToString(), conn))
				{
					if (!reader.HasRows)
						return null;
					reader.Read();
					return GetItem(reader);
				}
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

		// TODO: Server
		public bool IsValidMailRecpient(string name, out ulong id)
		{
			id = 0;
			if (!(new Regex(@"^[a-zA-Z0-9]{3,15}$")).IsMatch(name))
				return false;

			using (var conn = MabiDb.Instance.GetConnection())
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

			account.Vars.Save(account.Name, 0);
		}

		public void SaveCharacter(Account account, MabiPC character)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				// Corrections
				// ----------------------------------------------------------
				// Inside dungeon, would make ppl stuck at loading.
				// TODO: Other areas should not be saved, eg Alby Arena (29)
				if (character.Region >= 10000 && character.Region <= 20000)
				{
					// TODO: Implement a "return to" location.
					character.SetLocation(13, 3329, 2948); // Alby altar
				}

				var characterLocation = character.GetPosition();

				var mc = new MySqlCommand(
					"UPDATE `characters` SET" +
					" `race` = @race, `status` = @status, `height` = @height, `fatness` = @fatness, `upper` = @upper, `lower` = @lower," +
					" `region` = @region, `x` = @x, `y` = @y, `direction` = @direction, `weaponSet` = @weaponSet, `title` = @title, `optionTitle` = @optionTitle," +
					" `talentTitle` = @talentTitle, `grandmasterTalent` = @grandmasterTalent, `lifeDelta` = @lifeDelta, `injuries` = @injuries, `lifeMax` = @lifeMax," +
					" `manaDelta` = @manaDelta, `manaMax` = @manaMax," +
					" `staminaDelta` = @staminaDelta, `staminaMax` = @staminaMax, `food` = @food," +
					" `level` = @level, `totalLevel` = @totalLevel, `experience` = @experience, `age` = @age," +
					" `color1` = @color1, `color2` = @color2, `color3` = @color3," +
					" `strength` = @strength, `dexterity` = @dexterity, `intelligence` = @intelligence, `will` = @will, `luck` = @luck," +
					" `abilityPoints` = @abilityPoints, `lastTown` = @lastTown, `lastDungeon` = @lastDungeon," +
					" `pvpWins` = @pvpWins, `pvpLosses` = @pvpLosses, `evGEnabled` = @evGEnabled, `evGSupportRace` = @evGSupportRace, `transPvPEnabled` = @transPvPEnabled," +
					" `causeOfDeath` = @causeOfDeath" +
					" WHERE `characterId` = @characterId"
				, conn);

				mc.Parameters.AddWithValue("@characterId", character.Id);
				mc.Parameters.AddWithValue("@race", character.Race);
				mc.Parameters.AddWithValue("@status", (uint)character.State);
				mc.Parameters.AddWithValue("@height", character.Height);
				mc.Parameters.AddWithValue("@fatness", character.Weight);
				mc.Parameters.AddWithValue("@upper", character.Upper);
				mc.Parameters.AddWithValue("@lower", character.Lower);
				mc.Parameters.AddWithValue("@region", character.Region);
				mc.Parameters.AddWithValue("@x", characterLocation.X);
				mc.Parameters.AddWithValue("@y", characterLocation.Y);
				mc.Parameters.AddWithValue("@direction", character.Direction);
				mc.Parameters.AddWithValue("@battleState", character.BattleState);
				mc.Parameters.AddWithValue("@weaponSet", (byte)character.Inventory.WeaponSet);
				mc.Parameters.AddWithValue("@lifeDelta", character.LifeMax - character.Life);
				mc.Parameters.AddWithValue("@injuries", character.Injuries);
				mc.Parameters.AddWithValue("@lifeMax", character.LifeMaxBase);
				mc.Parameters.AddWithValue("@manaDelta", character.ManaMax - character.Mana);
				mc.Parameters.AddWithValue("@manaMax", character.ManaMaxBase);
				mc.Parameters.AddWithValue("@staminaDelta", character.StaminaMax - character.Stamina);
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
				mc.Parameters.AddWithValue("@optionTitle", character.OptionTitle);
				mc.Parameters.AddWithValue("@talentTitle", (ushort)character.Talents.SelectedTitle);
				mc.Parameters.AddWithValue("@grandmasterTalent", (byte)character.Talents.Grandmaster);
				mc.Parameters.AddWithValue("@maxLevel", 200);
				mc.Parameters.AddWithValue("@rebirthCount", character.RebirthCount);
				mc.Parameters.AddWithValue("@jobId", 0);
				mc.Parameters.AddWithValue("@color1", character.Color1);
				mc.Parameters.AddWithValue("@color2", character.Color2);
				mc.Parameters.AddWithValue("@color3", character.Color3);
				mc.Parameters.AddWithValue("@pvpWins", character.PvPWins);
				mc.Parameters.AddWithValue("@pvpLosses", character.PvPLosses);
				mc.Parameters.AddWithValue("@evGEnabled", character.EvGEnabled);
				mc.Parameters.AddWithValue("@evGSupportRace", character.EvGSupportRace);
				mc.Parameters.AddWithValue("@transPvPEnabled", character.TransPvPEnabled);
				mc.Parameters.AddWithValue("@causeOfDeath", character.CauseOfDeath);

				mc.ExecuteNonQuery();
			}

			this.SaveQuests(character);
			this.SaveItems(character);
			this.SaveKeywords(character);
			this.SaveSkills(character);
			this.SaveTitles(character);
			this.SaveCooldowns(character);

			character.Vars.Save(account.Name, character.Id);
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
				catch
				{
					transaction.Rollback();
					throw;
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
				catch
				{
					transaction.Rollback();
					throw;
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

					foreach (var skill in character.Skills.List.Values)
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
				catch
				{
					transaction.Rollback();
					throw;
				}
			}
		}

		private void SaveTitles(MabiPC character)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				MySqlTransaction transaction = null;
				try
				{
					transaction = conn.BeginTransaction();

					var delmc = new MySqlCommand("DELETE FROM titles WHERE characterId = @characterId", conn, transaction);
					delmc.Parameters.AddWithValue("@characterId", character.Id);
					delmc.ExecuteNonQuery();

					foreach (var title in character.Titles)
					{
						if (title.Key == 60000 || title.Key == 60001 || title.Key == 50000) // GM, devCAT, Guild. Dynamic title, so should not be saved
							continue;

						var mc = new MySqlCommand("INSERT INTO titles VALUES (@characterId, @titleId, @usable)", conn, transaction);
						mc.Parameters.AddWithValue("@characterId", character.Id);
						mc.Parameters.AddWithValue("@titleId", title.Key);
						mc.Parameters.AddWithValue("@usable", title.Value);
						mc.ExecuteNonQuery();
					}

					transaction.Commit();
				}
				catch
				{
					transaction.Rollback();
					throw;
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
			catch
			{
				transaction.Rollback();
				throw;
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

					foreach (var item in character.Inventory.Items)
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
				catch
				{
					transaction.Rollback();
					throw;
				}
			}
		}

		// XXX: Should we really do all this on-the-fly?
		public MabiGuild GetGuildForChar(ulong charId)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				var mc = new MySqlCommand("SELECT guildId FROM guild_members WHERE characterId = " + charId, conn);

				using (var reader = mc.ExecuteReader())
				{
					if (reader.Read())
						return this.GetGuild(reader.GetUInt64("guildId"));
				}
			}

			return null;
		}

		public MabiGuild GetGuild(ulong guildId)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				var mc = new MySqlCommand(
					"SELECT g.*, c.characterId AS leaderId, c.name AS leaderName " +
					"FROM guilds AS g " +
					"INNER JOIN guild_members AS gm ON g.guildId = gm.guildId " +
					"INNER JOIN characters AS c ON gm.characterId = c.characterId " +
					"WHERE g.guildId = @guildId AND gm.rank = 0"
				, conn);

				mc.Parameters.AddWithValue("@guildId", guildId);

				using (var reader = mc.ExecuteReader())
				{
					if (reader.Read())
						return ParseGuild(reader);
				}
			}

			return null;
		}

		public List<MabiGuild> LoadGuilds()
		{
			var guilds = new List<MabiGuild>();

			using (var conn = MabiDb.Instance.GetConnection())
			{
				var mc = new MySqlCommand(
					"SELECT g.*, c.characterId AS leaderId, c.name AS leaderName " +
					"FROM guilds AS g " +
					"INNER JOIN guild_members AS gm ON g.guildId = gm.guildId " +
					"INNER JOIN characters AS c ON gm.characterId = c.characterId " +
					"WHERE gm.rank = 0"
				, conn);

				using (var reader = mc.ExecuteReader())
				{
					while (reader.Read())
						guilds.Add(ParseGuild(reader));
				}
			}

			return guilds;
		}

		private MabiGuild ParseGuild(MySqlDataReader reader)
		{
			var guild = new MabiGuild();
			guild.Id = reader.GetUInt64("guildId");
			guild.Name = reader.GetString("name");
			guild.IntroMessage = reader.GetString("intro");
			guild.WelcomeMessage = reader.GetString("welcome");
			guild.LeavingMessage = reader.GetString("leaving");
			guild.RejectionMessage = reader.GetString("rejection");

			guild.GuildLevel = (GuildLevel)reader.GetByte("level");
			guild.Type = (GuildType)reader.GetByte("type");

			guild.Region = reader.GetUInt32("region");
			guild.X = reader.GetUInt32("x");
			guild.Y = reader.GetUInt32("y");
			guild.Rotation = reader.GetByte("rotation");

			guild.Gp = reader.GetUInt32("gp");
			guild.Gold = reader.GetUInt32("gold");
			guild.StoneClass = (GuildStoneType)reader.GetUInt32("stone_type");

			guild.Title = reader.GetStringSafe("title");
			guild.Options = reader.GetByte("Options");

			guild.LeaderName = reader.GetStringSafe("leaderName");

			if (guild.LeaderName == null)
				Logger.Warning("Guild \"{0}\" doesn't have a leader!", guild.Name);

			return guild;
		}

		/// <summary>
		/// Checks if the name is okay, and if a guild with the given name exists.
		/// </summary>
		public bool GuildNameOkay(string name)
		{
			if (!Regex.IsMatch(name, @"^[a-zA-Z0-9]{1,15}$"))
				return false;

			using (var conn = MabiDb.Instance.GetConnection())
			{
				name = MySqlHelper.EscapeString(name);

				var mc = new MySqlCommand("SELECT guildId FROM guilds WHERE name = @name", conn);
				mc.Parameters.AddWithValue("@name", name);

				using (var reader = mc.ExecuteReader())
					return !reader.HasRows;
			}
		}

		public void DeleteGuildMember(MabiGuildMember m)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				MabiDb.Instance.QueryN("DELETE FROM guild_members WHERE characterId = " + m.CharacterId, conn);
			}
		}

		public void DeleteGuild(MabiGuild guild)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				var delmc = new MySqlCommand("DELETE FROM guilds WHERE guildId = @guildId", conn);
				delmc.Parameters.AddWithValue("@guildId", guild.Id);
				delmc.ExecuteNonQuery();
			}
		}

		public ulong GetGuildLeaderId(ulong guildId)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				var mc = new MySqlCommand("SELECT * FROM guild_members WHERE guildId = @guildId AND rank = 0", conn);

				using (var reader = mc.ExecuteReader())
				{
					if (reader.Read())
						return reader.GetUInt64("characterId");
				}
			}

			return 0;
		}

		public ulong SaveGuild(MabiGuild guild)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				var mc = new MySqlCommand(
					"INSERT INTO `guilds`" +
					"(`name`, `intro`, `welcome`, `leaving`, `rejection`, `level`, `type`, `region`, `x`, `y`, `rotation`, `gp`, `gold`, `stone_type`) " +
					"VALUES (@name, @intro, @welcome, @leaving, @rejection, @level, @type, @region, @x, @y, @rotation, @gp, @gold, @stone_type) " +
					"ON DUPLICATE KEY UPDATE `gp` = @gp, `gold` =  @gold"
				, conn);

				mc.Parameters.AddWithValue("@name", guild.Name);
				mc.Parameters.AddWithValue("@intro", guild.IntroMessage);
				mc.Parameters.AddWithValue("@welcome", guild.WelcomeMessage);
				mc.Parameters.AddWithValue("@leaving", guild.LeavingMessage);
				mc.Parameters.AddWithValue("@rejection", guild.RejectionMessage);
				mc.Parameters.AddWithValue("@level", (byte)guild.GuildLevel);
				mc.Parameters.AddWithValue("@type", (byte)guild.Type);
				mc.Parameters.AddWithValue("@region", guild.Region);
				mc.Parameters.AddWithValue("@x", guild.X);
				mc.Parameters.AddWithValue("@y", guild.Y);
				mc.Parameters.AddWithValue("@rotation", guild.Rotation);
				mc.Parameters.AddWithValue("@gp", guild.Gp);
				mc.Parameters.AddWithValue("@gold", guild.Gold);
				mc.Parameters.AddWithValue("@stone_type", (uint)guild.StoneClass);

				mc.ExecuteNonQuery();

				using (var r = MabiDb.Instance.Query("SELECT LAST_INSERT_ID()", conn))
				{
					r.Read();
					return (ulong)r.GetInt64(0); // TODO: Why doesn't the mc.LastInsertId work?
				}

				//return (ulong)mc.LastInsertedId;
			}
		}

		public void SaveGuildMember(MabiGuildMember member, ulong guildId)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				var mc = new MySqlCommand(
					"INSERT INTO `guild_members` (`characterId`, `guildId`, `rank`, `joined`, `guildPoints`, `appMessage`) " +
					"VALUES (@characterId, @guildId, @rank, @joined, @guildPoints, @appMessage) " +
					"ON DUPLICATE KEY UPDATE `guildPoints` = @guild_points"
				, conn);

				mc.Parameters.AddWithValue("@characterId", member.CharacterId);
				mc.Parameters.AddWithValue("@guildId", guildId);
				mc.Parameters.AddWithValue("@rank", (byte)member.MemberRank);
				mc.Parameters.AddWithValue("@joined", member.JoinedDate);
				mc.Parameters.AddWithValue("@guildPoints", (uint)member.Gp);
				mc.Parameters.AddWithValue("@appMessage", member.ApplicationText);

				mc.ExecuteNonQuery();
			}
		}

		public List<MabiGuildMember> GetGuildMembers(ulong guildId)
		{
			var members = new List<MabiGuildMember>();

			using (var conn = MabiDb.Instance.GetConnection())
			{
				var mc = new MySqlCommand("SELECT * FROM guild_members WHERE guildId = @guildId", conn);
				mc.Parameters.AddWithValue("@guildId", guildId);

				using (var reader = mc.ExecuteReader())
				{
					while (reader.Read())
					{
						var member = new MabiGuildMember();
						member.CharacterId = reader.GetUInt64("characterId");
						member.GuildId = reader.GetUInt64("guildId");
						member.MemberRank = (GuildMemberRank)reader.GetByte("rank");
						member.JoinedDate = reader.GetDateTime("joined");
						member.Gp = reader.GetUInt32("guildPoints");
						member.ApplicationText = reader.GetString("appMessage");

						members.Add(member);
					}
				}
			}

			return members;
		}

		public MabiGuildMember GetGuildMember(ulong characterId)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				var mc = new MySqlCommand("SELECT * FROM guild_members WHERE characterId = @characterId", conn);
				mc.Parameters.AddWithValue("@characterId", characterId);

				using (var reader = mc.ExecuteReader())
				{
					if (reader.Read())
					{
						var member = new MabiGuildMember();
						member.CharacterId = reader.GetUInt64("characterId");
						member.GuildId = reader.GetUInt64("guildId");
						member.MemberRank = (GuildMemberRank)reader.GetByte("rank");
						member.JoinedDate = reader.GetDateTime("joined");
						member.Gp = reader.GetUInt32("guildPoints");
						member.ApplicationText = reader.GetString("appMessage");

						return member;
					}
				}
			}

			return null;
		}

		private void GetCooldowns(MabiCreature creature)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				using (var reader = MabiDb.Instance.Query("SELECT `type`+0, id, expires, errorMessage FROM cooldowns WHERE characterId = " + creature.Id, conn))
				{
					while (reader.Read())
					{
						creature.AddCooldown((CooldownType)reader.GetUInt16("`type`+0"), reader.GetUInt32("id"),
							reader.GetDateTime("expires"), reader.GetStringSafe("errorMessage"));
					}
				}
			}
		}

		private void SaveCooldowns(MabiCreature character)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				MySqlTransaction transaction = null;
				try
				{
					transaction = conn.BeginTransaction();

					var delmc = new MySqlCommand("DELETE FROM cooldowns WHERE characterId = @characterId", conn, transaction);
					delmc.Parameters.AddWithValue("@characterId", character.Id);
					delmc.ExecuteNonQuery();

					foreach (var cooldown in character.Cooldowns)
					{
						var mc = new MySqlCommand("INSERT INTO cooldowns VALUES (@characterId, @type, @id, @expires, @errorMessage)", conn, transaction);
						mc.Parameters.AddWithValue("@characterId", character.Id);
						mc.Parameters.AddWithValue("@type", (ushort)cooldown.Type);
						mc.Parameters.AddWithValue("@id", cooldown.Id);
						mc.Parameters.AddWithValue("@expires", cooldown.Expires);
						mc.Parameters.AddWithValue("@errorMessage", cooldown.ErrorMessage);
						mc.ExecuteNonQuery();
					}

					transaction.Commit();
				}
				catch
				{
					transaction.Rollback();
					throw;
				}
			}
		}

		public VariableManager LoadVars(string accountName, ulong characterId)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				var mc = new MySqlCommand("SELECT * FROM vars WHERE accountId = @accountId AND characterId = @characterId", conn);
				mc.Parameters.AddWithValue("@accountId", accountName);
				mc.Parameters.AddWithValue("@characterId", characterId);

				var vars = new VariableManager();

				using (var reader = mc.ExecuteReader())
				{
					while (reader.Read())
						this.ReadVars(reader, vars);
				}

				return vars;
			}
		}

		private void ReadVars(MySqlDataReader reader, IDictionary<string, object> vars)
		{
			var name = reader.GetString("name");
			var type = reader.GetString("type");
			var val = reader.GetStringSafe("value");

			if (val == null)
				return;

			switch (type)
			{
				case "1u": vars[name] = byte.Parse(val); break;
				case "2u": vars[name] = ushort.Parse(val); break;
				case "4u": vars[name] = uint.Parse(val); break;
				case "8u": vars[name] = ulong.Parse(val); break;
				case "1": vars[name] = sbyte.Parse(val); break;
				case "2": vars[name] = short.Parse(val); break;
				case "4": vars[name] = int.Parse(val); break;
				case "8": vars[name] = long.Parse(val); break;
				case "f": vars[name] = float.Parse(val); break;
				case "d": vars[name] = double.Parse(val); break;
				case "b": vars[name] = bool.Parse(val); break;
				case "s": vars[name] = val; break;
				case "o":
					{
						var buffer = Convert.FromBase64String(val);
						var bf = new BinaryFormatter();
						using (var ms = new MemoryStream(buffer))
						{
							vars[name] = bf.Deserialize(ms);
						}

						break;
					}
				default:
					throw new Exception("Unknown variable type '" + type + "'");
			}
		}

		public void SaveVars(string accountName, ulong characterId, IDictionary<string, object> vars)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				MySqlTransaction transaction = null;
				try
				{
					transaction = conn.BeginTransaction();

					var deleteMc = new MySqlCommand("DELETE FROM vars WHERE accountId = @accountId AND characterId = @characterId", conn, transaction);
					deleteMc.Parameters.AddWithValue("@accountId", accountName);
					deleteMc.Parameters.AddWithValue("@characterId", characterId);
					deleteMc.ExecuteNonQuery();

					var bf = new BinaryFormatter();

					lock (vars)
					{
						foreach (var var in vars)
						{
							if (var.Value == null)
								continue;

							// Get type
							string type;
							if (var.Value is byte) type = "1u";
							else if (var.Value is ushort) type = "2u";
							else if (var.Value is uint) type = "4u";
							else if (var.Value is ulong) type = "8u";
							else if (var.Value is sbyte) type = "1";
							else if (var.Value is short) type = "2";
							else if (var.Value is int) type = "4";
							else if (var.Value is long) type = "8";
							else if (var.Value is float) type = "f";
							else if (var.Value is double) type = "d";
							else if (var.Value is bool) type = "b";
							else if (var.Value is string) type = "s";
							else type = "o";

							// Get value
							var val = string.Empty;
							if (type != "o")
							{
								val = var.Value.ToString();
							}
							else
							{
								using (var ms = new MemoryStream())
								{
									bf.Serialize(ms, var.Value);
									val = Convert.ToBase64String(ms.ToArray());
								}
							}

							if (val.Length > ushort.MaxValue)
							{
								Logger.Warning("Skipping variable '{0}', it's too big.");
								continue;
							}

							// Save
							var mc = new MySqlCommand(
								"INSERT INTO vars (accountId, characterId, name, type, value) " +
								"VALUES (@accountId, @characterId, @name, @type, @value)"
							, conn, transaction);
							mc.Parameters.AddWithValue("@accountId", accountName);
							mc.Parameters.AddWithValue("@characterId", characterId);
							mc.Parameters.AddWithValue("@name", var.Key);
							mc.Parameters.AddWithValue("@type", type);
							mc.Parameters.AddWithValue("@value", val);
							mc.ExecuteNonQuery();
						}

						transaction.Commit();
					}
				}
				catch (Exception ex)
				{
					Logger.Exception(ex);
					transaction.Rollback();
				}
			}
		}
	}
}
