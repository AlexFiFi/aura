// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Aura.Login.Network;

namespace Aura.Login
{
	public class Program
	{
		static void Main(string[] args)
		{
			LoginServer.Instance.Run(args);
		}
	}
}