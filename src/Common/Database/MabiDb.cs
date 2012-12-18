// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Common.World;
using System.Text.RegularExpressions;
using Common.Tools;
using Common.Constants;

namespace Common.Database
{
	public class MabiDb
	{
		public static readonly MabiDb Instance = new MabiDb();
		static MabiDb() { }
		private MabiDb() { }

		private string _connectionString = "";

		public void Init(string host, string user, string password, string database)
		{
			_connectionString = string.Format("server={0}; database={1}; uid={2}; password={3}; pooling=true; min pool size=0; max pool size=100;", host, database, user, password);
		}

		/// <summary>
		/// Returns a new valid connection.
		/// </summary>
		/// <returns></returns>
		public MySqlConnection GetConnection()
		{
			var result = new MySqlConnection(_connectionString);
			result.Open();

			return result;
		}

		/// <summary>
		/// Tries to get a connection to the database. If an error occures,
		/// it throws the error. Nothing special, just less to type.
		/// </summary>
		public void TestConnection()
		{
			MySqlConnection conn = null;
			try
			{
				conn = this.GetConnection();
			}
			finally
			{
				if (conn != null)
					conn.Close();
			}
		}

		/// <summary>
		/// Runs SQL command and returns a reader object.
		/// Typical use: SELECT
		/// </summary>
		public MySqlDataReader Query(string sql, MySqlConnection conn)
		{
			return (new MySqlCommand(sql, conn).ExecuteReader());
		}

		/// <summary>
		/// Runs SQL command and returns the number of affected rows.
		/// Typical use: UPDATE, DELETE
		/// </summary>
		public int QueryN(string sql, MySqlConnection conn)
		{
			int result = 0;

			using (var query = new MySqlCommand(sql, conn))
			{
				result = query.ExecuteNonQuery();
			}

			return result;
		}

		/// <summary>
		/// Runs SQL command and returns the Id of the last inserted row.
		/// Typical use: INSERT
		/// </summary>
		public long QueryI(string sql, MySqlConnection conn)
		{
			long result = 0;

			using (var query = new MySqlCommand(sql, conn))
			{
				query.ExecuteNonQuery();
				result = query.LastInsertedId;
			}

			return result;
		}

		/// <summary>
		/// Deletes contents of table 'sessions'.
		/// Passes on exceptions.
		/// </summary>
		public void ClearDatabaseCache()
		{
			var conn = this.GetConnection();
			try
			{
				this.QueryN("DELETE FROM sessions", conn);
			}
			finally
			{
				conn.Close();
			}
		}

		/// <summary>
		/// Reads the id for the given name and increments it by 1.
		/// If the type doesn't exists, it's created using the default value.
		/// </summary>
		/// <param name="type">Identificator (Example: "item")</param>
		/// <param name="def">Default value</param>
		/// <returns>Returns new usable Id.</returns>
		private ulong GetNewPoolId(string type, ulong def)
		{
			var conn = this.GetConnection();
			try
			{
				ulong id = 0;

				using (var reader = this.Query("SELECT id FROM id_pool WHERE type = '" + type + "'", conn))
				{
					if (reader.Read())
					{
						id = reader.GetUInt64("id");
					}
				}

				if (id == 0)
				{
					id = def;
					this.QueryI("INSERT INTO id_pool (type, id) VALUES ('" + type + "', " + ++def + ")", conn);
				}
				else
				{
					this.QueryN("UPDATE id_pool SET id = id + 1 WHERE type = '" + type + "'", conn);
				}

				return id;
			}
			finally
			{
				conn.Close();
			}
		}

		/// <summary>
		/// Returns a new usable character Id.
		/// </summary>
		/// <returns></returns>
		private ulong GetNewCharacterId()
		{
			return this.GetNewPoolId("characters", Id.Characters);
		}

		/// <summary>
		/// Returns a new usable pet Id.
		/// </summary>
		/// <returns></returns>
		private ulong GetNewPetId()
		{
			return this.GetNewPoolId("pets", Id.Pets);
		}

