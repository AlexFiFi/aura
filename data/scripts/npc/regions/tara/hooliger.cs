using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class HooligerScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_hooliger");
		SetRace(10002);
		SetBody(height: 1.1f, fat: 1.3f, upper: 1.1f, lower: 1.1f);
		SetFace(skin: 23, eye: 0, eyeColor: 0, lip: 0);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x1324, 0x939A, 0x1AB67C, 0x495000);
		EquipItem(Pocket.Hair, 0xFAE, 0x747373, 0x747373, 0x747373);
		EquipItem(Pocket.Armor, 0x3BA2, 0x858300, 0x54AB8E, 0xC39238);
		EquipItem(Pocket.Glove, 0xEA80, 0xFFFFFF, 0xD8A200, 0x236BA4);
		EquipItem(Pocket.Shoe, 0x42F7, 0xCABEA7, 0x484848, 0x612650);
		EquipItem(Pocket.RightHand1, 0x9D16, 0xB08000, 0xD0B8A6, 0x5B4768);

		SetLocation(region: 428, x: 68438, y: 106267);

		SetDirection(31);
		SetStand("");

		Phrases.Add("I called it!");
		Phrases.Add("Whoooooooa!");
	}
}
