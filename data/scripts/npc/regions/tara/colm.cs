using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class ColmScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_colm");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 12, eyeColor: 162, lip: 0);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x1324, 0x19A7F, 0xFBA852, 0xF35F47);
		EquipItem(Pocket.Hair, 0xFA3, 0x80B6CB, 0x80B6CB, 0x80B6CB);
		EquipItem(Pocket.Armor, 0x3B17, 0xFFE3B5, 0x595F49, 0xACACAC);
		EquipItem(Pocket.Shoe, 0x4291, 0xBD9B55, 0x676058, 0x808080);

		SetLocation(region: 435, x: 1775, y: 1720);

		SetDirection(161);
		SetStand("human/male/anim/male_natural_stand_npc_Duncan");

		Phrases.Add("If you are looking for weapons or armors, come speak to me.");
	}
}
