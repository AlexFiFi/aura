// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aura.Shared.Util
{
	public static class MabiMath
	{
		public static float DirToRad(byte direction)
		{
			return (float)Math.PI * 2 / 255 * direction;
		}
	}
}
