using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class MevenScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_meven");
		SetRace(10002);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 21, eye: 5, eyeColor: 27, lip: 0);

		EquipItem(Pocket.Face, 0x1324, 0xA4AFD9);
		EquipItem(Pocket.Hair, 0xFBA, 0xEBE0C0);
		EquipItem(Pocket.Armor, 0x3A9E, 0x313727, 0x282C2B, 0xF0DA4A);
		EquipItem(Pocket.Shoe, 0x4274, 0x313727, 0xFFFFFF, 0xA0927D);

		SetLocation(region: 4, x: 954, y: 2271);

		SetDirection(198);
		SetStand("human/male/anim/male_natural_stand_npc_Meven");
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

	public override void OnTalk(WorldClient c)
	{
		Msg(c, false, false, "Dressed in a robe, this composed man of moderate build maintains a very calm posture.",
			"Every bit of his appearance and the air surrounding him show that he is unfailingly a man of the clergy.",
			"Silvery hair frames his friendly face, and his gentle eyes suggest a rather quaint and quiet mood with flashes of hidden humor.");
		MsgSelect(c, "Welcome to the Church of Lymilark.", "Start Conversation", "@talk");
	}

	public override void OnSelect(WorldClient c, string r, string i = null)
	{
		switch (r)
		{
			case "@talk":
				Msg(c, "It's nice to see you again.");
				Msg(c, true, false, "(Meven is waiting for me to say something.)");
				ShowKeywords(c);
				break;
				
			default:
				Msg(c, "...", "I really don't know.");
				ShowKeywords(c);
				break;
		}
	}
}
