// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Aura.Shared.Const;
using Aura.Shared.Util;
using Aura.World.Skills;
using Aura.World.World;
using System.Collections;
using Aura.Shared.Network;
using Aura.World.Util;
using Aura.World.Network;

namespace Aura.World.Scripting
{
	public enum AggroState : byte
	{
		None = 0,
		Notice = 1,
		Aggro = 2
	}

	public abstract class AIScript : CreatureScript
	{
		public delegate bool Trigger(out int totalConditions); // Returns T/F if conditions match. 
		//Out param is the number of conditions the trigger has, so we can always choose the moust specific one.
		public delegate IEnumerable Behavior();

		public bool Active { get; private set; }
		public uint MinimumActiveBeats { get; private set; }

		public new MabiNPC Creature { get { return base.Creature as MabiNPC; } set { base.Creature = value; } }

		protected class AiAction
		{
			public Trigger Trigger;
			public Behavior Behavior;

			public AiAction(Trigger t, Behavior b)
			{
				this.Trigger = t;
				this.Behavior = b;
			}
		}

		private IEnumerator _brain;
		private AiAction _currentAction;
		protected IEnumerator Brain { get { return _brain; } }
		protected AiAction CurrentAction { get { return _currentAction; } set { _currentAction = value; _brain = _currentAction == null ? null : _currentAction.Behavior().GetEnumerator(); } }
		protected List<AiAction> _actions = new List<AiAction>();
		protected uint _aggroRange = 500;
		protected AggroState _aggroState;

		private Timer _heartbeatTimer;

		// This controls the "speed" at which the AI can think.
		// If it's too long things like following a target becomes choppy,
		// because changing direction takes longer.
		private const int Heartbeat = 50;//ms

		/// <summary>
		/// To be executed when the script is added to the creature.
		/// </summary>
		public override void OnLoad()
		{
			_heartbeatTimer = new Timer(Heartbeat);
			_heartbeatTimer.AutoReset = true;
			_heartbeatTimer.Elapsed += new ElapsedEventHandler(OnHeartbeat);
			this.Definition();
		}

		/// <summary>
		/// Returns number of beats, that are required till
		/// the specified amount of milliseconds passed.
		/// </summary>
		/// <param name="ms"></param>
		/// <returns></returns>
		public int GetBeats(int ms)
		{
			return (ms / Heartbeat);
		}

		/// <summary>
		/// Called by the intern timer every 500ms.
		/// </summary>
		/// <param name="stateobj"></param>
		public void OnHeartbeat(object stateobj, ElapsedEventArgs e)
		{
			if (this.MinimumActiveBeats != 0)
				this.MinimumActiveBeats--;

			// TODO: try, catch, logging

			// Stop if there are no actions or no creature
			if (this.Creature == null && _actions.Count == 0)
				return;

			// Stop if there are no characters in range
			var inSight = WorldManager.Instance.GetPlayersInRange(this.Creature, 2900);
			if (inSight.Count < 1 && MinimumActiveBeats == 0)
			{
				if (this.Creature.Target != null)
				{
					ResetTarget();
				}
				this.Deactivate();
				return;
			}

			// Stop if creature is unable to do anything
			if (this.Creature.IsDead || this.Creature.IsStunned)
			{
				// TODO: Stun should later be a state.
				return;
			}

			if (this.Creature.Target != null)
			{
				if (this.Creature.Target.IsDead || !WorldManager.InRange(this.Creature, this.Creature.Target, 2900))
				{
					ResetTarget();
					WorldManager.Instance.Broadcast(new MabiPacket(Op.CombatSetTarget, Creature.Id).PutLong(0), SendTargets.Range, this.Creature);
					this.Creature.BattleExp = 0;
					Send.ChangesStance(this.Creature);
				}
			}

			if (this.Creature.Target != null && _aggroState == AggroState.None) // Mob was attacked
			{
				foreach (var a in Aggro())
					;

				CheckForInterrupt();
			}

			Think();
		}

		protected void ResetTarget()
		{
			this.Creature.Target = null;
			_aggroState = AggroState.None;
			CheckForInterrupt();
		}

		public abstract void Definition(); // TODO: Base implementation?

		protected virtual void DefineAction(Trigger t, Behavior b)
		{
			_actions.Add(new AiAction(t, b));
		}

		public void Activate(uint timeTillArrive)
		{
			if (!this.Active)
			{
				this.Active = true;
				_heartbeatTimer.Start();
			}

			this.MinimumActiveBeats = Math.Max(this.MinimumActiveBeats, timeTillArrive * (1000 / (uint)_heartbeatTimer.Interval));
		}

		public void Deactivate()
		{
			this.Active = false;
			_heartbeatTimer.Stop();
		}

		private void Think()
		{
			if (Brain == null || !Brain.MoveNext())
				this.SelectBehavior();

			var result = Brain.Current;

			if (result is bool && !(bool)result)
			{
				CurrentAction = null;
			}
		}

