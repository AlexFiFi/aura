// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Linq;
using Aura.Data;
using Aura.Shared.Const;
using Aura.Shared.Database;
using Aura.Shared.Network;
using Aura.Shared.Util;
using Aura.World.Database;
using Aura.World.Events;
using Aura.World.Player;
using Aura.World.Scripting;
using Aura.World.Skills;
using Aura.World.Util;
using Aura.World.World;

namespace Aura.World.Network
{
	public partial class WorldServer : BaseServer<WorldClient>
	{
		protected override void OnServerStartUp()
		{
			this.RegisterPacketHandler(Op.LoginW, HandleLogin);
			this.RegisterPacketHandler(Op.DisconnectW, HandleDisconnect);
			this.RegisterPacketHandler(Op.CharInfoRequestW, HandleCharacterInfoRequest);

			this.RegisterPacketHandler(Op.Walk, HandleMove);
			this.RegisterPacketHandler(Op.Run, HandleMove);
			this.RegisterPacketHandler(Op.Chat, HandleChat);
			this.RegisterPacketHandler(Op.TakeOff, HandleTakeOff);
			this.RegisterPacketHandler(Op.FlyTo, HandleFlyTo);
			this.RegisterPacketHandler(Op.Land, HandleLand);
			this.RegisterPacketHandler(Op.WhisperChat, HandleWhisperChat);
			this.RegisterPacketHandler(Op.VisualChat, HandleVisualChat);

			this.RegisterPacketHandler(Op.ItemMove, HandleItemMove);
			this.RegisterPacketHandler(Op.ItemPickUp, HandleItemPickUp);
			this.RegisterPacketHandler(Op.ItemDrop, HandleItemDrop);
			this.RegisterPacketHandler(Op.ItemDestroy, HandleItemDestroy);
			this.RegisterPacketHandler(Op.ItemSplit, HandleItemSplit);
			this.RegisterPacketHandler(Op.SwitchSet, HandleSwitchSet);
			this.RegisterPacketHandler(Op.ItemStateChange, HandleItemStateChange);
			this.RegisterPacketHandler(Op.ItemUse, HandleItemUse);
			this.RegisterPacketHandler(Op.ViewEquipment, HandleViewEquipment);
			this.RegisterPacketHandler(Op.UmbrellaJump, HandleUmbrellaJump);
			this.RegisterPacketHandler(Op.UmbrellaLand, HandleUmbrellaLand);
			this.RegisterPacketHandler(Op.DyePaletteReq, HandleDyePaletteReq);
			this.RegisterPacketHandler(Op.DyePickColor, HandleDyePickColor);

			this.RegisterPacketHandler(Op.QuestComplete, HandleQuestComplete);
			this.RegisterPacketHandler(Op.QuestGiveUp, HandleQuestGiveUp);

			this.RegisterPacketHandler(Op.NPCTalkStart, HandleNPCTalkStart);
			this.RegisterPacketHandler(Op.NPCTalkEnd, HandleNPCTalkEnd);
			this.RegisterPacketHandler(Op.NPCTalkPartner, HandleNPCTalkPartner);
			this.RegisterPacketHandler(Op.NPCTalkKeyword, HandleNPCTalkKeyword);
			this.RegisterPacketHandler(Op.NPCTalkSelect, HandleNPCTalkSelect);

			this.RegisterPacketHandler(Op.LeaveSoulStream, HandleLeaveSoulStream);

			this.RegisterPacketHandler(Op.CancelBeautyShop, HandleCancelBeautyShop);

			this.RegisterPacketHandler(Op.CutsceneFinished, HandleCutsceneFinished);

			this.RegisterPacketHandler(Op.PartyCreate, HandlePartyCreate);
			this.RegisterPacketHandler(Op.PartyJoin, HandlePartyJoin);
			this.RegisterPacketHandler(Op.PartyLeave, HandlePartyLeave);
			this.RegisterPacketHandler(Op.PartyRemove, HandlePartyRemove);
			this.RegisterPacketHandler(Op.PartyChangeSetting, HandlePartyChangeSettings);
			this.RegisterPacketHandler(Op.PartyChangePassword, HandlePartyChangePassword);
			this.RegisterPacketHandler(Op.PartyChangeLeader, HandlePartyChangeLeader);
			this.RegisterPacketHandler(Op.PartyWantedShow, HandlePartyWantedShow);
			this.RegisterPacketHandler(Op.PartyWantedHide, HandlePartyWantedHide);
			this.RegisterPacketHandler(Op.PartyChangeFinish, HandlePartyChangeFinish);
			this.RegisterPacketHandler(Op.PartyChangeExp, HandlePartyChangeExp);

			this.RegisterPacketHandler(Op.ShopBuyItem, HandleShopBuyItem);
			this.RegisterPacketHandler(Op.ShopSellItem, HandleShopSellItem);

			this.RegisterPacketHandler(Op.GetMails, HandleGetMails);
			this.RegisterPacketHandler(Op.ConfirmMailRecipent, HandleConfirmMailRecipient);
			this.RegisterPacketHandler(Op.SendMail, HandleSendMail);
			this.RegisterPacketHandler(Op.MarkMailRead, HandleMarkMailRead);
			this.RegisterPacketHandler(Op.ReturnMail, HandleReturnMail);
			this.RegisterPacketHandler(Op.RecallMail, HandleRecallMail);
			this.RegisterPacketHandler(Op.RecieveMailItem, HandleRecieveMailItem);
			this.RegisterPacketHandler(Op.DeleteMail, HandleDeleteMail);

			this.RegisterPacketHandler(Op.ChangeStance, HandleChangeStance);
			this.RegisterPacketHandler(Op.CombatSetTarget, HandleCombatSetTarget);
			this.RegisterPacketHandler(Op.CombatAttack, HandleCombatAttack);
			this.RegisterPacketHandler(Op.StunMeter, HandleStunMeterDummy);
			this.RegisterPacketHandler(Op.SubsribeStun, HandleStunMeterRequest); // subscription ?
			this.RegisterPacketHandler(Op.Revive, HandleRevive);
			this.RegisterPacketHandler(Op.DeadMenu, HandleDeadMenu);

			this.RegisterPacketHandler(Op.SkillPrepare, HandleSkillPrepare);
			this.RegisterPacketHandler(Op.SkillReady, HandleSkillReady);
			this.RegisterPacketHandler(Op.SkillUse, HandleSkillUse);
			this.RegisterPacketHandler(Op.SkillComplete, HandleSkillComplete);
			this.RegisterPacketHandler(Op.SkillCancel, HandleSkillCancel);
			this.RegisterPacketHandler(Op.SkillStart, HandleSkillStart);
			this.RegisterPacketHandler(Op.SkillStop, HandleSkillStop);
			this.RegisterPacketHandler(Op.SkillAdvance, HandleSkillAdvance);

			this.RegisterPacketHandler(Op.CombatSetAim, HandleCombatSetAim);

			this.RegisterPacketHandler(Op.PetSummon, HandlePetSummon);
			this.RegisterPacketHandler(Op.PetUnsummon, HandlePetUnsummon);
			this.RegisterPacketHandler(Op.PetMount, HandlePetMount);
			this.RegisterPacketHandler(Op.PetUnmount, HandlePetUnmount);

			this.RegisterPacketHandler(Op.TouchProp, HandleTouchProp);
			this.RegisterPacketHandler(Op.HitProp, HandleHitProp);

			this.RegisterPacketHandler(Op.EnterRegion, HandleEnterRegion);
			this.RegisterPacketHandler(Op.AreaChange, HandleAreaChange);

			this.RegisterPacketHandler(Op.ChangeTitle, HandleTitleChange);
			this.RegisterPacketHandler(Op.TalentTitleChange, HandleTalentTitleChange);
			this.RegisterPacketHandler(Op.MailsRequest, HandleMailsRequest);
			this.RegisterPacketHandler(Op.SosButton, HandleSosButton);
			this.RegisterPacketHandler(Op.MoonGateRequest, HandleMoonGateRequest);
			this.RegisterPacketHandler(Op.UseGesture, HandleGesture);
			this.RegisterPacketHandler(Op.HomesteadInfoRequest, HandleHomesteadInfo);
			this.RegisterPacketHandler(Op.OpenItemShop, HandleOpenItemShop);

			this.RegisterPacketHandler(Op.ConvertGp, HandleConvertGp);
			this.RegisterPacketHandler(Op.ConvertGpConfirm, HandleConvertGpConfirm);
			this.RegisterPacketHandler(Op.GuildDonate, HandleGuildDonate);
			this.RegisterPacketHandler(Op.GuildApply, HandleGuildApply);

			this.RegisterPacketHandler(Op.CollectionRequest, HandleCollectionRequest);
			this.RegisterPacketHandler(Op.ShamalaTransformationUse, HandleShamalaTransformation);
			this.RegisterPacketHandler(Op.ShamalaTransformationEnd, HandleShamalaTransformationEnd);

			this.RegisterPacketHandler(Op.GMCPSummon, HandleGMCPSummon);
			this.RegisterPacketHandler(Op.GMCPMoveToChar, HandleGMCPMoveToChar);
			this.RegisterPacketHandler(Op.GMCPMove, HandleGMCPMove);
			this.RegisterPacketHandler(Op.GMCPRevive, HandleGMCPRevive);
			this.RegisterPacketHandler(Op.GMCPInvisibility, HandleGMCPInvisibility);
			this.RegisterPacketHandler(Op.GMCPExpel, HandleGMCPExpel);
			this.RegisterPacketHandler(Op.GMCPBan, HandleGMCPBan);
			this.RegisterPacketHandler(Op.GMCPClose, (c, p) => { }); // TODO: Maybe add this to a gm log.
			this.RegisterPacketHandler(Op.GMCPNPCList, HandleGMCPListNPCs);

			this.RegisterPacketHandler(Op.Internal.ServerIdentify, HandleServerIdentify);
			this.RegisterPacketHandler(Op.ChannelStatus, HandleChannelStatus);

			// Temp/Unknown
			// --------------------------------------------------------------

			this.RegisterPacketHandler(0xA43D, (client, packet) =>
			{
				client.Send(new MabiPacket(0xA43E, client.Character.Id).PutByte(1));

				// Requested/Sent on login?
				// Alternative structure: (Conti and Nao warps)
				// 001 [..............00]  Byte   : 0
				// 002 [000039BA86EA43C0]  Long   : 000039BA86EA43C0 // 2012-May-22 15:30:00
				// 003 [000039BA86FABE80]  Long   : 000039BA86FABE80 // 2012-May-22 15:48:00
			});

			this.RegisterPacketHandler(0x61A8, (client, packet) =>
			{
				// TODO: Send entities?
				// NOTE: No idea what this does anymore...
			});

			this.RegisterPacketHandler(0xA897, (client, packet) =>
			{
				// Sent while logging in.

				client.Send(new MabiPacket(0xA898, client.Character.Id));

				// Reply example:
				// 001 [00000000000A0F4D]  Long   : 00000000000A0F4D
				// 002 [000000000000000F]  Long   : 000000000000000F
				// 003 [........00000004]  Int    : 4
				// 004 [............0008]  Short  : 8
				// 005 [........00000001]  Int    : 1
				// 006 [........00014599]  Int    : 83353
				// 007 [........00000002]  Int    : 2
				// 008 [........0002C836]  Int    : 182326
				// 009 [........00000003]  Int    : 3
				// 010 [........000158C4]  Int    : 88260
				// 011 [........00000004]  Int    : 4
				// 012 [........0001BAA8]  Int    : 113320
				// 013 [........00000005]  Int    : 5
				// 014 [........0003B3E3]  Int    : 242659
				// 015 [........00000006]  Int    : 6
				// 016 [........00007FC0]  Int    : 32704
				// 017 [........00000007]  Int    : 7
				// 018 [........00008BBA]  Int    : 35770
				// 019 [........00000008]  Int    : 8
				// 020 [........0008D5FF]  Int    : 579071
			});

			this.RegisterPacketHandler(0xAAEC, (client, packet) =>
			{
				// Apperantly sent when switching weapon sets, but only if
				// there are items equipped on that set.
				// Also sent when pressing ESC?
			});
		}

#pragma warning disable 0162
		private void HandleLogin(WorldClient client, MabiPacket packet)
		{
			if (client.State != ClientState.LoggingIn)
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

				client.Account = WorldDb.Instance.GetAccount(userName);
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

			// Purpose not clear, doesn't seem necessary.
			//foreach (var skill in creature.Skills)
			//{
			//    client.Send(new MabiPacket(Op.SkillInfo, creature.Id).PutBin(skill.Value.Info));
			//    client.Send(new MabiPacket(0x699E, creature.Id).PutShort(skill.Key).PutByte(1));
			//}

			p = new MabiPacket(Op.LoginWR, Id.World);
			p.PutByte(1);
			p.PutLong(creature.Id);
			p.PutLong(MabiTime.Now.DateTime);
			p.PutInt(1);
			p.PutString("");
			client.Send(p);

			//EntityEvents.Instance.OnPlayerChangesRegion(creature);

			if (creature.Has(CreatureStates.EverEnterWorld))
			{
				p = new MabiPacket(Op.CharacterLock, creature.Id);
				p.PutInt(0xEFFFFFFE);
				p.PutInt(0);
				client.Send(p);

				client.SendEnterRegionPermission(creature);
			}
			else
			{
				//var pp = new MabiPacket(0x000065C2, 0x1000000000000001);
				//pp.PutByte(1);
				//pp.PutByte(0);
				//client.Send(pp);

				// Set location, so character can talk to Nao,
				// and doesn't appear in the world.
				creature.SetLocation(1000, 0, 0);

				// Update state, so we don't get here again automatically.
				creature.State |= CreatureStates.EverEnterWorld;

				if (WorldManager.Instance.GetCreatureById(Id.Nao) == null)
					Logger.Warning("Nao NPC not found.");

				// With this packet many buttons and stuff are disabled,
				// until you're really logged in.
				var charInfo = new MabiPacket(Op.SpecialLogin, Id.World);
				charInfo.PutByte(1);
				charInfo.PutInt(1000); // Region
				charInfo.PutInt(3200); // X
				charInfo.PutInt(3200); // Y
				charInfo.PutLong(Id.Nao);
				creature.AddPrivateToPacket(charInfo);
				client.Send(charInfo);
			}

			client.State = ClientState.LoggedIn;

			ServerEvents.Instance.OnPlayerLogsIn(creature as MabiPC);
		}
#pragma warning restore 0162

