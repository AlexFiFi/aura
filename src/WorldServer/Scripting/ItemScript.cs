// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Aura.Shared.Util;
using Aura.World.World;

namespace Aura.World.Scripting
{
	public abstract class ItemScript : BaseScript
	{
		public virtual void OnUse(MabiCreature cr, MabiItem i)
		{ }

		public virtual void OnEquip(MabiCreature cr, MabiItem i)
		{ }

		public virtual void OnUnequip(MabiCreature cr, MabiItem i)
		{ }

		// Functions
		// ------------------------------------------------------------------

		protected void Heal(MabiCreature creature, double life, double mana, double stamina)
		{
			creature.Life += (float)life;
			creature.Mana += (float)mana;
			creature.Stamina += (float)stamina;
		}

		protected void Treat(MabiCreature creature, double injuries)
		{
			creature.Injuries -= (float)injuries;
		}

		protected void FullHeal(MabiCreature creature)
		{
			creature.Injuries = 0;
			creature.Hunger = 0;
			creature.Life = creature.LifeMax;
			creature.Mana = creature.ManaMax;
			creature.Stamina = creature.StaminaMax;
		}

		protected void Poison(MabiCreature creature, double foodPoison)
		{
			//creature.X += (float)foodPoison;
		}

		protected void Feed(MabiCreature creature, double hunger)
		{
			creature.Hunger -= (float)hunger;
		}
	}
}
