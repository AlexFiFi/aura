// Aura Script
// --------------------------------------------------------------------------
// Anghus - Some kind of dock swordsman
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;
using Aura.Shared.Util;

public class AnghusScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_anghus");
		SetRace(10002);
		SetBody(height: 1.2f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 12, eyeColor: 134, lip: 2);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x1324, 0x72664B, 0xDDAA57, 0xE5D952);
		EquipItem(Pocket.Hair, 0x100E, 0x616161, 0x616161, 0x616161);
		EquipItem(Pocket.Armor, 0x3C12, 0x3366, 0x3366, 0xFFFFFF);
		EquipItem(Pocket.Shoe, 0x4338, 0x22632, 0x1B2157, 0x4D3500);
		EquipItem(Pocket.RightHand2, 0x9C4A, 0xA7A7A7, 0x495458, 0xFFFFFF);

		SetLocation(region: 23, x: 29028, y: 35741);

		SetDirection(133);
		SetStand("");
        
		Phrases.Add("I salute to you, Admiral Owen!");
		Phrases.Add("I want to wipe out the pirates again.");
		Phrases.Add("I wonder how Admiral Owen is doing.");
		Phrases.Add("It's a nice day out. Admiral Owen would like it.");
		Phrases.Add("Morning exercise is a must for a soldier.");
		Phrases.Add("Once a pirate, always a pirate, Annick!");
		Phrases.Add("Only if I were a little younger...");
		Phrases.Add("Why is Annick here? She's a pirate! Humph.");
	}
    
    public override IEnumerable OnTalk(WorldClient c)
    {
        switch(RandomProvider.Get().Next(0, 5))
        {
            case 0: Msg(c, "I assisted Admiral Owen<br/>when he kicked the pirates out of Belvast.<br.>I'll never forget that sense of victory."); break;
            case 1: Msg(c, "I thought I'd always work under his direction...<br/>It's just not the same now."); break;
            case 2: Msg(c, "Admiral Owen is a respectable man."); break;
            case 3: Msg(c, "The Admiral's enemies are my enemies too.<br/>I can't understand is why he<br/>doesn't do anything about<br/>a pirate like Annick.<br/>A pirate is a pirate for like."); break;
            case 4: Msg(c, "I wonder if the Admiral would approve<br/>of me capturing Annick..."); break;
            case 5: Msg(c, "Do you want to hear about my heroic exploits?"); break;
        }
        End();
    }
}
