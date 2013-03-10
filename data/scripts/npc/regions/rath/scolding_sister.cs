using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Scolding_sisterScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_scolding_sister");
		SetRace(10001);
		SetBody(height: 0.9f, fat: 1f, upper: 1f, lower: 0.9f);
		SetFace(skin: 15, eye: 8, eyeColor: 0, lip: 2);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0xF3C, 0xF0377E, 0x2047, 0xCAA5CD);
		EquipItem(Pocket.Hair, 0x138A, 0x624848, 0x624848, 0x624848);
		EquipItem(Pocket.Armor, 0x3C29, 0xD19700, 0x74AB9F, 0xC43232);
		EquipItem(Pocket.Shoe, 0x42AF, 0xD1B4F7, 0x5FAF6A, 0x808080);

		SetLocation(region: 401, x: 123688, y: 122902);

		SetDirection(98);
		SetStand("human/male/anim/male_stand_Tarlach_anguish");

		Phrases.Add("Babies are delivered by storks!");
		Phrases.Add("How many times do I have to tell you?");
	}
}
