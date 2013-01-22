// Aura Script
// --------------------------------------------------------------------------
// Meven - Priest
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Common.Constants;
using Common.World;
using World.Network;
using World.Scripting;
using World.World;

public class MevenScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_meven");
		SetRace(10002);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 21, eye: 5, eyeColor: 27, lip: 0);
		SetStand("human/male/anim/male_natural_stand_npc_Meven");
		SetLocation("tir_church", 954, 2271, 198);

		EquipItem(Pocket.Face, 0x1324, 0xA4AFD9);
		EquipItem(Pocket.Hair, 0xFBA, 0xEBE0C0);
		EquipItem(Pocket.Armor, 0x3A9E, 0x313727, 0x282C2B, 0xF0DA4A);
		EquipItem(Pocket.Shoe, 0x4274, 0x313727, 0xFFFFFF, 0xA0927D);

		Phrases.Add("!");
		Phrases.Add("(Smile)");
		Phrases.Add("...");
		Phrases.Add("?");
		Phrases.Add("??");
		Phrases.Add("???");
		Phrases.Add("Ah, I forgot I have some plowing to do.");
		Phrases.Add("Oops! I forgot to iron my robes!");
		Phrases.Add("Perhaps I could take a quick rest now.");
		Phrases.Add("Hiccup! Hiccup! (confused)");
	}

	public override IEnumerable OnTalk(WorldClient c)
	{
		Disable(c, Options.FaceAndName);
		Msg(c, "Dressed in a robe, this composed man of moderate build maintains a very calm posture.",
			"Every bit of his appearance and the air surrounding him show that he is unfailingly a man of the clergy.",
			"Silvery hair frames his friendly face, and his gentle eyes suggest a rather quaint and quiet mood with flashes of hidden humor.");
		Enable(c, Options.FaceAndName);
		MsgSelect(c, "Welcome to the Church of Lymilark.", "Start Conversation", "@talk");
		
		var r = Wait();
		if(r == "@talk")
		{
			Msg(c, "It's nice to see you again.");
			
		L_Keywords:
			Msg(c, Options.Name, "(Meven is waiting for me to say something.)");
			ShowKeywords(c);
			
			var keyword = Wait();
			
			Msg(c, "...<br/>I really don't know.");
			goto L_Keywords;
		}
	}
}
