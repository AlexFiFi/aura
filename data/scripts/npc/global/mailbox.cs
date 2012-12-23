using Common.Network;
using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Mailbox : NPCScript
{
	public override void OnLoad()
	{
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		SetStand("");
	}

	public override void OnTalk(WorldClient c)
	{
		c.Send(new MabiPacket(Op.OpenMail, c.Character.Id).PutLong(NPC.Id));
		MsgSelect(c, "(You can check your mail through the mailbox.)", "Quit", "@end");
	}
}
