// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Linq;
using System.Text.RegularExpressions;
using Common.Constants;
using Common.Database;
using Common.Events;
using Common.Network;
using Common.Tools;
using Common.World;
using World.Tools;
using World.World;
using Common.Data;

namespace World.Network
{
	public partial class WorldServer : Server<WorldClient>
	{
		protected override void InitPacketHandlers()
		{
			this.RegisterPacketHandler(0x4E22, HandleLogin);
			this.RegisterPacketHandler(0x4E24, HandleDisconnect);
			this.RegisterPacketHandler(0x5208, HandleCharacterInfoRequest);
			this.RegisterPacketHandler(0x526C, HandleChat);
			this.RegisterPacketHandler(0x53FE, HandleRevive);
			this.RegisterPacketHandler(0x5401, HandleDeadMenu);
			this.RegisterPacketHandler(0x540E, HandleGesture);
			this.RegisterPacketHandler(0x55F0, HandleNPCTalkStart);
			this.RegisterPacketHandler(0x55F2, HandleNPCTalkEnd);
			this.RegisterPacketHandler(0x59D8, HandleItemMove);
			this.RegisterPacketHandler(0x59DA, HandleItemPick);
			this.RegisterPacketHandler(0x59DC, HandleItemDrop);
			this.RegisterPacketHandler(0x59E8, HandleItemSplit);
			this.RegisterPacketHandler(0x5BCD, HandleSwitchSet);
			this.RegisterPacketHandler(0x5BD0, HandleItemStateChange);
			this.RegisterPacketHandler(0x5DC4, HandleNPCTalkKeyword);
			this.RegisterPacketHandler(0x61A8, HandleIamWatchingYou);
			this.RegisterPacketHandler(0x6598, HandleEnterRegion);
			this.RegisterPacketHandler(0x6982, HandleSkillPrepare);
			this.RegisterPacketHandler(0x6983, HandleSkillPrepared);
			this.RegisterPacketHandler(0x6986, HandleSkillUse);
			this.RegisterPacketHandler(0x6987, HandleSkillUsed);
			this.RegisterPacketHandler(0x6989, HandleSkillCancel);
			this.RegisterPacketHandler(0x698A, HandleSkillStart);
			this.RegisterPacketHandler(0x698B, HandleSkillStop);
			this.RegisterPacketHandler(0x6E28, HandleChangeStance);
			this.RegisterPacketHandler(0x7150, HandleShopBuyItem);
			this.RegisterPacketHandler(0x7152, HandleShopSellItem);
			this.RegisterPacketHandler(0x7920, HandleCombatSetTarget);
			this.RegisterPacketHandler(0x8FC4, HandleTitleChange);
			this.RegisterPacketHandler(0x88B8, HandleAreaChange);
			this.RegisterPacketHandler(0x902C, HandlePetSummon);
			this.RegisterPacketHandler(0x9031, HandlePetUnsummon);
			this.RegisterPacketHandler(0x908B, HandlePortalUse);
			this.RegisterPacketHandler(0xA898, HandleUnknown1);
			this.RegisterPacketHandler(0xA9A9, HandleSosButton);
			this.RegisterPacketHandler(0xAA1C, HandleStunMeterRequest); // subscription ?
			//this.RegisterPacketHandler(0xA428, HandleMoonGateRequest);
			this.RegisterPacketHandler(0x13883, HandleNPCTalkSelect);
			this.RegisterPacketHandler(0x1FBD0, HandlePetMount);
			this.RegisterPacketHandler(0x1FBD2, HandlePetUnmount);
			this.RegisterPacketHandler(0x0F213303, HandleMove);
			this.RegisterPacketHandler(0x0FCC3231, HandleCombatAttack);
			this.RegisterPacketHandler(0x0FF23431, HandleMove);
			this.RegisterPacketHandler(0x4EEB, HandleGMCPSummon);
			this.RegisterPacketHandler(0x4EEC, HandleGMCPMoveToChar);
			this.RegisterPacketHandler(0x4EED, HandleGMCPMove);
			this.RegisterPacketHandler(0x4EEE, HandleGMCPRevive);
		}

		private void HandleLogin(WorldClient client, MabiPacket packet)
		{
			if (client.State != SessionState.Login)
				return;

			var userName = packet.GetString();
			var userName2 = packet.GetString(); // why...?
			var seedKey = packet.GetLong();
			var charID = packet.GetLong();
			byte unk1 = packet.GetByte();

			MabiPacket p;

			if (client.Account == null)
			{
				if (!MabiDb.Instance.IsSessionKey(userName, seedKey))
				{
					Logger.Warning("Invalid session key.");
					client.Kill();
					return;
				}

				client.Account = MabiDb.Instance.GetAccount(userName);
			}

			MabiPC creature = client.Account.Characters.FirstOrDefault(a => a.Id == charID);
			if (creature == null && (creature = client.Account.Pets.FirstOrDefault(a => a.Id == charID)) == null)
			{
				Logger.Warning("Creature not in account.");
				return;
			}

			creature.Save = true;

			client.Creatures.Add(creature);
			client.Character = creature;
			client.Character.Client = client;

			p = new MabiPacket(0x90A1, creature.Id);
			p.PutByte(0);
			p.PutByte(1);
			p.PutInt(0);
			p.PutInt(0);
			client.Send(p);

			p = new MabiPacket(0x4E23, 0x1000000000000001);
			p.PutByte(1);
			p.PutLong(creature.Id);
			p.PutLong(DateTime.Now);
			p.PutInt(1);
			p.PutString("");
			client.Send(p);

			p = new MabiPacket(0x701E, creature.Id);
			p.PutInt(0xEFFFFFFE);
			p.PutInt(0);
			client.Send(p);

			client.Send(PacketCreator.EnterRegionPermission(creature));

			client.State = SessionState.LoggedIn;

			ServerEvents.Instance.OnPlayerLogsIn(creature);
		}

