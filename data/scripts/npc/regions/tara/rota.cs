using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class RotaScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_rota");
		SetRace(9001);
		SetBody(height: 1.2f, fat: 1f, upper: 1f, lower: 1.1f);
		SetFace(skin: 15, eye: 32, eyeColor: 0, lip: 0);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0x170C, 0xFA9C59, 0xF9AFAD, 0x19945);
		EquipItem(Pocket.Hair, 0x138B, 0xD0B171, 0xD0B171, 0xD0B171);
		EquipItem(Pocket.Armor, 0x36DD, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Glove, 0x409C, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Shoe, 0x42E7, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Head, 0x487C, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.RightHand1, 0x9D60, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 428, x: 67852, y: 107566);

		SetDirection(43);
		SetStand("");

		Phrases.Add("Ah...men! Hehe.");
		Phrases.Add("Brace yourself, it might hurt a bit.");
		Phrases.Add("Hehe, don't be afraid.");
	}
}
