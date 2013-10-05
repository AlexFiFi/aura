using System;
using System.Collections.Generic;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.World;
using Aura.World.Player;

public class BankGoldCommandExample : Command
{
	public BankGoldCommandExample()
	{
		Name = "bank.gold";
		Auth = Authority.Admin;
		Func = this.Run;
	}

	private CommandResult Run(WorldClient client, MabiCreature creature, string[] args, string msg)
	{
		client.Account.BankManager = new AccountBankManager(client.Account, 500000);
		
		return CommandResult.Okay;
	}
}