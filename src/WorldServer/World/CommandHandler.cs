// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Constants;
using Common.Network;
using Common.Tools;
using Common.World;
using CSScriptLibrary;
using World.Network;
using World.Scripting;
using World.Tools;
using Common.Data;
using System.Text.RegularExpressions;

namespace World.World
{
	public static class Authority
	{
		public const byte Player = 0;
		public const byte VIP = 1;
		public const byte GameMaster = 50;
		public const byte Admin = 99;
	}

	public enum CommandResult { Okay, WrongParameter, Fail }

	public delegate CommandResult CommandFunc(WorldClient client, MabiCreature creature, string[] args, string msg);

	public class Command
	{
		public string Name, Parameters;
		public byte Auth;
		public CommandFunc Func;

		public Command() { }

		public Command(string name, string parameters, byte authority, CommandFunc func)
		{
			this.Name = name;
			this.Parameters = parameters;
			this.Auth = authority;
			this.Func = func;
		}

		public Command(string name, byte authority, CommandFunc func)
			: this(name, null, authority, func)
		{
		}
	}

	public class CommandHandler
	{
		public static readonly CommandHandler Instance = new CommandHandler();
		static CommandHandler() { }
		private CommandHandler() { }

		private static Dictionary<string, Command> _commands = new Dictionary<string, Command>();

		public void AddCommand(Command command)
		{
			_commands.Add(command.Name, command);
		}

		public void AddCommand(string name, byte authority, CommandFunc func)
		{
			this.AddCommand(new Command(name, authority, func));
		}

		public void AddCommand(string name, string parameters, byte authority, CommandFunc func)
		{
			this.AddCommand(new Command(name, parameters, authority, func));
		}

		public void Load()
		{
			this.AddCommand("where", Authority.Player, Command_where);
			this.AddCommand("info", Authority.Player, Command_info);
			this.AddCommand("motion", "<category> <motion>", Authority.Player, Command_motion);
			this.AddCommand("setrace", "<race>", Authority.Player, Command_setrace);

			this.AddCommand("go", "<destination>", Authority.VIP, Command_go);

			this.AddCommand("gmcp", Authority.GameMaster, Command_gmcp);
			this.AddCommand("item", "<id|item_name> [<amount|[color1> <color2> <color3>]]", Authority.GameMaster, Command_item);
			this.AddCommand("drop", "<id|item_name> [<amount|[color1> <color2> <color3>]]", Authority.GameMaster, Command_item);
			this.AddCommand("iteminfo", "<item name>", Authority.GameMaster, Command_iteminfo);
			this.AddCommand("ii", "<item name>", Authority.GameMaster, Command_iteminfo);
			this.AddCommand("mi", "<item name>", Authority.GameMaster, Command_monsterinfo);
			this.AddCommand("goto", "<region> [x] [y]", Authority.GameMaster, Command_warp);
			this.AddCommand("warp", "<region> [x] [y]", Authority.GameMaster, Command_warp);
			this.AddCommand("jump", "<x> [y]", Authority.GameMaster, Command_jump);
			this.AddCommand("clean", Authority.GameMaster, Command_clean);
			this.AddCommand("statuseffect", Authority.GameMaster, Command_statuseffect);
			this.AddCommand("se", Authority.GameMaster, Command_statuseffect);
			this.AddCommand("skill", Authority.GameMaster, Command_skill);
			this.AddCommand("spawn", Authority.GameMaster, Command_spawn);

			this.AddCommand("ri", Authority.GameMaster, Command_randomitem);

			// GMCP
			this.AddCommand("set_inventory", "/c [/p:<pocket>]", Authority.GameMaster, Command_set_inventory);

			this.AddCommand("test", Authority.Admin, Command_test);
			this.AddCommand("reloadnpcs", Authority.Admin, Command_reloadnpcs); //Leaving this in here
			//as a note to future devs. Due to appdomains, NPCs CANNOT be reloaded, the server
			//must be restarted. Loading NPCs into a new appdomain would incur a large performance hit.
			//However, we still need to be able to reload NPCs. So we'll just add new
			//assemblies in. This causes a large memory leak every time the code runs!--Xcelled
			this.AddCommand("reloaddata", Authority.Admin, Command_reloaddata);

			// Load script commands
			var commandsPath = Path.Combine(WorldConf.ScriptPath, "command");
			var scriptPaths = Directory.GetFiles(commandsPath);
			foreach (var path in scriptPaths)
			{
				try
				{
					this.AddCommand(CSScript.Load(path).CreateObject("*") as Command);
				}
				catch (Exception ex)
				{
					Logger.Exception(ex, "Script: " + path);
					continue;
				}

			}
		}

