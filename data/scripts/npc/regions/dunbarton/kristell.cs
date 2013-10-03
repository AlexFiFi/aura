// Aura Script
// --------------------------------------------------------------------------
// Kristell - Churche Priestess
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class KristellScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_kristell");
		SetRace(10001);
		SetBody(height: 0.97f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 3, eyeColor: 191, lip: 0);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0xF3C, 0x136EAA, 0xFEE1D3, 0x7B6C3C);
		EquipItem(Pocket.Hair, 0xBC9, 0xEE937E, 0xEE937E, 0xEE937E);
		EquipItem(Pocket.Armor, 0x3AA1, 0x303133, 0xC6D8EA, 0xDBC741);
		EquipItem(Pocket.Shoe, 0x4277, 0x303133, 0x7BCDB7, 0x6E6565);

		SetLocation(region: 14, x: 34657, y: 42808);

		SetDirection(0);
		SetStand("human/female/anim/female_natural_stand_npc_Kristell");

        Shop.AddTabs("Gift");

        //----------------
        // Gift
        //----------------

        //Page 1
        Shop.AddItem("Gift", 52012);		//Candlestick
        Shop.AddItem("Gift", 52013);		//Flowerpot
        Shop.AddItem("Gift", 52020);		//Flowerpot
        Shop.AddItem("Gift", 52024);		//Flowerpot
        
		Phrases.Add("...");
		Phrases.Add("I wish there was someone who could ring the bell on time...");
		Phrases.Add("In the name of Lymilark...");
		Phrases.Add("It's too much to go up and down these stairs to get here...");
		Phrases.Add("The Church duties just keep coming. What should I do?");
		Phrases.Add("The donations have decreased a little...");
		Phrases.Add("There should be a message from the Pontiff's Office any day now.");
		Phrases.Add("Why do these villagers obsess so much over their current lives?");
	}
    public override IEnumerable OnTalk(WorldClient c)
    {
        Msg(c, Options.FaceAndName, "This priestess, in her neat Lymilark priestess robe, has eyes and hair the color of red wine.<br/>Gazing into the distance, she wears a tilted cross, a symbol of Lymilark, around her neck.<br/>She wears dangling earrings made of the same material which emanate a gentle glow.");
        Msg(c, "Welcome to the Church of Lymilark.", Button("Start a Conversation", "@talk"), Button("Shop", "@shop"));

        var r = Select(c);
        switch (r)
        {
            case "@talk":
			{
				Msg(c, "I am Priestess Kristell. Nice to meet you.");

			L_Keywords:
				Msg(c, Options.Name, "(Kristell is waiting for me to say something.)");
				ShowKeywords(c);
				var keyword = Select(c);

				Msg(c, "Can we change the subject?");
				goto L_Keywords;
			}
            case "@shop":
			{
				Msg(c, "What is it that you are looking for?");
				OpenShop(c);
				End();
			}
		}
	}
}
