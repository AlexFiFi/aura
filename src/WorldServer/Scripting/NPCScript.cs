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

namespace Aura.World.Scripting
{
	public enum NPCLoadType { Real = 1, Virtual = 2 }

	public abstract class NPCDialogElement
	{
		public List<NPCDialogElement> Children = new List<NPCDialogElement>();

		public NPCDialogElement Add(params NPCDialogElement[] elements)
		{
			Children.AddRange(elements);
			return this;
		}

		public virtual void Render(WorldClient client, StringBuilder sb, NPCScript script)
		{
			foreach (var child in Children)
				child.Render(client, sb, script);
		}

		protected string MakeXmlSafe(string message)
		{
			return System.Web.HttpUtility.HtmlEncode(message);
		}

		public virtual void Send(WorldClient client, NPCScript script)
		{
			var sb = new StringBuilder();

			this.Render(client, sb, script);

			var xmlScript = string.Format(
				  "<call convention=\"thiscall\" syncmode=\"non-sync\">" +
					  "<this type=\"character\">{0}</this>" +
					  "<function>" +
						    "<prototype>void character::ShowTalkMessage(character, string)</prototype>" +
							"<arguments>" +
								"<argument type=\"character\">{0}</argument>" +
								"<argument type=\"string\">{1}</argument>" +
							"</arguments>" +
						  "</function>" +
				  "</call>",
				client.Character.Id, MakeXmlSafe(sb.ToString()));

			this.SendScript(client, xmlScript);
		}

		protected virtual void Select(WorldClient client)
		{
			var script = string.Format(
				"<call convention=\"thiscall\" syncmode=\"sync\" session=\"{1}\">" +
					"<this type=\"character\">{0}</this>" +
					"<function>" +
						"<prototype>string character::SelectInTalk(string)</prototype>" +
						"<arguments><argument type=\"string\">&#60;keyword&#62;&#60;gift&#62;</argument></arguments>" +
					"</function>" +
				"</call>",
				client.Character.Id, client.NPCSession.SessionId);

			this.SendScript(client, script);
		}

		protected void SendScript(WorldClient client, string script)
		{
			var p = new MabiPacket(Op.NPCTalkSelectable, client.Character.Id);
			p.PutString(script);
			p.PutBin(new byte[] { 0 });
			client.Send(p);
		}
	}

	public class NPCDialogMsg : NPCDialogElement
	{
		public NPCDialogMsg(params NPCDialogElement[] elements)
		{
			this.Add(elements);
		}

		public override void Render(WorldClient client, StringBuilder sb, NPCScript script)
		{
			// Check wheather a face tag has to be included, to disable the
			// face/name, or activate a custom one.
			if (script.IsEnabled(client, Options.Face))
			{
				var dval = script.GetDialogFace(client);
				if (dval != null)
					sb.AppendFormat("<npcportrait name=\"{0}\"/>", dval);
			}
			else
				sb.Append("<npcportrait name=\"NONE\"/>");

			if (script.IsEnabled(client, Options.Name))
			{
				var dval = script.GetDialogName(client);
				if (dval != null)
					sb.AppendFormat("<title name=\"{0}\"/>", dval);
			}
			else
				sb.Append("<title name=\"NONE\"/>");

			base.Render(client, sb, script);
		}
	}

	public class NPCDialogImage : NPCDialogElement
	{
		string _path;
		uint _h, _w;
		bool _local;

		public NPCDialogImage(string name, bool localize = false, uint width = 0, uint height = 0)
		{
			_path = name;
			_local = localize;
			_w = width;
			_h = height;
		}

		public override void Render(WorldClient client, StringBuilder sb, NPCScript script)
		{
			sb.Append("<image");
			if (_local)
				sb.Append(" local=\"true\"");
			sb.AppendFormat(" name=\"{0}\"", _path);
			if (_w != 0)
				sb.AppendFormat(" width=\"{0}\"", _w);
			if (_h != 0)
				sb.AppendFormat(" height=\"{0}\"", _h);

			sb.Append("/>");

			base.Render(client, sb, null);
		}
	}

