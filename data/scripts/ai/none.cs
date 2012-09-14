// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Common.World;
using World.Scripting;
using World.World;

public class NoneAI : AIScript
{
	public override void Definition()
	{
		Define(AIState.Idle, AIAction.WalkRandom, intVal1: 600, cooldown: 5000);
		Define(AIState.Aggro, AIAction.Attack);
	}
}
