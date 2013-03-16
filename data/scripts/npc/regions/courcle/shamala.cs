using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class ShamalaScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_shamala");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 25, eye: 168, eyeColor: 22, lip: 2);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0xF55, 0xFFECB7, 0xFDBC47, 0x117B4B);
		EquipItem(Pocket.Hair, 0xC4F, 0x111124, 0x111124, 0x111124);
		EquipItem(Pocket.Armor, 0x3E6E, 0xCCA981, 0x1E1E1E, 0x808080);
		EquipItem(Pocket.Glove, 0x4224, 0xE68F2E, 0x808080, 0x808080);

		SetLocation(region: 3300, x: 251893, y: 186005);

		SetDirection(244);
		SetStand("chapter4/human/female/anim/female_c4_npc_wildwoman");
        
		Phrases.Add("...");
		Phrases.Add("Get out of my way.");
		Phrases.Add("What do you want?");
	}
}