		private void HandleDisconnect(WorldClient client, MabiPacket packet)
		{
			// TODO: Some check or move the unsafe stuff!

			Logger.Info("'" + client.Account.Name + "' is closing the connection. Saving...");

			WorldDb.Instance.SaveAccount(client.Account);

			foreach (var pc in client.Creatures)
			{
				WorldManager.Instance.RemoveCreature(pc);
			}

			client.Creatures.Clear();
			client.Character = null;
			client.Account = null;

			var p = new MabiPacket(Op.DisconnectWR, Id.World);
			p.PutByte(0);
			client.Send(p);
		}

		private void HandleCharacterInfoRequest(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
			{
				client.Send(new MabiPacket(Op.CharInfoRequestWR, Id.World).PutByte(false));
				return;
			}

			// Spawn effect
			if (creature.Owner != null)
				WorldManager.Instance.Broadcast(PacketCreator.SpawnEffect(creature.Owner, SpawnEffect.Pet, creature.GetPosition()), SendTargets.Range, creature);

			// Infamous 5209, aka char info
			var p = new MabiPacket(Op.CharInfoRequestWR, Id.World);
			p.PutByte(1);
			(creature as MabiPC).AddPrivateToPacket(p);
			client.Send(p);

			if (creature.Owner != null)
			{
				if (creature.RaceInfo.VehicleType > 0)
				{
					WorldManager.Instance.VehicleUnbind(null, creature, true);
				}

				if (creature.IsDead)
				{
					WorldManager.Instance.Broadcast(new MabiPacket(Op.DeadFeather, creature.Id).PutShort(1).PutInt(10).PutByte(0), SendTargets.Range, creature);
				}
			}

			if (creature == client.Character)
			{
				if (creature.Guild != null)
					client.Send(new MabiPacket(Op.GuildstoneLocation, creature.Id).PutByte(1).PutInts(creature.Guild.Region, creature.Guild.X, creature.Guild.Y));

				client.Send(new MabiPacket(Op.UnreadMailCount, creature.Id).PutInt((uint)MabiMail.GetUnreadCount(creature)));

				if (WorldConf.EnableItemShop)
				{
					// Button will be disabled if we don't send this packet.
					client.Send(new MabiPacket(Op.ItemShopInfo, creature.Id).PutByte(0));
				}

				if (WorldConf.AutoSendGMCP && client.Account.Authority >= WorldConf.MinimumGMCP)
				{
					client.Send(new MabiPacket(Op.GMCPOpen, creature.Id));
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
				var args = message.Substring(1).Split(' ');
				args[0] = args[0].ToLower();

				if (CommandHandler.Instance.Handle(client, creature, args, message))
					return;
			}

			WorldManager.Instance.CreatureTalk(creature, message, type);
		}

		private void HandleWhisperChat(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var target = WorldManager.Instance.GetCharacterByName(packet.GetString());
			if (target == null)
			{
				client.Send(PacketCreator.SystemMessage(creature, Localization.Get("world.whisper_no_target"))); // The target character does not exist.
				return;
			}

			var msg = packet.GetString();
			client.Send(PacketCreator.Whisper(creature, creature.Name, msg));
			target.Client.Send(PacketCreator.Whisper(target, creature.Name, msg));
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

				// TODO: Temp fix, add information to motion list.
				var loop = false;
				if (info.Name == "listen_music")
					loop = true;

				WorldManager.Instance.CreatureUseMotion(creature, info.Category, info.Type, loop);
			}

			var p = new MabiPacket(Op.UseGestureR, creature.Id);
			p.PutByte(result);
			client.Send(p);
		}

		public void HandleItemUse(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null || creature.IsDead)
				return;

			var itemOId = packet.GetLong();

			// Check for item
			var item = creature.GetItem(itemOId);
			if (item == null)
			{
				client.Send(new MabiPacket(Op.UseItemR, creature.Id).PutByte(false));
				return;
			}

			// Check for use script
			var script = ScriptManager.Instance.GetItemScript(item.Info.Class);
			if (script == null)
			{
				Logger.Unimplemented("Missing script for item '{0}' ({1}).", item.DataInfo.Name, item.Info.Class);
				client.Send(new MabiPacket(Op.UseItemR, creature.Id).PutByte(false));
				return;
			}

			// Use item
			script.OnUse(creature, item);
			creature.DecItem(item);

			// Mandatory stat update
			WorldManager.Instance.CreatureStatsUpdate(creature);

			client.Send(new MabiPacket(Op.UseItemR, creature.Id).PutByte(true).PutInt(item.Info.Class));
		}

		public void HandleGMCPMove(WorldClient client, MabiPacket packet)
		{
			if (client.Account.Authority < WorldConf.MinimumGMCP || client.Account.Authority < WorldConf.MinimumGMCPMove)
			{
				client.Send(PacketCreator.SystemMessage(client.Character, Localization.Get("gm.gmcp_auth"))); // You're not authorized to use the GMCP.
				return;
			}

			var region = packet.GetInt();
			var x = packet.GetInt();
			var y = packet.GetInt();

			client.Warp(region, x, y);
		}

		private void HandleGMCPMoveToChar(WorldClient client, MabiPacket packet)
		{
			if (client.Account.Authority < WorldConf.MinimumGMCP || client.Account.Authority < WorldConf.MinimumGMCPCharWarp)
			{
				client.Send(PacketCreator.SystemMessage(client.Character, Localization.Get("gm.gmcp_auth"))); // You're not authorized to use the GMCP.
				return;
			}

			var targetName = packet.GetString();
			var target = WorldManager.Instance.GetCharacterByName(targetName, false);
			if (target == null)
			{
				client.Send(PacketCreator.MsgBoxFormat(client.Character, Localization.Get("gm.gmcp_nochar"), targetName)); // Character '{0}' couldn't be found.
				return;
			}

			var targetPos = target.GetPosition();
			client.Warp(target.Region, targetPos.X, targetPos.Y);
		}

		private void HandleGMCPRevive(WorldClient client, MabiPacket packet)
		{
			if (client.Account.Authority < WorldConf.MinimumGMCP || client.Account.Authority < WorldConf.MinimumGMCPRevive)
			{
				client.Send(PacketCreator.SystemMessage(client.Character, Localization.Get("gm.gmcp_auth"))); // You're not authorized to use the GMCP.
				return;
			}

			var creature = WorldManager.Instance.GetCreatureById(packet.Id);
			if (creature == null || !creature.IsDead)
				return;

			var pos = creature.GetPosition();
			var region = creature.Region;

			var response = new MabiPacket(Op.Revived, creature.Id);
			response.PutInt(1);
			response.PutInt(region);
			response.PutInt(pos.X);
			response.PutInt(pos.Y);
			client.Send(response);

			WorldManager.Instance.ReviveCreature(creature);

			creature.FullHeal();
		}

		private void HandleGMCPSummon(WorldClient client, MabiPacket packet)
		{
			if (client.Account.Authority < WorldConf.MinimumGMCP || client.Account.Authority < WorldConf.MinimumGMCPSummon)
			{
				client.Send(PacketCreator.SystemMessage(client.Character, Localization.Get("gm.gmcp_auth"))); // You're not authorized to use the GMCP.
				return;
			}

			var targetName = packet.GetString();
			var target = WorldManager.Instance.GetCharacterByName(targetName) as MabiPC;
			if (target == null)
			{
				client.Send(PacketCreator.MsgBoxFormat(client.Character, Localization.Get("gm.gmcp_nochar"), targetName)); // Character '{0}' couldn't be found.
				return;
			}

			var targetClient = (target.Client as WorldClient);
			var pos = client.Character.GetPosition();

			targetClient.Send(PacketCreator.ServerMessage(target, Localization.Get("gm.gmcp_summon"), client.Character.Name)); // You've been summoned by '{0}'.
			targetClient.Warp(client.Character.Region, pos.X, pos.Y);
		}

