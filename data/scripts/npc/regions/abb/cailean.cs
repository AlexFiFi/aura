using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class CaileanScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_cailean");
		SetRace(10002);
		SetBody(height: 0.1000001f, fat: 1.2f, upper: 1.3f, lower: 1.1f);
		SetFace(skin: 21, eye: 18, eyeColor: 176, lip: 25);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x132C, 0xC20053, 0x69696D, 0x38B65);
		EquipItem(Pocket.Hair, 0x2328, 0x4A1E01, 0x4A1E01, 0x4A1E01);
		EquipItem(Pocket.Armor, 0x3A98, 0xADAEC6, 0x3399, 0x4B6A73);
		EquipItem(Pocket.RightHand1, 0x9C41, 0x546C74, 0xB604, 0x4AB94D);

		SetLocation(region: 302, x: 125940, y: 85570);

		SetDirection(105);
		SetStand("chapter4/human/female/anim/female_c4_npc_lonnie_stand3");
        
		Phrases.Add("I want a puppy...");
		Phrases.Add("I'm bored!");
		Phrases.Add("Woo-hoo! Run!");
		Phrases.Add("Yeah?");
	}
}
