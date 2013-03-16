using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class WovokaScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_wovoka");
		SetRace(10002);
		SetBody(height: 1.3f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 25, eye: 0, eyeColor: 0, lip: 0);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x1330, 0xF8BF58, 0x76001E, 0xB73525);
		EquipItem(Pocket.Hair, 0xFFC, 0x3A322F, 0x3A322F, 0x3A322F);
		EquipItem(Pocket.Armor, 0x3B89, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Head, 0x46F8, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 3300, x: 246740, y: 187670);

		SetDirection(189);
		SetStand("human/male/anim/male_natural_stand_npc_yoff");
        
		Phrases.Add("I can't skip archery practice, not even for one day.");
		Phrases.Add("I fight for the honor of my tribe and the Great Spirit.");
		Phrases.Add("I want to become a great warrior.");
		Phrases.Add("I want to become strong.");
		Phrases.Add("I'm Waboka, the Courcle Warrior.");
		Phrases.Add("Jackals never lose their calm, even if they're ambushed by enemies.");
		Phrases.Add("May the Great Spirit of Irinid be with you.");
		Phrases.Add("Warriors must know what to do in the critical moment of battle.");
	}
}
