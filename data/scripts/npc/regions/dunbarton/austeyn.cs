// Aura Script
// --------------------------------------------------------------------------
// Austeyn - Dunbarton Banker
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class AusteynScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_austeyn");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1.2f, lower: 1f);
		SetFace(skin: 16, eye: 8, eyeColor: 84, lip: 1);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0x1328, 0x81A6, 0x806588, 0x8DA62C);
		EquipItem(Pocket.Hair, 0xFBB, 0xD1D9E3, 0xD1D9E3, 0xD1D9E3);
		EquipItem(Pocket.Armor, 0x3A9B, 0x36485A, 0xBDC2B1, 0x626C76);
		EquipItem(Pocket.Shoe, 0x4271, 0x36485A, 0xFFE1B9, 0x9A004E);

		SetLocation(region: 20, x: 660, y: 770);

		SetDirection(251);
		SetStand("");

        Shop.AddTabs("License");

        //----------------
        // License
        //----------------

        //Page 1
        Shop.AddItem("License", 60102); //Dunbarton Merchant License
        Shop.AddItem("License", 81010); //Purple Personal Shop Brownie Work-For-Hire Contract
        Shop.AddItem("License", 81011); //Pink Personal Shop Brownie Work-For-Hire Contract
        Shop.AddItem("License", 81012); //Green Personal Shop Brownie Work-For-Hire Contract
        
        Phrases.Add("*Doze off*");
		Phrases.Add("*Yawn*");
		Phrases.Add("Ah... How boring...");
		Phrases.Add("Come to think of it, it's been a while since my last hair cut.");
		Phrases.Add("It's boring during the day with everyone attending school.");
		Phrases.Add("Let's see... That fellow should be coming to the Bank soon.");
		Phrases.Add("Mmm... I must have dozed off.");
		Phrases.Add("Mmm... I'm tired...");
		Phrases.Add("My body's not like it used be... Hahaha.");
		Phrases.Add("Oops. The mistakes have been getting more frequent lately.");
		Phrases.Add("Perhaps I should hire a cute office assistant. Who knows? Maybe that will bring in more business.");
		Phrases.Add("That fellow looks like he might have some Gold on him...");

	}
    public override IEnumerable OnTalk(WorldClient c)
    {
        Msg(c, Options.FaceAndName, "His gray hair and mustache may show his age, but his firm build and the smile on his face show a youthful presence.<br/>It's as if he wants to prove that he can smile even with his small eyes.");
        MsgSelect(c, "Now, what can i help you with?", Button("Start a Conversation", "@talk"), Button("Open My Account", "@bank"), Button("Redeem Coupon", "@redeem"), Button("Trade", "@shop"));

        var r = Wait();
        switch (r)
        {
            case "@talk":
                {
                    Msg(c, "Welcome to the Dunbarton branch of the Erskin Bank.");

                L_Keywords:
                    Msg(c, Options.Name, "(Austeyn is looking at me.)");
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
					MsgSelect(c, "Do you want to redeem your coupon?<br/>Then please give me the number of the coupon you want to redeem.<br/>Slowly, one digit at a time.", Input("Exchange Coupon", "Enter your coupon number"));
					var input = Wait();
                    if (input == "@cancel")
                        End();

                    if (!CheckCode(c, input))
                    {
                        Msg(c, "Strange coupon number.<br/>Are you sure that's the right number?<br/>Think about it one more time... carefully.");
                        End();
                    }

                    // Unofficial response.
                    Msg(c, "There you go, have a nice day.");
                    End();
                }

            case "@shop":
                {
                    Msg(c, "Ah, so you need a Personal Shop License?<br/>You must have one if you want to sell<br/>merchandise around here, so keep it with you.");
                    OpenShop(c);
                    End();
                }
        }
    }
}
