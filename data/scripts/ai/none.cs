// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.Collections;
using Aura.World.Scripting;
using Aura.World.World;

public class NoneAI : AIScript
{
	public override void Definition()
	{
		DefineAction(IsIdleTrigger, Idle);
	}

	public IEnumerable Idle()
	{
		SubAction(Wander(false, false));
	}
}
