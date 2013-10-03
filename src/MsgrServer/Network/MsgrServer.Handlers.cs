// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Aura.Shared.Database;
using Aura.Shared.Network;
using Aura.Shared.Util;
using Aura.Msgr.Chat;
using Aura.Msgr.Database;

namespace Aura.Msgr.Network
{
	public partial class MsgrServer : BaseServer<MsgrClient>
	{
		protected override void OnServerStartUp()
		{
			this.RegisterPacketHandler(Op.Msgr.Login, HandleLogin);

			this.RegisterPacketHandler(Op.Msgr.FriendList, HandleFriendList);

			this.RegisterPacketHandler(Op.Msgr.NoteList, HandleNoteList);
			this.RegisterPacketHandler(Op.Msgr.SendNote, HandleNoteSend);

			this.RegisterPacketHandler(Op.Msgr.Refresh, HandleRefresh);
		}

		public void HandleLogin(MsgrClient client, MabiPacket packet)
		{
			if (client.State != ClientState.LoggingIn)
				return;

			var unk1 = packet.GetString(); // "-1"
			var charid = packet.GetLong();
			var charName = packet.GetString();
			var server = packet.GetString();
			var channel = packet.GetString();
			var accountName = packet.GetString();
			var sessionKey = packet.GetLong();
			var unk2 = packet.GetLong(); // 2xxxxxxxxxxxxxxx11
			var unk3 = packet.GetInt(); // 0

			var result = LoginResult.Okay;

			// Check session
			if (!MabiDb.Instance.IsSessionKey(accountName, sessionKey))
			{
				Logger.Warning("Invalid session key.");
				client.Kill();
				return;
			}

			// Check character
			if (!MabiDb.Instance.AccountHasCharacter(accountName, charName))
			{
				Logger.Warning("Account '" + accountName + "' doesn't have character '" + charName + "'.");
				client.Kill();
				return;
			}

			// Check contact
			client.Contact = MsgrDb.GetContactOrCreate(charid, charName, server);
			if (client.Contact != null)
			{
				client.Contact.Client = client;
				client.State = ClientState.LoggedIn;
				Manager.Instance.AddContact(client.Contact);
			}
			else
			{
				result = LoginResult.Fail;
			}

			var p = new MabiPacket(Op.Msgr.LoginR);
			p.PutInt((uint)result);
			if (result == LoginResult.Okay)
			{
				p.PutInt(client.Contact.Id);
				p.PutString(client.Contact.FullName);
				p.PutString("");      //?
				p.PutInt(0x80000000); //?
				p.PutByte(0x10);      //?
			}
			client.Send(p);
		}

		// 11 results in the messenger window not opening.
		private enum LoginResult : int { Okay = 0, Fail = 1, Pet = 11 }

		public void HandleFriendList(MsgrClient client, MabiPacket packet)
		{
			// Groups
			var gp = new MabiPacket(Op.Msgr.GroupList);
			gp.PutInt(1);
			//001 [........00000005] Int    : 5
			//002 [........00000001] Int    : 1
			//003 [................] String : abc
			//004 [........00000002] Int    : 2
			//005 [................] String : def
			//006 [........00000003] Int    : 3
			//007 [................] String : ghi
			//008 [........00000004] Int    : 4
			//009 [................] String : jkl
			//010 [........FFFFFFFF] Int    : 4294967295
			//011 [................] String : 
			gp.PutSInt(-1); // group id (-1 = etc)
			gp.PutString(""); // group name

			// Friends
			var fp = new MabiPacket(Op.Msgr.FriendListR);
			fp.PutInt(0); // count
			//    fp.PutInt(0); // contact id
			//    fp.PutByte(6); // 0 = normal, 1 = blocked, 3 = inviting, 4 = invited, 7 = hidden?
			//    fp.PutString("test1@Aura");
			//    fp.PutSInt(-1); // group id

			client.Send(gp, fp);
		}

		public void HandleNoteList(MsgrClient client, MabiPacket packet)
		{
			var p = new MabiPacket(Op.Msgr.NoteListR);
			p.PutByte(1);
			p.PutSInt(client.Contact.Notes.Count);
			foreach (var note in client.Contact.Notes)
			{
				p.PutLong(note.Id);
				p.PutString(note.Sender);
				p.PutString(note.Message);
				p.PutLong(ulong.Parse(note.Time.ToString("yyyyMddHms0")));
				p.PutByte(note.Read);
				p.PutByte(0); // icon?
			}

			client.Send(p);
		}

		public void HandleNoteSend(MsgrClient client, MabiPacket packet)
		{
			var accountFrom = packet.GetString();
			var targetFull = packet.GetString();
			var msg = packet.GetString();

			var success = Manager.Instance.SendNote(client.Contact.FullName, targetFull, msg);

			client.Send(
				new MabiPacket(Op.Msgr.SendNoteR)
				.PutSInt(success ? 0 : 2)
			);
		}

		public void HandleRefresh(MsgrClient client, MabiPacket packet)
		{

			// send new notes
			//Op: 0000C385, Id: 0000000000000000

			//001 [00000000013452F7] Long   : note id
			//002 [................] String : name
			//003 [................] String : server

		}
	}
}
