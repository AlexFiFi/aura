// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Msgr.Network;

namespace Msgr
{
	class Program
	{
		static void Main(string[] args)
		{
			MsgrServer.Instance.Run(args);
		}
	}
}
