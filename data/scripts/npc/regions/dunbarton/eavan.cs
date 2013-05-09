// Aura Script
// --------------------------------------------------------------------------
// Eavan - Administrave Public Servant Of Dunbarton
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class EavanScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_eavan");
		SetRace(10001);
		SetBody(height: 0.9999999f, fat: 0.7f, upper: 0.7f, lower: 0.7f);
		SetFace(skin: 15, eye: 3, eyeColor: 3, lip: 0);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0xF3C, 0xD1DFF2, 0x38BD96, 0xF69365);
		EquipItem(Pocket.Hair, 0xBCE, 0xFFEEAA, 0xFFEEAA, 0xFFEEAA);
		EquipItem(Pocket.Armor, 0x3AC1, 0xFFCCCC, 0x80C5D3, 0xA7ACB4);
		EquipItem(Pocket.Glove, 0x3E8F, 0xFFFFFF, 0xE6F2E2, 0x6161AC);
		EquipItem(Pocket.Shoe, 0x4270, 0xDDAACC, 0xF79B2F, 0xE10175);

		SetLocation(region: 14, x: 40024, y: 41041);

		SetDirection(192);
		SetStand("human/female/anim/female_natural_stand_npc_Eavan");

        Shop.AddTabs("Quest", "Party Quest", "Guild", "Gift", "Arena", "Guild Quest");

        //----------------
        // Quest
        //----------------

        //Page 1
        Shop.AddItem("Quest", 70080);		//All Hunting Quests
	Shop.AddItem("Quest", 70081);
	Shop.AddItem("Quest", 70082);
	Shop.AddItem("Quest", 70086);
	Shop.AddItem("Quest", 70095);
        Shop.AddItem("Quest", 70096);
	Shop.AddItem("Quest", 70097);
	Shop.AddItem("Quest", 70100);
	Shop.AddItem("Quest", 70102);
	Shop.AddItem("Quest", 70103);
        Shop.AddItem("Quest", 70104);
	Shop.AddItem("Quest", 70106);
	Shop.AddItem("Quest", 70105);
	Shop.AddItem("Quest", 70108);
	Shop.AddItem("Quest", 70109);
        Shop.AddItem("Quest", 70114);
	Shop.AddItem("Quest", 70115);
	Shop.AddItem("Quest", 70116);
	Shop.AddItem("Quest", 70118);
	Shop.AddItem("Quest", 70139);
        Shop.AddItem("Quest", 70119);
	Shop.AddItem("Quest", 70099);
	Shop.AddItem("Quest", 70091);
	Shop.AddItem("Quest", 70092);
	Shop.AddItem("Quest", 70093);
	Shop.AddItem("Quest", 70137);
	Shop.AddItem("Quest", 70138);

        //----------------
        // Party Quest
        //----------------

        //Page 1
        Shop.AddItem("Party Quest", 70025);	//All Party Quests
        Shop.AddItem("Party Quest", 70025);
        Shop.AddItem("Party Quest", 70025);
        Shop.AddItem("Party Quest", 70025);
        Shop.AddItem("Party Quest", 70025);
        Shop.AddItem("Party Quest", 70025);
        Shop.AddItem("Party Quest", 70025);
        Shop.AddItem("Party Quest", 70025);
        Shop.AddItem("Party Quest", 70025);
        Shop.AddItem("Party Quest", 70025);
        Shop.AddItem("Party Quest", 70025);
        Shop.AddItem("Party Quest", 70025);
        Shop.AddItem("Party Quest", 70025);

        //----------------
        // Guild
        //----------------

        //Page 1
        Shop.AddItem("Guild", 63040); 		//Guild Formation Permit
        Shop.AddItem("Guild", 63041); 		//Guild Stone Installation Permit

        //----------------
        // Gift
        //----------------

        //Page 1
        Shop.AddItem("Gift", 52014);		//Teddy Bear
        Shop.AddItem("Gift", 52016);		//Bunny Doll
        Shop.AddItem("Gift", 52015);		//Pearl Necklace
        Shop.AddItem("Gift", 52025);		//Gift Ring

        //----------------
        // Arena
        //----------------

        //Page 1
        Shop.AddItem("Arena", 63050, 10);	//Rabbie Battle Coins
        Shop.AddItem("Arena", 63050, 20);
        Shop.AddItem("Arena", 63050, 50);
        Shop.AddItem("Arena", 63050, 100);

        //----------------
        // Guild Quest
        //----------------

        //Page 1
        Shop.AddItem("Guild Quest", 70152);	//All Guild Quests
        Shop.AddItem("Guild Quest", 70152);
        Shop.AddItem("Guild Quest", 70152);
        Shop.AddItem("Guild Quest", 70152);
        Shop.AddItem("Guild Quest", 70152);

        
        Phrases.Add("*Sigh* Back to work.");
		Phrases.Add("Hmm. This letter is fairly well done. B+.");
		Phrases.Add("Next person please!");
		Phrases.Add("Next, please!");
		Phrases.Add("Registration is this way!");
		Phrases.Add("Teehee... Another love letter.");
		Phrases.Add("The Adventurers' Association is this way!");
		Phrases.Add("Ugh. I wish I could take a breather...");
		Phrases.Add("What's with this letter? How unpleasant!");
		Phrases.Add("Whew. I want to take a break...");
	}
    public override IEnumerable OnTalk(WorldClient c)
    {
        Msg(c, Options.FaceAndName,
            "Wearing a rosy pink blouse, her shoulders are gently covered by her blonde hair that seems to wave in the breeze.",
            "An oval face, a pair of calm eyes with depth, and a slightly small nose right a rounded tip...",
	    "Beneath are the lips that shine in the same color as her blouse."
        );
        MsgSelect(c, "This is the Adventurers' Association.", Button("Start a Conversation", "@talk"), Button("Shop", "@shop"), Button("Retrieve Lost Items", "@retrieve"), Button("About Daily Events", "@aboutdaily"));

        var r = Wait();
        switch (r)
        {
            case "@talk":
                {
                    Msg(c, "Hmm. I've seen someone that look like you before.");

                L_Keywords:
                    Msg(c, Options.Name, "(Eavan is paying attention to me.)");
                    ShowKeywords(c);
                    var keyword = Wait();

                    Msg(c, "Can we change the subject?");
                    goto L_Keywords;
                }
            case "@shop":
                {
                    Msg(c, "Welcome to the Adventurer's Association.");
                    OpenShop(c);
                    End();
                }
            case "@retrieve":
                {
                    Msg(c, "(Unimplemented)");
                    End();
                }
            case "@aboutdaily":
                {
                    Msg(c, "Did you receive today's Daily Event quest?<br/>Every day, you'll get a mission for each region.<br/>For instance, you can complete one mission each<br/>at Tara and at Taillteann.");
		    Msg(c, "Once you have completed an event quest from one region,<br/>you will automatically receive the next region's event quest.");
		    Msg(c, "Expired daily event quests will automatically disappear, so<br/>don't forget to do them!");
                    End();
                }
	}
}
}
