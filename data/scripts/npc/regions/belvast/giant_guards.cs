// Aura Script
// --------------------------------------------------------------------------
// Giant Guards
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class _GiantGuardBelfastBaseScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetRace(8002);
		SetBody(height: 1.6f, fat: 1f, upper: 1.3f, lower: 1.2f);
		SetFace(skin: 19, eye: 45, eyeColor: 26, lip: 26);
		SetColor(0x808080, 0x808080, 0x808080);	

		EquipItem(Pocket.Hair, 0x1F53, 0x99CCCC, 0x99CCCC, 0x99CCCC);
		EquipItem(Pocket.Armor, 0x32FD, 0x828182, 0xD2D46, 0x6F605E);
		EquipItem(Pocket.Glove, 0x407D, 0x828182, 0xD2D46, 0x6F605E);
		EquipItem(Pocket.Shoe, 0x428C, 0x828182, 0xD2D46, 0x6F605E);
		EquipItem(Pocket.Head, 0x485D, 0x524F5D, 0x846707, 0x579247);
		EquipItem(Pocket.RightHand2, 0x9CEC, 0x434343, 0x9F9F9F, 0xBFBFBF);
		EquipItem(Pocket.LeftHand2, 0xB3C3, 0x7D4827, 0x37727F, 0x809092);

		Phrases.Add("Good job getting here.");
		Phrases.Add("The admiral is inside.");
		Phrases.Add("What is this about?");
	}
}

public class GiantGuardBelfast1Script : _GiantGuardBelfastBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_giantguard_belfast_01");
		SetLocation(4005, 23520, 29900);

		EquipItem(Pocket.Face, 0x22C4, 0xD9B055, 0xFB9C49, 0x97737F);
	}
}

public class GiantGuardBelfast2Script : _GiantGuardBelfastBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_giantguard_belfast_02");
		SetLocation(region: 4005, x: 23520, y: 28760);

		EquipItem(Pocket.Face, 0x22C4, 0x5327, 0xEBE92C, 0xF7775B);
	}
}
