using System;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class _TaillteannGiantBaseScript : NPCScript
{
	public override void OnLoad()
	{
		SetRace(8002);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		
		EquipItem(Pocket.Armor, 0x3BF0, 0x2A2719, 0x8E8B78, 0x808080);
		EquipItem(Pocket.RightHand2, 0x9CF0, 0x232323, 0x615739, 0x0);

		Phrases.Add("I miss Vales...");
		Phrases.Add("I wonder how long the Princess plans to stay here.");
		Phrases.Add("It is so strange that it does not snow here.");
	}
}

public class TaillteannGiant1Script : _TaillteannGiantBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_taillteann_giant1");
		SetFace(skin: 23, eye: 45, eyeColor: 27, lip: 28);
		SetLocation(300, 205354, 191114, 7);

		EquipItem(Pocket.Face, 0x22C4, 0x670061, 0x840F69, 0xCF9461);
		EquipItem(Pocket.Hair, 0x1F51, 0x393839, 0x393839, 0x393839);
	}
}

public class TaillteannGiant2Script : _TaillteannGiantBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_taillteann_giant2");
		SetFace(skin: 19, eye: 53, eyeColor: 27, lip: 27);
		SetLocation(300, 205354, 191814, 7);

		EquipItem(Pocket.Face, 0x22C4, 0x626B58, 0xA5C4E6, 0xBEAD0A);
		EquipItem(Pocket.Hair, 0x1F56, 0xCC9933, 0xCC9933, 0xCC9933);
	}
}
