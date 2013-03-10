// Aura Script
// --------------------------------------------------------------------------
// Evidence - ?
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class _EvidenceBaseScript : NPCScript
{
	public override void OnLoad()
	{
		SetRace(990056);
		SetColor(0x808080, 0x808080, 0x808080);
	}
}

public class Evidence1Script : _EvidenceBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_evidence_01");
		SetLocation(411, 6717, 6743, 40);
	}
}

public class Evidence2Script : _EvidenceBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_evidence_02");
		SetLocation(411, 8009, 7801, 40);
	}
}

public class Evidence3Script : _EvidenceBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_evidence_03");
		SetLocation(411, 6413, 10109, 40);
	}
}

public class Evidence4Script : _EvidenceBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_evidence_04");
		SetLocation(411, 10944, 10304, 40);
	}
}

public class Evidence5Script : _EvidenceBaseScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		
		SetName("_evidence_05");
		SetLocation(411, 10316, 8108, 40);
	}
}
