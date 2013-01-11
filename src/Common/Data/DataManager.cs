// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.IO.Compression;
using Common.Tools;

namespace Common.Data
{
	public abstract class DataManager<T> where T : class, new()
	{
		public List<T> Entries = new List<T>();

		/// <summary>
		/// Loads all entries from the given file.
		/// </summary>
		/// <param name="filepath"></param>
		public virtual void LoadFromCsv(string filePath, bool reload = false)
		{
			if (!File.Exists(filePath))
				throw new FileNotFoundException("File not found: " + filePath);

			if (reload)
				this.Entries.Clear();
		}

		/// <summary>
		/// Reads the given string and parses it as CSV.
		/// </summary>
		/// <param name="csv"></param>
		/// <returns></returns>
		protected List<string> ParseCsvLine(string csv, char seperator = ',')
		{
			var result = new List<string>();

			csv = csv.Trim();

			if (csv != string.Empty && !csv.StartsWith("//"))
			{
				bool quoteFound = false, quotation = false, comment = false;
				for (int i = 0, ptr = 0; i < csv.Length; )
				{
					while (csv[i] == ' ') ++i;

					if (csv[i] == '"' && csv[i - 1] != '\\')
					{
						++i; quoteFound = !quoteFound;
						if (!quotation) quotation = true;
						continue;
					}

					if (!quoteFound && csv[i] == '/' && csv[i + 1] == '/')
						comment = true;

					if ((csv[i] == seperator && !quoteFound) || i == csv.Length - 1 || comment)
					{
						if (ptr > 0) ++ptr;
						if (i == csv.Length - 1) ++i;

						if (!quotation) result.Add(csv.Substring(ptr, i - ptr).Trim());
						else result.Add(csv.Substring(ptr, i - ptr).Trim().Trim('"'));

						ptr = i; quotation = false;
					}

					if (comment)
						break;

					++i;
				}
			}

			return result;
		}

		protected void ReadCsv(string filePath, int minFields)
		{
			using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
			using (var reader = new StreamReader(fs))
			{
				for (int i = 0; !reader.EndOfStream; ++i)
				{
					var csv = this.ParseCsvLine(reader.ReadLine());
					if (csv.Count < 1)
						continue;

					if (csv.Count < minFields)
					{
						Logger.Warning("Missing information on line " + (i + 1).ToString() + " in " + Path.GetFileName(filePath) + ", skipping.");
						continue;
					}

					try
					{
						var info = new T();
						this.CsvToEntry(info, csv, i + 1);
						this.Entries.Add(info);
					}
					catch (Exception ex)
					{
						Logger.Warning("Unable to read information on line " + (i + 1).ToString() + " in " + Path.GetFileName(filePath) + ", skipping.");
						Logger.Warning("Problem: " + ex.Message);
						continue;
					}
				}
			}
		}

		protected virtual void CsvToEntry(T info, List<string> csv, int currentLine)
		{
		}
	}
}
