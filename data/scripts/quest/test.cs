// --- Aura Script ----------------------------------------------------------
//  Test Quests
// --------------------------------------------------------------------------

using Common.Data;
using World.Scripting;

public class TestQuestScript : QuestScript
{
	public override void OnLoad()
	{
		SetId(1000000);
		SetName("Test Quest");
		SetDescription("This is a simple talk test quest.");
		SetReceive(Receive.OnLogin);

		AddObjective("talk_duncan", "Talk to Duncan", true, ObjectiveType.Talk, "duncan", 1, 15409, 38310);

		AddReward(RewardType.Exp, 2000);
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

		AddObjective("kill_wolf", "Kill 10 Gray Wolves", true, ObjectiveType.Kill, 20001, 10);

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

		AddObjective("kill", "Kill 3 Red Foxes", true, ObjectiveType.Kill, 50002, 3);
		AddObjective("talk", "Talk to Duncan", true, ObjectiveType.Talk, "duncan", 1, 15409, 38310);

		AddReward(RewardType.Exp, 10000);
	}
}
