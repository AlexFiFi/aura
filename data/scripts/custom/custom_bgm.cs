// --- Aura Script ----------------------------------------------------------
//  Custom BGM
// --- Description ----------------------------------------------------------
//  Automatically changes BGM upon entering, depending on configuration.
// --- By -------------------------------------------------------------------
//  exec
// --------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Aura.Shared.Network;
using Aura.World.Events;
using Aura.World.Player;
using Aura.World.Scripting;
using Aura.World.World;

public class CustomBGMScript : BaseScript
{
	private void Init()
	{
		// -- <configuration> -----------------------------------------------

		// Add(<region>, <file name>[, <play type>]);
		// The files reside in the mp3 folder; play type can be Once,
		// or Indefinitely (the default, if omitted).
		
		//Add(1, "NPC_Castanea.mp3", PlayType.Once);
		//Add(3001, "Field_Osna_Sail.mp3");

		// -- </configuration> ----------------------------------------------
	}

	private Dictionary<ulong, string> _playerStorage = new Dictionary<ulong, string>();
	private Dictionary<uint, Tuple<string, PlayType>> _regions = new Dictionary<uint, Tuple<string, PlayType>>();

	private void Add(uint region, string bgm, PlayType type = PlayType.Indefinitely)
	{
		_regions[region] = new Tuple<string, PlayType>(bgm, type);
	}

	public override void OnLoad()
	{
		Init();
		EntityEvents.Instance.PlayerChangesRegion += OnPlayerChangesRegion;
	}

	public override void Dispose()
	{
		EntityEvents.Instance.PlayerChangesRegion -= OnPlayerChangesRegion;

		base.Dispose();
	}

	public void OnPlayerChangesRegion(object sender, EntityEventArgs args)
	{
		var creature = sender as MabiPC;
		if (creature == null)
			return;

		Tuple<string, PlayType> info;
		_regions.TryGetValue(creature.Region, out info);
		if (info != null)
		{
			SendSetBGM(creature, info.Item1, info.Item2);
			lock (_playerStorage)
				_playerStorage[creature.Id] = info.Item1;
		}
		else if (_playerStorage.ContainsKey(creature.Id))
		{
			SendUnsetBGM(creature, _playerStorage[creature.Id]);
			lock (_playerStorage)
				_playerStorage.Remove(creature.Id);
		}
	}

	private void SendSetBGM(MabiCreature creature, string file, PlayType type)
	{
		var p = new MabiPacket(Op.SetBgm, creature.Id);
		p.PutString(file);
		p.PutInt((uint)type);

		creature.Client.Send(p);
	}

	private void SendUnsetBGM(MabiCreature creature, string file)
	{
		var p = new MabiPacket(Op.UnsetBgm, creature.Id);
		p.PutString(file);

		creature.Client.Send(p);
	}

	private enum PlayType : uint { Indefinitely = 0, Once = 1 }
}