	public class NPCDialogHotkey : NPCDialogElement
	{
		string _name;

		public NPCDialogHotkey(string name)
		{
			_name = name;
		}

		public override void Render(WorldClient client, StringBuilder sb, NPCScript script)
		{
			sb.AppendFormat("<hotkey name=\"{0}\"/>", _name);
			base.Render(client, sb, null);
		}
	}

	public class NPCDialogText : NPCDialogElement
	{
		private string _msg;

		public NPCDialogText(string msg)
		{
			_msg = msg;
		}

		public override void Render(WorldClient client, StringBuilder sb, NPCScript script)
		{
			sb.Append(_msg);
			base.Render(client, sb, null);
		}
	}

	public class NPCDialogMsgSelect : NPCDialogMsg
	{
		public NPCDialogMsgSelect(params NPCDialogElement[] elements) : base(elements)
		{
		}

		public override void Send(WorldClient client, NPCScript script)
		{
			base.Send(client, script);
			Select(client);
		}
	}

	public class NPCDialogListbox : NPCDialogElement
	{
		uint _visible;
		string _text, _cancelKW;

		public NPCDialogListbox(string title, string cancelKw = "@end", uint height = 10, params NPCDialogElement[] elements)
		{
			_visible = height;
			_text = title;
			_cancelKW = cancelKw;
			this.Add(elements);
		}

		public override void Render(WorldClient client, StringBuilder sb, NPCScript script)
		{
			sb.AppendFormat("<listbox page_size =\"{0}\" title = \"{1}\" cancel = \"{2}\">", _visible, _text, _cancelKW);
			base.Render(client, sb, null);
			sb.Append("</listbox>");
		}

		public override void Send(WorldClient client, NPCScript script)
		{
			base.Send(client, script);
			this.Select(client);
		}
	}

	public class NPCDialogButton : NPCDialogElement
	{
		private string _text, _kw, _onframe;

		public NPCDialogButton(string text, string keyword = null, string onframe = null)
		{
			if (keyword == null)
				keyword = "@" + text.Replace(" ", "_").ToLower();

			_text = text;
			_kw = keyword;
			_onframe = onframe;
		}

		public override void Render(WorldClient client, StringBuilder sb, NPCScript script)
		{
			sb.AppendFormat("<button title=\"{0}\" keyword=\"{1}\"", _text, _kw);
			if (_onframe != null)
				sb.AppendFormat(" onframe=\"{0}\"", _onframe);
			sb.Append("/>");
			base.Render(client, sb, null);
		}
	}

	public class NPCDialogBGM : NPCDialogElement
	{
		string _music;

		public NPCDialogBGM(string filename)
		{
			_music = filename;
		}

		public override void Render(WorldClient client, StringBuilder sb, NPCScript script)
		{
			sb.AppendFormat("<music name=\"{0}\"/>", _music);
			base.Render(client, sb, null);
		}
	}

	public class NPCDialogKeywords : NPCDialogElement
	{
		public override void Render(WorldClient client, StringBuilder sb, NPCScript script)
		{
			throw new InvalidOperationException("This element cannot be nested!");
		}

		public override void Send(WorldClient client, NPCScript script)
		{
			var xmlScript = string.Format(
									   "<call convention=\"thiscall\" syncmode=\"non-sync\">" +
									   "<this type=\"character\">{0}</this>" +
									   "<function>" +
									   "<prototype>void character::OpenTravelerMemo(string)</prototype>" +
									   "<arguments>" +
									   "<argument type=\"string\">(null)</argument>" +
									   "</arguments>" +
									   "</function>" +
									   "</call>",
				client.Character.Id);

			this.SendScript(client, xmlScript);

			this.Select(client);
		}
	}

	public class NPCDialogInput : NPCDialogElement
	{
		string _title, _desc;
		byte _maxLen;
		bool _cancelable;