		private void HandleGMCPListNPCs(WorldClient client, MabiPacket packet)
		{
			if (client.Account.Authority < WorldConf.MinimumGMCP)
			{
				client.Send(PacketCreator.SystemMessage(client.Character, Localization.Get("gm.gmcp_auth"))); // You're not authorized to use the GMCP.
				return;
			}

			client.Send(PacketCreator.SystemMessage(client.Character, Localization.Get("aura.unimplemented"))); // Unimplemented.
		}

		private void HandleGMCPInvisibility(WorldClient client, MabiPacket packet)
		{
			if (client.Account.Authority < WorldConf.MinimumGMCP || client.Account.Authority < WorldConf.MinimumGMCPInvisible)
			{
				client.Send(PacketCreator.SystemMessage(client.Character, Localization.Get("gm.gmcp_auth"))); // You're not authorized to use the GMCP.
				return;
			}

			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var toggle = packet.GetByte();
			creature.Conditions.A = (toggle == 1 ? (creature.Conditions.A | CreatureConditionA.Invisible) : (creature.Conditions.A & ~CreatureConditionA.Invisible));
			WorldManager.Instance.SendStatusEffectUpdate(creature);

			var p = new MabiPacket(Op.GMCPInvisibilityR, creature.Id);
			p.PutByte(1);
			client.Send(p);
		}

		private void HandleGMCPExpel(WorldClient client, MabiPacket packet)
		{
			if (client.Account.Authority < WorldConf.MinimumGMCP || client.Account.Authority < WorldConf.MinimumGMCPExpel)
			{
				client.Send(PacketCreator.SystemMessage(client.Character, Localization.Get("gm.gmcp_auth"))); // You're not authorized to use the GMCP.
				return;
			}

			var targetName = packet.GetString();
			var target = WorldManager.Instance.GetCharacterByName(targetName) as MabiPC;
			if (target == null)
			{
				client.Send(PacketCreator.MsgBoxFormat(client.Character, Localization.Get("gm.gmcp_nochar"), targetName)); // Character '{0}' couldn't be found.
				return;
			}

			client.Send(PacketCreator.MsgBoxFormat(client.Character, Localization.Get("gm.gmcp_kicked"), targetName)); // '{0}' has been kicked.

			// Better kill the connection, modders could bypass a dc request.
			target.Client.Kill();
		}

		private void HandleGMCPBan(WorldClient client, MabiPacket packet)
		{
			if (client.Account.Authority < WorldConf.MinimumGMCP || client.Account.Authority < WorldConf.MinimumGMCPBan)
			{
				client.Send(PacketCreator.SystemMessage(client.Character, Localization.Get("gm.gmcp_auth"))); // You're not authorized to use the GMCP.
				return;
			}

			var targetName = packet.GetString();
			var target = WorldManager.Instance.GetCharacterByName(targetName) as MabiPC;
			if (target == null)
			{
				client.Send(PacketCreator.MsgBoxFormat(client.Character, Localization.Get("gm.gmcp_nochar"), targetName)); // Character '{0}' couldn't be found.
				return;
			}

			var end = DateTime.Now.AddMinutes(packet.GetInt());
			(target.Client as WorldClient).Account.BannedExpiration = end;
			(target.Client as WorldClient).Account.BannedReason = packet.GetString();

			client.Send(PacketCreator.MsgBoxFormat(client.Character, Localization.Get("gm.gmcp_banned"), targetName, end)); // '{0}' has been banned till '{1}'.

			// Better kill the connection, modders could bypass a dc request.
			target.Client.Kill();
		}

		private void HandleNPCTalkStart(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var npcId = packet.GetLong();

			var target = WorldManager.Instance.GetCreatureById(npcId) as MabiNPC;
			if (target == null)
			{
				Logger.Warning("Unknown NPC: " + npcId.ToString("X"));
			}
			else if (target.Script == null)
			{
				Logger.Warning("Script for '" + target.Name + "' is null.");
				target = null;
			}
			else if (creature.Region != target.Region || !WorldManager.InRange(creature, target, 1000))
			{
				client.Send(PacketCreator.MsgBox(creature, Localization.Get("world.too_far"))); // You're too far away.
				target = null;
			}

			var p = new MabiPacket(Op.NPCTalkStartR, creature.Id);

			if (target == null)
			{
				p.PutByte(0);
				client.Send(p);
				return;
			}

			p.PutByte(1);
			p.PutLong(npcId);
			client.Send(p);

			client.NPCSession.Start(target);

			// Get enumerator and start first run.
			client.NPCSession.State = target.Script.OnTalk(client).GetEnumerator();
			if (client.NPCSession.State.MoveNext())
				client.NPCSession.Response = client.NPCSession.State.Current as Response;
		}

		private void HandleNPCTalkPartner(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var id = packet.GetLong();

			var target = client.Creatures.FirstOrDefault(a => a.Id == id);
			if (target == null)
			{
				Logger.Warning("Talk to unspawned partner: " + id.ToString());
			}

			var npc = WorldManager.Instance.GetCreatureByName("_partnerdummy") as MabiNPC;
			if (npc == null || npc.Script == null)
			{
				Logger.Warning("NPC or script of '_partnerdummy' is null.");
				npc = null;
			}

			var p = new MabiPacket(Op.NPCTalkPartnerR, creature.Id);

			if (target == null || npc == null)
			{
				p.PutByte(0);
				client.Send(p);
				return;
			}

			p.PutByte(1);
			p.PutLong(id);
			p.PutString(creature.Name + "'s " + target.Name);
			p.PutString(creature.Name + "'s " + target.Name);
			client.Send(p);

			client.NPCSession.Start(npc);

			//npc.Script.OnTalk(client);
			client.NPCSession.State = npc.Script.OnTalk(client).GetEnumerator();
			if (client.NPCSession.State.MoveNext())
				client.NPCSession.Response = client.NPCSession.State.Current as Response;
		}

		private void HandleNPCTalkEnd(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var npcId = packet.GetLong();
			var target = client.NPCSession.Target;

			//var p = new MabiPacket(Op.NPCTalkEndR, creature.Id);
			//p.PutByte(1);
			//p.PutLong(target.Id);
			//p.PutString("");
			//client.Send(p);

			if (target == null || target.Script == null)
			{
				Logger.Warning("Ending empty NPC session.");
				return;
			}

			target.Script.OnEnd(client);

			client.NPCSession.Clear();
		}

		/// <summary>
		/// Parameters:
		///		string  Keyword
		///	Description:
		///		Sent when selecting a keyword. Purpose unknown,
		///		NPCTalkSelect is sent as well.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="packet"></param>
		private void HandleNPCTalkKeyword(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var keyword = packet.GetString();

			if (client.NPCSession.IsValid)
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
			if (creature == null)
				return;

			if (!client.NPCSession.IsValid)
			{
				Logger.Warning("Invalid NPC session for '{0}', talking to '{1}'.", creature.Name, (client.NPCSession.Target != null ? client.NPCSession.Target.Script.ScriptName : "<unknown>"));
				return;
			}

			var response = packet.GetString();
			var sessionId = packet.GetInt();

			if (sessionId != client.NPCSession.Id)
			{
				Logger.Debug("No target or session id incorrect ({0}:{1})", sessionId, client.NPCSession.Id);
				return;
			}

			int pos = -1;
			if ((pos = response.IndexOf("<return type=\"string\">")) < 1)
			{
				Logger.Debug("No return value found.");
				return;
			}
			response = response.Substring(pos += 22, response.IndexOf('<', pos) - pos);

			if (response == "@end")
			{
				client.Send(new MabiPacket(Op.NPCTalkSelectEnd, creature.Id));

				client.NPCSession.Target.Script.OnEnd(client);
			}
			else if (response.StartsWith("@input"))
			{
				var splitted = response.Split(':');
				if (client.NPCSession.Response != null)
					client.NPCSession.Response.Value = splitted[1];
				if (client.NPCSession.State.MoveNext())
					client.NPCSession.Response = client.NPCSession.State.Current as Response;
			}
			else
			{
				if (client.NPCSession.Response != null)
					client.NPCSession.Response.Value = response;
				if (client.NPCSession.State.MoveNext())
					client.NPCSession.Response = client.NPCSession.State.Current as Response;
			}
		}

		private void HandleGetMails(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var p = new MabiPacket(Op.GetMailsR, creature.Id);

			var toReturn = new System.Collections.Generic.List<MabiMail>();

			foreach (var m in WorldDb.Instance.GetRecievedMail(creature.Id))
			{
				if (WorldConf.MailExpires > 0 && (DateTime.Today - m.Sent).Days > WorldConf.MailExpires)
					toReturn.Add(m);
				else
					m.AddEntityData(p, creature);
			}

			foreach (var m in WorldDb.Instance.GetSentMail(creature.Id))
			{
				if (WorldConf.MailExpires > 0 && (DateTime.Today - m.Sent).Days > WorldConf.MailExpires)
					toReturn.Add(m);
				else
					m.AddEntityData(p, creature);
			}

			foreach (var m in toReturn)
				m.Return("Mail is valid for " + WorldConf.MailExpires + " days, and the mail's valid period has expired. The mail has been returned to its sender.");

			p.PutLong(0);
			client.Send(p);
		}

		private void HandleConfirmMailRecipient(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			ulong recipId;

			if (WorldDb.Instance.IsValidMailRecpient(packet.GetString(), out recipId))
			{
				client.Send(new MabiPacket(Op.ConfirmMailRecipentR, creature.Id).PutByte(1).PutLong(recipId));
			}
			else
			{
				client.Send(new MabiPacket(Op.ConfirmMailRecipentR, creature.Id).PutByte(0));
			}
		}

		private void HandleSendMail(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;
			var mail = new MabiMail();
			mail.RecipientName = packet.GetString();

			if (!WorldDb.Instance.IsValidMailRecpient(mail.RecipientName, out mail.RecipientId))
			{
				client.Send(
					PacketCreator.MsgBox(creature, Localization.Get("world.mail_invalid")), // Invaild recipient
					new MabiPacket(Op.SendMailR, creature.Id).PutByte(0)
				);
				return;
			}

			mail.Text = packet.GetString();
			mail.ItemId = packet.GetLong();

			if (mail.ItemId == 0)
			{
				mail.Type = (byte)MailTypes.Normal;
			}
			else
			{
				mail.Type = (byte)MailTypes.Item;
				var item = creature.Items.Find(i => i.Id == mail.ItemId);

				if (item == null)
				{
					client.Send(PacketCreator.MsgBox(creature, Localization.Get("world.mail_item")), // You can't send an item you don't have!
						new MabiPacket(Op.SendMailR, creature.Id).PutByte(0));
					return;
				}
				else
				{
					client.Send(PacketCreator.ItemRemove(creature, item));
					creature.Items.Remove(item);
					WorldDb.Instance.SaveMailItem(item, null);
				}
			}
			mail.COD = packet.GetInt();

			mail.SenderName = creature.Name;
			mail.SenderId = creature.Id;

			mail.Save(true);

			var p = new MabiPacket(Op.SendMailR, creature.Id);
			p.PutByte(1);
			mail.AddEntityData(p, creature);

			client.Send(p);
		}

		private void HandleMarkMailRead(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var m = WorldDb.Instance.GetMail(packet.GetLong());

			var p = new MabiPacket(Op.MarkMailReadR, creature.Id);
			if (m != null)
			{
				p.PutByte(1);
				p.PutLong(m.MessageId);

				m.Read = 2;

				m.Save(false);
			}
			else
				p.PutByte(0);

			client.Send(p);
		}

