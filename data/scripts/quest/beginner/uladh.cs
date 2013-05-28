using System.Collections;
using Aura.Data;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class NaosLetterOfIntroductionQuest : QuestScript
{
	public override void OnLoad()
	{
		SetId(200501);
		SetName("Nao's Letter of Introduction");
		SetDescription("Dear [Chief Duncan],\r\nI am directing someone to you. This person is from another world. Please help them adjust to life in Erinn. Thank you, and I hope I will be able to visit you soon. - Nao Pryderi -");
		SetReceive(Receive.Manually);

		AddObjective("talk_duncan", "Go to Tir Chonaill and deliver the Letter to Chief Duncan.", 1, 15409, 38310, Talk("duncan"));

		AddReward(Exp(100));
		
		AddHook("_duncan", "after_intro", TalkDuncan);
	}
	
	public IEnumerable TalkDuncan(WorldClient c, NPCScript n)
	{
		if (QuestActive(c, this.Id))
		{
			FinishQuestObjective(c, this.Id, "talk_duncan");
			
			n.Msg(c, Options.Name, "(You hand Nao's Letter of Introduction to Duncan.)");
			n.Msg(c, "Ah, a letter from Nao.<br/>Hard to believe that little<br/>tomboy's all grown up...");
			n.Msg(c, Options.Name, "(Duncan folds the letter in half and puts it in his pocket.)");
			n.Msg(c, "So, you're <username/>.<br/>I'm Duncan, the chief of this town.<br/>Welcome to Tir Chonaill.");
			n.Msg(c, "Would you like to learn how to complete quests?");
			n.Msg(c, n.Image("npctalk_questwindow", true, 272, 235), n.Text("Press the "), n.Hotkey("QuestView"), n.Text(" key or<br/>press the Quest button at the bottom of your screen.<br>The quest window will appear and display your current quests."));

			while (true)
			{
				n.Msg(c, n.Text("Press the "), n.Hotkey("QuestView"), n.Text(" key or<br/>press the Quest button at the bottom of your screen."), n.Button("I pressed the Quest button", "@pressed"), n.Button("$hidden", "@quest_btn_clicked", "autoclick_QuestView"));
				var r = n.Select(c);
				if (r != "@pressed")
					break;
				n.Msg(c, "Hmm... Are you sure you pressed the Quest button?<br/>It's possible that the Quest Window was already open, so<br/>try pressing it again.");
			}

			n.Msg(c, "Well done. See the list of quests?<br/>Clicking on a quest brings up the quest's details.<br/>Quests will show a yellow Complete button<br/>next to their names when you finish them.");
			n.Msg(c, "Try pressing the Complete button now.<br/>As important as it is to complete quests,<br/>it's just as important to press the \"Complete\" button<br/>afterwards to recieve your rewards.");
			n.Msg(c, "(Duncan looks at you with his benevolent hazel eyes.)");
			n.Msg(c, "You've just learned one very basic skill<br/>to survive in Erinn.");
			n.Msg(c, "Soon, you will recieve a quest from an owl.<br/>Then, you will be able to start your training for real.");

			Break();
		}
		
		End();
	}
}

public class RescueResidentQuest : QuestScript
{
	public override void OnLoad()
	{
		SetId(200502);
		SetName("Rescue Resident");
		SetDescription("I'm Trefor, serving as a guard in the north part of the town, past the Healer's House. One of the residents of this town went to Alby Dungeon and has not come back yet. I'm worried about it, so I need you to help me search for the lost resident. - Trefor -");
		SetReceive(Receive.Auto);

		AddPrerequisite(QuestCompleted(200501));

		AddObjective("talk_trefor", "Talk with Trefor", 1, 8692, 52637, Talk("trefor"));
		AddObjective("kill_foxes", "Hunt 5 Young Brown Foxes", 1, 9124, 52108, Kill(5, 50001, 50007));
		AddObjective("talk_trefor2", "Talk with Trefor", 1, 8692, 52637, Talk("trefor"));
		AddObjective("clear_alby", "Rescue a town resident from Alby Dungeon", 13, 3203, 3199, Talk("trefor"));
		
		AddReward(Exp(300));
		AddReward(Gold(1800));
		AddReward(AP(3));
		
		AddHook("_trefor", "after_intro", TalkTrefor);
	}
	
	public IEnumerable TalkTrefor(WorldClient c, NPCScript n)
	{
		if (QuestActive(c, this.Id))
		{
			switch(QuestObjective(c, this.Id))
			{
				case "talk_trefor":
				{
					FinishQuestObjective(c, this.Id, "talk_trefor");
					
					n.Msg(c, "Welcome, I am Trefor, the guard.<br/>Someone from the town went into Alby Dungeon a while ago, but hasn't returned yet.<br/>I wish I could go there myself, but I can't leave my post. I'd really appreciate it if you can go and look for in Alby Dungeon.");
					n.Msg(c, "Since the dungeon is a dangerous place to be in, I'll teach you a skill that will help you in an emergency situation.<br/>It's called the Smash skill. If you use it, you can knock down a monster with a single blow!<br/>It is also highly effective when you sneak up on a target and deliver the blow without warning.");
					n.Msg(c, "Against monsters that are using the Defense skill,<br/>Smash will be the only way to penetrate that skill and deliver a killer blow.");
					n.Msg(c, "However... looking at the way you're holding your sword, I'm not sure if you are up to the task.<br/>Let me test your skills first. Do you see those brown foxes wandering in front of me?<br/>They're quite a nuisance, praying on those roosters in town.<br/>I want you to go and hunt 5 Young Brown Foxes right now.");
					n.Msg(c, "Foxes use the Defense Skill a lot, and as I told you before, regular attacks do not work against defending targets.<br/>That's then the Smash skill comes in handy.<br/><br/>Watch how I do it, and try picking up the important parts so you can use it too.<br/>You don't need to overstrain yourself by going for the Brown Foxes. Young Brown Foxes will do just fine.");

					// cutscene
					
					Break();
				}
				break;
				case "talk_trefor2":
				{
					FinishQuestObjective(c, this.Id, "talk_trefor2");
					
					n.Msg(c, "Good, I see that you're getting the hang of it.<br/>Well, I was able to do that when I was 8, but whatever...<br/>It is now time for you to go and search for the missing Villager.");
					n.Msg(c, "Follow the road up and turn right and you'll find the Alby Dungeon.<br/>You can enter the dungeon by dropping this item on the altar.<br/>If you either lose it or fail to rescue her, come back to me so I can give you another one. Please be careful.", n.Image("dungeonpass", 128, 128));
					
					n.GiveItem(c, 63140, 1);
					
					Break();
				}
				break;
			}
		}
		
		End();
	}
}
