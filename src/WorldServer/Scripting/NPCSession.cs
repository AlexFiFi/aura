// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Common.Tools;
using World.World;

namespace World.Scripting
{
	public class NPCSession
	{
		public MabiNPC Target = null;
		public int SessionId = -1;

		public int Start(MabiNPC target)
		{
			this.Target = target;
			return (this.SessionId = RandomProvider.Get().Next(0, 5000));
		}

		public void Clear()
		{
			this.Target = null;
			this.SessionId = -1;
		}

		public bool IsValid()
		{
			return (this.SessionId >= 0 && this.SessionId <= 5000);
		}
	}

}
