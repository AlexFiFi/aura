// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Threading;

namespace Common.Tools
{
	public static class RandomProvider
	{
		private static int seed = Environment.TickCount;

		private static ThreadLocal<Random> randomWrapper = new ThreadLocal<Random>(() =>
			new Random(Interlocked.Increment(ref seed))
		);

		public static Random Get()
		{
			return randomWrapper.Value;
		}
	}
}
