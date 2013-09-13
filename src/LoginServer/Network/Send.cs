// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aura.Shared.Network;
using Aura.Shared.Const;
using Aura.Login.Database;
using Aura.Shared.Database;

namespace Aura.Login.Network
{
	public static class Send
	{
		/// <summary>
		/// Sends response to client identification.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="success"></param>
		/// <param name="dateTime"></param>
		public static void ClientIdentResponse(LoginClient client, bool success, DateTime dateTime)
		{
			var packet = new MabiPacket(Op.ClientIdentR, Id.Login);
			packet.PutByte(success);
			packet.PutLong(dateTime);

			client.Send(packet);
		}

		/// <summary>
		/// Sends message as response to login.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public static void LoginResponse(LoginClient client, string format, params object[] args)
		{
			var packet = new MabiPacket(Op.LoginR, Id.Login);
			packet.PutByte(51);
			packet.PutInt(14);
			packet.PutInt(1);
			packet.PutString(format, args);

			client.Send(packet);
		}

		/// <summary>
		/// Sends (negative) login result.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="result"></param>
		public static void LoginResponse(LoginClient client, LoginResult result)
		{
			var packet = new MabiPacket(Op.LoginR, Id.Login);
			packet.PutByte((byte)result);
			if (result == LoginResult.SecondaryFail)
			{
				packet.PutInt(12);
				packet.PutByte(1);
			}

			client.Send(packet);
		}

		/// <summary>
		/// Sends positive response to login.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="account"></param>
		/// <param name="sessionKey"></param>
		/// <param name="servers"></param>
		public static void LoginResponse(LoginClient client, Account account, ulong sessionKey, IEnumerable<MabiServer> servers)
		{
			var packet = new MabiPacket(Op.LoginR, Id.Login);
			packet.PutByte((byte)LoginResult.Success);
			packet.PutString(account.Name);
			// [160XXX] Double account name
			{
				packet.PutString(account.Name);
			}
			packet.PutLong(sessionKey);
			packet.PutByte(0);

			// Servers
			// --------------------------------------------------------------
			packet.PutByte((byte)servers.Count());
			foreach (var server in servers)
				packet.Add(server);

			// Account Info
			// --------------------------------------------------------------
			packet.Add(account);

			client.Send(packet);
		}

		/// <summary>
		/// Sends request to input the secondary password.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="account"></param>
		/// <param name="sessionKey"></param>
		public static void RequestSecondaryPassword(LoginClient client, Account account, ulong sessionKey)
		{
			var packet = new MabiPacket(Op.LoginR, Id.Login);
			packet.PutByte((byte)LoginResult.SecondaryReq);
			packet.PutString(account.Name); // Official seems to send this
			packet.PutString(account.Name); // back hashed.
			packet.PutLong(sessionKey);
			if (account.SecondaryPassword == null)
				packet.PutString("FIRST");
			else
				packet.PutString("NOT_FIRST");
			client.Send(packet);
		}

		public static void CharacterInfoFail(LoginClient client, uint op)
		{
			CharacterInfo(client, op, null, null);
		}

		/// <summary>
		/// Sends character info. 
		/// </summary>
		/// <param name="client"></param>
		/// <param name="character"></param>
		public static void CharacterInfo(LoginClient client, uint op, Character character, IEnumerable<Item> items)
		{
			var packet = new MabiPacket(op, Id.Login);
			if (character != null)
			{
				packet.PutByte(true);
				packet.PutString(character.Server);
				packet.PutLong(character.Id);
				packet.PutByte(1);
				packet.PutString(character.Name);
				packet.PutString("");
				packet.PutString("");
				packet.PutInt(character.Race);
				packet.PutByte(character.SkinColor);
				packet.PutByte(character.Eye);
				packet.PutByte(character.EyeColor);
				packet.PutByte(character.Mouth);
				packet.PutInt(0);
				packet.PutFloat(character.Height);
				packet.PutFloat(character.Weight);
				packet.PutFloat(character.Upper);
				packet.PutFloat(character.Lower);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutByte(0);
				packet.PutInt(0);
				packet.PutByte(0);
				packet.PutInt(character.Color1);
				packet.PutInt(character.Color2);
				packet.PutInt(character.Color3);
				packet.PutFloat(0.0f);
				packet.PutString("");
				packet.PutFloat(49.0f);
				packet.PutFloat(49.0f);
				packet.PutFloat(0.0f);
				packet.PutFloat(49.0f);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutShort(0);
				packet.PutLong(0);
				packet.PutString("");
				packet.PutByte(0);

				packet.PutSInt(items.Count());
				foreach (var item in items)
				{
					packet.PutLong(item.Id);
					packet.PutBin(item.Info);
				}

				packet.PutInt(0);  // PetRemainingTime
				packet.PutLong(0); // PetLastTime
				packet.PutLong(0); // PetExpireTime
			}
			else
			{
				packet.PutByte(false);
			}

			client.Send(packet);
		}

