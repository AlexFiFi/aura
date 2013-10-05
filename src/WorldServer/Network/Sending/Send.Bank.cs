using Aura.Shared.Const;
using Aura.Shared.Network;
using Aura.World.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aura.World.Network
{
    public static partial class Send
    {
        public static void BankStatus(WorldClient client, List<BankPocket> pockets)
        {
            /*
            Op: 00007212, Id: 00100000000DD69B

            001 [..............01] Byte   : 1
            002 [..............01] Byte   : 1
            003 [000039C47B440580] Long   : 63516044428672
            004 [..............01] Byte   : 1
            005 [................] String : efe527f57b24
            006 [................] String : DunbartonBank
            007 [................] String : Dunbarton Bank
            008 [........000ECD9C] Int    : 970140
            009 [........00000001] Int    : 1
            010 [................] String : Fallere
            011 [..............01] Byte   : 1
            012 [........0000000C] Int    : 12
            013 [........00000008] Int    : 8
            014 [........00000000] Int    : 0
            */
            var character = client.Character;
            if (character == null) return;

            byte race = 0;
            if (character.IsHuman) race = 0;
            else if (character.IsElf) race = 1;
            else if (character.IsGiant) race = 2;

            var packet = new MabiPacket(Op.BankInventoryRequestR, client.Character.Id)
                .PutByte(true) // Success / Fail?
                .PutByte(race) // Might also be index..? But what about multiple pockets?
                .PutLong((ulong)DateTime.Now.Ticks) // Might be last-access timestamp?
                .PutByte(0) // Seen 1 and 0
                .PutString(client.Account.Password) // Hashed password here? Length: 12 chars
                .PutString("DunbartonBank") // Bank name
                .PutString("") // "Dunbarton Bank", dialog title, but I don't like it, font is weird
                .PutInt(client.Account.BankManager.Gold); // Gold amount

            // Add pockets
            packet.PutInt((uint)pockets.Count);
            foreach (BankPocket pocket in pockets)
            {
                packet.AddBankPocket(pocket);
            }
        }

        public static void BankGoldAmount(WorldClient client)
        {
            var character = client.Character;
            character.Client.Send(new MabiPacket(Op.BankGoldAmount, character.Id)
                .PutInt(client.Account.BankManager.Gold));
        }

        public static void BankWithdrawR(WorldClient client, bool success)
        {
            var character = client.Character;
            character.Client.Send(new MabiPacket(Op.BankGoldWithdrawR, character.Id)
                .PutByte(success));
        }

        public static void BankDepositR(WorldClient client, bool success)
        {
            var character = client.Character;
            character.Client.Send(new MabiPacket(Op.BankGoldDepositR, character.Id)
                .PutByte(success));
        }

        private static void AddBankPocket(this MabiPacket packet, BankPocket pocket)
        {
            packet.PutString(pocket.Name) // Tabname
                  .PutByte(pocket.Index)  // Index
                  .PutInt(pocket.Width)   // Width
                  .PutInt(pocket.Height); // Height

            // Add items...
            // Dunno how this should be handled yet, no items
            packet.PutInt(0);
        }
    }
}