// --- Aura Script ----------------------------------------------------------
//  Oriental Town
// --- Description ----------------------------------------------------------
//  Creates portal near Ciar Dungeon, to reach the Oriental Town region.
// --- By -------------------------------------------------------------------
//  Miro, exec
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class OrientalTownProps : BaseScript
{
	public override void OnLoad()
	{
		SpawnProp(42935, "tir", 32748, 27200, 2, 1f);
		SpawnProp(41121, "tir", 32748, 27150, 0.5f, 2f, PropWarp("orient", 2503, 4929));
		DefineProp(45294583369760777, "orient", 2203, 4892, PropWarp("tir", 32627, 27613)); 
	}
}