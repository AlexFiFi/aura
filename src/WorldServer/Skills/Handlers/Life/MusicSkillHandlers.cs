// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using Aura.Shared.Const;
using Aura.Shared.Network;
using Aura.Shared.Util;
using Aura.World.Network;
using Aura.World.Util;
using Aura.World.World;

namespace Aura.World.Skills
{
	/// <summary>
	/// Var 1: Melody max length
	/// Var 2: Harmony 1 max length
	/// Var 3: Harmony 2 max length
	/// Var 4: Magical Effect chance
	/// </summary>
	[SkillAttr(SkillConst.Composing)]
	public class ComposingHandler : SkillHandler
	{
		const int MMLMaxLength = 10000;

		public override SkillResults Prepare(MabiCreature creature, MabiSkill skill, MabiPacket packet, uint castTime)
		{
			var itemId = packet.GetLong();
			var title = packet.GetString();
			var author = packet.GetString();
			var mml = packet.GetString();
			var singing = packet.GetString(); // [180300, NA166 (18.09.2013)] Singing
			var hidden = packet.GetByte();

			var item = creature.GetItem(itemId);
			if (item == null)
				return SkillResults.Failure;

			var mmlParts = mml.Split(',');

			// Check total score length
			if (mml.Length > MMLMaxLength)
				return SkillResults.Failure;
			// Melody length
			if (mmlParts[0].Length > skill.RankInfo.Var1)
				return SkillResults.Failure;
			// Harmony 1 length
			if (mmlParts.Length > 1 && mmlParts[1].Length > skill.RankInfo.Var2)
				return SkillResults.Failure;
			// Harmony 2 length
			if (mmlParts.Length > 2 && mmlParts[2].Length > skill.RankInfo.Var3)
				return SkillResults.Failure;

			creature.Temp.SkillItem1 = item;

			var level = SkillRank.Novice;

			// Score level = Musical Knowledge rank
			var knowledge = creature.Skills.Get(SkillConst.MusicalKnowledge);
			if (knowledge != null)
				level = knowledge.Rank;

			item.Tags.SetString("TITLE", title);
			item.Tags.SetString("AUTHOR", author);
			item.Tags.SetString("SCORE", MabiZip.Compress(mml));
			item.Tags.SetString("SCSING", ""); // [180300, NA166 (18.09.2013)] Singing
			item.Tags.SetByte("HIDDEN", hidden);
			item.Tags.SetShort("LEVEL", (ushort)level);

			Send.SkillUse(creature.Client, creature, skill.Id, itemId);

			return SkillResults.Okay;
		}

		public override SkillResults Complete(MabiCreature creature, MabiSkill skill, MabiPacket packet)
		{
			if (creature.Temp.SkillItem1 == null)
				return SkillResults.Failure;

			Send.ItemUpdate(creature.Client, creature, creature.Temp.SkillItem1);
			Send.SkillComplete(creature.Client, creature, skill.Id, creature.Temp.SkillItem1.Id);

			creature.Temp.SkillItem1 = null;

			return SkillResults.Okay;
		}
	}

	[SkillAttr(SkillConst.PlayingInstrument)]
	public class PlayingInstrumentHandler : SkillHandler
	{
		// tmp
		const int RandomScoreMin = 1, RandomScoreMax = 52;
		const int DurabilityUse = 1000;

