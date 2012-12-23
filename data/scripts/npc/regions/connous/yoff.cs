using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class YoffScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_yoff");
		SetRace(17105);
		SetBody(height: 1.2f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		



		SetLocation(region: 3103, x: 1200, y: 3700);

		SetDirection(185);
		SetStand("elf/male/anim/elf_npc_yoff_stand_friendly");
	}
}