		/// <summary>
		/// Sends channel information, needed to connect. Response will be
		/// negative if channel is null.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="channel"></param>
		/// <param name="characterId"></param>
		public static void EnterGameResponse(LoginClient client, MabiChannel channel, ulong characterId)
		{
			var packet = new MabiPacket(Op.ChannelInfo, Id.World);
			if (channel != null)
			{
				packet.PutByte(true);
				packet.PutString(channel.ServerName);
				packet.PutString(channel.Name);
				packet.PutShort(6); // Channel "Id"? (seems to be equal to channel nr)
				packet.PutString(channel.IP);
				packet.PutString(channel.IP);
				packet.PutShort(channel.Port);
				packet.PutShort((ushort)(channel.Port + 2));
				packet.PutInt(1);
				packet.PutLong(characterId);
			}
			else
			{
				packet.PutByte(false);
			}

			client.Send(packet);
		}

		/// <summary>
		/// Sends negative response to delete/request/recover.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="op">Is always request op + 1.</param>
		public static void DeleteFail(LoginClient client, uint op)
		{
			DeleteResponse(client, op, null, 0);
		}

		/// <summary>
		/// Sends response to delete/request/recover.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="op">Is always request op + 1.</param>
		/// <param name="serverName"></param>
		/// <param name="id"></param>
		public static void DeleteResponse(LoginClient client, uint op, string serverName, ulong id)
		{
			var packet = new MabiPacket(op, Id.Login);
			if (serverName != null)
			{
				packet.PutByte(true);
				packet.PutString(serverName);
				packet.PutLong(id);
				packet.PutLong(0);
			}
			else
			{
				packet.PutByte(false);
			}

			client.Send(packet);
		}

		/// <summary>
		/// Sends response to name check.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="result"></param>
		public static void NameCheckResponse(LoginClient client, NameCheckResult result)
		{
			var response = new MabiPacket(Op.NameCheckR, Id.Login);
			response.PutByte(result == NameCheckResult.Okay);
			response.PutByte((byte)result);

			client.Send(response);
		}

		/// <summary>
		/// Sends response to accept gift. Response will be negative if gift is null.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="gift"></param>
		public static void AcceptGiftResponse(LoginClient client, Gift gift)
		{
			var packet = new MabiPacket(Op.AcceptGiftR, Id.Login);
			if (gift != null)
			{
				packet.PutByte(true);
				packet.PutByte(gift.IsCharacter);
				packet.PutInt(0); // ?
				packet.PutInt(0); // ?
				packet.PutInt(gift.Type);
				// ?
			}
			else
			{
				packet.PutByte(false);
			}

			client.Send(packet);
		}

		/// <summary>
		/// Sends negative response to create character.
		/// </summary>
		/// <param name="client"></param>
		public static void CreateCharacterFail(LoginClient client)
		{
			CreateCharacterResponse(client, null, 0);
		}

		/// <summary>
		/// Sends response to create character.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="serverName"></param>
		/// <param name="id"></param>
		public static void CreateCharacterResponse(LoginClient client, string serverName, ulong id)
		{
			var response = new MabiPacket(Op.CharacterCreated, Id.Login);
			if (serverName != null)
			{
				response.PutByte(true);
				response.PutString(serverName);
				response.PutLong(id);
			}
			else
			{
				response.PutByte(false);
			}

			client.Send(response);
		}