		protected void SelectBehavior()
		{
			var matches = GetPotentialActions();

			CurrentAction = matches.Count == 0 ? null : matches[rnd.Next(0, matches.Count)].Key;

			if (CurrentAction == null)
			{
				//Logger.Warning("AI " + this.Creature.RaceInfo.AI + " does not define a condition. Using wait instead"); // TODO: Dump state
				CurrentAction = new AiAction(null, NullBehavior);
			}
		}

		private IEnumerable NullBehavior()
		{
			foreach (var a in Wait(1))
				yield return a;
		}

		private List<KeyValuePair<AiAction, int>> GetPotentialActions()
		{
			var potential = new Dictionary<AiAction, int>();

			foreach (var t in _actions)
			{
				int n;
				if (t.Trigger(out n))
					potential.Add(t, n);
			}

			if (potential.Count == 0)
				return new List<KeyValuePair<AiAction, int>>();

			var max = potential.Max(kvp => kvp.Value);

			var matches = potential.Where(kvp => kvp.Value == max).ToList();
			return matches;
		}

		/// <summary>
		/// Checks to see if there are any better actions to execute.
		/// ***WILL RESET THE BRAIN IF IT FINDS ONE.***
		/// </summary>
		protected void CheckForInterrupt()
		{
			if (!GetPotentialActions().Exists(kvp => kvp.Key == CurrentAction))
				SelectBehavior();
		}


		// Built-in triggers --------------------------
		protected bool IsIdleTrigger(out int x)
		{
			x = 1;
			return _aggroState == AggroState.None;
		}

		protected bool OnNoticeTrigger(out int x)
		{
			x = 1;
			return _aggroState == AggroState.Notice;
		}

		protected bool OnAggroTrigger(out int x)
		{
			x = 1;
			return _aggroState == AggroState.Aggro;
		}

		// Built-in Behaviors --------------------------
		protected IEnumerable WalkTo(MabiVertex dest, bool wait)
		{
			var pos = this.Creature.GetPosition();

			// Check for collision, set destination 200 points before the
			// intersection, to prevent glitching through.
			MabiVertex intersection;
			if (WorldManager.Instance.FindCollision(this.Creature.Region, pos, dest, out intersection))
				dest = WorldManager.CalculatePosOnLine(pos, intersection, -200);

			this.Creature.Move(dest, true);

			//WorldManager.Instance.CreatureMove(this.Creature, pos, dest, true);

			while (wait && Creature.IsMoving)
			{
				CheckForInterrupt();
				yield return true;
			}
		}

		protected IEnumerable RunTo(MabiVertex dest, bool wait)
		{
			if (!this.Creature.IsDestination(dest))
			{
				var pos = this.Creature.Move(dest, false);

				//WorldManager.Instance.CreatureMove(this.Creature, pos, dest, false);

				while (wait && this.Creature.IsMoving)
				{
					CheckForInterrupt();
					yield return true;
				}
			}
		}

		protected IEnumerable Circle(MabiEntity center, bool clockwise, int radius, bool wait)
		{
			var centerPos = center.GetPosition();
			var myPos = this.Creature.GetPosition();

			var deltaX = (double)myPos.X - (double)centerPos.X;
			var deltaY = (double)myPos.Y - (double)centerPos.Y;

			var angle = Math.Atan2(deltaY, deltaX);

			angle += (clockwise ? -1 : 1) * rnd.NextDouble() * (Math.PI / 6);

			var x = (int)(Math.Cos(angle) * radius);
			var y = (int)(Math.Sin(angle) * radius);

			var dest = new MabiVertex(centerPos.X + x, centerPos.Y + y);

			foreach (var a in WalkTo(dest, wait))
				yield return a;
		}

		protected IEnumerable Wander(bool checkForNoticeWhileIdle, bool changeStanceOnNotice)
		{
			var pos = this.Creature.GetPosition();

			MabiVertex dest;

			if (!WorldManager.InRange(pos, this.Creature.AnchorPoint, 2000))
			{
				dest = new MabiVertex(this.Creature.AnchorPoint.X, this.Creature.AnchorPoint.Y);
			}
			else
			{
				do
				{
					var x = (uint)(pos.X + rnd.Next(-600, 600 + 1));
					var y = (uint)(pos.Y + rnd.Next(-600, 600 + 1));
					dest = new MabiVertex(x, y);
				} while (!WorldManager.InRange(pos, this.Creature.AnchorPoint, 2000));
			}

			foreach (var a in this.WalkTo(dest, true))
				yield return a;

			var waitTime = this.GetBeats(rnd.Next(5000, 10000));

			if (checkForNoticeWhileIdle)
			{
				var beats = 0;
				while (beats < waitTime)
				{
					beats++;
					yield return true;
					foreach (var a in this.TryNotice(true, changeStanceOnNotice))
					{
						beats++;
						yield return true;
					}
				}
			}
			else
			{
				foreach (var a in this.Wait(waitTime))
					yield return a;
			}
		}

		protected IEnumerable WalkPath(params MabiVertex[] points)
		{
			foreach (var p in points)
				foreach (var a in this.WalkTo(p, true))
					yield return a;
		}

