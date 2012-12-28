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
		base.OnLoad();
		SetName("_tarlachbear");
		SetRace(70001);
		SetBody(.8f);
		SetColor(0x553A26, 0x0000FF00, 0x000000FF);

		SetLocation(region: 48, x: 11100, y: 30400);
		SetDirection(167);

		ServerEvents.Instance.ErinnDaytimeTick += On12HrTick;

		Phrases.Add("Growl...");
		Phrases.Add("Rooooar...");
		Phrases.Add("Rooar...");
		Phrases.Add("Roar...");
		Phrases.Add(".....");
	}

	public override void Dispose()
	{
		ServerEvents.Instance.ErinnDaytimeTick -= On12HrTick;
		base.Dispose();
	}

	private void On12HrTick(object sender, TimeEventArgs e)
	{
		if (e.Hour >= 6 && e.Hour < 18) //Daytime
			Warp(region: 48, x: 11100, y: 30400, flash: false);
		else
			Warp(region: 15, x: 0, y: 0, flash: false);
	}

	public override void OnTalk(WorldClient c)
	{
		Disable(c, Options.Name | Options.Face);
		Msg(c, "The Brown Bear is stout and well built.",
			"Standing solemnly in a white field of snow, it seems to be searching for something.",
			"Steam flows out from the bear's mouth with every breath while the bear sniffs the ground from time to time, using its front paw to dig the grass.",
			"It constantly looks around, seemingly watching out for some kind of threat or danger.");
		Enable(c, Options.Name | Options.Face);
		MsgSelect(c, "Grr...", "Start Conversation", "@talk");
	}

	public override void OnSelect(WorldClient c, string r)
	{
		switch (r)
		{
			case "@talk":
				Msg(c, "...");
				Disable(c, Options.Name);
				Msg(c, "(Bear is waiting for me to say something.)");
				Enable(c, Options.Name);
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