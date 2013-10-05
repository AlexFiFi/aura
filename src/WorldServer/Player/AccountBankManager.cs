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
        public Dictionary<byte, BankPocket> Pockets { get; private set; }

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
        public bool HasPassword
        {
            get { return (_password != null && !_password.Equals("")); }
        }

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
        private static TaxCallback _withdrawCheckTax = AccountBankManager.CreateCheckTaxDefault;
        private static TaxCallback _depositCheckTax = null; // No withdraw tax by default

        private static CheckAmountCallback _canMakeCheck = AccountBankManager.CanMakeCheckDefault;

        public BankSession Session { get; private set; }

        public Account Account { get; private set; }

        public AccountBankManager(Account account, uint gold = 0, string pass = "")
        {
            this.Account = account;

            _gold = gold;
            _password = pass;

            this.Session = new BankSession();

            this.Pockets = new Dictionary<byte, BankPocket>();
        }

        public BankPocket GetPocketOrNull(byte index)
        {
            BankPocket pocket = null;
            if (this.Pockets.TryGetValue(index, out pocket))
                return pocket;
            return null;
        }

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

        public bool IsPassword(string pass)
        {
            return (!this.HasPassword || _password.Equals(pass));
        }

        public bool Deposit(MabiCharacter character, uint gold, bool isCheck = false)
        {
            var success = true;

            // fuck the IRS
            uint tax = 0;
            if (_depositTax != null)
                tax = (uint)(_depositTax(null, this) * gold);

            // Atomic enough?
            lock (_lock)
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

            return success;
        }

        public bool Withdraw(MabiCharacter character, uint gold, bool isCheck = false)
        {
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

            return success;
        }

        /// <summary>
        /// Give a check to a player.
        /// </summary>
        /// <param name="character"></param>
        /// <param name="gold"></param>
        /// <returns></returns>
        private MabiItem GiveCheck(MabiCharacter character, uint gold)
        {
            // Check class Id: 0x7D4
            var item = character.GiveItem(0x7D4, 1); // Not sure if this sends ItemNew..?
            item.Tags.SetInt("EVALUE", gold);
            character.ItemUpdate(item, false); // Eh, whatever works for now
            return item;
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

        private static double CreateCheckTaxDefault(Account account, AccountBankManager bankManager)
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
        public bool IsEnabled { get; private set; }

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

        public BankSession() { }

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