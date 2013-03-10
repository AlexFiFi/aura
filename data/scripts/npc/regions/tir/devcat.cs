// Aura Script
// --------------------------------------------------------------------------
// Weird Cat
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class DevcatScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_devcat");
		SetRace(100);
		SetStand("monster/anim/devcat/devcat_stand_friendly");
		SetLocation("tir_duncan", 2198, 1243, 31);

		AddPhrases("Meeow", "Meoooow.", "Meow.");
	}

	public override IEnumerable OnTalk(WorldClient c)
	{
		MsgSelect(c, "Meeeoow", "End Conversation", "@end");
		End();
	}
}
