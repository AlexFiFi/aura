// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aura.Shared.Const;
using Aura.Data;
using Aura.Shared.Network;
using Aura.World.Network;

namespace Aura.World.World
{
	public class CreatureTalentManager
	{
		private MabiCreature _creature;

		public Dictionary<TalentId, Dictionary<SkillConst, uint>> ExpList { get; private set; }
		public TalentId Grandmaster { get; set; }
		public TalentTitle SelectedTitle { get; set; }

		//public int Count { get { return this.List.Values.Count; } }

		public CreatureTalentManager(MabiCreature creature)
		{
			_creature = creature;

			this.ExpList = new Dictionary<TalentId, Dictionary<SkillConst, uint>>();
		}

		public static ushort GetTalentTitle(TalentTitle talent, TalentLevel level)
		{
			if (level == 0)
				return 0;

			return (ushort)((ushort)talent + (byte)level);
		}

		public List<ushort> GetTitles()
		{
			var titles = new List<ushort>();

			var adventureLevel = this.GetTalentLevel(TalentId.Adventure);
			var warriorLevel = this.GetTalentLevel(TalentId.Warrior);
			var mageLevel = this.GetTalentLevel(TalentId.Mage);
			var archerLevel = this.GetTalentLevel(TalentId.Archer);
			var merchantLevel = this.GetTalentLevel(TalentId.Merchant);
			var battleAlchemyLevel = this.GetTalentLevel(TalentId.BattleAlchemy);
			var fighterLevel = this.GetTalentLevel(TalentId.Fighter);
			var bardLevel = this.GetTalentLevel(TalentId.Bard);
			var puppeteerLevel = this.GetTalentLevel(TalentId.Puppeteer);
			var knightLevel = this.GetTalentLevel(TalentId.Knight);
			var holyArtsLevel = this.GetTalentLevel(TalentId.HolyArts);
			var transmutaionLevel = this.GetTalentLevel(TalentId.Transmutaion);
			var cookingLevel = this.GetTalentLevel(TalentId.Cooking);
			var blacksmithLevel = this.GetTalentLevel(TalentId.Blacksmith);
			var tailoringLevel = this.GetTalentLevel(TalentId.Tailoring);
			var medicineLevel = this.GetTalentLevel(TalentId.Medicine);
			var carpentryLevel = this.GetTalentLevel(TalentId.Carpentry);

			titles.Add(GetTalentTitle(TalentTitle.Adventure, adventureLevel));
			titles.Add(GetTalentTitle(TalentTitle.Warrior, warriorLevel));
			titles.Add(GetTalentTitle(TalentTitle.Mage, mageLevel));
			titles.Add(GetTalentTitle(TalentTitle.Archer, archerLevel));
			titles.Add(GetTalentTitle(TalentTitle.Merchant, merchantLevel));
			titles.Add(GetTalentTitle(TalentTitle.BattleAlchemy, battleAlchemyLevel));
			titles.Add(GetTalentTitle(TalentTitle.Fighter, fighterLevel));
			titles.Add(GetTalentTitle(TalentTitle.Bard, bardLevel));
			titles.Add(GetTalentTitle(TalentTitle.Puppeteer, puppeteerLevel));
			titles.Add(GetTalentTitle(TalentTitle.Knight, knightLevel));
			titles.Add(GetTalentTitle(TalentTitle.HolyArts, holyArtsLevel));
			titles.Add(GetTalentTitle(TalentTitle.Transmutaion, transmutaionLevel));
			titles.Add(GetTalentTitle(TalentTitle.Cooking, cookingLevel));
			titles.Add(GetTalentTitle(TalentTitle.Blacksmith, blacksmithLevel));
			titles.Add(GetTalentTitle(TalentTitle.Tailoring, tailoringLevel));
			titles.Add(GetTalentTitle(TalentTitle.Medicine, medicineLevel));
			titles.Add(GetTalentTitle(TalentTitle.Carpentry, carpentryLevel));

			if ((byte)blacksmithLevel >= 12 && (byte)tailoringLevel >= 12 && (byte)carpentryLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Artisan, GetHybridTalentLevel(blacksmithLevel, tailoringLevel, carpentryLevel)));

			if ((byte)bardLevel >= 12 && (byte)tailoringLevel >= 12 && (byte)carpentryLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Artiste, GetHybridTalentLevel(bardLevel, tailoringLevel, carpentryLevel)));

