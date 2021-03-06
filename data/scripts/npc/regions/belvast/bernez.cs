using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class BernezScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_bernez");
		SetRace(25);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 26, eyeColor: 31, lip: 0);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x1324, 0x93005F, 0xC1BE60, 0xDC0073);
		EquipItem(Pocket.Hair, 0x100E, 0xFFF38C, 0xFFF38C, 0xFFF38C);
		EquipItem(Pocket.Armor, 0x3AD4, 0x0, 0xF7F3DE, 0xFFFFFF);
		EquipItem(Pocket.Shoe, 0x4299, 0x0, 0x9C5D42, 0x808080);
		EquipItem(Pocket.LeftHand1, 0xB3C7, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 4005, x: 29165, y: 40283);

		SetDirection(180);
		SetStand("chapter4/human/male/anim/male_c4_npc_priest");

		Phrases.Add("A sincere heart is more important than a big church.");
		Phrases.Add("Darkness and Light have always co-existed.");
		Phrases.Add("It is our dream to share the Light will all Fomors.");
		Phrases.Add("May the blessing of God be upon you.");
		Phrases.Add("One day, we will share the Light with them...");
	}
}
