using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class HeleddScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_heledd");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 27, eyeColor: 98, lip: 1);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF3C, 0x691F63, 0x820035, 0xFFCD9F);
		EquipItem(Pocket.Hair, 0xBD0, 0xFFCC66, 0xFFCC66, 0xFFCC66);
		EquipItem(Pocket.Armor, 0x3BF7, 0xFFD7B5, 0x4F5256, 0xFFFFFF);
		EquipItem(Pocket.Shoe, 0x42E8, 0xB5ECEF, 0x83838C, 0x2C273D);

		SetLocation(region: 432, x: 1647, y: 1261);

		SetDirection(6);
		SetStand("human/female/anim/female_stand_npc_emain_05");
	}
}