		/// <summary>
		/// Returns true if a command was used.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="creature"></param>
		/// <param name="args"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public bool Handle(WorldClient client, MabiCreature creature, string[] args, string msg)
		{
			var command = _commands.FirstOrDefault(a => a.Key == args[0]).Value;
			if (command != null)
			{
				if (client.Account.Authority >= command.Auth)
				{
					try
					{
						var result = command.Func(client, creature, args, msg);
						if (result == CommandResult.WrongParameter)
						{
							client.Send(PacketCreator.ServerMessage(creature, string.Format("Usage: {0} {1}", command.Name, command.Parameters)));
						}
					}
					catch (Exception ex)
					{
						client.Send(PacketCreator.ServerMessage(creature, "Error while executing command."));
						Logger.Exception(ex, "Unable to execute command '" + args[0] + "'. Message: '" + msg + "'");
					}

					return true;
				}

				// TODO: Add option if unknown commands should appear in public.
				client.Send(PacketCreator.ServerMessage(creature, "Unknown command."));
				return true;
			}

			return false;
		}

		// Commands
		// ==================================================================

		private CommandResult Command_where(WorldClient client, MabiCreature creature, string[] args, string msg)
		{
			var pos = creature.GetPosition();
			client.Send(PacketCreator.ServerMessage(creature, "Region: " + creature.Region + ", X: " + pos.X + ", Y: " + pos.Y));

			return CommandResult.Okay;
		}

		private CommandResult Command_info(WorldClient client, MabiCreature creature, string[] args, string msg)
		{
			MabiVertex pos = creature.GetPosition();
			client.Send(PacketCreator.ServerMessage(creature, "Welcome to Aura!"));
			client.Send(PacketCreator.ServerMessage(creature, string.Format("Creatures in world : {0}, items in world : {1}", WorldManager.Instance.GetCreatureCount(), WorldManager.Instance.GetItemCount())));
			client.Send(PacketCreator.ServerMessage(creature, "You are at pos@" + creature.Region + "," + pos.X + "," + pos.Y));

			return CommandResult.Okay;
		}

		private CommandResult Command_gmcp(WorldClient client, MabiCreature creature, string[] args, string msg)
		{
			var p = new MabiPacket(0x4EE9, creature.Id);
			client.Send(p);

			return CommandResult.Okay;
		}

		char[] colorCodes = new char[] {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'A', 'B', 'C', 'D', 'E', 'F' };
		private string getRandomColor()
		{
			string s = "0x";
			for (int i = 0; i < 8; i++)
				s += colorCodes[RandomProvider.Get().Next(colorCodes.Length - 1)];

			return s;
		}

		private CommandResult Command_randomitem(WorldClient client, MabiCreature creature, string[] args, string msg)
		{
			var item = MabiData.ItemDb.Entries[RandomProvider.Get().Next(MabiData.ItemDb.Entries.Count - 1)];
			string[] a = new string[] {"drop", item.Id.ToString(),  getRandomColor(), getRandomColor(), getRandomColor() };

			return Command_item(client, creature, a, msg);
		}

