// Aura Script
// --------------------------------------------------------------------------
// Commerce Elephant Base
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class CommerceElephantScript : NPCScript
{
	public override void OnLoad()
	{
		SetRace(377);
		SetBody(height: 2f, fat: 1f, upper: 1f, lower: 1f);

		Phrases.Add("Bhoo hoo...");
		Phrases.Add("Bhoo!");
		Phrases.Add("Bhoo! Bhoo!");
		Phrases.Add("Boo!");
		Phrases.Add("Boooo!");
	}

	public override IEnumerable OnTalk(WorldClient c)
	{
		Msg(c, Options.FaceAndName, "It has been trumpeting and shuffling non-stop.<br/>As it stomps the ground with its giant feel, it bellows again.<br/>Now, it stares softly at you with its two innocent eyes.");
		Msg(c, "Boooo?<br/>Bhoo!<p/>Bhooo!<br/>Bhoo, bhoooo!");
		End();
	}
}
