// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Aura.Shared.Util
{
	/// <summary>
	/// "Generic" dictionary wrapper that can hold various var types and
	/// serializes to this format: SOMEINT:4:1234;SOMESTR:s:test;
	/// which is primarily used in items and quests.
	/// </summary>
	public class MabiTags
	{
		// Stored as object so we can put anything in
		private Dictionary<string, object> _tags = new Dictionary<string, object>();
		private string _cache = null;

		public MabiTags()
		{ }

		public MabiTags(string toParse)
		{
			this.Parse(toParse);
		}

		private void Set<T>(string key, T val)
		{
			_tags[key] = val;
			_cache = null;
		}

		public void SetByte(string key, byte val) { this.Set(key, val); }
		public void SetShort(string key, ushort val) { this.Set(key, val); }
		public void SetInt(string key, uint val) { this.Set(key, val); }
		public void SetLong(string key, ulong val) { this.Set(key, val); }
		public void SetFloat(string key, float val) { this.Set(key, val); }
		public void SetString(string key, string val) { this.Set(key, val); }
		public void SetBool(string key, bool val) { this.Set(key, val); }

		/// <summary>
		/// Returns the value with the given key, or null it wasn't found.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public object Get(string key)
		{
			object result;
			_tags.TryGetValue(key, out result);
			return result;
		}

		/// <summary>
		/// Removes the value with the given key.
		/// </summary>
		/// <param name="key"></param>
		public void Remove(string key)
		{
			_tags.Remove(key);
			_cache = null;
		}

		/// <summary>
		/// Removes all values.
		/// </summary>
		public void Clear()
		{
			_tags.Clear();
			_cache = null;
		}

		/// <summary>
		/// Returns number of values.
		/// </summary>
		public int Count
		{
			get { return _tags.Count; }
		}

		/// <summary>
		/// Returns whether a value exists for the given key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool Has(string key)
		{
			return _tags.ContainsKey(key);
		}

		/// <summary>
		/// Access to the values via indexing. Returns dynamic,
		/// so we don't have to cast the value before using it.
		/// (This can be a performance problem, don't overuse.)
		/// Returns null if value doesn't exist.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public dynamic this[string key]
		{
			get { return this.Get(key); }

			// Bad performance, probably cause of dynamic.
			//set { _cache = null; this.Set(key, value); }
		}

		/// <summary>
		/// Returns string type identifier for the object.
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		private string ValToTypeStr(object val)
		{
			if (val is byte || val is sbyte) return "1";
			else if (val is ushort || val is short) return "2";
			else if (val is uint || val is int) return "4";
			else if (val is ulong || val is long) return "8";
			else if (val is float) return "f";
			else if (val is string) return "s";
			else if (val is bool) return "b";
			else
				throw new Exception("Unsupported type '" + val.GetType().ToString() + "'");
		}

		/// <summary>
		/// Returns tags in the format "key:varType:value;...".
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			if (_tags.Count < 1)
				return string.Empty;

			if (_cache != null)
				return _cache;

			var sb = new StringBuilder();

			foreach (var tag in _tags)
			{
				var sType = this.ValToTypeStr(tag.Value);
				if (sType == "b")
					sb.AppendFormat("{0}:{1}:{2};", tag.Key, sType, (bool)tag.Value ? "1" : "0");
				else if (sType == "s")
					sb.AppendFormat("{0}:{1}:{2};", tag.Key, sType, ((string)tag.Value).Replace(";", "%S").Replace(":", "%C"));
				else
					sb.AppendFormat("{0}:{1}:{2};", tag.Key, sType, tag.Value);
			}

			return (_cache = sb.ToString());
		}

		/// <summary>
		/// Reads a string in the format "key:varType:value;..." and adds
		/// the values to this tag collection.
		/// </summary>
		/// <param name="str"></param>
		public void Parse(string str)
		{
			if (string.IsNullOrWhiteSpace(str))
				return;

			var matches = Regex.Matches(str, "([^:]+):([^:]+):([^;]+);");
			foreach (Match match in matches)
			{
				var key = match.Groups[1].Value;
				var val = match.Groups[3].Value;

				switch (match.Groups[2].Value)
				{
					case "1": this.SetByte(key, Convert.ToByte(val)); break;
					case "2": this.SetShort(key, Convert.ToUInt16(val)); break;
					case "4": this.SetInt(key, Convert.ToUInt32(val)); break;
					case "8": this.SetLong(key, Convert.ToUInt64(val)); break;
					case "f": this.SetFloat(key, Convert.ToSingle(val, CultureInfo.InvariantCulture)); break;
					case "s": this.SetString(key, val.Replace("%S", ";")); break;
					case "b": this.SetBool(key, val == "1"); break;
				}
			}
		}

		/// <summary>
		/// Parses string and tries to return the value.
		/// Returns T's default if the key can't be found or the type is incorrect.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="from"></param>
		/// <returns></returns>
		public static T Fetch<T>(string key, string from)
		{
			var tags = new MabiTags(from);
			var val = tags.Get(key);
			if (val != null && val is T)
				return (T)val;
			return default(T);
		}
	}
}
