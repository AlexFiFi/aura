using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Taillteann_repairman2Script : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_taillteann_repairman2");
		SetRace(9001);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 17, eye: 37, eyeColor: 168, lip: 0);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x170C, 0x484300, 0xCC002B, 0x87263F);
		EquipItem(Pocket.Hair, 0x138B, 0xC6C3C6, 0xC6C3C6, 0xC6C3C6);
		EquipItem(Pocket.Armor, 0x3BEF, 0x647692, 0x78829D, 0xBFBFBF);
		EquipItem(Pocket.RightHand2, 0x9D21, 0xD8B77E, 0xC67139, 0x0);
		EquipItem(Pocket.LeftHand2, 0x9C46, 0xC5C5C5, 0x6B613B, 0x0);

		SetLocation(region: 300, x: 227281, y: 192457);

		SetDirection(78);
		SetStand("");
        
		Phrases.Add("Please let me know if you want to purchase Elf weapons.");
	}
}
