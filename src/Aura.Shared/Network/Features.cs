// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aura.Shared.Network
{
	/// <summary>
	/// Holds information about features, automatically enabled based on
	/// version and region, that require packet changes. This does not
	/// actually enable any server features for now.
	/// </summary>
	public static class FeatureManager
	{
		/// <summary>
		/// Rule setup.
		/// </summary>
		private static void Init()
		{
			// The rules passed to Setup (Enable/Disable) are all checked,
			// one after the other. You have to watch not to disable something
			// that should be enabled, by a following rule.

			Setup(Feature.MD5Passwords, Enable(150100));
			Setup(Feature.Commerce, Enable(150100));
			Setup(Feature.ExpiringPockets, Enable(150100));
			Setup(Feature.ConditionD, Enable(150100));
			Setup(Feature.FarmingTemp, Enable(150100), Disable(170400));
			Setup(Feature.NPCOptions, Enable(150100));
			Setup(Feature.BombEvent, Enable(150100));
			Setup(Feature.UnkAny1, Enable(150100));

			Setup(Feature.DoubleAccName, Enable(160000));
			Setup(Feature.UnkNewShopInfo, Enable(160200));

			Setup(Feature.Talents, Enable(170100));
			Setup(Feature.UnkAny2, Enable(170100));
			Setup(Feature.UnkAny3, Enable(170100));

			Setup(Feature.Shamala, Enable(170300));
			Setup(Feature.UnkAny4, Enable(170300));
			Setup(Feature.NewPVPInfo, Enable(170300));

			Setup(Feature.UnkAny5, Enable(170400));

			Setup(Feature.NewPremiumThing, Enable(170402), Enable(170300, MabiRegion.TW));

			Setup(Feature.UnkNATW1, Enable(170403, MabiRegion.NA), Enable(170300, MabiRegion.TW));
			Setup(Feature.UnkJP1, Enable(MabiRegion.JP));

			Setup(Feature.UnkAny6, Enable(180100));
			Setup(Feature.ZeroTalent, Enable(180100));
			Setup(Feature.NewPoisonCrInfo, Enable(180100));

			Setup(Feature.NewUnkCrInfo, Enable(180300));
		}

		// First and only region >=| But we seriously won't ever care about that code again...
		#region Where the magic happens~
		static FeatureManager()
		{
			_enabled = new bool[(int)Feature.Max];

			Init();
		}

		public static bool IsEnabled(this Feature feature)
		{
			return _enabled[(int)feature];
		}

		private static TargetSet Enable(MabiRegion region)
		{ return Enable(0, region); }

		private static TargetSet Enable(uint version = 0, MabiRegion region = MabiRegion.Any)
		{
			return new TargetSet(version, region, true);
		}

		private static TargetSet Disable(MabiRegion region)
		{ return Disable(0, region); }

		private static TargetSet Disable(uint version = 0, MabiRegion region = MabiRegion.Any)
		{
			return new TargetSet(version, region, false);
		}

		private static void Setup(Feature feature, params TargetSet[] sets)
		{
			foreach (var set in sets)
			{
				if (Op.Version >= set.Version && (set.Region == Op.Region || set.Region == MabiRegion.Any))
					_enabled[(int)feature] = set.Enabled;
			}
		}

		private static bool[] _enabled;

		private class TargetSet
		{
			public uint Version { get; protected set; }
			public MabiRegion Region { get; protected set; }
			public bool Enabled { get; set; }

			public TargetSet(uint version, MabiRegion region, bool enabled)
			{
				this.Version = version;
				this.Region = region;
				this.Enabled = enabled;
			}
		}
		#endregion
	}

	public enum Feature
	{
		/// <summary>
		/// Post G14 clients hash the passwords before sending them.
		/// </summary>
		MD5Passwords,

		/// <summary>
		/// Commerce info in creature packets.
		/// </summary>
		Commerce,

		/// <summary>
		/// Support for expiring pockets, like Style, in 5209.
		/// </summary>
		ExpiringPockets,

		/// <summary>
		/// Additional condition long.
		/// </summary>
		ConditionD,

		/// <summary>
		/// Unk farming information; existed between G15 and G17 in Creature.
		/// </summary>
		FarmingTemp,

		/// <summary>
		/// Special NPC information in creature packet.
		/// </summary>
		NPCOptions,

		/// <summary>
		/// Some bomb even information for creature.
		/// </summary>
		BombEvent,

		/// <summary>
		/// Int after skills in creatures.
		/// </summary>
		UnkAny1,

		/// <summary>
		/// Long after conditions in creatures.
		/// </summary>
		UnkAny2,

		/// <summary>
		/// New values after followers in characters.
		/// </summary>
		UnkAny3,

		/// <summary>
		/// New values after events in creatures.
		/// </summary>
		UnkAny4,

		/// <summary>
		/// Repeating acc name in login packets.
		/// </summary>
		DoubleAccName,

		/// <summary>
		/// New byte in shops.
		/// </summary>
		UnkNewShopInfo,

		/// <summary>
		/// Talent information in creatures.
		/// </summary>
		Talents,

		/// <summary>
		/// Shamala transformation and diary info in creatures.
		/// </summary>
		Shamala,

		/// <summary>
		/// New values in PvP information in creatures.
		/// </summary>
		NewPVPInfo,

		/// <summary>
		/// New byte in creatures.
		/// </summary>
		UnkAny5,

		/// <summary>
		/// New premium information in 5209 and on login.
		/// </summary>
		NewPremiumThing,

		/// <summary>
		/// Values only found in NA in creatures.
		/// </summary>
		UnkNATW1,

		/// <summary>
		/// Int found in JP only, in 5209.
		/// </summary>
		UnkJP1,

		/// <summary>
		/// New values after Shamala in 5209.
		/// </summary>
		UnkAny6,

		/// <summary>
		/// New talent information in creatures.
		/// </summary>
		ZeroTalent, // ?

		/// <summary>
		/// New values between poison information in creatures.
		/// </summary>
		NewPoisonCrInfo, // ?

		/// <summary>
		/// New values in creatures.
		/// </summary>
		NewUnkCrInfo,

		// ------------------------------------------------------------------

		/// <summary>
		/// Indexer, don't mind.
		/// </summary>
		Max,
	}
}
