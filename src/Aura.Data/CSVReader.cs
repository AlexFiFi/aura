﻿// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Aura.Data
{
	public class CSVReader : IDisposable
	{
		public string Path { get; set; }
		public char Seperator { get; set; }

		public CSVReader(string path)
		{
			this.Path = path;
			this.Seperator = ',';
		}

		public IEnumerable<CSVEntry> Next()
		{
			using (var fs = new FileStream(this.Path, FileMode.Open, FileAccess.Read, FileShare.Read))
			using (var reader = new StreamReader(fs))
			{
				for (int i = 0; !reader.EndOfStream; ++i)
				{
					var entry = this.GetEntry(reader.ReadLine());
					if (entry.Count < 1)
						continue;

					entry.Line = i + 1;

					yield return entry;
				}
			}

			yield break;
		}

		protected CSVEntry GetEntry(string csv)
		{
			var result = new CSVEntry();

			csv = csv.Trim();

			// Check for empty and commented lines.
			if (csv != string.Empty && !csv.StartsWith("//"))
			{
				int ptr = 0, braces = 0;
				bool inString = false, comment = false;
				for (int i = 0; i < csv.Length; ++i)
				{
					// End of line?
					var eol = (i == csv.Length - 1);

					// Quotes
					if (csv[i] == '"' && braces == 0)
						inString = !inString;

					// Braces
					if (!inString)
					{
						if (csv[i] == '{')
							braces++;
						else if (csv[i] == '}')
							braces--;
					}

					// Comments
					if (!inString && csv[i] == '/' && csv[i + 1] == '/')
						comment = true;

					if ((csv[i] == Seperator && braces == 0 && !inString) || eol || comment)
					{
						// Inc by one to get the last char
						if (eol) i++;

						// Get value
						var v = csv.Substring(ptr, i - ptr).Trim(' ', '\t', '"');

						// Trim surrounding braces
						if (v.Length >= 2 && v[0] == '{' && v[v.Length - 1] == '}')
							v = v.Substring(1, v.Length - 2);

						result.Fields.Add(v);

						// Skip over seperator
						ptr = i + 1;

						// Stop at comments
						if (comment)
							break;
					}
				}
			}

			return result;
		}

		public void Dispose()
		{ }
	}

	public class CSVEntry
	{
		public List<string> Fields { get; set; }
		public int Pointer { get; set; }
		public int Line { get; set; }

		public int Count { get { return this.Fields.Count; } }

		public bool End
		{
			get { return (this.Pointer > this.Fields.Count - 1); }
		}

		public int Remaining
		{
			get { return (this.Fields.Count - this.Pointer - 1); }
		}

		public CSVEntry()
		{
			this.Fields = new List<string>();
		}

		public CSVEntry(List<string> fields)
		{
			this.Fields = fields;
		}

		public void Skip(int fields)
		{
			this.Pointer += fields;
			if (this.Pointer >= this.Fields.Count)
				this.Pointer = 0;
		}

		public bool IsFieldEmpty(int index = -1)
		{
			return string.IsNullOrWhiteSpace(this.Fields[index < 0 ? this.Pointer : index]);
		}

		public byte ReadUByte(int index = -1) { if (IsFieldEmpty(index)) { this.Pointer++; return 0; } return Convert.ToByte(this.Fields[index < 0 ? this.Pointer++ : index]); }
		public sbyte ReadSByte(int index = -1) { if (IsFieldEmpty(index)) { this.Pointer++; return 0; } return Convert.ToSByte(this.Fields[index < 0 ? this.Pointer++ : index]); }
		public byte ReadUByteHex(int index = -1) { if (IsFieldEmpty(index)) { this.Pointer++; return 0; } return Convert.ToByte(this.Fields[index < 0 ? this.Pointer++ : index], 16); }
		public sbyte ReadSByteHex(int index = -1) { if (IsFieldEmpty(index)) { this.Pointer++; return 0; } return Convert.ToSByte(this.Fields[index < 0 ? this.Pointer++ : index], 16); }

		public ushort ReadUShort(int index = -1) { if (IsFieldEmpty(index)) { this.Pointer++; return 0; } return Convert.ToUInt16(this.Fields[index < 0 ? this.Pointer++ : index]); }
		public short ReadSShort(int index = -1) { if (IsFieldEmpty(index)) { this.Pointer++; return 0; } return Convert.ToInt16(this.Fields[index < 0 ? this.Pointer++ : index]); }
		public ushort ReadUShortHex(int index = -1) { if (IsFieldEmpty(index)) { this.Pointer++; return 0; } return Convert.ToUInt16(this.Fields[index < 0 ? this.Pointer++ : index], 16); }
		public short ReadSShortHex(int index = -1) { if (IsFieldEmpty(index)) { this.Pointer++; return 0; } return Convert.ToInt16(this.Fields[index < 0 ? this.Pointer++ : index], 16); }

		public uint ReadUInt(int index = -1) { if (IsFieldEmpty(index)) { this.Pointer++; return 0; } return Convert.ToUInt32(this.Fields[index < 0 ? this.Pointer++ : index]); }
		public int ReadSInt(int index = -1) { if (IsFieldEmpty(index)) { this.Pointer++; return 0; } return Convert.ToInt32(this.Fields[index < 0 ? this.Pointer++ : index]); }
		public uint ReadUIntHex(int index = -1) { if (IsFieldEmpty(index)) { this.Pointer++; return 0; } return Convert.ToUInt32(this.Fields[index < 0 ? this.Pointer++ : index], 16); }
		public int ReadSIntHex(int index = -1) { if (IsFieldEmpty(index)) { this.Pointer++; return 0; } return Convert.ToInt32(this.Fields[index < 0 ? this.Pointer++ : index], 16); }

		public ulong ReadULong(int index = -1) { if (IsFieldEmpty(index)) { this.Pointer++; return 0; } return Convert.ToUInt64(this.Fields[index < 0 ? this.Pointer++ : index]); }
		public long ReadSLong(int index = -1) { if (IsFieldEmpty(index)) { this.Pointer++; return 0; } return Convert.ToInt64(this.Fields[index < 0 ? this.Pointer++ : index]); }
		public ulong ReadULongHex(int index = -1) { if (IsFieldEmpty(index)) { this.Pointer++; return 0; } return Convert.ToUInt64(this.Fields[index < 0 ? this.Pointer++ : index], 16); }
		public long ReadSLongHex(int index = -1) { if (IsFieldEmpty(index)) { this.Pointer++; return 0; } return Convert.ToInt64(this.Fields[index < 0 ? this.Pointer++ : index], 16); }

		public float ReadFloat(int index = -1) { if (IsFieldEmpty(index)) { this.Pointer++; return 0; } return float.Parse(this.Fields[index < 0 ? this.Pointer++ : index], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US")); }
		public double ReadDouble(int index = -1) { if (IsFieldEmpty(index)) { this.Pointer++; return 0; } return double.Parse(this.Fields[index < 0 ? this.Pointer++ : index], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US")); }

		public string ReadString(int index = -1) { return (this.Fields[index < 0 ? this.Pointer++ : index]); }
		public string[] ReadStringList(int index = -1) { return this.ReadString(index).Split(':'); }
	}
}
