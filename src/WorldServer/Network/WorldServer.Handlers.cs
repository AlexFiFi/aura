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
			this.RegisterPacketHandler(Op.LoginW, HandleLogin);
			this.RegisterPacketHandler(Op.DisconnectW, HandleDisconnect);
			this.RegisterPacketHandler(Op.CharInfoRequestW, HandleCharacterInfoRequest);

			this.RegisterPacketHandler(Op.Walk, HandleMove);
			this.RegisterPacketHandler(Op.Run, HandleMove);
			this.RegisterPacketHandler(Op.Chat, HandleChat);

			this.RegisterPacketHandler(Op.ItemMove, HandleItemMove);
			this.RegisterPacketHandler(Op.ItemPickUp, HandleItemPickUp);
			this.RegisterPacketHandler(Op.ItemDrop, HandleItemDrop);
			this.RegisterPacketHandler(Op.ItemDestroy, HandleItemDestroy);
			this.RegisterPacketHandler(Op.ItemSplit, HandleItemSplit);
			this.RegisterPacketHandler(Op.SwitchSet, HandleSwitchSet);
			this.RegisterPacketHandler(Op.ItemStateChange, HandleItemStateChange);
			this.RegisterPacketHandler(Op.ItemUse, HandleItemUse);

			this.RegisterPacketHandler(Op.NPCTalkStart, HandleNPCTalkStart);
			this.RegisterPacketHandler(Op.NPCTalkEnd, HandleNPCTalkEnd);
			this.RegisterPacketHandler(Op.NPCTalkKeyword, HandleNPCTalkKeyword);
			this.RegisterPacketHandler(Op.NPCTalkSelect, HandleNPCTalkSelect);
			this.RegisterPacketHandler(Op.ShopBuyItem, HandleShopBuyItem);
			this.RegisterPacketHandler(Op.ShopSellItem, HandleShopSellItem);

			this.RegisterPacketHandler(Op.ChangeStance, HandleChangeStance);
			this.RegisterPacketHandler(Op.CombatSetTarget, HandleCombatSetTarget);
			this.RegisterPacketHandler(Op.CombatAttack, HandleCombatAttack);
			this.RegisterPacketHandler(Op.StunMeter, HandleStunMeterDummy);
			this.RegisterPacketHandler(Op.SubsribeStun, HandleStunMeterRequest); // subscription ?
			this.RegisterPacketHandler(Op.Revive, HandleRevive);
			this.RegisterPacketHandler(Op.DeadMenu, HandleDeadMenu);

			this.RegisterPacketHandler(Op.SkillPrepare, HandleSkillPrepare);
			this.RegisterPacketHandler(Op.SkillPrepared, HandleSkillPrepared);
			this.RegisterPacketHandler(Op.SkillUse, HandleSkillUse);
			this.RegisterPacketHandler(Op.SkillUsed, HandleSkillUsed);
			this.RegisterPacketHandler(Op.SkillCancel, HandleSkillCancel);
			this.RegisterPacketHandler(Op.SkillStart, HandleSkillStart);
			this.RegisterPacketHandler(Op.SkillStop, HandleSkillStop);

			this.RegisterPacketHandler(Op.PetSummon, HandlePetSummon);
			this.RegisterPacketHandler(Op.PetUnsummon, HandlePetUnsummon);
			this.RegisterPacketHandler(Op.PetMount, HandlePetMount);
			this.RegisterPacketHandler(Op.PetUnmount, HandlePetUnmount);

			this.RegisterPacketHandler(Op.TouchProp, HandleTouchProp);
			this.RegisterPacketHandler(Op.HitProp, HandleHitProp);

			this.RegisterPacketHandler(Op.EnterRegion, HandleEnterRegion);
			this.RegisterPacketHandler(Op.AreaChange, HandleAreaChange);

			this.RegisterPacketHandler(Op.ChangeTitle, HandleTitleChange);
			this.RegisterPacketHandler(Op.MailsRequest, HandleMailsRequest);
			this.RegisterPacketHandler(Op.SosButton, HandleSosButton);
			this.RegisterPacketHandler(Op.MoonGateRequest, HandleMoonGateRequest);
			this.RegisterPacketHandler(Op.UseGesture, HandleGesture);
			this.RegisterPacketHandler(0x61A8, HandleIamWatchingYou);
			this.RegisterPacketHandler(Op.HomesteadInfoRequest, HandleHomesteadInfo);

			this.RegisterPacketHandler(Op.GMCPSummon, HandleGMCPSummon);
			this.RegisterPacketHandler(Op.GMCPMoveToChar, HandleGMCPMoveToChar);
			this.RegisterPacketHandler(Op.GMCPMove, HandleGMCPMove);
			this.RegisterPacketHandler(Op.GMCPRevive, HandleGMCPRevive);
			this.RegisterPacketHandler(Op.GMCPInvisibility, HandleGMCPInvisibility);
			this.RegisterPacketHandler(Op.GMCPExpel, HandleGMCPExpel);
			this.RegisterPacketHandler(Op.GMCPBan, HandleGMCPBan);
			this.RegisterPacketHandler(Op.GMCPClose, (c, p) => { }); // TODO: Maybe add this to a gm log.
			this.RegisterPacketHandler(Op.GMCPNPCList, HandleGMCPListNPCs);
		}

