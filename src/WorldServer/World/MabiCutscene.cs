using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aura.Shared.Const;
using Aura.Shared.Network;
using Aura.World.Network;

namespace Aura.World.World
{
	public class MabiCutscene
	{
		private MabiCreature _leader;
		private string _name;
		private List<Tuple<string, byte[]>> _actors = new List<Tuple<string, byte[]>>();

		public Action<WorldClient> OnComplete = null;

		public MabiCutscene(MabiCreature leader, string cutsceneName)
		{
			_leader = leader;
			_name = cutsceneName;
		}

		public void AddActor(string name, MabiCreature creature)
		{
			var p = new MabiPacket(0);
			creature.AddToPacket(p);
			AddActor(name, p.Build(false));
		}

		public void AddActor(string name, byte[] creatureData)
		{
			_actors.Add(new Tuple<string, byte[]>(name, creatureData));
		}

		public bool IsLeader(MabiCreature creature)
		{
			return _leader == creature;
		}

		public void Send(WorldClient client)
		{
			WorldManager.Instance.Broadcast(PacketCreator.EntityLeaves(client.Character), SendTargets.Range, client.Character);
			client.Send(PacketCreator.EntitiesLeave(WorldManager.Instance.GetCreaturesInRange(client.Character)));
			client.Send(PacketCreator.Lock(client.Character));

			var p = new MabiPacket(Op.CutsceneStart, Id.World);
			p.PutLongs(client.Character.Id, _leader.Id);
			p.PutString(_name);
			p.PutInt((uint)_actors.Count);
			foreach (var c in _actors)
			{
				p.PutString(c.Item1);
				p.PutShort((ushort)c.Item2.Length);
				p.PutBin(c.Item2);
			}

			p.PutInt(1);
			p.PutLong(client.Character.Id);

			client.Character.CurrentCutscene = this;

			client.Send(p);
		}

		public static class Predefined
		{
			private static MabiCreature _brownFox;
			public static MabiCreature BrownFox
			{
				get
				{
					if (_brownFox != null)
						return _brownFox;

					_brownFox = new MabiNPC();
					_brownFox.Name = "#brownfox";
					_brownFox.Race = 50001;
					_brownFox.Color1 = 0xA07070;
					_brownFox.Color2 = _brownFox.Color3 = 0;
					_brownFox.Life = 15;
					_brownFox.LifeMaxBase = 15;
					_brownFox.LifeMaxMod = 15;

					return _brownFox;
				}
			}

			private static MabiCreature _morrighanG1;
			public static MabiCreature MorrighanG1
			{
				get
				{
					if (_morrighanG1 != null)
						return _morrighanG1;

					_morrighanG1 = new MabiNPC();
					_morrighanG1.Name = "#morrighan";
					_morrighanG1.Race = 2;
					_morrighanG1.SkinColor = 17;
					_morrighanG1.Eye = 3;
					_morrighanG1.EyeColor = 27;
					_morrighanG1.Lip = 0;
					_morrighanG1.State = (CreatureStates)2684358656;
					_morrighanG1.Life = 100;
					_morrighanG1.LifeMaxBase = 100;
					_morrighanG1.LifeMaxMod = 0;

					return _morrighanG1;
				}
			}