		private void HandleDisconnect(WorldClient client, MabiPacket packet)
		{
			// TODO: Some check or move the unsafe stuff!

			Logger.Info("'" + client.Account.Username + "' is closing the connection. Saving...");

			MabiDb.Instance.SaveAccount(client.Account);

			foreach (var pc in client.Creatures)
			{
				WorldManager.Instance.RemoveCreature(pc);
			}

			client.Creatures.Clear();
			client.Character = null;
			client.Account = null;

			var p = new MabiPacket(0x4E25, 0x1000000000000001);
			p.PutByte(0);
			client.Send(p);
		}

		private void HandleCharacterInfoRequest(WorldClient client, MabiPacket packet)
		{
			var p = new MabiPacket(0x5209, 0x1000000000000001);

			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
			{
				p.PutByte(0);
				client.Send(p);
				return;
			}

			p.PutByte(1);
			(creature as MabiPC).AddPrivateEntityData(p);
			client.Send(p);

			if (creature.Owner != null)
			{
				var pos = creature.GetPosition();
				WorldManager.Instance.Effect(creature, 29, creature.Region, pos.X, pos.Y);

				if (creature.RaceInfo.VehicleType > 0)
				{
					WorldManager.Instance.VehicleUnbind(null, creature, true);
				}

				if (creature.IsDead())
				{
					WorldManager.Instance.Broadcast(new MabiPacket(0x5403, creature.Id).PutShort(1).PutInt(10).PutByte(0), SendTargets.Range, creature);
				}
			}
		}

		private void HandleChat(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var type = packet.GetByte();
			var message = packet.GetString();

			if (message[0] == WorldConf.CommandPrefix)
			{
				message = message.ToLower();

				var args = message.Substring(1).Split(' ');

				if (CommandHandler.Instance.Handle(client, creature, args, message))
					return;
			}

			WorldManager.Instance.CreatureTalk(creature, message, type);
		}

		private void HandleGesture(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			creature.StopMove();

			byte result = 0;

			var info = MabiData.MotionDb.Find(packet.GetString());
			if (info != null)
			{
				result = 1;
				WorldManager.Instance.CreatureUseMotion(creature, info.Category, info.Type, false);
			}

			var p = new MabiPacket(0x540F, creature.Id);
			p.PutByte(result);
			client.Send(p);
		}

		public void HandleGMCPMove(WorldClient client, MabiPacket packet)
		{
			var region = packet.GetInt();
			var x = packet.GetInt();
			var y = packet.GetInt();
			client.Warp(region, x, y);
		}

		private void HandleGMCPMoveToChar(WorldClient client, MabiPacket packet)
		{
			string targetName = packet.GetString();
			var target = WorldManager.Instance.GetCharacterByName(targetName);
			if (target == null)
			{
				Logger.Warning("Tried to move to a nonexisting character!");
				client.Send(PacketCreator.SystemMessage(client.Character, "Character \"" + targetName + "\" does not exist"));
				return;
			}
			var region = target.Region;
			var targetPos = target.GetPosition();
			client.Warp(region, targetPos.X, targetPos.Y);
		}

		private void HandleGMCPRevive(WorldClient client, MabiPacket packet)
		{
			var creature = WorldManager.Instance.GetCreatureById(packet.Id);
			if (creature == null || !creature.IsDead())
			{
				Logger.Warning("Tried to revive to a nonexisting/non-dead character!");
				client.Send(PacketCreator.SystemMessage(client.Character, "Character does not exist or is not knocked out."));
				return;				
			}
			WorldManager.Instance.CreatureRevive(creature);
			var pos = creature.GetPosition();
			var region = creature.Region;
			var response = new MabiPacket(0x53FF, creature.Id);
			response.PutInt(1);
			response.PutInt(region);
			response.PutInt(pos.X);
			response.PutInt(pos.Y);
			client.Send(response);

			creature.FullHeal();
		}

		private void HandleGMCPSummon(WorldClient client, MabiPacket packet)
		{
			var myPos = client.Character.GetPosition();
			var region = client.Character.Region;
			string targetName = packet.GetString();
			var target = WorldManager.Instance.GetCharacterByName(targetName);
			if (target == null)
			{
				Logger.Warning("Tried to summon a nonexisting character!");
				client.Send(PacketCreator.SystemMessage(client.Character, "Character \"" + targetName + "\" does not exist"));
				return;
			}
			if (target.Client == null || !(target.Client is WorldClient)) //We'll let the summon continue, but we should warn them.
			{
				Logger.Warning("Summoning a non-client controlled creature! (Lich in Dunby?)");
				//"Force" the summon. Probably not a good idea, but.........
				target.SetLocation(region, myPos.X, myPos.Y);
			}
			else
			{
				(target.Client as WorldClient).Warp(region, myPos.X, myPos.Y);
			}
		}

		private void HandleNPCTalkStart(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
			{
				return;
			}

			var npcId = packet.GetLong();

			var target = (MabiNPC)WorldManager.Instance.GetCreatureById(npcId);
			if (target == null)
			{
				Logger.Warning("Unknown NPC: " + npcId.ToString());
			}
			else if (target.Script == null)
			{
				Logger.Warning("Script for '" + target.Name + "' is null.");
				target = null;
			}
			else if (!WorldManager.InRange(creature, target, 1000))
			{
				client.Send(PacketCreator.MsgBox(creature, "You're too far away."));
				target = null;
			}

			var p = new MabiPacket(0x55F1, creature.Id);

			p.PutByte((byte)(target != null ? 1 : 0));
			p.PutLong(npcId);

			client.Send(p);

			if (target == null)
			{
				return;
			}

			client.NPCSession.Start(target);

			target.Script.OnTalk(client);
		}

