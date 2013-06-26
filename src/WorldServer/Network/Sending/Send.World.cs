// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aura.Shared.Network;
using Aura.World.World;
using Aura.Shared.Const;
using Aura.Shared.Util;
using Aura.World.Player;
using Aura.World.Events;

namespace Aura.World.Network
{
	public static partial class Send
	{
		/// <summary>
		/// Broadcasts Phoenix Feather above dead creature effect in creature's range.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="creature"></param>
		public static void DeadFeather(MabiCreature creature, DeadMenuOptions options)
		{
			var bits = (uint)options;
			var flags = new List<uint>();

			// Break down options bit by bit, and add them to flags if set.
			for (uint i = 1; bits != 0; ++i, bits >>= 1)
			{
				if ((bits & 1) != 0)
					flags.Add(i);
			}

			var packet = new MabiPacket(Op.DeadFeather, creature.Id);

			packet.PutShort((ushort)flags.Count);
			foreach (var f in flags)
				packet.PutInt(f);

			packet.PutByte(0);

			WorldManager.Instance.Broadcast(packet, SendTargets.Range, creature);
		}

		public static void Chat(MabiCreature creature, string format, params object[] args)
		{
			Chat(creature, 0, format, args);
		}

		public static void Chat(MabiCreature creature, byte type, string format, params object[] args)
		{
			var packet = new MabiPacket(Op.Chat, creature.Id);
			packet.PutByte(type);
			packet.PutString(creature.Name);
			packet.PutString(format, args);

			WorldManager.Instance.Broadcast(packet, SendTargets.Range, creature);
		}

		/// <summary>
		/// Sends whisper chat to both clients.
		/// </summary>
		/// <param name="sourceClient"></param>
		/// <param name="targetClient"></param>
		/// <param name="target"></param>
		/// <param name="sender"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public static void Whisper(Client sourceClient, Client targetClient, MabiCreature target, string sender, string format, params object[] args)
		{
			var packet = new MabiPacket(Op.WhisperChat, target.Id);
			packet.PutStrings(sender);
			packet.PutString(format, args);

			if (sourceClient != null)
				sourceClient.Send(packet);
			if (targetClient != null)
				targetClient.Send(packet);
		}

		/// <summary>
		/// Broadcasts motion use. Also sends MotionCancel, is cancel is true.
		/// </summary>
		/// <param name="creature"></param>
		/// <param name="category"></param>
		/// <param name="type"></param>
		/// <param name="loop"></param>
		/// <param name="cancel"></param>
		public static void UseMotion(MabiCreature creature, uint category, uint type, bool loop = false, bool cancel = true)
		{
			if (cancel)
			{
				// Cancel motion
				var cancelPacket = new MabiPacket(Op.MotionCancel, creature.Id);
				cancelPacket.PutByte(0);
				WorldManager.Instance.Broadcast(cancelPacket, SendTargets.Range, creature);
			}

			// Do motion
			var doPacket = new MabiPacket(Op.UseMotion, creature.Id);
			doPacket.PutInt(category);
			doPacket.PutInt(type);
			doPacket.PutByte(loop);
			doPacket.PutShort(0);

			WorldManager.Instance.Broadcast(doPacket, SendTargets.Range, creature);
		}

		/// <summary>
		/// Broadcasts appear in range of entity.
		/// </summary>
		/// <param name="entity"></param>
		public static void EntityAppears(MabiEntity entity)
		{
			WorldManager.Instance.Broadcast(GetEntityAppears(entity), SendTargets.Range, entity);
		}

		/// <summary>
		/// Broadcasts appear to everybody in range, except for entity itself.
		/// </summary>
		/// <param name="entity"></param>
		public static void EntityAppearsOthers(MabiEntity entity)
		{
			WorldManager.Instance.Broadcast(GetEntityAppears(entity), SendTargets.Range | SendTargets.ExcludeSender, entity);
		}

		/// <summary>
		/// Sends appear to client.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="entity"></param>
		public static void EntityAppears(Client client, MabiEntity entity)
		{
			client.Send(GetEntityAppears(entity));
		}

		private static MabiPacket GetEntityAppears(MabiEntity entity)
		{
			var op = Op.EntityAppears;
			if (entity.EntityType == EntityType.Item)
				op = Op.ItemAppears;
			else if (entity.EntityType == EntityType.Prop)
				op = Op.PropAppears;

			var packet = new MabiPacket(op, Id.Broadcast);
			packet.AddPublicEntityInfo(entity);
			//entity.AddToPacket(packet);

			return packet;
		}

		/// <summary>
		/// Broadcasts disappear in range of entity.
		/// </summary>
		/// <param name="entity"></param>
		public static void EntityDisappears(MabiEntity entity)
		{
			WorldManager.Instance.Broadcast(GetEntityDisappears(entity), SendTargets.Range, entity);
		}

		/// <summary>
		/// Sends dispappear to client.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="entity"></param>
		public static void EntityDisappears(Client client, MabiEntity entity)
		{
			client.Send(GetEntityDisappears(entity));
		}

		private static MabiPacket GetEntityDisappears(MabiEntity entity)
		{
			uint op = Op.EntityDisappears;
			if (entity is MabiItem)
				op = Op.ItemDisappears;

			var packet = new MabiPacket(op, Id.Broadcast);
			packet.PutLong(entity.Id);
			packet.PutByte(0);

			return packet;
		}

