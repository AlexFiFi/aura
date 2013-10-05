using System;
using System.Collections.Generic;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.World;
using Aura.World.Player;

public class TestBankCommandExample : Command
{
	public TestBankCommandExample()
	{
		Name = "test.bank";
		Auth = Authority.Player;
		Func = this.Run;
	}

	private CommandResult Run(WorldClient client, MabiCreature creature, string[] args, string msg)
	{
		var pockets = new List<BankPocket>();
		pockets.Add(new BankPocket("Test 1", 0, true, 36, 24));
		//pockets.Add(new BankPocket("Test 2", 1, true, 12, 8));
		//pockets.Add(new BankPocket("Test 3", 2, true, 12, 8));
	
		Send.BankStatus(client, pockets);
		
		return CommandResult.Okay;
	}
}