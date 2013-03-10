// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Aura.Msgr.Network;

namespace Aura.Msgr
{
	class Program
	{
		static void Main(string[] args)
		{
			MsgrServer.Instance.Run(args);
		}
	}
}