		private void HandleNPCTalkEnd(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
			{
				return;
			}

			var npcId = packet.GetLong();
			var target = (MabiNPC)WorldManager.Instance.GetCreatureById(npcId);
			if (target == null || target.Script == null)
			{
				Logger.Warning("Script for '" + target.Name + "' is null.");
				return;
			}

			var p = new MabiPacket(0x55F3, creature.Id);
			p.PutByte(1);
			p.PutLong(target.Id);
			p.PutString("");
			client.Send(p);

			target.Script.OnEnd(client);

			client.NPCSession.Clear();
		}

		private void HandleNPCTalkKeyword(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null || client.NPCSession.SessionId == -1)
				return;

			var keyword = packet.GetString();

			var target = client.NPCSession.Target;
			if (target == null || client.NPCSession.SessionId == -1)
			{
				Logger.Debug("No target or no session.");
				return;
			}

			var p = new MabiPacket(0x5DC5, creature.Id);
			p.PutByte(1);
			p.PutString(keyword);
			client.Send(p);
		}

		private void HandleNPCTalkSelect(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null || client.NPCSession.SessionId == -1)
				return;

			var response = packet.GetString();
			var sessionId = packet.GetInt();
			var target = client.NPCSession.Target;

			if (target == null || sessionId != client.NPCSession.SessionId)
			{
				Logger.Debug("No target or sessionId incorrect (" + sessionId.ToString() + " : " + (client.NPCSession.SessionId) + ")");
				return;
			}

			int pos = -1;
			if ((pos = response.IndexOf("<return type=\"string\">")) < 1)
			{
				Logger.Debug("No return value found.");
				return;
			}

			pos += 22;

			response = response.Substring(pos, response.IndexOf('<', pos) - pos);

			if (response == "@end")
			{
				client.Send(new MabiPacket(0x59F9, creature.Id));

				target.Script.OnEnd(client);
				return;
			}

