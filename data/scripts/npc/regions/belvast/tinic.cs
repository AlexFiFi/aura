using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class TinicScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_tinic");
		SetRace(146);
		SetBody(height: 0.1f, fat: 1.4f, upper: 1.3f, lower: 1.1f);
		SetFace(skin: 16, eye: 0, eyeColor: 37, lip: 1);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x1324, 0x5ACAE4, 0x86578C, 0x6F73);
		EquipItem(Pocket.Hair, 0x1779, 0x99CC00, 0x99CC00, 0x99CC00);
		EquipItem(Pocket.Armor, 0x3AD6, 0x9A555E, 0xE3E3E3, 0xF3DCBE);
		EquipItem(Pocket.Shoe, 0x42F7, 0x605F5B, 0x587F64, 0x60C3C7);
		EquipItem(Pocket.Head, 0x46EF, 0x5F6D3C, 0xB5D2E1, 0x90A2EB);
		EquipItem(Pocket.RightHand1, 0x9C42, 0x7B2C10, 0x5E7F00, 0x4D98F9);

		SetLocation(region: 4005, x: 51930, y: 34262);

		SetDirection(4);
		SetStand("chapter4/human/female/anim/female_c4_npc_lonnie_stand3");

		Phrases.Add("Everyone retreat!");
		Phrases.Add("Haha, catch me if you can!");
		Phrases.Add("Hyah! Bring it on!");
		Phrases.Add("I'll take over this area next! Mwahahaha!");
		Phrases.Add("Shh, please keep this a secret.");
		Phrases.Add("Stop right there!");
	}
}
