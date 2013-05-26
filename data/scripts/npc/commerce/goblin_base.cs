// Aura Script
// --------------------------------------------------------------------------
// Commerce Goblin Base
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class CommerceGoblinScript : NPCScript
{
	public override void OnLoad()
	{
		SetRace(348);
		SetBody(height: 0.7f, fat: 1f, upper: 1f, lower: 1f);
		SetStand("chapter4/monster/anim/goblin/mon_c4_goblin_commerce", "chapter4/monster/anim/goblin/mon_c4_goblin_commerce_talk");
        
        EquipItem(Pocket.RightHand1, 0x9DF4, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.LeftHand1, 0xB3C7, 0x808080, 0x808080, 0x808080);

		Phrases.Add("Sell your goods for more in faraway towns.");
		Phrases.Add("Buy trade goods here and sell them in other towns.");
		Phrases.Add("Consider the profits while transporting!");
		Phrases.Add("One Ducat is all you need to receive VIP treatment!");
		Phrases.Add("Popular items will increase your profits.");
		Phrases.Add("Sell goods in towns not frequented by others!");
		Phrases.Add("Trading can earn you money!");
		Phrases.Add("Use a Letter of Guarantee to get better prices for your Trade Goods!");
	}

	public override IEnumerable OnTalk(WorldClient c)
	{
		Msg(c, Options.FaceAndName, "Shuffling about with boxes and sacks of trade goods, this fellow seems too busy to bother with you.");
		Msg(c,
			"Money is something you can never have too much of.<br/>How much did you earn?",
			Button("Trade"), Button("Repair Fomor Weapons", "@repair"), Button("Commerce Explanation", "@explain"),
			Button("Ducats"), Button("End Conversation", "@end")
		);
		
		var r = Select(c);
		switch (r)
		{
			case "@trade":
			{
				Msg(c, "(Unimplemented.)");
				End();
			}
				
			case "@repair":
			{
				Msg(c, "If it's a Fomor weapon, I can repair it.", Button("End Conversation", "@endcare"));
				Msg(c, "Okay, take good care of it.");
				End();
			}
			
			case "@explain":
			{
				Msg(c, "Buy low, sell high!<br/>This is the number one principle of trading.<br/>You purchase trade goods and sell them in other towns.");
				Msg(c, "If you carry goods to far away places, or places where they are rare,<br/>you'll make quite a bit more. Always check the market price<br/>and head into the direction of the highest profit, but keep the travel times in mind.");
				Msg(c, "Of course, there's more to it than that.<br/>Bandits prowl the trade routes, looking to steal whatever they can.<br/>Listen to the guide Imp and be cautious, but your best bet<br/>is to always be ready for a fight.");
				Msg(c, "You'll start with a Backpack to carry goods, but you can get others from the Ogre.<br/>Y'know, instead of listening to me go on and on, try it yourself!<br/>Start by purchasing some goods from me to trade.");
				End();
			}
			
			case "@ducats":
			{
				Msg(c, "You do know that Gold is not the currency that we use, right?<br/>The kingdom came up with Ducats for commerce transactions.<br/>Use what you earn to purchase new items from the Imp.");
				End();
			}
		}
	}
	
	public override void OnEnd(WorldClient c)
	{
		Close(c, "(You have ended your conversation with the Trade Helper.)");
	}
}
