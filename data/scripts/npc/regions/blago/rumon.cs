using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class RumonScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_rumon");
		SetRace(10002);
		SetBody(height: 0.8000003f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 1, eyeColor: 27, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1324, 0xEBA443, 0x986307, 0xF57648);
		EquipItem(Pocket.Hair, 0xFF5, 0x827815, 0x827815, 0x827815);
		EquipItem(Pocket.Armor, 0x3C27, 0x81939D, 0x223240, 0x5789EC);
		EquipItem(Pocket.Shoe, 0x42AA, 0x202121, 0x75BFF5, 0x808080);

		SetLocation(region: 402, x: 24078, y: 30176);

		SetDirection(192);
		SetStand("");
        
		Phrases.Add("I wonder how Edana is doing.");
		Phrases.Add("I wonder if I could ever become a Wine Hero.");
		Phrases.Add("I wonder what caused Lezarro and Eluned to have such a bad relationship.");
		Phrases.Add("So many people are suddenly interested in wine.");
		Phrases.Add("This year's grapes are really looking good.");
	}
}
