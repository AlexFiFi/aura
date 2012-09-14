// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;

namespace Common.World
{
	public class MabiCard
	{
		private static UInt64 _index = 0x0001000000000000;

		public UInt64 Id;
		public uint Race;
		public bool InDatabase;

		private static UInt64 _getIndex()
		{
			return ++_index;
		}

		public MabiCard(uint type)
		{
			InDatabase = false;
			Id = _getIndex();
			Race = type;
		}

		public MabiCard(UInt64 id, uint type)
		{
			InDatabase = true;
			Id = id;
			Race = type;
		}
	}
}