			private static MabiCreature _tarlachG1;
			public static MabiCreature TarlachG1
			{
				get
				{
					if (_tarlachG1 != null)
						return _tarlachG1;

					_tarlachG1 = new MabiNPC();
					_tarlachG1.Name = "#tarlach";
					_tarlachG1.Race = 10002;
					_tarlachG1.SkinColor = 15;
					_tarlachG1.Eye = 4;
					_tarlachG1.EyeColor = 54;
					_tarlachG1.Lip = 0;
					_tarlachG1.State = (CreatureStates)4096;
					_tarlachG1.StateEx = 0;
					_tarlachG1.Height = 1.1f;
					_tarlachG1.Fat = 0.001f;
					_tarlachG1.Upper = 0.4f;
					_tarlachG1.Lower = 0.6f;
					_tarlachG1.Color1 = 0x0;
					_tarlachG1.Color2 = 0x0;
					_tarlachG1.Color3 = 0x0;
					_tarlachG1.StandStyle = "";
					_tarlachG1.Life = 302;
					_tarlachG1.LifeMaxBase = 252;
					_tarlachG1.LifeMaxMod = 50;
					_tarlachG1.Title = 0;
					var item0 = new MabiItem(4901);
					item0.Info.Amount = 0;
					item0.Info.ColorA = 8592782;
					item0.Info.ColorB = 16019521;
					item0.Info.ColorC = 5740343;
					item0.Info.FigureA = 0;
					item0.Info.KnockCount = 2;
					item0.Info.Pocket = 3;
					item0.Info.Region = 0;
					_tarlachG1.Items.Add(item0);
					var item1 = new MabiItem(4021);
					item1.Info.Amount = 0;
					item1.Info.ColorA = 14531456;
					item1.Info.ColorB = 14531456;
					item1.Info.ColorC = 14531456;
					item1.Info.FigureA = 0;
					item1.Info.KnockCount = 2;
					item1.Info.Pocket = 4;
					item1.Info.Region = 0;
					_tarlachG1.Items.Add(item1);
					var item2 = new MabiItem(15069);
					item2.Info.Amount = 0;
					item2.Info.ColorA = 16777215;
					item2.Info.ColorB = 16777215;
					item2.Info.ColorC = 16777215;
					item2.Info.FigureA = 0;
					item2.Info.KnockCount = 0;
					item2.Info.Pocket = 5;
					_tarlachG1.Items.Add(item2);
					var item3 = new MabiItem(17032);
					item3.Info.Amount = 0;
					item3.Info.ColorA = 5648913;
					item3.Info.ColorB = 16571605;
					item3.Info.ColorC = 8235060;
					item3.Info.FigureA = 0;
					item3.Info.KnockCount = 0;
					item3.Info.Pocket = 7;
					_tarlachG1.Items.Add(item3);
					var item4 = new MabiItem(18028);
					item4.Info.Amount = 0;
					item4.Info.ColorA = 6446916;
					item4.Info.ColorB = 12632256;
					item4.Info.ColorC = 6296681;
					item4.Info.FigureA = 0;
					item4.Info.KnockCount = 0;
					item4.Info.Pocket = 8;
					_tarlachG1.Items.Add(item4);
					var item5 = new MabiItem(19004);
					item5.Info.Amount = 0;
					item5.Info.ColorA = 13269812;
					item5.Info.ColorB = 11817009;
					item5.Info.ColorC = 14335111;
					item5.Info.FigureA = 1;
					item5.Info.KnockCount = 0;
					item5.Info.Pocket = 9;
					_tarlachG1.Items.Add(item5);
					var item6 = new MabiItem(40017);
					item6.Info.Amount = 0;
					item6.Info.ColorA = 14336956;
					item6.Info.ColorB = 11194312;
					item6.Info.ColorC = 11627338;
					item6.Info.KnockCount = 2;
					item6.Info.Pocket = 11;
					_tarlachG1.Items.Add(item6);

					return _tarlachG1;
				}
			}

			private static MabiCreature _ruairiG1;
			public static MabiCreature RuairiG1
			{
				get
				{
					if (_ruairiG1 != null)
						return _ruairiG1;

					_ruairiG1 = new MabiNPC();
					_ruairiG1.Name = "#ruairi";
					_ruairiG1.Race = 10002;
					_ruairiG1.SkinColor = 17;
					_ruairiG1.Eye = 12;
					_ruairiG1.EyeColor = 37;
					_ruairiG1.Lip = 13;
					_ruairiG1.State = (CreatureStates)4096;
					_ruairiG1.Height = 1.3f;
					_ruairiG1.Fat = 1;
					_ruairiG1.Upper = 1.3f;
					_ruairiG1.Lower = 1;
					_ruairiG1.Color1 = 0x0;
					_ruairiG1.Color2 = 0x0;
					_ruairiG1.Color3 = 0x0;
					_ruairiG1.StandStyle = "";
					_ruairiG1.Life = 454;
					_ruairiG1.LifeMaxBase = 354;
					_ruairiG1.LifeMaxMod = 100;
					_ruairiG1.Title = 0;
					var item0 = new MabiItem(4900);
					item0.Info.Amount = 0;
					item0.Info.ColorA = 3193476;
					item0.Info.ColorB = 12565859;
					item0.Info.ColorC = 14216409;
					item0.Info.FigureA = 0;
					item0.Info.KnockCount = 2;
					item0.Info.Pocket = 3;
					_ruairiG1.Items.Add(item0);
					var item1 = new MabiItem(4029);
					item1.Info.Amount = 0;
					item1.Info.ColorA = 268435494;
					item1.Info.ColorB = 268435494;
					item1.Info.ColorC = 268435494;
					item1.Info.FigureA = 0;
					item1.Info.KnockCount = 2;
					item1.Info.Pocket = 4;
					_ruairiG1.Items.Add(item1);
					var item2 = new MabiItem(13021);
					item2.Info.Amount = 0;
					item2.Info.ColorA = 8421504;
					item2.Info.ColorB = 8421504;
					item2.Info.ColorC = 8421504;
					item2.Info.FigureA = 0;
					item2.Info.KnockCount = 0;
					item2.Info.Pocket = 5;
					_ruairiG1.Items.Add(item2);
					var item3 = new MabiItem(16508);
					item3.Info.Amount = 0;
					item3.Info.ColorA = 8421504;
					item3.Info.ColorB = 8421504;
					item3.Info.ColorC = 8421504;
					item3.Info.FigureA = 0;
					item3.Info.KnockCount = 0;
					item3.Info.Pocket = 6;
					_ruairiG1.Items.Add(item3);
					var item4 = new MabiItem(17509);
					item4.Info.Amount = 0;
					item4.Info.ColorA = 8421504;
					item4.Info.ColorB = 8421504;
					item4.Info.ColorC = 8421504;
					item4.Info.FigureA = 0;
					item4.Info.KnockCount = 0;
					item4.Info.Pocket = 7;
					_ruairiG1.Items.Add(item4);
					var item5 = new MabiItem(40028);
					item5.Info.Amount = 0;
					item5.Info.ColorA = 8421504;
					item5.Info.ColorB = 8421504;
					item5.Info.ColorC = 8421504;
					item5.Info.FigureA = 0;
					item5.Info.KnockCount = 1;
					item5.Info.Pocket = 10;
					_ruairiG1.Items.Add(item5);

					return _ruairiG1;
				}
			}

