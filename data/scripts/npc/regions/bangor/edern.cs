using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class EdernScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_edern");
		SetRace(10002);
		SetBody(height: 1.3f, fat: 1.4f, upper: 2f, lower: 1.4f);
		SetFace(skin: 25, eye: 9, eyeColor: 38, lip: 2);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1328, 0xF89A47, 0xF9B6CA, 0xF89947);
		EquipItem(Pocket.Hair, 0xFBC, 0xC0BC92, 0xC0BC92, 0xC0BC92);
		EquipItem(Pocket.Armor, 0x3ABF, 0x9B5033, 0x9A835F, 0x321007);
		EquipItem(Pocket.Glove, 0x407E, 0xEABE7D, 0x808080, 0x57685E);
		EquipItem(Pocket.Shoe, 0x4460, 0x2B1C09, 0x857756, 0x321007);
		EquipItem(Pocket.RightHand1, 0x9C58, 0xFACB5F, 0x4F3C26, 0xFAB052);

		SetLocation(region: 31, x: 10972, y: 13373);

		SetDirection(76);
		SetStand("human/male/anim/male_natural_stand_npc_edern");
	}
}
