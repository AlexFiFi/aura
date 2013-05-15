using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aura.Shared.Const;

namespace Aura.World.World
{
	/// <summary>
	/// Holds information that creatures only need for a short period of time.
	/// </summary>
	public class CreatureTemp
	{
		// Skills
		// ------------------------------------------------------------------
		public PlayingQuality PlayingInstrumentQuality;

		/// <summary>
		/// Primary item used in skill handlers.
		/// (e.g. Phoenix Feather)
		/// </summary>
		public MabiItem SkillItem1;

		/// <summary>
		/// Secondary item used in skill handlers.
		/// (e.g. Dye + Item)
		/// </summary>
		public MabiItem SkillItem2;

		public byte[] DyeCursors;
	}
}