		/// <summary>
		/// Sends negative response to create pet.
		/// </summary>
		/// <param name="client"></param>
		public static void CreatePetFail(LoginClient client)
		{
			CreatePetResponse(client, null, 0);
		}

		/// <summary>
		/// Sends response to create pet.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="serverName"></param>
		/// <param name="id"></param>
		public static void CreatePetResponse(LoginClient client, string serverName, ulong id)
		{
			var response = new MabiPacket(Op.PetCreated, Id.Login);
			if (serverName != null)
			{
				response.PutByte(true);
				response.PutString(serverName);
				response.PutLong(id);
			}
			else
			{
				response.PutByte(false);
			}

			client.Send(response);
		}

		/// <summary>
		/// Sends response to refuse gift.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="success"></param>
		public static void RefuseGiftResponse(LoginClient client, bool success)
		{
			var packet = new MabiPacket(Op.RefuseGiftR, Id.Login);
			packet.PutByte(success);
			// ?

			client.Send(packet);
		}

		/// <summary>
		/// Sends account information. Response will be negative if account is null.
		/// </summary>
		/// <param name="client"></param>
		public static void AccountInfoRequestResponse(LoginClient client, Account account)
		{
			var packet = new MabiPacket(Op.AccountInfoRequestR, Id.Login);
			if (account != null)
			{
				packet.PutByte(true);
				packet.Add(account);
			}
			else
			{
				packet.PutByte(false);
			}

			client.Send(packet);
		}

		/// <summary>
		/// Sends server/channel status update to all connected clients,
		/// incl channels.
		/// </summary>
		public static void ChannelUpdate()
		{
			var packet = new MabiPacket(Op.ChannelStatus, Id.Login);
			packet.PutByte((byte)LoginServer.Instance.ServerList.Count);
			foreach (var server in LoginServer.Instance.ServerList.Values)
				packet.Add(server);

			LoginServer.Instance.Broadcast(packet);
		}

		/// <summary>
		/// Sends response to server identification from channel.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="success"></param>
		public static void ServerIdentifyResponse(LoginClient client, bool success)
		{
			var packet = new MabiPacket(Op.Internal.ServerIdentify);
			packet.PutByte(success);

			client.Send(packet);
		}

		/// <summary>
		/// Adds information about server and its channels to packet.
		/// </summary>
		/// <param name="packet"></param>
		/// <param name="server"></param>
		private static void Add(this MabiPacket packet, MabiServer server)
		{
			packet.PutString(server.Name);
			packet.PutShort(0); // Server type?
			packet.PutShort(0);
			packet.PutByte(1);

			// Channels
			// ----------------------------------------------------------
			packet.PutInt((uint)server.Channels.Count);
			foreach (var channel in server.Channels.Values)
			{
				var state = channel.State;
				if ((DateTime.Now - channel.LastUpdate).TotalSeconds > 90)
					state = ChannelState.Maintenance;

				packet.PutString(channel.Name);
				packet.PutInt((uint)state);
				packet.PutInt((uint)channel.Events);
				packet.PutInt(0); // 1 for Housing? Hidden?
				packet.PutShort(channel.Stress);
			}
		}

