using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Taillteann_royalguard1Script : Taillteann_royalguard_baseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_taillteann_human1");
		SetRace(10002);
		SetFace(skin: 16, eye: 5, eyeColor: 0, lip: 0);

		EquipItem(Pocket.Face, 0x1324, 0x7038, 0xFFF218, 0x4CCAEF);
		EquipItem(Pocket.Hair, 0xFC7, 0x211C39, 0x211C39, 0x211C39);
		EquipItem(Pocket.Armor, 0x3BEC, 0x785C3E, 0x633C31, 0x808080);
		EquipItem(Pocket.RightHand2, 0x9C90, 0x808080, 0x6F6F6F, 0x0);

		SetLocation(region: 300, x: 212248, y: 200090);

		SetDirection(200);
	}
}
