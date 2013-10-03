// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;

namespace Aura.Shared.Util
{
	/// <summary>
	/// FileReader is a general class, to read all lines from
	/// some file, and do something with it.
	/// 
	/// Features:
	///     - Various comment prefixes ('!', ';' , '#', '//', '--')
	///     - Including of files ('include|require filename')
	/// </summary>
	public static class FileReader
	{
		public delegate void LineDoer(string line);

		/// <summary>
		/// Reads all non-empty and non-comment lines from a file
		/// and returns them in a list.
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static List<string> GetAllLines(string filePath)
		{
			var result = new List<string>();

			FileReader.DoEach(filePath, (string line) =>
			{
				result.Add(line);
			});

			return result;
		}

		/// <summary>
		/// Calls the passed function for every non-empty and non-comment
		/// line in the file.
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="fn"></param>
		public static void DoEach(string filePath, LineDoer fn)
		{
			if (!File.Exists(filePath))
				throw new FileNotFoundException("File not found.");

			using (var sr = new StreamReader(filePath))
			{
				string line;
				while ((line = sr.ReadLine()) != null)
				{
					line = ParseLine(line);
					if (line == null)
						continue;

					bool require = false;

					// Including (include foo.conf)
					if (line.StartsWith("include ") || (require = line.StartsWith("require ")))
					{
						var includeFilePath = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(filePath)), line.Substring(8));
						try
						{
							FileReader.DoEach(includeFilePath, fn);
						}
						catch (FileNotFoundException)
						{
							// Ignore exceptions for includes
							if (require)
								throw new Exception("Required file '" + includeFilePath + "' not found.");
						}
						continue;
					}

					fn(line);
				}
			}
		}

		/// <summary>
		/// Calls the passed function for every non-empty and non-comment
		/// string in the array. A path is still required, to be used as
		/// root folder for include/require. If those aren't required,
		/// pass null, which will also disable this feature.
		/// 
		/// TODO: Maybe add a DoString method, DRY.
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="fn"></param>
		public static void DoEach(string[] args, string filePath, LineDoer fn)
		{
			foreach (var arg in args)
			{
				var line = ParseLine(arg);
				if (line == null)
					continue;

				bool require = false;

				if (filePath != null && (line.StartsWith("include ") || (require = line.StartsWith("require "))))
				{
					var includeFilePath = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(filePath)), line.Substring(8));
					try
					{
						FileReader.DoEach(includeFilePath, fn);
					}
					catch (FileNotFoundException)
					{
						if (require)
							throw new Exception("Required file '" + includeFilePath + "' not found.");
					}
					continue;
				}

				fn(line);
			}
		}

		/// <summary>
		/// Parses a single line, checks if it's valid, and not a comment, and returns it.
		/// </summary>
		/// <param name="line"></param>
		/// <param name="filePath"></param>
		private static string ParseLine(string line)
		{
			line = line.Trim();

			// Ignore short lines and comments
			if (line.Length < 3 || line[0] == '!' || line[0] == ';' || line[0] == '#' || line.StartsWith("//") || line.StartsWith("--"))
				return null;

			return line;

			//int pos = -1;

			// Check for seperators
			//if ((pos = line.IndexOf('=')) < 0 && (pos = line.IndexOf(':')) < 0 && (pos = line.IndexOf(' ')) < 0)
			//	return;

			//_options[line.Substring(0, pos).Trim()] = line.Substring(pos + 1).Trim();
			//Add(line.Substring(0, pos).Trim(), line.Substring(pos + 1).Trim());
		}
	}
}
