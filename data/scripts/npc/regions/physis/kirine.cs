using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class KirineScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_kirine");
		SetRace(27);
		SetBody(height: 2.5f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.RightHand1, 0x9DA5, 0x704215, 0x735858, 0x6E6B69);

		SetLocation(region: 3200, x: 289702, y: 212703);

		SetDirection(82);
		SetStand("giant/female/anim/giant_npc_kirine");
	}
}
