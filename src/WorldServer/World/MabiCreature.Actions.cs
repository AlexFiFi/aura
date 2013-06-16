// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using Aura.World.Events;
using Aura.World.Network;
using Aura.Shared.Util;
using Aura.Shared.Network;
using Aura.World.Player;
using Aura.Shared.Const;
using Aura.World.Skills;
using Aura.Data;
using Aura.World.Scripting;

namespace Aura.World.World
{
	public abstract partial class MabiCreature : MabiEntity
	{
		public void ChangeTitle(ushort title, ushort optionTitle)
		{
			this.Title = title;
			this.OptionTitle = optionTitle;
			this.TitleApplied = (ulong)DateTime.Now.Ticks;

			WorldManager.Instance.Broadcast(new MabiPacket(Op.ChangedTitle, this.Id).PutShort(title).PutShort(optionTitle), SendTargets.Range, this);
		}

		public void SetTarget(MabiCreature target)
		{
			this.Target = target;

			WorldManager.Instance.Broadcast(new MabiPacket(Op.CombatTargetSet, this.Id).PutLong(target != null ? target.Id : 0), SendTargets.Range, this);
		}

		/// <summary>
		/// Broadcasts creature-moving information, begins calculating the creature's position, activates mobs, and raises the CreatureMoves event.
		/// </summary>
		public void Move(MabiVertex to, bool walking = false)
		{
			var from = this.GetPosition();

			var p = new MabiPacket((walking ? Op.Walking : Op.Running), this.Id);
			p.PutInt(from.X);
			p.PutInt(from.Y);
			p.PutInt(to.X);
			p.PutInt(to.Y);

			WorldManager.Instance.Broadcast(p, SendTargets.Range, this);

			this.StartMoveCalculation(to, walking);

			if (this is MabiPC)
				WorldManager.Instance.ActivateMobs(this, from, to);

			switch (this.ActiveSkillId)
			{
				case SkillConst.RangedCombatMastery:
				case SkillConst.ArrowRevolver:
				case SkillConst.ArrowRevolver2:
				case SkillConst.MagnumShot:
				case SkillConst.SupportShot:
				case SkillConst.ElvenMagicMissile:
				case SkillConst.MirageMissile:
				case SkillConst.CrashShot:
					CombatHelper.ResetCreatureAim(this);
					break;
			}

			EventManager.Instance.CreatureEvents.OnCreatureMoves(this, new MoveEventArgs(this, from, to));
		}

		public void StopMove()
		{
			this.StopMoveCalculation();

			var pos = this.GetPosition();

			var p = new MabiPacket(Op.RunTo, this.Id);
			p.PutInts(pos.X, pos.Y); // From
			p.PutInts(pos.X, pos.Y); // To
			p.PutBytes(1, 0);

			WorldManager.Instance.Broadcast(p, SendTargets.Range, this);
		}

		public void SwitchSet(byte set = 255)
		{
			if (set == 255)
				set = (byte)(this.WeaponSet == 1 ? 0 : 1);

			this.WeaponSet = set;

			this.UpdateItemsFromPockets(Pocket.RightHand1);
			this.UpdateItemsFromPockets(Pocket.LeftHand1);
			this.UpdateItemsFromPockets(Pocket.Magazine1);

			WorldManager.Instance.Broadcast(new MabiPacket(Op.SwitchedSet, this.Id).PutByte(this.WeaponSet), SendTargets.Range, this);

			this.BroadcastStatsUpdate();
		}

		public void SitDown()
		{
			this.State |= CreatureStates.SittingDown;

			WorldManager.Instance.Broadcast(new MabiPacket(Op.Resting, this.Id).PutByte(this.RestPose), SendTargets.Range, this);
		}

		public void StandUp()
		{
			this.State &= ~CreatureStates.SittingDown;

			WorldManager.Instance.Broadcast(new MabiPacket(Op.StandUp, this.Id).PutByte(1), SendTargets.Range, this);
		}