		public static void EntitiesAppear(Client client, IEnumerable<MabiEntity> entities)
		{
			var packet = new MabiPacket(Op.EntitiesAppear, Id.Broadcast);

			packet.PutShort((ushort)entities.Count());
			foreach (var entity in entities)
			{
				var data = new MabiPacket(0, 0);
				data.AddPublicEntityInfo(entity);
				var dataBytes = data.Build(false);

				packet.PutShort(entity.DataType);
				packet.PutInt((uint)dataBytes.Length);
				packet.PutBin(dataBytes);
			}

			client.Send(packet);
		}

		/// <summary>
		/// Sends list of disappearing entites to client.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="entities"></param>
		public static void EntitiesDisappear(Client client, IEnumerable<MabiEntity> entities)
		{
			var packet = new MabiPacket(Op.EntitiesDisappear, Id.Broadcast);

			packet.PutShort((ushort)entities.Count());
			foreach (var entity in entities)
			{
				packet.PutShort(entity.DataType);
				packet.PutLong(entity.Id);
			}

			client.Send(packet);
		}

		/// <summary>
		/// Broadcasts RunTo. If to is null, the creature's position is used.
		/// </summary>
		/// <param name="wm"></param>
		/// <param name="creature"></param>
		public static void RunTo(MabiCreature creature, MabiVertex to = null)
		{
			var pos = creature.GetPosition();

			var p = new MabiPacket(Op.RunTo, creature.Id);
			p.PutInts(pos.X, pos.Y); // From
			p.PutInts(pos.X, pos.Y); // To
			p.PutBytes(1, 0);

			WorldManager.Instance.Broadcast(p, SendTargets.Range, creature);
		}

		/// <summary>
		/// Broadcasts WalkTo. If to is null, the creature's position is used.
		/// </summary>
		/// <param name="wm"></param>
		/// <param name="creature"></param>
		public static void WalkTo(MabiCreature creature, MabiVertex to = null)
		{
			var pos = creature.GetPosition();

			var p = new MabiPacket(Op.WalkTo, creature.Id);
			p.PutInts(pos.X, pos.Y); // From
			p.PutInts(pos.X, pos.Y); // To
			p.PutBytes(1, 0);

			WorldManager.Instance.Broadcast(p, SendTargets.Range, creature);
		}

		/// <summary>
		/// Broadcasts prop update in its region.
		/// </summary>
		/// <param name="prop"></param>
		public static void PropUpdate(MabiProp prop)
		{
			var packet = new MabiPacket(Op.PropUpdate, prop.Id);
			packet.AddPropUpdateInfo(prop);

			WorldManager.Instance.BroadcastRegion(packet, prop.Region);
		}

		/// <summary>
		/// Broadcasts update packet for Title and OptionTitle.
		/// </summary>
		/// <param name="creature"></param>
		public static void TitleUpdate(MabiCreature creature)
		{
			var packet = new MabiPacket(Op.TitleUpdate, creature.Id);
			packet.PutShort(creature.Title);
			packet.PutShort(creature.OptionTitle);

			WorldManager.Instance.Broadcast(packet, SendTargets.Range, creature);
		}

		public static void CombatTargetSet(MabiCreature creature, MabiCreature target)
		{
			var packet = new MabiPacket(Op.CombatTargetSet, creature.Id);
			packet.PutLong(target != null ? target.Id : 0);

			WorldManager.Instance.Broadcast(packet, SendTargets.Range, creature);
		}

		public static void SitDown(MabiCreature creature)
		{
			var packet = new MabiPacket(Op.SitDown, creature.Id);
			packet.PutByte(creature.RestPose);

			WorldManager.Instance.Broadcast(packet, SendTargets.Range, creature);
		}

		public static void StandUp(MabiCreature creature)
		{
			var packet = new MabiPacket(Op.StandUp, creature.Id);
			packet.PutByte(1);

			WorldManager.Instance.Broadcast(packet, SendTargets.Range, creature);
		}

		public static void ChangesStance(MabiCreature creature, byte unk = 1)
		{
			var packet = new MabiPacket(Op.ChangesStance, creature.Id);
			packet.PutByte(creature.BattleState);
			packet.PutByte(unk);

			WorldManager.Instance.Broadcast(packet, SendTargets.Range, creature);
		}

		public static void EquipmentMoved(MabiCreature creature, Pocket from)
		{
			var packet = new MabiPacket(Op.EquipmentMoved, creature.Id);
			packet.PutByte((byte)from);
			packet.PutByte(1);

			WorldManager.Instance.Broadcast(packet, SendTargets.Range, creature);

			// TODO: !
			WorldManager.Instance.CreatureStatsUpdate(creature);
		}

		public static void EquipmentChanged(MabiCreature creature, MabiItem item)
		{
			var packet = new MabiPacket(Op.EquipmentChanged, creature.Id);
			packet.PutBin(item.Info);
			packet.PutByte(1);

			WorldManager.Instance.Broadcast(packet, SendTargets.Range, creature);

			// TODO: !
			WorldManager.Instance.CreatureStatsUpdate(creature);
		}
	}
}
