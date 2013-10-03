using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class LucasScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_lucas");
		SetRace(10002);
		SetBody(height: 1.5f, fat: 1f, upper: 1.8f, lower: 1.3f);
		SetFace(skin: 20, eye: 17, eyeColor: 0, lip: 4);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0x1328, 0xF49D32, 0xCF005C, 0x7A613D);
		EquipItem(Pocket.Hair, 0xFF3, 0x22180F, 0x22180F, 0x22180F);
		EquipItem(Pocket.Armor, 0x3A9F, 0x100701, 0xC0501, 0x6D3C12);
		EquipItem(Pocket.Shoe, 0x4287, 0x210F0E, 0x6D0036, 0x4B0007);
		EquipItem(Pocket.RightHand2, 0x9C4B, 0xFFFFFF, 0xB0909, 0xFFFFFF);

		SetLocation(region: 57, x: 6900, y: 6090);

		SetDirection(165);
		SetStand("human/male/anim/male_natural_stand_npc_Ranald");
        
		Phrases.Add("*Cough*...");
		Phrases.Add("*Cough*... This is what I'm talking about.");
		Phrases.Add("Drink and be merry!");
		Phrases.Add("Hey! Behave...");
		Phrases.Add("If you want something special, let me know. *Chuckle*...");
		Phrases.Add("Interesting... I wonder what's going to happen today...");
		Phrases.Add("No problem. Have fun!");
		Phrases.Add("That is not funny at all. Get lost.");
		Phrases.Add("This is your lucky day. This round's on me.");
		Phrases.Add("This shop is as good as any of the ones in Tara, if not better.");
		Phrases.Add("We don't deal with any old fogies around here though.");
		Phrases.Add("You smell nice.");
	}
}
