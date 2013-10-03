using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class AustellScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_austell");
		SetRace(10002);
		SetBody(height: 1.2f, fat: 1f, upper: 1.2f, lower: 1f);
		SetFace(skin: 17, eye: 41, eyeColor: 192, lip: 1);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x1AFB, 0x36BD9C, 0x800032, 0xF57E56);
		EquipItem(Pocket.Hair, 0x1026, 0xFFD438, 0xFFD438, 0xFFD438);
		EquipItem(Pocket.Armor, 0x3DCC, 0x253445, 0xCCAF0B, 0xA2D00D);
		EquipItem(Pocket.Shoe, 0x4368, 0x2A4949, 0x808080, 0x808080);
		EquipItem(Pocket.RightHand1, 0x9EBE, 0x466C8E, 0x959595, 0x3C4155);

		SetLocation(region: 302, x: 125745, y: 84686);

		SetDirection(52);
		SetStand("human/anim/tool/female_tool_Bhand_M01_playing_lute_01");
        
		Phrases.Add("Hello!");
		Phrases.Add("Is this your first time here?");
		Phrases.Add("Nice to meet you.");
		Phrases.Add("Welcome.");
		Phrases.Add("What can I help you with?");
	}
}
