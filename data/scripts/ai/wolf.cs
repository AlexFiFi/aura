// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.Collections;
using Aura.World.Scripting;
using Aura.World.World;
using Aura.Shared.Const;

public class WolfAI : AIScript
{
	public override void Definition()
	{
		DefineAction(IsIdleTrigger, Idle);
		DefineAction(OnNoticeTrigger, Noticed);
		DefineAction(OnAggroTrigger, Aggroed);
	}

	protected IEnumerable Idle()
	{
		SubAction(Wander(true, true));
	}

	protected IEnumerable Noticed()
	{
		SubAction(Wait(GetBeats(rnd.Next(2000, 5000))));
		if (Creature.Target.BattleState == 1)
		{
			SubAction(Aggro());
		}

		var rndAction = rnd.NextDouble(); // 33% each of loading def, counter, or doing nothing
		if (rndAction < .33)
		{
			SubAction(PrepareSkill(SkillConst.Defense, true, null));
		}
		else if (rndAction < .66)
		{
			SubAction(PrepareSkill(SkillConst.MeleeCounterattack, true, null));
			SubAction(Wait(GetBeats(rnd.Next(2000, 5000))));
			SubAction(CancelSkill());
		}

		var clockwise = rnd.NextDouble() < .5;
		for (int i = 0, t = rnd.Next(1, 5); i < t; i++)
			SubAction(Circle(Creature.Target, clockwise, 600, true));

		SubAction(CancelSkill());
	}

	protected IEnumerable Aggroed()
	{
		SubAction(CancelSkill());

		while (true)
		{
			var rndAction = rnd.NextDouble();

			if (Creature.ActiveSkillId == 0)
			{
				if (rndAction < .3)
				{
					//SubAction(PrepareSkill(SkillConst.Smash, true, null));
				}
			}

			SubAction(Attack());
		}
	}
}
