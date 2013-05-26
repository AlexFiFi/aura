// Aura Script
// --------------------------------------------------------------------------
// Madoc - Nails for Stones quest.
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class MadocScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_madoc");
		SetRace(10002);
		SetBody(height: 1.1f, fat: 1.5f, upper: 1f, lower: 1.5f);
		SetFace(skin: 18, eye: 14, eyeColor: 0, lip: 16);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x1324, 0xFA9D50, 0x4B6570, 0xF78E8B);
		EquipItem(Pocket.Hair, 0x1027, 0xC8A81A, 0xC8A81A, 0xC8A81A);
		EquipItem(Pocket.Armor, 0x3C15, 0x4E89, 0x646E8F, 0x3F5786);
		EquipItem(Pocket.Shoe, 0x4303, 0x3366, 0xE285B, 0x202020);
		EquipItem(Pocket.Head, 0x479A, 0x3497C, 0x888C00, 0x53003E);

		SetLocation(region: 23, x: 26960, y: 37796);

		SetDirection(193);
		SetStand("");
        
		Phrases.Add("Don't you want to give it to me for free? Heh heh...");
		Phrases.Add("Have you heard what happened to Tamon's ship? *Chuckles*");
		Phrases.Add("Hey you. Get me something to eat will ya?");
		Phrases.Add("I'm no swindler.");
		Phrases.Add("I'm not worried about my ship.");
		Phrases.Add("It looks like a storm coming this way.");
		Phrases.Add("Nothing in this life is free.");
		Phrases.Add("Pirates are paying customers too.");
		Phrases.Add("Please, don't tell Tamon I said that...");
		Phrases.Add("This is the way all businesses should be run.");
	}
    
    public override IEnumerable OnTalk(WorldClient c)
    {
        Msg(c, "What you're wearing doesn't look fancy at all...<br/>But would you like me to look at it anyway?", Button("Request from Madoc", "@requestm"));

        var r = Select(c);
        switch (r)
        {
            case "@requestm":
            {
                Msg(c, "(Unimplemented)");
                End();
            }
        }
    }
}
