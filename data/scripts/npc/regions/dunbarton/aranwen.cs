// Aura Script
// --------------------------------------------------------------------------
// Aranwen - School Combat Instructor
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class AranwenScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_aranwen");
		SetRace(10001);
		SetBody(height: 1.15f, fat: 0.9f, upper: 1.1f, lower: 0.8f);
		SetFace(skin: 15, eye: 3, eyeColor: 192, lip: 0);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0xF3C, 0xBED5EE, 0xF48E71, 0x7E9C);
		EquipItem(Pocket.Hair, 0xBD2, 0xBDC2E5, 0xBDC2E5, 0xBDC2E5);
		EquipItem(Pocket.Armor, 0x32D0, 0xC6D8EA, 0xC6D8EA, 0x635985);
		EquipItem(Pocket.Glove, 0x4077, 0xC6D8EA, 0xB20859, 0xA7131C);
		EquipItem(Pocket.Shoe, 0x4460, 0xC6D8EA, 0xC6D8EA, 0x3F6577);
		EquipItem(Pocket.RightHand1, 0x9C4C, 0xC0C0C0, 0x8C84A4, 0x403C47);

		SetLocation(region: 14, x: 43378, y: 40048);

		SetDirection(125);
		SetStand("");

        Shop.AddTabs("Party Quest");

        //----------------
        // Party Quest
        //----------------

        //Page 1
        Shop.AddItem("Party Quest", 70025); //Party Quest
        Shop.AddItem("Party Quest", 70025);
        Shop.AddItem("Party Quest", 70025);
        Shop.AddItem("Party Quest", 70025);
        Shop.AddItem("Party Quest", 70025);
        Shop.AddItem("Party Quest", 70025);
        Shop.AddItem("Party Quest", 70025);
        Shop.AddItem("Party Quest", 70025);
        Shop.AddItem("Party Quest", 70025);
        
        Phrases.Add("...");
		Phrases.Add("A sword does not betray its own will.");
		Phrases.Add("A sword is not a stick. I don't feel any tension from you!");
		Phrases.Add("Aren't you well?");
		Phrases.Add("Focus when you're practicing.");
		Phrases.Add("Hahaha.");
		Phrases.Add("If you're done resting, let's keep practicing!");
		Phrases.Add("It's those people who really need to learn swordsmanship.");
		Phrases.Add("Put more into the wrists!");
		Phrases.Add("That student may need to rest a while.");
	}
    
    public override IEnumerable OnTalk(WorldClient c)
    {
        Msg(c, Options.FaceAndName, "A lady decked out in shining armor is confidently training students in swordsmanship in front of the school.<br/>Unlike a typical swordswoman, her moves seem delicate and elegant.<br/>Her long, braided silver hair falls down her back, leaving her eyes sternly fixed on me.");
        Msg(c, "What brings you here?", Button("Start Conversation", "@talk"), Button("Shop", "@shop"), Button("Modify Item", "@modify"));

        var r = Select(c);
        switch (r)
        {
            case "@talk":
            {
                Msg(c, "Yes? Please don't block my view.");

            L_Keywords:
                Msg(c, Options.Name, "(Aranwen is waiting for me to say something.)");
                ShowKeywords(c);

                var keyword = Select(c);

                Msg(c, "Can we change the subject?");
                goto L_Keywords;
            }
            case "@shop":
            {
                Msg(c, "Are you looking for a party quest scroll?");
                OpenShop(c);
                End();
            }
            case "@modify":
            {
                Msg(c,
                    "Please select the weapon you'd like to modify.<br/>Each weapon can be modified according to its kind.",
                    Button("End Conversation", "@endmodify")
                );
                r = Select(c);
                Msg(c, "A bow is weaker than a crossbow?<br/>That's because you don't know a bow vers well.<br/>Crossbows are advanced weapons for sure,<br/>but a weapon that reflects your strength and senses is closer to nature than machinery.");
                End();
            }
        }
    }
}
