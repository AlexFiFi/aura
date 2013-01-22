// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.IO;

namespace Common.Tools
{
	public static class Logger
	{
		[Flags]
		public enum LogLevel : byte
		{
			None = 255,
			Info = 1,
			Warning = 2,
			Error = 4,
			Debug = 8,
			Status = 16,
			Exception = 32,
			Unimplemented = 64,
		}

		private static string _logFile = null;
		public static LogLevel Hide { get; set; }

		public static string FileLog
		{
			get { return _logFile; }
			set
			{
				if (value != null && File.Exists(value))
					File.Delete(value);

				_logFile = value;
			}
		}

		public static void Info(string s, bool newLine = true)
		{
			Write(LogLevel.Info, s, newLine);
		}
		public static void Info(string format, params object[] args)
		{
			Info(string.Format(format, args));
		}

		public static void Unimplemented(string s)
		{
			Write(LogLevel.Unimplemented, s);
		}
		public static void Unimplemented(string format, params object[] args)
		{
			Unimplemented(string.Format(format, args));
		}

		public static void Warning(string s)
		{
			Write(LogLevel.Warning, s);
		}
		public static void Warning(string format, params object[] args)
		{
			Warning(string.Format(format, args));
		}

		public static void Error(string s)
		{
			Write(LogLevel.Error, s);
		}
		public static void Error(string format, params object[] args)
		{
			Error(string.Format(format, args));
		}

		public static void Debug(string s)
		{
			Write(LogLevel.Debug, s);
		}
		public static void Debug(string format, params object[] args)
		{
			Debug(string.Format(format, args));
		}
		public static void Debug(object s)
		{
			Debug(s.ToString());
		}

		public static void Status(string s)
		{
			Write(LogLevel.Status, s);
		}

		public static void Exception(Exception ex, string s = null, bool stackTrace = false)
		{
			if (Logger.Hide.HasFlag(LogLevel.Exception))
				return;

			if (s != null)
			{
				Write(LogLevel.Error, s);
			}

			Write(LogLevel.Exception, ex.Source + ", in " + ex.TargetSite);
			Write(LogLevel.Exception, ex.Message + (!stackTrace ? "" : "\n" + ex.StackTrace));
		}

		public static void Write(string format, bool newLine = true)
		{
			Write(LogLevel.None, format, newLine);
		}

		public static void Write(string format, params object[] args)
		{
			Write(LogLevel.None, string.Format(format, args));
		}

		public static void Write(LogLevel lvl, string s, bool newLine = true)
		{
			if (Logger.Hide.HasFlag(lvl))
				return;

			lock (Console.Out)
			{
				switch (lvl)
				{
					case LogLevel.Info: Console.ForegroundColor = ConsoleColor.White; break;
					case LogLevel.Warning: Console.ForegroundColor = ConsoleColor.Yellow; break;
					case LogLevel.Error: Console.ForegroundColor = ConsoleColor.Red; break;
					case LogLevel.Debug: Console.ForegroundColor = ConsoleColor.Cyan; break;
					case LogLevel.Status: Console.ForegroundColor = ConsoleColor.Green; break;
					case LogLevel.Exception: Console.ForegroundColor = ConsoleColor.DarkRed; break;
					case LogLevel.Unimplemented: Console.ForegroundColor = ConsoleColor.DarkGray; break;
				}

				if (lvl != LogLevel.None)
					Console.Write("[" + lvl.ToString() + "]");

				Console.ForegroundColor = ConsoleColor.Gray;

				if (lvl != LogLevel.None)
					Console.Write(" - ");

				if (newLine)
					Console.WriteLine(s);
				else
					Console.Write(s);

				if (_logFile != null)
				{
					using (var file = new StreamWriter(_logFile, true))
					{
						file.Write(DateTime.Now + " ");
						if (lvl != LogLevel.None)
							file.Write("[" + lvl.ToString() + "] - ");
						file.WriteLine(s);
						file.Flush();
					}
				}
			}
		}

		public static void ClearLine()
		{
			Console.Write("\r                                                                              \r");
		}

		public static void RLine()
		{
			Console.Write("\r");
		}
	}
}
