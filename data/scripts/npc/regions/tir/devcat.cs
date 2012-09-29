using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class DevcatScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_devcat");
		SetRace(100);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x72672E;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		



		SetLocation(region: 3, x: 2198, y: 1243);

		SetDirection(31);
		SetStand("monster/anim/devcat/devcat_stand_friendly");

		Phrases.Add("Meeow");
		Phrases.Add("Meoooow.");
		Phrases.Add("Meow.");

	}

	public override void OnTalk(WorldClient c)
	{
		//TODO: Generation flags
		MsgSelect(c, "Meeeoow", "End Conversation", "@end");
	}

	public override void OnSelect(WorldClient c, string r)
	{
		switch (r)
		{
			default:
				Msg(c, "Can we change the subject?");
				ShowKeywords(c);
				break;
		}
	}
}
