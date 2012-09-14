// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Text;
using System.IO.Compression;

namespace MabiNatives
{
	public abstract class DataManager<T> where T : class
	{
		public List<T> Entries = new List<T>();

		/// <summary>
		/// Loads all entries from the XML file at the given path and adds
		/// them to the entries list.
		/// </summary>
		/// <param name="path"></param>
		public virtual void LoadFromXml(string path)
		{
			if (!File.Exists(path))
				throw new Exception("File not found: " + path);
		}

		/// <summary>
		/// Loads all entries from the JSON file at the given path and adds
		/// them to the entries list.
		/// </summary>
		/// <param name="path"></param>
		public virtual void LoadFromJson(string path)
		{
			if (!File.Exists(path))
				throw new FileNotFoundException("File not found: " + path);

			var content = File.ReadAllText(path);

			if (path.EndsWith(".gz"))
			{
				using (var min = new MemoryStream(Convert.FromBase64String(content)))
				using (var mout = new MemoryStream())
				{
					using (var gzip = new GZipStream(min, CompressionMode.Decompress))
					{
						gzip.CopyTo(mout);
					}

					content = Encoding.UTF8.GetString(mout.ToArray());
				}
			}

			content = this.ParseJson(content);
			this.Entries = JsonConvert.DeserializeObject<List<T>>(content);
		}

		/// <summary>
		/// Exports all current entries to the given file in JSON format.
		/// </summary>
		/// <param name="path"></param>
		public virtual void ExportToJson(string path)
		{
			File.Delete(path);

			var sb = new StringBuilder();
			sb.AppendLine(this.GetHeader());
			sb.AppendLine("[");
			foreach (var entry in this.Entries)
			{
				string serialized = JsonConvert.SerializeObject(entry, Newtonsoft.Json.Formatting.None);
				sb.AppendLine(serialized + ",");
			}
			sb.AppendLine("]");

			var data = sb.ToString();

			if (path.EndsWith(".gz"))
			{
				using (var min = new MemoryStream(Encoding.UTF8.GetBytes(data)))
				using (var mout = new MemoryStream())
				{
					using (var gzip = new GZipStream(mout, CompressionMode.Compress))
					{
						min.CopyTo(gzip);
					}
					data = Convert.ToBase64String(mout.ToArray());
				}
			}

			using (var writer = File.AppendText(path))
			{
				writer.Write(this.FormatJson(data));
			}
		}

		/// <summary>
		/// Reads the file at the given path and does preperation work
		/// for Aura.
		/// </summary>
		/// <param name="json"></param>
		/// <returns></returns>
		protected virtual string ParseJson(string json)
		{
			// Remove one-line comments (not supported by the parser for some reason)
			json = Regex.Replace(json, @"//.*\r?\n", "");

			return json;
		}

		protected virtual string FormatJson(string json)
		{
			json = json.Replace("{", "{ ");
			json = json.Replace("}", " }");
			json = json.Replace(",", ", ");
			json = json.Replace("\":", "\": ");

			return json;
		}

		/// <summary>
		/// Writes a few standard header lines for Aura.
		/// </summary>
		/// <param name="writer"></param>
		protected virtual string GetHeader()
		{
			// TODO: This should be configurable by the program using this lib.
			return
				"// Aura" + "\r\n" +
				"// Database file" + "\r\n" +
				"//---------------------------------------------------------------------------" + "\r\n" +
				"";
		}
	}
}
