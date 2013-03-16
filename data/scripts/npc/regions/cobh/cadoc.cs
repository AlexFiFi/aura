using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class CadocScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_cadoc");
		SetRace(8002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1.3f, lower: 1.2f);
		SetFace(skin: 22, eye: 54, eyeColor: 76, lip: 34);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x22C4, 0xAAAB23, 0xB30058, 0x2E4602);
		EquipItem(Pocket.Hair, 0x1F50, 0x362825, 0x362825, 0x362825);
		EquipItem(Pocket.Armor, 0x32FB, 0x6B4F5F, 0x4A4A4A, 0xAFAE05);
		EquipItem(Pocket.Glove, 0x3F08, 0x808080, 0xB7AFC0, 0x10BAD7);
		EquipItem(Pocket.Shoe, 0x42F1, 0x313A58, 0x455873, 0x8830F0);
		EquipItem(Pocket.RightHand1, 0xABE7, 0x704215, 0x735858, 0x6E6B69);

		SetLocation(region: 23, x: 26460, y: 37337);

		SetDirection(3);
		SetStand("chapter4/giant/male/anim/giant_npc_cadoc");
        
		Phrases.Add("(Shrugs shoulders.)");
		Phrases.Add("A real man has real muscles!");
		Phrases.Add("Admiral Owen is a role model for real men!");
		Phrases.Add("Breathe in, breathe out.");
		Phrases.Add("Fish for sale! Fish for sale!");
		Phrases.Add("Hahah! Bam bam!");
		Phrases.Add("Hmm... Do you envy my muscles?");
		Phrases.Add("Men these days are weaklings.");
		Phrases.Add("Whenever I see a log, I test my stength by picking it up swinging it.");
	}
}
