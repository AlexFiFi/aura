using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class GuardmerchantScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_guardmerchant");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 32, eyeColor: 170, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF3C, 0xF46369, 0xF8A43E, 0xEBA442);
		EquipItem(Pocket.Hair, 0xBCA, 0x2C0000, 0x2C0000, 0x2C0000);
		EquipItem(Pocket.Armor, 0x3BED, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 401, x: 82364, y: 122028);

		SetDirection(118);
		SetStand("");

		Phrases.Add("... I want some more sleep.");
		Phrases.Add("Are you here to purchase a weapon?");
		Phrases.Add("I'm going to be an awesome soldier like Padan.");
		Phrases.Add("Yaaawwn. Is it morning already?");
	}
}
