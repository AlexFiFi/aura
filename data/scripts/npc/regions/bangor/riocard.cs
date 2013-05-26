// Aura Script
// --------------------------------------------------------------------------
// Riocard - NPC In the Cafe
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class RiocardScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_riocard");
		SetRace(10002);
		SetBody(height: 0.6999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 20, eye: 2, eyeColor: 60, lip: 1);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0x1324, 0xFECDBC, 0x551866, 0x780000);
		EquipItem(Pocket.Hair, 0xFA1, 0xAF7B34, 0xAF7B34, 0xAF7B34);
		EquipItem(Pocket.Armor, 0x3AC0, 0xEFCE4B, 0xB5C27E, 0x37553F);
		EquipItem(Pocket.Shoe, 0x4272, 0x512522, 0x9E0075, 0xB80075);
		EquipItem(Pocket.Head, 0x4657, 0xEFCE4B, 0x6CB4E4, 0x1A890);

		SetLocation(region: 31, x: 15286, y: 8898);

		SetDirection(136);
		SetStand("human/male/anim/male_natural_stand_npc_riocard");
        
		Phrases.Add("I could use a good story right about now.");
		Phrases.Add("I guess taking it easy every now and then isn't such a  bad idea.");
		Phrases.Add("I'm getting bored...");
		Phrases.Add("It's been a while since I started working here.");
		Phrases.Add("Let's see. What should I do now?");
		Phrases.Add("Phew... It's dusty already.");
		Phrases.Add("There is no end to this mess.");
		Phrases.Add("Well, work is work.");
		Phrases.Add("Why should I clean up everyone's mess?");
	}
    
    public override IEnumerable OnTalk(WorldClient c)
    {
        Msg(c, Options.FaceAndName, "He wears a yellow beret backwards at a slight angle as his hair sticks out on the sides.<br/>The yellow shirt that he seemed to have put on in a rush matches his hat.<br/>Between the narrow shoulders, his face is still full of boyish charm.<br/>Every time he blinks, his eyelid casts a slight shadow over his innocent, light green eyes.");
        Msg(c, "Mmm? What is it?", Button("Start Conversation", "@talk"));

        var r = Select(c);
        switch (r)
        {
            case "@talk":
            {
                Msg(c, "You've been here before, haven't you? I recognize your face.");

            L_Keywords:
                Msg(c, Options.Name, "(Riocard is looking at me.)");
                ShowKeywords(c);

                var keyword = Select(c);

                Msg(c, "Can we change the subject?");
                goto L_Keywords;
            }
        }
    }
}
