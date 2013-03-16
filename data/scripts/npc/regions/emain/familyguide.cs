using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class FamilyguideScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_familyguide");
		SetRace(10002);
		SetBody(height: 0.7699999f, fat: 1.02f, upper: 1f, lower: 1.02f);
		SetFace(skin: 15, eye: 12, eyeColor: 8, lip: 2);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0x1324, 0xF4C7DD, 0xDC0047, 0x143D);
		EquipItem(Pocket.Hair, 0x100E, 0xFFB39897, 0xFFB39897, 0xFFB39897);
		EquipItem(Pocket.Armor, 0x3B14, 0xFF000000, 0xFF3D4363, 0xFF713E5E);
		EquipItem(Pocket.Shoe, 0x4272, 0xFF1D1C31, 0xFFA4755B, 0xFFFFFFFF);

		SetLocation(region: 61, x: 6087, y: 5807);

		SetDirection(20);
		SetStand("");
	}
}
