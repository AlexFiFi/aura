// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aura.Data
{
	public class ExpInfo
	{
		//public uint Race;
		public ushort Level;
		public uint Exp;
	}

	public class ExpDb : DatabaseCSV<ExpInfo>
	{
		/// <summary>
		/// Returns total exp required to reach the level after the given one.
		/// </summary>
		/// <param name="currentLv"></param>
		/// <returns></returns>
		public uint GetTotalForNextLevel(ushort currentLv)
		{
			uint result = 0;

			if (currentLv >= this.Entries.Count)
				currentLv = (ushort)this.Entries.Count;

			for (ushort i = 0; i < currentLv; ++i)
			{
				result += this.Entries[i].Exp;
			}

			return result;
		}

		/// <summary>
		/// Returns the exp required for the next level.
		/// </summary>
		/// <param name="currentLv"></param>
		/// <returns></returns>
		public uint GetForLevel(ushort currentLv)
		{
			if (currentLv < 1)
				return 0;
			if (currentLv > this.GetMaxLevel())
				currentLv = (ushort)this.Entries.Count;

			currentLv -= 1;
			return this.Entries[currentLv].Exp;
		}

		/// <summary>
		/// Calculates exp remaining till the next level. Required for stat update.
		/// </summary>
		/// <param name="currentLv"></param>
		/// <param name="totalExp"></param>
		/// <returns></returns>
		public ulong CalculateRemaining(ushort currentLv, ulong totalExp)
		{
			return this.GetForLevel(currentLv) - (this.GetTotalForNextLevel(currentLv) - totalExp) + this.GetForLevel((ushort)(currentLv - 1));
		}

		public ushort GetMaxLevel()
		{
			return (ushort)this.Entries.Count;
		}

		protected override void ReadEntry(CSVEntry entry)
		{
			// Replace previous values if there is more than 1 line.
			this.Entries = new List<ExpInfo>(entry.Count);

			while (!entry.End)
			{
				var info = new ExpInfo();
				info.Level = (ushort)(entry.Pointer + 1);
				info.Exp = entry.ReadUInt();

				this.Entries.Add(info);
			}
		}
	}
}
