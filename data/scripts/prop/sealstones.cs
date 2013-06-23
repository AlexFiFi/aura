// --- Aura Script ----------------------------------------------------------
//  Seal Stones
// --- Description ----------------------------------------------------------
//  Restrict access to specific areas, through placement of shapely rocks.
// --------------------------------------------------------------------------

using System;
using Aura.Shared.Const;
using Aura.Shared.Network;
using Aura.World.Network;
using Aura.World.Player;
using Aura.World.Scripting;
using Aura.World.World;

// Dugald
// --------------------------------------------------------------------------

public class DugaldSealStoneScript : _SealStoneScript
{
	public override void Init()
	{
		SetName("Seal Stone of Dugald Aisle", "_sealstone_dugald");
		SetLocation(16, 19798, 4456, 1.48f);
		SetHelp("The Seal of Dugald Aisle\n\nGet 20 ranks in skills.");
	}
	
	public override bool Check(WorldClient c, MabiPC cr, MabiProp prop)
	{
		// Number Total Skill Ranks over 20 
		int rank = 0;
		foreach(var skill in cr.Skills.Values)
		{
			if((rank += skill.Info.Rank) >= 20)
				return true;
		}
		
		return false;
	}
	
	public override void OnBreak(MabiPC cr)
	{
		cr.Titles[10002] = true; // the Dugald Aisle Seal Breaker
	}
}

// Ciar
// --------------------------------------------------------------------------

public class CiarSealStoneScript : _SealStoneScript
{
	public override void Init()
	{
		SetName("Seal Stone of Ciar Dungeon", "_sealstone_ciar");
		SetLocation(1, 28003, 30528, 0.16f);
		SetHelp("The Seal of Ciar Dungeon\n\nGet over 35 Strength.");
	}
	
	public override bool Check(WorldClient c, MabiPC cr, MabiProp prop)
	{
		return (cr.Str >= 35);
	}
	
	public override void OnBreak(MabiPC cr)
	{
		cr.Titles[10003] = true; // the Ciar Seal Breaker
	}
}

// Rabbie
// --------------------------------------------------------------------------

public class RabbieSealStoneScript : _SealStoneScript
{
	public override void Init()
	{
		SetName("Seal Stone of Rabbie Dungeon", "_sealstone_rabbie");
		SetLocation(14, 16801, 58978, 4.71f);
		SetHelp("The Seal of Rabbie Dungeon\n\nReach level 35+.");
	}
	
	public override bool Check(WorldClient c, MabiPC cr, MabiProp prop)
	{
		return (cr.Level >= 35);
	}
	
	public override void OnBreak(MabiPC cr)
	{
		cr.Titles[10004] = true; // the Rabbie Seal Breaker
	}
}

// Math
// --------------------------------------------------------------------------

public class MathSealStoneScript : _SealStoneScript
{
	public override void Init()
	{
		SetName("Seal Stone of Math Dungeon", "_sealstone_math");
		SetLocation(14, 58409, 58185, 4.71f);
		SetHelp("The Seal of Math Dungeon\n\nBe a good little bard.");
	}
	
	public override bool Check(WorldClient c, MabiPC cr, MabiProp prop)
	{
		// Must have rank D Playing Instrument, Composing, and Musical Knowledge
		return (
			(cr.HasSkill(SkillConst.PlayingInstrument) && cr.GetSkill(SkillConst.PlayingInstrument).Rank >= SkillRank.RD) &&
			(cr.HasSkill(SkillConst.Composing) && cr.GetSkill(SkillConst.Composing).Rank >= SkillRank.RD) &&
			(cr.HasSkill(SkillConst.MusicalKnowledge) && cr.GetSkill(SkillConst.MusicalKnowledge).Rank >= SkillRank.RD)
		);
	}
	
	public override void OnBreak(MabiPC cr)
	{
		cr.Titles[10005] = true; // the Math Seal Breaker
	}
}

// Bangor
// --------------------------------------------------------------------------

public class BangorSealStoneScript : _SealStoneScript
{
	public override void Init()
	{
		SetName("Seal Stone of Bangor", "_sealstone_bangor");
		SetLocation(30, 39189, 17014, 1.54f);
		SetHelp("The Seal of Bangor\n\nBangor needs archers! Eh...");
	}
	
	public override bool Check(WorldClient c, MabiPC cr, MabiProp prop)
	{
		// Must have 13+ ranks of Archery Skills
		int rank = 0;
		foreach(var skill in cr.Skills.Values)
		{
			if(
				skill.Id != SkillConst.RangedCombatMastery && 
				skill.Id != SkillConst.MagnumShot && 
				skill.Id != SkillConst.ArrowRevolver && 
				skill.Id != SkillConst.ArrowRevolver2 && 
				skill.Id != SkillConst.SupportShot && 
				skill.Id != SkillConst.MirageMissile
			)
				continue;
			
			if((rank += skill.Info.Rank) >= 13)
				return true;
		}
		
		return false;
	}
	
