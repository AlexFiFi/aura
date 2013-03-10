using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Castle_churchScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_castle_church");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1.2f, upper: 1f, lower: 1f);
		SetFace(skin: 21, eye: 14, eyeColor: 0, lip: 21);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1324, 0x5F4E62, 0x657009, 0x836600);
		EquipItem(Pocket.Hair, 0x100F, 0x663300, 0x663300, 0x663300);
		EquipItem(Pocket.Armor, 0x3BE6, 0x2D2F30, 0x585858, 0xFFFFFF);
		EquipItem(Pocket.Shoe, 0x42AA, 0x585858, 0x594FB9, 0x808080);

		SetLocation(region: 401, x: 104858, y: 112269);

		SetDirection(29);
		SetStand("");

		Phrases.Add("Are you disrespecting Lymilark?");
		Phrases.Add("Such thoughts are vile and do not do anyone justice.");
		Phrases.Add("That is blasphemy.");
	}
}
