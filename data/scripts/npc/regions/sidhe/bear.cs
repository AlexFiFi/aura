using System;
using Common.World;
using World.Network;
using World.Scripting;
using World.World;
using Common.Constants;
using Common.Events;

public class TarlachBearScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_tarlachbear");
		SetRace(70001);
		SetBody(.8f);

		SetLocation(region: 48, x: 11100, y: 30400);

		ServerEvents.Instance.Erinn12HourTick += On12HrTick;

		SetDirection(167);

		NPC.ColorA = 0x00553A26;
		NPC.ColorB = 0x0000FF00;
		NPC.ColorC = 0x000000FF;

		Phrases.Add("Growl...");
		Phrases.Add("Rooooar...");
		Phrases.Add("Rooar...");
		Phrases.Add("Roar...");
		Phrases.Add(".....");
	}

	public override void Dispose()
	{
		ServerEvents.Instance.Erinn12HourTick -= On12HrTick;
		base.Dispose();
	}

	private void On12HrTick(object sender, TimeEventArgs e)
	{
		if (e.Hour >= 6 && e.Hour < 18) //Daytime
			SetLocation(region: 48, x: 11100, y: 30400);
		else
			SetLocation(region: 15, x: 0, y: 0);
	}

	public override void OnTalk(WorldClient c)
	{
		Msg(c, false, true, "The Brown Bear is stout and well built.",
			"Standing solemnly in a white field of snow, it seems to be searching for something.",
			"Steam flows out from the bear's mouth with every breath while the bear sniffs the ground from time to time, using its front paw to dig the grass.",
			"It constantly looks around, seemingly watching out for some kind of threat or danger.");
		MsgSelect(c, "Grr...", "Start Conversation", "@talk");
	}

	public override void OnSelect(WorldClient c, string r)
	{
		switch (r)
		{
			case "@talk":
				Msg(c, "...");
				Msg(c, true, false, "(Bear is waiting for me to say something.)");
				ShowKeywords(c);
				break;
			default:
				Msg(c, ".....");
				Msg(c, "(I can't possibly communicate with it.)");
				ShowKeywords(c);
				break;
		}
	}

	public override void OnEnd(WorldClient c)
	{
		Close(c, "(You ended your conversation with Bear.)");
	}
}