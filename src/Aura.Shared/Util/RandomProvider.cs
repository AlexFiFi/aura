// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Threading;

namespace Aura.Shared.Util
{
	/// <summary>
	/// Thread-safe provider for "Random" instances. Use whenever no custom
	/// seed is required.
	/// </summary>
	public static class RandomProvider
	{
		private static int _seed = Environment.TickCount;

		private static ThreadLocal<Random> randomWrapper = new ThreadLocal<Random>(() =>
			new Random(Interlocked.Increment(ref _seed))
		);

		/// <summary>
		/// Returns an instance of Random for the calling thread.
		/// </summary>
		/// <returns></returns>
		public static Random Get()
		{
			return randomWrapper.Value;
		}
	}
}
