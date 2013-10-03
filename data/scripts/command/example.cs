using System;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.World;

public class ScriptCommandExample : Command
{
	public ScriptCommandExample()
	{
		Name = "example";
		Auth = Authority.Player;
		Func = this.Run;
	}

	private CommandResult Run(WorldClient client, MabiCreature creature, string[] args, string msg)
	{
		Console.WriteLine("example command executed");
		
		return CommandResult.Okay;
	}
}
