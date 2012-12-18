// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System;
using System.Collections.Generic;
using System.Text;
using Common.Constants;
using Common.Data;
using Common.Events;
using Common.Network;
using Common.Tools;
using Common.World;
using World.Network;
using World.World;

namespace World.Scripting
{
	public enum NPCLoadType { Real = 1, Virtual = 2 }

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
			ServerEvents.Instance.ErinnTimeTick += ErinnTimeTick;
		}

		public override void Dispose()
		{
			ServerEvents.Instance.ErinnTimeTick -= ErinnTimeTick;
			Shop.Dispose();
			base.Dispose();
		}

		public virtual void OnTalk(WorldClient client)
		{
		}

		public virtual void OnSelect(WorldClient client, string response)
		{
			this.OnSelect(client, response, null);
		}

		public virtual void OnSelect(WorldClient client, string response, string input)
		{
			// TODO: I guess we need a check later, if the player actually has the keyword.
			this.MsgSelect(client, "(Server: Unknown keyword.)");
		}

		public virtual void OnEnd(WorldClient client)
		{
			string properNPCname;
			if (string.IsNullOrWhiteSpace(_dialogName))
			{
				properNPCname = NPC.Name.Replace("<mini>NPC</mini>", "").Substring(1);
				properNPCname = properNPCname.Substring(0, 1).ToUpper() + properNPCname.Substring(1);
			}
			else
			{
				properNPCname = _dialogName;
			}
			this.Close(client, "(You ended your conversation with " + properNPCname + ".)");
		}

		private void ErinnTimeTick(object sender, TimeEventArgs e)
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

		protected void Enable(WorldClient client, Options what)
		{
			client.NPCSession.Options |= what;
		}

		protected void Disable(WorldClient client, Options what)
		{
			client.NPCSession.Options &= ~what;
		}

		protected bool IsEnabled(WorldClient client, Options what)
		{
			return (client.NPCSession.Options & what) == what;
		}

		// Built in methods
		// ==================================================================

		protected void GiveItem(WorldClient client, string name, int color1 = 0, int color2 = 0, int color3 = 0, uint amount = 1)
		{
			var item = MabiData.ItemDb.Find(name);
			if (item == null)
			{
				Logger.Warning("Unknown item '" + name + "' cannot be given. Try specifying the ID manually.");
				return;
			}
			GiveItem(client, item.Id, color1, color2, color3, amount);
		}

		protected void GiveItem(WorldClient client, uint id, int color1 = 0, int color2 = 0, int color3 = 0, uint amount = 1)
		{
			client.Character.GiveItem(id, amount);
		}

		protected void Notice(WorldClient client, params string[] messages)
		{
			string message = string.Join("\n", messages);
			client.Send(PacketCreator.Notice(message));
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

			this.SetLocation(regionid, x, y);
		}

		protected virtual void SetLocation(uint region, uint x, uint y)
		{
			this.NPC.Region = region;
			this.NPC.SetPosition(x, y);
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
			NPC.ColorA = c1;
			NPC.ColorB = c2;
			NPC.ColorC = c3;
		}

		protected virtual void SetDirection(byte direction)
		{
			this.NPC.Direction = direction;
		}

		protected virtual void SetStand(string style, string talk_style = "")
		{
			this.NPC.StandStyle = style;
			this.NPC.StandStyleTalk = talk_style;
		}

		protected void SendScript(WorldClient client, string script)
		{
			var p = new MabiPacket(Op.NPCTalkSelectable, client.Character.Id);
			p.PutString(script);
			p.PutBin(new byte[] { 0 });
			client.Send(p);
		}

		protected string GetDialogFace(WorldClient client)
		{
			if (client.NPCSession.DialogFace != null)
				return client.NPCSession.DialogFace;

			return _dialogFace;
		}

		protected string GetDialogName(WorldClient client)
		{
			if (client.NPCSession.DialogName != null)
				return client.NPCSession.DialogName;

			return _dialogName;
		}

		protected virtual void Msg(WorldClient client, params string[] lines)
		{
			// Concate the strings to one line with <br/>s in between,
			// and replace \n with it as well.
			var message = string.Join("<br/>", lines).Replace("\\n", "<br/>");

			// Check wheather a face tag has to be included, to disable the
			// face/name, or activate a custom one.
			if (this.IsEnabled(client, Options.Face))
			{
				var dval = this.GetDialogFace(client);
				if (dval != null)
					message = "<npcportrait name=\"" + _dialogFace + "\"/>" + message;
			}
			else
				message = "<npcportrait name=\"NONE\"/>" + message;

			if (this.IsEnabled(client, Options.Name))
			{
				var dval = this.GetDialogName(client);
				if (dval != null)
					message = "<title name=\"" + _dialogName + "\"/>" + message;
			}
			else
				message = "<title name=\"NONE\"/>" + message;

			// message is going to be inside an XML tag,
			// get rid of special chars.
			message = System.Web.HttpUtility.HtmlEncode(message);

			var script = string.Format(
				"<call convention=\"thiscall\" syncmode=\"non-sync\">" +
					"<this type=\"character\">{0}</this>" +
					"<function>" +
						"<prototype>void character::ShowTalkMessage(character, string)</prototype>" +
						"<arguments>" +
							"<argument type=\"character\">{0}</argument>" +
							"<argument type=\"string\">{1}</argument>" +
						"</arguments>" +
					"</function>" +
				"</call>"
			, client.Character.Id.ToString(), message);

			this.SendScript(client, script);
		}

		protected virtual void MsgSelect(WorldClient client, string message, params string[] buttons)
		{
			if (buttons.Length > 0 && buttons.Length % 2 == 0)
			{
				var sb = new StringBuilder();
				for (int i = 0; i < buttons.Length; )
				{
					sb.Append("<button title =\"" + buttons[i++] + "\" keyword=\"" + buttons[i++] + "\"/>");
				}
				message = message + sb.ToString();
			}

			this.Msg(client, message);
			this.Select(client);
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
				"</call>"
			, client.Character.Id, client.NPCSession.SessionId);

			this.SendScript(client, script);
		}

		protected virtual void ShowKeywords(WorldClient client)
		{
			var script = string.Format(
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

			this.SendScript(client, script);

			this.Select(client);
		}

		protected virtual void MsgInput(WorldClient client, string message, string title = "Input", string description = "", byte maxLen = 20, bool cancelable = true)
		{
			this.Msg(client, message, "<inputbox title='" + title + "' caption='" + description + "' max_len='" + maxLen.ToString() + "' allow_cancel='" + (cancelable ? "true" : "false") + "'/>");
			this.Select(client);
		}

		protected virtual void Close(WorldClient client, string message = "")
		{
			message = "<npcportrait name=\"NONE\"/><title name=\"NONE\"/>" + message;

			var p = new MabiPacket(Op.NPCTalkEndR, client.Character.Id);
			p.PutByte(1);
			p.PutLong(client.NPCSession.Target.Id);
			p.PutString(message);
			client.Send(p);
		}

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
					item.AddPrivateEntityData(p);
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

		protected virtual void SpawnProp(uint propClass, string region, uint x, uint y, float scale = 1, float direction = 1)
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

			this.SpawnProp(propClass, regionid, x, y, scale, direction);
		}

		protected virtual void SpawnProp(uint propClass, uint region, uint x, uint y, float scale = 1, float direction = 1)
		{
			var prop = new MabiProp();
			prop.Info.Class = propClass;
			prop.Region = region;
			prop.Info.X = x;
			prop.Info.Y = y;
			prop.Info.Scale = scale;
			prop.Info.Direction = direction;

			WorldManager.Instance.AddProp(prop);
		}
	}
}
