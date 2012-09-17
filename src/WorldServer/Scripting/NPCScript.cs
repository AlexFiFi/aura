// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

using System.Text;
using Common.Constants;
using Common.Network;
using Common.Tools;
using Common.World;
using World.Network;
using World.World;
using MabiNatives;

namespace World.Scripting
{
	public abstract class NPCScript : BaseScript
	{
		public MabiNPC NPC { get; set; }
		public MabiShop Shop = new MabiShop();

		public override void OnLoad()
		{
			this.NPC.Region = 0;
			this.NPC.SetPosition(0, 0);
		}

		public virtual void OnTalk(WorldClient client)
		{
		}

		public virtual void OnSelect(WorldClient client, string response)
		{
			// TODO: I guess we need a check later, if the player actually has the keyword.
			this.MsgSelect(client, "(Server: Unknown keyword.)");
		}

		public virtual void OnEnd(WorldClient client)
		{
			this.Close(client);
		}

		// Built in methods
		// ==================================================================

		protected virtual void SetName(string name)
		{
			this.NPC.Name = name;
		}

		protected virtual void SetRace(uint race)
		{
			this.NPC.Race = race;
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

		protected virtual void SetDirection(byte direction)
		{
			this.NPC.Direction = direction;
		}

		protected virtual void SetStand(string style)
		{
			this.NPC.StandStyle = style;
		}

		protected void SendScript(WorldClient client, string script)
		{
			var p = new MabiPacket(0x13882, client.Character.Id);
			p.PutString(script);
			p.PutBin(new byte[] { 0 });
			client.Send(p);
		}

		protected virtual void Msg(WorldClient client, params string[] lines)
		{
			this.Msg(client, true, true, lines);
		}

		protected virtual void Msg(WorldClient client, bool face, bool name, params string[] lines)
		{
			string message = string.Join("<br/>", lines);

			if (!face)
				message = "<npcportrait name=\"NONE\"/>" + message;
			if (!name)
				message = "<title name=\"NONE\"/>" + message;

			message = System.Web.HttpUtility.HtmlEncode(message);

			string script = string.Format(
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

		protected virtual void Close(WorldClient client, string message = "")
		{
			message = "<npcportrait name=\"NONE\"/><title name=\"NONE\"/>" + message;

			var p = new MabiPacket(0x55F3, client.Character.Id);
			p.PutByte(1);
			p.PutLong(client.NPCSession.Target.Id);
			p.PutString(message);
			client.Send(p);
		}

		protected virtual void Speak(string message)
		{
			WorldManager.Instance.CreatureTalk(this.NPC, message);
		}

		protected virtual void OpenShop(WorldClient client)
		{
			var p = new MabiPacket(0x715E, client.Character.Id);
			p.PutString("shopname");
			p.PutByte(0);
			p.PutByte(0);
			p.PutInt(0);
			p.PutByte((byte)this.Shop.Tabs.Count);
			for (var i = 0; i < this.Shop.Tabs.Count; ++i)
			{
				p.PutString("[" + i + "]" + this.Shop.Tabs[i].Name);
				p.PutByte(0);
				p.PutShort((ushort)this.Shop.Tabs[i].Items.Count);
				foreach (var item in this.Shop.Tabs[i].Items)
				{
					item.AddPrivateEntityData(p);
				}
			}
			client.Send(p);
		}

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
				Logger.Warning("Unknown item '" + itemName + "'.");
				return;
			}

			this.EquipItem(slot, dbInfo.Id, color1, color2, color3);
		}
	}
}
