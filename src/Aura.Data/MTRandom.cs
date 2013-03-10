/* 
   A C-program for MT19937, with initialization improved 2002/1/26.
   Coded by Takuji Nishimura and Makoto Matsumoto.

   Before using, initialize the state by using init_genrand(seed)  
   or init_by_array(init_key, key_length).

   Copyright (C) 1997 - 2002, Makoto Matsumoto and Takuji Nishimura,
   All rights reserved.                          
   Copyright (C) 2005, Mutsuo Saito,
   All rights reserved.                          

   Redistribution and use in source and binary forms, with or without
   modification, are permitted provided that the following conditions
   are met:

	 1. Redistributions of source code must retain the above copyright
		notice, this list of conditions and the following disclaimer.

	 2. Redistributions in binary form must reproduce the above copyright
		notice, this list of conditions and the following disclaimer in the
		documentation and/or other materials provided with the distribution.

	 3. The names of its contributors may not be used to endorse or promote 
		products derived from this software without specific prior written 
		permission.
*/

/*
   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
   "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
   LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
   A PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE COPYRIGHT OWNER OR
   CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
   EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
   PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
   PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
   LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
   NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
   SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.


   Any feedback is very welcome.
   http://www.math.sci.hiroshima-u.ac.jp/~m-mat/MT/emt.html
   email: m-mat @ math.sci.hiroshima-u.ac.jp (remove space)
*/

/* 
   A C#-program for MT19937, with initialization improved 2006/01/06.
   Coded by Mitil.

   Copyright (C) 2006, Mitil, All rights reserved.

   Any feedback is very welcome.
   URL: http://meisui.psk.jp/
   email: m-i-t-i-l [at@at] p-s-k . j-p
		   (remove dash[-], and replace [at@at] --> @)
*/

using System;
using System.Security.Cryptography;

namespace Aura.Data
{
	public class MTRandom
	{
		/* Period parameters */
		private const Int16 N = 624;
		private const Int16 M = 397;
		private const UInt32 MATRIX_A = (UInt32)0x9908b0df;   /* constant vector a */
		private const UInt32 UPPER_MASK = (UInt32)0x80000000; /* most significant w-r bits */
		private const UInt32 LOWER_MASK = (UInt32)0x7fffffff; /* least significant r bits */
		private UInt32[] mt; /* the array for the state vector  */
		private UInt16 mti; /* mti==N+1 means mt[N] is not initialized */
		private UInt32[] mag01;

		public MTRandom(Int32 seed = 5489)
			: this((uint)seed)
		{ }

		public MTRandom(UInt32 seed = 5489)
		{
			mt = new UInt32[N];

			mag01 = new UInt32[] { 0, MATRIX_A };
			/* mag01[x] = x * MATRIX_A  for x=0,1 */

			mti = N + 1;
			mt[0] = seed;

			for (mti = 1; mti < N; mti++)
			{
				mt[mti] = ((UInt32)1812433253 * (mt[mti - 1] ^ (mt[mti - 1] >> 30)) + mti);
				/* See Knuth TAOCP Vol2. 3rd Ed. P.106 for multiplier. */
				/* In the previous versions, MSBs of the seed affect   */
				/* only MSBs of the array mt[].                        */
				/* 2002/01/09 modified by Makoto Matsumoto             */
			}
		}

		~MTRandom()
		{
			mt = null;
			mag01 = null;
		}

		/* generates a random number on [0,0xffffffff]-Interval */
		public UInt32 GetUInt()
		{
			UInt32 y;

			if (mti >= N)
			{ /* generate N words at one time */
				Int16 kk;

				for (kk = 0; kk < N - M; kk++)
				{
					y = ((mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK)) >> 1;
					mt[kk] = mt[kk + M] ^ mag01[mt[kk + 1] & 1] ^ y;
				}
				for (; kk < N - 1; kk++)
				{
					y = ((mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK)) >> 1;
					mt[kk] = mt[kk + (M - N)] ^ mag01[mt[kk + 1] & 1] ^ y;
				}
				y = ((mt[N - 1] & UPPER_MASK) | (mt[0] & LOWER_MASK)) >> 1;
				mt[N - 1] = mt[M - 1] ^ mag01[mt[0] & 1] ^ y;

				mti = 0;
			}

			y = mt[mti++];

			/* Tempering */
			y ^= (y >> 11);
			y ^= (y << 7) & 0x9d2c5680;
			y ^= (y << 15) & 0xefc60000;
			y ^= (y >> 18);

			return y;
		}
	}
}
