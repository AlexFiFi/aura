using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class BlythScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_blyth");
		SetRace(25);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 12, eyeColor: 27, lip: 0);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x1324, 0x132645, 0xFA9E50, 0xD6C75F);
		EquipItem(Pocket.Hair, 0xFB7, 0x776236, 0x776236, 0x776236);
		EquipItem(Pocket.Armor, 0x3B03, 0x9C5D42, 0x1C344E, 0x633C31);
		EquipItem(Pocket.Shoe, 0x4295, 0x21140C, 0x808080, 0x808080);
		EquipItem(Pocket.RightHand1, 0x9C58, 0x808080, 0x9C5D42, 0xE4D179);
		EquipItem(Pocket.LeftHand1, 0x9C4A, 0xA7A7A7, 0x495458, 0xFFFFFF);

		SetLocation(region: 4005, x: 45479, y: 21828);

		SetDirection(4);
		SetStand("chapter4/human/male/anim/male_c4_npc_weapon");

		Phrases.Add("*Shhkkkk shhkkk*");
		Phrases.Add("*Shhkkkk*");
		Phrases.Add("...");
		Phrases.Add("I will have my revenge...");
		Phrases.Add("I'll never let these blades dull...");
		Phrases.Add("I'm glad you appreciate my work.");
		Phrases.Add("What is it?");
	}
}
