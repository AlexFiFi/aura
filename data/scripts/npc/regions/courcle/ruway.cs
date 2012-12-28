using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class RuwayScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_ruway");
		SetRace(32);
		SetBody(height: 0.6f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0xFF9423;
		NPC.ColorB = 0xC5B494;
		NPC.ColorC = 0xFF5837;		


		SetLocation(region: 3300, x: 257130, y: 183500);

		SetDirection(103);
		SetStand("");
        
		Phrases.Add("Kkyrrng Kkyrrng");
		Phrases.Add("Kkyrrr.");
		Phrases.Add("Kyrr.");
		Phrases.Add("Kyrrrr.");
	}
}
