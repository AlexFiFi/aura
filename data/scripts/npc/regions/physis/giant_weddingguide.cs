using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Giant_weddingguideScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_giant_weddingguide");
		SetRace(8002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 50, eyeColor: 126, lip: 29);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x22C4, 0x77CDC9, 0x8A0060, 0x85CA);
		EquipItem(Pocket.Hair, 0x1F50, 0x444449, 0x444449, 0x444449);
		EquipItem(Pocket.Armor, 0x3B6C, 0xACB4B6, 0x212628, 0x808080);
		EquipItem(Pocket.Shoe, 0x42C0, 0xACB4B6, 0x345695, 0x59BEFB);

		SetLocation(region: 3200, x: 290372, y: 213000);

		SetDirection(82);
		SetStand("giant/male/anim/giant_npc_wedding");
	}
}