			if ((byte)bardLevel >= 6 && (byte)warriorLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.BattleBard, GetHybridTalentLevel(bardLevel, warriorLevel)));

			if ((byte)warriorLevel >= 6 && (byte)mageLevel >= 6 && (byte)fighterLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.BattleMage, GetHybridTalentLevel(warriorLevel, mageLevel, fighterLevel)));

			if ((byte)archerLevel >= 6 && (byte)battleAlchemyLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Bombardier, GetHybridTalentLevel(archerLevel, battleAlchemyLevel)));

			if ((byte)archerLevel >= 12 && (byte)carpentryLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Bowyer, GetHybridTalentLevel(archerLevel, carpentryLevel)));

			if ((byte)warriorLevel >= 6 && (byte)fighterLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Brawler, GetHybridTalentLevel(warriorLevel, fighterLevel)));

			if ((byte)fighterLevel >= 12 && (byte)adventureLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Challenger, GetHybridTalentLevel(fighterLevel, adventureLevel)));

			if ((byte)bardLevel >= 12 && (byte)holyArtsLevel >= 12 && (byte)adventureLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Cheerleader, GetHybridTalentLevel(bardLevel, holyArtsLevel, adventureLevel)));

			if ((byte)holyArtsLevel >= 6 && (byte)bardLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Choirmaster, GetHybridTalentLevel(holyArtsLevel, bardLevel)));

			if ((byte)puppeteerLevel >= 6 && (byte)tailoringLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.CostumeDesigner, GetHybridTalentLevel(puppeteerLevel, tailoringLevel)));

			if ((byte)medicineLevel >= 12 && (byte)merchantLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Doctor, GetHybridTalentLevel(medicineLevel, merchantLevel)));

			if ((byte)mageLevel >= 6 && (byte)battleAlchemyLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Elementalist, GetHybridTalentLevel(mageLevel, battleAlchemyLevel)));

			if ((byte)warriorLevel >= 6 && (byte)carpentryLevel >= 6 && (byte)knightLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Executioner, GetHybridTalentLevel(warriorLevel, carpentryLevel, knightLevel)));

			if ((byte)fighterLevel >= 12 && (byte)cookingLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.FoodFighter, GetHybridTalentLevel(fighterLevel, cookingLevel)));

			if ((byte)battleAlchemyLevel >= 6 && (byte)transmutaionLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.FullAlchemist, GetHybridTalentLevel(battleAlchemyLevel, transmutaionLevel)));

			if ((byte)cookingLevel >= 12 && (byte)adventureLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Gourmet, GetHybridTalentLevel(cookingLevel, adventureLevel)));

			if ((byte)bardLevel >= 12 && (byte)adventureLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Gypsy, GetHybridTalentLevel(bardLevel, adventureLevel)));

			if ((byte)merchantLevel >= 12 && (byte)blacksmithLevel >= 12 && (byte)tailoringLevel >= 12 && (byte)carpentryLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Hawker, GetHybridTalentLevel(merchantLevel, blacksmithLevel, tailoringLevel, carpentryLevel)));

			if ((byte)holyArtsLevel >= 6 && (byte)archerLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.HolyArcher, GetHybridTalentLevel(holyArtsLevel, archerLevel)));

			if ((byte)holyArtsLevel >= 6 && (byte)knightLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.HolyKnight, GetHybridTalentLevel(holyArtsLevel, knightLevel)));

			if ((byte)holyArtsLevel >= 6 && (byte)warriorLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.HolyWarrior, GetHybridTalentLevel(warriorLevel, holyArtsLevel)));

			if ((byte)cookingLevel >= 12 && (byte)tailoringLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.HomeEconomist, GetHybridTalentLevel(cookingLevel, tailoringLevel)));

			if ((byte)puppeteerLevel >= 6 && (byte)bardLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Idol, GetHybridTalentLevel(puppeteerLevel, bardLevel)));

			if ((byte)fighterLevel >= 12 && (byte)blacksmithLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.IronFist, GetHybridTalentLevel(fighterLevel, blacksmithLevel)));

			if ((byte)puppeteerLevel >= 6 && (byte)holyArtsLevel >= 6 && (byte)adventureLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.JoySpreader, GetHybridTalentLevel(puppeteerLevel, holyArtsLevel, adventureLevel)));

			if ((byte)adventureLevel >= 12 && (byte)knightLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.KnightErrant, GetHybridTalentLevel(adventureLevel, warriorLevel)));

			if ((byte)carpentryLevel >= 12 && (byte)adventureLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Lumberjack, GetHybridTalentLevel(carpentryLevel, adventureLevel)));

			if ((byte)knightLevel >= 6 && (byte)mageLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.MagicKnight, GetHybridTalentLevel(knightLevel, mageLevel)));

			if ((byte)blacksmithLevel >= 12 && (byte)adventureLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Miner, GetHybridTalentLevel(blacksmithLevel, adventureLevel)));

			if ((byte)adventureLevel >= 12 && (byte)holyArtsLevel >= 12 && (byte)medicineLevel >= 12 && (byte)merchantLevel >= 12 && (byte)carpentryLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Missionary, GetHybridTalentLevel(adventureLevel, holyArtsLevel, medicineLevel, medicineLevel, carpentryLevel)));

			if ((byte)fighterLevel >= 12 && (byte)holyArtsLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Monk, GetHybridTalentLevel(fighterLevel, holyArtsLevel)));

			if ((byte)cookingLevel >= 12 && (byte)medicineLevel >= 12 && (byte)transmutaionLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Nutritionist, GetHybridTalentLevel(cookingLevel, medicineLevel)));

			if ((byte)adventureLevel >= 12 && (byte)merchantLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Peddler, GetHybridTalentLevel(adventureLevel, merchantLevel)));

			if ((byte)medicineLevel >= 12 && (byte)transmutaionLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Philosopher, GetHybridTalentLevel(medicineLevel, transmutaionLevel)));

			if ((byte)archerLevel >= 12 && (byte)bardLevel >= 12 && (byte)fighterLevel >= 12 && (byte)battleAlchemyLevel >= 12 && (byte)mageLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Polymath, GetHybridTalentLevel(archerLevel, bardLevel, fighterLevel, battleAlchemyLevel, mageLevel)));

			if ((byte)puppeteerLevel >= 6 && (byte)transmutaionLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Puppetmancer, GetHybridTalentLevel(puppeteerLevel, transmutaionLevel)));

			if ((byte)archerLevel >= 6 && (byte)warriorLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Ranger, GetHybridTalentLevel(warriorLevel, archerLevel)));

			if ((byte)mageLevel >= 6 && (byte)transmutaionLevel >= 6 && (byte)medicineLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Researcher, GetHybridTalentLevel(mageLevel, transmutaionLevel, medicineLevel)));

			if ((byte)mageLevel >= 6 && (byte)holyArtsLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Sage, GetHybridTalentLevel(mageLevel, holyArtsLevel)));

			if ((byte)mageLevel >= 6 && (byte)transmutaionLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Scholar, GetHybridTalentLevel(mageLevel, transmutaionLevel)));

			if ((byte)warriorLevel >= 6 && (byte)knightLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Slayer, GetHybridTalentLevel(warriorLevel, knightLevel)));

			if ((byte)warriorLevel >= 6 && (byte)mageLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Spellsword, GetHybridTalentLevel(warriorLevel, mageLevel)));

			if ((byte)warriorLevel >= 12 && (byte)knightLevel >= 12 && (byte)fighterLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Striker, GetHybridTalentLevel(warriorLevel, knightLevel, fighterLevel)));

			if ((byte)puppeteerLevel >= 6 && (byte)carpentryLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.ToyMaker, GetHybridTalentLevel(puppeteerLevel, carpentryLevel)));

			if ((byte)archerLevel >= 6 && (byte)fighterLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Tracker, GetHybridTalentLevel(archerLevel, fighterLevel)));

			if ((byte)archerLevel >= 6 && (byte)tailoringLevel >= 6 && (byte)carpentryLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Trapper, GetHybridTalentLevel(archerLevel, tailoringLevel, carpentryLevel)));

			if ((byte)bardLevel >= 12 && (byte)cookingLevel >= 12 && (byte)adventureLevel >= 12)
				titles.Add(GetTalentTitle(TalentTitle.Troubadour, GetHybridTalentLevel(bardLevel, cookingLevel, adventureLevel)));

			if ((byte)puppeteerLevel >= 6 && (byte)bardLevel >= 6 && (byte)adventureLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Trouper, GetHybridTalentLevel(puppeteerLevel, bardLevel)));

			if ((byte)adventureLevel >= 6 && (byte)warriorLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Vagabond, GetHybridTalentLevel(warriorLevel, adventureLevel)));

			if ((byte)fighterLevel >= 6 && (byte)knightLevel >= 6)
				titles.Add(GetTalentTitle(TalentTitle.Vanguard, GetHybridTalentLevel(fighterLevel, knightLevel)));


			return titles.Where(a => a != 0).ToList();
		}

		public TalentLevel GetTalentLevel(TalentId talent)
		{
			var exp = this.GetExp(talent) / 1000;

			if (exp < 1) return TalentLevel.None;
			else if (exp < 11) return TalentLevel.Fledgling;
			else if (exp < 36) return TalentLevel.Novice;
			else if (exp < 76) return TalentLevel.Amateur;
			else if (exp < 136) return TalentLevel.Green;
			else if (exp < 216) return TalentLevel.Naive;
			else if (exp < 316) return TalentLevel.Apprentice;
			else if (exp < 436) return TalentLevel.Senior;
			else if (exp < 576) return TalentLevel.Advanced;
			else if (exp < 736) return TalentLevel.Seasoned;
			else if (exp < 926) return TalentLevel.Skilled;
			else if (exp < 1146) return TalentLevel.Expert;
			else if (exp < 1396) return TalentLevel.Great;
			else if (exp < 1696) return TalentLevel.Champion;
			else if (exp < 2046) return TalentLevel.Wise;

			return (this.Grandmaster == talent ? TalentLevel.Grandmaster : TalentLevel.Master);
		}

		public static TalentLevel GetHybridTalentLevel(params TalentLevel[] levels)
		{
			return (TalentLevel)levels.Min(a => (byte)a);
		}


		public void UpdateExp(SkillConst skill, SkillRank rank, bool notifyRankUp = false)
		{
			var mask = (byte)this.GetRaceMask();

			var exps = MabiData.TalentExpDb.Entries.Where(a => a.SkillId == (ushort)skill && a.SkillRank <= (byte)rank && (a.Race & mask) != 0);
			if (exps == null)
				return;
			var info = exps.FirstOrDefault(a => a.SkillRank == (ushort)rank);
			if (info == null)
				return;

			foreach (var talent in info.Exps.Keys)
			{
				uint exp = (uint)exps.Sum(a => a.Exps[talent]);

				if (!this.ExpList.ContainsKey((TalentId)talent))
					this.ExpList.Add((TalentId)talent, new Dictionary<SkillConst, uint>());

				var expInfo = this.ExpList[(TalentId)talent];

				if (!expInfo.ContainsKey(skill))
					expInfo.Add(skill, 0);

				expInfo[skill] = exp;

				if (notifyRankUp)
					this.SendUpdate();
			}
		}

		public void SendUpdate()
		{
			Send.TalentInfoUpdate(_creature.Client, _creature);

			this.UpdateStats();

			WorldManager.Instance.CreatureStatsUpdate(_creature);
		}

		public void UpdateStats()
		{
			foreach (TalentId talentObj in Enum.GetValues(typeof(TalentId)))
			{
				var talentId = talentObj;

				var info = MabiData.TalentRankDb.Find((byte)talentId, (byte)this.GetTalentLevel(talentId), (byte)this.GetRaceMask());

				if (info != null)
				{
					foreach (var kvp in info.Bonuses)
					{
						_creature.StatMods.Remove((Stat)kvp.Key, StatModSource.Talent, (ulong)talentId);
						_creature.StatMods.Add((Stat)kvp.Key, kvp.Value, StatModSource.Talent, (ulong)talentId);
						switch ((Stat)kvp.Key)
						{
							case Stat.LifeMaxMod: _creature.Life += kvp.Value; break;
							case Stat.StaminaMaxMod: _creature.Stamina += kvp.Value; break;
							case Stat.ManaMaxMod: _creature.Mana += kvp.Value; break;
						}
					}
				}
			}
		}

		public TalentRace GetRaceMask()
		{
			if (_creature.IsHuman) return TalentRace.Human;
			else if (_creature.IsElf) return TalentRace.Elf;
			else if (_creature.IsGiant) return TalentRace.Giant;

			return TalentRace.None;
		}

		public uint GetExp(TalentId talent)
		{
			if (!this.ExpList.ContainsKey(talent))
				return 0;

			uint exp = 0;

			foreach (var skill in this.ExpList[talent])
				exp += skill.Value;

			return exp;
		}
	}
}
