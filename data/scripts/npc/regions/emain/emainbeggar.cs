using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class EmainbeggarScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_emainbeggar");
		SetRace(10002);
		SetBody(height: 0.6199999f, fat: 0.94f, upper: 1f, lower: 1f);
		SetFace(skin: 15, eye: 0, eyeColor: 27, lip: 0);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;

		EquipItem(Pocket.Face, 0x1324, 0xD9BE03, 0xFBBD59, 0xFBB758);
		EquipItem(Pocket.Hair, 0xFA6, 0xA2917B, 0xA2917B, 0xA2917B);
		EquipItem(Pocket.Robe, 0x4A39, 0x83827C, 0xFFFFFF, 0xFFFFFF);
        
        NPC.GetItemInPocket(Pocket.Robe).Info.FigureA = 1;

		SetLocation(region: 52, x: 41435, y: 42285);

		SetDirection(190);
		SetStand("human/anim/male_natural_sit_01");
        
		Phrases.Add("Come on, you know you got some!");
		Phrases.Add("Got some Gold");
		Phrases.Add("Help out the poor. You'll be blessed with longevity!");
		Phrases.Add("Hook up a brother!");
		Phrases.Add("Spare change, pleeeeeease");
		Phrases.Add("Spare change?");
		Phrases.Add("You know I'm here!!");
	}
}
