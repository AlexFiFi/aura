using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class KrugScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_krug");
		SetRace(28);
		SetBody(height: 1.3f, fat: 1f, upper: 1.2f, lower: 1f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		



		SetLocation(region: 3200, x: 289307, y: 212562);

		SetDirection(76);
		SetStand("giant/male/anim/giant_npc_krug");
	}
}
