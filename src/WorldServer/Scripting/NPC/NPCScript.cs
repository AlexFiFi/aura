// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Aura.Shared.Const;
using Aura.Data;
using Aura.Shared.Network;
using Aura.Shared.Util;
using Aura.World.Network;
using Aura.World.World;
using Aura.World.Events;
using System.Web;
using System.Text.RegularExpressions;

namespace Aura.World.Scripting
{
	public enum NPCLoadType { Real = 1, Virtual = 2 }

	public abstract class NPCScript : CreatureScript
	{
		public MabiNPC NPC { get { return this.Creature as MabiNPC; } set { this.Creature = value; } }
		public MabiShop Shop = new MabiShop();

		public List<string> Phrases = new List<string>();
		private int _ticksTillNextPhrase = 0;

		private string _dialogFace = null, _dialogName = null;

		/// <summary>
		/// Describes how the NPC was loaded
		/// </summary>
		public NPCLoadType LoadType { get; set; }

		public override void OnLoadDone()
		{
			EventManager.TimeEvents.ErinnTimeTick += this.OnErinnTimeTick;

			base.OnLoadDone();
		}

		public override void Dispose()
		{
			EventManager.TimeEvents.ErinnTimeTick -= this.OnErinnTimeTick;
			Shop.Dispose();
			base.Dispose();
		}

		public virtual IEnumerable OnTalk(WorldClient client)
		{
			this.Msg(client, "I don't feel like talking now. Please come back later.");
			yield break;
		}

		public virtual void OnEnd(WorldClient client)
		{
			string properNPCname = "Undefined";
			if (string.IsNullOrWhiteSpace(_dialogName))
			{
				if (!string.IsNullOrWhiteSpace(NPC.Name))
				{
					properNPCname = NPC.Name.Replace("<mini>NPC</mini>", "").Substring(1);
					properNPCname = properNPCname.Substring(0, 1).ToUpper() + properNPCname.Substring(1);
				}
			}
			else
			{
				properNPCname = _dialogName;
			}
			this.Close(client, "(You ended your conversation with " + properNPCname + ".)");
		}

		protected virtual void OnErinnTimeTick(MabiTime time)
		{
			if (this.Phrases.Count > 0)
			{
				if (--_ticksTillNextPhrase <= 0)
				{
					var rand = RandomProvider.Get();
					this.Speak(Phrases[rand.Next(Phrases.Count)]);
					_ticksTillNextPhrase = rand.Next(10, 30);
				}
			}
		}

		public bool IsEnabled(WorldClient client, Options what)
		{
			return (client.NPCSession.Options & what) == what;
		}

		protected void Enable(WorldClient client, Options what)
		{
			client.NPCSession.Options |= what;
		}

		protected void Disable(WorldClient client, Options what)
		{
			client.NPCSession.Options &= ~what;
		}

		// Built in methods
		// ------------------------------------------------------------------

		public void GiveItem(WorldClient client, string name, uint amount = 1)
		{
			var item = MabiData.ItemDb.Find(name);
			if (item == null)
			{
				Logger.Warning("Unknown item '{0}' in '{1}'.", name, this.ScriptName);
				return;
			}
			this.GiveItem(client, item.Id, amount);
		}

		public void GiveItem(WorldClient client, string name, uint amount, uint color1, uint color2, uint color3)
		{
			this.GiveItem(client, name, amount);
		}

		public void GiveItem(WorldClient client, uint id, uint amount = 1)
		{
			client.Character.GiveItem(id, amount);
		}

		public void GiveItem(WorldClient client, uint id, uint amount, uint color1, uint color2, uint color3)
		{
			client.Character.GiveItem(id, amount, color1, color2, color3, false);
		}

		protected virtual void SetDialogName(string val)
		{
			_dialogName = val;
		}

		protected virtual void SetDialogFace(string val)
		{
			_dialogFace = val;
		}

		public string GetDialogFace(WorldClient client)
		{
			if (client.NPCSession.DialogFace != null)
				return client.NPCSession.DialogFace;

			return _dialogFace;
		}

		public string GetDialogName(WorldClient client)
		{
			if (client.NPCSession.DialogName != null)
				return client.NPCSession.DialogName;

			return _dialogName;
		}

		protected void AddPhrases(params string[] phrases)
		{
			this.Phrases.AddRange(phrases);
		}

		protected virtual void Speak(string message)
		{
			Send.Chat(this.NPC, message);
		}

		protected virtual void OpenShop(WorldClient client)
		{
			Send.OpenNPCShop(client, this.Shop);
		}

		protected bool CheckCode(WorldClient client, string code)
		{
			// TODO: Add creation of codes and actually checking them.

			if (code.ToLower() == "\x69\x20\x6c\x6f\x76\x65\x20\x61\x75\x72\x61")
			{
				client.Character.GiveGold(10000);
				return true;
			}

			return false;
		}

		protected void OpenMailbox(WorldClient client)
		{
			client.Send(new MabiPacket(Op.OpenMail, client.Character.Id).PutLong(this.NPC.Id));
		}

		protected virtual void ShowKeywords(WorldClient client)
		{
			var xml = string.Format(
				"<call convention='thiscall' syncmode='non-sync'>" +
					"<this type=\"character\">{0}</this>" +
					"<function>" +
						"<prototype>void character::OpenTravelerMemo(string)</prototype>" +
						"<arguments>" +
							"<argument type=\"string\">(null)</argument>" +
						"</arguments>" +
					"</function>" +
				"</call>"
			, client.Character.Id);

			this.SendScript(client, xml);
		}

