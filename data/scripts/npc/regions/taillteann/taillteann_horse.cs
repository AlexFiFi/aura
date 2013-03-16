using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Taillteann_horseScript : CommerceHorseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_taillteann_horse");
        
		SetColor(0x808780, 0x180C0C, 0x808080);

		SetLocation(region: 300, x: 239713, y: 192902);

		SetDirection(200);
	}
}
