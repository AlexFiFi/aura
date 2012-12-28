using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class HagelScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_hagel");
		SetRace(9002);
		SetBody(height: 1.3f, fat: 1f, upper: 1.2f, lower: 1f);
		SetFace(skin: 20, eye: 38, eyeColor: 27, lip: 0);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0x1AF4, 0x550000, 0x36566E, 0x9075B6);
		EquipItem(Pocket.Hair, 0xFC6, 0xE8EFDC, 0xE8EFDC, 0xE8EFDC);
		EquipItem(Pocket.Armor, 0x36C9, 0xA39984, 0x24280D, 0x143422);
		EquipItem(Pocket.Glove, 0x408F, 0x4C5A35, 0x25302B, 0x808080);
		EquipItem(Pocket.Shoe, 0x4469, 0x222A30, 0x4D5475, 0xA0A686);

		SetLocation(region: 3100, x: 366322, y: 425294);

		SetDirection(218);
		SetStand("elf/male/anim/elf_npc_hagel_stand_friendly");
        
		Phrases.Add("Dangers lurk everywhere. It should be accepted.");
		Phrases.Add("Every job yields a reward.");
		Phrases.Add("Hope is all about persistently working on yourself.");
		Phrases.Add("If you have any courage left, just keep challenging yourself!");
		Phrases.Add("Just like the hidden artifacts underneath the desert sand, the secret will be revealed one day.");
		Phrases.Add("No matter where you are, it's important not to lose your sense of direction.");
		Phrases.Add("The thing that bothers me is the sand stuck inside your shoes...");
		Phrases.Add("The truth will be revealed one day.");
		Phrases.Add("When will the sand dust settle down...");
	}
}
