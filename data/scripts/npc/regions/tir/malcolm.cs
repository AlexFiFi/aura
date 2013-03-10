// Aura Script
// --------------------------------------------------------------------------
// Malcolm - General Shop
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class MalcolmScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_malcolm");
		SetRace(10002);
		SetBody(height: 1.22f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 16, eye: 26, eyeColor: 162, lip: 0);
		SetStand("human/male/anim/male_natural_stand_npc_Malcolm_retake", "human/male/anim/male_natural_stand_npc_Malcolm_talk");
		SetLocation("tir_general", 1238, 1655, 59);

		EquipItem(Pocket.Face, 0x1324, 0xBA0562);
		EquipItem(Pocket.Hair, 0x103B, 0xECBC58);
		EquipItem(Pocket.Armor, 0x3D27, 0xD8C9B7, 0x112A13, 0x131313);
		EquipItem(Pocket.Shoe, 0x4387, 0x544838, 0x0, 0x0);
		EquipItem(Pocket.LeftHand1, 0x9E2B, 0x808080, 0x0, 0x0);
		EquipItem(Pocket.RightHand1, 0x9C51, 0x3F7246, 0xC0B584, 0x3F4B40);

		Phrases.Add("Aww! My legs hurt. My feet are all swollen from standing all day long.");
		Phrases.Add("Dear love, you live right next door, yet I cannot see you... I can't sleep at night thinking of you...");
		Phrases.Add("Ha ha, look at what that person is wearing. (laugh)");
		Phrases.Add("I wonder what Nora is doing now...");
		Phrases.Add("It isn't easy running a shop alone... Maybe I should hire a clerk.");
		Phrases.Add("Maybe I should wrap it up and call it a day... (confused)");
		Phrases.Add("So much work, so little time... I'm in trouble!");
		Phrases.Add("These travelers will buy something sooner or later.");
	}

	public override IEnumerable OnTalk(WorldClient c)
	{
		Msg(c, Options.FaceAndName,
			"While his thin face makes him look weak,",
			"and his soft and delicate hands seem much too feminine,",
			"his cool long blonde hair gives him a suave look.",
			"He looks like he just came out of a workshop since he's wearing a heavy leather apron."
		);
		MsgSelect(c, "What can I do for you?", "Start Conversation", "@talk", "Shop", "@shop", "Repair Item", "@repair");
		
		var r = Wait();
		switch (r)
		{
			case "@talk":
			{
				Msg(c, "Welcome to the General Shop. This must be your first visit here.");
				
			L_Keywords:
				Msg(c, Options.Name, "(Malcolm is waiting for me to say something.)");
				ShowKeywords(c);
				
				var keyword = Wait();
				
				Msg(c, "Sorry, I don't know.<br/>Hmm... Maybe I should have a travel diary to write things down.");
				goto L_Keywords;
			}
			
			case "@repair":
			{
				Msg(c, "What item do you want to repair?<br/>You can repair various items such as Music Instruments and Glasses.");
				End();
			}

			case "@shop":
			{
				Msg(c, "Welcome to Malcolm's General Shop.<br/>Look around as much as you wish. Clothes, accessories and other goods are in stock.");
				OpenShop(c);
				End();
			}
		}
	}
}