#pragma warning disable 0162
		private void HandleLogin(WorldClient client, MabiPacket packet)
		{
			if (client.State != SessionState.Login)
				return;

			var userName = packet.GetString();
			if (Op.Version > 160000)
				packet.GetString(); // double acc name
			var seedKey = packet.GetLong();
			var charID = packet.GetLong();
			//byte unk1 = packet.GetByte();

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

			//p = new MabiPacket(0x90A1, creature.Id);
			//p.PutByte(0);
			//p.PutByte(1);
			//p.PutInt(0);
			//p.PutInt(0);
			//client.Send(p);

			p = new MabiPacket(Op.LoginWR, 0x1000000000000001);
			p.PutByte(1);
			p.PutLong(creature.Id);
			p.PutLong(DateTime.Now);
			p.PutInt(1);
			p.PutString("");
			client.Send(p);

			p = new MabiPacket(Op.LoginWLock, creature.Id);
			p.PutInt(0xEFFFFFFE);
			p.PutInt(0);
			client.Send(p);

			client.Send(PacketCreator.EnterRegionPermission(creature));

			client.State = SessionState.LoggedIn;

			ServerEvents.Instance.OnPlayerLogsIn(creature);
		}
#pragma warning restore 0162

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

			var p = new MabiPacket(Op.DisconnectWR, 0x1000000000000001);
			p.PutByte(0);
			client.Send(p);
		}

		private void HandleCharacterInfoRequest(WorldClient client, MabiPacket packet)
		{
			var p = new MabiPacket(Op.CharInfoRequestWR, 0x1000000000000001);

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
				if (creature.RaceInfo.VehicleType > 0)
				{
					WorldManager.Instance.VehicleUnbind(null, creature, true);
				}

				if (creature.IsDead())
				{
					WorldManager.Instance.Broadcast(new MabiPacket(Op.DeadFeather, creature.Id).PutShort(1).PutInt(10).PutByte(0), SendTargets.Range, creature);
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

			var p = new MabiPacket(Op.UseGestureR, creature.Id);
			p.PutByte(result);
			client.Send(p);
		}

		public void HandleItemUse(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null || creature.IsDead())
				return;

			var itemId = packet.GetLong();

			var item = creature.GetItem(itemId);
			if (item == null || item.DataInfo == null || item.Type != ItemType.Usable)
			{
				client.Send(new MabiPacket(Op.UseItemR, creature.Id).PutByte(0));
				return;
			}

			// Doing this with ifs might be better.
			switch ((UsableType)item.DataInfo.UsableType)
			{
				// TODO: Remaining positive and negative effects.
				case UsableType.Life:
					creature.Life += item.DataInfo.UsableVar;
					break;
				case UsableType.Mana:
					creature.Mana += item.DataInfo.UsableVar;
					break;
				case UsableType.Stamina:
					creature.Stamina += item.DataInfo.UsableVar;
					break;
				case UsableType.Injury:
					creature.Injuries -= item.DataInfo.UsableVar;
					break;
				case UsableType.LifeMana:
					creature.Life += item.DataInfo.UsableVar;
					creature.Mana += item.DataInfo.UsableVar;
					break;
				case UsableType.LifeStamina:
					creature.Life += item.DataInfo.UsableVar;
					creature.Stamina += item.DataInfo.UsableVar;
					break;
				case UsableType.Food:
					creature.Hunger -= item.DataInfo.UsableVar;
					break;
				case UsableType.Recovery:
					// Full Recovery
					if (item.DataInfo.UsableVar == 100)
					{
						// Manually, to handle multiple recovery items at
						// once later, and we don't need 2 update packets.
						creature.Injuries = 0;
						creature.Hunger = 0;
						creature.Life = creature.LifeMax;
						creature.Mana = creature.ManaMax;
						creature.Stamina = creature.StaminaMax;
					}
					// Various chocolates? Full recovery as well?
					//else if (item.DataInfo.UsableVar == 300)
					//{
					//}
					// Recovery booster
					//else if (item.DataInfo.UsableVar == 1)
					//{
					//}
					else goto default;
					break;
				case UsableType.Antidote:
				case UsableType.Elixir:
				case UsableType.Others:
				default:
					client.Send(new MabiPacket(Op.UseItemR, creature.Id).PutByte(0));
					Logger.Unimplemented("This usable type is not supported yet.");
					return;
			}

			item -= 1;
			if (item.Count > 0)
			{
				client.Send(PacketCreator.ItemAmount(creature, item));
			}
			else
			{
				creature.Items.Remove(item);
				client.Send(PacketCreator.ItemRemove(creature, item));
			}

			WorldManager.Instance.CreatureStatsUpdate(creature);

			client.Send(new MabiPacket(Op.UseItemR, creature.Id).PutByte(1).PutInt(item.Info.Class));
		}

		public void HandleGMCPMove(WorldClient client, MabiPacket packet)
		{
			if (client.Account.Authority < Authority.GameMaster)
				return;

			var region = packet.GetInt();
			var x = packet.GetInt();
			var y = packet.GetInt();

			client.Warp(region, x, y);
		}

		private void HandleGMCPMoveToChar(WorldClient client, MabiPacket packet)
		{
			if (client.Account.Authority < Authority.GameMaster)
				return;

			var targetName = packet.GetString();
			var target = WorldManager.Instance.GetCharacterByName(targetName, false);
			if (target == null)
			{
				client.Send(PacketCreator.MsgBox(client.Character, "Character '" + targetName + "' couldn't be found."));
				return;
			}

			var targetPos = target.GetPosition();
			client.Warp(target.Region, targetPos.X, targetPos.Y);
		}

		private void HandleGMCPRevive(WorldClient client, MabiPacket packet)
		{
			if (client.Account.Authority < Authority.GameMaster)
				return;

			var creature = WorldManager.Instance.GetCreatureById(packet.Id);
			if (creature == null || !creature.IsDead())
				return;

			var pos = creature.GetPosition();
			var region = creature.Region;

			var response = new MabiPacket(Op.Revived, creature.Id);
			response.PutInt(1);
			response.PutInt(region);
			response.PutInt(pos.X);
			response.PutInt(pos.Y);
			client.Send(response);

			WorldManager.Instance.CreatureRevive(creature);

			creature.FullHeal();
		}

		private void HandleGMCPSummon(WorldClient client, MabiPacket packet)
		{
			if (client.Account.Authority < Authority.GameMaster)
				return;

			var targetName = packet.GetString();
			var target = WorldManager.Instance.GetCharacterByName(targetName);
			if (target == null || target.Client == null)
			{
				client.Send(PacketCreator.MsgBox(client.Character, "Character '" + targetName + "' couldn't be found."));
				return;
			}

			var targetClient = (target.Client as WorldClient);
			var pos = client.Character.GetPosition();

			targetClient.Send(PacketCreator.ServerMessage(target, "You've been summoned by '" + client.Character.Name + "'."));
			targetClient.Warp(client.Character.Region, pos.X, pos.Y);
		}

		private void HandleGMCPListNPCs(WorldClient client, MabiPacket packet)
		{
			client.Send(PacketCreator.SystemMessage(client.Character, "Unimplimented at the moment!"));
		}

		private void HandleGMCPInvisibility(WorldClient client, MabiPacket packet)
		{
			if (client.Account.Authority < Authority.GameMaster)
				return;

			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var toggle = packet.GetByte();
			creature.StatusEffects.A = toggle == 1 ? (creature.StatusEffects.A | CreatureConditionA.Invisible) : (creature.StatusEffects.A & ~CreatureConditionA.Invisible);
			WorldManager.Instance.CreatureStatusEffectsChange(creature);

			var p = new MabiPacket(Op.GMCPInvisibilityR, creature.Id);
			p.PutByte(1);
			client.Send(p);
		}

		private void HandleGMCPExpel(WorldClient client, MabiPacket packet)
		{
			if (client.Account.Authority < Authority.GameMaster)
				return;

			var targetName = packet.GetString();
			var target = WorldManager.Instance.GetCharacterByName(targetName);
			if (target == null || target.Client == null)
			{
				client.Send(PacketCreator.MsgBox(client.Character, "Character '" + targetName + "' couldn't be found."));
				return;
			}

			client.Send(PacketCreator.MsgBox(client.Character, "'" + targetName + "' has been kicked."));

			// Better kill the connection, modders could bypass a dc request.
			target.Client.Kill();
		}

		private void HandleGMCPBan(WorldClient client, MabiPacket packet)
		{
			if (client.Account.Authority < Authority.GameMaster)
				return;

			var targetName = packet.GetString();
			var target = WorldManager.Instance.GetCharacterByName(targetName);
			if (target == null || target.Client == null)
			{
				client.Send(PacketCreator.MsgBox(client.Character, "Character '" + targetName + "' couldn't be found."));
				return;
			}

			var end = DateTime.Now.AddMinutes(packet.GetInt());
			target.Client.Account.BannedExpiration = end;
			target.Client.Account.BannedReason = packet.GetString();

			client.Send(PacketCreator.MsgBox(client.Character, "'" + targetName + "' has been banned till '" + end.ToString() + "'."));

			// Better kill the connection, modders could bypass a dc request.
			target.Client.Kill();
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

			var p = new MabiPacket(Op.NPCTalkStartR, creature.Id);

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

			var p = new MabiPacket(Op.NPCTalkEndR, creature.Id);
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

			var p = new MabiPacket(Op.NPCTalkKeywordR, creature.Id);
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
				client.Send(new MabiPacket(Op.NPCTalkSelectEnd, creature.Id));

				target.Script.OnEnd(client);
				return;
			}
			else if (response.StartsWith("@input"))
			{
				var splitted = response.Split(':');
				target.Script.OnSelect(client, splitted[0], splitted[1]);
			}
			else
			{
				target.Script.OnSelect(client, response);
			}
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

			// Inv -> Cursor
			// --------------------------------------------------------------
			if (target == Pocket.Cursor)
			{
				// Move
				client.Send(
					new MabiPacket(Op.ItemMoveInfo, creature.Id)
					.PutLong(item.Id).PutBytes((byte)source, (byte)target)
					.PutByte(unk).PutBytes(0, 0)
				);

				item.Move(target, targetX, targetY);
				this.CheckItemMove(creature, item, source);

				// Update euip
				//if (source >= Pocket.Armor && source <= Pocket.Accessory2)
				//    WorldManager.Instance.CreatureUnequip(creature, source, target);

				// Okay
				client.Send(new MabiPacket(Op.ItemMoveR, creature.Id).PutByte(1));
				return;
			}

			// Cursor -> Inv
			// --------------------------------------------------------------

			// Check for item at the target space
			var collidingItem = creature.GetItemColliding(target, targetX, targetY, item);

			// Is there a collision?
			if (collidingItem != null && ((collidingItem.StackType == BundleType.Sac && (collidingItem.StackItem == item.Info.Class || collidingItem.StackItem == item.StackItem)) || (item.StackType == BundleType.Stackable && item.Info.Class == collidingItem.Info.Class)))
			{
				if (collidingItem.Info.Amount < collidingItem.StackMax)
				{
					var diff = (ushort)(collidingItem.StackMax - collidingItem.Info.Amount);

					collidingItem.Info.Amount += Math.Min(diff, item.Info.Amount);
					client.Send(PacketCreator.ItemAmount(creature, collidingItem));

					item.Info.Amount -= Math.Min(diff, item.Info.Amount);
					if (item.Info.Amount > 0)
					{
						client.Send(PacketCreator.ItemAmount(creature, item));
					}
					else
					{
						creature.Items.Remove(item);
						client.Send(PacketCreator.ItemRemove(creature, item));
					}

					client.Send(new MabiPacket(Op.ItemMoveR, creature.Id).PutByte(1));

					return;
				}
			}

			var p = new MabiPacket((uint)(collidingItem == null ? Op.ItemMoveInfo : Op.ItemSwitchInfo), creature.Id);
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
			this.CheckItemMove(creature, item, target);

			// Update Equip
			if (target.IsEquip())
				WorldManager.Instance.CreatureEquip(creature, item);

			client.Send(new MabiPacket(Op.ItemMoveR, creature.Id).PutByte(1));
		}

		/// <summary>
		/// Checks for moving second hand equipment and unequiping,
		/// and sends the needed packets.
		/// </summary>
		/// <param name="creature"></param>
		/// <param name="item"></param>
		/// <param name="pocket"></param>
		// TODO: Move this to MabiCreature.
		private void CheckItemMove(MabiCreature creature, MabiItem item, Pocket pocket)
		{
			// Check for moving second hand
			if (pocket == Pocket.LeftHand1 || pocket == Pocket.LeftHand2)
			{
				var secSource = pocket + 2; // RightHand1/2
				var secItem = creature.GetItemInPocket(secSource);
				if (secItem != null || (secItem = creature.GetItemInPocket(secSource += 2)) != null)
				{
					var secTarget = Pocket.Inventory;
					var free = creature.GetFreeItemSpace(secItem, secTarget);
					if (free == null)
					{
						secTarget = Pocket.Temporary;
						free = new MabiVertex(0, 0);
					}
					creature.Client.Send(
						new MabiPacket(Op.ItemMoveInfo, creature.Id)
						.PutLong(secItem.Id).PutBytes((byte)secSource, (byte)secTarget)
						.PutByte(2).PutBytes((byte)free.X, (byte)free.Y)
					);
					secItem.Move(secTarget, free.X, free.Y);
					WorldManager.Instance.CreatureUnequip(creature, secSource);
				}
			}

			// Notify clients of equip change if equipment is being dropped
			if (pocket.IsEquip())
				WorldManager.Instance.CreatureUnequip(creature, pocket);
		}

		private void HandleItemDrop(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var itemId = packet.GetLong();
			var unk = packet.GetByte();

			var item = creature.Items.FirstOrDefault(a => a.Id == itemId);
			if (item == null || item.Type == ItemType.Hair || item.Type == ItemType.Face)
				return;

			var source = (Pocket)item.Info.Pocket;

			creature.Items.Remove(item);
			this.CheckItemMove(creature, item, source);
			client.Send(PacketCreator.ItemRemove(creature, item));

			// Drop it
			var pos = creature.GetPosition();
			item.Region = creature.Region;
			item.Info.X = pos.X;
			item.Info.Y = pos.Y;
			item.Info.Pocket = 0;
			item.DisappearTime = DateTime.Now.AddSeconds(60);

			WorldManager.Instance.AddItem(item);

			// Done
			var p = new MabiPacket(Op.ItemDropR, creature.Id);
			p.PutByte(1);
			client.Send(p);
		}

		public void HandleItemDestroy(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var itemId = packet.GetLong();
			var item = creature.GetItem(itemId);
			if (item == null || item.Type == ItemType.Hair || item.Type == ItemType.Face)
				return;

			creature.Items.Remove(item);
			this.CheckItemMove(creature, item, (Pocket)item.Info.Pocket);

			client.Send(PacketCreator.ItemRemove(creature, item));
			client.Send(new MabiPacket(Op.ItemDestroyR, creature.Id).PutByte(1));
		}

		private void HandleItemPickUp(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var itemId = packet.GetLong();

			byte result = 2;

			var item = WorldManager.Instance.GetItemById(itemId);
			if (item != null)
			{
				if (item.StackType == BundleType.Stackable)// && item.Type == ItemType.Sac)
				{
					foreach (var invItem in creature.Items)
					{
						if (item.Info.Class == invItem.Info.Class || item.Info.Class == invItem.StackItem)
						{
							if (invItem.Info.Amount + item.Info.Amount > invItem.StackMax)
							{
								if (invItem.Info.Amount < invItem.StackMax)
								{
									item.Info.Amount -= (ushort)(invItem.StackMax - invItem.Info.Amount);
									invItem.Info.Amount = invItem.StackMax;

									client.Send(PacketCreator.ItemAmount(creature, invItem));

									result = 1;
								}
							}
							else
							{
								invItem.Info.Amount += item.Info.Amount;
								item.Info.Amount = 0;

								WorldManager.Instance.RemoveItem(item);
								client.Send(PacketCreator.ItemAmount(creature, invItem));

								result = 1;
							}
						}
					}
				}

				if (item.Info.Amount > 0 || (item.Type == ItemType.Sac && item.StackType == BundleType.Sac))
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

			var response = new MabiPacket(Op.ItemPickUpR, creature.Id);
			response.PutByte(result);
			client.Send(response);
		}

		private void HandleItemSplit(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var itemId = packet.GetLong();
			var amount = packet.GetShort();
			var unk1 = packet.GetByte();

			packet = new MabiPacket(Op.ItemSplitR, creature.Id);

			var item = creature.GetItem(itemId);
			if (item != null && item.StackType != BundleType.None)
			{
				if (item.Info.Amount < amount)
					amount = item.Info.Amount;

				item.Info.Amount -= amount;

				MabiItem splitItem;
				if (item.StackItem == 0)
					splitItem = new MabiItem(item);
				else
					splitItem = new MabiItem(item.StackItem);
				splitItem.Info.Amount = amount;
				splitItem.Move(Pocket.Cursor, 0, 0);
				creature.Items.Add(splitItem);

				// New item on cursor
				client.Send(PacketCreator.ItemInfo(creature, splitItem));

				// Update amount or remove
				if (item.Info.Amount > 0 || item.StackItem != 0)
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

			var response = new MabiPacket(Op.SwitchSetR, creature.Id);
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
					var item = creature.GetItemInPocket((Pocket)target);
					if (item != null)
					{
						item.Info.FigureA = (byte)(item.Info.FigureA == 1 ? 0 : 1);
						WorldManager.Instance.CreatureEquip(creature, item);
					}
				}
			}

			MabiPacket response = new MabiPacket(Op.ItemStateChangeR, creature.Id);
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

			p = new MabiPacket(Op.LoginWUnlock, creature.Id);
			p.PutInt(0xEFFFFFFE);
			client.Send(p);

			p = new MabiPacket(Op.EnterRegionR, 0x1000000000000001);
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

				p = new MabiPacket(Op.WarpRegion, creature.Id);
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
					WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(11).PutString("healing"), SendTargets.Range, creature);
					break;

				case SkillConst.Windmill:
					WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(11).PutString(""), SendTargets.Range, creature);
					break;

				case SkillConst.HiddenResurrection:
					client.Send(new MabiPacket(Op.SkillPrepared, creature.Id).PutShort(skillId).PutString(parameters));
					return;

				default:
					WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(11).PutString("flashing"), SendTargets.Range, creature);
					break;
			}

			var r = new MabiPacket(Op.SkillPrepare, creature.Id);
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

			client.Send(new MabiPacket(Op.SkillStackSet, creature.Id).PutBytes(creature.ActiveSkillStacks, creature.ActiveSkillStacks).PutShort(creature.ActiveSkillId));

			switch ((SkillConst)skillId)
			{
				case SkillConst.Healing:
					WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(13).PutString("healing_stack").PutBytes(creature.ActiveSkillStacks, 0), SendTargets.Range, creature);
					WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(12).PutString("healing"), SendTargets.Range, creature);
					break;

				case SkillConst.FinalHit:
					WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(69).PutBytes(1, 1), SendTargets.Range, creature);
					break;
			}

			var r = new MabiPacket(Op.SkillPrepared, creature.Id);
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
					WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(14).PutString("healing").PutLong(targetId), SendTargets.Range, creature);
					WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(13).PutString("healing_stack").PutBytes(creature.ActiveSkillStacks, 0), SendTargets.Range, creature);
					client.Send(new MabiPacket(Op.SkillStackUpdate, creature.Id).PutBytes(creature.ActiveSkillStacks, 1, 0).PutShort(skillId));
					break;

				case SkillConst.Windmill:
					WorldManager.Instance.CreatureUseMotion(creature, 8, 4);

					if (MabiCombat.Attack(creature, null) != AttackResult.Okay)
					{
						client.Send(PacketCreator.Notice(creature, "Unable to use when there is no target.", NoticeType.MIDDLE));
						client.Send(new MabiPacket(Op.SkillUnkown1, creature.Id));
						return;
					}
					goto default;

				case SkillConst.HiddenResurrection:
					creature.ActiveSkillTarget = WorldManager.Instance.GetCreatureById(targetId);
					if (creature.ActiveSkillTarget == null)
						return;

					WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(14).PutString("healing_phoenix").PutLong(targetId), SendTargets.Range, creature);
					break;

				default:
					client.Send(new MabiPacket(Op.SkillStackUpdate, creature.Id).PutBytes(--creature.ActiveSkillStacks, 1, 0).PutShort(skillId));
					break;
			}

			var r = new MabiPacket(Op.SkillUse, creature.Id);
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
					if (creature.ActiveSkillItem == null || creature.ActiveSkillItem.Info.Amount < 1)
						return;

					if (creature.ActiveSkillTarget.IsDead())
					{
						creature.ActiveSkillItem.Info.Amount--;
						creature.Client.Send(PacketCreator.ItemAmount(creature, creature.ActiveSkillItem));

						WorldManager.Instance.CreatureRevive(creature.ActiveSkillTarget);

						client.Send(new MabiPacket(Op.SkillUsed, creature.Id).PutShort(skillId).PutLong(creature.ActiveSkillTarget.Id).PutInts(0, 1));
					}

					creature.ActiveSkillId = 0;
					creature.ActiveSkillTarget = null;
					return;
			}

			var r = new MabiPacket(Op.SkillUsed, creature.Id);
			r.PutShort(skillId);
			client.Send(r);

			if (creature.ActiveSkillStacks > 0 && skill.RankInfo.Stack > 1)
			{
				client.Send(new MabiPacket(Op.SkillPrepared, creature.Id).PutShort(creature.ActiveSkillId));
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
					WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(121), SendTargets.Range, creature);
					break;

				case SkillConst.Rest:
					creature.Status |= CreatureStates.SitDown;
					WorldManager.Instance.CreatureSitDown(creature);
					break;
			}

			var r = new MabiPacket(Op.SkillStart, creature.Id);
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
			//var parameters = packet.GetByte();

			switch ((SkillConst)skillId)
			{
				case SkillConst.Rest:
					creature.Status &= ~CreatureStates.SitDown;
					WorldManager.Instance.CreatureStandUp(creature);
					break;
			}

			var r = new MabiPacket(Op.SkillStop, creature.Id);
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
			client.Send(new MabiPacket(Op.ChangeStanceR, creature.Id));
		}

		private void HandleShopBuyItem(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			if (!client.NPCSession.IsValid())
				return;

			var itemId = packet.GetLong();
			var targetPocket = packet.GetByte(); // 0:cursor, 1:inv
			var unk2 = packet.GetByte(); // storage gold?

			var newItem = client.NPCSession.Target.Script.Shop.GetItem(itemId);
			if (newItem == null)
				return;

			if (targetPocket == 0)
			{
				newItem.Move(Pocket.Cursor, 0, 0);
			}
			else if (targetPocket == 1)
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

			var p = new MabiPacket(Op.ShopBuyItemR, creature.Id);
			if (creature.HasGold(newItem.OptionInfo.Price) && newItem != null)
			{
				creature.RemoveGold(newItem.OptionInfo.Price);

				creature.Items.Add(newItem);
				client.Send(PacketCreator.ItemInfo(creature, newItem));
			}
			else
			{
				client.Send(PacketCreator.MsgBox(creature, "Insufficient amount of gold."));

				p.PutByte(0);
			}
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

			var p = new MabiPacket(Op.ShopSellItemR, creature.Id);

			creature.Items.Remove(item);
			client.Send(PacketCreator.ItemRemove(creature, item));

			var sellingPrice = item.OptionInfo.SellingPrice;

			if (item.StackType == BundleType.Sac)
			{
				var stackItem = MabiData.ItemDb.Find(item.StackItem);
				if (stackItem != null)
				{
					sellingPrice += stackItem.SellingPrice * item.Info.Amount;
				}
			}
			else if (item.StackType == BundleType.Stackable)
			{
				sellingPrice *= item.Info.Amount;
			}

			creature.GiveGold(sellingPrice);

			// TODO: There could be an optional tab for rebuying sold things.

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

			var answer = new MabiPacket(Op.ChangeTitleR, creature.Id);
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
				p = new MabiPacket(Op.PetSummonR, creature.Id);
				p.PutByte(0);
				p.PutLong(petId);
				client.Send(p);
				return;
			}

			// Set pet position near the summoner
			var pos = creature.GetPosition();
			var rand = RandomProvider.Get();
			pos.X = (uint)(pos.X + rand.Next(-400, 401));
			pos.Y = (uint)(pos.Y + rand.Next(-400, 401));
			pet.SetLocation(creature.Region, pos.X, pos.Y);
			pet.Direction = (byte)rand.Next(255);

			pet.Owner = creature;
			client.Creatures.Add(pet);

			pet.Save = true;

			p = new MabiPacket(Op.PetRegister, creature.Id);
			p.PutLong(pet.Id);
			p.PutByte(2);
			client.Send(p);

			p = new MabiPacket(Op.PetSummonR, creature.Id);
			p.PutByte(1);
			p.PutLong(petId);
			client.Send(p);

			p = new MabiPacket(Op.LoginWLock, petId);
			p.PutInt(0xEFFFFFFE);
			p.PutInt(0);
			client.Send(p);

			client.Send(PacketCreator.EnterRegionPermission(pet));

			WorldManager.Instance.Effect(creature, 29, creature.Region, pos.X, pos.Y);
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
				p = new MabiPacket(Op.PetUnsummonR, creature.Id);
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
			p = new MabiPacket(Op.PetUnRegister, creature.Id);
			p.PutLong(pet.Id);
			WorldManager.Instance.Broadcast(p, SendTargets.Range, creature);

			// Disappear
			p = new MabiPacket(Op.Disappear, 0x1000000000000001);
			p.PutLong(pet.Id);
			WorldManager.Instance.Broadcast(p, SendTargets.Range, creature);

			// Result
			p = new MabiPacket(Op.PetUnsummonR, creature.Id);
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
			if (pet == null || pet.IsDead() || pet.RaceInfo.VehicleType == 0 || pet.RaceInfo.VehicleType == 17)
			{
				p = new MabiPacket(Op.PetMountR, creature.Id);
				p.PutByte(0);
				client.Send(p);
				return;
			}

			creature.Vehicle = pet;

			WorldManager.Instance.VehicleBind(creature, pet);

			p = new MabiPacket(Op.PetMountR, creature.Id);
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
				p = new MabiPacket(Op.PetUnmountR, creature.Id);
				p.PutByte(0);
				client.Send(p);
				return;
			}

			WorldManager.Instance.VehicleUnbind(creature, creature.Vehicle);

			creature.Vehicle = null;

			p = new MabiPacket(Op.PetUnmountR, creature.Id);
			p.PutByte(1);
			client.Send(p);
		}

		private void HandleTouchProp(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			byte success = 0;

			// TODO: If all portals are props, and not all props are portals, what's this?

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
				Logger.Warning("Unknown prop: " + portalId.ToString());
			}

			var p = new MabiPacket(Op.TouchPropR, creature.Id);
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

			var answer = new MabiPacket(Op.CombatAttackR, creature.Id);

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

			client.Send(new MabiPacket(Op.StunMeter, creature.Id).PutLong(target.Id).PutByte(1).PutFloat(target.Stun));
		}

		public void HandleDeadMenu(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null && creature.IsDead())
				return;

			var response = new MabiPacket(Op.DeadMenuR, creature.Id);
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
				var feather = new MabiPacket(Op.DeadFeather, creature.Id);
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

			var response = new MabiPacket(Op.Revived, creature.Id);
			response.PutInt(1);
			response.PutInt(region);
			response.PutInt(x);
			response.PutInt(y);
			client.Send(response);
		}

		public void HandleMailsRequest(WorldClient client, MabiPacket packet)
		{
			client.Send(new MabiPacket(Op.MailsRequestR, client.Character.Id));

			// Mails
			// client.Send(new MabiPacket(0x7255, client.Character.Id).PutInt(3));
		}

		public void HandleIamWatchingYou(WorldClient client, MabiPacket packet)
		{
			// TODO : Send entities?
			// NOTE: Don't use funny names, no idea what this does anymore...
		}

		public void HandleSosButton(WorldClient client, MabiPacket packet)
		{
			// Enable = 1, Disable = 0
			client.Send(new MabiPacket(Op.SosButtonR, client.Character.Id).PutByte(false));
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

			var response = new MabiPacket(Op.StunMeter, creature.Id);
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

		public void HandleStunMeterDummy(WorldClient client, MabiPacket packet)
		{
			// Something about the stun meter I guess.
		}

		public void HandleHomesteadInfo(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null || creature.IsDead())
				return;

			client.Send(new MabiPacket(Op.HomesteadInfoRequestR, creature.Id).PutBytes(0, 0, 1));

			// Seems to be only called on login, good place for the MOTD.
			if (WorldConf.Motd != string.Empty)
				client.Send(PacketCreator.ServerMessage(client.Character, WorldConf.Motd));
		}

		public void HandleHitProp(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null || creature.IsDead())
				return;

			var targetId = packet.GetLong();

			// TODO: Check for prop to exist?
			// TODO: Drops. Hard, we'll have to create a prop database...

			var pos = creature.GetPosition();
			WorldManager.Instance.Broadcast(new MabiPacket(Op.HittingProp, creature.Id).PutLong(targetId).PutInt(2000).PutFloat(pos.X).PutFloat(pos.Y), SendTargets.Region, creature);

			client.Send(new MabiPacket(Op.HitPropR, creature.Id).PutByte(1));
		}

		public void HandleMoonGateRequest(WorldClient client, MabiPacket packet)
		{
			//001 [................]  String : _moongate_tara_west
			//002 [................]  String : _moongate_tirchonaill

			// Send no gates for now.
			client.Send(new MabiPacket(Op.MoonGateRequestR, 0x3000000000000000));
		}
	}
}
