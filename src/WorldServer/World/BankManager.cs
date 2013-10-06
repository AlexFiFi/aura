using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aura.Data;

namespace Aura.World.World
{
    /// <summary>
    /// Singleton class for managing bank stuff.
    /// </summary>
    public class BankManager
    {
        public static readonly BankManager Instance = new BankManager();

        public BankDb Banks = MabiData.BankDb;

        // TODO: Add functions for getting distances, calculating time that items
        // take to be transported between different towns

        /// <summary>
        /// Singleton constructor.
        /// </summary>
        private BankManager() { }

        /// <summary>
        /// Get a BankInfo from the string Id, or null if none.
        /// </summary>
        /// <param name="name">Id of bank</param>
        /// <returns>BankInfo of bank, or null if unrecognized Id</returns>
        public BankInfo GetBankOrNull(string id)
        {
            BankInfo info = null;
            if (this.Banks.Entries.TryGetValue(id, out info))
                return info;
            return null;
        }

        /// <summary>
        /// Get the amount of time (in milliseconds?) an item should take to be delivered
        /// to the user. (Needs more research)
        /// </summary>
        /// <param name="item">Item to transfer</param>
        /// <param name="storedBank">Bank the item is stored in</param>
        /// <param name="currBank">Bank where the item is to be delivered to</param>
        /// <returns>Time in milliseconds?</returns>
        public ulong GetItemTransferTime(MabiItem item, BankInfo storedBank, BankInfo currBank)
        {
            // Temporarily just return 0 for no transfer time
            return 0;
        }
    }
}