		public NPCDialogInput(string title = "Input", string description = "", byte maxLen = 20, bool cancelable = true)
		{
			_title = title;
			_desc = description;
			_maxLen = maxLen;
			_cancelable = cancelable;
		}

		public override void Render(WorldClient client, StringBuilder sb, NPCScript script)
		{
			sb.AppendFormat("<inputbox title=\"{0}\" caption=\"{1}\" max_len=\"{2}\" allow_cancel=\"{3}\"/>",
				_title, _desc, _maxLen.ToString(), _cancelable ? "true" : "false");
			base.Render(client, sb, null);
		}

		public override void Send(WorldClient client, NPCScript script)
		{
			base.Send(client, script);
			this.Select(client);
		}
	}

	public abstract class NPCScript : BaseScript
	{
		public MabiNPC NPC { get; set; }
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
			ServerEvents.Instance.ErinnTimeTick += this.OnErinnTimeTick;
		}

		public override void Dispose()
		{
			ServerEvents.Instance.ErinnTimeTick -= this.OnErinnTimeTick;
			Shop.Dispose();
			base.Dispose();
		}

		public virtual IEnumerable OnTalk(WorldClient client)
		{
			this.MsgSelect(client, "I don't feel like talking now. Please come back later!", Button("End Conversation", "@end"));
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

		protected virtual void OnErinnTimeTick(object sender, TimeEventArgs e)
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

		protected void GiveItem(WorldClient client, string name, uint amount = 1)
		{
			var item = MabiData.ItemDb.Find(name);
			if (item == null)
			{
				Logger.Warning("Unknown item '{0}' in '{1}'.", name, this.ScriptName);
				return;
			}
			this.GiveItem(client, item.Id, amount);
		}

		protected void GiveItem(WorldClient client, string name, uint amount, uint color1, uint color2, uint color3)
		{
			this.GiveItem(client, name, amount);
		}

		protected void GiveItem(WorldClient client, uint id, uint amount = 1)
		{
			client.Character.GiveItem(id, amount);
		}

		protected void GiveItem(WorldClient client, uint id, uint amount, uint color1, uint color2, uint color3)
		{
			client.Character.GiveItem(id, amount, color1, color2, color3, false);
		}

		protected virtual void SetName(string name)
		{
			this.NPC.Name = name;
		}

		protected virtual void SetDialogName(string val)
		{
			_dialogName = val;
		}

		protected virtual void SetDialogFace(string val)
		{
			_dialogFace = val;
		}

		protected virtual void SetRace(uint race)
		{
			this.NPC.Race = race;
		}

		protected virtual void SetLocation(string region, uint x, uint y)
		{
			this.SetLocation(region, x, y, 0);
		}

		protected virtual void SetLocation(string region, uint x, uint y, byte direction)
		{
			uint regionid = 0;
			if (!uint.TryParse(region, out regionid))
			{
				var mapInfo = MabiData.MapDb.Find(region);
				if (mapInfo != null)
					regionid = mapInfo.Id;
				else
				{
					Logger.Warning(this.ScriptName + " : Map '" + region + "' not found.");
				}
			}

			this.SetLocation(regionid, x, y, direction);
		}

		protected virtual void SetLocation(uint region, uint x, uint y)
		{
			this.SetLocation(region, x, y, this.NPC.Direction);
		}

		protected virtual void SetLocation(uint region, uint x, uint y, byte direction)
		{
			this.NPC.Region = region;
			this.NPC.SetPosition(x, y);
			this.SetDirection(direction);
		}

		protected void WarpNPC(uint region, uint x, uint y, bool flash = true)
		{
			if (flash)
			{
				WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, this.NPC.Id).PutInts(Effect.ScreenFlash, 3000, 0), SendTargets.Range, this.NPC);
				WorldManager.Instance.Broadcast(new MabiPacket(Op.PlaySound, this.NPC.Id).PutString("data/sound/Tarlach_change.wav"), SendTargets.Range, this.NPC);
			}
			WorldManager.Instance.CreatureLeaveRegion(this.NPC);
			SetLocation(region, x, y);
			if (flash)
			{
				WorldManager.Instance.Broadcast(new MabiPacket(Op.Effect, this.NPC.Id).PutInts(Effect.ScreenFlash, 3000, 0), SendTargets.Range, this.NPC);
				WorldManager.Instance.Broadcast(new MabiPacket(Op.PlaySound, this.NPC.Id).PutString("data/sound/Tarlach_change.wav"), SendTargets.Range, this.NPC);
			}
			WorldManager.Instance.Broadcast(PacketCreator.EntityAppears(this.NPC), SendTargets.Range, this.NPC);
		}

		protected virtual void SetBody(float height = 1.0f, float fat = 1.0f, float lower = 1.0f, float upper = 1.0f)
		{
			this.NPC.Height = height;
			this.NPC.Fat = fat;
			this.NPC.Lower = lower;
			this.NPC.Upper = upper;
		}

		protected virtual void SetFace(byte skin, byte eye, byte eyeColor, byte lip)
		{
			this.NPC.SkinColor = skin;
			this.NPC.Eye = eye;
			this.NPC.EyeColor = eyeColor;
			this.NPC.Lip = lip;
		}

		protected virtual void SetColor(uint c1 = 0x808080, uint c2 = 0x808080, uint c3 = 0x808080)
		{
			NPC.Color1 = c1;
			NPC.Color2 = c2;
			NPC.Color3 = c3;
		}

		protected virtual void SetDirection(byte direction)
		{
			this.NPC.Direction = direction;
		}

		protected virtual void SetStand(string style, string talkStyle = "")
		{
			this.NPC.StandStyle = style;
			this.NPC.StandStyleTalk = talkStyle;
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

		// ============================================================
		// Dialog functions

		protected virtual void Bgm(WorldClient client, string fileName)
		{
			this.Bgm(fileName).Send(client, this);
		}

		protected virtual NPCDialogBGM Bgm(string filename)
		{
			return new NPCDialogBGM(filename);
		}

		protected virtual NPCDialogButton Button(string text, string keyword = null)
		{
			return new NPCDialogButton(text, keyword);
		}

		protected virtual void Close(WorldClient client, string message = "")
		{
			message = "<npcportrait name=\"NONE\"/><title name=\"NONE\"/>" + message;

			this.CloseCustom(client, message);
		}

		protected virtual void CloseCustom(WorldClient client, string message = "<end/>")
		{
			var p = new MabiPacket(Op.NPCTalkEndR, client.Character.Id);
			p.PutByte(1);
			p.PutLong(client.NPCSession.Target.Id);
			p.PutString(message);
			client.Send(p);
		}

		protected virtual NPCDialogHotkey Hotkey(string name)
		{
			return new NPCDialogHotkey(name);
		}

		protected NPCDialogImage Image(string name)
		{
			return Image(name, false, 0, 0);
		}

		protected NPCDialogImage Image(string name, bool localize)
		{
			return Image(name, localize, 0, 0);
		}

		protected virtual NPCDialogImage Image(string name, uint width, uint height)
		{
			return Image(name, false, width, height);
		}

		protected virtual NPCDialogImage Image(string name, bool localize, uint width, uint height)
		{
			return new NPCDialogImage(name, localize, width, height);
		}

		protected virtual NPCDialogInput Input(string title = "Input", string description = "", byte maxLen = 20, bool cancelable = true)
		{
			return new NPCDialogInput(title, description, maxLen, cancelable);
		}

		protected virtual NPCDialogListbox Listbox(string title, string cancelKw = "@end", uint height = 10, params NPCDialogElement[] elements)
		{
			return new NPCDialogListbox(title, cancelKw, height, elements);
		}

		protected virtual NPCDialogListbox Listbox(string title, params NPCDialogElement[] elements)
		{
			return new NPCDialogListbox(title, elements:elements);
		}

		public virtual void Msg(WorldClient client, Options disable = Options.None, params string[] lines)
		{
			this.Disable(client, disable);
			this.Msg(lines).Send(client, this);
			this.Enable(client, disable);
		}

		public virtual void Msg(WorldClient client, params string[] lines)
		{
			this.Msg(lines).Send(client, this);
		}

		public virtual NPCDialogMsg Msg(params string[] lines)
		{
			return new NPCDialogMsg(Text(lines));	
		}

		protected virtual void MsgInput(WorldClient client, string message, string title = "Input", string description = "", byte maxLen = 20, bool cancelable = true)
		{
			new NPCDialogMsgSelect(Text(message)).Add(this.Input(title, description, maxLen, cancelable)).Send(client, this);
		}

		protected virtual void MsgListbox(WorldClient client, string message, string title, params NPCDialogElement[] elements)
		{
			new NPCDialogMsgSelect(Text(message)).Add(this.Listbox(title, elements:elements)).Send(client, this);
		}

		protected virtual void MsgListbox(WorldClient client, string message, string title, string cancelKw = "@end", uint height = 10, params NPCDialogElement[] elements)
		{
			new NPCDialogMsgSelect(Text(message)).Add(this.Listbox(title, cancelKw, height, elements)).Send(client, this);
		}

		public virtual void MsgSelect(WorldClient client, string message, params NPCDialogButton[] buttons)
		{
			this.MsgSelect(message, buttons).Send(client, this);
		}

		public virtual void MsgSelect(WorldClient client, Options disable, string message, params NPCDialogButton[] buttons)
		{
			this.Disable(client, disable);
			this.MsgSelect(message, buttons).Send(client, this);
			this.Enable(client, disable);
		}

		public virtual NPCDialogMsgSelect MsgSelect(string message, params NPCDialogButton[] buttons)
		{
			return new NPCDialogMsgSelect(Text(message)).Add(buttons) as NPCDialogMsgSelect;
		}

		protected virtual void ShowKeywords(WorldClient client)
		{
			new NPCDialogKeywords().Send(client, this);
		}

		protected virtual NPCDialogText Text(params string[] lines)
		{
			return new NPCDialogText(string.Join("<br/>", lines));
		}

		// End dialog functions
		// =========================================================

		protected virtual void Speak(string message)
		{
			WorldManager.Instance.CreatureTalk(this.NPC, message);
		}

#pragma warning disable 0162
		protected virtual void OpenShop(WorldClient client)
		{
			var p = new MabiPacket(Op.ShopOpen, client.Character.Id);
			p.PutString("shopname");
			p.PutByte(0);
			p.PutByte(0);
			p.PutInt(0);
			p.PutByte((byte)this.Shop.Tabs.Count);
			for (var i = 0; i < this.Shop.Tabs.Count; ++i)
			{
				p.PutString("[" + i + "]" + this.Shop.Tabs[i].Name);
				if (Op.Version >= 160200)
					p.PutByte(0);
				p.PutShort((ushort)this.Shop.Tabs[i].Items.Count);
				foreach (var item in this.Shop.Tabs[i].Items)
				{
					item.AddToPacket(p, ItemPacketType.Private);
				}
			}
			client.Send(p);
		}
#pragma warning restore 0162

		protected virtual void EquipItem(Pocket slot, uint itemClass, uint color1 = 0, uint color2 = 0, uint color3 = 0)
		{
			var item = new MabiItem(itemClass);
			item.Info.ColorA = color1;
			item.Info.ColorB = color2;
			item.Info.ColorC = color3;
			item.Info.Pocket = (byte)slot;

			//var inPocket = this.NPC.GetItemInPocket(slot);
			//if (inPocket != null)
			//    this.NPC.Items.Remove(inPocket);

			this.NPC.Items.Add(item);
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

		protected void AddPhrases(params string[] phrases)
		{
			this.Phrases.AddRange(phrases);
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
