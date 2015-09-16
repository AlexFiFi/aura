// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;

namespace Aura.Data
{
    public class BankInfo
    {
        public string Id;
        public string DialogTitle;
        public bool CanDeposit;
        public bool CanWithdraw;
        public bool CanDepositItem;
        public bool CanWithdrawItem;
        public bool CanModifyLock;
    }

    /// <summary>
    /// Indexed by item id.
    /// </summary>
    public class BankDb : DatabaseCSVIndexed<string, BankInfo>
    {
        protected override void ReadEntry(CSVEntry entry)
        {
            if (entry.Count != 7)
                throw new FieldCountException(7);

            var info = new BankInfo();
            info.Id = entry.ReadString();
            info.DialogTitle = entry.ReadString();
            info.CanDeposit = entry.ReadBool();
            info.CanWithdraw = entry.ReadBool();
            info.CanDepositItem = entry.ReadBool();
            info.CanWithdrawItem = entry.ReadBool();
            info.CanModifyLock = entry.ReadBool();

            this.Entries.Add(info.Id, info);
        }
    }
}