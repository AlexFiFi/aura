using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aura.Shared.Network;
using Aura.World.Network;

namespace Aura.World.Scripting
{
    public partial class BaseScript
    {
        public void OpenBank(WorldClient client)
        {
            // Send an initial BankStatus
            Send.BankStatus(client);
        }
    }
}