		public override SkillResults Prepare(MabiCreature creature, MabiSkill skill, MabiPacket packet, uint castTime)
		{
			var rnd = RandomProvider.Get();

			// Check for instrument
			if (creature.RightHand == null || creature.RightHand.Type != ItemType.Instrument)
				return SkillResults.Failure;

			// Score scrolls go into the magazine pocket and need a SCORE tag.
			// XXX: Is it possbile to play random with a low durability scroll?
			bool hasScroll = (creature.Magazine != null && creature.Magazine.Tags.Has("SCORE") && creature.Magazine.OptionInfo.Durability >= DurabilityUse);

			// Random score if no usable scroll was found.
			uint rndScore = (!hasScroll ? (uint)rnd.Next(RandomScoreMin, RandomScoreMax + 1) : 0);

			// Quality seems to go from 0 (worst) to 3 (best).
			// Should be generated based on skills + random.
			var quality = (PlayingQuality)rnd.Next((int)PlayingQuality.VeryBad, (int)PlayingQuality.VeryGood + 1);

			// Up quality by chance, based on Musical Knowledge
			var knowledge = creature.Skills.Get(SkillConst.MusicalKnowledge);
			if (knowledge != null && rnd.Next(0, 100) < knowledge.RankInfo.Var2)
				quality++;

			if (quality > PlayingQuality.VeryGood)
				quality = PlayingQuality.VeryGood;

			// Save quality before checking perfect play option,
			// we want proper skill training.
			creature.Temp.PlayingInstrumentQuality = quality;

			if (WorldConf.PerfectPlay)
			{
				quality = PlayingQuality.VeryGood;
				Send.ServerMessage(creature.Client, creature, Localization.Get("skills.perfect_play")); // Regardless of the result, perfect play will let your performance sound perfect.
			}

			// Reduce scroll's durability.
			if (hasScroll)
			{
				creature.Magazine.ReduceDurability(DurabilityUse);
				creature.Client.Send(
					new MabiPacket(Op.ItemDurabilityUpdate, creature.Id)
					.PutLong(creature.Magazine.Id)
					.PutInt(creature.Magazine.OptionInfo.Durability)
				);
			}

			// Music effect
			{
				var effect = hasScroll
					? PacketCreator.PlayEffect(creature, creature.RightHand.DataInfo.Instrument, quality, creature.Magazine.Tags["SCORE"])
					: PacketCreator.PlayEffect(creature, creature.RightHand.DataInfo.Instrument, quality, rndScore);
				WorldManager.Instance.Broadcast(effect, SendTargets.Range, creature);
			}

			// Use skill
			{
				var use = new MabiPacket(Op.SkillUse, creature.Id);
				use.PutShort(skill.Info.Id);
				use.PutLong(0);
				use.PutByte(hasScroll);
				if (!hasScroll)
					use.PutInt(rndScore);
				else
					use.PutString(creature.Magazine.Tags["SCORE"]);
				use.PutByte((byte)creature.RightHand.DataInfo.Instrument);
				use.PutByte(1);
				use.PutByte(0);
				creature.Client.Send(use);

				creature.ActiveSkillId = skill.Id;
			}

			// Change motion for Battle Mandolin (no idea if this official, but I like it =P) [exec]
			//if (creature.RightHand.Info.Class == 40367)
			//    WorldManager.Instance.CreatureUseMotion(creature, 88, 2, true);

			return SkillResults.Okay;
		}

		public override SkillResults Complete(MabiCreature creature, MabiSkill skill, MabiPacket packet)
		{
			// Stop effect
			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.StopMusic), SendTargets.Range, creature);

			// Result notice
			Send.Notice(creature.Client, this.GetRandomComplete(creature.Temp.PlayingInstrumentQuality));

			// skill training

			Send.SkillComplete(creature.Client, creature, skill.Id);

			return SkillResults.Okay;
		}

		public override SkillResults Cancel(MabiCreature creature, MabiSkill skill)
		{
			// Stop effect
			WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, creature.Id).PutInt(Effect.StopMusic), SendTargets.Range, creature);

			return SkillResults.Okay;
		}

		/// <summary>
		/// Returns a random result message for the given quality.
		/// </summary>
		/// <param name="quality"></param>
		/// <returns></returns>
		private string GetRandomComplete(PlayingQuality quality)
		{
			string[] msgs = null;
			switch (quality)
			{
				// Messages are stored in one line per quality, seperated by semicolons.
				case PlayingQuality.VeryGood: msgs = Localization.Get("skills.quality_verygood").Split(';'); break;
				case PlayingQuality.Good: msgs = Localization.Get("skills.quality_good").Split(';'); break;
				case PlayingQuality.Bad: msgs = Localization.Get("skills.quality_bad").Split(';'); break;
				case PlayingQuality.VeryBad: msgs = Localization.Get("skills.quality_verybad").Split(';'); break;
			}

			if (msgs.Length < 1)
				return "...";

			return msgs[RandomProvider.Get().Next(0, msgs.Length)].Trim();
		}
	}
}
