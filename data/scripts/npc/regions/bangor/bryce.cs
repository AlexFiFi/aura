// Aura Script
// --------------------------------------------------------------------------
// Bryce - Banker
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class BryceScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_bryce");
		SetRace(10002);
		SetBody(height: 1.1f, fat: 1f, upper: 1.5f, lower: 1f);
		SetFace(skin: 20, eye: 5, eyeColor: 76, lip: 12);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0x1326, 0x155782, 0x84B274, 0xFBC25C);
		EquipItem(Pocket.Hair, 0xFBB, 0x5B482B, 0x5B482B, 0x5B482B);
		EquipItem(Pocket.Armor, 0x3ABA, 0xFAF7EB, 0x3C2D22, 0x100C0A);
		EquipItem(Pocket.Shoe, 0x4271, 0x0, 0xF69A2B, 0x4B676F);

		SetLocation(region: 31, x: 11365, y: 9372);

		SetDirection(0);
		SetStand("human/male/anim/male_natural_stand_npc_bryce");

        Shop.AddTabs("License");

        //----------------
        // License
        //----------------

        //Page 1
        Shop.AddItem("License", 60103); //Bangor Merchant License
        Shop.AddItem("License", 81010); //Purple Personal Shop Brownie Work-For-Hire Contract
        Shop.AddItem("License", 81011); //Pink Personal Shop Brownie Work-For-Hire Contract
        Shop.AddItem("License", 81012); //Green Personal Shop Brownie Work-For-Hire Contract
        
		Phrases.Add("*Cough* There's just too much dust in here.");
		Phrases.Add("Anyway, where did Ibbie go again?");
		Phrases.Add("Have my eyes really become this bad?");
		Phrases.Add("I don't even have time to read a book these days.");
		Phrases.Add("I'll just have to fight through it.");
		Phrases.Add("It's about the time Ibbie returned.");
		Phrases.Add("It's almost time.");
		Phrases.Add("Mmm... Up to where did I calculate?");
		Phrases.Add("Sion, you little punk... You'll pay if you bully my Ibbie.");
		Phrases.Add("Tomorrow will be better than today.");
		Phrases.Add("Well, cheer up!");
		Phrases.Add("What should I buy Ibbie today?");
		Phrases.Add("When was I supposed to be contacted from Dunbarton?");
	}
    
    public override IEnumerable OnTalk(WorldClient c)
    {
        Msg(c, Options.FaceAndName, "He's dressed neatly in a high neck shirt and a brown vest.<br/>His cleft chin is cleanly shaved and his hair has been well groomed and flawlessly brushed back.<br/>He stares at you with shining hazelnut eyes that are deep-set in his pale face.");
        MsgSelect(c, "What is it?", Button("Start Conversation", "@talk"), Button("Open My Account", "@bank"), Button("Redeem Coupon", "@redeem"), Button("Shop", "@shop"));

        var r = Wait();
        switch (r)
        {
            case "@talk":
            {
                Msg(c, "Welcome to the Bangor branch of the Erskin Bank.");

            L_Keywords:
                Msg(c, Options.Name, "(Bryce is looking at me.)");
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
                MsgInput(c, "Would you like to redeem your coupon?<br/>You're a blessed one.<br/>Please input the number of coupons you wish to redeem.", "Exchange Coupon", "Enter your coupon number");
                var input = Wait();
                if (input == "@cancel")
                    End();

                if (!CheckCode(c, input))
                {
                    Msg(c, "......<br/>I'm not sure what kind of coupon this is.<br/>Please make sure that you have inputted the correct coupon number.");
                    End();
                }

                // Unofficial response.
                Msg(c, "There you go, have a nice day.");
                End();
            }

            case "@shop":
            {
                Msg(c, "You need a license to open a Personal Shop here.<br/>...I recommend buying one in case you need it.");
                OpenShop(c);
                End();
            }
        }
    }
}
