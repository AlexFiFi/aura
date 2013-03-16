using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class LepusScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_lepus");
		SetRace(9002);
		SetBody(height: 0.9f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 36, eyeColor: 91, lip: 1);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0x1AF4, 0x5D2661, 0x6E5F65, 0xA12C91);
		EquipItem(Pocket.Hair, 0x1775, 0x48B4D6, 0x48B4D6, 0x48B4D6);
		EquipItem(Pocket.Armor, 0x3B40, 0xFFA72B, 0x182C18, 0x112320);
		EquipItem(Pocket.Shoe, 0x427D, 0xAE820E, 0x630837, 0xEEE5B7);
		EquipItem(Pocket.Head, 0x46CF, 0x86E360, 0xAC9ACB, 0xB1A301);

		SetLocation(region: 3100, x: 363093, y: 426159);

		SetDirection(200);
		SetStand("elf/male/anim/elf_npc_lepus_stand_friendly");
        
		Phrases.Add("I don't know if you'd like this but I made it myself.");
		Phrases.Add("I should be getting used to it by now.");
		Phrases.Add("I'm generally quite shy.");
		Phrases.Add("The clouds are looking so pretty today.");
		Phrases.Add("The spinning wheel is turning and turning.");
		Phrases.Add("What's there to think about?");
		Phrases.Add("When the sun is out and it's warm, I feel much better.");
	}
}
