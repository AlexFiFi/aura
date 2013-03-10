// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aura.World.Player
{
	/// <summary>
	/// Infrmation holder about available transformations for MabiPC.
	/// </summary>
	public class ShamalaTransformation
	{
		public uint Id { get; set; }
		public byte Counter { get; set; }
		public ShamalaState State { get; set; }

		public ShamalaTransformation(uint id, byte count = 0, ShamalaState state = ShamalaState.None)
		{
			this.Id = id;
			this.Counter = count;
			this.State = state;
		}
	}

	public enum ShamalaState : byte { None = 0, Hunted = 1, Available = 2 }
}
