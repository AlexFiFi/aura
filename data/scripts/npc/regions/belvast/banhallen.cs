using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class BanhallenScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_banhallen");
		SetRace(10001);
		SetBody(height: 0.5f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 32, eyeColor: 98, lip: 1);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF3C, 0xC08A6B, 0xC8015B, 0xE3AA4B);
		EquipItem(Pocket.Hair, 0xBE3, 0xCC3300, 0xCC3300, 0xCC3300);
		EquipItem(Pocket.Armor, 0x3B71, 0xFF99CC, 0x424563, 0xFFCCCC);
		EquipItem(Pocket.Shoe, 0x42A0, 0x161616, 0x808080, 0xBA9AE2);

		SetLocation(region: 4005, x: 45560, y: 24520);

		SetDirection(12);
		SetStand("chapter4/human/female/anim/female_c4_npc_banallen");

		Phrases.Add("Hey! Stop by and say hello!");
		Phrases.Add("I can get whatever you need!");
		Phrases.Add("I can make any fish taste great!");
		Phrases.Add("I think I’ve come up with a new bisque!");
		Phrases.Add("I’m just the cook!");
		Phrases.Add("I'd love to travel the world one day.");
		Phrases.Add("Maybe you should slow down on the drinks.");
		Phrases.Add("My dad has got to be the best dad ever!");
		Phrases.Add("My dad wouldn't hurt a sandflea.");
		Phrases.Add("My General Store is open for business!");
		Phrases.Add("There’s no place in the world better than Belvast Island.");
		Phrases.Add("Welcome to the best pub in town!");
		Phrases.Add("Why are all these guys scared of my dad?");
	}
}
