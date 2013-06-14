using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aura.Shared.Const
{
	/// <summary>
	/// These are technically flags, but the order they're sent to the client matters
	/// </summary>
	public enum DeadMenuOptions : uint
	{
		// Use bit-shift notation to more easily see the client option
		// Client option = shift + 1
		None = 0,
		Town =				1 << 0,
		Here =				1 << 1,
		DungeonEntrance =	1 << 2,
		StatueOfGoddess =	1 << 3,
		ArenaSide =			1 << 4,
		ArenaLobby =		1 << 5,
		NaoRevival1 =		1 << 7,
		WaitForRescue =		1 << 8,
		FeatherUp =			1 << 9,
		BarriLobby =		1 << 11,
		TirChonaill =		1 << 15,
		HereNoPenalty =		1 << 17,
		HerePvP =			1 << 18,
		InCamp =			1 << 19,
		ArenaWaitingRoom =	1 << 20,
		NaoStone =			1 << 26
	}

	public static class DeadMenuHelper
	{
		/// <summary>
		/// Text-ifys the options. If more than one flag is set, the order of the
		/// options returned is abitrary
		/// </summary>
		/// <param name="options"></param>
		/// <returns></returns>
		public static IEnumerable<string> GetStrings(DeadMenuOptions options)
		{
			List<string> ret = new List<string>();

			if ((options & DeadMenuOptions.ArenaLobby) != 0)
				ret.Add("arena_lobby");
			if ((options & DeadMenuOptions.ArenaSide) != 0)
				ret.Add("arena_side");
			if ((options & DeadMenuOptions.ArenaWaitingRoom) != 0)
				ret.Add("arena_waiting");
			if ((options & DeadMenuOptions.BarriLobby) != 0)
				ret.Add("barri_lobby");
			if ((options & DeadMenuOptions.NaoStone) != 0)
				ret.Add("naocoupon");
			if ((options & DeadMenuOptions.DungeonEntrance) != 0)
				ret.Add("dungeon_lobby");
			if ((options & DeadMenuOptions.Here) != 0)
				ret.Add("here");
			if ((options & DeadMenuOptions.HereNoPenalty) != 0)
				ret.Add("trnsfrm_pvp_here");
			if ((options & DeadMenuOptions.HerePvP) != 0)
				ret.Add("showdown_pvp_here");
			if ((options & DeadMenuOptions.InCamp) != 0)
				ret.Add("camp");
			if ((options & DeadMenuOptions.StatueOfGoddess) != 0)
				ret.Add("dungeon_statue");
			if ((options & DeadMenuOptions.TirChonaill) != 0)
				ret.Add("tirchonaill");
			if ((options & DeadMenuOptions.Town) != 0)
				ret.Add("town");
			if ((options & DeadMenuOptions.WaitForRescue) != 0)
				ret.Add("stay");

			return ret;
		}

		/// <summary>
		/// Takes an array of revival options and returns a string suitable for sending to the client.
		/// </summary>
		/// <param name="options"></param>
		/// <returns></returns>
		public static string ConvertToClientString(IEnumerable<string> options)
		{
			return string.Join(";", options);
		}

		public static DeadMenuOptions ConvertFromClientOption(uint option)
		{
			if (option == 0)
				return DeadMenuOptions.None;

			return (DeadMenuOptions)(1 << (int)--option);
		}

		public static uint ConvertToClientOption(DeadMenuOptions option)
		{
			uint opt = (uint)option; // Avoid backfill
			uint count = 0;
			while (opt != 0)
			{
				count++;
				opt >>= 1;
			}

			return count;
		}
	}

	public enum DeathCauses
	{
		None = 0,
		Mob,
		PvP,
		EvG,
		TransPvP,
		Arena
	}
}
