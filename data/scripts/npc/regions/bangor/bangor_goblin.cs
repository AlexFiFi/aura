using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Bangor_goblinScript : CommerceGoblinScript
{
	public override void OnLoad()
	{
		base.OnLoad();
        base.OnLoad();
        
		SetName("_bangor_goblin");
		SetRace(322);
		SetFace(skin: 168, eye: 3, eyeColor: 7, lip: 2);

		NPC.ColorA = 0xCA9045;
		NPC.ColorB = 0x6C6D6C;
		NPC.ColorC = 0x41AF3D;		

		EquipItem(Pocket.RightHand1, 0x9DF4, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.LeftHand1, 0xB3C7, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 31, x: 13225, y: 22523);

		SetDirection(160);
	}
}
