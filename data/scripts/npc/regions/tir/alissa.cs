// Aura Script
// --------------------------------------------------------------------------
// Alissa - Windmill manager
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class AlissaScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_alissa");
		SetRace(10001);
		SetBody(height: 0.1f, fat: 1.3f, upper: 1.3f, lower: 1.4f);
		SetFace(skin: 19, eye: 10, eyeColor: 148, lip: 2);
		SetStand("human/female/anim/female_natural_stand_npc_alissa");
		SetLocation("tir", 15765, 31015, 120);

		EquipItem(Pocket.Face, 0xF3C, 0xFCD7D7);
		EquipItem(Pocket.Hair, 0xC47, 0xD57527);
		EquipItem(Pocket.Armor, 0x3D26, 0xDECDB0, 0x6C7553, 0x9B9E7B);
		EquipItem(Pocket.Shoe, 0x4274, 0x693F1E, 0x0, 0x0);
		EquipItem(Pocket.Head, 0x47E6, 0xDECDB0, 0x0, 0x0);

		Phrases.Add("Hmm... Ferghus must have made another mistake.");
		Phrases.Add("How are you going to make flour without any wheat?");
		Phrases.Add("La la la la.");
		Phrases.Add("La la la, one leaf, la la la, two leaves.");
		Phrases.Add("My sister needs to grow up...");
		Phrases.Add("There's a guard at the wheat field, and I'm watching the Windmill.");
		Phrases.Add("When is Caitin going to teach me how to bake bread?");
		Phrases.Add("You can gather wheat at the wheat field.");
	}

	public override IEnumerable OnTalk(WorldClient c)
	{
		Bgm(c, "NPC_Alissa.mp3");
		
		Msg(c, Options.FaceAndName, "A young girl stands with her habds on her hips like she's a person of great importance.<br/>She wears a worn out hat that frames her soft hair, round face, and button nose.<br/>As she stands there, you notice that her apron is actually too big, and she's discreetly trying to keep it from slipping.<br/>In spite of all that, her cherry eyes sparkle with curiosity.");
		Msg(c, "So, what can I do for you?", Button("Start Conversation", "@talk"), Button("Operate the Windmill", "@windmill"));
		
		var r = Select(c);
		switch(r)
		{
			case "@talk":
			{
				Msg(c, "Hello, we haven't met. My name is Alissa. Your name is " + c.Character.Name + ", right?<br/>How did I know?<br/>Haha, it's written above your head. Don't tell me you don't see it?");
			
			L_Keywords:
				Msg(c, Options.Name, "(Alissa is looking at me.)");
				ShowKeywords(c);
				var keyword = Select(c);
				
				Msg(c, "Can we change the subject?");
				goto L_Keywords;
			}
			
			case "@windmill":
			{
				Msg(c,
					"How long do you want to use the Mill?<br/>It's 100 Gold for one minute and 450 Gold for 5 minutes.<br/>Once it starts working, anyone can use the Mill.",
					Button("1 Minute", "@onemin"), Button("5 Minutes", "@fivemin"), Button("Forget It", "@forget")
				);
				var duration = Select(c);
				
				if(duration == "@forget")
				{
					Msg(c, "I'm sorry I have to charge you...<br/>But I was taught to do my job well.");
					End();
				}
				
				Msg(c, "(Unimplemented)");
				End();
			}
		}
	}
}
