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
		public MabiNPC Target { get; private set; }
		public int Id { get; private set; }

		public Options Options = Options.FaceAndName;
		public string DialogFace = null;
		public string DialogName = null;

		public IEnumerator State = null;
		public Response Response = null;

		public bool IsValid
		{
			get
			{
				return (this.Target != null && this.State != null);
			}
		}

		public NPCSession()
		{
			// We'll only set this once for every char, for the entire session.
			// In some cases the client doesn't seem to take the new id,
			// which results in a mismatch.
			this.Id = RandomProvider.Get().Next(1, 5000);
		}

		public void Start(MabiNPC target)
		{
			this.Target = target;
		}

		public void Clear()
		{
			this.Target = null;
			this.State = null;
			this.Response = null;
		}
	}
}
