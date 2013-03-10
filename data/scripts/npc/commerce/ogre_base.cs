// Aura Script
// --------------------------------------------------------------------------
// Commerce Ogre Base
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class CommerceOgreScript : NPCScript
{
	public override void OnLoad()
	{
		SetRace(323);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetStand("chapter4/monster/anim/ogre/ogre_c4_npc_commerce", "chapter4/monster/anim/ogre/ogre_c4_npc_commerce_talk");

		EquipItem(Pocket.RightHand1, 0xA03B, 0x808080, 0x808080, 0x808080);

		Phrases.Add("Belly hunger!");
		Phrases.Add("Hungry, hungry Ogre!");
		Phrases.Add("Imp also say not eat elephant. Silly imp, what Ogre supposed to eat?!");
		Phrases.Add("Imp say not eat horse. What imp know?");
		Phrases.Add("Meat? Meat. Meat!");
		Phrases.Add("Movement speed potion make elephant run fast as horse, but make them taste bad too.");
	}

	public override IEnumerable OnTalk(WorldClient c)
	{
		Msg(c, Options.FaceAndName, "It has innocent eyes, but it's gobbling meat like a starving lion.");
		MsgSelect(c,
			"I have a... H-handcart, and a Wagon.<br/>Oh, and a Pack Elephant, too!<br/>Ogre has big hands, but Ogre can repair also! Heh heh.",
			"Repair Fomor Weapons", "@repair", "End Conversation", "@end"
		);
			
		var r = Wait();
		if(r == "@repair")
		{
			MsgSelect(c, "Ogre no good at repairs.<br/>But repair cost cheap.<br/>I only take enough money to buy meat. Heh heh.", "End Conversation", "@endmeat");
			r = Wait();
			Msg(c, "Take good care of equipment. As valuable as meat.");
			End();
		}
	}
	
	public override void OnEnd(WorldClient c)
	{
		Close(c, "(You have ended your conversation with the Transportation Helper.)");
	}
}
