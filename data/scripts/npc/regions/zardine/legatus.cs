using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class LegatusScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_legatus");
		SetRace(37);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);

		SetColor(0x808080, 0x808080, 0x808080);

		SetLocation(region: 3400, x: 322199, y: 203607);

		SetDirection(170);
		SetStand("monster/anim/dragon/dragon_standing_friendly_02");
        
		Phrases.Add("...");
		Phrases.Add("...And all light disappears.");
		Phrases.Add("Grrr...");
		Phrases.Add("It is time.");
		Phrases.Add("The shackles of the burning land has been broken and lifeless soil is seeping through.");
		Phrases.Add("When the feather of the large bird brings upon darkness on the land");
	}
}
