using System;
using Common.World;
using World.Network;
using World.Scripting;
using World.World;
using Common.Constants;
using Common.Events;

public class TarlachScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_tarlach");
		SetRace(10002);
		SetBody(height: 1.1f, fat: 0.9f, upper: 0.4f, lower: 0.6f);
		SetFace(skin: 15, eye: 4, eyeColor: 54, lip: 0);

		EquipItem(Pocket.Face, 4901, 13020774, 4930657, 16399);
		EquipItem(Pocket.Hair, 4021, 268435491, 268435491, 268435491);
		EquipItem(Pocket.Head, 18028, 6446916, 12632256, 6296681);
		EquipItem(Pocket.Robe, 19004, 13269812, 11817009, 14335111); //Muffler robe
		NPC.GetItemInPocket(Pocket.Robe).Info.FigureA = 1;
		EquipItem(Pocket.Armor, 15002, 12058662, 12058662, 5729677);
		EquipItem(Pocket.Shoe, 17009, 5648913, 16571605, 8235060);

		SetLocation(region: 48, x: 11100, y: 30400);

		ServerEvents.Instance.ErinnDaytimeTick += On12HrTick;

		SetDirection(167);

		SetStand("human/male/anim/male_natural_stand_npc_Duncan");

		Phrases.Add("...I'll just wait a little longer...");
		Phrases.Add("Ah...");
		Phrases.Add("(Cough, Cough)");
		Phrases.Add("...Is it not morning yet?");
		Phrases.Add("Sigh...");
		Phrases.Add("...It's definitely cold at night...");
		Phrases.Add("...My head...");
		Phrases.Add("...I can take it...");
		Phrases.Add("I guess not yet...");
	}

	public override void Dispose()
	{
		ServerEvents.Instance.ErinnDaytimeTick -= On12HrTick;
		base.Dispose();
	}

	private void On12HrTick(object sender, TimeEventArgs e)
	{
		if (e.Hour >= 18 || e.Hour < 6) //Nighttime
			Warp(region: 48, x: 11100, y: 30400);
		else
			Warp(region: 15, x: 0, y: 0);
	}

	public override void OnTalk(WorldClient c)
	{
		Disable(c, Options.FaceAndName);
		Msg(c, "A man wearing a light brown robe silently glares this way.", 
			"He has wavy blonde hair and white skin with a well defined chin that gives off a gentle impression.",
			"Behind his thick glasses, however, are his cold emerald eyes filled with silent gloom.");
		Enable(c, Options.FaceAndName);
		MsgSelect(c, "...Mmm...", "Start Conversation", "@talk");
	}

	public override void OnSelect(WorldClient c, string r)
	{
		switch (r)
		{
			case "@talk":
				Msg(c, "(Cough, Cough)...", "So you have made it through the barrier and have reached this desolate place.");
				Disable(c, Options.Name);
				Msg(c, "(Tarlach is looking at me.)");
				Enable(c, Options.Name);
				ShowKeywords(c);
				break;

			default:
				Msg(c, "Can we change the subject?");
				ShowKeywords(c);
				break;
		}
	}

	public override void OnEnd(WorldClient c)
	{
		Close(c, "(You ended your conversation with Tarlach.)");
	}
}