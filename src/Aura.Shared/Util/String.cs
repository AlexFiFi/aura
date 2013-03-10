// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;

namespace Aura.Shared.Util
{
	public static class StringExtension
	{
		/// <summary>
		/// Calculates differences in 2 strings.
		/// </summary>
		/// <param name="str"></param>
		/// <param name="comp"></param>
		/// <returns></returns>
		public static int LevenshteinDistance(this string str, string comp)
		{
			var sLen = str.Length;
			var cLen = comp.Length;

			if (sLen == 0)
				return cLen;
			if (cLen == 0)
				return sLen;

			var res = new int[sLen + 1, cLen + 1];

			for (int i = 1; i <= sLen; ++i)
			{
				for (int j = 1; j <= cLen; ++j)
				{
					int cost = (comp[j - 1] == str[i - 1]) ? 0 : 1;
					res[i, j] = Math.Min(Math.Min(res[i - 1, j] + 1, res[i, j - 1] + 1), res[i - 1, j - 1] + cost);
				}
			}

			return res[sLen, cLen];
		}
	}
}
