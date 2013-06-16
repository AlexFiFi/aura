// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Aura.Shared.Network;
using Aura.World.World;
using System;
using Aura.World.Network;
using Aura.Shared.Const;

namespace Aura.World.Skills
{
	public class SpiritOfOrderHandler : SkillHandler
	{
		public override SkillResults Start(MabiCreature creature, MabiSkill skill)
		{
			WorldManager.Instance.Broadcast(GetPacket(creature, skill, true), SendTargets.Range, creature);

			//creature.Title = 40003;
			//WorldManager.Instance.CreatureChangeTitle(creature);

			//Pdef
			//creature.Client.Send(new MabiPacket(0x9091, creature.Id).PutInts(6500, 58).PutShort(40011));
			//creature.Client.Send(new MabiPacket(0x9091, creature.Id).PutInts(8000, 58).PutShort(40012));

			this.AddStatBonus(creature, skill);

			creature.FullHeal();
			creature.BroadcastStatsUpdate();

			return SkillResults.Okay;
		}

		public override SkillResults Stop(MabiCreature creature, MabiSkill skill)
		{
			WorldManager.Instance.Broadcast(GetPacket(creature, skill, false), SendTargets.Range, creature);

			this.RemoveStatBonus(creature, skill);

			creature.BroadcastStatsUpdate();

			return SkillResults.Okay;
		}

		public virtual void AddStatBonus(MabiCreature creature, MabiSkill skill)
		{
			creature.StatMods.Add(Stat.LifeMaxMod, skill.RankInfo.Var1, StatModSource.Skill, (ulong)skill.Id);
			creature.StatMods.Add(Stat.ManaMaxMod, skill.RankInfo.Var2, StatModSource.Skill, (ulong)skill.Id);
			creature.StatMods.Add(Stat.StaminaMaxMod, skill.RankInfo.Var3, StatModSource.Skill, (ulong)skill.Id);
			creature.StatMods.Add(Stat.StrMod, 100 + 10 * skill.Info.Rank, StatModSource.Skill, (ulong)skill.Id);
			creature.StatMods.Add(Stat.DexMod, 100 + 10 * skill.Info.Rank, StatModSource.Skill, (ulong)skill.Id);
		}

		public virtual void RemoveStatBonus(MabiCreature creature, MabiSkill skill)
		{
			creature.StatMods.Remove(Stat.LifeMaxMod, StatModSource.Skill, (ulong)skill.Id);
			creature.StatMods.Remove(Stat.ManaMaxMod, StatModSource.Skill, (ulong)skill.Id);
			creature.StatMods.Remove(Stat.StaminaMaxMod, StatModSource.Skill, (ulong)skill.Id);
			creature.StatMods.Remove(Stat.StrMod, StatModSource.Skill, (ulong)skill.Id);
			creature.StatMods.Remove(Stat.DexMod, StatModSource.Skill, (ulong)skill.Id);
		}
		private MabiPacket GetPacket(MabiCreature creature, MabiSkill skill, bool transforming)
		{
			// TODO: Add actual transform level from rank info.
			// intVal1 = look, intVal2 = titleId
			var p = new MabiPacket(0xA41C, creature.Id);
			p.PutByte((byte)(transforming ? this.TransformId : (byte)0));
			p.PutShort((ushort)(transforming ? 15 : 0));
			p.PutShort((ushort)(transforming ? ((skill.Info.Rank + 1) / 4) : 0));
			p.PutByte(1);
			return p;
		}

		protected virtual byte TransformId { get { return 1; } }
	}

	public class SoulOfChaosHandler : SpiritOfOrderHandler
	{
		protected override byte TransformId { get { return 2; } }

		public override SkillResults Start(MabiCreature creature, MabiSkill skill)
		{
			var r = base.Start(creature, skill);
			if (Util.WorldConf.DkSoundFix && (r & SkillResults.Okay) == SkillResults.Okay)
			{
				System.Threading.Thread t = new System.Threading.Thread(() =>
					{
						WorldManager.Instance.Broadcast(new MabiPacket(Op.PlaySound, creature.Id).PutString("data/sound/Glasgavelen_blowaway_endure.wav"), SendTargets.Range, creature);
						System.Threading.Thread.Sleep(420);
						WorldManager.Instance.Broadcast(new MabiPacket(Op.PlaySound, creature.Id).PutString("data/sound/g1_darkmagic_0.wav"), SendTargets.Range, creature);
						System.Threading.Thread.Sleep(2050);
						WorldManager.Instance.Broadcast(new MabiPacket(Op.PlaySound, creature.Id).PutString("data/sound/g1_scene_change.wav"), SendTargets.Range, creature);
						System.Threading.Thread.Sleep(470);
						WorldManager.Instance.Broadcast(new MabiPacket(Op.PlaySound, creature.Id).PutString("data/sound/g1_scene_change.wav"), SendTargets.Range, creature);
					});
				t.Start();
			}
			return r;
		}

		// Bonuses must be stored somewhere, to properly remove them again.
		//public override void AddStatBonus(MabiCreature creature, MabiSkill skill)
		//{
		//    var rnd = RandomProvider.Get();

		//    creature.LifeMaxMod += rnd.Next((int)skill.RankInfo.Var1, (int)skill.RankInfo.Var1 * 2 + 1);
		//    creature.ManaMaxMod += rnd.Next((int)skill.RankInfo.Var2, (int)skill.RankInfo.Var2 * 2 + 1);
		//    creature.StaminaMaxMod += rnd.Next((int)skill.RankInfo.Var1 / 2, (int)skill.RankInfo.Var1 * 2 + 1); // < wrong
		//    creature.StrMod += 999;
		//    creature.DexMod += 999;
		//}

		//public override void RemoveStatBonus(MabiCreature creature, MabiSkill skill)
		//{
		//    creature.LifeMaxMod -= skill.RankInfo.Var1;
		//    creature.ManaMaxMod -= skill.RankInfo.Var2;
		//    creature.StaminaMaxMod -= skill.RankInfo.Var3;
		//    creature.StrMod -= 999;
		//    creature.DexMod -= 999;
		//}
	}

	public class FuryOfConnousHandler : SpiritOfOrderHandler
	{
		protected override byte TransformId { get { return 4; } }
	}
}
