// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Timers;
using Aura.Shared.Util;

namespace Aura.World.Scripting
{
	/// <summary>
	/// The main purpose for this class is giving easy access to the
	/// global scripting variables and saving them reguarly, for now.
	/// </summary>
	public static class Global
	{
		private const string AccountName = "Aura System";
		private const int VarSaveInterval = 1000 * 60 * 10;

		private static Timer _varSaveTimer;

		public static ScriptingVariables Vars { get; set; }

		static Global()
		{
			Vars = new ScriptingVariables();
		}

		public static void Init()
		{
			Vars.Load(AccountName, 0);

			_varSaveTimer = new Timer(VarSaveInterval);
			_varSaveTimer.AutoReset = true;
			_varSaveTimer.Elapsed += RegularVarSave;
			_varSaveTimer.Start();
		}

		public static void RegularVarSave(object sender, EventArgs args)
		{
			Vars.Save(AccountName, 0);
			Logger.Info("Saving global scripting variables.");
		}
	}
}
