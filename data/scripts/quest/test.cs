// --- Aura Script ----------------------------------------------------------
//  Test Quests
// --------------------------------------------------------------------------

using System.Collections;
using Common.Constants;
using Common.Data;
using World.Scripting;
using World.Network;

public class TestQuestScript : QuestScript
{
	public override void OnLoad()
	{
		SetId(1000000);
		SetName("Test Quest");
		SetDescription("This is a simple talk test quest.");
		SetReceive(Receive.OnLogin);

		AddObjective("talk_duncan", "Talk to Duncan", ObjectiveType.Talk, "duncan", 1, 15409, 38310);

		AddReward(RewardType.Exp, 2000);
		AddReward(RewardType.Item, 63016, 2);
		AddReward(RewardType.Gold, 500);
		AddReward(RewardType.Skill, SkillConst.Assault, SkillRank.R2);
		
		AddHook("_duncan", "quests", TalkDuncan);
	}
	
	public IEnumerable TalkDuncan(WorldClient c, NPCScript n)
	{
		if(QuestActive(c, this.Id))
		{
			n.Msg(c, "Great! You tested the test out of that test, thank you!");
			FinishQuestObjective(c, this.Id, "talk_duncan");
			
			n.Msg(c, "Now do this!");
			StartQuest(c, 1000002);
			
			Break();
		}
		End();
	}
}

public class TestQuest2Script : QuestScript
{
	public override void OnLoad()
	{
		SetId(1000001);
		SetName("Test Quest 2");
		SetDescription("This is a simple kill test quest.");
		SetReceive(Receive.OnLogin);

		AddObjective("kill_wolf", "Kill 10 Gray Wolves", ObjectiveType.Kill, 20001, 10);

		AddReward(RewardType.Exp, 5000);
	}
}

public class TestQuest3Script : QuestScript
{
	public override void OnLoad()
	{
		SetId(1000002);
		SetName("Test Quest 3");
		SetDescription("This is a simple kill+talk test quest.");
		SetCancelable();

		AddObjective("kill", "Kill 3 Red Foxes", ObjectiveType.Kill, 50002, 3);
		AddObjective("collect", "Find 2 apples", ObjectiveType.Collect, 50003, 2);
		AddObjective("talk", "Talk to Duncan", ObjectiveType.Talk, "duncan", 1, 15409, 38310);

		AddReward(RewardType.Exp, 10000);
		
		AddHook("_duncan", "quests", TalkDuncan);
	}
	
	public IEnumerable TalkDuncan(WorldClient c, NPCScript n)
	{
		if(QuestActive(c, this.Id, "talk"))
		{
			n.Msg(c, "You did it, I'm impressed!");
			if(c.Character.HasItem(50003, 2))
			{
				FinishQuestObjective(c, this.Id, "talk");
				c.Character.RemoveItem(50003, 2);
			}
			else
				n.Msg(c, "Wait... where are the apples? I'm hungry!");
			
			Break();
		}
		End();
	}
}
