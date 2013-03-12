// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using Aura.Data;
using Aura.Shared.Database;
using Aura.Shared.Util;
using MySql.Data.MySqlClient;

namespace Aura.Login.Database
{
	public class LoginDb
	{
		public static readonly LoginDb Instance = new LoginDb();
		static LoginDb() { }
		private LoginDb() { }

		// Account management
		// ------------------------------------------------------------------

		/// <summary>
		/// Creates a new account using the given name and password.
		/// Password is encrypted using BCrypt.
		/// </summary>
		/// <param name="accountName"></param>
		/// <param name="password"></param>
		public void CreateAccount(string accountName, string password)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				var mc = new MySqlCommand("INSERT INTO `accounts` (`accountId`, `password`, `creation`) VALUES (@id, @password, @creation)", conn);
				mc.Parameters.AddWithValue("@id", accountName);
				mc.Parameters.AddWithValue("@password", BCrypt.HashPassword(password, BCrypt.GenerateSalt(12)));
				mc.Parameters.AddWithValue("@creation", DateTime.Now);
				mc.ExecuteNonQuery();
			}
		}

		/// <summary>
		/// Updates lastLogin, lastIp, and login status.
		/// </summary>
		/// <param name="account"></param>
		public void UpdateAccount(Account account)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				var mc = new MySqlCommand("UPDATE `accounts` SET `lastlogin` = @lastlogin, `lastip` = @lastip, `loggedIn` = @loggedIn WHERE `accountId` = @id", conn);
				mc.Parameters.AddWithValue("@id", account.Name);
				mc.Parameters.AddWithValue("@lastlogin", account.LastLogin);
				mc.Parameters.AddWithValue("@lastip", account.LastIp);
				mc.Parameters.AddWithValue("@loggedIn", account.LoggedIn);
				mc.ExecuteNonQuery();
			}
		}

		/// <summary>
		/// Returns account with the given id.
		/// </summary>
		/// <param name="accountName"></param>
		/// <returns></returns>
		public Account GetAccount(string accountName)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				var mc = new MySqlCommand("SELECT * FROM `accounts` WHERE `accountId` = @id", conn);
				mc.Parameters.AddWithValue("@id", accountName);

				using (var reader = mc.ExecuteReader())
				{
					if (!reader.Read())
						return null;

					return new Account()
					{
						Name = accountName,
						Password = reader.GetString("password"),
						Authority = reader.GetByte("authority"),
						Creation = reader["creation"] as DateTime? ?? DateTime.MinValue,
						LastIp = reader.GetString("lastip"),
						LastLogin = reader["lastlogin"] as DateTime? ?? DateTime.MinValue,
						BannedReason = reader.GetString("bannedreason"),
						BannedExpiration = reader["bannedexpiration"] as DateTime? ?? DateTime.MinValue,
					};
				}
			}
		}

		/// <summary>
		/// Creates a sessions for the given account, and returns the new session Id.
		/// </summary>
		/// <param name="accountName"></param>
		/// <returns>Returns new session Id.</returns>
		public ulong CreateSession(string accountName)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				var sessionKey = (ulong)RandomProvider.Get().Next(1, int.MaxValue);

				var mc = new MySqlCommand("UPDATE `accounts` SET `session` = @session WHERE `accountId` = @id", conn);
				mc.Parameters.AddWithValue("@id", accountName);
				mc.Parameters.AddWithValue("@session", sessionKey);
				mc.ExecuteNonQuery();

				return sessionKey;
			}
		}

		// Cards
		// ------------------------------------------------------------------

		/// <summary>
		/// Returns all gifted cards present for this account.
		/// </summary>
		/// <param name="accountName"></param>
		/// <returns></returns>
		public List<Gift> GetGifts(string accountName)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				var mc = new MySqlCommand("SELECT * FROM `cards` WHERE `accountId` = @id AND `isGift`", conn);
				mc.Parameters.AddWithValue("@id", accountName);

				var result = new List<Gift>();
				using (var reader = mc.ExecuteReader())
				{
					while (reader.Read())
					{
						var gift = new Gift();
						gift.Id = reader.GetUInt32("cardId");
						gift.Type = reader.GetUInt32("type");
						gift.Race = reader.GetUInt32("race");
						gift.Message = reader.GetStringSafe("message");
						gift.Sender = reader.GetStringSafe("sender");
						gift.SenderServer = reader.GetStringSafe("senderServer");
						gift.Receiver = reader.GetStringSafe("receiver");
						gift.ReceiverServer = reader.GetStringSafe("receiverServer");
						gift.Added = reader["added"] as DateTime? ?? DateTime.Now;

						result.Add(gift);
					}
				}

				return result;
			}
		}

		/// <summary>
		/// Deletes the card with the given id on the given account.
		/// </summary>
		/// <param name="accountName"></param>
		/// <param name="table"></param>
		/// <param name="cardId"></param>
		public void DeleteCard(string accountName, ulong cardId)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				var mc = new MySqlCommand("DELETE FROM `cards` WHERE `accountId` = @accountId AND cardId = @cardId", conn);
				mc.Parameters.AddWithValue("@accountId", accountName);
				mc.Parameters.AddWithValue("@cardId", cardId);
				mc.ExecuteNonQuery();
			}
		}

		/// <summary>
		/// Changes values of the (gift) card in the database, to make it a regular card.
		/// </summary>
		/// <param name="cardId"></param>
		public void ChangeGiftToCard(ulong cardId)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				var mc = new MySqlCommand("UPDATE `cards` SET `isGift` = FALSE WHERE cardId = @cardId", conn);
				mc.Parameters.AddWithValue("@cardId", cardId);
				mc.ExecuteNonQuery();
			}
		}

		// Get Characters/Pets
		// ------------------------------------------------------------------

		/// <summary>
		/// Returns a list of all characters on this account.
		/// </summary>
		/// <param name="accountName"></param>
		/// <returns></returns>
		public List<Character> GetCharacters(string accountName)
		{ return this.GetCharacters(accountName, "CHARACTER"); }

		/// <summary>
		/// Returns a list of all pets/partners on this account.
		/// </summary>
		/// <param name="accountName"></param>
		/// <returns></returns>
		public List<Character> GetPets(string accountName)
		{ return this.GetCharacters(accountName, "PET"); }

		private List<Character> GetCharacters(string accountName, string type)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				var mc = new MySqlCommand("SELECT * FROM `characters` WHERE `accountId` = @id AND type = @type", conn);
				mc.Parameters.AddWithValue("@id", accountName);
				mc.Parameters.AddWithValue("@type", type);

				var result = new List<Character>();
				using (var reader = mc.ExecuteReader())
				{
					while (reader.Read())
					{
						var character = new Character(type == "CHARACTER" ? CharacterType.Character : CharacterType.Pet);
						character.Id = reader.GetUInt64("characterId");
						character.Name = reader.GetString("name");
						character.Server = reader.GetString("server");
						character.Race = reader.GetUInt32("race");
						character.DeletionTime = reader["deletionTime"] as DateTime? ?? DateTime.MinValue;
						character.SkinColor = reader.GetByte("skinColor");
						character.Eye = reader.GetByte("eyeType");
						character.EyeColor = reader.GetByte("eyeColor");
						character.Mouth = reader.GetByte("mouthType");
						character.Height = reader.GetFloat("height");
						character.Weight = reader.GetFloat("fatness");
						character.Upper = reader.GetFloat("upper");
						character.Lower = reader.GetUInt32("lower");
						character.Color1 = reader.GetUInt32("color1");
						character.Color2 = reader.GetUInt32("color2");
						character.Color3 = reader.GetUInt32("color3");
						result.Add(character);
					}
				}

				return result;
			}
		}

		/// <summary>
		/// Adds the given character to the database, with a new id.
		/// </summary>
		/// <param name="accountName"></param>
		/// <param name="character"></param>
		/// <returns></returns>
		public ulong CreateCharacter(string accountName, Character character)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				if (character.Type == CharacterType.Character)
					character.Id = MabiDb.Instance.GetNewCharacterId();
				else if (character.Type == CharacterType.Pet)
					character.Id = MabiDb.Instance.GetNewPetId();
				else if (character.Type == CharacterType.Partner)
				{
					character.Id = MabiDb.Instance.GetNewPartnerId();
					character.Type = CharacterType.Pet;
				}

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
				, conn);

				mc.Parameters.AddWithValue("@characterId", character.Id);
				mc.Parameters.AddWithValue("@server", character.Server);
				mc.Parameters.AddWithValue("@type", character.Type.ToString().ToUpper());
				mc.Parameters.AddWithValue("@accountId", accountName);
				mc.Parameters.AddWithValue("@name", character.Name);
				mc.Parameters.AddWithValue("@race", character.Race);
				mc.Parameters.AddWithValue("@skinColor", character.SkinColor);
				mc.Parameters.AddWithValue("@eyeType", character.Eye);
				mc.Parameters.AddWithValue("@eyeColor", character.EyeColor);
				mc.Parameters.AddWithValue("@mouthType", character.Mouth);
				mc.Parameters.AddWithValue("@status", 0);

				mc.Parameters.AddWithValue("@height", character.Height);
				mc.Parameters.AddWithValue("@fatness", character.Weight);
				mc.Parameters.AddWithValue("@upper", character.Upper);
				mc.Parameters.AddWithValue("@lower", character.Lower);

				mc.Parameters.AddWithValue("@region", character.Region);
				mc.Parameters.AddWithValue("@x", character.X);
				mc.Parameters.AddWithValue("@y", character.Y);

				mc.Parameters.AddWithValue("@direction", 0);
				mc.Parameters.AddWithValue("@battleState", 0);
				mc.Parameters.AddWithValue("@weaponSet", 0);

				ushort ap = 0;
				float life = 10, mana = 10, stamina = 10;
				float str = 10, int_ = 10, dex = 10, will = 10, luck = 10;
				if (character.Type == CharacterType.Character)
				{
					var ageInfo = MabiData.StatsBaseDb.Find(character.Race, character.Age);
					if (ageInfo == null)
						Logger.Warning("Unable to find age info for race '{0}', age '{1}'.", character.Race, character.Age);
					else
					{
						life = ageInfo.Life;
						mana = ageInfo.Mana;
						stamina = ageInfo.Stamina;
						str = ageInfo.Str;
						int_ = ageInfo.Int;
						dex = ageInfo.Dex;
						will = ageInfo.Will;
						luck = ageInfo.Luck;
						ap = ageInfo.AP;
					}
				}

				mc.Parameters.AddWithValue("@life", life);
				mc.Parameters.AddWithValue("@injuries", 0);
				mc.Parameters.AddWithValue("@lifeMax", life);
				mc.Parameters.AddWithValue("@mana", mana);
				mc.Parameters.AddWithValue("@manaMax", mana);
				mc.Parameters.AddWithValue("@stamina", stamina);
				mc.Parameters.AddWithValue("@staminaMax", stamina);
				mc.Parameters.AddWithValue("@food", 0);
				mc.Parameters.AddWithValue("@level", 1);
				mc.Parameters.AddWithValue("@totalLevel", 1);
				mc.Parameters.AddWithValue("@experience", 0);
				mc.Parameters.AddWithValue("@age", character.Age);
				mc.Parameters.AddWithValue("@strength", str);
				mc.Parameters.AddWithValue("@dexterity", dex);
				mc.Parameters.AddWithValue("@intelligence", int_);
				mc.Parameters.AddWithValue("@will", will);
				mc.Parameters.AddWithValue("@luck", luck);
				mc.Parameters.AddWithValue("@abilityPoints", ap);
				mc.Parameters.AddWithValue("@attackMin", 0);
				mc.Parameters.AddWithValue("@attackMax", 0);
				mc.Parameters.AddWithValue("@wattackMin", 0);
				mc.Parameters.AddWithValue("@wattackMax", 0);
				mc.Parameters.AddWithValue("@critical", 0);
				mc.Parameters.AddWithValue("@protect", 0);
				mc.Parameters.AddWithValue("@defense", 0);
				mc.Parameters.AddWithValue("@rate", 0);
				mc.Parameters.AddWithValue("@strBoost", 0);
				mc.Parameters.AddWithValue("@dexBoost", 0);
				mc.Parameters.AddWithValue("@intBoost", 0);
				mc.Parameters.AddWithValue("@willBoost", 0);
				mc.Parameters.AddWithValue("@luckBoost", 0);
				mc.Parameters.AddWithValue("@lastTown", "");
				mc.Parameters.AddWithValue("@lastDungeon", "");
				mc.Parameters.AddWithValue("@birthday", DateTime.MinValue);
				mc.Parameters.AddWithValue("@title", 0);
				mc.Parameters.AddWithValue("@deletionTime", DateTime.MinValue);
				mc.Parameters.AddWithValue("@maxLevel", 200);
				mc.Parameters.AddWithValue("@rebirthCount", 0);
				mc.Parameters.AddWithValue("@jobId", 0);
				mc.Parameters.AddWithValue("@color1", character.Color1);
				mc.Parameters.AddWithValue("@color2", character.Color2);
				mc.Parameters.AddWithValue("@color3", character.Color3);

				mc.ExecuteNonQuery();

				return character.Id;
			}
		}

		/// <summary>
		/// Updates deletionTime or deletes character, depending on DeletionFlag.
		/// </summary>
		/// <param name="character"></param>
		public void SetDelete(Character character)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				if (character.DeletionFlag == DeletionFlag.Delete)
				{
					var mc = new MySqlCommand("DELETE FROM `characters` WHERE `characterId` = @characterId", conn);
					mc.Parameters.AddWithValue("@characterId", character.Id);
					mc.ExecuteNonQuery();
				}
				else
				{
					var mc = new MySqlCommand("UPDATE `characters` SET `deletionTime` = @time WHERE `characterId` = @characterId", conn);
					mc.Parameters.AddWithValue("@characterId", character.Id);
					mc.Parameters.AddWithValue("@time", character.DeletionTime);
					mc.ExecuteNonQuery();
				}
			}
		}

		// Items
		// ------------------------------------------------------------------

		/// <summary>
		/// Returns all visible items of the character.
		/// </summary>
		/// <param name="characterId"></param>
		/// <returns></returns>
		public List<Item> GetEquipment(ulong characterId)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				// TODO: Filter visible items.
				var mc = new MySqlCommand("SELECT * FROM `items` WHERE `characterId` = @characterId", conn);
				mc.Parameters.AddWithValue("@characterId", characterId);

				using (var reader = mc.ExecuteReader())
				{
					var result = new List<Item>();
					while (reader.Read())
					{
						var item = new Item();
						item.Id = reader.GetUInt64("itemID");
						item.Info.Pocket = reader.GetByte("pocketId");
						item.Info.Class = reader.GetUInt32("class");
						item.Info.ColorA = reader.GetUInt32("color_01");
						item.Info.ColorB = reader.GetUInt32("color_02");
						item.Info.ColorC = reader.GetUInt32("color_03");
						item.Info.Amount = reader.GetUInt16("bundle");
						item.Info.X = reader.GetUInt32("pos_x");
						item.Info.Y = reader.GetUInt32("pos_y");
						item.Info.FigureA = (byte)reader.GetUInt32("figure");

						result.Add(item);
					}
					return result;
				}
			}
		}

		/// <summary>
		/// Adds the given items to the database for the character.
		/// </summary>
		/// <param name="characterId"></param>
		/// <param name="cardItems"></param>
		public void AddItems(ulong characterId, List<CharCardSetInfo> cardItems)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				var mc = new MySqlCommand(
					"INSERT INTO `items`"
					+ " (`characterId`, `itemID`, `class`, `pocketId`, `pos_x`, `pos_y`, `varint`, `color_01`, `color_02`, `color_03`, `price`, `bundle`,"
					+ " `linked_pocket`, `figure`, `flag`, `durability`, `durability_max`, `origin_durability_max`, `attack_min`, `attack_max`,"
					+ " `wattack_min`, `wattack_max`, `balance`, `critical`, `defence`, `protect`, `effective_range`, `attack_speed`,"
					+ " `experience`, `exp_point`, `upgraded`, `upgraded_max`, `grade`, `prefix`, `suffix`, `data`, `option`, `sellingprice`, `expiration`, `update_time`, `tags`)"

					+ " VALUES (@characterId, @itemID, @class, @pocketId, @pos_x, @pos_y, @varint, @color_01, @color_02, @color_03, @price, @bundle,"
					+ " @linked_pocket, @figure, @flag, @durability, @durability_max, @origin_durability_max, @attack_min, @attack_max,"
					+ " @wattack_min, @wattack_max, @balance, @critical, @defence, @protect, @effective_range, @attack_speed,"
					+ " @experience, @exp_point, @upgraded, @upgraded_max, @grade, @prefix, @suffix, @data, @option, @sellingprice, @expiration, @update_time, '')"
				, conn);

				foreach (var item in cardItems)
				{
					var dataInfo = MabiData.ItemDb.Find(item.Class);
					if (dataInfo == null)
					{
						Logger.Warning("Item '{0}' couldn't be found in the database.", item.Class);
						continue;
					}

					mc.Parameters.Clear();
					mc.Parameters.AddWithValue("@characterId", characterId);
					mc.Parameters.AddWithValue("@itemID", MabiDb.Instance.GetNewItemId());
					mc.Parameters.AddWithValue("@class", item.Class);
					mc.Parameters.AddWithValue("@pocketId", item.Pocket);
					mc.Parameters.AddWithValue("@pos_x", 0);
					mc.Parameters.AddWithValue("@pos_y", 0);
					mc.Parameters.AddWithValue("@varint", 0); // ???
					mc.Parameters.AddWithValue("@color_01", item.Color1);
					mc.Parameters.AddWithValue("@color_02", item.Color2);
					mc.Parameters.AddWithValue("@color_03", item.Color3);
					mc.Parameters.AddWithValue("@price", dataInfo.Price);
					mc.Parameters.AddWithValue("@bundle", 1);
					mc.Parameters.AddWithValue("@linked_pocket", 0);
					mc.Parameters.AddWithValue("@figure", 0);
					mc.Parameters.AddWithValue("@flag", 1);
					mc.Parameters.AddWithValue("@durability", dataInfo.Durability);
					mc.Parameters.AddWithValue("@durability_max", dataInfo.Durability);
					mc.Parameters.AddWithValue("@origin_durability_max", dataInfo.Durability);
					mc.Parameters.AddWithValue("@attack_min", dataInfo.AttackMin);
					mc.Parameters.AddWithValue("@attack_max", dataInfo.AttackMax);
					mc.Parameters.AddWithValue("@wattack_min", 0);
					mc.Parameters.AddWithValue("@wattack_max", 0);
					mc.Parameters.AddWithValue("@balance", dataInfo.Balance);
					mc.Parameters.AddWithValue("@critical", dataInfo.Critical);
					mc.Parameters.AddWithValue("@defence", dataInfo.Defense);
					mc.Parameters.AddWithValue("@protect", dataInfo.Protection);
					mc.Parameters.AddWithValue("@effective_range", 0);
					mc.Parameters.AddWithValue("@attack_speed", dataInfo.AttackSpeed);
					mc.Parameters.AddWithValue("@experience", 0);
					mc.Parameters.AddWithValue("@exp_point", 0); // ?
					mc.Parameters.AddWithValue("@upgraded", 0);
					mc.Parameters.AddWithValue("@upgraded_max", 0);
					mc.Parameters.AddWithValue("@grade", 0);
					mc.Parameters.AddWithValue("@prefix", 0);
					mc.Parameters.AddWithValue("@suffix", 0);
					mc.Parameters.AddWithValue("@data", "");
					mc.Parameters.AddWithValue("@option", "");
					mc.Parameters.AddWithValue("@sellingprice", dataInfo.SellingPrice);
					mc.Parameters.AddWithValue("@expiration", 0);
					mc.Parameters.AddWithValue("@update_time", DateTime.Now);
					mc.ExecuteNonQuery();
				}
			}
		}

		// Keywords
		// ------------------------------------------------------------------

		/// <summary>
		/// Adds the given keywords to the database for the character.
		/// </summary>
		/// <param name="characterId"></param>
		/// <param name="keywordIds"></param>
		public void AddKeywords(ulong characterId, params ushort[] keywordIds)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				var mc = new MySqlCommand("INSERT INTO keywords (`keywordId`, `characterId`) VALUES (@keywordId, @characterId)", conn);

				foreach (var keywordId in keywordIds)
				{
					mc.Parameters.Clear();
					mc.Parameters.AddWithValue("@characterId", characterId);
					mc.Parameters.AddWithValue("@keywordId", keywordId);
					mc.ExecuteNonQuery();
				}
			}
		}

		// Skills
		// ------------------------------------------------------------------

		/// <summary>
		/// Adds the given skills to the database for the character.
		/// </summary>
		/// <param name="characterId"></param>
		/// <param name="skills"></param>
		public void AddSkills(ulong characterId, params Skill[] skills)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				var mc = new MySqlCommand("INSERT INTO skills VALUES (@skillId, @characterId, @rank, 0)", conn);

				foreach (var skill in skills)
				{
					mc.Parameters.Clear();
					mc.Parameters.AddWithValue("@characterId", characterId);
					mc.Parameters.AddWithValue("@skillId", skill.Id);
					mc.Parameters.AddWithValue("@rank", skill.Rank);
					mc.ExecuteNonQuery();
				}
			}
		}
	}
}
