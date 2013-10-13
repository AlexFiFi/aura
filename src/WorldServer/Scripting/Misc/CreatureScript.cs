// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using Aura.Data;
using Aura.Shared.Util;
using Aura.World.Network;
using Aura.World.Util;
using Aura.World.World;
using Aura.World.Events;
using Aura.Shared.Network;
using Aura.Shared.Const;
using Aura.Shared.World;

namespace Aura.World.Scripting
{
	public class CreatureScript : BaseScript
	{
		public MabiCreature Creature;

		protected virtual void SetLocation(string region, uint x, uint y)
		{
			this.SetLocation(region, x, y, this.Creature.Direction);
		}

		protected virtual void SetLocation(string region, uint x, uint y, byte direction)
		{
			uint regionid = 0;
			if (!uint.TryParse(region, out regionid))
			{
				var mapInfo = MabiData.RegionDb.Find(region);
				if (mapInfo != null)
					regionid = mapInfo.Id;
				else
				{
					Logger.Warning("{0} : Map '{1}' not found.", this.ScriptName, region);
				}
			}

			this.SetLocation(regionid, x, y, direction);
		}

		protected virtual void SetLocation(uint region, uint x, uint y)
		{
			this.SetLocation(region, x, y, this.Creature.Direction);
		}

		protected virtual void SetLocation(uint region, uint x, uint y, byte direction)
		{
			this.Creature.Region = region;
			this.Creature.SetPosition(x, y);
			this.SetDirection(direction);
		}

		protected virtual void SetBody(float height = 1.0f, float fat = 1.0f, float lower = 1.0f, float upper = 1.0f)
		{
			this.Creature.Height = height;
			this.Creature.Weight = fat;
			this.Creature.Lower = lower;
			this.Creature.Upper = upper;
		}

		protected virtual void SetFace(byte skin, byte eye, byte eyeColor, byte lip)
		{
			this.Creature.SkinColor = skin;
			this.Creature.EyeType = eye;
			this.Creature.EyeColor = eyeColor;
			this.Creature.Mouth = lip;
		}

		protected virtual void SetColor(uint c1 = 0x808080, uint c2 = 0x808080, uint c3 = 0x808080)
		{
			Creature.Color1 = c1;
			Creature.Color2 = c2;
			Creature.Color3 = c3;
		}

		protected virtual void SetDirection(byte direction)
		{
			this.Creature.Direction = direction;
		}

		protected virtual void SetStand(string style, string talkStyle = "")
		{
			this.Creature.StandStyle = style;
			this.Creature.StandStyleTalk = talkStyle;
		}

		protected virtual void SetId(ulong id)
		{
			this.Creature.Id = id;
		}

		protected virtual void SetName(string name)
		{
			this.Creature.Name = name;
		}

		protected virtual void SetRace(uint race)
		{
			this.Creature.Race = race;
		}

		protected void WarpNPC(uint region, uint x, uint y, bool flash = true)
		{
			if (flash)
			{
				Send.Effect(Effect.ScreenFlash, this.Creature, 3000u, 0u);
				Send.PlaySound("data/sound/Tarlach_change.wav", this.Creature);
			}

			WorldManager.Instance.CreatureLeaveRegion(this.Creature);
			SetLocation(region, x, y);

			if (flash)
			{
				Send.Effect(Effect.ScreenFlash, this.Creature, 3000u, 0u);
				Send.PlaySound("data/sound/Tarlach_change.wav", this.Creature);
			}

			Send.EntityAppears(this.Creature);
		}

		protected virtual void EquipItem(Pocket pocket, uint itemClass, uint color1 = 0, uint color2 = 0, uint color3 = 0)
		{
			if (!pocket.IsEquip())
				return;

			var item = new MabiItem(itemClass);
			item.Info.ColorA = color1;
			item.Info.ColorB = color2;
			item.Info.ColorC = color3;

			//var inPocket = this.Creature.GetItemInPocket(slot);
			//if (inPocket != null)
			//    this.Creature.Items.Remove(inPocket);

			this.Creature.Inventory.ForceAdd(item, pocket);

			Send.EquipmentChanged(this.Creature, item);
		}

		protected void SetHoodDown()
		{
			var item = this.Creature.Inventory.GetItemAt(Pocket.Robe, 0, 0);
			if (item != null)
				item.Info.FigureA = 0;
			item = this.Creature.Inventory.GetItemAt(Pocket.RobeStyle, 0, 0);
			if (item != null)
				item.Info.FigureA = 0;
		}

		protected virtual void EquipItem(Pocket slot, string itemName, uint color1 = 0, uint color2 = 0, uint color3 = 0)
		{
			var dbInfo = MabiData.ItemDb.Find(itemName);
			if (dbInfo == null)
			{
				Logger.Warning("Unknown item '" + itemName + "' cannot be eqipped. Try specifying the ID manually.");
				return;
			}

			this.EquipItem(slot, dbInfo.Id, color1, color2, color3);
		}
	}
}
