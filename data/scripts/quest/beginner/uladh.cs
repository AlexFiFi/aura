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
	
	public override void OnCompleted(WorldClient c, MabiQuest q)
	{
		//StartQuest(c, 200502); // visit_the_healers_house
	}
	
	public IEnumerable TalkDuncan(WorldClient c, NPCScript n)
	{
		// Nao's Letter of Introduction
		if (QuestActive(c, 200501))
		{
			FinishQuestObjective(c, 200501, "talk_duncan");
			
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
