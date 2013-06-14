// --- Aura Script ----------------------------------------------------------
//  Battle arena props
// --- Description ----------------------------------------------------------
//  Allow entry to arenas
// --------------------------------------------------------------------------

using System;
using Aura.Shared.Const;
using Aura.Shared.Network;
using Aura.Shared.World;
using Aura.Shared.Util;
using Aura.World.Network;
using Aura.World.Player;
using Aura.World.Scripting;
using Aura.World.World;

public class AlbyBooth : BaseScript
{
	MabiVertex[] _revivalLocations = new MabiVertex[]
	{
		new MabiVertex(1800, 4644),
		new MabiVertex(1800, 1845),
		new MabiVertex(4675, 1845),
		new MabiVertex(4675, 4644)
	};

	MabiProp prop;
	public override void OnLoad()
	{
		prop = SpawnProp(0xA0001C00010004, "", "", "", 10100, 28, 1190, 3645, 4.712385f, 1.0f, OnTouch);
		prop.State = "open";
	}

	public void OnTouch(WorldClient c, MabiPC cr, MabiProp pr)
	{
		var r = RandomProvider.Get();
		var p = _revivalLocations[r.Next(_revivalLocations.Length)];
		// Just warp them for now
		c.Warp(29, p.X, p.Y);
	}
}
