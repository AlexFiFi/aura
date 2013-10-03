using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class ArzulScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_arzul");
		SetRace(8002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 22, eye: 107, eyeColor: 28, lip: 29);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x22CE, 0xFABCD4, 0xFFDD07, 0xF78A39);
		EquipItem(Pocket.Hair, 0x1F5E, 0x414141, 0x414141, 0x414141);
		EquipItem(Pocket.Armor, 0x5A3C, 0xC8CEE8, 0x425883, 0x999999);
		EquipItem(Pocket.RightHand1, 0x9EC9, 0xFFFFFF, 0x282828, 0x282828);

		SetLocation(region: 4005, x: 25287, y: 14225);

		SetDirection(9);
		SetStand("");
	}
}
