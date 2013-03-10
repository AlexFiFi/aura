using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class CobhworkerScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_cobhworker");
		SetRace(123);
		SetBody(height: 1f, fat: 1.1f, upper: 1.1f, lower: 1f);
		SetFace(skin: 15, eye: 20, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1324, 0xFFF78A, 0x673F60, 0x747);
		EquipItem(Pocket.Hair, 0x1027, 0x585454, 0x585454, 0x585454);
		EquipItem(Pocket.Armor, 0x3ACB, 0xB5C8D0, 0x4F5251, 0x325757);
		EquipItem(Pocket.Shoe, 0x427C, 0x52697C, 0x888C00, 0x808080);
		EquipItem(Pocket.RightHand1, 0x9DFB, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 23, x: 33031, y: 37178);

		SetDirection(5);
		SetStand("");
        
		Phrases.Add("I need to find a way to make some money...");
		Phrases.Add("I need to work hard and save up...");
		Phrases.Add("I should be able to get a job once the dock is open, right?");
		Phrases.Add("I used to be really successful... Then the pirates came.");
		Phrases.Add("I want to set sail on the sea...");
		Phrases.Add("I'm not scared of those ruddy pirates!");
		Phrases.Add("The weather is so nice!");
		Phrases.Add("Those blasted pirates!");
		Phrases.Add("Unemployment is so frustrating!");
		Phrases.Add("When will the construction at the dock end?");
	}
}
