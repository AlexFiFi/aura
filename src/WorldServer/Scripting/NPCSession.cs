// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections;
using Aura.Shared.Util;
using Aura.World.World;

namespace Aura.World.Scripting
{
	public enum Options { None = 0, Face = 1, Name = 2, FaceAndName = 3 }

	public class NPCSession
	{
		public MabiNPC Target = null;
		public int SessionId = -1;

		public Options Options = Options.FaceAndName;
		public string DialogFace = null;
		public string DialogName = null;

		public IEnumerator State = null;
		public Response Response = null;

		public int Start(MabiNPC target)
		{
			this.Target = target;
			return (this.SessionId = RandomProvider.Get().Next(0, 5000));
		}

		public void Clear()
		{
			this.Target = null;
			this.SessionId = -1;
			this.State = null;
			this.Response = null;
		}

		public bool IsValid
		{
			get { return (this.SessionId >= 0 && this.SessionId <= 5000); }
		}
	}
}