			private static MabiCreature _mariG1;
			public static MabiCreature MariG1
			{
				get
				{
					if (_mariG1 != null)
						return _mariG1;

					_mariG1 = new MabiNPC();
					_mariG1.Name = "#mari";
					_mariG1.Race = 10001;
					_mariG1.SkinColor = 17;
					_mariG1.Eye = 0;
					_mariG1.EyeColor = 155;
					_mariG1.Lip = 1;
					_mariG1.State = (CreatureStates)4096;
					_mariG1.StateEx = 0;
					_mariG1.Height = -0.2f;
					_mariG1.Fat = 1.2f;
					_mariG1.Upper = 1;
					_mariG1.Lower = 1.2f;
					_mariG1.Color1 = 0x0;
					_mariG1.Color2 = 0x0;
					_mariG1.Color3 = 0x0;
					_mariG1.StandStyle = "";
					_mariG1.Life = 269;
					_mariG1.LifeMaxBase = 199;
					_mariG1.LifeMaxMod = 70;
					_mariG1.Title = 0;
					var item0 = new MabiItem(3900);
					item0.Info.Amount = 0;
					item0.Info.ColorA = 7378227;
					item0.Info.ColorB = 7231332;
					item0.Info.ColorC = 41376;
					item0.Info.FigureA = 0;
					item0.Info.KnockCount = 2;
					item0.Info.Pocket = 3;
					_mariG1.Items.Add(item0);
					var item1 = new MabiItem(3028);
					item1.Info.Amount = 0;
					item1.Info.ColorA = 16754293;
					item1.Info.ColorB = 16754293;
					item1.Info.ColorC = 16754293;
					item1.Info.FigureA = 0;
					item1.Info.KnockCount = 2;
					item1.Info.Pocket = 4;
					_mariG1.Items.Add(item1);
					var item2 = new MabiItem(15054);
					item2.Info.Amount = 0;
					item2.Info.ColorA = 8421504;
					item2.Info.ColorB = 8421504;
					item2.Info.ColorC = 8421504;
					item2.Info.FigureA = 0;
					item2.Info.KnockCount = 0;
					item2.Info.Pocket = 5;
					_mariG1.Items.Add(item2);
					var item3 = new MabiItem(17015);
					item3.Info.Amount = 0;
					item3.Info.ColorA = 8081712;
					item3.Info.ColorB = 15119571;
					item3.Info.ColorC = 22913;
					item3.Info.FigureA = 0;
					item3.Info.KnockCount = 0;
					item3.Info.Pocket = 7;
					_mariG1.Items.Add(item3);
					var item4 = new MabiItem(40029);
					item4.Info.Amount = 0;
					item4.Info.ColorA = 7629908;
					item4.Info.ColorB = 11315480;
					item4.Info.ColorC = 14659750;
					item4.Info.FigureA = 0;
					item4.Info.KnockCount = 2;
					item4.Info.Pocket = 10;
					_mariG1.Items.Add(item4);
					var item5 = new MabiItem(45001);
					item5.Info.Amount = 100;
					item5.Info.ColorA = 24129;
					item5.Info.ColorB = 10044466;
					item5.Info.ColorC = 16569951;
					item5.Info.FigureA = 0;
					item5.Info.KnockCount = 2;
					item5.Info.Pocket = 14;
					_mariG1.Items.Add(item5);

					return _mariG1;
				}
			}
		}
	}
}
