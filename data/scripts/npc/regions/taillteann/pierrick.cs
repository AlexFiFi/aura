using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class PierrickScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_pierrick");
		SetRace(47);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 17, eye: 0, eyeColor: 0, lip: 0);

		SetColor(0x808080, 0x660066, 0xF6E07D);

		EquipItem(Pocket.RightHand1, 0x9C6E, 0xBB2222, 0x38961F, 0x0);

		SetLocation(region: 300, x: 221497, y: 191138);

		SetDirection(216);
		SetStand("");
        
		Phrases.Add("Did I remember to feed the pigs?");
		Phrases.Add("I don't even feel like doing my rose trick since I don't have an audience.");
		Phrases.Add("I haven't seen Brenda all day today.");
		Phrases.Add("I really need to take a trip somewhere.");
		Phrases.Add("The glow of the sunset tonight indicates a clear day for tommorow!");
		Phrases.Add("The halo in the western sky must mean it's going to rain tomorrow!");
		Phrases.Add("The people in Taillteann just don't seem to care about looking good. I can't understand it!");

	}
}
