using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Imp_kuruScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_imp_kuru");
		SetRace(321);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 21, eye: 3, eyeColor: 7, lip: 2);

		NPC.ColorA = 0x4C655C;
		NPC.ColorB = 0xAB8E8E;
		NPC.ColorC = 0x35503D;		



		SetLocation(region: 4005, x: 33488, y: 40812);

		SetDirection(104);
		SetStand("chapter4/monster/anim/imp/imp_c4_npc_diet");
	}
}