		/// <summary>
		/// Adds extended account information to packet.
		/// </summary>
		/// <param name="account"></param>
		/// <param name="packet"></param>
		private static void Add(this  MabiPacket packet, Account account)
		{
			packet.PutLong(63461647475710);	// Last Login
			packet.PutLong(63461647484213);	// Last Logout
			packet.PutInt(0);
			packet.PutByte(1);
			packet.PutByte(34);
			packet.PutInt(0x800200FF);
			packet.PutByte(1);

			// Premium services, listed in char selection
			// --------------------------------------------------------------
			// All 3 are visible, if one is set.
			packet.PutByte(false);			// Nao's Support
			packet.PutLong(0);
			packet.PutByte(false);			// Extra Storage
			packet.PutLong(0);
			packet.PutByte(false);			// Advanced Play
			packet.PutLong(0);

			packet.PutByte(0);
			packet.PutByte(1);

			// Always visible?
			packet.PutByte(false);          // Inventory Plus Kit
			packet.PutLong(0);              // DateTime
			packet.PutByte(false);          // Mabinogi Premium Pack
			packet.PutLong(0);
			packet.PutByte(false);          // Mabinogi VIP
			packet.PutLong(0);

			// [170402, TW170300] New premium thing
			{
				// Invisible?
				packet.PutByte(0);
				packet.PutLong(0);
			}

			packet.PutByte(0);
			packet.PutByte(0);				// 1: 프리미엄 PC방 서비스 사용중, 16: Free Play Event
			packet.PutByte(false);			// Free Beginner Service

			// Characters
			// --------------------------------------------------------------
			packet.PutShort((ushort)account.Characters.Count);
			foreach (var character in account.Characters)
			{
				packet.PutString(character.Server);
				packet.PutLong(character.Id);
				packet.PutString(character.Name);
				packet.PutByte((byte)character.DeletionFlag);
				packet.PutLong(0);
				packet.PutInt(0);
				packet.PutByte(0); // 0: Human, 1: Elf, 2: Giant
				packet.PutByte(0);
				packet.PutByte(0);
			}

			// Pets
			// --------------------------------------------------------------
			packet.PutShort((ushort)account.Pets.Count);
			foreach (var pet in account.Pets)
			{
				packet.PutString(pet.Server);
				packet.PutLong(pet.Id);
				packet.PutString(pet.Name);
				packet.PutByte((byte)pet.DeletionFlag);
				packet.PutLong(0);
				packet.PutInt(pet.Race);
				packet.PutLong(0);
				packet.PutLong(0);
				packet.PutInt(0);
				packet.PutByte(0);
			}

			// Character cards
			// --------------------------------------------------------------
			packet.PutShort((ushort)account.CharacterCards.Count);
			foreach (var card in account.CharacterCards)
			{
				packet.PutByte(0x01);
				packet.PutLong(card.Id);
				packet.PutInt(card.Type);
				packet.PutLong(0);
				packet.PutLong(0);
				packet.PutInt(0);
			}

			// Pet cards
			// --------------------------------------------------------------
			packet.PutShort((ushort)account.PetCards.Count);
			foreach (var card in account.PetCards)
			{
				packet.PutByte(0x01);
				packet.PutLong(card.Id);
				packet.PutInt(card.Type);
				packet.PutInt(card.Race);
				packet.PutLong(0);
				packet.PutLong(0);
				packet.PutInt(0);
			}

			// Gifts
			// --------------------------------------------------------------
			packet.PutShort((ushort)account.Gifts.Count);
			foreach (var gift in account.Gifts)
			{
				packet.PutLong(gift.Id);
				packet.PutByte(gift.IsCharacter);
				packet.PutInt(gift.Type);
				packet.PutInt(gift.Race);
				packet.PutString(gift.Sender);
				packet.PutString(gift.SenderServer);
				packet.PutString(gift.Message);
				packet.PutString(gift.Receiver);
				packet.PutString(gift.ReceiverServer);
				packet.PutLong(gift.Added);
			}

			packet.PutByte(0);
		}
	}

	public enum LoginType
	{
		KR = 0x00,
		FromChannel = 0x02,
		NewHash = 0x05,
		Normal = 0x0C,
		CmdLogin = 0x10,
		EU = 0x12,
		SecondaryPassword = 0x14
	}

	public enum LoginResult
	{
		Fail = 0,
		Success = 1,
		Empty = 2,
		IdOrPassIncorrect = 3,
		/* IdOrPassIncorrect = 4, */
		TooManyConnections = 6,
		AlreadyLoggedIn = 7,
		UnderAge = 33,
		SecondaryReq = 90,
		SecondaryFail = 91,
		Banned = 101
	}
}