		/// <summary>
		/// Returns a new usable partner Id.
		/// </summary>
		/// <returns></returns>
		private ulong GetNewPartnerId()
		{
			return this.GetNewPoolId("partners", Id.Partners);
		}

		/// <summary>
		/// Returns a new usable item Id.
		/// </summary>
		/// <returns></returns>
		private ulong GetNewItemId()
		{
			return this.GetNewPoolId("items", Id.Items);
		}

		/// <summary>
		/// Creates a sessions for the given account, and returns the new session Id.
		/// </summary>
		/// <param name="accName"></param>
		/// <returns>Returns new session Id.</returns>
		public ulong CreateSession(string accName)
		{
			var conn = this.GetConnection();
			try
			{
				accName = MySqlHelper.EscapeString(accName);

				ulong sessionKey = (ulong)(RandomProvider.Get().NextDouble() * ulong.MaxValue);

				this.QueryN("REPLACE INTO sessions VALUES ('" + accName + "'," + sessionKey.ToString() + ")", conn);

				return sessionKey;
			}
			finally
			{
				conn.Close();
			}
		}

		/// <summary>
		/// Checks if a session for the given account name and key exists.
		/// </summary>
		/// <param name="accName"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool IsSessionKey(string accName, ulong key)
		{
			var conn = this.GetConnection();
			try
			{
				accName = MySqlHelper.EscapeString(accName);

				var reader = this.Query("SELECT session FROM sessions WHERE accountId = '" + accName + "' AND session = " + key.ToString(), conn);

				bool isKey = reader.Read();
				reader.Close();

				return isKey;
			}
			finally
			{
				conn.Close();
			}
		}

		/// <summary>
		/// Checks if an account with the given name exists.
		/// </summary>
		/// <param name="accName"></param>
		/// <returns></returns>
		public bool AccountExists(string accName)
		{
			var conn = this.GetConnection();
			try
			{
				accName = MySqlHelper.EscapeString(accName);

				var reader = this.Query("SELECT accountId FROM accounts WHERE accountId = '" + accName + "'", conn);

				bool isKey = reader.Read();
				reader.Close();

				return isKey;
			}
			finally
			{
				conn.Close();
			}
		}

