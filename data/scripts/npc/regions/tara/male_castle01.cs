using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Male_castle01Script : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_male_castle01");
		SetRace(10002);
		SetBody(height: 0.9f, fat: 1f, upper: 1f, lower: 0.9f);
		SetFace(skin: 18, eye: 26, eyeColor: 82, lip: 1);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x1324, 0x472789, 0x35003D, 0xAAB06D);
		EquipItem(Pocket.Hair, 0x1775, 0xE6BF95, 0xE6BF95, 0xE6BF95);
		EquipItem(Pocket.Armor, 0x3C27, 0x85AC1C, 0x284A68, 0x6583AE);
		EquipItem(Pocket.Shoe, 0x42A1, 0x374F58, 0x4C7EA7, 0x808080);

		SetLocation(region: 401, x: 108200, y: 110710);

		SetDirection(226);
		SetStand("human/male/anim/male_stand_Tarlach_anguish");

		Phrases.Add("Ah, a lonely life.");
		Phrases.Add("Life and death are both meaningless...");
		Phrases.Add("This won't make her come back...");
	}
}
