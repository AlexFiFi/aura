// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.Collections.Generic;
using Aura.Shared.Database;
using Aura.Msgr.Chat;
using MySql.Data.MySqlClient;

namespace Aura.Msgr.Database
{
	public static class MsgrDb
	{
		public static Contact GetContactOrCreate(ulong characterId, string name, string server)
		{
			var conn = MabiDb.Instance.GetConnection();
			try
			{
				var mc = new MySqlCommand(
					"SELECT co.contactId " +
					"FROM contacts AS co " +
					"INNER JOIN characters AS ch ON co.characterId = ch.characterId " +
					"WHERE ch.characterId = @characterId AND ch.name = @name AND ch.server = @server"
				, conn);
				mc.Parameters.AddWithValue("@characterId", characterId);
				mc.Parameters.AddWithValue("@name", name);
				mc.Parameters.AddWithValue("@server", server);

				var contact = new Contact();
				contact.Id = 0;
				contact.Name = name;
				contact.Server = server;
				contact.FullName = name + "@" + server;

				using (var reader = mc.ExecuteReader())
				{
					if (reader.Read())
					{
						contact.Id = reader.GetUInt32("contactId");

						// load groups, friends, notes

						contact.Notes = GetNotes(contact.Id);
						return contact;
					}
				}

				// Create new contact
				mc = new MySqlCommand("INSERT INTO contacts (characterId) VALUES (@characterId)", conn);
				mc.Parameters.AddWithValue("@characterId", characterId);
				mc.ExecuteNonQuery();
				contact.Id = (uint)mc.LastInsertedId;

				return contact;
			}
			finally
			{
				conn.Close();
			}
		}

		public static uint GetContactId(string fullName)
		{

			var conn = MabiDb.Instance.GetConnection();
			try
			{
				var mc = new MySqlCommand(
					"SELECT co.contactId " +
					"FROM contacts AS co " +
					"INNER JOIN characters AS ch ON co.characterId = ch.characterId " +
					"WHERE CONCAT(ch.name, '@', ch.server) = @fullName"
				, conn);
				mc.Parameters.AddWithValue("@fullName", fullName);

				using (var reader = mc.ExecuteReader())
				{
					if (!reader.Read())
						return 0;

					return reader.GetUInt32("contactId");
				}
			}
			finally
			{
				conn.Close();
			}
		}

		public static List<Note> GetNotes(ulong contactId)
		{
			var conn = MabiDb.Instance.GetConnection();
			try
			{
				var mc = new MySqlCommand("SELECT * FROM notes WHERE contactId = @contactId", conn);
				mc.Parameters.AddWithValue("@contactId", contactId);

				using (var reader = mc.ExecuteReader())
				{
					var result = new List<Note>();

					while (reader.Read())
					{
						var note = new Note();
						note.Id = reader.GetUInt32("noteId");
						note.Sender = reader.GetString("from");
						note.Message = reader.GetString("msg");
						note.Time = reader.GetDateTime("time");
						note.Read = reader.GetBoolean("read");
						result.Add(note);
					}

					return result;
				}
			}
			finally
			{
				conn.Close();
			}
		}

		public static ulong AddNote(Note note)
		{
			var conn = MabiDb.Instance.GetConnection();
			try
			{
				var mc = new MySqlCommand(
					"INSERT INTO notes (`contactId`, `from`, `msg`, `time`, `read`) " +
					"VALUES (@contactId, @from, @msg, @time, @read)"
				, conn);
				mc.Parameters.AddWithValue("@contactId", note.ContactId);
				mc.Parameters.AddWithValue("@from", note.Sender);
				mc.Parameters.AddWithValue("@msg", note.Message);
				mc.Parameters.AddWithValue("@time", note.Time);
				mc.Parameters.AddWithValue("@read", note.Read);
				mc.ExecuteNonQuery();

				return (ulong)mc.LastInsertedId;
			}
			finally
			{
				conn.Close();
			}
		}
	}
}