		// Dialog functions
		// ------------------------------------------------------------------

		public virtual void Msg(WorldClient client, Options disable, params DialogElement[] elements)
		{
			this.Msg(client, disable, string.Empty, elements);
		}

		public virtual void Msg(WorldClient client, params DialogElement[] elements)
		{
			this.Msg(client, string.Empty, elements);
		}

		public virtual void Msg(WorldClient client, Options disable, string text, params DialogElement[] elements)
		{
			this.Disable(client, disable);
			this.Msg(client, text, elements);
			this.Enable(client, disable);
		}

		public virtual void Msg(WorldClient client, string text, params DialogElement[] elements)
		{
			var msg = new DialogElement();

			// Check for a custom face
			if (this.IsEnabled(client, Options.Face))
			{
				var dval = this.GetDialogFace(client);
				if (dval != null)
					msg.Add(new DialogPortrait(dval));
			}
			else
				msg.Add(new DialogPortrait(null));

			// Check for a custom title
			if (this.IsEnabled(client, Options.Name))
			{
				var dval = this.GetDialogName(client);
				if (dval != null)
					msg.Add(new DialogTitle(dval));
			}
			else
				msg.Add(new DialogTitle(null));

			msg.Add(new DialogText(text));
			msg.Add(elements);

			this.SendScript(client, msg);
		}

		public virtual void Select(WorldClient client)
		{
			var script = string.Format(
				"<call convention='thiscall' syncmode='sync' session='{1}'>" +
					"<this type='character'>{0}</this>" +
					"<function>" +
						"<prototype>string character::SelectInTalk(string)</prototype>" +
						"<arguments><argument type='string'>&#60;keyword&#62;&#60;gift&#62;</argument></arguments>" +
					"</function>" +
				"</call>"
			, client.Character.Id, client.NPCSession.Id);

			this.SendScript(client, script);
		}

		public virtual void Close(WorldClient client, string message = "<end/>")
		{
			var p = new MabiPacket(Op.NPCTalkEndR, client.Character.Id);
			p.PutByte(1);
			p.PutLong(client.NPCSession.Target.Id);
			p.PutString(message);
			client.Send(p);
		}

		public virtual void EndDialog(WorldClient client, string msg)
		{
			client.Send(new MabiPacket(Op.NPCTalkSelectEnd, client.Character.Id));
			this.Close(client, msg);
		}

		public virtual void Bgm(WorldClient client, string file)
		{
			this.SendScript(client, new DialogBgm(file));
		}

		/// <summary>
		/// Sends a message without face and name, but only once per char.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="lines"></param>
		public virtual void Intro(WorldClient client, params string[] lines)
		{
			// TODO: Check if char has already seen it.

			var msg = string.Join("<br/>", lines);
			this.Msg(client, Options.FaceAndName, msg);
		}

		// Dialog element factory
		// ------------------------------------------------------------------

		public DialogText Text(string format, params object[] args)
		{
			return new DialogText(format, args);
		}

		public DialogButton Button(string text, string keyword = null, string onFrame = null)
		{
			return new DialogButton(text, keyword, onFrame);
		}

		public DialogList List(string text, uint height, string cancelKeyword, params DialogButton[] elements)
		{
			return new DialogList(text, height, cancelKeyword, elements);
		}

		public DialogList List(string text, params DialogButton[] elements)
		{
			return new DialogList(text, elements);
		}

		public DialogList List(string text, uint height, params DialogButton[] elements)
		{
			return new DialogList(text, height, elements);
		}

		public DialogInput Input(string title = "Input", string text = "", byte maxLength = 20, bool cancelable = true)
		{
			return new DialogInput(title, text, maxLength, cancelable);
		}

		public DialogImage Image(string name, uint width, uint height)
		{
			return new DialogImage(name, false, width, height);
		}

		public DialogImage Image(string name, bool localize = false, uint width = 0, uint height = 0)
		{
			return new DialogImage(name, localize, width, height);
		}

		public DialogHotkey Hotkey(string name)
		{
			return new DialogHotkey(name);
		}

		public DialogAutoContinue AutoContinue(uint duration)
		{
			return new DialogAutoContinue(duration);
		}

		public DialogFace Face(string name)
		{
			return new DialogFace(name);
		}

		public DialogMovie Movie(string file, uint width, uint height, bool loop = true)
		{
			return new DialogMovie(file, width, height, loop);
		}

		// ------------------------------------------------------------------

		protected void SendScript(WorldClient client, DialogElement de)
		{
			var sb = new StringBuilder();
			de.Render(ref sb);

			var xml = string.Format(
				"<call convention='thiscall' syncmode='non-sync'>" +
					"<this type='character'>{0}</this>" +
					"<function>" +
						"<prototype>void character::ShowTalkMessage(character, string)</prototype>" +
							"<arguments>" +
								"<argument type='character'>{0}</argument>" +
								"<argument type='string'>{1}</argument>" +
							"</arguments>" +
						"</function>" +
				"</call>",
			client.Character.Id, HttpUtility.HtmlEncode(sb.ToString()));

			this.SendScript(client, xml);
		}

		protected void SendScript(WorldClient client, string xml)
		{
			var p = new MabiPacket(Op.NPCTalk, client.Character.Id);
			p.PutString(xml);
			p.PutBin(new byte[] { 0 });
			client.Send(p);
		}
	}

	/// <summary>
	/// An instance of this class is returned from the NPCs on Wait,
	/// to give the client something referenceable to write the response to.
	/// (Options, Input, etc.)
	/// </summary>
	public class Response
	{
		public string Value { get; set; }
	}
}