		private void HandleReturnMail(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var m = WorldDb.Instance.GetMail(packet.GetLong());

			if (m != null)
			{
				m.Return(packet.GetString());

				client.Send(new MabiPacket(Op.ReturnMailR, creature.Id).PutByte(1).PutLong(m.MessageId));
			}
			else
			{
				client.Send(new MabiPacket(Op.ReturnMailR, creature.Id).PutByte(0));
			}
		}

		private void HandleRecallMail(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var m = WorldDb.Instance.GetMail(packet.GetLong());

			if (m != null && m.ItemId != 0)
			{
				var item = WorldDb.Instance.GetItem(m.ItemId);

				m.Delete();

				item.Info.Pocket = (byte)Pocket.Temporary; //Todo: Inv

				WorldDb.Instance.SaveMailItem(item, creature);

				creature.Items.Add(item);

				client.Send(PacketCreator.ItemInfo(creature, item));

				client.Send(new MabiPacket(Op.RecallMailR, creature.Id).PutByte(1).PutLong(m.MessageId));

			}
			else
			{
				client.Send(new MabiPacket(Op.RecallMailR, creature.Id).PutByte(0));
			}
		}

		private void HandleRecieveMailItem(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var m = WorldDb.Instance.GetMail(packet.GetLong());

			if (m != null && m.ItemId != 0)
			{

				//TODO: COD
				var item = WorldDb.Instance.GetItem(m.ItemId);

				m.Delete();

				item.Info.Pocket = (byte)Pocket.Temporary; //Todo: Inv

				WorldDb.Instance.SaveMailItem(item, creature);

				creature.Items.Add(item);

				client.Send(PacketCreator.ItemInfo(creature, item));

				client.Send(new MabiPacket(Op.RecieveMailItemR, creature.Id).PutByte(1).PutLong(m.MessageId));
			}
			else
			{
				client.Send(new MabiPacket(Op.RecieveMailItemR, creature.Id).PutByte(0));
			}
		}

		private void HandleDeleteMail(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var m = WorldDb.Instance.GetMail(packet.GetLong());

			if (m != null)
			{
				m.Delete();

				client.Send(new MabiPacket(Op.DeleteMailR, creature.Id).PutByte(1).PutLong(m.MessageId));
			}
			else
			{
				client.Send(new MabiPacket(Op.DeleteMailR, creature.Id).PutByte(0));
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
			if ((target >= Pocket.RightHand1 && target <= Pocket.Magazine2) || (source >= Pocket.RightHand1 && source <= Pocket.Magazine2))
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
			{
				WorldManager.Instance.CreatureEquip(creature, item);
				switch (item.Info.Class)
				{
					// Umbrella Skill
					case 41021:
					case 41022:
					case 41023:
					case 41025:
					case 41026:
					case 41027:
					case 41061:
					case 41062:
					case 41063:
						if (!creature.HasSkill(SkillConst.Umbrella))
							creature.GiveSkill(SkillConst.Umbrella, SkillRank.Novice);
						break;
				}
			}

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
			if (pocket == Pocket.RightHand1 || pocket == Pocket.RightHand2)
			{
				var secSource = pocket + 2; // RightHand1/2
				var secItem = creature.GetItemInPocket(secSource);
				if (secItem != null || (secItem = creature.GetItemInPocket(secSource = (Pocket)((byte)secSource + 2))) != null)
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

			creature.UpdateItemsFromPockets(pocket);

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

			if (HandleDungeonDrop(client, creature, item))
				return;

			// Drop it
			item.Id = MabiItem.NewItemId;
			WorldManager.Instance.CreatureDropItem(creature, new ItemEventArgs(item));
			EntityEvents.Instance.OnCreatureItemAction(creature, item.Info.Class);

			// Done
			var p = new MabiPacket(Op.ItemDropR, creature.Id);
			p.PutByte(1);
			client.Send(p);
		}

		/// <summary>
		/// Temp function to separate this stuff from HandleItemDrop.
		/// </summary>
		uint DGID1 = 10001;
		uint DGID2 = 10002;
		ulong INSTANCEID = Id.Instances;
		string DGDESIGN = "TirCho_Alby_Low_Dungeon";
		private bool HandleDungeonDrop(WorldClient client, MabiCreature creature, MabiItem item)
		{
			// TODO: Go through the list of dungeons (scriptable?), check the
			//   altars there, return true if dungeon drop, etc etc.

			if (creature.OnAltar != DungeonAltar.Alby)
				return false;

			//DGID1 += 2;
			//DGID2 += 2;
			//ITID2++;
			//INSTANCEID++;

			client.SendLock(creature);

			// Done
			var p = new MabiPacket(Op.ItemDropR, creature.Id);
			p.PutByte(1);
			client.Send(p);

			WorldManager.Instance.CreatureLeaveRegion(creature);
			creature.SetLocation(DGID1, 3262, 3139);

			// Dungeon info
			// --------------------------------------------------------------
			var dunp = new MabiPacket(Op.DungeonInfo, Id.Broadcast);

			dunp.PutLong(creature.Id);
			dunp.PutLong(INSTANCEID);            // Instance id?

			dunp.PutByte(1);
			dunp.PutString(DGDESIGN);            // Dungeon name (dungeondb.xml)

			dunp.PutInt(1234567890);
			dunp.PutInt(0987654321);
			dunp.PutInt(0);

			dunp.PutInt(2);                      // ? Count, Entry + Floors?
			dunp.PutInt(DGID1);                  // Imaginary entrance region id?
			dunp.PutInt(DGID2);                  // Imaginary dungeon region id?

			dunp.PutString("<option/>");

			dunp.PutInt(1);			             // Floor Count
			{
				dunp.PutInt(4);                  // Room Count
				{
					dunp.PutByte(0);
					dunp.PutByte(0);

					dunp.PutByte(0);
					dunp.PutByte(0);

					dunp.PutByte(0);
					dunp.PutByte(0);

					dunp.PutByte(0);
					dunp.PutByte(0);
				}
			}

			dunp.PutInt(0);
			dunp.PutInt(1);					     // Floor Count?
			{
				dunp.PutInt(543210987);
				dunp.PutInt(0);
			}

			client.Send(dunp);

			return true;
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
			EntityEvents.Instance.OnCreatureItemAction(creature, item.Info.Class);

			client.Send(PacketCreator.ItemRemove(creature, item));
			client.Send(new MabiPacket(Op.ItemDestroyR, creature.Id).PutByte(1));
		}

		// TODO: This code is kinda redundant... we should try to use
		//   MabiCreature.GiveItem somehow.
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
						client.Send(PacketCreator.SystemMessage(creature, Localization.Get("world.insufficient_space"))); // Not enough space.
					}
				}

				EntityEvents.Instance.OnCreatureItemAction(creature, item.Info.Class);
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

			// TODO: Check this, GetItemInPocket doesn't return the correct stuff.
			creature.WeaponSet = set;

			creature.UpdateItemsFromPockets(Pocket.RightHand1);
			creature.UpdateItemsFromPockets(Pocket.LeftHand1);
			creature.UpdateItemsFromPockets(Pocket.Magazine1);

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

			client.SendUnlock(creature);

			// Sent on log in, but not when switching regions?
			client.Send(new MabiPacket(Op.EnterRegionR, Id.World).PutByte(1).PutLongs(creature.Id).PutLong(MabiTime.Now.DateTime));
			WorldManager.Instance.AddCreature(creature);

			if (creature == client.Character)
			{
				var entities = WorldManager.Instance.GetEntitiesInRange(creature);

				if (entities.Count != 0)
				{
					client.Send(PacketCreator.EntitiesAppear(entities));
				}
			}

			var pos = creature.GetPosition();
			client.Send(new MabiPacket(Op.WarpRegion, creature.Id).PutByte(1).PutInts(creature.Region, pos.X, pos.Y));

			EntityEvents.Instance.OnPlayerChangesRegion(creature);

			// Send Conformation?
			client.Send(new MabiPacket(0xA925, Id.Broadcast).PutInts(creature.Region, 0));

			if (creature == client.Character)
			{
				foreach (var rider in creature.Riders.Where(c => c.Client != client))
					((WorldClient)rider.Client).Warp(creature.Region, pos.X, pos.Y);
			}

