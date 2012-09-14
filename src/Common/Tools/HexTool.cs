// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Tools
{
	/// <summary>
	/// Converter to hex string, for various variable types.
	/// </summary>
	public static class HexTool
	{
		public static char[] HexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

		public static string ToString(long val, int length = 8)
		{
			var result = new StringBuilder();

			for (int i = length - 1; i >= 0; --i)
			{
				result.Append(((val >> i * 8) & 0xFF).ToString("X2") + " ");
			}

			return result.ToString().Trim();
		}

		public static string ToString(ulong val)
		{
			return ToString((long)val, 8);
		}

		public static string ToString(int val)
		{
			return ToString(val, 4);
		}

		public static string ToString(uint val)
		{
			return ToString(val, 4);
		}

		public static string ToString(short val)
		{
			return ToString(val, 2);
		}

		public static string ToString(ushort val)
		{
			return ToString(val, 2);
		}

		public static string ToString(byte val)
		{
			return val.ToString("X2");
		}

		public static string ToString(byte[] val, int length = -1)
		{
			var result = new StringBuilder();

			if (length < 1)
			{
				length = val.Length;
			}

			for (int i = 0; i < length; ++i)
			{
				result.Append(val[i].ToString("X2") + " ");
			}

			return result.ToString().Trim();
		}

		public static string ToString(string val)
		{
			return ToString(Encoding.ASCII.GetBytes(val));
		}
	}
}
