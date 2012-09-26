// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder


namespace World.Scripting
{
	public class BaseScript : System.IDisposable
	{
		private bool _disposed = false;
		public bool Disposed { get { return _disposed; } }
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
			_disposed = true;
		}
	}
}
