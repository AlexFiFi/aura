// Aura Script
// --------------------------------------------------------------------------
// Cadoc - Fish Shop
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class CadocScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_cadoc");
		SetRace(8002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1.3f, lower: 1.2f);
		SetFace(skin: 22, eye: 54, eyeColor: 76, lip: 34);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x22C4, 0xAAAB23, 0xB30058, 0x2E4602);
		EquipItem(Pocket.Hair, 0x1F50, 0x362825, 0x362825, 0x362825);
		EquipItem(Pocket.Armor, 0x32FB, 0x6B4F5F, 0x4A4A4A, 0xAFAE05);
		EquipItem(Pocket.Glove, 0x3F08, 0x808080, 0xB7AFC0, 0x10BAD7);
		EquipItem(Pocket.Shoe, 0x42F1, 0x313A58, 0x455873, 0x8830F0);
		EquipItem(Pocket.RightHand1, 0xABE7, 0x704215, 0x735858, 0x6E6B69);

		SetLocation(region: 23, x: 26460, y: 37337);

		SetDirection(3);
		SetStand("chapter4/giant/male/anim/giant_npc_cadoc");

        Shop.AddTabs("Fish");

        //----------------
        // Fish
        //----------------

        //Page 1
        Shop.AddItem("Fish", 50255); 		//Jellyfish
        Shop.AddItem("Fish", 50204); 		//Golden Scale Fish
        Shop.AddItem("Fish", 50254); 		//Flying Fish
        Shop.AddItem("Fish", 50673); 		//Black Sea Bream
        Shop.AddItem("Fish", 50672); 		//Blowfish
        Shop.AddItem("Fish", 50674); 		//Red Sea Bream
        Shop.AddItem("Fish", 50257); 		//Marine Pearl Oyster
        Shop.AddItem("Fish", 50248); 		//Shellfish
        Shop.AddItem("Fish", 50256); 		//King Crab
        Shop.AddItem("Fish", 50518); 		//Sturgeon
        Shop.AddItem("Fish", 50127); 		//Shrimp
        Shop.AddItem("Fish", 50158); 		//Taitinn Carp
        
		Phrases.Add("(Shrugs shoulders.)");
		Phrases.Add("A real man has real muscles!");
		Phrases.Add("Admiral Owen is a role model for real men!");
		Phrases.Add("Breathe in, breathe out.");
		Phrases.Add("Fish for sale! Fish for sale!");
		Phrases.Add("Hahah! Bam bam!");
		Phrases.Add("Hmm... Do you envy my muscles?");
		Phrases.Add("Men these days are weaklings.");
		Phrases.Add("Whenever I see a log, I test my stength by picking it up swinging it.");
	}
    
    public override IEnumerable OnTalk(WorldClient c)
    {
        Msg(c, Options.FaceAndName,
            "He is an extremely tall man. Huge muscles press his",
            "veins against tanned skin. He looks much",
            "too strong to handle something as delicate as fish."
        );
        MsgSelect(c, "I can't believe how skinny you are! I know what<br/>you need to build some bulk. Buy some of my fish.<br/>They're rich in protein and can be used as bait.", Button("Shop", "@shop"));

        var r = Wait();
        switch (r)
        {
            case "@shop":
            {
                Msg(c, "How would you like your fish?");
                OpenShop(c);
                End();
            }
        }
    }
}
