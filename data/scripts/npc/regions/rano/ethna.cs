using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class EthnaScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_ethna");
		SetRace(10001);
		SetBody(height: 0.7099999f, fat: 1f, upper: 1.02f, lower: 1f);
		SetFace(skin: 17, eye: 32, eyeColor: 26, lip: 1);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0xF3C, 0x6D6E53, 0xDF6843, 0x18AC4);
		EquipItem(Pocket.Hair, 0xBEB, 0xFFF29A4A, 0xFFF29A4A, 0xFFF29A4A);
		EquipItem(Pocket.Armor, 0x3B38, 0xFFC10B0B, 0xFF7B2C10, 0xFFFFD7B5);
		EquipItem(Pocket.Shoe, 0x429C, 0xFF7B2C10, 0xFFEF9252, 0xFFFFF38C);

		SetLocation(region: 3001, x: 164140, y: 169589);

		SetDirection(9);
		SetStand("human/female/anim/female_natural_stand_npc_Nora");
        
		Phrases.Add("Alright! It's you against me!");
		Phrases.Add("And if he's a customer, it's all about guts!");
		Phrases.Add("It's all about your spirit! Spirit! Use your spirit to make things happen! Push! Push to save!");
	}
}