	public override void OnBreak(MabiPC cr)
	{
		cr.Titles[10006] = true; // the Bangor Breaker
	}
}

// Fiodh
// --------------------------------------------------------------------------

public class FiodhSealStoneScript : _SealStoneScript
{
	public override void Init()
	{
		SetName("Seal Stone of Fiodh Dungeon", "_sealstone_fiodh");
		SetLocation(30, 10696, 83099, 4.7f);
		SetHelp("The Seal of Fiodh Dungeon\n\nGot titles?");
	}
	
	public override bool Check(WorldClient c, MabiPC cr, MabiProp prop)
	{
		return (cr.Titles.Count >= 18);
	}
	
	public override void OnBreak(MabiPC cr)
	{
		cr.Titles[10008] = true; // the Fiodh Breaker
	}
}

// North Emain Macha
// --------------------------------------------------------------------------

public class NorthEmainSealStoneScript : _SealStoneScript
{
	public override void Init()
	{
		SetName("Seal Stone of North Emain Macha", "_sealstone_osnasail");
		SetLocation(70, 7844, 13621, 0);
		SetHelp("The Seal of North Emain Macha\n\nExperience before Age.");
	}
	
	public override bool Check(WorldClient c, MabiPC cr, MabiProp prop)
	{
		return (cr.Level >= (cr.Age * 4));
	}
	
	public override void OnBreak(MabiPC cr)
	{
		cr.Titles[10025] = true; // the North Emain Macha Seal Breaker
	}
}

// South Emain Macha
// --------------------------------------------------------------------------

public class SouthEmainSealStoneScript : _SealStoneScript
{
	public override void Init()
	{
		SetName("Seal Stone of South Emain Macha", "_sealstone_south_emainmacha");
		SetLocation(53, 67830, 107710, 0);
		SetHelp("The Seal of South Emain Macha\n\nExperience before Age.");
	}
	
	public override bool Check(WorldClient c, MabiPC cr, MabiProp prop)
	{
		return (cr.Level >= (cr.Age * 4));
	}
	
	public override void OnBreak(MabiPC cr)
	{
		cr.Titles[10009] = true; // the South Emain Macha Seal Breaker
	}
}

// Abb Neagh
// --------------------------------------------------------------------------

public class AbbSealStoneScript : _SealStoneScript
{
	public override void Init()
	{
		SetName("Seal Stone of Abb Neagh", "_sealstone_south_taillteann");
		SetLocation(14, 14023, 56756, 0);
		SetHelp("The Seal of Abb Neagh\n\nBlah, Wand, blah, Mage.");
	}
	
	public override bool Check(WorldClient c, MabiPC cr, MabiProp prop)
	{
		// Wand
		if(cr.RightHand != null && cr.RightHand.Info.Class >= 40038 && cr.RightHand.Info.Class <= 40041)
			return true;
		
		return false;
	}
	
	public override void OnBreak(MabiPC cr)
	{
		cr.Titles[10068] = true; // the Abb Neagh Seal Breaker
	}
}

// Sliab Cuilin
// --------------------------------------------------------------------------

public class SliabSealStoneScript : _SealStoneScript
{
	public override void Init()
	{
		SetName("Seal Stone of Sliab Cuilin", "_sealstone_east_taillteann");
		SetLocation(16, 6336, 62882, 0);
		SetHelp("The Seal of Sliab Cuilin\n\nUtilize Tracy's Secret.");
	}
	
	public override bool Check(WorldClient c, MabiPC cr, MabiProp prop)
	{
		return (cr.LeftHand != null && cr.LeftHand.Info.Class == 1028); // Tracy's Secret
	}
	
	public override void OnBreak(MabiPC cr)
	{
		cr.Titles[10067] = true; // the Sliab Cuilin Seal Breaker
	}
}

// Tara
// --------------------------------------------------------------------------

public class TaraSealStoneScript : _SealStoneScript
{
	public override void Init()
	{
		SetName("Seal Stone of Tara", "_sealstone_tara");
		SetLocation(400, 56799, 33820, 2.23f);
		SetHelp("The Seal of Tara\n\nAlchemists only!!!");
	}
	
