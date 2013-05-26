// Aura Script
// -------------------------------------------------------------------------
// Mailboxes
// -------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

// Base
public class _MailboxBaseScript : NPCScript
{
	public override void OnLoad()
	{
		SetRace(990019);
	}

	public override IEnumerable OnTalk(WorldClient c)
	{
		OpenMailbox(c);
		MsgSelect(c, Options.Name, "(You can check your mail through the mailbox.)", Button("Quit", "@end"));
		End();
	}
	
	public override void OnEnd(WorldClient c)
	{
		Close(c, "(Finished checking the mailbox.)");
	}
}

// Uladh
public class MailboxTirScript : _MailboxBaseScript { public override void OnLoad() { base.OnLoad(); SetName("_Mailbox_TirChonaill"); SetLocation(1, 11351, 39397, 222); } }
public class MailboxDugaldScript : _MailboxBaseScript { public override void OnLoad() { base.OnLoad(); SetName("_MailBox_Housing_Dugald"); SetLocation(200, 20108, 18733, 231); } }
public class MailboxDunbartonScript : _MailboxBaseScript { public override void OnLoad() { base.OnLoad(); SetName("_MailBox_Dunbarton"); SetLocation(14, 35745, 37086, 0); } }
public class MailboxEmainScript : _MailboxBaseScript { public override void OnLoad() { base.OnLoad(); SetName("_MailBox_EmainMacha"); SetLocation(52, 36147, 46147, 144); } }
public class MailboxSenMagScript : _MailboxBaseScript { public override void OnLoad() { base.OnLoad(); SetName("_MailBox_Housing_SenMag"); SetLocation(202, 42172, 49415, 202); } }
public class MailboxBangorScript : _MailboxBaseScript { public override void OnLoad() { base.OnLoad(); SetName("_MailBox_Bangor"); SetLocation(31, 11328, 9239, 0); } }
public class MailboxAbbResidentialScript : _MailboxBaseScript { public override void OnLoad() { base.OnLoad(); SetName("_Mailbox_Housing_AbbNeagh"); SetLocation(214, 53305, 51991, 214); } }
public class MailboxTaillteannScript : _MailboxBaseScript { public override void OnLoad() { base.OnLoad(); SetName("_MailBox_Taillteann"); SetLocation(300, 216819, 193700, 156); } }
public class MailboxTaraScript : _MailboxBaseScript { public override void OnLoad() { base.OnLoad(); SetName("_MailBox_Tara"); SetLocation(401, 109136, 94295, 194); } }
public class MailboxTaraResidentialScript : _MailboxBaseScript { public override void OnLoad() { base.OnLoad(); SetName("_MailBox_Housing_Cuilin"); SetLocation(60204, 53305, 51991, 214); } }
public class MailboxCobhScript : _MailboxBaseScript { public override void OnLoad() { base.OnLoad(); SetName("_MailBox_Cobh"); SetLocation(23, 27657, 41599, 0); } }

// Iria
public class MailboxRanoScript : _MailboxBaseScript { public override void OnLoad() { base.OnLoad(); SetName("_MailBox_IriaRano"); SetLocation(3001, 164133, 169740, 253); SetRace(990021); } }
public class MailboxConnousScript : _MailboxBaseScript { public override void OnLoad() { base.OnLoad(); SetName("_MailBox_IriaConnous"); SetLocation(3100, 366122, 425133, 222); SetRace(990020); } }
public class MailboxPhysisScript : _MailboxBaseScript { public override void OnLoad() { base.OnLoad(); SetName("_MailBox_IriaPhysis"); SetLocation(3200, 287527, 222681, 222); SetRace(990020); } }
public class MailboxCourcleScript : _MailboxBaseScript { public override void OnLoad() { base.OnLoad(); SetName("_MailBox_IriaCourcle"); SetLocation(3300, 251450, 183913, 250); SetRace(990021); } }
public class MailboxZardineScript : _MailboxBaseScript { public override void OnLoad() { base.OnLoad(); SetName("_MailBox_IriaZardine"); SetLocation(3400, 329447, 176173, 178); } }

// Belfast
public class MailboxBelfastScript : _MailboxBaseScript { public override void OnLoad() { base.OnLoad(); SetName("_Mailbox_Belfast"); SetLocation(4005, 42381, 24835, 34); } }