		/// <summary>
		/// Broadcasts the Motion packet and raises the CreatureUsesMotion event
		/// </summary>
		/// <param name="category"></param>
		/// <param name="type"></param>
		/// <param name="loop"></param>
		/// <param name="cancel"></param>
		public void UseMotion(uint category, uint type, bool loop = false, bool cancel = true)
		{
			MabiPacket p;

			if (cancel)
			{
				// Cancel motion
				p = new MabiPacket(Op.MotionCancel, this.Id).PutByte(0);
				WorldManager.Instance.Broadcast(p, SendTargets.Range, this);
			}

			// Do motion
			p = new MabiPacket(Op.Motions, this.Id)
				.PutInt(category)
				.PutInt(type)
				.PutByte(loop)
				.PutShort(0);
			WorldManager.Instance.Broadcast(p, SendTargets.Range, this);

			EventManager.Instance.CreatureEvents.OnCreatureUsesMotion(this, new MotionEventArgs(this, category, type, loop));
		}

		public void ChangeBattleState(bool battleState, byte unk = 1)
		{
			this.BattleState = battleState;

			var p = new MabiPacket(Op.ChangesStance, this.Id);
			p.PutByte(this.BattleState);
			p.PutByte(unk);

			WorldManager.Instance.Broadcast(p, SendTargets.Range, this);
		}

		public void Unequip(Pocket from)
		{
			var p = new MabiPacket(Op.EquipmentMoved, this.Id);
			p.PutByte((byte)from);
			p.PutByte(1);

			this.BroadcastStatsUpdate();

			WorldManager.Instance.Broadcast(p, SendTargets.Range, this);

			this.UpdateItemsFromPockets();
		}

		/// <summary>
		/// "Unequip's" complement
		/// </summary>
		/// <param name="item"></param>
		public void EquipmentChanged(MabiItem item)
		{
			var p = new MabiPacket(Op.EquipmentChanged, this.Id);
			p.PutBin(item.Info);
			p.PutByte(1);

			WorldManager.Instance.Broadcast(p, SendTargets.Range, this);

			this.BroadcastStatsUpdate();

			this.UpdateItemsFromPockets();
		}

		public void SkillUpdate(MabiSkill skill, bool isNew = false)
		{
			if (isNew)
			{
				this.Client.Send(new MabiPacket(Op.SkillInfo, this.Id).PutBin(skill.Info));
			}
			else
			{
				this.Client.Send(new MabiPacket(Op.SkillRankUp, this.Id).PutByte(1).PutBin(skill.Info).PutFloat(0));
			}
			WorldManager.Instance.Broadcast(new MabiPacket(Op.RankUp, this.Id).PutShorts(skill.Info.Id, 1), SendTargets.Range, this);
		}

		public void LeaveRegion()
		{
			WorldManager.Instance.Broadcast(PacketCreator.EntityLeaves(this), SendTargets.Range, this);

			EventManager.Instance.EntityEvents.OnEntityLeavesRegion(this, new EntityEventArgs(this));
		}

		public void Talk(string message, byte type = 0)
		{
			var p = new MabiPacket(Op.Chat, this.Id);
			p.PutByte(type);
			p.PutString(this.Name);
			p.PutString(message);

			WorldManager.Instance.Broadcast(p, SendTargets.Range, this);

			EventManager.Instance.CreatureEvents.OnCreatureTalks(this, new ChatEventArgs(this, message));
		}

		public void EnterRegion()
		{
			if (this.ArenaPvPManager != null && this.Region != this.ArenaPvPManager.LobbyRegion && this.Region != this.ArenaPvPManager.ArenaRegion)
			{
				this.ArenaPvPManager.Leave(this);
				this.ArenaPvPManager = null;
				WorldManager.Instance.Broadcast(PacketCreator.PvPInfoChanged(this), SendTargets.Range, this);
			}

			foreach (var arena in WorldManager.Instance.ArenaPvPs)
			{
				if (this.Region == arena.LobbyRegion)
				{
					this.ArenaPvPManager = arena;
					arena.EnterLobby(this);
				}
				else if (this.Region == arena.ArenaRegion)
				{
					this.ArenaPvPManager = arena;
					arena.EnterArena(this);
				}
			}
		}

