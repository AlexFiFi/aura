// Aura Script
// --------------------------------------------------------------------------
// Commerce Imp Base
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class CommerceImpScript : NPCScript
{
	public override void OnLoad()
	{
		SetRace(321);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetStand("chapter4/monster/anim/imp/mon_c4_imp_commerce", "chapter4/monster/anim/imp/mon_c4_imp_commerce_talk");
	
		Shop.AddTabs("General", "Fomor Weapons", "Limited Time Only", "Quest", "Wine Ingredients");
		
		Phrases.Add("Ah, you ran out of money. So sad.");
		Phrases.Add("Get your Fomor weapons, right here.");
		Phrases.Add("Greetings!");
		Phrases.Add("I wish I could see the Aura.World...");
		Phrases.Add("I'll exchange your Bandit Badges for Ducats.");
		Phrases.Add("Try buying a more powerful Fomor weapon.");
		Phrases.Add("Use a Letter of Guarantee to earn Ducats immediately!");
		Phrases.Add("Welcome!");
		Phrases.Add("What will you do with all that money? Use it here!");
	}

	public override IEnumerable OnTalk(WorldClient c)
	{
		MsgSelect(c,
			"Your wine is aging, your wine is aging!<br/>Buy interesting goods and trade in Bandit Badges!",
			Button("Trade", "@shop"), Button("Trade In Bandit Badges", "@trade"), 
			Button("Exchange Bandit Badges","@exchange"), Button("Repair Fomor Weapons",
			"@repair"), Button("Ferment wine", "@ferment"), Button("End Conversation", "@end")
		);
		
		var r = Wait();
		switch (r)
		{
			case "@shop":
			{
				Msg(c, "What are you looking for?");
				OpenShop(c);
				End();
			}
				
			case "@trade":
			{
				MsgSelect(c, "So, were you able to catch plenty of those bandits?<br/>I'll give you some Ducats for proof that you took care of them.", Button("End Conversation", "@endtrade"));
				MsgSelect(c, "I'll be seeing you, then.", Button("Continue", "@end"));
				End();
			}
				
			case "@exchange":
			{
				MsgSelect(c, "So, were you able to catch plenty of those bandits?<br/>Hey, if you're sick of carrying all those badges, I'll trade you<br/>for a better one. I know how heavy they can get.", Button("End Conversation", "@endexchange"));
				MsgSelect(c, "I'll be seeing you, then.", Button("Continue", "@end"));
				End();
			}
				
			case "@repair":
			{
				MsgSelect(c, "If it's a Fomor weapon, just leave it to me.",  Button("Continue", "@endme"));
				MsgSelect(c, "If it breaks again, come to me for repairs. Just me. Not the others. Me.", Button("Continue", "@end"));
				End();
			}
				
			case "@ferment":
			{
				Msg(c, "(Unimplmented)");
				End();
			}
		}
	}
	
	public override void OnEnd(WorldClient c)
	{
		Close(c, "(You have ended your conversation with the Trade Assistant Imp.)");
	}
}
