// Aura Script
// --------------------------------------------------------------------------
// Augustine - Banker
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class AugustineScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_augustine");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 61, eyeColor: 126, lip: 48);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x1324, 0xDBE7F7, 0xFAB243, 0x5E5064);
		EquipItem(Pocket.Hair, 0xFB9, 0x393839, 0x393839, 0x393839);
		EquipItem(Pocket.Armor, 0x3C13, 0x75CAE4, 0x516181, 0xACC6FE);
		EquipItem(Pocket.Shoe, 0x4301, 0x4B5AA7, 0x8B99B5, 0x424563);

		SetLocation(region: 23, x: 27630, y: 41361);

		SetDirection(245);
		SetStand("chapter4/human/male/anim/male_c4_npc_augustine");

        Shop.AddTabs("License");

        //----------------
        // License
        //----------------

        //Page 1
        Shop.AddItem("License", 60117); //Cobh Personal Shop License
        Shop.AddItem("License", 81010); //Purple Personal Shop Brownie Work-For-Hire Contract
        Shop.AddItem("License", 81011); //Pink Personal Shop Brownie Work-For-Hire Contract
        Shop.AddItem("License", 81012); //Green Personal Shop Brownie Work-For-Hire Contract
        
		Phrases.Add("(Flinches)");
		Phrases.Add("All these people come here to ride the ship, so I'm busy during the day.");
		Phrases.Add("Hmm... That's funny.");
		Phrases.Add("Let's see... maybe they'll leave soon...");
		Phrases.Add("Oh... I can't stand this boredom...");
		Phrases.Add("Oh... So fun.");
		Phrases.Add("Should I try hiring a Goblin as a clerk? That should stop people from coming.");
		Phrases.Add("That person looks poor...");
		Phrases.Add("There are no other officials who are as reliable as Admiral Owen.");
		Phrases.Add("These days, I just stand here without doing anything!");
		Phrases.Add("Tsk. I just got a perm yesterday, but I don't like it.");
		Phrases.Add("Yawn... I'm so sleepy.");
	}
    
    public override IEnumerable OnTalk(WorldClient c)
    {
        Msg(c, Options.FaceAndName, "His soft, shiny black hair exudes youth.<br/>His tightly shut lips and sharp eyes give the<br/>impression of experience beyond his years.<br/>He glares at you with large, impatient eyes.");
        MsgSelect(c, "I'm busy, so just tell me what you are here for.", Button("Start a Conversation", "@talk"), Button("Open My Account", "@bank"), Button("Redeem Coupon", "@redeem"), Button("Shop", "@shop"));

        var r = Wait();
        switch (r)
        {
            case "@talk":
            {
                Msg(c, "Why are you here?");

            L_Keywords:
                Msg(c, Options.Name, "(Augustine is waiting for me to say something.)");
                ShowKeywords(c);

                var keyword = Wait();

                Msg(c, "Can we change the subject?");
                goto L_Keywords;
            }

            case "@bank":
            {
                Msg(c, "(Unimplemented)");
                End();
            }

            case "@redeem":
            {
				MsgSelect(c, "Tell me the coupon number.<br/>Make sure it's the right one.", Input("Exchange Coupon", "Enter your coupon number"));
				var input = Wait();
                if (input == "@cancel")
                    End();

                if (!CheckCode(c, input))
                {
                    Msg(c, "This coupon number didn't work.<br/>I told you to make sure it's the right one.");
                    End();
                }

                // Unofficial response.
                Msg(c, "There you go, have a nice day.");
                End();
            }

            case "@shop":
            {
                Msg(c, "You'll need a Shop License if you want to sell<br/>goods in Port Cobh. Luckily, I have them in stock.<br/>Get one while you still can.");
                OpenShop(c);
                End();
            }
        }
    }
}
