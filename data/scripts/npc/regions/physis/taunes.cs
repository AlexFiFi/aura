using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class TaunesScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_taunes");
		SetRace(31);
		SetBody(height: 1.2f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.RightHand1, 0x24B29, 0x704215, 0x735858, 0x6E6B69);

		SetLocation(region: 3200, x: 297463, y: 217702);

		SetDirection(153);
		SetStand("giant/male/anim/giant_npc_taunes");
	}
}