		private CommandResult Command_item(WorldClient client, MabiCreature creature, string[] args, string msg)
		{
			if (args.Length < 2 || args.Length == 4 || args.Length > 5)
				return CommandResult.WrongParameter;

			uint itemId = 0;
			if (!uint.TryParse(args[1], out itemId))
			{
				// Couldn't parse uint, must be a spawn by name

				var itemName = args[1].Replace('_', ' ');

				var newItemInfo = MabiData.ItemDb.Find(itemName);
				if (newItemInfo == null)
				{
					client.Send(PacketCreator.ServerMessage(creature, "Item '" + itemName + "' not found in database."));
					return CommandResult.Fail;
				}

				itemId = newItemInfo.Id;
			}

			if (MabiData.ItemDb.Find(itemId) == null)
			{
				client.Send(PacketCreator.ServerMessage(creature, "Item '" + itemId + "' not found in database."));
				return CommandResult.Fail;
			}

			var newItem = new MabiItem(itemId);
			newItem.Info.Bundle = 1;

			if (args.Length == 3)
			{
				// command id amount

				if (!ushort.TryParse(args[2], out newItem.Info.Bundle))
				{
					client.Send(PacketCreator.ServerMessage(creature, "Invalid amount."));
					return CommandResult.Fail;
				}
			}
			else if (args.Length == 5)
			{
				// command id color1-3

				uint[] color = { 0, 0, 0 };

				for (int i = 0; i < 3; ++i)
				{
					if (args[i + 2].StartsWith("0x"))
					{
						color[i] = uint.Parse(args[i + 2].Substring(2), System.Globalization.NumberStyles.HexNumber);
					}
					else
					{
						if (uint.TryParse(args[i + 2], out color[i]))
						{
							color[i] += 0x10000000;
						}
						else
						{
							switch (args[i + 2])
							{
								case "black": color[i] = 0xFF000000; break;
								case "white": color[i] = 0xFFFFFFFF; break;
								default:
									client.Send(PacketCreator.ServerMessage(creature, "Unknown color."));
									return CommandResult.Fail;
							}
						}
					}
				}

				newItem.Info.ColorA = color[0];
				newItem.Info.ColorB = color[1];
				newItem.Info.ColorC = color[2];
			}

			if (args[0] == "drop")
			{
				// >drop

				var pos = creature.GetPosition();
				newItem.Info.Region = creature.Region;
				newItem.Info.X = pos.X;
				newItem.Info.Y = pos.Y;

				WorldManager.Instance.AddItem(newItem);
			}
			else
			{
				// >item

				newItem.Info.Pocket = (byte)Pocket.Temporary;
				creature.Items.Add(newItem);

				client.Send(PacketCreator.ItemInfo(creature, newItem));
			}

			return CommandResult.Okay;
		}

		private CommandResult Command_iteminfo(WorldClient client, MabiCreature creature, string[] args, string msg)
		{
			if (args.Length < 2)
				return CommandResult.WrongParameter;

			var itemName = msg.Substring(args[0].Length + 2).Replace('_', ' ');

			var itemInfos = MabiData.ItemDb.FindAll(itemName);
			if (itemInfos.Count < 1)
			{
				client.Send(PacketCreator.ServerMessage(creature, "No items found."));
				return CommandResult.Fail;
			}

			for (int i = 0; i < itemInfos.Count && i < 20; ++i)
			{
				client.Send(PacketCreator.ServerMessage(creature, itemInfos[i].Id.ToString() + ": " + itemInfos[i].Name));
			}

			client.Send(PacketCreator.ServerMessage(creature, "Results: " + itemInfos.Count.ToString() + " (Max. 20 shown)"));

			return CommandResult.Okay;
		}

		private CommandResult Command_monsterinfo(WorldClient client, MabiCreature creature, string[] args, string msg)
		{
			if (args.Length < 2)
				return CommandResult.WrongParameter;

			var monsterName = msg.Substring(args[0].Length + 2).Replace('_', ' ');

			var infos = MabiData.MonsterDb.FindAll(monsterName);
			if (infos.Count < 1)
			{
				client.Send(PacketCreator.ServerMessage(creature, "No monsters found."));
				return CommandResult.Fail;
			}

			for (int i = 0; i < infos.Count && i < 20; ++i)
			{
				client.Send(PacketCreator.ServerMessage(creature, infos[i].Id.ToString() + ": " + infos[i].Name));

				string drops = "";
				foreach (var drop in infos[i].Drops)
				{
					drops += drop.ItemId.ToString() + " (" + (drop.Chance * 100).ToString() + "),";
				}
				client.Send(PacketCreator.ServerMessage(creature, "Drops: " + drops));
			}

			client.Send(PacketCreator.ServerMessage(creature, "Results: " + infos.Count.ToString() + " (Max. 20 shown)"));

			return CommandResult.Okay;
		}

		private CommandResult Command_spawn(WorldClient client, MabiCreature creature, string[] args, string msg)
		{
			//if (args.Length < 2)
			//    return CommandResult.WrongParameter;

			//uint monsterId = 0;
			//if (!uint.TryParse(args[1], out monsterId))
			//    return CommandResult.Fail;

			// TODO: Move some stuff around in NPCManager and add a monster spawn from here.

			return CommandResult.Okay;
		}