		public bool AccountHasCharacter(string accountName, string charName)
		{
			var conn = this.GetConnection();
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
			var conn = this.GetConnection();
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

			var conn = this.GetConnection();
			try
			{
				name = MySqlHelper.EscapeString(name);

				using (var reader = this.Query("SELECT characterId FROM characters WHERE name = '" + name + "' AND server = '" + server + "'", conn))
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
		public MabiAccount GetAccount(string accName)
		{
			var conn = this.GetConnection();
			try
			{
				accName = MySqlHelper.EscapeString(accName);
				var account = new MabiAccount();

				using (var reader = this.Query("SELECT * FROM accounts WHERE accountId = '" + accName + "'", conn))
				{
					if (!reader.Read())
					{
						return null;
					}

					account.Username = accName;
					account.Userpass = reader.GetString("password");
					account.Authority = reader.GetByte("authority");
					account.Creation = reader["creation"] as DateTime? ?? DateTime.MinValue;
					account.LastIp = reader.GetString("lastip");
					account.LastLogin = reader["lastlogin"] as DateTime? ?? DateTime.MinValue;
					account.BannedReason = reader.GetString("bannedreason");
					account.BannedExpiration = reader["bannedexpiration"] as DateTime? ?? DateTime.MinValue;
				}

				this.GetCards(account);
				this.GetCharacters(account);

				return account;
			}
			finally
			{
				conn.Close();
			}
		}

		/// <summary>
		/// Reads all cards for the given account from the database, and adds them to it.
		/// </summary>
		/// <param name="account"></param>
		private void GetCards(MabiAccount account)
		{
			var conn = this.GetConnection();
			try
			{
				using (var reader = this.Query("SELECT cardId, type FROM character_cards WHERE accountId = '" + account.Username + "'", conn))
				{
					while (reader.Read())
					{
						var card = new MabiCard(reader.GetUInt64("cardId"), reader.GetUInt32("type"));
						account.CharacterCards.Add(card);
					}
					reader.Close();
				}

				using (var reader = this.Query("SELECT cardId, type FROM pet_cards WHERE accountId = '" + account.Username + "'", conn))
				{
					while (reader.Read())
					{
						var card = new MabiCard(reader.GetUInt64("cardId"), reader.GetUInt32("type"));
						account.PetCards.Add(card);
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
		/// Reads all characters in the given account from the database and adds them to it.
		/// </summary>
		/// <param name="account"></param>
		private void GetCharacters(MabiAccount account)
		{
			var conn = this.GetConnection();
			try
			{
				account.Characters = new List<MabiCharacter>();
				account.Pets = new List<MabiPet>();

				using (var reader = this.Query("SELECT characterId FROM characters WHERE accountId = '" + account.Username + "' AND type = 'CHARACTER'", conn))
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

				using (var reader = this.Query("SELECT characterId FROM characters WHERE accountId = '" + account.Username + "' AND type = 'PET'", conn))
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
			var conn = this.GetConnection();
			try
			{
				var character = new T();

				using (var reader = this.Query("SELECT * FROM characters WHERE characterId = " + id.ToString(), conn))
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
					character.Level = reader.GetUInt32("level");
					character.LevelTotal = reader.GetUInt16("totalLevel");
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
					character.ColorA = reader.GetUInt32("color1");
					character.ColorB = reader.GetUInt32("color2");
					character.ColorC = reader.GetUInt32("color3");
					character.DeletionTime = reader["deletionTime"] as DateTime? ?? DateTime.MinValue;
					character.State = (CreatureStates)reader.GetUInt32("status") & ~CreatureStates.SitDown;

					character.LoadDefault();
				}

				this.GetItems(character);
				this.GetKeywords(character);
				this.GetSkills(character);

				return character;
			}
			finally
			{
				conn.Close();
			}
		}

		private void GetKeywords(MabiPC character)
		{
			var conn = this.GetConnection();
			try
			{
				using (var reader = this.Query("SELECT keywordId FROM keywords WHERE characterId = " + character.Id.ToString(), conn))
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
			var conn = this.GetConnection();
			try
			{
				using (var reader = this.Query("SELECT skillId, rank, exp FROM skills WHERE characterId = " + character.Id.ToString(), conn))
				{
					while (reader.Read())
					{
						var skill = new MabiSkill(reader.GetUInt16("skillId"), reader.GetByte("rank"), character.Race);
						skill.Info.Experience = reader.GetInt32("exp");
						if (skill.IsRankable)
							skill.Info.Flag |= (ushort)SkillFlags.Rankable;
						character.Skills.Add(skill);
					}
				}
			}
			finally
			{
				conn.Close();

				if (!character.HasSkill(SkillConst.MeleeCombatMastery))
					character.Skills.Add(new MabiSkill(SkillConst.MeleeCombatMastery, SkillRank.RF, character.Race));
			}
		}

		/// <summary>
		/// Reads all items for the given character from the database, and adds them to it.
		/// </summary>
		/// <param name="character"></param>
		private void GetItems(MabiPC character)
		{
			var conn = this.GetConnection();
			try
			{
				using (var reader = this.Query("SELECT * FROM items WHERE characterId = " + character.Id.ToString() + " ORDER BY pocketId, pos_y, pos_x", conn))
				{
					while (reader.Read())
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

						character.Items.Add(item);
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
		/// Writes a full account to the database, incl. characters, cards, etc.
		/// </summary>
		/// <param name="account"></param>
		public void SaveAccount(MabiAccount account)
		{
			var conn = this.GetConnection();
			try
			{
				var mc = new MySqlCommand(
					"INSERT INTO `accounts`"
					+ " (`accountId`, `password`, `creation`) "
					+ " VALUES (@username, @password, @creation) "
					+ " ON DUPLICATE KEY UPDATE"
					+ " `password` = @password, `authority` = @authority, `creation` = @creation, `lastlogin` = @lastlogin, `lastip` = @lastip,"
					+ " `bannedreason` = @bannedreason, `bannedexpiration` = @bannedexpiration"
				, conn);

				mc.Parameters.AddWithValue("@username", account.Username);
				mc.Parameters.AddWithValue("@password", account.Userpass);
				mc.Parameters.AddWithValue("@authority", account.Authority);
				mc.Parameters.AddWithValue("@lastlogin", account.LastLogin);
				mc.Parameters.AddWithValue("@lastip", account.LastIp);
				mc.Parameters.AddWithValue("@bannedreason", account.BannedReason);
				mc.Parameters.AddWithValue("@bannedexpiration", account.BannedExpiration);
				mc.Parameters.AddWithValue("@creation", account.Creation);

				mc.ExecuteNonQuery();

				this.SaveCards(account);

				// Save characters
				foreach (var character in account.Characters)
				{
					if (!character.Save)
						continue;

					if (character.DeletionTime < DateTime.MaxValue)
						this.SaveCharacter(account, character);
					else
						this.QueryN("DELETE FROM characters WHERE characterId = " + character.Id.ToString(), conn);
				}

				foreach (var pet in account.Pets)
				{
					if (!pet.Save)
						continue;

					if (pet.DeletionTime < DateTime.MaxValue)
						this.SaveCharacter(account, pet);
					else
						this.QueryN("DELETE FROM characters WHERE characterId = " + pet.Id.ToString(), conn);
				}
			}
			finally
			{
				conn.Close();
			}
		}

		/// <summary>
		/// Writes all cards in the given account object to the database.
		/// </summary>
		/// <param name="account"></param>
		public void SaveCards(MabiAccount account)
		{
			var conn = this.GetConnection();
			try
			{
				this.QueryN("DELETE FROM character_cards WHERE accountId = '" + account.Username + "'", conn);
				this.QueryN("DELETE FROM pet_cards WHERE accountId = '" + account.Username + "'", conn);

				foreach (var card in account.CharacterCards)
				{
					this.QueryI(string.Format("INSERT INTO character_cards (accountId, type) VALUES('{0}', {1});", account.Username, card.Race), conn);
				}

				foreach (var card in account.PetCards)
				{
					this.QueryI(string.Format("INSERT INTO pet_cards (accountId, type) VALUES('{0}', {1});", account.Username, card.Race), conn);
				}
			}
			finally
			{
				conn.Close();
			}
		}

		public void AddCard(string table, string accountId, int cardId)
		{
			var conn = this.GetConnection();
			try
			{
				var mc = new MySqlCommand("INSERT INTO " + table + " (accountId, type) VALUES(@account, @card)", conn);
				mc.Parameters.AddWithValue("@account", accountId);
				mc.Parameters.AddWithValue("@card", cardId);
				mc.ExecuteNonQuery();
			}
			finally
			{
				conn.Close();
			}
		}

		public void SaveCharacter(MabiAccount account, MabiPC character)
		{
			var conn = this.GetConnection();
			try
			{
				if (character.Id <= 0x30000000000)
				{
					if (character is MabiCharacter)
						character.Id = this.GetNewCharacterId();
					else if (character is MabiPet)
						character.Id = (character.Id == 0 ? this.GetNewPetId() : this.GetNewPartnerId());
				}

				var characterLocation = character.GetPosition();

				var mc = new MySqlCommand(
					"INSERT INTO `characters`"
					+ " (`characterId`, `server`, `type`, `accountId`, `name`, `race`, `skinColor`, `eyeType`, `eyeColor`, `mouthType`,"
					+ " `status`, `height`, `fatness`, `upper`, `lower`, `region`, `x`, `y`, `direction`, `battleState`, `weaponSet`,"
					+ " `life`, `injuries`, `lifeMax`, `mana`, `manaMax`, `stamina`, `staminaMax`, `food`, `level`, `totalLevel`,"
					+ " `experience`, `age`, `strength`, `dexterity`, `intelligence`, `will`, `luck`, `abilityPoints`, `attackMin`, `attackMax`,"
					+ " `wattackMin`, `wattackMax`, `critical`, `protect`, `defense`, `rate`,"
					+ " `strBoost`, `dexBoost`, `intBoost`, `willBoost`, `luckBoost`,"
					+ " `color1`, `color2`, `color3`,"
					+ " `lastTown`, `lastDungeon`, `birthday`, `title`, `deletionTime`, `maxLevel`, `rebirthCount`, `jobId`) "

					+ " VALUES"
					+ " (@characterId, @server, @type, @accountId, @name, @race, @skinColor, @eyeType, @eyeColor, @mouthType,"
					+ " @status, @height, @fatness, @upper, @lower, @region, @x, @y, @direction, @battleState, @weaponSet,"
					+ " @life, @injuries, @lifeMax, @mana, @manaMax, @stamina, @staminaMax, @food, @level, @totalLevel,"
					+ " @experience, @age, @strength, @dexterity, @intelligence, @will, @luck, @abilityPoints, @attackMin, @attackMax,"
					+ " @wattackMin, @wattackMax, @critical, @protect, @defense, @rate,"
					+ " @strBoost, @dexBoost, @intBoost, @willBoost, @luckBoost,"
					+ " @color1, @color2, @color3,"
					+ " @lastTown, @lastDungeon, @birthday, @title, @deletionTime, @maxLevel, @rebirthCount, @jobId) "

					+ " ON DUPLICATE KEY UPDATE"
					+ " `region` = @region, `x` = @x, `y` = @y, `direction` = @direction, `weaponSet` = @weaponSet,"
					+ " `status` = @status, `deletionTime` = @deletionTime, `title` = @title,"
					+ " `life` = @life, `injuries` = @injuries, `lifeMax` = @lifeMax,"
					+ " `mana` = @mana, `manaMax` = @manaMax,"
					+ " `stamina` = @stamina, `staminaMax` = @staminaMax, `food` = @food,"
					+ " `level` = @level, `totalLevel` = @totalLevel, `experience` = @experience, `age` = @age, `race` = @race,"
					+ " `color1` = @color1, `color2` = @color2, `color3` = @color3,"
					+ " `strength` = @strength, `dexterity` = @dexterity, `intelligence` = @intelligence, `will` = @will, `luck` = @luck,"
					+ " `abilityPoints` = @abilityPoints"
				, conn);

				mc.Parameters.AddWithValue("@characterId", character.Id);
				mc.Parameters.AddWithValue("@server", character.Server);
				mc.Parameters.AddWithValue("@type", character.EntityType.ToString().ToUpper());
				mc.Parameters.AddWithValue("@accountId", account.Username);
				mc.Parameters.AddWithValue("@name", character.Name);
				mc.Parameters.AddWithValue("@race", character.Race);
				mc.Parameters.AddWithValue("@skinColor", character.SkinColor);
				mc.Parameters.AddWithValue("@eyeType", character.Eye);
				mc.Parameters.AddWithValue("@eyeColor", character.EyeColor);
				mc.Parameters.AddWithValue("@mouthType", character.Lip);
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
				mc.Parameters.AddWithValue("@attackMin", 0);
				mc.Parameters.AddWithValue("@attackMax", 0);
				mc.Parameters.AddWithValue("@wattackMin", 0);
				mc.Parameters.AddWithValue("@wattackMax", 0);
				mc.Parameters.AddWithValue("@critical", 0);
				mc.Parameters.AddWithValue("@protect", 0);
				mc.Parameters.AddWithValue("@defense", 0);
				mc.Parameters.AddWithValue("@rate", 0);
				//mc.Parameters.AddWithValue("@strBoost", character.StrMod);
				//mc.Parameters.AddWithValue("@dexBoost", character.DexMod);
				//mc.Parameters.AddWithValue("@intBoost", character.IntMod);
				//mc.Parameters.AddWithValue("@willBoost", character.WillMod);
				//mc.Parameters.AddWithValue("@luckBoost", character.LuckMod);
				mc.Parameters.AddWithValue("@strBoost", 0);
				mc.Parameters.AddWithValue("@dexBoost", 0);
				mc.Parameters.AddWithValue("@intBoost", 0);
				mc.Parameters.AddWithValue("@willBoost", 0);
				mc.Parameters.AddWithValue("@luckBoost", 0);
				mc.Parameters.AddWithValue("@lastTown", "");
				mc.Parameters.AddWithValue("@lastDungeon", "");
				mc.Parameters.AddWithValue("@birthday", DateTime.MinValue);
				mc.Parameters.AddWithValue("@title", character.Title);
				mc.Parameters.AddWithValue("@deletionTime", character.DeletionTime);
				mc.Parameters.AddWithValue("@maxLevel", 200);
				mc.Parameters.AddWithValue("@rebirthCount", character.RebirthCount);
				mc.Parameters.AddWithValue("@jobId", 0);
				mc.Parameters.AddWithValue("@color1", character.ColorA);
				mc.Parameters.AddWithValue("@color2", character.ColorB);
				mc.Parameters.AddWithValue("@color3", character.ColorC);

				mc.ExecuteNonQuery();

				this.SaveItems(character);
				this.SaveKeywords(character);
				this.SaveSkills(character);
			}
			finally
			{
				conn.Close();
			}
		}

		private void SaveKeywords(MabiPC character)
		{
			var conn = this.GetConnection();
			try
			{
				foreach (var keywordId in character.Keywords)
				{
					var mc = new MySqlCommand("REPLACE INTO keywords VALUES (@keywordId, @characterId)", conn);

					mc.Parameters.AddWithValue("@keywordId", keywordId);
					mc.Parameters.AddWithValue("@characterId", character.Id);

					mc.ExecuteNonQuery();
				}
			}
			finally
			{
				conn.Close();
			}
		}

		private void SaveSkills(MabiPC character)
		{
			var conn = this.GetConnection();
			try
			{
				foreach (var skill in character.Skills)
				{
					var mc = new MySqlCommand("REPLACE INTO skills VALUES (@skillId, @characterId, @rank, @exp)", conn);

					mc.Parameters.AddWithValue("@skillId", skill.Info.Id);
					mc.Parameters.AddWithValue("@characterId", character.Id);
					mc.Parameters.AddWithValue("@rank", skill.Info.Rank);
					mc.Parameters.AddWithValue("@exp", skill.Info.Experience);

					mc.ExecuteNonQuery();
				}
			}
			finally
			{
				conn.Close();
			}
		}

		private void SaveItems(MabiPC character)
		{
			var conn = this.GetConnection();
			MySqlTransaction transaction = null;

			try
			{
				transaction = conn.BeginTransaction();

				var delmc = new MySqlCommand("DELETE FROM items WHERE characterId = @id", conn);
				delmc.Transaction = transaction;
				delmc.Parameters.AddWithValue("@id", character.Id);
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

				foreach (var item in character.Items)
				{
					// If item has a temporary Id
					if (item.Id >= Id.TmpItems)
					{
						item.Id = this.GetNewItemId();
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

					mc.ExecuteNonQuery();
				}

				transaction.Commit();
			}
			catch (Exception ex)
			{
				transaction.Rollback();
				throw ex;
			}
			finally
			{
				conn.Close();
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
			var conn = this.GetConnection();
			try
			{
				this.QueryN(string.Format(
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
			var conn = this.GetConnection();
			try
			{
				var serverList = new List<MabiServers>();

				using (var reader = this.Query("SELECT * FROM channels ORDER BY name ASC", conn))
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
