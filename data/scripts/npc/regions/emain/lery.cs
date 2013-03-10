using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class LeryScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_lery");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 16, eye: 0, eyeColor: 139, lip: 1);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1324, 0xC5AA73, 0x77C479, 0x82B28E);
		EquipItem(Pocket.Hair, 0x1773, 0xB97312, 0xB97312, 0xB97312);
		EquipItem(Pocket.Armor, 0x3C76, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Glove, 0x3EB3, 0x634C22, 0x808080, 0x808080);
		EquipItem(Pocket.Shoe, 0x4319, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 52, x: 31251, y: 43891);

		SetDirection(164);
		SetStand("");
        
		Phrases.Add("(He gazes off into space.)");
		Phrases.Add("*Yawn!*");
		Phrases.Add("Duty's duty, I suppose.");
		Phrases.Add("Is it time for a part-time job yet?");
		Phrases.Add("So! Bored!");
		Phrases.Add("Someday, this assignment will end...");
	}
}
