using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class CastaneaScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_castanea");
		SetRace(9001);
		SetBody(height: 1.2f, fat: 1f, upper: 1f, lower: 1.1f);
		SetFace(skin: 18, eye: 39, eyeColor: 3, lip: 2);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x170C, 0xCBA0C8, 0xB97B7E, 0x53686D);
		EquipItem(Pocket.Hair, 0x138B, 0x926287, 0x926287, 0x926287);
		EquipItem(Pocket.Armor, 0x3B41, 0xFF808080, 0xFF07497A, 0xFF07497A);
		EquipItem(Pocket.Shoe, 0x42A9, 0xD0B18, 0xB0506, 0x947200);
		EquipItem(Pocket.Robe, 0x4A42, 0x2C0309, 0x0, 0x611532);
        
        NPC.GetItemInPocket(Pocket.Robe).Info.FigureA = 1;

		SetLocation(region: 3100, x: 379510, y: 425770);

		SetDirection(149);
		SetStand("elf/female/anim/elf_npc_castanea_stand_friendly");
        
		Phrases.Add("A long dark winter is beginning...");
		Phrases.Add("Don't be afraid of the sun going down and the night beginning.");
		Phrases.Add("Everything will start getting fuzzier like sand dust.");
		Phrases.Add("Memories are like water passing through your fingers, they flow away.");
		Phrases.Add("Memories will fade with time...");
		Phrases.Add("Sheep's wool that hasn't been combed, and sheep that aren't ready for shearing...");
		Phrases.Add("The heart falls into darkness so easily.");
		Phrases.Add("When your memories are gone, all the hurt and mistakes will disappear with it.");
		Phrases.Add("You should remember that trying so hard to reach for the stars, you may find yourself missing out on the little flower petal right in front of your toes.");
	}
}
