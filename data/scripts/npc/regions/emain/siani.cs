using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class SianiScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_siani");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 29, eyeColor: 47, lip: 36);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0xF3C, 0xDB018A, 0xF36279, 0x6FBB);
		EquipItem(Pocket.Hair, 0xC2C, 0xD18B00, 0xD18B00, 0xD18B00);
		EquipItem(Pocket.Armor, 0x3C1B, 0xC5880B, 0x0, 0x765008);
		EquipItem(Pocket.Shoe, 0x4274, 0x292B35, 0x808080, 0x808080);
		EquipItem(Pocket.Head, 0x46F0, 0x252A2F, 0x808080, 0x808080);
		EquipItem(Pocket.RightHand1, 0x9E2A, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 52, x: 29109, y: 42874);

		SetDirection(197);
		SetStand("chapter4/human/male/anim/male_c4_npc_dollmaker02");
        
		Phrases.Add("Aren't the puppets wonderful?");
		Phrases.Add("Huw is a genius...");
		Phrases.Add("Huw's the best!");
		Phrases.Add("I hope the old boss gets well soon.");
		Phrases.Add("If only I could make puppets all day long...");
		Phrases.Add("Someday I'll be able to make puppets like this!");
	}
}
