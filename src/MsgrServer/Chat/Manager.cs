// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using Aura.Shared.Network;
using Aura.Msgr.Database;

namespace Aura.Msgr.Chat
{
	public class Manager
	{
		public readonly static Manager Instance = new Manager();

		private Manager()
		{ }

		private Dictionary<string, Contact> _contacts = new Dictionary<string, Contact>();

		public void AddContact(Contact contact)
		{
			lock (_contacts)
				_contacts.Add(contact.FullName, contact);

			// Check friends, send online
		}

		public void RemoveContact(Contact contact)
		{
			lock (_contacts)
				_contacts.Remove(contact.FullName);

			// Check friends, send offline
		}

		public bool IsOnline(string fullName)
		{
			lock (_contacts)
				return _contacts.ContainsKey(fullName);
		}

		/// <summary>
		/// Returns contact or null if not online.
		/// </summary>
		/// <param name="fullName"></param>
		/// <returns></returns>
		public Contact GetContact(string fullName)
		{
			lock (_contacts)
			{
				Contact result;
				_contacts.TryGetValue(fullName, out result);
				return result;
			}
		}

		// ------------------------------------------------------------------

		public uint GetContactId(string fullName)
		{
			uint result = 0;
			var contact = GetContact(fullName);
			if (contact == null)
			{
				result = MsgrDb.GetContactId(fullName);
			}
			else
				result = contact.Id;

			return result;
		}

		public bool SendNote(string from, string to, string msg)
		{
			var contactId = this.GetContactId(to);
			if (contactId < 1)
				return false;

			var note = new Note();
			note.ContactId = contactId;
			note.Sender = from;
			note.Message = msg;
			note.Time = DateTime.Now;
			note.Id = MsgrDb.AddNote(note);
			if (note.Id < 1)
				return false;

			var contact = this.GetContact(to);
			if (contact == null)
				return true;

			contact.Notes.Add(note);

			// Send notification (goes into HandleRefresh later)
			contact.Client.Send(
				new MabiPacket(Op.Msgr.YouGotNote)
				.PutLong(note.Id)
				.PutString(contact.Name)
				.PutString(contact.Server)
			);

			return true;
		}
	}
}
