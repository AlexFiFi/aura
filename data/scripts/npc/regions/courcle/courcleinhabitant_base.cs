using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Courcleinhabitant_baseScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 25, eye: 4, eyeColor: 27, lip: 0);

		SetColor(0x808080, 0x808080, 0x808080);

		SetStand("");
        
		Phrases.Add("May Irinid bless you!");
		Phrases.Add("May the Great Spirit of Irinid bless you on the journey.");
		Phrases.Add("Oh, the Great Spirit...");
	}
}
