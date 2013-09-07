// --- Aura Script ----------------------------------------------------------
//  Chalk on death
// --- Description ----------------------------------------------------------
//  Spawns chalk outline (prop) when a creature dies.
// --- By -------------------------------------------------------------------
//  Xcelled, exec
// --------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Aura.Shared.Network;
using Aura.World.Events;
using Aura.World.Player;
using Aura.World.Scripting;
using Aura.World.World;

public class ChalkOnDeathScript : BaseScript
{
	public override void OnLoad()
	{
		// -- <configuration> -----------------------------------------------
		
		// Enable for players?
		players = true;
		
		// Enable for NPCs (monsters)?
		npcs = true;
		
		// Keep chalk till next reload?
		permanent = false;
		
		// Minutes to keep chalk, if not permanent?
		duration = 5;

		// -- </configuration> ----------------------------------------------
	}

	private bool players, npcs, permanent;
	private int duration;
	
	protected override void Subscribe()
	{
		EventManager.CreatureEvents.CreatureKilled += OnCreatureKilled;
	}

	protected override void Unsubscribe()
	{
		EventManager.CreatureEvents.CreatureKilled -= OnCreatureKilled;
	}

	public void OnCreatureKilled(MabiCreature victim, MabiCreature killer)
	{
		if((victim is MabiPC && players) || (victim is MabiNPC && npcs))
		{
			var pos = victim.GetPosition();
			var prop = new MabiProp(50, victim.Region, pos.X, pos.Y, (float)Math.PI * 2 / 255 * victim.Direction);
			if(!permanent)
				prop.DisappearTime = DateTime.Now.AddMinutes(duration);

			WorldManager.Instance.AddProp(prop);
		}
	}
}
