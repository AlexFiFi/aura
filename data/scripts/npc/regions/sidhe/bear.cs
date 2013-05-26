// Aura Script
// --------------------------------------------------------------------------
// Tarlach (Bear)
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Events;
using Aura.Shared.Util;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class TarlachBearScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_tarlachbear");
		SetRace(70001);
		SetBody(0.8f);
		SetColor(0x553A26, 0x00FF00, 0x0000FF);
		SetLocation("sidhe_south", 11100, 30400, 167);

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
		if(!e.Time.IsNight)
			WarpNPC(48, 11100, 30400, false);
		else
			WarpNPC(15, 0, 0, false);
	}

	public override IEnumerable OnTalk(WorldClient c)
	{
		Msg(c, Options.FaceAndName, "The Brown Bear is stout and well built.<br/>Standing solemnly in a white field of snow, it seems to be searching for something.<br/>Steam flows out from the bear's mouth with every breath while the bear sniffs the ground from time to time, using its front paw to dig the grass.<br/>It constantly looks around, seemingly watching out for some kind of threat or danger.");
		Msg(c, "Grr...", Button("Start Conversation", "@talk"));
		
		var r = Select(c);
		if(r == "@talk")
		{
			Msg(c, "...");
			
		L_Keywords:
			Msg(c, Options.Name, "(Bear is waiting for me to say something.)");
			ShowKeywords(c);
			
			var keyword = Select(c);
			
				Msg(c, ".....<br/>(I can't possibly communicate with it.)");
			goto L_Keywords;
		}
	}

	public override void OnEnd(WorldClient c)
	{
		Close(c, "(You ended your conversation with Bear.)");
	}
}