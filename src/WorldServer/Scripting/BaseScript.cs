// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;

namespace World.Scripting
{
	public class BaseScript : IDisposable
	{
		public bool Disposed { get; protected set; }

		public virtual void OnLoad()
		{
		}

		public virtual void OnLoadDone()
		{
		}

		/// <inheritdoc/>
		/// <summary>
		/// Cleans up after the NPC (In case of reloading)
		/// Every derived class should call base.Dispose()
		/// </summary>
		public virtual void Dispose()
		{
			this.Disposed = true;
		}
	}
}
