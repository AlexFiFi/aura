using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class NerysScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_nerys");
		SetRace(10001);
		SetBody(height: 0.9f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 16, eye: 4, eyeColor: 31, lip: 0);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0xF3C, 0xF79E59, 0xF79D38, 0x573295);
		EquipItem(Pocket.Hair, 0xBCF, 0x994433, 0x994433, 0x994433);
		EquipItem(Pocket.Armor, 0x3AC3, 0x94C1C5, 0x6C9D9A, 0xBE8C92);
		EquipItem(Pocket.Glove, 0x3E88, 0x818775, 0x117C7D, 0xA3DC);
		EquipItem(Pocket.Shoe, 0x4269, 0x823021, 0x82C991, 0xF2597B);

		SetLocation(region: 14, x: 44229, y: 35842);

		SetDirection(139);
		SetStand("human/female/anim/female_natural_stand_npc_Nerys");
        
        Phrases.Add("At this rate, I won't have enough arrows next month...");
		Phrases.Add("Do you need something else?");
		Phrases.Add("I should have gone on the trip myself...");
		Phrases.Add("Manus is showing off his muscles again...");
		Phrases.Add("See something you like?");
		Phrases.Add("There are so many weapon repair requests this month.");
		Phrases.Add("This way, people. This way.");
		Phrases.Add("Wait, I shouldn't be doing this right now.");
	}
}
