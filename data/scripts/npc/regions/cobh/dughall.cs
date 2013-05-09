// Aura Script
// --------------------------------------------------------------------------
// Dughall - Random Pirate
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;
using Aura.Shared.Util;

public class DughallScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_dughall");
		SetRace(8002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 82, eyeColor: 0, lip: 30);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x22C4, 0x362988, 0xF59D31, 0x2A55);
		EquipItem(Pocket.Hair, 0x1F5E, 0xEF9252, 0xEF9252, 0xEF9252);
		EquipItem(Pocket.Armor, 0x3BF6, 0x241919, 0xB79B8A, 0xA253E);
		EquipItem(Pocket.Shoe, 0x42F1, 0xA253E, 0x1E45AD, 0x89AD0F);
		EquipItem(Pocket.Head, 0x4740, 0xA253E, 0xFFFFFF, 0x316900);

		SetLocation(region: 23, x: 27862, y: 36669);

		SetDirection(133);
		SetStand("");
        
		Phrases.Add("I don't ever want to go back to that place.");
		Phrases.Add("I hope they'd stop now.");
		Phrases.Add("I must always ask for forgiveness.");
		Phrases.Add("I was a terrible person...");
		Phrases.Add("I wonder if those poor souls were ever released...");
		Phrases.Add("That wouldn't happen here, would it?");
		Phrases.Add("There should be no trading with them.");
		Phrases.Add("There was no mercy back then...");
		Phrases.Add("This is a peaceful town... I like it.");
		Phrases.Add("Will I ever be forgiven?");
	}
    
    public override IEnumerable OnTalk(WorldClient c)
    {
        switch(RandomProvider.Get().Next(0, 6))
        {
            case 0: Msg(c, "Someday, I will be forgiven."); break;
            case 1: Msg(c, "This is a peaceful place..."); break;
            case 2: Msg(c, "It was pretty dangerous where i used to live.<br/>You probably should stay away from me too."); break;
            case 3: Msg(c, "I regret the things... awful things...I've done.<br/>But now I'm trying to make up for."); break;
            case 4: Msg(c, "Sympathy is a luxury for pirates."); break;
            case 5: Msg(c, "Slaves are weak, pitiable people.<br/>It is our duty to rescue and protect them."); break;
            case 6: Msg(c, "Will you excuse me?<br/>I'd like to be alone right now..."); break;
        }
        End();
    }

}