			if (creature.Pet != null)
			{
				creature.Pet.SetLocation(creature.Region, pos.X, pos.Y);
				client.SendEnterRegionPermission(creature.Pet);

				foreach (var rider in creature.Pet.Riders.Where(c => c.Client != client))
					((WorldClient)rider.Client).Warp(creature.Region, pos.X, pos.Y);
			}
		}

		/// <summary>
		/// Checks Stamina and Mana, sends fail response and stats update. No success response.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="packet"></param>
		private void HandleSkillPrepare(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var skillId = packet.GetShort();

			MabiSkill skill; SkillHandler handler;
			SkillManager.CheckOutSkill(creature, skillId, out skill, out handler);
			if (skill == null || handler == null)
			{
				client.SendSkillPrepareFail(creature);
				return;
			}

			// Check Mana
			if (creature.Mana < skill.RankInfo.ManaCost)
			{
				client.SendSystemMsg(creature, Localization.Get("skills.insufficient_mana")); // Insufficient Mana
				client.SendSkillPrepareFail(creature);
				return;
			}

			// Check Stamina
			if (creature.Stamina < skill.RankInfo.StaminaCost)
			{
				client.SendSystemMsg(creature, Localization.Get("skills.insufficient_stamina")); // Insufficient Stamina
				client.SendSkillPrepareFail(creature);
				return;
			}

			if (creature.ActiveSkillId != skill.Id && creature.ActiveSkillId != 0)
			{
				SkillManager.GetHandler(creature.ActiveSkillId).Cancel(creature, creature.GetSkill(creature.ActiveSkillId));
			}

			creature.ActiveSkillId = skill.Id;

			// Save cast time when preparation is finished.
			var castTime = WorldConf.DynamicCombat ? skill.RankInfo.NewLoadTime : skill.RankInfo.LoadTime;
			creature.ActiveSkillPrepareEnd = DateTime.Now.AddMilliseconds(castTime);

			var result = handler.Prepare(creature, skill, packet, castTime);

			if ((result & SkillResults.Failure) != 0)
			{
				client.SendSkillPrepareFail(creature);
				return;
			}

			if (skill.RankInfo.ManaCost > 0)
				creature.Mana -= skill.RankInfo.ManaCost;
			if (skill.RankInfo.StaminaCost > 0)
				creature.Stamina -= skill.RankInfo.StaminaCost;

			WorldManager.Instance.CreatureStatsUpdate(creature);

			// Not Okay or NoReply
			//if ((result & SkillResults.Okay) == 0 || (result & SkillResults.NoReply) != 0)
			//    return;

			//client.SendSkillPrepare(creature, skill.Id, castTime);
		}

		/// <summary>
		/// No success response.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="packet"></param>
		private void HandleSkillReady(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var skillId = packet.GetShort();
			//var parameters = packet.GetStringOrEmpty();

			MabiSkill skill; SkillHandler handler;
			SkillManager.CheckOutSkill(creature, skillId, out skill, out handler);
			if (skill == null || handler == null)
				return;

			var result = handler.Ready(creature, skill);

			if ((result & SkillResults.Okay) == 0)
				return;

			//client.SendSkillReady(creature, skill.Id, parameters);
		}

		/// <summary>
		/// Sends insufficient Stamina msg. No success response.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="packet"></param>
		private void HandleSkillUse(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var skillId = packet.GetShort();

			MabiSkill skill; SkillHandler handler;
			SkillManager.CheckOutSkill(creature, skillId, out skill, out handler);
			if (skill == null || handler == null)
				return;

			var result = handler.Use(creature, skill, packet);

			if ((result & SkillResults.InsufficientStamina) != 0)
				client.SendSystemMsg(creature, Localization.Get("skills.insufficient_stamina")); // Insufficient Stamina
		}

		/// <summary>
		/// No success response.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="packet"></param>
		private void HandleSkillComplete(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var skillId = packet.GetShort();

			MabiSkill skill; SkillHandler handler;
			SkillManager.CheckOutSkill(creature, skillId, out skill, out handler);
			if (skill == null || handler == null)
				return;

			var result = handler.Complete(creature, skill, packet);

			if (creature.ActiveSkillStacks < 1)
				creature.ActiveSkillId = SkillConst.None;

			//if ((result & SkillResults.Okay) == 0 || (result & SkillResults.NoReply) != 0)
			//{
			//    creature.ActiveSkillId = 0;
			//    return;
			//}

			//client.SendSkillComplete(creature, skill.Id);

			//if (creature.ActiveSkillStacks > 0 && skill.RankInfo.Stack > 1)
			//{
			//    // Send new ready packet if there are stacks left.
			//    client.SendSkillReady(creature, skill.Id);
			//}
			//else
			//{
			//    creature.ActiveSkillId = 0;
			//    creature.ActiveSkillTarget = null;
			//}
		}

		/// <summary>
		/// Properly cancels skill, calls skill's cancel handler,
		/// but doesn't require manual success.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="packet"></param>
		private void HandleSkillCancel(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			WorldManager.Instance.CreatureSkillCancel(creature);
		}

		/// <summary>
		/// Full handling.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="packet"></param>
		private void HandleSkillStart(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var skillId = packet.GetShort();
			var parameters = packet.GetStringOrEmpty();

			MabiSkill skill; SkillHandler handler;
			SkillManager.CheckOutSkill(creature, skillId, out skill, out handler);
			if (skill == null || handler == null)
			{
				client.Send(new MabiPacket(Op.SkillStart, creature.Id).PutShort(skillId));
				return;
			}

			var result = handler.Start(creature, skill);

			if ((result & SkillResults.Okay) == 0)
				return;

			var r = new MabiPacket(Op.SkillStart, creature.Id);
			r.PutShort(skillId);
			r.PutString("");
			client.Send(r);
		}

		/// <summary>
		/// Full handling.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="packet"></param>
		private void HandleSkillStop(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var skillId = packet.GetShort();
			var parameters = packet.GetStringOrEmpty();

			MabiSkill skill; SkillHandler handler;
			SkillManager.CheckOutSkill(creature, skillId, out skill, out handler);
			if (skill == null || handler == null)
				return;

			var result = handler.Stop(creature, skill);

			//if ((result & SkillResults.Okay) == 0)
			//    return;

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

			if (creature.Vehicle != null && creature == creature.Vehicle.Owner)
			{
				creature.Vehicle.BattleState = mode;
				WorldManager.Instance.CreatureChangeStance(creature.Vehicle);
			}
			if (creature.Owner != null && creature.Riders.Contains(creature.Owner))
			{
				creature.Owner.BattleState = mode;
				WorldManager.Instance.CreatureChangeStance(creature.Owner);
			}

			// Unlock
			client.Send(new MabiPacket(Op.ChangeStanceR, creature.Id));
		}

		private void HandleShopBuyItem(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			if (!client.NPCSession.IsValid)
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

			// The client expects the price for a full stack to be sent,
			// so we have to calculate the actual price here.
			var price = newItem.OptionInfo.Price;
			if (newItem.StackType == BundleType.Stackable)
				price = (uint)(price / newItem.StackMax * newItem.Count);

			var p = new MabiPacket(Op.ShopBuyItemR, creature.Id);
			if (creature.HasGold(price) && newItem != null)
			{
				creature.RemoveGold(price);

				creature.Items.Add(newItem);
				client.Send(PacketCreator.ItemInfo(creature, newItem));
			}
			else
			{
				client.Send(PacketCreator.MsgBox(creature, Localization.Get("world.shop_gold"))); // Insufficient amount of gold.

				p.PutByte(0);
			}
			client.Send(p);
		}

		private void HandleShopSellItem(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			if (!client.NPCSession.IsValid)
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
					sellingPrice += (stackItem.SellingPrice / stackItem.StackMax) * item.Info.Amount;
				}
			}
			else if (item.StackType == BundleType.Stackable)
			{
				sellingPrice = (uint)(sellingPrice / item.StackMax * item.Count);
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
			var optTitle = packet.GetShort();

			var answer = new MabiPacket(Op.ChangeTitleR, creature.Id);

			bool update = false;

			// Make sure the character has this title enabled
			var character = creature as MabiPC;
			if (title == 0 || (character.Titles.ContainsKey(title)) && character.Titles[title])
			{
				creature.Title = title;
				answer.PutByte(1);
				update = true;
			}
			else
			{
				answer.PutByte(0);
			}

			if (optTitle == 0 || (character.Titles.ContainsKey(optTitle)) && character.Titles[optTitle])
			{
				creature.OptionTitle = optTitle;
				answer.PutByte(1);
				update = true;
			}
			else
			{
				answer.PutByte(0);
			}

			if (update)
				WorldManager.Instance.CreatureChangeTitle(creature);

			client.Send(answer);
		}

		private void HandlePetSummon(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			// No pets in soul stream.
			// TODO: implement something like map flags?
			if (creature.Region == 1000)
			{
				client.Send(new MabiPacket(Op.PetSummonR, creature.Id).PutByte(false));
				return;
			}

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

			pet.IsFlying = false;

			// Set pet position near the summoner
			var pos = creature.GetPosition();
			var rand = RandomProvider.Get();
			pos.X = (uint)(pos.X + rand.Next(-350, 351));
			pos.Y = (uint)(pos.Y + rand.Next(-350, 351));
			pet.SetLocation(creature.Region, pos.X, pos.Y);
			pet.Direction = (byte)rand.Next(255);

			pet.Owner = creature;
			creature.Pet = pet;
			client.Creatures.Add(pet);

			pet.Save = true;

			// Doesn't fix giant mount problems.
			if (creature.IsGiant)
				pet.StateEx |= CreatureStatesEx.SummonedByGiant;

			p = new MabiPacket(Op.PetRegister, creature.Id);
			p.PutLong(pet.Id);
			p.PutByte(2);
			client.Send(p);

			p = new MabiPacket(Op.PetSummonR, creature.Id);
			p.PutByte(1);
			p.PutLong(petId);
			client.Send(p);

			p = new MabiPacket(Op.CharacterLock, petId);
			p.PutInt(0xEFFFFFFE);
			p.PutInt(0);
			client.Send(p);

			client.SendEnterRegionPermission(pet);
		}

		private void HandlePetUnsummon(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var petId = packet.GetLong();

			var pet = client.Creatures.FirstOrDefault(a => a.Id == petId);
			if (pet == null)
			{
				var p = new MabiPacket(Op.PetUnsummonR, creature.Id);
				p.PutByte(0);
				p.PutLong(petId);
				client.Send(p);
				return;
			}

			client.Creatures.Remove(pet);

			pet.StopMove();
			var pos = pet.GetPosition();

			WorldManager.Instance.Broadcast(PacketCreator.SpawnEffect(creature, SpawnEffect.PetDespawn, pos), SendTargets.Range, pet);
			WorldManager.Instance.RemoveCreature(pet);

			if (pet.Riders.Count > 0)
			{
				if (pet.IsFlying)
				{
					client.SendUnlock(pet, 0xFFFFBDFF);
					foreach (var rider in pet.Riders)
						client.SendUnlock(rider, 0xFFFFBDFF);
					pet.IsFlying = false;
				}
				foreach (var rider in pet.Riders)
				{
					rider.Vehicle = null;
					WorldManager.Instance.VehicleUnbind(rider, pet);
					rider.StopMove();
				}
				pet.Riders.Clear();
			}

			if (pet.Owner != null)
			{
				pet.Owner.Pet = null;
				pet.Owner = null;
			}

			// ?
			WorldManager.Instance.Broadcast(new MabiPacket(Op.PetUnRegister, creature.Id).PutLong(pet.Id), SendTargets.Range, creature);

			// Disappear
			client.Send(new MabiPacket(Op.Disappear, Id.World).PutLong(pet.Id));

			// Result
			client.Send(new MabiPacket(Op.PetUnsummonR, creature.Id).PutByte(true).PutLong(petId));
		}

		private void HandlePetMount(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var petId = packet.GetLong();

			var creatureIsSitting = creature.Has(CreatureStates.SitDown);

			var pet = client.Account.Pets.FirstOrDefault(a => a.Id == petId);
			if (pet == null || pet.IsDead || pet.RaceInfo.VehicleType == 0 || pet.RaceInfo.VehicleType == 17 || creatureIsSitting || !WorldManager.InRange(creature, pet, 200))
			{
				if (creatureIsSitting)
					client.Send(PacketCreator.Notice(Localization.Get("world.mount_sit"), NoticeType.MiddleTop)); // You cannot mount while resting.

				client.Send(new MabiPacket(Op.PetMountR, creature.Id).PutByte(false));
				return;
			}

			creature.Vehicle = pet;
			pet.Riders.Add(creature);

			WorldManager.Instance.VehicleBind(creature, pet);

			// Mount motion (horse)
			var p = new MabiPacket(Op.Motions, creature.Id);
			p.PutInt(21);
			p.PutInt(0);
			p.PutByte(0);
			p.PutShort(0);
			client.Send(p);

			client.Send(new MabiPacket(Op.PetMountR, creature.Id).PutByte(true));
		}

		private void HandlePetUnmount(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			if (creature.Vehicle == null || !creature.Vehicle.Riders.Contains(creature) || creature.Vehicle.IsFlying)
			{
				client.Send(new MabiPacket(Op.PetUnmountR, creature.Id).PutByte(false));
				return;
			}

			WorldManager.Instance.VehicleUnbind(creature, creature.Vehicle);

			// Unmount motion (horse)
			var p2 = new MabiPacket(Op.Motions, creature.Id);
			p2.PutInt(21);
			p2.PutInt(1);
			p2.PutByte(0);
			p2.PutShort(0);
			client.Send(p2);

			var pos = creature.Vehicle.GetPosition();

			creature.SetPosition(pos.X, pos.Y);

			creature.Vehicle.Riders.Remove(creature);
			creature.Vehicle = null;

			client.Send(new MabiPacket(Op.PetUnmountR, creature.Id).PutByte(true));
		}

		private void HandleTouchProp(WorldClient client, MabiPacket packet)
		{
			var character = client.Creatures.FirstOrDefault(a => a.Id == packet.Id) as MabiPC;
			if (character == null)
				return;

			byte success = 0;

			var propId = packet.GetLong();
			var pb = WorldManager.Instance.GetPropBehavior(propId);
			if (pb != null)
			{
				if (character.Region == pb.Prop.Region && WorldManager.InRange(character, (uint)pb.Prop.Info.X, (uint)pb.Prop.Info.Y, 1500))
				{
					success = 1;
					pb.Func(client, character, pb.Prop);
				}
			}
			else
			{
				// Dungeon test stuff
				if (character.Region == DGID1)
				{
					var pos = character.GetPosition();
					//client.Send(new MabiPacket(Op.WARP_ENTER, creature.Id).PutByte(1).PutInts(creature.Region, pos.X, pos.Y));
					//
					//
					WorldManager.Instance.CreatureLeaveRegion(character);
					client.Send(new MabiPacket(Op.CharacterLock, character.Id).PutInts(0xEFFFFFFE, 0));

					character.SetLocation(DGID2, 5992, 5614);
					client.SendEnterRegionPermission(character);

					success = 1;
				}
				else
				{
					Logger.Unimplemented("Unknown prop (touch): " + propId.ToString());
				}
			}

			var p = new MabiPacket(Op.TouchPropR, character.Id);
			p.PutByte(success);
			client.Send(p);
		}

		public void HandleHitProp(WorldClient client, MabiPacket packet)
		{
			var character = client.Creatures.FirstOrDefault(a => a.Id == packet.Id) as MabiPC;
			if (character == null || character.IsDead)
				return;

			var propId = packet.GetLong();

			// Hit prop animation
			var pos = character.GetPosition();
			WorldManager.Instance.Broadcast(new MabiPacket(Op.HittingProp, character.Id).PutLong(propId).PutInt(2000).PutFloat(pos.X).PutFloat(pos.Y), SendTargets.Region, character);

			// Check for behavior and run it.
			var pb = WorldManager.Instance.GetPropBehavior(propId);
			if (pb != null)
			{
				if (character.Region == pb.Prop.Region && WorldManager.InRange(character, (uint)pb.Prop.Info.X, (uint)pb.Prop.Info.Y, 1500))
				{
					pb.Func(client, character, pb.Prop);
				}
			}
			else
			{
				Logger.Unimplemented("Unknown prop (hit): " + propId.ToString());
			}

			// Send success in any case, just like hit ani.
			client.Send(new MabiPacket(Op.HitPropR, character.Id).PutByte(1));
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

#if false
			// Collision
			MabiVertex intersection;
			if (WorldManager.Instance.FindCollisionInTree(creature.Region, pos, dest, out intersection))
			{
				//Logger.Debug("intersection " + intersection);
				// TODO: Uhm... do something.
			}
#endif

			var walking = (packet.Op == Op.Walk);

			// TODO: Update creature position on unmount?
			creature.StartMove(dest, walking);
			if (creature.Vehicle != null)
			{
				creature.Vehicle.StartMove(dest, walking);
				WorldManager.Instance.CreatureMove(creature.Vehicle, creature.GetPosition(), dest, walking);
			}

			WorldManager.Instance.CreatureMove(creature, pos, dest, walking);
		}

		private void HandleTakeOff(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			//Todo: Check if can fly? or use that --v

			if (creature.RaceInfo.FlightInfo == null)
			{
				Logger.Unimplemented("Missing flight info for race '{0}'.", creature.Race);
				client.Send(new MabiPacket(Op.TakeOffR, packet.Id).PutByte(false));
				return;
			}

			if (creature.IsFlying)
			{
				client.Send(new MabiPacket(Op.TakeOffR, packet.Id).PutByte(false));
				return;
			}

			var ascentTime = packet.GetFloat();

			client.SendLock(creature, 0xFFFFBDDF);
			foreach (var rider in creature.Riders)
				client.SendLock(rider, 0xFFFFBDDF);

			var pos = creature.GetPosition();
			creature.SetPosition(pos.X, pos.Y, 10000);
			creature.IsFlying = true;

			WorldManager.Instance.Broadcast(new MabiPacket(Op.TakingOff, packet.Id).PutFloat(ascentTime), SendTargets.Range, creature);
			client.Send(new MabiPacket(Op.TakeOffR, packet.Id).PutByte(true));
		}

		private void HandleFlyTo(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			if (!creature.IsFlying)
				return;

			var toX = packet.GetFloat();
			var toH = packet.GetFloat();
			var toY = packet.GetFloat();
			var dir = packet.GetFloat();

			var pos = creature.GetPosition();

			WorldManager.Instance.Broadcast(new MabiPacket(Op.FlyingTo, packet.Id).PutFloats(toX, toH, toY, dir, pos.X, pos.H, pos.Y), SendTargets.Range, creature);

			creature.Direction = (byte)dir;
			creature.StartMove(new MabiVertex(toX, toY, toH));
			foreach (var rider in creature.Riders)
			{
				rider.Direction = creature.Direction;
				rider.StartMove(new MabiVertex(toX, toY));
			}
		}

		private void HandleLand(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var pos = creature.GetPosition();

			if (!creature.IsFlying /*|| pos.H > 16000*/)
			{
				client.Send(new MabiPacket(Op.CanLand, creature.Id).PutByte(false));
				return;
			}

			client.SendUnlock(creature, 0xFFFFBDFF);
			foreach (var rider in creature.Riders)
				client.SendUnlock(rider, 0xFFFFBDFF);

			// TODO: angled decent
			creature.SetPosition(pos.X, pos.Y, 0);
			creature.IsFlying = false;

			WorldManager.Instance.Broadcast(new MabiPacket(Op.Landing, creature.Id).PutFloats(pos.X, pos.Y).PutByte(0), SendTargets.Range, creature);
			client.Send(new MabiPacket(Op.CanLand, creature.Id).PutByte(true));
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
			WorldManager.Instance.Broadcast(new MabiPacket(Op.CombatSetTarget, creature.Id)
				.PutLong(targetId)
				.PutByte(unk1)
				.PutString(unk2)
			, SendTargets.Range, creature);

			// XXX: Should this better be placed in the skill handlers?
			WorldManager.Instance.CreatureSetTarget(creature, target);
		}

		private void HandleCombatAttack(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var targetId = packet.GetLong();
			var target = WorldManager.Instance.GetCreatureById(targetId);

			if (target == null || !target.IsAttackableBy(creature))
			{
				client.Send(new MabiPacket(Op.CombatAttackR, creature.Id));
				return;
			}

			if (creature.Vehicle != null)
				creature = creature.Vehicle;

			// TODO: Check if mount is able to attack anything? (this is done with a status)

			var attackResult = SkillResults.Failure;

			var handler = SkillManager.GetHandler(SkillConst.MeleeCombatMastery) as CombatMasteryHandler;
			if (handler == null)
				return;

			attackResult = handler.Use(creature, targetId);

			var answer = new MabiPacket(Op.CombatAttackR, creature.Id);

			if (attackResult == SkillResults.Okay)
			{
				answer.PutByte(1);
			}
			else if (attackResult == SkillResults.OutOfRange)
			{
				// Let the creature run to the target.
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
			else if (attackResult == SkillResults.Failure)
			{
				// No target, no skill, message should be more clear.
				client.Send(PacketCreator.SystemMessage(creature, "Something went wrong here, sry =/"));
			}
			else
			{
				// Stunned, missing handler, failure, should be more clear.
				answer.PutByte(0);
			}

			client.Send(answer);

			//if (target != null)
			//    client.Send(new MabiPacket(Op.StunMeter, creature.Id).PutLong(target.Id).PutByte(1).PutFloat(target.Stun));
		}

		public void HandleDeadMenu(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null && creature.IsDead)
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
			if (creature == null && creature.IsDead)
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
				WorldManager.Instance.ReviveCreature(creature);

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

		public void HandleSosButton(WorldClient client, MabiPacket packet)
		{
			// Enable = 1, Disable = 0
			client.Send(new MabiPacket(Op.SosButtonR, client.Character.Id).PutByte(false));
		}

		public void HandleAreaChange(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null && creature.IsDead)
				return;

			var eventId = packet.GetLong();
			var type = packet.GetInt();
			var unk3 = packet.GetString();

			// Check if creature is in the same region as the event.
			var region = (eventId & 0x00FFFF00000000) >> 32;
			if (creature.Region != region)
				return;

			var evnt = MabiData.RegionDb.FindEvent(eventId);
			if (evnt == null)
			{
				Logger.Warning("Unknown event: {0}", eventId.ToString("X"));
				return;
			}

			if (evnt.IsAltar)
			{
				if (type == 101)
				{
					creature.OnAltar = (DungeonAltar)eventId;
				}
				else
				{
					creature.OnAltar = DungeonAltar.None;
				}
			}

			creature.LastEventTriggered = eventId;
		}

		public void HandleStunMeterRequest(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null && creature.IsDead)
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
			if (creature == null)
				return;

			client.Send(new MabiPacket(Op.HomesteadInfoRequestR, creature.Id).PutBytes(0, 0, 1));

			// Seems to be only called on login, good place for the MOTD.
			if (WorldConf.Motd != string.Empty)
				client.Send(PacketCreator.ServerMessage(client.Character, WorldConf.Motd));

			ServerEvents.Instance.OnPlayerLoggedIn(creature as MabiPC);
		}

		public void HandleMoonGateRequest(WorldClient client, MabiPacket packet)
		{
			//001 [................]  String : _moongate_tara_west
			//002 [................]  String : _moongate_tirchonaill

			// Send no gates for now.
			client.Send(new MabiPacket(Op.MoonGateRequestR, Id.Broadcast));
		}

		public void HandleOpenItemShop(WorldClient client, MabiPacket packet)
		{
			// 1 = succes?, test = key passed to the url
			client.Send(new MabiPacket(0xA44E, client.Character.Id).PutByte(1).PutString(client.Account.Name));
		}

		public void HandleVisualChat(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null || !WorldConf.EnableVisual)
				return;

			var url = packet.GetString();
			var width = packet.GetShort();
			var height = packet.GetShort();

			var p = new MabiPacket(Op.VisualChat, creature.Id);
			p.PutString(creature.Name);
			p.PutString(url);
			p.PutShorts(width, height);
			p.PutByte(0);
			WorldManager.Instance.Broadcast(p, SendTargets.Range, creature);
		}

		public void HandleViewEquipment(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var p = new MabiPacket(Op.ViewEquipmentR, client.Character.Id);

			var targetID = packet.GetLong();
			var target = WorldManager.Instance.GetCreatureById(targetID);
			if (target == null /* || TODO: Check visibility. */)
			{
				p.PutByte(0);
				client.Send(p);
				return;
			}

			var items = target.Items.FindAll(a => a.IsEquipped() && a.Type != ItemType.Hair && a.Type != ItemType.Face);

			p.PutByte(1);
			p.PutLong(targetID);
			p.PutInt((ushort)items.Count);
			foreach (var item in items)
				item.AddToPacket(p, ItemPacketType.Private);

			client.Send(p);
		}

		public void HandleSkillAdvance(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var skillId = packet.GetShort();
			var skill = creature.GetSkill(skillId);
			if (skill == null || !skill.IsRankable)
				return;

			creature.GiveSkill(skill.Id, skill.Rank + 1);

			WorldManager.Instance.CreatureStatsUpdate(creature);
		}

		private void HandleUmbrellaJump(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			uint height = (uint)packet.GetFloat(), x = (uint)packet.GetFloat(), y = (uint)packet.GetFloat();

			creature.SetPosition(x, y);

			WorldManager.Instance.Broadcast(new MabiPacket(Op.UmbrellaJumpR, creature.Id).PutByte(2), SendTargets.Range, creature);
		}

		private void HandleUmbrellaLand(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			WorldManager.Instance.Broadcast(new MabiPacket(Op.MotionCancel2, creature.Id).PutByte(0), SendTargets.Range, creature);
		}

		protected void HandleCollectionRequest(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var p = new MabiPacket(Op.CollectionRequestR, creature.Id);

			p.PutByte(0);
			p.PutInt(0);

			p.PutByte(1);
			p.PutInt(0);

			p.PutByte(2);
			p.PutInt(0);

			client.Send(p);

			//p.PutByte(0); // Type : 0-Fishing, 1-Cooking, 2-Taming
			//p.PutInt(7);  // Count
			//{
			//    p.PutInt(50253);           // Id (scrapbook)
			//    p.PutInt(4100);
			//    p.PutInt(3002);            // Region
			//    p.PutLong(63469614973173); // Timestamp
			//    p.PutByte(1);
			//    p.PutByte(0);
			//
			//    p.PutInt(50254);
			//    p.PutInt(2200);
			//    p.PutInt(3002);
			//    p.PutLong(63469593461570);
			//    p.PutByte(1);
			//    p.PutByte(0);
			//
			//    p.PutInt(50255);
			//    p.PutInt(6000);
			//    p.PutInt(3002);
			//    p.PutLong(63469450426497);
			//    p.PutByte(1);
			//    p.PutByte(0);
			//
			//    p.PutInt(50256);
			//    p.PutInt(10200);
			//    p.PutInt(3002);
			//    p.PutLong(63469610919920);
			//    p.PutByte(1);
			//    p.PutByte(0);
			//
			//    p.PutInt(50257);
			//    p.PutInt(900);
			//    p.PutInt(3002);
			//    p.PutLong(63469589348657);
			//    p.PutByte(1);
			//    p.PutByte(0);
			//
			//    p.PutInt(50258);
			//    p.PutInt(11000);
			//    p.PutInt(3002);
			//    p.PutLong(63469439821427);
			//    p.PutByte(1);
			//    p.PutByte(0);
			//
			//    p.PutInt(50260);
			//    p.PutInt(9800);
			//    p.PutInt(3002);
			//    p.PutLong(63469451258313);
			//    p.PutByte(1);
			//    p.PutByte(0);
			//}
			//p.PutByte(1);
			//p.PutInt(1);
			//{
			//    p.PutInt(50537);
			//    p.PutInt(0);
			//    p.PutInt(20);
			//    p.PutLong(63481333396833);
			//    p.PutByte(0);
			//}
			//p.PutByte(2);
			//p.PutInt(3);
			//{
			//    p.PutInt(1007);
			//    p.PutInt(420001);
			//    p.PutInt(3300);
			//    p.PutLong(63481310925307);
			//    p.PutFloat(790);
			//    p.PutByte(0);
			//
			//    p.PutInt(1013);
			//    p.PutInt(201701);
			//    p.PutInt(401);
			//    p.PutLong(63481386587503);
			//    p.PutFloat(600);
			//    p.PutByte(0);
			//
			//    p.PutInt(2010);
			//    p.PutInt(201801);
			//    p.PutInt(401);
			//    p.PutLong(63481386642277);
			//    p.PutFloat(600);
			//    p.PutByte(0);
			//}
		}

		protected void HandleShamalaTransformation(WorldClient client, MabiPacket packet)
		{
			var creature = client.GetCreatureOrNull(packet.Id);
			if (creature == null)
				return;

			// Check skill
			if (creature.HasSkill(SkillConst.TransformationMastery))
			{
				var transId = packet.GetInt();

				// Check transformation
				var strans = MabiData.ShamalaDb.Find(transId);
				if (strans != null)
				{
					// Check character's transformations
					if ((creature as MabiPC).Shamalas.Exists(a => a.Id == transId && a.State == ShamalaState.Available))
					{
						// Check race
						var race = MabiData.RaceDb.Find(strans.Race);
						if (race != null)
						{
							creature.ShamalaRace = race;
							creature.Shamala = strans;

							// Success
							WorldManager.Instance.Broadcast(new MabiPacket(Op.ShamalaTransformation, creature.Id)
								.PutByte(1)
								.PutInt(strans.Id)
								.PutByte(1)
								.PutInt(race.Id)
								.PutFloat(strans.Size)
								.PutInt(strans.Color1)
								.PutInt(strans.Color2)
								.PutInt(strans.Color3)
								.PutByte(0)
								.PutByte(0)
							, SendTargets.Range, creature);
						}
					}
				}
			}

			// Fail
			var p = new MabiPacket(Op.ShamalaTransformation, creature.Id);
			p.PutByte(0);
			client.Send(p);
			return;

			// for reference
			//client.Send(new MabiPacket(Op.ShamalaTransformationUpdate, creature.Id)
			//    .PutInt(9)
			//    .PutByte(1)
			//    .PutByte(2)
			//    .PutByte(2)
			//);
		}

		protected void HandleShamalaTransformationEnd(WorldClient client, MabiPacket packet)
		{
			var creature = client.GetCreatureOrNull(packet.Id);
			if (creature == null)
				return;

			if (creature.Shamala == null)
			{
				client.Send(new MabiPacket(Op.ShamalaTransformationEndR).PutByte(0));
				return;
			}

			creature.Shamala = null;
			creature.ShamalaRace = null;

			// Broadcast end, success with showing ani.
			WorldManager.Instance.Broadcast(new MabiPacket(Op.ShamalaTransformationEndR, creature.Id).PutBytes(1, 1), SendTargets.Range, creature);
		}

		protected void HandleQuestComplete(WorldClient client, MabiPacket packet)
		{
			var creature = client.GetCreatureOrNull(packet.Id);
			if (creature == null)
				return;

			var character = creature as MabiPC;
			var questId = packet.GetLong();

			var quest = character.GetQuestOrNull(questId);
			if (quest == null || !quest.IsDone)
			{
				client.Send(new MabiPacket(Op.QuestCompleteR, creature.Id).PutByte(0));
				return;
			}

			// Set quest complete and complete it with reward.
			quest.State = MabiQuestState.Complete;
			WorldManager.Instance.CreatureCompletesQuest(creature, quest, true);

			// Success
			client.Send(new MabiPacket(Op.QuestCompleteR, creature.Id).PutByte(1));
		}

		protected void HandleQuestGiveUp(WorldClient client, MabiPacket packet)
		{
			var creature = client.GetCreatureOrNull(packet.Id);
			if (creature == null)
				return;

			var character = creature as MabiPC;
			var questId = packet.GetLong();
			var unk = packet.GetByte();

			var quest = character.GetQuestOrNull(questId);
			if (quest == null)
			{
				client.Send(new MabiPacket(Op.QuestGiveUpR, creature.Id).PutByte(0));
				return;
			}

			// Remove quest from char and log, without reward.
			character.Quests.Remove(quest.Class);
			WorldManager.Instance.CreatureCompletesQuest(creature, quest, false);

			// Success
			client.Send(new MabiPacket(Op.QuestGiveUpR, creature.Id).PutByte(1));
		}

		protected void HandleConvertGp(WorldClient client, MabiPacket packet)
		{
			var creature = client.GetCreatureOrNull(packet.Id);
			if (creature == null)
				return;

			if (creature.Guild == null)
				return;

			client.Send(new MabiPacket(Op.ConvertGpR, creature.Id).PutByte(1).PutInt((uint)creature.GuildMemberInfo.Gp));
		}

		protected void HandleConvertGpConfirm(WorldClient client, MabiPacket packet)
		{
			var creature = client.GetCreatureOrNull(packet.Id);
			if (creature == null)
				return;

			if (creature.Guild == null)
				return;

			creature.Guild = WorldDb.Instance.GetGuildForChar(creature.Id);

			creature.Guild.Gp += (uint)creature.GuildMemberInfo.Gp;
			client.Send(PacketCreator.GuildMessage(creature.Guild, creature, "Added " + creature.GuildMemberInfo.Gp + " Point(s)"));
			creature.GuildMemberInfo.Gp = 0;

			creature.Guild.Save();
			WorldDb.Instance.SaveGuildMember(creature.GuildMemberInfo, creature.Guild.Id);

			client.Send(new MabiPacket(Op.ConvertGpConfirmR, creature.Id).PutByte(1));
			client.Send(new MabiPacket(Op.ConvertGpConfirmR, creature.Id).PutByte(0)); // TODO: Do we really need both of these?
		}

		protected void HandleGuildDonate(WorldClient client, MabiPacket packet)
		{
			var creature = client.GetCreatureOrNull(packet.Id);
			if (creature == null)
				return;

			if (creature.Guild == null)
				return;

			var amount = packet.GetInt();

			if (!creature.HasGold(amount))
			{
				client.Send(new MabiPacket(Op.GuildDonateR, creature.Id).PutByte(0));
				return;
			}

			creature.Guild = WorldDb.Instance.GetGuildForChar(creature.Id);
			creature.Guild.Gold += amount;
			creature.RemoveGold(amount);

			creature.Guild.Save();

			client.Send(PacketCreator.GuildMessage(creature.Guild, creature, "You have donated " + amount + " Gold"));
			client.Send(new MabiPacket(Op.GuildDonateR, creature.Id).PutByte(1));
		}

		protected void HandleGuildApply(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var gid = packet.GetLong();
			var appText = packet.GetString();

			if (WorldDb.Instance.GetGuildForChar(creature.Id) != null)
			{
				client.Send(PacketCreator.MsgBox(creature, "You are already a member of a guild"));
				client.Send(new MabiPacket(Op.GuildApplyR, creature.Id).PutByte(0));
				return;
			}

			var guild = WorldDb.Instance.GetGuild(gid);

			if (guild == null)
			{
				client.Send(PacketCreator.MsgBox(creature, "Guild does not exist"));
				client.Send(new MabiPacket(Op.GuildApplyR, creature.Id).PutByte(0));
				return;
			}

			var memInfo = new MabiGuildMemberInfo()
			{
				CharacterId = creature.Id,
				JoinedDate = DateTime.Now,
				Gp = 0,
				MemberRank = (byte)GuildMemberRank.Applied,
				MessageFlags = (byte)GuildMessageFlags.None,
				ApplicationText = appText
			};

			WorldDb.Instance.SaveGuildMember(memInfo, gid);

			client.Send(PacketCreator.GuildMessage(guild, creature, "Your application has been accepted.\nPlease wait for the Guild Leader to make the final confirmation."));

			client.Send(new MabiPacket(Op.GuildApplyR, creature.Id).PutByte(1));
		}

		private void HandlePartyCreate(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var newParty = new MabiParty(creature);
			newParty.LoadFromPacket(packet);
			WorldManager.Instance.AddParty(newParty);
			creature.Party = newParty;

			var p = new MabiPacket(Op.PartyCreateR, creature.Id).PutByte(1);
			creature.Party.AddPartyPacket(p);
			creature.Client.Send(p);

			WorldManager.Instance.PartyMemberWantedShow(newParty);
		}

		private void HandlePartyJoin(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var partyLeader = WorldManager.Instance.GetCreatureById(packet.GetLong());
			if (partyLeader.Party == null)
				return;

			var party = partyLeader.Party;
			var password = packet.GetString();
			// var zero = packet.GetByte(); / ?

			if (party.Members.Count >= party.MaxSize)
			{
				creature.Client.Send(new MabiPacket(Op.PartyJoinR, creature.Id).PutByte(0));
				return;
			}

			// Password check
			if (password != party.Password)
			{
				creature.Client.Send(new MabiPacket(Op.PartyJoinR, creature.Id).PutByte(4));
				return;
			}

			// Add new member to party
			party.AddPartyMember(creature);
			creature.Party = party;

			foreach (var member in party.Members)
			{
				if (member != creature)
				{
					var p = new MabiPacket(Op.PartyJoinUpdate, member.Id);
					party.AddMemberPacket(p, creature);
					member.Client.Send(p);
				}
			}

			if (party.IsOpen)
				WorldManager.Instance.PartyMemberWantedRefresh(partyLeader.Party);

			var partyInfoPacket = new MabiPacket(Op.PartyJoinR, creature.Id).PutByte(1);
			partyLeader.Party.AddPartyPacket(partyInfoPacket);
			creature.Client.Send(partyInfoPacket);
		}

		private void HandlePartyLeave(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			if (creature.Party == null)
				return;

			// TODO: Check if allowed to leave party
			var canLeave = true;

			if (canLeave)
				WorldManager.Instance.CreatureLeaveParty(creature);

			creature.Client.Send(new MabiPacket(Op.PartyLeaveR, creature.Id).PutByte(canLeave));
		}

		private void HandlePartyRemove(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			if (creature.Party == null)
				return;

			// TODO: Check if allowed to leave party
			var canRemove = true;

			var target = WorldManager.Instance.GetCreatureById(packet.GetLong());
			if (canRemove)
			{
				WorldManager.Instance.CreatureLeaveParty(target);
				target.Client.Send(new MabiPacket(0xA43C, target.Id).PutLong(creature.Id).PutByte(1).PutByte(1).PutShort(0).PutInt(0));
				target.Client.Send(new MabiPacket(Op.PartyRemoved, target.Id));
			}

			creature.Client.Send(new MabiPacket(Op.PartyRemoveR, creature.Id).PutByte(canRemove));
		}

		private void HandlePartyChangeSettings(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			if (creature.Party == null)
				return;

			if (creature.Party.Leader != creature)
				return;

			var prevType = creature.Party.Type;
			creature.Party.LoadFromPacket(packet);

			foreach (var member in creature.Party.Members)
			{
				if (prevType != creature.Party.Type)
					member.Client.Send(new MabiPacket(Op.PartyTypeUpdate, member.Id).PutInt(creature.Party.Type));
				member.Client.Send(new MabiPacket(Op.PartySettingUpdate, member.Id).PutString(creature.Party.Name));
			}

			if (creature.Party.IsOpen)
				WorldManager.Instance.PartyMemberWantedRefresh(creature.Party);

			var p = new MabiPacket(Op.PartyChangeSettingR, creature.Id).PutByte(1);
			creature.Party.AddPartyPacket(p);
			creature.Client.Send(p);
		}

		private void HandlePartyChangePassword(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			if (creature.Party == null)
				return;

			if (creature.Party.Leader != creature)
				return;

			creature.Party.Password = packet.GetString();

			if (creature.Party.IsOpen)
				WorldManager.Instance.PartyMemberWantedRefresh(creature.Party);

			creature.Client.Send(new MabiPacket(Op.PartyChangePasswordR, creature.Id).PutLong(1));
		}

		private void HandlePartyChangeLeader(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			if (creature.Party == null)
				return;

			if (creature.Party.Leader != creature)
				return;

			// TODO: Check if able to change leader
			var leaderChangeAllow = true;

			if (leaderChangeAllow)
			{
				var newLeader = WorldManager.Instance.GetCreatureById(packet.GetLong());
				WorldManager.Instance.PartyChangeLeader(newLeader, creature.Party);
			}

			creature.Client.Send(new MabiPacket(Op.PartyChangeLeaderR, creature.Id).PutByte(leaderChangeAllow));
		}

		private void HandlePartyWantedHide(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			if (creature.Party == null)
				return;

			if (creature.Party.Leader != creature)
				return;

			WorldManager.Instance.PartyMemberWantedHide(creature.Party);
			creature.Client.Send(new MabiPacket(Op.PartyWantedHideR, creature.Id).PutByte(1));
		}

		private void HandlePartyWantedShow(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			if (creature.Party == null)
				return;

			if (creature.Party.Leader != creature)
				return;

			WorldManager.Instance.PartyMemberWantedShow(creature.Party);
			creature.Client.Send(new MabiPacket(Op.PartyWantedShowR, creature.Id).PutByte(1));
		}

		private void HandlePartyChangeFinish(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			if (creature.Party == null)
				return;

			if (creature.Party.Leader != creature)
				return;

			creature.Party.Finish = (PartyFinishRule)packet.GetInt();
			foreach (var member in creature.Party.Members)
				member.Client.Send(new MabiPacket(Op.PartyFinishUpdate, member.Id).PutInt((uint)creature.Party.Finish));
			creature.Client.Send(new MabiPacket(Op.PartyChangeFinishR, creature.Id).PutByte(1));
		}

		private void HandlePartyChangeExp(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			if (creature.Party == null)
				return;

			if (creature.Party.Leader != creature)
				return;

			creature.Party.ExpShare = (PartyExpSharing)packet.GetInt();
			foreach (var member in creature.Party.Members)
				member.Client.Send(new MabiPacket(Op.PartyExpUpdate, member.Id).PutInt((uint)creature.Party.ExpShare));
			creature.Client.Send(new MabiPacket(Op.PartyChangeExpR, creature.Id).PutByte(1));
		}

		private void HandleServerIdentify(WorldClient client, MabiPacket packet)
		{
			var success = packet.GetBool();

			if (!success)
			{
				Logger.Error("Login Server expected a different password.");
				return;
			}

			client.State = ClientState.LoggedIn;
			this.SendChannelStatus(null, null);
		}

		private void HandleCutsceneFinished(WorldClient client, MabiPacket packet)
		{
			var creature = client.GetCreatureOrNull(packet.Id);
			if (creature == null)
				return;

			// TODO: Check if vaild (is leader and whatnot)
			// if cutscene.IsLeader(creature)

			client.Send(new MabiPacket(Op.CutsceneEnd, Id.World).PutLong(creature.Id));

			WorldManager.Instance.Broadcast(PacketCreator.EntityAppears(creature), SendTargets.Range | SendTargets.ExcludeSender, creature);
			client.Send(PacketCreator.EntitiesAppear(WorldManager.Instance.GetEntitiesInRange(creature)));

			client.SendUnlock(creature);

			//client.Send(new MabiPacket(Op.CutsceneEnd+1, Id.World).PutLong(creature.Id));

			if (creature.CurrentCutscene != null)
			{
				if (creature.CurrentCutscene.OnComplete != null)
					creature.CurrentCutscene.OnComplete(client);

				creature.CurrentCutscene = null;
			}
		}

		private void HandleTalentTitleChange(WorldClient client, MabiPacket packet)
		{
			var creature = client.GetCreatureOrNull(packet.Id);
			if (creature == null)
				return;

			// TODO: Check if vaild

			var title = packet.GetShort();

			WorldManager.Instance.Broadcast(new MabiPacket(Op.TalentTitleChangedR, creature.Id).PutByte(1).PutShort(title), SendTargets.Range, creature);

			creature.SelectedTalentTitle = (TalentTitle)title;
		}

		private void HandleCombatSetAim(WorldClient client, MabiPacket packet)
		{
			var creature = client.Creatures.FirstOrDefault(a => a.Id == packet.Id);
			if (creature == null)
				return;

			var targetId = packet.GetLong();
			creature.AimStart = DateTime.Now;

			client.Send(new MabiPacket(Op.CombatSetAimR, creature.Id)
			.PutByte(1)
			.PutLong(targetId)
			.PutShort((ushort)creature.ActiveSkillId)
			.PutByte(0));
		}

		/// <summary>
		/// Parameters: None
		/// Description: Sent after Dye skill was prepared.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="packet"></param>
		private void HandleDyePaletteReq(WorldClient client, MabiPacket packet)
		{
			var creature = client.GetCreatureOrNull(packet.Id);
			if (creature == null)
				return;

			// Wave parameters for the client's "color pattern change algo".
			var p = new MabiPacket(Op.DyePaletteReqR, creature.Id);
			p.PutByte(true);
			p.PutInt(0); //p.PutInt(62);
			p.PutInt(0); //p.PutInt(123);
			p.PutInt(0); //p.PutInt(6);
			p.PutInt(0); //p.PutInt(238);
			client.Send(p);
		}

		/// <summary>
		/// Parameters:
		///		ulong  Item Object Id
		/// Description: Sent when clicking "Pick Color".
		/// </summary>
		/// <param name="client"></param>
		/// <param name="packet"></param>
		private void HandleDyePickColor(WorldClient client, MabiPacket packet)
		{
			var creature = client.GetCreatureOrNull(packet.Id);
			if (creature == null)
				return;

			var itemId = packet.GetLong();
			var item = creature.GetItem(itemId);
			if (item == null)
			{
				client.Send(new MabiPacket(Op.DyePickColorR, creature.Id).PutByte(false));
				return;
			}

			if (WorldConf.SafeDye)
			{
				creature.Temp.DyeCursors = new byte[20];
			}
			else
			{
				// 5x x+y. First byte is +, second -?
				creature.Temp.DyeCursors = new byte[]
				{ 
					0x00, 0x00, 0x00, 0x00, // Color Picker 1
					0xF5, 0xFF, 0xF5, 0xFF, // Color Picker 2
					0x0A, 0x00, 0xF5, 0xFF, // Color Picker 3
					0xF5, 0xFF, 0x0A, 0x00,	// Color Picker 4
					0x0A, 0x00, 0x0A, 0x00,	// Color Picker 5
				};
			}

			var p = new MabiPacket(Op.DyePickColorR, creature.Id);
			p.PutByte(true);
			p.PutBin(creature.Temp.DyeCursors);
			client.Send(p);
		}

		private void HandleChannelStatus(WorldClient client, MabiPacket packet)
		{
			// TODO: Fill channel list
		}

		private void HandleCancelBeautyShop(WorldClient client, MabiPacket packet)
		{
			client.Send(new MabiPacket(Op.CancelBeautyShopR, client.Character.Id));
		}

		/// <summary>
		/// Parameters: None
		/// Description: Sent when closing the chat with Nao.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="packet"></param>
		private void HandleLeaveSoulStream(WorldClient client, MabiPacket packet)
		{
			var creature = client.GetCreatureOrNull(packet.Id);
			if (creature == null)
				return;

			client.Send(new MabiPacket(Op.LeaveSoulStreamR, Id.World));

			client.SendLock(creature);
			client.SendEnterRegionPermission(creature);
		}
	}
}
