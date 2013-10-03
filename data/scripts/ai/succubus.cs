// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.Collections;
using Aura.World.Scripting;
using Aura.World.World;
using Aura.Shared.Const;

public class SuccubusAI : AIScript
{
	protected static readonly uint[] Hairs = { 3081, 3083, 3001, 3002, 3003, 3012, 3013, 3014, 3016, 3017, 3018, 3021, 3022, 3004, 3005, 3024, 3084, 3011, 3008 };
	protected static readonly uint[] HairColors = { 0x000000, 0xffffff, 0xffff00, 0xff9000, 0xffa040, 0x900000, 0xff4020, 0x808080 };
	
	public override void Definition()
	{
		DefineAction(IsIdleTrigger, Idle);
		DefineAction(OnNoticeTrigger, Noticed);
		DefineAction(OnAggroTrigger, Attack);

		SetFace(16, (byte)rnd.Next(3,6), 221, (byte)rnd.Next(0,3));

		EquipItem(Pocket.Head, 18033);
		EquipItem(Pocket.Face, 3900);
		EquipItem(Pocket.Armor, 15046);
		EquipItem(Pocket.Hair, Hairs[rnd.Next(0, Hairs.Length)], HairColors[rnd.Next(0, HairColors.Length)]);
		EquipItem(Pocket.RightHand1, 40010);
	}

	protected IEnumerable Idle()
	{
		SubAction(Wander(true, false));
	}

	protected IEnumerable Noticed()
	{
		SubAction(Wait(GetBeats(rnd.Next(5000, 7000))));
		if (Creature.Target.BattleState == 1)
		{
			SubAction(Aggro());
		}

		var rndAction = rnd.NextDouble();
		if (rndAction < .1)
		{
			SubAction(Say("Who are you?"));
		}
		else if (rndAction < .2)
		{
			SubAction(Say("Why did you come here?"));
		}
		else if (rndAction < .3)
		{
			SubAction(Say("=)"));
		}
		else if (rndAction < .4)
		{
			SubAction(Say("I'm coming closer."));
		}
		else if (rndAction < .5)
		{
			SubAction(Say("You're very cute."));
		}
		else if (rndAction < .6)
		{
			SubAction(Say("What are you thinking about?"));
		}
		else if (rndAction < .7)
		{
			SubAction(PrepareSkill(SkillConst.Firebolt, true, null));
			SubAction(Wait(GetBeats(5000)));
			SubAction(CancelSkill());
			SubAction(Say("Ha ha ha!"));
		}

		var clockwise = rnd.NextDouble() < .5;
		for (int i = 0, t = rnd.Next(1, 7); i < t; i++)
			SubAction(Circle(Creature.Target, clockwise, 200, true));
	}
}
