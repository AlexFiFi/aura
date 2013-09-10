// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.Text.RegularExpressions;
using Aura.Shared.Const;
using MySql.Data.MySqlClient;

namespace Aura.Shared.Database
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
		/// Reads the id for the given name and increments it by 1.
		/// If the type doesn't exists, it's created using the default value.
		/// </summary>
		/// <param name="type">Identificator (Example: "item")</param>
		/// <param name="def">Default value</param>
		/// <returns>Returns new usable Id.</returns>
		public ulong GetNewPoolId(string type, ulong def)
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
		public ulong GetNewCharacterId()
		{
			return this.GetNewPoolId("characters", Id.Characters);
		}

		/// <summary>
		/// Returns a new usable pet Id.
		/// </summary>
		/// <returns></returns>
		public ulong GetNewPetId()
		{
			return this.GetNewPoolId("pets", Id.Pets);
		}

		/// <summary>
		/// Returns a new usable partner Id.
		/// </summary>
		/// <returns></returns>
		public ulong GetNewPartnerId()
		{
			return this.GetNewPoolId("partners", Id.Partners);
		}

		/// <summary>
		/// Returns a new usable item Id.
		/// </summary>
		/// <returns></returns>
		public ulong GetNewItemId()
		{
			return this.GetNewPoolId("items", Id.Items);
		}

		/// <summary>
		/// Checks if a session for the given account name and key exists.
		/// </summary>
		/// <param name="accName"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool IsSessionKey(string accName, ulong key)
		{
			using (var conn = this.GetConnection())
			{
				var mc = new MySqlCommand("SELECT session FROM accounts WHERE accountId = @id AND session = @session", conn);
				mc.Parameters.AddWithValue("@id", accName);
				mc.Parameters.AddWithValue("@session", key);

				using (var reader = mc.ExecuteReader())
					return reader.HasRows;
			}
		}

		/// <summary>
		/// Checks if an account with the given name exists.
		/// </summary>
		/// <param name="accName"></param>
		/// <returns></returns>
		public bool AccountExists(string accountName)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				var mc = new MySqlCommand("SELECT * FROM `accounts` WHERE `accountId` = @accountId", conn);
				mc.Parameters.AddWithValue("@accountId", accountName);

				using (var reader = mc.ExecuteReader())
					return reader.HasRows;
			}
		}

		public bool AccountHasCharacter(string accountName, string charName)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				var mc = new MySqlCommand("SELECT `characterId` FROM `characters` WHERE `accountId` = @accountId AND `name` = @name", conn);
				mc.Parameters.AddWithValue("@accountId", accountName);
				mc.Parameters.AddWithValue("@name", charName);

				using (var reader = mc.ExecuteReader())
					return reader.HasRows;
			}
		}

		/// <summary>
		/// Sets the  authority for the given account.
		/// </summary>
		/// <param name="accountName"></param>
		/// <param name="level"></param>
		/// <returns></returns>
		public bool SetAuthority(string accountName, byte level)
		{
			using (var conn = this.GetConnection())
			{
				var mc = new MySqlCommand("UPDATE `accounts` SET `authority` = @auth WHERE `accountId` = @accountId", conn);
				mc.Parameters.AddWithValue("@accountId", accountName);
				mc.Parameters.AddWithValue("@auth", level);
				return (mc.ExecuteNonQuery() > 0);
			}
		}

		/// <summary>
		/// Checks if the name pattern is okay, and if a character (or pet)
		/// with the given name exists.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public NameCheckResult NameOkay(string name, string server)
		{
			// 3-15 alphanumeric characters.
			if (!(new Regex(@"^[a-zA-Z0-9]{3,15}$")).IsMatch(name))
				return NameCheckResult.Invalid;

			using (var conn = MabiDb.Instance.GetConnection())
			{
				var mc = new MySqlCommand("SELECT `characterId` FROM `characters` WHERE `server` = @server AND `name` = @name", conn);
				mc.Parameters.AddWithValue("@server", server);
				mc.Parameters.AddWithValue("@name", name);

				using (var reader = mc.ExecuteReader())
				{
					if (reader.HasRows)
						return NameCheckResult.Exists;
				}
			}

			return NameCheckResult.Okay;
		}

		/// <summary>
		/// Adds a new card with the given type and race.
		/// Returns new card id.
		/// </summary>
		/// <param name="accountName"></param>
		/// <param name="type"></param>
		/// <param name="race"></param>
		/// <returns></returns>
		public ulong AddCard(string accountName, uint type, uint race)
		{
			using (var conn = MabiDb.Instance.GetConnection())
			{
				var mc = new MySqlCommand("INSERT INTO `cards` (accountId, type, race) VALUES(@id, @type, @race)", conn);
				mc.Parameters.AddWithValue("@id", accountName);
				mc.Parameters.AddWithValue("@type", type);
				mc.Parameters.AddWithValue("@race", race);
				mc.ExecuteNonQuery();

				return (ulong)mc.LastInsertedId;
			}
		}
	}

	public static class MySqlDataReaderExtension
	{
		public static bool IsDBNull(this MySqlDataReader reader, string index)
		{
			return reader.IsDBNull(reader.GetOrdinal(index));
		}

		/// <summary>
		/// Same as GetString, except for a is null check. Returns null if NULL.
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public static string GetStringSafe(this MySqlDataReader reader, string index)
		{
			if (IsDBNull(reader, index))
				return null;
			else
				return reader.GetString(index);
		}
	}

	public enum NameCheckResult : byte
	{
		Okay = 0,
		Exists = 1,
		Invalid = 2,
	}
}
