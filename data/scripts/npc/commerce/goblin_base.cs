using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class CommerceGoblinScript : NPCScript
{
	public override void OnLoad()
	{
		SetRace(348);
		SetBody(height: 0.7f, fat: 1f, upper: 1f, lower: 1f);

		SetStand("chapter4/monster/anim/goblin/mon_c4_goblin_commerce", "chapter4/monster/anim/goblin/mon_c4_goblin_commerce_talk");
	
		Phrases.Add("Sell your goods for more in faraway towns.");
		Phrases.Add("Buy trade goods here and sell them in other towns.");
		Phrases.Add("Consider the profits while transporting!");
		Phrases.Add("One Ducat is all you need to receive VIP treatment!");
		Phrases.Add("Popular items will increase your profits.");
		Phrases.Add("Sell goods in towns not frequented by others!");
		Phrases.Add("Trading can earn you money!");
		Phrases.Add("Use a Letter of Guarantee to get better prices for your Trade Goods!");
	}

	public override void OnTalk(WorldClient c)
	{
		Disable(Options.Face | Options.Name);
		Msg(c, "Shuffling about with boxes and sacks of trade goods, this fellow seems too busy to bother with you.");
		Enable(Options.Face | Options.Name);
		MsgSelect(c, "Money is something you can never have too much of.<br/>How much did you earn?",
			"Trade", "@trade", "Repair Fomor Weapons", "@repair", "Commerce Explanation", "@explain", "Ducats", "@ducats", "End Conversation", "@end");
	}

	public override void OnSelect(WorldClient c, string r)
	{
		switch (r)
		{
			case "@ducats":
				Msg(c, "You do know that Gold is not the currence that we use, right?",
					"The kingdom came up with Ducats for commerce transactions.",
					"Use what you earn to purchase new items from the Imp.");
				break;
				
			case "@endcare":
				Msg(c, "Okay, take good care of it.");
				break;
				
			case "@explain":
				Msg(c, "Buy low, sell high!<br/>This is the number one principle of trading.", "You purchase trade goods and sell them in other towns.");
				Msg(c, "If you carry goods to far away places, or places where they are rare,",
					"you'll make quite a bit more. Always check the market price", "and head into the direction of the highest profit, but keep the travel times in mind.");
				Msg(c, "Of course, there's more to it than that.",
					"Bandits prowl the trade routes, looking to steal whatever they can.",
					"Listen to the guide Imp and be cautious, but your best bet",
					"is to always be ready for a fight.");
				Msg(c, "You'll start with a Backpack to carry goods, but you can get others from the Ogre.",
					"Y'know, instead of listening to me go on and on, try it yourself!",
					"Start by purchasing some goods from me to trade.");
				break;
				
			case "@repair":
				MsgSelect(c, "If it's a Fomor weapon, I can repair it.", "End Conversation", "@endcare");
				break;
				
			default:
				Msg(c, "Can we change the subject?");
				ShowKeywords(c);
				break;
		}
	}
	
	public override void OnEnd(WorldClient c)
	{
		Close(c, "(You have ended your conversation with the Trade Helper.)");
	}
}