		protected IEnumerable PrepareSkill(SkillConst skillId, bool wait, MabiPacket args)
		{
			MabiSkill skill; SkillHandler handler;
			SkillManager.CheckOutSkill(this.Creature, skillId, out skill, out handler);

			if (this.Creature.ActiveSkillId != 0)
				foreach (var a in CancelSkill())
					yield return a;

			this.Creature.ActiveSkillId = skill.Id;

			// Save cast time when preparation is finished.
			var castTime = skill.RankInfo.LoadTime;
			this.Creature.ActiveSkillPrepareEnd = DateTime.Now.AddMilliseconds(castTime);

			var r = handler.Prepare(this.Creature, skill, args, castTime);

			if ((r & SkillResults.Okay) == 0)
				yield return false;

			WorldManager.Instance.SharpMind(this.Creature, SharpMindStatus.Loading, skillId);

			System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(() =>
			{
				System.Threading.Thread.Sleep((int)castTime);
				if (Creature.ActiveSkillId == skill.Id)
				{
					handler.Ready(this.Creature, skill);
					WorldManager.Instance.SharpMind(this.Creature, SharpMindStatus.Loaded, skillId);
				}
			}));

			t.Start();

			yield return true;

			if (wait)
				foreach (var a in Wait(GetBeats((int)castTime)))
					yield return a;
		}

		protected IEnumerable CancelSkill()
		{
			if (this.Creature.ActiveSkillId != 0)
			{
				WorldManager.Instance.SharpMind(this.Creature, SharpMindStatus.Cancelling, this.Creature.ActiveSkillId);
				var handler = SkillManager.GetHandler(this.Creature.ActiveSkillId);

				var res = handler.Cancel(this.Creature, this.Creature.Skills.Get(this.Creature.ActiveSkillId));

				yield return (res & SkillResults.Okay) != 0;

				WorldManager.Instance.SharpMind(this.Creature, SharpMindStatus.None, this.Creature.ActiveSkillId);
			}
			yield return true;
		}

		protected IEnumerable Say(string text)
		{
			Send.Chat(this.Creature, text);
			yield return true;
		}

		protected IEnumerable Wait(int beats)
		{
			for (int i = 0; i < beats; i++)
			{
				CheckForInterrupt();
				yield return true;
			}
		}

		protected IEnumerable Attack()
		{
			if (Creature.Target == null || Creature.Target.IsDead)
				yield return false;

			var aResult = SkillResults.Failure;

			var handler = SkillManager.GetHandler(SkillConst.MeleeCombatMastery) as CombatMasteryHandler;

			try
			{
				if (handler != null)
					aResult = handler.Use(this.Creature, this.Creature.Target.Id);
			}
			catch
			{
				aResult = SkillResults.Failure;
			}

			if ((aResult & SkillResults.OutOfRange) != 0)
			{
				var targetPos = WorldManager.CalculatePosOnLine(this.Creature, this.Creature.Target, -40);
				foreach (var a in RunTo(targetPos, false))
					yield return a;
			}
			else
				yield return (aResult & SkillResults.Okay) != 0;
		}

		/// <summary>
		/// Sends the notice packet (!) and switches the aggro state to "Notice"
		/// </summary>
		/// <param name="showMark"></param>
		/// <returns></returns>
		protected IEnumerable TryNotice(bool showMark, bool changeStance)
		{
			var inRange = WorldManager.Instance.GetCreaturesInRange(this.Creature, _aggroRange).FindAll(c => c.IsAttackableBy(this.Creature));

			if (inRange.Count != 0)
			{
				// Select mob and aggro
				Creature.Target = inRange[rnd.Next(0, inRange.Count)];

				_aggroState = AggroState.Notice;
				if (showMark)
				{
					WorldManager.Instance.Broadcast(new MabiPacket(Op.CombatSetTarget, Creature.Id).PutLong(Creature.Target.Id).PutByte(1).PutString(""), SendTargets.Range, this.Creature);
					Send.Chat(this.Creature, "!");
				}
				if (changeStance)
				{
					this.Creature.BattleState = 1;
					Send.ChangesStance(this.Creature);
				}

				CheckForInterrupt();

				yield return true;
			}

			yield return false;
		}

		/// <summary>
		/// Sends the aggro packet (!!) and switches the aggro state to "Aggro". 
		/// </summary>
		/// <returns></returns>
		public IEnumerable Aggro()
		{
			if (Creature.Target == null || Creature.Target.IsDead)
				foreach (var a in TryNotice(false, false))
					yield return a;

			_aggroState = AggroState.Aggro;
			this.Creature.BattleState = 1;
			Send.ChangesStance(this.Creature);

			WorldManager.Instance.Broadcast(new MabiPacket(Op.CombatSetTarget, Creature.Id).PutLong(Creature.Target.Id).PutByte(2).PutString(""), SendTargets.Range, this.Creature);

			Send.Chat(this.Creature, "!!");

			CheckForInterrupt();

			yield return true;
		}
	}
}