	public override bool Check(WorldClient c, MabiPC cr, MabiProp prop)
	{
		// Have alchemist clothes, shoes, a Cylinder, and Beginner Alchemist title equipped ?
		
		if(cr.Title != 26)
			return false;
		
		// Shoes
		var item = cr.GetItemInPocket(Pocket.Shoe);
		if(item == null || (item.Info.Class != 17138))
			return false;
		
		// Clothes
		item = cr.GetItemInPocket(Pocket.Armor);
		if(item == null || (item.Info.Class != 15351))
			return false;
		
		// Cylinder
		if(cr.RightHand != null)
		{
			if(cr.RightHand.Info.Class == 40258) return true;
			if(cr.RightHand.Info.Class == 40270) return true;
			if(cr.RightHand.Info.Class == 40284) return true;
			if(cr.RightHand.Info.Class == 40285) return true;
			if(cr.RightHand.Info.Class == 40286) return true;
			if(cr.RightHand.Info.Class == 40287) return true;
			if(cr.RightHand.Info.Class == 40296) return true;
		}
		
		return false;
	}
	
	public override void OnBreak(MabiPC cr)
	{
		cr.Titles[10077] = true; // the Tara Seal Breaker
	}
}

// Base Script
// --------------------------------------------------------------------------

public abstract class _SealStoneScript : BaseScript
{
	protected const bool AllowMultiple = true;
	
	protected string _name, _ident;
	protected uint _region, _x, _y;
	protected float _direction;
	protected uint _hits, _required = 10;
	protected string _help;
	protected bool _locked;

	public override void OnLoad()
	{
		Init();
	
		var stone = new MabiProp(_ident, "", "", 40000, _region, _x, _y, _direction);
		
		stone.State = "state1";
		
		SpawnProp(stone, OnHit);
	}
	
	public void OnHit(WorldClient c, MabiPC cr, MabiProp pr)
	{
		var character = cr as MabiPC;
		
		lock(_ident)
		{
			if(_hits > _required)
				return;
				
			if(_locked)
			{
				Send.Notice(c, "This seal stone cannot be broken yet.");
				return;
			}
			
			// You can only become breaker once officially.
			if(IsBreaker(character) && !AllowMultiple)
			{
				Send.Notice(c, "Unable to break the Seal.\nYou already hold the title of a Seal Breaker.");
				return;
			}
			
			// Fulfilling the requirements?
			if(!Check(c, character, pr))
			{
				Send.Notice(c, _help);
				return;
			}
			
			_hits++;
			
			bool update = false;
			
			// Done
			if(_hits == _required)
			{
				pr.State = "state3";
				pr.ExtraData = string.Format("<xml breaker_id=\"{0}\" breaker_name=\"{1}\"/>", cr.Id, cr.Name);
				
				OnBreak(character);
				
				Send.PropUpdate(pr);
				Send.RegionNotice(cr.Region, "{0} successfully broke {1} apart.", cr.Name, _name);
			}
			// Cracks after half.
			else if(_hits == Math.Floor(_required / 2f))
			{
				pr.State = "state2";
				
				Send.PropUpdate(pr);
				Send.RegionNotice(cr.Region, "{0} has started breaking {1} apart.", cr.Name, _name);
			}
		}
	}
	
	public void SetName(string name, string ident) { _name = name; _ident = ident; }
	public void SetLocation(uint region, uint x, uint y, float direction) { _region = region; _x = x; _y = y; _direction = direction; }
	public void SetHelp(string help) { _help = help; }
	public void SetLock(bool locked) { _locked = locked; }
	
	public bool IsBreaker(MabiPC cr)
	{
		if(cr.Titles.ContainsKey(10002) && cr.Titles[10002]) return true; // the Dugald Aisle Seal Breaker
		if(cr.Titles.ContainsKey(10003) && cr.Titles[10003]) return true; // the Ciar Seal Breaker
		if(cr.Titles.ContainsKey(10004) && cr.Titles[10004]) return true; // the Rabbie Seal Breaker
		if(cr.Titles.ContainsKey(10005) && cr.Titles[10005]) return true; // the Math Seal Breaker
		if(cr.Titles.ContainsKey(10006) && cr.Titles[10006]) return true; // the Bangor Seal Breaker
		if(cr.Titles.ContainsKey(10008) && cr.Titles[10008]) return true; // the Fiodh Seal Breaker
		if(cr.Titles.ContainsKey(10009) && cr.Titles[10009]) return true; // the South Emain Macha Seal Breaker
		if(cr.Titles.ContainsKey(10025) && cr.Titles[10025]) return true; // the North Emain Macha Seal Breaker
		if(cr.Titles.ContainsKey(10067) && cr.Titles[10067]) return true; // the Sliab Cuilin Seal Breaker
		if(cr.Titles.ContainsKey(10068) && cr.Titles[10068]) return true; // the Abb Neagh Seal Breaker
		if(cr.Titles.ContainsKey(10077) && cr.Titles[10077]) return true; // the Tara Seal Breaker
		
		return false;
	}
	
	public virtual void OnBreak(MabiPC cr)
	{ }
	
	public abstract void Init();
	public abstract bool Check(WorldClient c, MabiPC cr, MabiProp prop);
}
