// Aura Script
// --------------------------------------------------------------------------
// Sion - Furnace/Windmill Boy
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class SionScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_sion");
		SetRace(10002);
		SetBody(height: 0.1000001f, fat: 1f, upper: 1.3f, lower: 1.3f);
		SetFace(skin: 17, eye: 2, eyeColor: 27, lip: 3);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0x1324, 0xF58A78, 0xCDD0EA, 0xB5E3E5);
		EquipItem(Pocket.Hair, 0xFA8, 0x2E4830, 0x2E4830, 0x2E4830);
		EquipItem(Pocket.Armor, 0x3AC4, 0x54697A, 0xCAD98C, 0x1F2E26);
		EquipItem(Pocket.Glove, 0x3E80, 0xAFA992, 0xB60659, 0xE1D5E9);
		EquipItem(Pocket.Shoe, 0x4274, 0x676149, 0x71485B, 0xF78A3D);
		EquipItem(Pocket.Head, 0x4668, 0x808000, 0xFFFFFF, 0xAA89C0);
		EquipItem(Pocket.RightHand1, 0x9C59, 0xC0C6BB, 0x8E6D59, 0xC7B0D5);

		SetLocation(region: 31, x: 12093, y: 15062);

		SetDirection(184);
		SetStand("human/anim/tool/Rhand_A/female_tool_Rhand_A02_stand_friendly");
        
		Phrases.Add("Dad should be coming any minute now...");
		Phrases.Add("I want to grow up quickly and be an adult soon.");
		Phrases.Add("I wonder what's for dinner. *Gulp*");
		Phrases.Add("Ibbie... I miss you...");
		Phrases.Add("If you want to activate the switch by the Watermill, let me know!");
		Phrases.Add("If you want to make an ingot, talk to me first!");
		Phrases.Add("If you want to refine ore, you have to come talk to me!");
		Phrases.Add("The Watermill never gets boring...");
		Phrases.Add("The way Gilmore talks is too hard to understand.");
		Phrases.Add("To fire up the furnace, come talk to me!");
		Phrases.Add("Why does Bryce not like me?");
		Phrases.Add("You have to pay. You have to pay to activate the switch!");
	}
    public override IEnumerable OnTalk(WorldClient c)
    {
        Msg(c, Options.FaceAndName, "Wearing a sturdy overall over his pale yellow shirt, this boy has soot and dust all over his face, hands, and clothes.<br/>His short and stubby fingers are quite calloused, and he repeatedly rubs his hands on the bulging pocket of his pants.<br/>His dark green hair is so coarse that even his hair band can't keep it neat. But between his messy hair, his brown sparkly eyes shine bright with curiosity.");
        Msg(c, "What's up?", Button("Start Conversation", "@talk"), Button("Use a Furnace", "@furnace"), Button("Upgrade Item", "@upgrade"));

        var r = Select(c);
        switch (r)
        {
            case "@talk":
            {
                Msg(c, "You're not from this town, are you? I don't think I've seen you before.");

            L_Keywords:
                Msg(c, Options.Name, "(Sion is paying attention to me.)");
                ShowKeywords(c);

                var keyword = Select(c);

                Msg(c, "Can we change the subject?");
                goto L_Keywords;
            }
            case "@furnace":
            {
                    Msg(c, "Do you want to use the furnace?<br/>You can use it for 1 minute with 100 Gold,<br/>and for 5 minutes with 450 Gold.");

                    Msg(c, "Hehe... It uses firewood, water, and other things...<br/>so I'm sorry but i have to charge you or I lose money. <br/>However, anyone can use it when it's running.",
                    Button("1 Minute", "@onemin"), Button("5 Minutes", "@fivemin"), Button("Forget It", "@forget")
                );
                var duration = Select(c);

                if (duration == "@forget")
                {
                    Msg(c, "You're not going to pay? Then you can't make ingots.<br/>You need fire to refine ore...");
                    End();
                }

                Msg(c, "(Unimplemented)");
                End();
            }
            case "@upgrade":
            {
                Msg(c,
                    "The Pickaxe?<br/>Well, I used to play with it quite a bit as a kid...<br/>Do you think it needs to be upgraded? Leave it up to me.",
                    Button("End Conversation", "@endupgrade")
                );

                r = Select(c);

                Msg(c, "Come see me anytime, especially if you need anything upgraded.");
                End();
            }
        }
    }
}
