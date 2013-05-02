// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using Aura.Shared.Database;
using Aura.Shared.Network;

namespace Aura.Login.Database
{
	public class Account
	{
		public string Name { get; set; }
		public string Password { get; set; }
		public string SecondaryPassword { get; set; }

		public byte Authority { get; set; }

		public DateTime Creation { get; set; }
		public DateTime LastLogin { get; set; }
		public string LastIp { get; set; }

		public string BannedReason { get; set; }
		public DateTime BannedExpiration { get; set; }

		public bool LoggedIn { get; set; }

		public List<Card> CharacterCards { get; set; }
		public List<Card> PetCards { get; set; }
		public List<Character> Characters { get; set; }
		public List<Character> Pets { get; set; }
		public List<Gift> Gifts { get; set; }

		public Account()
		{
			this.Creation = DateTime.Now;
			this.LastLogin = DateTime.Now;

			this.CharacterCards = new List<Card>();
			this.PetCards = new List<Card>();
			this.Gifts = new List<Gift>();

			this.Characters = new List<Character>();
			this.Pets = new List<Character>();
		}

		public Card GetCharacterCard(ulong id)
		{
			return this.CharacterCards.FirstOrDefault(a => a.Id == id);
		}

		public Card GetPetCard(ulong id)
		{
			return this.PetCards.FirstOrDefault(a => a.Id == id);
		}

		public Character GetCharacter(ulong id)
		{
			return this.Characters.FirstOrDefault(a => a.Id == id);
		}

		public Character GetPet(ulong id)
		{
			return this.Pets.FirstOrDefault(a => a.Id == id);
		}

		public Gift GetGift(ulong id)
		{
			return this.Gifts.FirstOrDefault(a => a.Id == id);
		}

		public void AddToPacket(MabiPacket packet)
		{
			// Premium services?
			// --------------------------------------------------------------
			packet.PutLong(63461647475710);
			packet.PutLong(63461647484213);
			packet.PutInt(0);
			packet.PutByte(1);
			packet.PutByte(34);
			packet.PutInt(0x800200FF);
			packet.PutByte(1);
			packet.PutByte(0);
			packet.PutLong(0); //
			packet.PutByte(0);
			packet.PutLong(0);
			packet.PutByte(0);
			packet.PutLong(0);
			packet.PutByte(0);
			packet.PutByte(1);
			packet.PutByte(0); // Inventory Plus Kit
			packet.PutLong(63362367600000);
			packet.PutByte(0); // Mabinogi Premium Pack
			packet.PutLong(63362367600000);
			packet.PutByte(0); // Mabinogi VIP
			packet.PutLong(0); // till next week = (ulong)(DateTime.Now.AddDays(7).Ticks/10000)
			if (Op.Version >= 170401)
			{
				packet.PutByte(0);
				packet.PutLong(0);
			}
			packet.PutByte(0);
			packet.PutByte(0);
			packet.PutByte(0);

			// Characters
			// --------------------------------------------------------------
			packet.PutShort((ushort)this.Characters.Count);
			foreach (var character in this.Characters)
			{
				packet.PutString(character.Server);
				packet.PutLong(character.Id);
				packet.PutString(character.Name);
				packet.PutByte((byte)character.DeletionFlag);
				packet.PutLong(0); // ??
				packet.PutInt(0);
				packet.PutByte(0); // 0 = Human. 1 = Elf. 2 = Giant.
				packet.PutByte(0);
				packet.PutByte(0);
			}

			// Pets
			// --------------------------------------------------------------
			packet.PutShort((ushort)this.Pets.Count);
			foreach (var pet in this.Pets)
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
			packet.PutShort((ushort)this.CharacterCards.Count);
			foreach (var card in this.CharacterCards)
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
			packet.PutShort((ushort)this.PetCards.Count);
			foreach (var card in this.PetCards)
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
			packet.PutShort((ushort)this.Gifts.Count);
			foreach (var gift in this.Gifts)
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
}