		public void BroadcastStatsUpdate()
		{
			// Public
			WorldManager.Instance.Broadcast(
				PacketCreator.StatUpdate(this, StatUpdateType.Public,
					Stat.Height, Stat.Weight, Stat.Upper, Stat.Lower, Stat.CombatPower,
					Stat.Life, Stat.LifeInjured, Stat.LifeMax, Stat.LifeMaxMod
				)
			, SendTargets.Range, this);

			// Private
			this.Client.Send(
				PacketCreator.StatUpdate(this, StatUpdateType.Private,
					Stat.Life, Stat.LifeInjured, Stat.LifeMax, Stat.LifeMaxMod,
					Stat.Mana, Stat.ManaMax, Stat.ManaMaxMod,
					Stat.Stamina, Stat.Food, Stat.StaminaMax, Stat.StaminaMaxMod,
					Stat.Str, Stat.Dex, Stat.Int, Stat.Luck, Stat.Will,
					Stat.StrMod, Stat.DexMod, Stat.IntMod, Stat.LuckMod, Stat.WillMod,
					Stat.Level, Stat.Experience, Stat.AbilityPoints, Stat.DefenseBaseMod, Stat.DefenseMod, Stat.ProtectBaseMod, Stat.ProtectMod
				)
			);
		}

		public void CompleteQuest(MabiQuest quest, bool rewards)
		{
			if (rewards)
			{
				quest.State = MabiQuestState.Complete;

				// Owl
				WorldManager.Instance.Broadcast(new MabiPacket(Op.QuestOwlComplete, this.Id).PutLong(quest.Id), SendTargets.Range, this);

				// Rewards
				foreach (var reward in quest.Info.Rewards)
				{
					switch (reward.Type)
					{
						case RewardType.Exp:
							this.GiveExp(reward.Amount);
							this.Client.Send(PacketCreator.AcquireExp(this, reward.Amount));
							this.BroadcastStatsUpdate();
							break;

						case RewardType.Gold:
							this.GiveItem(2000, reward.Amount);
							this.Client.Send(PacketCreator.AcquireItem(this, reward.Id, reward.Amount));
							break;

						case RewardType.Item:
							this.GiveItem(reward.Id, reward.Amount);
							this.Client.Send(PacketCreator.AcquireItem(this, reward.Id, reward.Amount));
							break;

						case RewardType.Skill:
							var id = (SkillConst)reward.Id;
							var rank = (SkillRank)reward.Amount;

							// Only give skill if char doesn't have it or rank is lower.
							var skill = this.GetSkill(id);
							if (skill == null || skill.Rank < rank)
							{
								this.GiveSkill(id, rank);
								this.BroadcastStatsUpdate();
							}
							break;

						default:
							Logger.Warning("Unsupported reward type '{0}'.", reward.Type);
							break;
					}
				}

				// Only call this if there were rewards, we're using this
				// method to clear quests as well.
				var script = ScriptManager.Instance.GetQuestScript(quest.Info.Id);
				if (script != null)
					script.OnCompleted(this.Client as WorldClient, quest);
			}

			this.Client.Send(PacketCreator.ItemInfo(this, quest.QuestItem));

			// Remove from quest log.
			this.Client.Send(new MabiPacket(Op.QuestClear, this.Id).PutLong(quest.Id));
		}

		public void UpdateQuest(MabiQuest quest)
		{
			var p = new MabiPacket(Op.QuestUpdate, this.Id);
			quest.AddProgressData(p);
			this.Client.Send(p);
		}

		public void ReceivesQuest(MabiQuest quest)
		{
			// Owl
			WorldManager.Instance.Broadcast(new MabiPacket(Op.QuestOwlNew, this.Id).PutLong(quest.Id), SendTargets.Range, this);

			// Quest item (required to complete quests)
			this.Client.Send(PacketCreator.ItemInfo(this, quest.QuestItem));

			// Quest info
			var p = new MabiPacket(Op.QuestNew, this.Id);
			quest.AddToPacket(p);
			this.Client.Send(p);
		}

		public void LeaveParty()
		{
			if (this.Client == null || this.Party == null)
				return;

			// Remove from party
			this.Party.RemovePartyMember(this);
			this.Party = null;
			this.PartyNumber = 0;
		}

