using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class GenieScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_genie");
		SetRace(145);
		SetBody(height: 0.3f, fat: 1.1f, upper: 1.1f, lower: 1.1f);
		SetFace(skin: 15, eye: 30, eyeColor: 27, lip: 1);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0xF3C, 0xFCBE56, 0x5A5A66, 0x496D4A);
		EquipItem(Pocket.Hair, 0xC20, 0x8C5973, 0x8C5973, 0x8C5973);
		EquipItem(Pocket.Armor, 0x3B46, 0xCC99CC, 0x73555A, 0x535D71);
		EquipItem(Pocket.Shoe, 0x4494, 0xC4AAAD, 0xADA659, 0x6D133C);
		EquipItem(Pocket.RightHand1, 0x9C42, 0x854E2D, 0x96DFC5, 0x78AD05);

		SetLocation(region: 4005, x: 51909, y: 31253);

		SetDirection(63);
		SetStand("chapter4/human/female/anim/female_c4_npc_lonnie_stand");

		Phrases.Add("Ha ha, how exciting!");
		Phrases.Add("I'm going to become a great warrior!");
		Phrases.Add("I'm practicing fencing right now.");
		Phrases.Add("We're gonna get in trouble again...let's be careful.");
		Phrases.Add("Whew, this sure is hard.");
		Phrases.Add("You want to try fighting me?");
	}
}
