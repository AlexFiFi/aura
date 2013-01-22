// Aura Script
// --------------------------------------------------------------------------
// Commerce Imp Base
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Common.Constants;
using Common.World;
using World.Network;
using World.Scripting;
using World.World;

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
		Phrases.Add("I wish I could see the world...");
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
			"Trade", "@shop", "Trade In Bandit Badges", "@trade", 
			"Exchange Bandit Badges","@exchange", "Repair Fomor Weapons",
			"@repair", "Ferment wine", "@ferment", "End Conversation", "@end"
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
				MsgSelect(c, "So, were you able to catch plenty of those bandits?<br/>I'll give you some Ducats for proof that you took care of them.", "End Conversation", "@endtrade");
				MsgSelect(c, "I'll be seeing you, then.", "Continue", "@end");
				End();
			}
				
			case "@exchange":
			{
				MsgSelect(c, "So, were you able to catch plenty of those bandits?<br/>Hey, if you're sick of carrying all those badges, I'll trade you<br/>for a better one. I know how heavy they can get.", "End Conversation", "@endexchange");
				MsgSelect(c, "I'll be seeing you, then.", "Continue", "@end");
				End();
			}
				
			case "@repair":
			{
				MsgSelect(c, "If it's a Fomor weapon, just leave it to me.",  "Continue", "@endme");
				MsgSelect(c, "If it breaks again, come to me for repairs. Just me. Not the others. Me.", "Continue", "@end");
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
