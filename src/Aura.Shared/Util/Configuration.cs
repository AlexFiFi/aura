// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;

namespace Aura.Shared.Util
{
	/// <summary>
	/// Basic configuration class.
	/// 
	/// Supported seperators   : '=', ':', ' ' (space)
	/// Supported comments     : '!', ';', '#', '//', '--'
	/// Supports includes with : include|require filepath
	/// </summary>
	public class Configuration
	{
		private Dictionary<string, string> _options = new Dictionary<string, string>();

		/// <summary>
		/// Parse the given file, and save the options.
		/// Throws if an error occures while reading the file, or if the file doesn't exist.
		/// </summary>
		/// <param name="filePath"></param>
		public void ReadFile(string filePath)
		{
			FileReader.DoEach(filePath, (string line) =>
			{
				this.ReadString(line);
			});
		}

		public void ReadArguments(string[] args, string filePath)
		{
			FileReader.DoEach(args, filePath, (string line) =>
			{
				this.ReadString(line);
			});
		}

		/// <summary>
		/// Parses a single line, hopefully containing a key and a value,
		/// which are added to the options.
		/// </summary>
		/// <param name="line"></param>
		/// <param name="filePath"></param>
		private void ReadString(string line)
		{
			int pos = -1;

			// Check for seperators
			if ((pos = line.IndexOf('=')) < 0 && (pos = line.IndexOf(':')) < 0 && (pos = line.IndexOf(' ')) < 0)
				return;

			this.Add(line.Substring(0, pos).Trim(), line.Substring(pos + 1).Trim());
		}

		public void Add(string key, string value, bool overwrite = true)
		{
			if (overwrite == false && this.Has(key))
				return;

			_options[key] = value;
		}

		/// <summary>
		/// Parses the given option as integer, and returns it.
		/// Returns default value (def) if the option doesn't exist.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="def"></param>
		/// <returns></returns>
		public int GetInt(string name, int def = 0)
		{
			if (!this.Has(name))
				return def;

			return int.Parse(_options[name]);
		}

		/// <summary>
		/// Returns the given option as string.
		/// Returns default value (def) if the option doesn't exist.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="def"></param>
		/// <returns></returns>
		public string GetString(string name, string def = "")
		{
			if (!this.Has(name))
				return def;

			return _options[name];
		}

		/// <summary>
		/// Parses the given option as boolean, and returns it.
		/// Returns default value (def) if the option doesn't exist.
		/// Valid values for true are: true, yes, and 1. Everything else is considered false.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="def"></param>
		/// <returns></returns>
		public bool GetBool(string name, bool def = false)
		{
			if (!this.Has(name))
				return def;

			string option = _options[name];
			return (option == "true" || option == "yes" || option == "1") ? true : false;
		}

		public T Get<T>(string name, T def = default(T))
		{
			var result = def;

			try
			{
				result = (T)Convert.ChangeType(_options[name], typeof(T));
			}
			catch (Exception) { }

			return result;
		}

		public bool Has(string name)
		{
			return _options.ContainsKey(name);
		}
	}
}