		public void DeadFeather(bool ukn, DeadMenuOptions opts)
		{
			var pkt = new MabiPacket(Op.DeadFeather, this.Id);

			var bits = (uint)opts; // Avoid backfill

			List<uint> flags = new List<uint>();

			uint index = 1; // 1 based
			while (bits != 0)
			{
				if ((bits & 1) != 0)
					flags.Add(index);
				index++;

				bits >>= 1;
			}

			pkt.PutShort((ushort)flags.Count);

			foreach (var f in flags)
				pkt.PutInt(f);

			pkt.PutByte(ukn);
			WorldManager.Instance.Broadcast(pkt, SendTargets.Range, this);
		}

		public void SkillCancel()
		{
			if (this.ActiveSkillId != SkillConst.None)
			{
				MabiSkill skill; SkillHandler handler;
				SkillManager.CheckOutSkill(this, (ushort)this.ActiveSkillId, out skill, out handler);
				if (skill == null || handler == null)
					return;

				var result = handler.Cancel(this, skill);

				if ((result & SkillResults.Okay) == 0)
					return;

				this.Client.SendSkillStackUpdate(this, skill.Id, 0);
				this.Client.SendSkillCancel(this);
			}

			this.ActiveSkillId = SkillConst.None;
			this.ActiveSkillStacks = 0;
		}

		public void VehicleBind(MabiCreature vehicle)
		{
			this.Vehicle = vehicle;
			vehicle.Riders.Add(this);

			var bind1 = new MabiPacket(Op.VehicleBond, this.Id);
			bind1.PutInt(vehicle.RaceInfo.VehicleType);
			bind1.PutInt(7);
			bind1.PutLong(vehicle.Id);
			bind1.PutInt(0);
			bind1.PutByte(1);
			bind1.PutLong(this.Id);

			var bind2 = new MabiPacket(Op.VehicleBond, vehicle.Id);
			bind2.PutInt(vehicle.RaceInfo.VehicleType);
			bind2.PutInt(0);
			bind2.PutLong(vehicle.Id);
			bind2.PutInt(32);
			bind2.PutByte(1);
			bind2.PutLong(this.Id);

			WorldManager.Instance.Broadcast(bind1, SendTargets.Range, this);
			WorldManager.Instance.Broadcast(bind2, SendTargets.Range, vehicle);
			//WorldManager.Instance.CreatureUseMotion(vehicle, 30, 0, false, false);
			//WorldManager.Instance.CreatureUseMotion(creature, 90, 0, false, false);
			WorldManager.Instance.Broadcast(bind1, SendTargets.Range, this);
			WorldManager.Instance.Broadcast(bind2, SendTargets.Range, vehicle);
		}

		public void VehicleUnbind(MabiCreature vehicle, bool spawn = false)
		{
			this.Vehicle = null;
			if (vehicle.Riders.Contains(this))
				vehicle.Riders.Remove(this);

			MabiPacket p;

			if (!spawn)
			{
				p = new MabiPacket(Op.VehicleBond, this.Id);
				p.PutInt(0);
				p.PutInt(0);
				p.PutLong(0);
				p.PutInt(0);
				p.PutByte(0);
				WorldManager.Instance.Broadcast(p, SendTargets.Range, this);

				p = new MabiPacket(Op.VehicleBond, this.Id);
				p.PutInt(0);
				p.PutInt(5);
				p.PutLong(this.Id);
				p.PutInt(32);
				p.PutByte(0);
				WorldManager.Instance.Broadcast(p, SendTargets.Range, this);
			}

			p = new MabiPacket(Op.VehicleBond, vehicle.Id);
			p.PutInt(0);
			p.PutInt(1);
			p.PutLong(vehicle.Id);
			p.PutInt(32);
			p.PutByte(0);
			WorldManager.Instance.Broadcast(p, SendTargets.Range, vehicle);
		}

		/// <summary>
		/// Sends conditions update to all players in range of creature.
		/// </summary>
		/// <param name="wm"></param>
		/// <param name="creature"></param>
		public void BroadcastStatusEffectUpdate()
		{
			var p = new MabiPacket(Op.StatusEffectUpdate, this.Id);
			p.PutLong((ulong)this.Conditions.A);
			p.PutLong((ulong)this.Conditions.B);
			p.PutLong((ulong)this.Conditions.C);
			p.PutLong((ulong)this.Conditions.D);
			p.PutInt(0);

			WorldManager.Instance.Broadcast(p, SendTargets.Range, this);
		}
	}
}