			target.Script.OnSelect(client, response);
		}

		private void HandleItemMove(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var itemId = packet.GetLong();
			var source = (Pocket)packet.GetByte();
			var target = (Pocket)packet.GetByte();
			var unk = packet.GetByte();
			var targetX = packet.GetByte();
			var targetY = packet.GetByte();

			var item = creature.GetItem(itemId);
			if (item == null || item.Type == ItemType.Hair || item.Type == ItemType.Face)
				return;

			// Stop moving when changing weapons
			if ((target >= Pocket.LeftHand1 && target <= Pocket.Arrow2) || (source >= Pocket.LeftHand1 && source <= Pocket.Arrow2))
				creature.StopMove();

			if (target != Pocket.Inventory)
			{
				targetX = 0;
				targetY = 0;
			}

			MabiPacket p;

			var collidingItem = creature.GetItemColliding(target, targetX, targetY, item);

			// If moved item collides, and can be added to the stack
			if (collidingItem != null && (item.Info.Class == collidingItem.Info.Class || item.Info.Class == collidingItem.StackItem) && collidingItem.BundleType != BundleType.None)
			{
				// If colliding stack doesn't have enough room
				if (collidingItem.Info.Bundle + item.Info.Bundle > collidingItem.BundleMax)
				{
					if (collidingItem.Info.Bundle < collidingItem.BundleMax)
					{
						item.Info.Bundle -= (ushort)(collidingItem.BundleMax - collidingItem.Info.Bundle);
						collidingItem.Info.Bundle = collidingItem.BundleMax;
					}
					else
					{
						collidingItem.Info.Bundle = item.Info.Bundle;
						item.Info.Bundle = item.BundleMax;
					}

					client.Send(PacketCreator.ItemAmount(creature, item));
				}
				else
				{
					collidingItem.Info.Bundle += item.Info.Bundle;
					item.Info.Bundle = 0;

					if (item.Type == ItemType.Sac && item.BundleType == BundleType.Sac)
					{
						client.Send(PacketCreator.ItemAmount(creature, item));
					}
					else
					{
						creature.Items.Remove(item);
						client.Send(PacketCreator.ItemRemove(creature, item));
					}
				}

				client.Send(PacketCreator.ItemAmount(creature, collidingItem));

				p = new MabiPacket(0x59D9, creature.Id);
				p.PutByte(1);
				client.Send(p);
				return;
			}

			p = new MabiPacket((uint)(collidingItem == null ? 0x59DE : 0x59DF), creature.Id);
			p.PutLong(item.Id);
			p.PutByte((byte)source);
			p.PutByte((byte)target);
			p.PutByte(unk);
			p.PutByte(targetX);
			p.PutByte(targetY);
			if (collidingItem != null)
			{
				p.PutLong(collidingItem.Id);
				p.PutByte(collidingItem.Info.Pocket);
				p.PutByte(1);
				p.PutByte(unk);
				p.PutByte(0);
				p.PutByte(0);

				collidingItem.Move(item.Info.Pocket, item.Info.X, item.Info.Y);
			}
			client.Send(p);

			item.Move(target, targetX, targetY);

			if (target >= Pocket.Armor && target <= Pocket.Accessory2)
			{
				WorldManager.Instance.CreatureChangeEquip(creature, item);
			}
			if (source >= Pocket.Armor && source <= Pocket.Accessory2)
			{
				WorldManager.Instance.CreatureMoveEquip(creature, source, target);
			}

			p = new MabiPacket(0x59D9, creature.Id);
			p.PutByte(1);
			client.Send(p);
		}

		private void HandleItemPick(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var itemId = packet.GetLong();

			byte result = 2;

			var item = WorldManager.Instance.GetItemById(itemId);
			if (item != null)
			{
				if (item.BundleType == BundleType.Stackable && item.Type == ItemType.Sac)
				{
					foreach (var invItem in creature.Items)
					{
						if (item.Info.Class == invItem.Info.Class || item.Info.Class == invItem.StackItem)
						{
							if (invItem.Info.Bundle + item.Info.Bundle > invItem.BundleMax)
							{
								if (invItem.Info.Bundle < invItem.BundleMax)
								{
									item.Info.Bundle -= (ushort)(invItem.BundleMax - invItem.Info.Bundle);
									invItem.Info.Bundle = invItem.BundleMax;

									client.Send(PacketCreator.ItemAmount(creature, invItem));

									result = 1;
								}
							}
							else
							{
								invItem.Info.Bundle += item.Info.Bundle;
								item.Info.Bundle = 0;

								WorldManager.Instance.RemoveItem(item);
								client.Send(PacketCreator.ItemAmount(creature, invItem));

								result = 1;
							}
						}
					}
				}

				if (item.Info.Bundle > 0 || (item.Type == ItemType.Sac && item.BundleType == BundleType.Sac))
				{
					var pos = creature.GetFreeItemSpace(item, Pocket.Inventory);
					if (pos != null)
					{
						WorldManager.Instance.RemoveItem(item);

						item.Move(Pocket.Inventory, pos.X, pos.Y);
						creature.Items.Add(item);

						client.Send(PacketCreator.ItemInfo(creature, item));

						result = 1;
					}
					else
					{
						client.Send(PacketCreator.SystemMessage(creature, "Not enough space."));
					}
				}
			}

			var response = new MabiPacket(0x59DB, creature.Id);
			response.PutByte(result);
			client.Send(response);
		}

		private void HandleItemDrop(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
			{
				return;
			}

			var itemId = packet.GetLong();
			var unk = packet.GetByte();

			var item = creature.Items.FirstOrDefault(a => a.Id == itemId);
			if (item == null || item.Type == ItemType.Hair || item.Type == ItemType.Face)
				return;

			// Notify clients of equip change if equipment is being dropped
			var source = (Pocket)item.Info.Pocket;
			if (source >= Pocket.Armor && source <= Pocket.Accessory2)
			{
				WorldManager.Instance.CreatureMoveEquip(creature, source, Pocket.None);
			}

			creature.Items.Remove(item);
			client.Send(PacketCreator.ItemRemove(creature, item));

			// Drop it
			var pos = creature.GetPosition();
			item.Region = creature.Region;
			item.Info.X = pos.X;
			item.Info.Y = pos.Y;
			item.Info.Pocket = 0;

			WorldManager.Instance.AddItem(item);

			// Done
			var p = new MabiPacket(0x59DD, creature.Id);
			p.PutByte(1);
			client.Send(p);
		}

		private void HandleItemSplit(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var itemId = packet.GetLong();
			var amount = packet.GetShort();
			var unk1 = packet.GetByte();

			packet = new MabiPacket(0x59E9, creature.Id);

			var item = creature.GetItem(itemId);
			if (item != null && item.BundleType != BundleType.None)
			{
				if (item.Info.Bundle < amount)
					amount = item.Info.Bundle;

				item.Info.Bundle -= amount;

				MabiItem splitItem;
				if (item.StackItem == 0)
					splitItem = new MabiItem(item);
				else
					splitItem = new MabiItem(item.StackItem);
				splitItem.Info.Bundle = amount;
				splitItem.Move(Pocket.Cursor, 0, 0);
				creature.Items.Add(splitItem);

				// New item on cursor
				client.Send(PacketCreator.ItemInfo(creature, splitItem));

				// Update amount or remove
				if (item.Info.Bundle > 0 || item.StackItem != 0)
				{
					client.Send(PacketCreator.ItemAmount(creature, item));
				}
				else
				{
					creature.Items.Remove(item);
					client.Send(PacketCreator.ItemRemove(creature, item));
				}

				packet.PutByte(1);
			}
			else
			{
				packet.PutByte(0);
			}

			client.Send(packet);
		}

		private void HandleSwitchSet(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var set = packet.GetByte();

			creature.StopMove();

			creature.WeaponSet = set;
			WorldManager.Instance.CreatureSwitchSet(creature);

			var response = new MabiPacket(0x5BCE, creature.Id);
			response.PutByte(1);
			client.Send(response);
		}

		private void HandleItemStateChange(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			// This might not be entirely correct, but works fine.
			// Robe is opened first, Helm secondly, then Robe and Helm are both closed.

			var firstTarget = packet.GetByte();
			var secondTarget = packet.GetByte();
			var unk = packet.GetByte();

			foreach (var target in new byte[] { firstTarget, secondTarget })
			{
				if (target > 0)
				{
					var item = creature.GetItem((Pocket)target);
					if (item != null)
					{
						item.Info.FigureA = (byte)(item.Info.FigureA == 1 ? 0 : 1);
						WorldManager.Instance.CreatureChangeEquip(creature, item);
					}
				}
			}

			MabiPacket response = new MabiPacket(0x5BD1, creature.Id);
			client.Send(response);
		}

		private void HandleEnterRegion(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
			{
				Logger.Warning("Creature not in account.");
				return;
			}

			// TODO: Maybe check if this action is valid.

			MabiPacket p;

			p = new MabiPacket(0x701F, creature.Id);
			p.PutInt(0xEFFFFFFE);
			client.Send(p);

			p = new MabiPacket(0x659C, 0x1000000000000001);
			p.PutByte(1);
			p.PutLong(creature.Id);
			p.PutLong(DateTime.Now);
			client.Send(p);

			WorldManager.Instance.AddCreature(creature);

			// Player logs in
			if (creature == client.Character)
			{
				// List of all entities in range
				var entities = WorldManager.Instance.GetEntitiesInRange(creature);
				if (entities.Count > 0)
				{
					client.Send(PacketCreator.EntitiesAppear(entities));
				}

				p = new MabiPacket(0x6599, creature.Id);
				var pos = creature.GetPosition();
				p.PutByte(1);
				p.PutInt(creature.Region);
				p.PutInt(pos.X);
				p.PutInt(pos.Y);
				client.Send(p);
			}
		}

		private void HandleSkillPrepare(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var skillId = packet.GetShort();
			var parameters = "";
			if (packet.GetElementType() == MabiPacket.ElementType.String)
				parameters = packet.GetString();

			if (parameters.Length > 0)
			{
				var match = Regex.Match(parameters, "ITEMID:[0-9]+:([0-9]+);");
				if (match.Success)
				{
					var itemId = Convert.ToUInt64(match.Groups[1].Value);
					var item = creature.GetItem(itemId);
					if (item == null)
						return;

					creature.ActiveSkillItem = item;
				}
			}

			var skill = creature.GetSkill(skillId);
			if (skill == null)
			{
				Logger.Warning(creature.Name + " tried to use skill '" + skillId.ToString() + "', which (s)he doesn't have.");
				return;
			}

			creature.ActiveSkillId = skillId;

			uint castTime = skill.RankInfo.LoadTime;

			switch ((SkillConst)skillId)
			{
				case SkillConst.Healing:
					creature.Mana -= skill.RankInfo.ManaCost;
					WorldManager.Instance.CreatureStatsUpdate(creature);
					WorldManager.Instance.Broadcast(new MabiPacket(0x9090, creature.Id).PutInt(11).PutString("healing"), SendTargets.Range, creature);
					break;

				case SkillConst.Windmill:
					WorldManager.Instance.Broadcast(new MabiPacket(0x9090, creature.Id).PutInt(11).PutString(""), SendTargets.Range, creature);
					break;

				case SkillConst.HiddenResurrection:
					client.Send(new MabiPacket(0x6983, creature.Id).PutShort(skillId).PutString(parameters));
					return;

				default:
					WorldManager.Instance.Broadcast(new MabiPacket(0x9090, creature.Id).PutInt(11).PutString("flashing"), SendTargets.Range, creature);
					break;
			}

			var r = new MabiPacket(0x6982, creature.Id);
			r.PutShort(skillId);

			if (parameters.Length > 0)
				r.PutString(parameters);
			else
				r.PutInt(castTime);

			client.Send(r);
		}

		private void HandleSkillPrepared(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var skillId = packet.GetShort();
			var parameters = "";
			if (packet.GetElementType() == MabiPacket.ElementType.String)
				parameters = packet.GetString();

			var skill = creature.GetSkill(skillId);
			if (skill == null)
			{
				Logger.Warning(creature.Name + " tried to use skill '" + skillId.ToString() + "', which (s)he doesn't have.");
				return;
			}

			creature.ActiveSkillStacks = skill.RankInfo.Stack;

			client.Send(new MabiPacket(0x6991, creature.Id).PutBytes(creature.ActiveSkillStacks, creature.ActiveSkillStacks).PutShort(creature.ActiveSkillId));

			switch ((SkillConst)skillId)
			{
				case SkillConst.Healing:
					WorldManager.Instance.Broadcast(new MabiPacket(0x9090, creature.Id).PutInt(13).PutString("healing_stack").PutBytes(creature.ActiveSkillStacks, 0), SendTargets.Range, creature);
					WorldManager.Instance.Broadcast(new MabiPacket(0x9090, creature.Id).PutInt(12).PutString("healing"), SendTargets.Range, creature);
					break;

				case SkillConst.FinalHit:
					WorldManager.Instance.Broadcast(new MabiPacket(0x9090, creature.Id).PutInt(69).PutBytes(1, 1), SendTargets.Range, creature);
					break;
			}

			var r = new MabiPacket(0x6983, creature.Id);
			r.PutShort(creature.ActiveSkillId);
			if (parameters.Length > 0)
				r.PutString(parameters);
			client.Send(r);
		}

		private void HandleSkillUse(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var skillId = packet.GetShort();
			var targetId = packet.GetLong();
			var unk1 = packet.GetInt();
			var unk2 = packet.GetInt();

			var skill = creature.GetSkill(skillId);
			if (skill == null)
			{
				Logger.Warning(creature.Name + " tried to use skill '" + skillId.ToString() + "', which (s)he doesn't have.");
				return;
			}

			switch ((SkillConst)skillId)
			{
				case SkillConst.Healing:
					creature.Life += skill.RankInfo.Var1;
					WorldManager.Instance.CreatureStatsUpdate(creature);

					creature.ActiveSkillStacks--;
					WorldManager.Instance.Broadcast(new MabiPacket(0x9090, creature.Id).PutInt(14).PutString("healing").PutLong(targetId), SendTargets.Range, creature);
					WorldManager.Instance.Broadcast(new MabiPacket(0x9090, creature.Id).PutInt(13).PutString("healing_stack").PutBytes(creature.ActiveSkillStacks, 0), SendTargets.Range, creature);
					client.Send(new MabiPacket(0x6992, creature.Id).PutBytes(creature.ActiveSkillStacks, 1, 0).PutShort(skillId));
					break;

				case SkillConst.Windmill:
					WorldManager.Instance.CreatureUseMotion(creature, 8, 4);

					if (MabiCombat.Attack(creature, null) != AttackResult.Okay)
					{
						client.Send(PacketCreator.Notice(creature, "Unable to use when there is no target.", NoticeType.MIDDLE));
						client.Send(new MabiPacket(0x698D, creature.Id));
						return;
					}
					goto default;

				case SkillConst.HiddenResurrection:
					creature.ActiveSkillTarget = WorldManager.Instance.GetCreatureById(targetId);
					if (creature.ActiveSkillTarget == null)
						return;

					WorldManager.Instance.Broadcast(new MabiPacket(0x9090, creature.Id).PutInt(14).PutString("healing_phoenix").PutLong(targetId), SendTargets.Range, creature);
					break;

				default:
					client.Send(new MabiPacket(0x6992, creature.Id).PutBytes(--creature.ActiveSkillStacks, 1, 0).PutShort(skillId));
					break;
			}

			var r = new MabiPacket(0x6986, creature.Id);
			r.PutShort(skillId);
			r.PutLong(targetId);
			r.PutInt(unk1);
			r.PutInt(unk2);
			client.Send(r);
		}

		private void HandleSkillUsed(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var skillId = packet.GetShort();

			var skill = creature.GetSkill(skillId);
			if (skill == null)
			{
				Logger.Warning(creature.Name + " tried to use skill '" + skillId.ToString() + "', which (s)he doesn't have.");
				return;
			}

			switch ((SkillConst)skillId)
			{
				case SkillConst.HiddenResurrection:
					if (creature.ActiveSkillItem == null || creature.ActiveSkillItem.Info.Bundle < 1)
						return;

					if (creature.ActiveSkillTarget.IsDead())
					{
						creature.ActiveSkillItem.Info.Bundle--;
						creature.Client.Send(PacketCreator.ItemAmount(creature, creature.ActiveSkillItem));

						WorldManager.Instance.CreatureRevive(creature.ActiveSkillTarget);

						client.Send(new MabiPacket(0x6987, creature.Id).PutShort(skillId).PutLong(creature.ActiveSkillTarget.Id).PutInts(0, 1));
					}

					creature.ActiveSkillId = 0;
					creature.ActiveSkillTarget = null;
					return;
			}

			var r = new MabiPacket(0x6987, creature.Id);
			r.PutShort(skillId);
			client.Send(r);

			if (creature.ActiveSkillStacks > 0 && skill.RankInfo.Stack > 1)
			{
				client.Send(new MabiPacket(0x6983, creature.Id).PutShort(creature.ActiveSkillId));
			}
			else
			{
				creature.ActiveSkillId = 0;
			}
		}

		private void HandleSkillCancel(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var unk1 = packet.GetByte();
			var unk2 = packet.GetByte();

			WorldManager.Instance.CreatureSkillCancel(creature);

			//var r = new MabiPacket(0x6989, creature.Id);
			//r.PutByte(unk1);
			//r.PutByte(unk2);
			//client.Send(r);
		}

		private void HandleSkillStart(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var skillId = packet.GetShort();
			var parameters = "";
			if (packet.GetElementType() == MabiPacket.ElementType.String)
				parameters = packet.GetString();

			switch ((SkillConst)skillId)
			{
				case SkillConst.ManaShield:
					creature.StatusEffects.A |= CreatureConditionA.ManaShield;
					WorldManager.Instance.CreatureStatusEffectsChange(creature);
					WorldManager.Instance.Broadcast(new MabiPacket(0x9090, creature.Id).PutInt(121), SendTargets.Range, creature);
					break;

				case SkillConst.Rest:
					creature.Status |= CreatureStates.SitDown;
					WorldManager.Instance.CreatureSitDown(creature);
					break;
			}

			var r = new MabiPacket(0x698A, creature.Id);
			r.PutShort(skillId);
			r.PutString("");
			client.Send(r);
		}

		private void HandleSkillStop(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var skillId = packet.GetShort();
			var parameters = packet.GetByte();

			switch ((SkillConst)skillId)
			{
				case SkillConst.Rest:
					creature.Status &= ~CreatureStates.SitDown;
					WorldManager.Instance.CreatureStandUp(creature);
					break;
			}

			var r = new MabiPacket(0x698B, creature.Id);
			r.PutShort(skillId);
			r.PutByte(1);
			client.Send(r);
		}

		private void HandleChangeStance(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var mode = packet.GetByte();

			// Clear target?
			if (mode == 0)
				WorldManager.Instance.CreatureSetTarget(creature, null);

			// Send info
			creature.BattleState = mode;
			WorldManager.Instance.CreatureChangeStance(creature);

			// Unlock
			client.Send(new MabiPacket(0x6E29, creature.Id));
		}

		private void HandleShopBuyItem(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			if (!client.NPCSession.IsValid())
				return;

			var itemId = packet.GetLong();
			var unk1 = packet.GetByte(); // 0:cursor, 1:inv
			var unk2 = packet.GetByte(); // storage gold?

			var newItem = client.NPCSession.Target.Script.Shop.GetItem(itemId);
			if (newItem == null)
				return;

			if (unk1 == 0)
			{
				newItem.Move(Pocket.Cursor, 0, 0);
			}
			else if (unk1 == 1)
			{
				var pos = creature.GetFreeItemSpace(newItem, Pocket.Inventory);
				if (pos != null)
				{
					newItem.Move(Pocket.Inventory, pos.X, pos.Y);
				}
				else
				{
					newItem = null;
				}
			}

			// TODO: check and reduce gold

			if (newItem != null)
			{
				creature.Items.Add(newItem);
				client.Send(PacketCreator.ItemInfo(creature, newItem));
			}

			var p = new MabiPacket(0x7151, creature.Id);
			client.Send(p);
		}

		private void HandleShopSellItem(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			if (!client.NPCSession.IsValid())
				return;

			var itemId = packet.GetLong();
			var unk1 = packet.GetByte();

			var item = creature.GetItem(itemId);
			if (item == null)
				return;

			// TODO: check space for gold and add it

			if (item != null)
			{
				creature.Items.Remove(item);
				client.Send(PacketCreator.ItemRemove(creature, item));

				// TODO: There could be an optional tab for rebuying sold things.
			}

			var p = new MabiPacket(0x7153, creature.Id);
			client.Send(p);
		}

		private void HandleTitleChange(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var title = packet.GetShort();

			// Make sure the character has this title enabled
			var character = creature as MabiPC;
			if (character.Titles.ContainsKey(title) && character.Titles[title])
			{
				creature.Title = title;
				WorldManager.Instance.CreatureChangeTitle(creature);
			}

			var answer = new MabiPacket(0x8FC6, creature.Id);
			answer.PutByte(1);
			answer.PutByte(0);
			client.Send(answer);
		}

		private void HandlePetSummon(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var petId = packet.GetLong();
			var unk1 = packet.GetByte();

			MabiPacket p;

			var pet = client.Account.Pets.FirstOrDefault(a => a.Id == petId);
			if (pet == null)
			{
				p = new MabiPacket(0x902D, creature.Id);
				p.PutByte(0);
				p.PutLong(petId);
				client.Send(p);
				return;
			}

			// Set pet position near the summoner
			var pos = creature.GetPosition();
			var rand = RandomProvider.Get();
			pet.Direction = (byte)rand.Next(255);
			pet.SetLocation(creature.Region, (uint)(pos.X + rand.Next(-400, 401)), (uint)(pos.Y + rand.Next(-400, 401)));

			pet.Owner = creature;
			client.Creatures.Add(pet);

			pet.Save = true;

			p = new MabiPacket(0x9024, creature.Id);
			p.PutLong(pet.Id);
			p.PutByte(2);
			WorldManager.Instance.Broadcast(p, SendTargets.Range, creature);

			p = new MabiPacket(0x902D, creature.Id);
			p.PutByte(1);
			p.PutLong(petId);
			client.Send(p);

			p = new MabiPacket(0x701E, petId);
			p.PutInt(0xEFFFFFFE);
			p.PutInt(0);
			client.Send(p);

			client.Send(PacketCreator.EnterRegionPermission(pet));
		}

		private void HandlePetUnsummon(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var petId = packet.GetLong();

			MabiPacket p;

			var pet = client.Creatures.FirstOrDefault(a => a.Id == petId);
			if (pet == null)
			{
				p = new MabiPacket(0x9032, creature.Id);
				p.PutByte(0);
				p.PutLong(petId);
				client.Send(p);
				return;
			}

			client.Creatures.Remove(pet);

			var pos = pet.GetPosition();
			WorldManager.Instance.Effect(pet, 29, pet.Region, pos.X, pos.Y);
			WorldManager.Instance.RemoveCreature(pet);

			if (pet.Owner.Vehicle == pet)
			{
				WorldManager.Instance.VehicleUnbind(pet.Owner, pet);
				pet.Owner.Vehicle = null;
			}

			// ?
			p = new MabiPacket(0x9025, creature.Id);
			p.PutLong(pet.Id);
			WorldManager.Instance.Broadcast(p, SendTargets.Range, creature);

			// Disappear
			p = new MabiPacket(0x4E2A, 0x1000000000000001);
			p.PutLong(pet.Id);
			WorldManager.Instance.Broadcast(p, SendTargets.Range, creature);

			// Result
			p = new MabiPacket(0x9032, creature.Id);
			p.PutByte(1);
			p.PutLong(petId);
			client.Send(p);
		}

		private void HandlePetMount(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var petId = packet.GetLong();

			MabiPacket p;

			var pet = client.Account.Pets.FirstOrDefault(a => a.Id == petId);
			if (pet == null || pet.IsDead())
			{
				p = new MabiPacket(0x1FBD1, creature.Id);
				p.PutByte(0);
				client.Send(p);
				return;
			}

			creature.Vehicle = pet;

			WorldManager.Instance.VehicleBind(creature, pet);

			p = new MabiPacket(0x1FBD1, creature.Id);
			p.PutByte(1);
			client.Send(p);
		}

		private void HandlePetUnmount(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			MabiPacket p;

			if (creature.Vehicle == null)
			{
				p = new MabiPacket(0x1FBD3, creature.Id);
				p.PutByte(0);
				client.Send(p);
				return;
			}

			WorldManager.Instance.VehicleUnbind(creature, creature.Vehicle);

			creature.Vehicle = null;

			p = new MabiPacket(0x1FBD3, creature.Id);
			p.PutByte(1);
			client.Send(p);
		}

		private void HandlePortalUse(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			byte success = 0;

			var portalId = packet.GetLong();
			var portalInfo = MabiData.PortalDb.Find(portalId);
			if (portalInfo != null)
			{
				if (creature.Region == portalInfo.Region && WorldManager.InRange(creature, portalInfo.X, portalInfo.Y, 1500))
				{
					success = 1;
					client.Warp(portalInfo.ToRegion, portalInfo.ToX, portalInfo.ToY);
				}
			}
			else
			{
				Logger.Warning("Missing portal: " + portalId.ToString());
			}

			var p = new MabiPacket(0x908C, creature.Id);
			p.PutByte(success);
			client.Send(p);
		}

		private void HandleMove(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var x = packet.GetInt();
			var y = packet.GetInt();

			var pos = creature.GetPosition();
			var dest = new MabiVertex(x, y);

			// TODO: Collision

			var walking = (packet.Op == 0x0FF23431);

			// TODO: Update creature position on unmount?
			creature.StartMove(dest, walking);
			if (creature.Vehicle != null)
				creature.Vehicle.StartMove(dest, walking);

			WorldManager.Instance.CreatureMove(creature, pos, dest, walking);
		}

		private void HandleCombatSetTarget(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var targetId = packet.GetLong();
			var unk1 = packet.GetByte();
			var unk2 = packet.GetString();

			MabiCreature target = null;
			if (targetId > 0)
			{
				target = WorldManager.Instance.GetCreatureById(targetId);
				if (target == null)
				{
					Logger.Warning("Target '" + targetId + "' doesn't exist.");
					return;
				}
			}

			creature.Target = target;
			WorldManager.Instance.CreatureSetTarget(creature, target);
		}

		private void HandleCombatAttack(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			if (creature.Vehicle != null)
				creature = creature.Vehicle;

			// TODO: Check if mount is able to attack anything?

			var attackResult = AttackResult.None;

			var target = WorldManager.Instance.GetCreatureById(packet.GetLong());
			if (target != null)
			{
				attackResult = MabiCombat.Attack(creature, target);
			}

			var answer = new MabiPacket(0x7D01, creature.Id);

			if (attackResult == AttackResult.Okay)
			{
				answer.PutByte(1);
			}
			else if (attackResult == AttackResult.OutOfRange)
			{
				var creaturePos = creature.GetPosition();
				var targetPos = target.GetPosition();

				answer.PutByte(100);
				answer.PutLong(target.Id);
				answer.PutByte(0);
				answer.PutByte(0);
				answer.PutInt(creaturePos.X);
				answer.PutInt(creaturePos.Y);
				answer.PutByte(0);
				answer.PutInt(targetPos.X);
				answer.PutInt(targetPos.Y);
				answer.PutString("");
			}
			else if (attackResult == AttackResult.None)
			{
				client.Send(PacketCreator.SystemMessage(creature, "Something went wrong here, sry =/"));
			}
			else
			{
				// Stunned
				answer.PutByte(0);
			}

			client.Send(answer);

			client.Send(new MabiPacket(0xAA1D, creature.Id).PutLong(target.Id).PutByte(1).PutFloat(target.Stun));
		}

		public void HandleDeadMenu(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null && creature.IsDead())
				return;

			var response = new MabiPacket(0x5402, creature.Id);
			response.PutByte(1);
			response.PutString("town;here;;stay");
			response.PutInt(0);
			response.PutInt(0);
			client.Send(response);
		}

		public void HandleRevive(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null && creature.IsDead())
				return;

			// 1 = Town, 2 = Here, 9 = Wait
			var option = packet.GetInt();

			uint region = 0, x = 0, y = 0;

			// TODO: Town etc support.
			if (option == 9)
			{
				var feather = new MabiPacket(0x5403, creature.Id);
				feather.PutShort((ushort)(creature.WaitingForRes ? 4 : 5));
				feather.PutInt(0);
				feather.PutInt(1);
				feather.PutInt(2);
				feather.PutInt(9);
				if (!creature.WaitingForRes)
					feather.PutInt(10);
				feather.PutByte(0);
				WorldManager.Instance.Broadcast(feather, SendTargets.Range, creature);

				creature.WaitingForRes = !creature.WaitingForRes;
			}
			else
			{
				WorldManager.Instance.CreatureRevive(creature);

				var pos = creature.GetPosition();
				region = creature.Region;
				x = pos.X;
				y = pos.Y;
			}

			var response = new MabiPacket(0x53FF, creature.Id);
			response.PutInt(1);
			response.PutInt(region);
			response.PutInt(x);
			response.PutInt(y);
			client.Send(response);
		}

		public void HandleUnknown1(WorldClient client, MabiPacket packet)
		{
			client.Send(new MabiPacket(0xA899, client.Character.Id));

			// Mails
			// client.Send(new MabiPacket(0x7255, client.Character.Id).PutInt(3));
		}

		public void HandleIamWatchingYou(WorldClient client, MabiPacket packet)
		{
			// TODO : Send entities?
		}

		public void HandleSosButton(WorldClient client, MabiPacket packet)
		{
			// Enable = 1, Disable = 0
			client.Send(new MabiPacket(0xA9AA, client.Character.Id).PutByte(false));
		}

		public void HandleAreaChange(WorldClient client, MabiPacket packet)
		{
			var unk1 = packet.GetLong(); // 00B000010003007C ?
			var unk2 = packet.GetInt(); // Area?
			var unk3 = packet.GetString();

			// TODO: Do something with this?
		}

		public void HandleStunMeterRequest(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null && creature.IsDead())
				return;

			var targetId = packet.GetLong();

			// TODO: Check target, get target, send back actual value

			var response = new MabiPacket(0xAA1D, creature.Id);
			response.PutLong(targetId);
			if (targetId > 0)
			{
				response.PutByte(1);
				response.PutFloat(50);
			}
			else
				response.PutByte(0);

			client.Send(response);
		}
	}
}