		private CommandResult Command_set_inventory(WorldClient client, MabiCreature creature, string[] args, string msg)
		{
			if (args.Length < 2)
				return CommandResult.WrongParameter;

			if (args[1] != "/c")
			{
				client.Send(PacketCreator.ServerMessage(creature, "Unknown paramter '" + args[1] + "'."));
				return CommandResult.Fail;
			}

			byte pocket = 2;
			if (args.Length >= 3)
			{
				var match = Regex.Match(args[2], "/p:(?<id>[0-9]+)");
				if (!match.Success)
				{
					client.Send(PacketCreator.ServerMessage(creature, "Unknown paramter '" + args[2] + "', please specify a pocket."));
					return CommandResult.Fail;
				}
				if (!byte.TryParse(match.Groups["id"].Value, out pocket) || pocket > (byte)Pocket.Max - 1)
				{
					client.Send(PacketCreator.ServerMessage(creature, "Invalid pocket."));
					return CommandResult.Fail;
				}
			}

			var toRemove = new List<MabiItem>();
			foreach (var item in creature.Items)
			{
				if (item.Info.Pocket == pocket)
					toRemove.Add(item);
			}
			foreach (var item in toRemove)
			{
				creature.Items.Remove(item);
				client.Send(PacketCreator.ItemRemove(creature, item));
			}

			client.Send(PacketCreator.ServerMessage(creature, "Cleared pocket '" + ((Pocket)pocket).ToString() + "'. (Deleted items: " + toRemove.Count.ToString()));

			return CommandResult.Okay;
		}

		private CommandResult Command_warp(WorldClient client, MabiCreature creature, string[] args, string msg)
		{
			if (args.Length < 2)
				return CommandResult.WrongParameter;

			MabiVertex pos = creature.GetPosition();
			uint region = uint.Parse(args[1]);
			uint x = pos.X, y = pos.Y;

			if (args.Length > 2)
			{
				x = uint.Parse(args[2]);

				if (args.Length > 3)
				{
					y = uint.Parse(args[3]);
				}
			}

			client.Warp(region, x, y);

			return CommandResult.Okay;
		}

		private CommandResult Command_jump(WorldClient client, MabiCreature creature, string[] args, string msg)
		{
			if (args.Length < 2)
				return CommandResult.WrongParameter;

			var pos = creature.GetPosition();

			uint region = creature.Region;
			uint x = pos.X, y = pos.Y;

			if (!uint.TryParse(args[1], out x))
				return CommandResult.Fail;

			if (args.Length >= 3 && !uint.TryParse(args[2], out y))
				return CommandResult.Fail;

			client.Warp(region, x, y);

			return CommandResult.Okay;
		}

		private CommandResult Command_go(WorldClient client, MabiCreature creature, string[] args, string msg)
		{
			if (args.Length < 2)
			{
				client.Send(PacketCreator.ServerMessage(creature, "Destinations: Tir Chonaill, Dugald Isle, Dunbarton, Gairech, Bangor, Emain Macha, Taillteann, Nekojima, GM Island"));
				return CommandResult.WrongParameter;
			}

			uint region = 0, x = 0, y = 0;
			var destination = msg.Substring(args[0].Length + 1).Trim();

			if (destination.StartsWith("tir")) { region = 1; x = 12991; y = 38549; }
			else if (destination.StartsWith("dugald")) { region = 16; x = 23017; y = 61244; }
			else if (destination.StartsWith("dun")) { region = 14; x = 38001; y = 38802; }
			else if (destination.StartsWith("gairech")) { region = 30; x = 39295; y = 53279; }
			else if (destination.StartsWith("bangor")) { region = 31; x = 12904; y = 12200; }
			else if (destination.StartsWith("emain")) { region = 52; x = 39818; y = 41621; }
			else if (destination.StartsWith("tail")) { region = 300; x = 212749; y = 192720; }
			else if (destination.StartsWith("neko")) { region = 600; x = 114430; y = 79085; }
			else if (destination.StartsWith("gm")) { region = 22; x = 2500; y = 2500; }
			else
			{
				client.Send(PacketCreator.ServerMessage(creature, "Unknown destination."));
				return CommandResult.Fail;
			}

			client.Warp(region, x, y);

			return CommandResult.Okay;
		}

		private CommandResult Command_motion(WorldClient client, MabiCreature creature, string[] args, string msg)
		{
			if (args.Length < 3)
				return CommandResult.WrongParameter;

			WorldManager.Instance.CreatureUseMotion(creature, ushort.Parse(args[1]), ushort.Parse(args[2]), true);

			return CommandResult.Okay;
		}

