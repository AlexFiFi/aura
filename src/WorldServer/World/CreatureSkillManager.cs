// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Events;

namespace Aura.World.World
{
	public class CreatureSkillManager
	{
		private MabiCreature _creature;

		public Dictionary<SkillConst, MabiSkill> List { get; private set; }

		public int Count { get { return this.List.Values.Count; } }

		public CreatureSkillManager(MabiCreature creature)
		{
			_creature = creature;

			this.List = new Dictionary<SkillConst, MabiSkill>();
		}

		/// <summary>
		/// Returns skill or null.
		/// </summary>
		/// <param name="skillId"></param>
		/// <returns></returns>
		public MabiSkill Get(SkillConst skillId)
		{
			MabiSkill skill;
			this.List.TryGetValue(skillId, out skill);
			return skill;
		}

		/// <summary>
		/// Returns true if the skill exists in this manager.
		/// </summary>
		/// <param name="skillId"></param>
		/// <returns></returns>
		public bool Has(SkillConst skillId)
		{
			return this.List.ContainsKey(skillId);
		}

		/// <summary>
		/// Returns true if the skill is there and its rank exceeds rank.
		/// </summary>
		/// <param name="skillId"></param>
		/// <param name="rank"></param>
		/// <returns></returns>
		public bool Has(SkillConst skillId, SkillRank rank)
		{
			var skill = this.Get(skillId);
			return (skill != null && skill.Rank >= rank);
		}

		/// <summary>
		/// Adds skill.
		/// </summary>
		/// <param name="skill"></param>
		public void Add(MabiSkill skill)
		{
			this.List.Add(skill.Id, skill);
		}

		/// <summary>
		/// Gives skills, or updates it. Updates bonuses and talent exp.
		/// </summary>
		/// <param name="skillId"></param>
		/// <param name="rank"></param>
		/// <param name="flash"></param>
		public void Give(SkillConst skillId, SkillRank rank, bool flash = true)
		{
			var skill = this.Get(skillId);
			if (skill == null)
			{
				this.Add(skill = new MabiSkill(skillId, rank, _creature.Race));

				Send.SkillInfo(_creature.Client, _creature, skill);
				if (flash)
					Send.RankUp(_creature);

				EventManager.CreatureEvents.OnCreatureSkillChange(_creature, skill, true);
			}
			else
			{
				this.RemoveSkillBonuses(skill);

				skill.Info.Experience = 0;
				skill.Info.Rank = (byte)rank;
				skill.LoadRankInfo();

				Send.SkillRankUp(_creature.Client, _creature, skill);
				if (flash)
					Send.RankUp(_creature, skill.Info.Id);

				EventManager.CreatureEvents.OnCreatureSkillChange(_creature, skill, false);
			}

			this.AddBonuses(skill);

			_creature.Talents.UpdateExp(skillId, rank, true);
		}

		public void AddBonuses(MabiSkill skill)
		{
			var mana = skill.RankInfo.ManaTotal;
			var life = skill.RankInfo.LifeTotal;
			var stamina = skill.RankInfo.StaminaTotal;

			_creature.StrBaseSkill += skill.RankInfo.StrTotal;
			_creature.WillBaseSkill += skill.RankInfo.WillTotal;
			_creature.IntBaseSkill += skill.RankInfo.IntTotal;
			_creature.LuckBaseSkill += skill.RankInfo.LuckTotal;
			_creature.DexBaseSkill += skill.RankInfo.DexTotal;
			_creature.ManaMaxBaseSkill += mana;
			_creature.Mana += mana;
			_creature.LifeMaxBaseSkill += life;
			_creature.Life += life;
			_creature.StaminaMaxBaseSkill += stamina;
			_creature.Stamina += stamina;

			if (skill.Id == SkillConst.MeleeCombatMastery)
			{
				_creature.StatMods.Add(Stat.LifeMaxMod, skill.RankInfo.Var3, StatModSource.SkillRank, skill.Info.Id);
				_creature.Life += skill.RankInfo.Var3;
			}
			else if (skill.Id == SkillConst.MagicMastery)
			{
				_creature.StatMods.Add(Stat.ManaMaxMod, skill.RankInfo.Var1, StatModSource.SkillRank, skill.Info.Id);
				_creature.Mana += skill.RankInfo.Var1;
			}
			else if (skill.Id == SkillConst.Defense)
			{
				_creature.DefenseBaseSkill += (int)skill.RankInfo.Var1;
			}
		}

		private void RemoveSkillBonuses(MabiSkill skill)
		{
			var mana = skill.RankInfo.ManaTotal;
			var life = skill.RankInfo.LifeTotal;
			var stamina = skill.RankInfo.StaminaTotal;

			_creature.StrBaseSkill -= skill.RankInfo.StrTotal;
			_creature.WillBaseSkill -= skill.RankInfo.WillTotal;
			_creature.IntBaseSkill -= skill.RankInfo.IntTotal;
			_creature.LuckBaseSkill -= skill.RankInfo.LuckTotal;
			_creature.DexBaseSkill -= skill.RankInfo.DexTotal;
			_creature.Mana -= mana;
			_creature.ManaMaxBaseSkill -= mana;
			_creature.Life -= life;
			_creature.LifeMaxBaseSkill -= life;
			_creature.Stamina -= stamina;
			_creature.StaminaMaxBaseSkill -= stamina;

			if (skill.Id == SkillConst.MeleeCombatMastery)
			{
				_creature.Life -= skill.RankInfo.Var3;
				_creature.StatMods.Remove(Stat.LifeMaxMod, StatModSource.SkillRank, skill.Info.Id);
			}
			else if (skill.Id == SkillConst.MagicMastery)
			{
				_creature.Mana -= skill.RankInfo.Var1;
				_creature.StatMods.Remove(Stat.ManaMaxMod, StatModSource.SkillRank, skill.Info.Id);
			}
			else if (skill.Id == SkillConst.Defense)
			{
				_creature.DefenseBaseSkill -= (int)skill.RankInfo.Var1;
			}
		}
	}
}
