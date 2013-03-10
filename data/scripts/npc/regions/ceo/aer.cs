using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class AerScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_aer");
		SetRace(19);
		SetBody(height: 1.3f, fat: 1f, upper: 1f, lower: 1.2f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		SetLocation(region: 68, x: 5599, y: 8550);

		SetDirection(192);
		SetStand("");

		Phrases.Add(".....");
		Phrases.Add("How do I look...?");
		Phrases.Add("I hear the water's grieving...");
		Phrases.Add("This place is pretty cozy...for a spirit...");
		Phrases.Add("Why do people...");
	}
}
