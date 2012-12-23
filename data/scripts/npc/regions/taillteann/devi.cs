using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class DeviScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_devi");
		SetRace(45);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 17, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0xCF9B5F;
		NPC.ColorC = 0x808080;		



		SetLocation(region: 300, x: 215391, y: 194644);

		SetDirection(171);
		SetStand("chapter3/human/male/anim/male_c3_npc_devi");
	}
}
