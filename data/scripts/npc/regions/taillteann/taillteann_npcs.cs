using System;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class _TaillteannNpcBaseScript : NPCScript
{
	public override void OnLoad()
	{
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);

        Phrases.Add("...");
		Phrases.Add("I like windy days.");
		Phrases.Add("I should take a walk. I ate too much.");
		Phrases.Add("I spent too much money yesterday on Enchants.");
		Phrases.Add("I want a pretty black dress.");
		Phrases.Add("Nice weather we're having.");
		Phrases.Add("Why isn't he here yet?");
    }
}

public class TaillteannNpc1Script : _TaillteannNpcBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_taillteann_npc1");
		SetRace(10001);
		SetFace(skin: 15, eye: 32, eyeColor: 0, lip: 14);
		SetLocation(300, 213382, 191852, 237);

		EquipItem(Pocket.Face, 0xF3C, 0x14B473, 0xD69F20, 0xFDAE4E);
		EquipItem(Pocket.Hair, 0xBDF, 0xFFF38C, 0xFFF38C, 0xFFF38C);
		EquipItem(Pocket.Armor, 0x3BF7, 0x4A4A61, 0xCC6633, 0xADAEC6);
		EquipItem(Pocket.Shoe, 0x42F2, 0x211C39, 0x0, 0x0);
		EquipItem(Pocket.Head, 0x4657, 0x424563, 0x8472BD, 0x717600);
	}
}

public class TaillteannNpc2Script : _TaillteannNpcBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_taillteann_npc2");
		SetRace(10001);
		SetFace(skin: 15, eye: 32, eyeColor: 0, lip: 14);
		SetLocation(300, 226148, 191802, 159);

		EquipItem(Pocket.Face, 0xF3C, 0x332E, 0xE19A50, 0xD2847B);
		EquipItem(Pocket.Hair, 0xBE3, 0xFFF38C, 0xFFF38C, 0xFFF38C);
		EquipItem(Pocket.Armor, 0x3BF7, 0x4A4A61, 0xCC6633, 0xADAEC6);
		EquipItem(Pocket.Shoe, 0x42F2, 0x211C39, 0x0, 0x0);
		EquipItem(Pocket.Head, 0x4657, 0x424563, 0x8472BD, 0x717600);
	}
}

public class TaillteannNpc3Script : _TaillteannNpcBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_taillteann_npc3");
		SetRace(10002);
		SetFace(skin: 15, eye: 15, eyeColor: 0, lip: 6);
		SetLocation(300, 223266, 195458, 176);

		EquipItem(Pocket.Face, 0x1324, 0xBED22E, 0xFAB150, 0xA3807D);
		EquipItem(Pocket.Hair, 0xFC8, 0xCC3300, 0xCC3300, 0xCC3300);
		EquipItem(Pocket.Armor, 0x3BF7, 0x545743, 0x6281AA, 0x93494E);
		EquipItem(Pocket.Shoe, 0x42F2, 0x663300, 0x0, 0x0);
	}
}

public class TaillteannNpc4Script : _TaillteannNpcBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_taillteann_npc4");
		SetRace(10001);
		SetFace(skin: 15, eye: 33, eyeColor: 0, lip: 23);
		SetLocation(300, 212476, 192867, 114);

		EquipItem(Pocket.Face, 0xF3C, 0xDCDD45, 0xA37507, 0x5D2E92);
		EquipItem(Pocket.Hair, 0xBE3, 0x173900, 0x173900, 0x173900);
		EquipItem(Pocket.Armor, 0x3AC1, 0xFFFFFF, 0x0, 0xFFFFFF);
		EquipItem(Pocket.Shoe, 0x4274, 0x0, 0x0, 0x808080);
	}
}

public class TaillteannNpc5Script : _TaillteannNpcBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_taillteann_npc5");
		SetRace(10002);
		SetFace(skin: 19, eye: 23, eyeColor: 76, lip: 15);
		SetLocation(300, 210462, 193062, 177);

		EquipItem(Pocket.Face, 0x1324, 0xB257, 0xFEE3D7, 0xF54230);
		EquipItem(Pocket.Hair, 0xFC8, 0xFFCC00, 0xFFCC00, 0xFFCC00);
		EquipItem(Pocket.Armor, 0x3A9B, 0x94C384, 0xBF8941, 0x4E3578);
		EquipItem(Pocket.Shoe, 0x429A, 0x666633, 0x666699, 0x808080);
		EquipItem(Pocket.Robe, 0x4A39, 0x666666, 0xEDB300, 0x5727F4);
	}
}

public class TaillteannNpc6Script : _TaillteannNpcBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_taillteann_npc6");
		SetRace(10001);
		SetFace(skin: 15, eye: 7, eyeColor: 0, lip: 24);
		SetLocation(300, 213822, 196933, 121);

		EquipItem(Pocket.Face, 0xF3C, 0x6F6C6E, 0x714856, 0x3AB5C);
		EquipItem(Pocket.Hair, 0xBD8, 0x9C5D42, 0x9C5D42, 0x9C5D42);
		EquipItem(Pocket.Armor, 0x3AEA, 0xEFE3B5, 0x663300, 0xE3E3FC);
		EquipItem(Pocket.Shoe, 0x426F, 0x996600, 0xC7E8FF, 0x808080);
	}
}
