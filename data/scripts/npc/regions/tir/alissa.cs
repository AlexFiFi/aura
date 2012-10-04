using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class AlissaScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_alissa");
		SetRace(10001);
		SetBody(height: 0.1f, fat: 1.3f, upper: 1.3f, lower: 1.4f);
		SetFace(skin: 19, eye: 10, eyeColor: 148, lip: 2);

		EquipItem(Pocket.Face, 0xF3C, 0xFCD7D7);
		EquipItem(Pocket.Hair, 0xC47, 0xD57527);
		EquipItem(Pocket.Armor, 0x3D26, 0xDECDB0, 0x6C7553, 0x9B9E7B);
		EquipItem(Pocket.Shoe, 0x4274, 0x693F1E, 0x0, 0x0);
		EquipItem(Pocket.Head, 0x47E6, 0xDECDB0, 0x0, 0x0);

		SetLocation(region: 1, x: 15765, y: 31015);

		SetDirection(120);
		SetStand("human/female/anim/female_natural_stand_npc_alissa");

		Phrases.Add("Hmm... Ferghus must have made another mistake.");
		Phrases.Add("How are you going to make flour without any wheat?");
		Phrases.Add("La la la la.");
		Phrases.Add("La la la, one leaf, la la la, two leaves.");
		Phrases.Add("My sister needs to grow up...");
		Phrases.Add("There's a guard at the wheat field, and I'm watching the Windmill.");
		Phrases.Add("When is Caitin going to teach me how to bake bread?");
		Phrases.Add("You can gather wheat at the wheat field.");
	}

	public override void OnTalk(WorldClient c)
	{
		Msg(c, false, false, "A young girl stands with her habds on her hips like she's a person of great importance.",
			"She wears a worn out hat that frames her soft hair, round face, and button nose.",
			"As she stands there, you notice that her apron is actually too big, and she's discreetly trying to keep it from slipping.",
			"In spite of all that, her cherry eyes sparkle with curiosity.");
		MsgSelect(c, "So, what can I do for you?", "Start Conversation", "@talk", "Operate the Windmill", "@windmill");
	}

	public override void OnSelect(WorldClient c, string r, string i = null)
	{
		switch (r)
		{
			case "@forget":
				Msg(c, "I'm sorry I have to charge you...",
					"But I was taught to do my job well.");
				break;
			case "@talk":
				Msg(c, "Hello, we haven't met. My name is Alissa. Your name is " + c.Character.Name + ", right?",
					"How did I know?",
					"Haha, it's written above your head. Don't tell me you don't see it?");
				Msg(c, true, false, "(Alissa is looking at me.)");
				ShowKeywords(c);
				break;
			case "@windmill":
				MsgSelect(c, "How long do you want to use the Mill?<br/>It's 100 Gold for one minute and 450 Gold for 5 minutes.<br/>Once it starts working, anyone can use the Mill.",
					"1 Minute", "@onemin", "5 Minutes", "@fivemin", "Forget It", "@forget");
				break;
				
			default:
				Msg(c, "Can we change the subject?");
				ShowKeywords(c);
				break;
		}
	}
}
