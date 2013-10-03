using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class PencastScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_pencast");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 0, eyeColor: 126, lip: 0);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x133A, 0xF49C33, 0x737171, 0xDD7785);
		EquipItem(Pocket.Hair, 0x100D, 0xADAAA5, 0xADAAA5, 0xADAAA5);
		EquipItem(Pocket.Armor, 0x3C1D, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Shoe, 0x4299, 0xEFE3B5, 0x660033, 0x808080);
		EquipItem(Pocket.Head, 0x4747, 0x808080, 0x4BA2E8, 0x8B2B2B);
		EquipItem(Pocket.RightHand1, 0x9D62, 0x808080, 0x808080, 0x156245);

		SetLocation(region: 401, x: 87883, y: 82244);

		SetDirection(255);
		SetStand("human/male/anim/male_natural_stand_npc_Duncan");

		Phrases.Add("Hello. My name is Cardinal Pencast.");
		Phrases.Add("I forgot to prepare the Blessing Water.");
		Phrases.Add("May the blessings of Lymilark be with you.");
		Phrases.Add("Oh no, I forgot my prayer time... I must be getting old.");
		Phrases.Add("May the light of Lymilark be with you.");
	}
}
