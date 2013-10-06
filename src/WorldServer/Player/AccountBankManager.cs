using Aura.World.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aura.World.Player
{
    public class AccountBankManager
    {
        public delegate double TaxCallback(Account account, AccountBankManager bankManager);
        public delegate bool CheckAmountCallback(uint gold);

        //public List<BankPocket> Pockets { get; private set; }
        public Dictionary<byte, BankPocket> Pockets { get; set; }

        private uint _gold = 0; // Sent as uint in 0x721F
        public uint Gold
        {
            get
            {
                lock (_lock)
                    return _gold;
            }
        }

        private string _password = "";
        public string Password
        {
            get { return _password; }
        }
        public bool HasPassword
        {
            get { return (_password != null && !_password.Equals("")); }
        }
        
        /// <summary>
        /// Last time a bank session was opened (closed? needs more research).
        /// Not yet implemented.
        /// </summary>
        public DateTime LastUse = DateTime.MinValue;

        private Object _lock = new Object();

        private static readonly uint MaxWithdraw = 50000; // Max gold withdraw at once, make preference later?

        /*
        private static readonly double DepositTax = 0f;
        private static readonly double WithdrawTax = 0f;
        private static readonly double DepositItemTax = 0.025f; // 2.5% item's NPC purchase price
        private static readonly double WithdrawItemTax = 0f;
        */

        private static TaxCallback _depositTax = AccountBankManager.DepositTaxDefault;
        private static TaxCallback _withdrawTax = null; // No wihdraw tax by default
        private static TaxCallback _depositItemTax = AccountBankManager.DepositItemTaxDefault;
        private static TaxCallback _withdrawItemTax = null; // No withdraw tax by default
        private static TaxCallback _withdrawCheckTax = AccountBankManager.WithdrawCheckTaxDefault;
        private static TaxCallback _depositCheckTax = null; // No withdraw tax by default

        private static CheckAmountCallback _canMakeCheck = AccountBankManager.CanMakeCheckDefault;

        public BankSession Session { get; private set; }

        public Account Account { get; private set; }

        /// <summary>
        /// Create a new bank manager.
        /// </summary>
        /// <param name="account">Parent account</param>
        /// <param name="gold">Initial amount of gold</param>
        /// <param name="pass">Password for lock</param>
        public AccountBankManager(Account account, uint gold = 0, string pass = "")
        {
            this.Account = account;

            _gold = gold;
            _password = pass;

            this.Session = new BankSession();

            this.Pockets = new Dictionary<byte, BankPocket>();
        }

        /// <summary>
        /// Get a pocket from this bank account.
        /// </summary>
        /// <param name="index">Pocket index</param>
        /// <returns>Pocket of specified index, or null if none found</returns>
        public BankPocket GetPocketOrNull(byte index)
        {
            BankPocket pocket = null;
            if (this.Pockets.TryGetValue(index, out pocket))
                return pocket;
            return null;
        }

        /// <summary>
        /// Add multiple pockets to this bank account.
        /// </summary>
        /// <param name="pockets">Pockets to add</param>
        public void AddPockets(IEnumerable<BankPocket> pockets)
        {
            if (pockets == null) return;
            foreach (BankPocket pocket in pockets)
                this.AddPocket(pocket);
        }

        /// <summary>
        /// Add a pocket to this bank account.
        /// </summary>
        /// <param name="pocket">Pocket to add</param>
        public void AddPocket(BankPocket pocket)
        {
            if (pocket == null) return;
            try { this.Pockets.Add(pocket.Index, pocket); }
            catch { }
        }

        /// <summary>
        /// Change the password of this bank account.
        /// </summary>
        /// <param name="oldPass">Old password, ignored if account not yet password protected</param>
        /// <param name="newPass">New password to use</param>
        /// <returns>true if successful, false otherwise (bad oldPass)</returns>
        public bool ChangePassword(string oldPass, string newPass)
        {
            // I don't see much purpose in making password changing atomic..

            if ((this.HasPassword && _password.Equals(oldPass)) // If oldPass matches existing password
              || !this.HasPassword) // If no existing password
            {
                _password = newPass;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Password check.
        /// </summary>
        /// <param name="pass">Password to check</param>
        /// <returns>true if password, false otherwise</returns>
        public bool IsPassword(string pass)
        {
            return (!this.HasPassword || _password.Equals(pass));
        }

        /// <summary>
        /// Deposit gold into this bank account (atomically).
        /// </summary>
        /// <param name="character">Character depositing the gold</param>
        /// <param name="gold">Amount of gold to deposit</param>
        /// <param name="isCheck">true to deposit a check, false to deposit gold</param>
        /// <returns>true if success, false otherwise</returns>
        public bool Deposit(MabiCharacter character, uint gold, bool isCheck = false)
        {
            // TODO: Add support for checks/check tax. (For now, isCheck is ignored)

            var success = true;
            
            uint tax = 0;
            if (_depositTax != null)
                tax = (uint)(_depositTax(null, this) * gold);

            // Atomic enough?
            lock (_lock)
            {
                // Warning: Double lock, don't call any session funcs/accessors
                // inside this block (they share the same lock)
                lock (this.Session.Lock)
                {
                    // Make sure we have enough gold
                    if (!character.HasGold(gold + tax))
                    {
                        success = false;
                    }
                    else
                    {
                        character.RemoveGold(gold + tax);
                        _gold += gold;
                    }
                }
            }

            return success;
        }

        /// <summary>
        /// Withdraw gold from this bank account (atomically).
        /// </summary>
        /// <param name="character">Character withdrawing gold</param>
        /// <param name="gold">Amount of gold to withdraw</param>
        /// <param name="isCheck">true to withdraw as a check, false to withdraw as gold</param>
        /// <returns></returns>
        public bool Withdraw(MabiCharacter character, uint gold, bool isCheck = false)
        {
            // Not really the correct way to force activity within a session..
            if (!this.Session.IsActive)
                return false;

            if (gold > AccountBankManager.MaxWithdraw)
                return false;

            var success = true;

            // :C
            uint tax = 0;
            if (!isCheck)
            {
                // Withdraw gold tax
                if (_withdrawTax != null)
                    tax = (uint)(_withdrawTax(null, this) * gold);
            }
            else
            {
                // Withdraw check tax
                if (_withdrawCheckTax != null)
                    tax = (uint)(_withdrawCheckTax(null, this) * gold);
            }


            lock (_lock)
            {
                // Warning: Double lock, don't call any session funcs/accessors
                // inside this block (they share the same lock)
                lock (this.Session.Lock)
                {
                    // For now, take ONLY from bank account

                    if (_gold < (gold + tax))
                        success = false;
                    else
                    {
                        _gold -= (gold + tax);

                        if (!isCheck)
                            character.GiveGold(gold); // Give gold
                        else
                        {
                            // Give check
                            this.GiveCheck(character, gold);
                        }
                    }
                }
            }

            return success;
        }

        /// <summary>
        /// Give a check to a player.
        /// </summary>
        /// <param name="character">Player to give the check to</param>
        /// <param name="gold">Amount of gold the check is worth</param>
        /// <returns>Check item</returns>
        private MabiItem GiveCheck(MabiCharacter character, uint gold)
        {
            // Check class Id: 0x7D4
            var item = character.GiveItem(0x7D4, 1); // Not sure if this sends ItemNew..? (Edit: it does)
            item.Tags.SetInt("EVALUE", gold);
            character.ItemUpdate(item, false); // Eh, whatever works for now
            return item;
        }

        public bool IsSessionActive
        {
            get
            {
                return this.Session.IsActive;
            }
        }

        public void OpenSession(MabiCharacter character)
        {
            this.Session.Open();
        }

        public void CloseSession(MabiCharacter character)
        {
            this.Session.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="bankManager"></param>
        /// <returns></returns>
        private static double DepositTaxDefault(Account account, AccountBankManager bankManager)
        {
            return AccountBankManager.WednesdayBonus(0.1d); // 10%
        }

        private static double DepositItemTaxDefault(Account account, AccountBankManager bankManager)
        {
            return AccountBankManager.WednesdayBonus(0.025d); // 2.5%
        }

        private static double WithdrawCheckTaxDefault(Account account, AccountBankManager bankManager)
        {
            return AccountBankManager.WednesdayBonus(0.05d); // 5%
        }

        private static double WednesdayBonus(double t)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Wednesday)
                return (t * 0.75d);
            else return t;
        }

        /// <summary>
        /// Default callback for whether or not a check can be created with some
        /// amount of gold. If the amount is divisible by 10,000 then the check
        /// can be made.
        /// </summary>
        /// <param name="gold">Amount of gold the check is worth</param>
        /// <returns>true if check can be made, false otherwise</returns>
        private static bool CanMakeCheckDefault(uint gold)
        {
            return ((gold % 10000) == 0);
        }
    }

    /// <summary>
    /// A bank pocket, or inventory.
    /// </summary>
    public class BankPocket
    {
        /// <summary>
        /// Width of this pocket in squares.
        /// </summary>
        public uint Width { get; private set; }

        /// <summary>
        /// Height of this pocket in squares.
        /// </summary>
        public uint Height { get; private set; }

        /// <summary>
        /// The tabname of this pocket.
        /// </summary>
        public string Name = null;

        /// <summary>
        /// Whether or not this bank pocket is enabled. Should mainly be disabled
        /// for characters that have not yet been created (indexes 0, 1, 2).
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Index of the bank pocket, relative to Account. Each account may have
        /// up to 256 bank pockets, with the first three being defaults for the
        /// human character, elf character, and giant character.
        /// </summary>
        public byte Index { get; private set; }

        public BankPocket(string name, byte index, bool enabled, uint width, uint height)
        {
            this.Name = name;
            this.Index = index;
            this.IsEnabled = enabled;
            this.Width = width;
            this.Height = height;
        }
    }

    public class BankSession
    {
        bool _open = false;

        private Object _lock = new Object();
        public Object Lock { get { return _lock; } }

        public BankSession() { }

        public bool IsActive
        {
            get
            {
                lock(_lock)
                    return _open;
            }
        }

        public void Apply()
        {

        }

        public void Close()
        {
            lock (_lock)
                _open = false;
        }

        public void Open()
        {
            lock (_lock)
                _open = true;
        }
    }
}