		private CommandResult Command_setrace(WorldClient client, MabiCreature creature, string[] args, string msg)
		{
			if (args.Length < 2)
				return CommandResult.WrongParameter;

			ushort raceId = 0;
			if (!ushort.TryParse(args[1], out raceId))
				return CommandResult.Fail;

			// TODO: Check if this can be done without relog.
			creature.Race = raceId;

			client.Send(PacketCreator.SystemMessage(creature, "Your race has been changed. You'll be logged out to complete the process."));
			client.Send(PacketCreator.SystemMessage(creature, "See you in 30 seconds :)"));

			client.Kill();

			return CommandResult.Okay;
		}

		private CommandResult Command_clean(WorldClient client, MabiCreature creature, string[] args, string msg)
		{
			var items = WorldManager.Instance.GetItemsInRegion(creature.Region);
			foreach (var item in items)
			{
				WorldManager.Instance.RemoveItem(item);
			}

			return CommandResult.Okay;
		}

		private CommandResult Command_statuseffect(WorldClient client, MabiCreature creature, string[] args, string msg)
		{
			ulong val1 = 0, val2 = 0, val3 = 0, val4 = 0;

			if (args.Length > 1)
				val1 = ulong.Parse(args[1], System.Globalization.NumberStyles.HexNumber);
			if (args.Length > 2)
				val2 = ulong.Parse(args[2], System.Globalization.NumberStyles.HexNumber);
			if (args.Length > 3)
				val3 = ulong.Parse(args[3], System.Globalization.NumberStyles.HexNumber);
			if (args.Length > 4)
				val4 = ulong.Parse(args[4], System.Globalization.NumberStyles.HexNumber);

			creature.StatusEffects.A = (CreatureConditionA)val1;
			creature.StatusEffects.B = (CreatureConditionA)val2;
			creature.StatusEffects.C = (CreatureConditionA)val3;
			creature.StatusEffects.D = (CreatureConditionA)val4;

			WorldManager.Instance.CreatureStatusEffectsChange(creature);

			return CommandResult.Okay;
		}

		private CommandResult Command_test(WorldClient client, MabiCreature creature, string[] args, string msg)
		{
			var searchId = 2000;
			var total = 0;
			foreach (var item in client.Character.Items)
			{
				if (item.Info.Class == searchId || item.StackItem == searchId)
					total += item.Info.Bundle;
			}

			client.Send(PacketCreator.ServerMessage(creature, "Your gold: " + total.ToString()));

			return CommandResult.Okay;
		}

		private CommandResult Command_skill(WorldClient client, MabiCreature creature, string[] args, string msg)
		{
			if (args.Length < 2)
				return CommandResult.WrongParameter;

			ushort skillId = 0;
			if (!ushort.TryParse(args[1], out skillId))
				return CommandResult.Fail;

			byte rank = (byte)SkillRank.R9;
			if (args.Length > 2 && !byte.TryParse(args[2], out rank))
				return CommandResult.Fail;

			MabiSkill skill = creature.Skills.FirstOrDefault(a => a.Info.Id == skillId);
			if (skill != null)
				creature.Skills.Remove(skill);

			skill = new MabiSkill(skillId, rank, creature.Race);
			creature.Skills.Add(skill);

			client.Send(new MabiPacket(0x6979, creature.Id).PutBin(skill.Info));
			client.Send(PacketCreator.ServerMessage(creature, "Congratulations, you got skill '" + skillId + "'."));

			return CommandResult.Okay;
		}


		//Leaving this in here
		//as a note to future devs. Due to appdomains, NPCs CANNOT be reloaded, the server
		//must be restarted. Loading NPCs into a new appdomain would incur a large performance hit.
		//However, we still need to be able to reload NPCs. So we'll just add new
		//assemblies in. This causes a large memory leak every time the code runs!--Xcelled
		private CommandResult Command_reloadnpcs(WorldClient client, MabiCreature creature, string[] args, string msg)
		{
			client.Send(PacketCreator.ServerMessage(creature, "Reloading NPCs..."));

			WorldServer.Instance.LoadData(WorldConf.DataPath, DataLoad.Npcs, true);

			WorldManager.Instance.RemoveAllNPCs();
			NPCManager.Instance.LoadNPCs();
			NPCManager.Instance.LoadSpawns();

			client.Send(PacketCreator.ServerMessage(creature, "done."));

			return CommandResult.Okay;
		}

		private CommandResult Command_reloaddata(WorldClient client, MabiCreature creature, string[] args, string msg)
		{
			client.Send(PacketCreator.ServerMessage(creature, "Reloading data..."));

			WorldServer.Instance.LoadData(WorldConf.DataPath, DataLoad.Data, true);

			this.Command_reloadnpcs(client, creature, null, null);

			return CommandResult.Okay;
		}
	}
}
