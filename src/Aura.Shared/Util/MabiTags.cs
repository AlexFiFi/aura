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
	/// serialized to this format: SOMEINT:4:1234;SOMESTR:s:test;
	/// which is used primarily used in items and quests.
	/// </summary>
	public class MabiTags
	{
		private Dictionary<string, object> _tags = new Dictionary<string, object>();
		private string _cache = null;

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

		public void Remove(string key)
		{
			_tags.Remove(key);
			_cache = null;
		}

		public void Clear()
		{
			_tags.Clear();
		}

		public int Count
		{
			get { return _tags.Count; }
		}

		public bool Has(string key)
		{
			return _tags.ContainsKey(key);
		}

		public dynamic this[string key]
		{
			get { _cache = null; return _tags[key]; }

			// Bad performance, probably cause of dynamic.
			//set { this.Set(key, value); }
		}

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
		/// Returns tags in the format "key:varType:value;..."
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
					sb.AppendFormat("{0}:{1}:{2};", tag.Key, sType, ((string)tag.Value).Replace(";", "%S"));
				else
					sb.AppendFormat("{0}:{1}:{2};", tag.Key, sType, tag.Value);
			}

			return (_cache = sb.ToString());
		}

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
	}
}
