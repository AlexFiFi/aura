using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class BrianaScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_briana");
		SetRace(10001);
		SetBody(height: 0.6999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 18, eye: 102, eyeColor: 192, lip: 1);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0xF3C, 0x534C, 0xAC003B, 0xE74D2A);
		EquipItem(Pocket.Hair, 0xBC5, 0xD97849, 0xD97849, 0xD97849);
		EquipItem(Pocket.Armor, 0x3B1F, 0x34865D, 0x2F4F39, 0xDCD8CD);
		EquipItem(Pocket.Shoe, 0x42AE, 0x8AA6AF, 0x808080, 0x808080);

		SetLocation(region: 416, x: 4794, y: 4799);

		SetDirection(63);
		SetStand("human/female/anim/female_natural_stand_npc_Aeira");
	}
}
