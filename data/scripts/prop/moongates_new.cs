// --- Aura Script ----------------------------------------------------------
//  Moongates
// --- Description ----------------------------------------------------------
//  New version of the moon gates, the "Moon Tunnels". Traveling between
//  specific locations at night.
// --- Notes ----------------------------------------------------------------
//  While it would nice to have custom moon gates, the client doesn't seem
//  to take them.
// --------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Aura.Shared.Network;
using Aura.Shared.Util;
using Aura.World.Events;
using Aura.World.Network;
using Aura.World.Player;
using Aura.World.Scripting;
using Aura.World.World;

public class MoongateScript : BaseScript
{
	public override void OnLoad()
	{
		// <config> ---------------------------------------------------------

		// Gates
		AddGate("tirchonaill",     0xA000010004001F, 10134, 1,   13569,  22321,  2.382423f);
		AddGate("dunbarton",       0xA0000E000A02B4, 10135, 14,  26331,  38199,  5.961426f);
		AddGate("bangor",          0xA0001E0008002D, 10136, 30,  43149,  19016,  2.968136f);
		AddGate("emainmacha",      0xA000340008003D, 10137, 52,  50878,  34454,  0.000000f);
		AddGate("ceoisland",       0xA0003800010001, 10138, 56,  8473,   8943,   0.863496f);
		AddGate("ceann_harbor",    0xA000640001002C, 10139, 100, 24019,  42174,  0.000000f);
		AddGate("taillteann_west", 0xA0012C001900C3, 10140, 300, 193871, 205934, 4.225929f);
		AddGate("tara_west",       0xA00191000802F2, 10141, 401, 59727,  124203, 5.397424f);
		
		// Allow use of all gates from the start?
		_freeRoaming = false;

		// </config> --------------------------------------------------------

		// Spawn props
		foreach (var gate in _gates.Values)
			gate.Prop = SpawnProp(gate.PropId, gate.Ident, "", "", 40100, gate.Region, gate.X, gate.Y, gate.Direction, 1, OpenMapWindow);
		
		// Event for opening/closing
		EventManager.Instance.TimeEvents.ErinnDaytimeTick += OnErinnDaytimeTick;
		
		// Handling the moon gate stuff completely in the script is much easier.
		WorldServer.Instance.RegisterPacketHandler(Op.MoonGateUse, HandleMoonGateUse);
	}

	private Dictionary<string, MoonGate> _gates = new Dictionary<string, MoonGate>();
	private bool _freeRoaming = false;
	
	private void AddGate(string ident, ulong propId, ushort keywordId, uint region, uint x, uint y, float direction)
	{
		ident = "_moontunnel_" + ident;
		_gates.Add(ident, new MoonGate(propId, keywordId, ident, region, x, y, direction));
	}
	
	public override void Dispose()
	{
		EventManager.Instance.TimeEvents.ErinnDaytimeTick -= OnErinnDaytimeTick;
		base.Dispose();
	}

	public void OnErinnDaytimeTick(object sender, TimeEventArgs args)
	{
		var state = "closed";
		if(args.Time.IsNight)
			state = "open";

		foreach (var gate in _gates.Values)
		{
			gate.Prop.State = state;
			WorldManager.Instance.SendPropUpdate(gate.Prop);
		}
	}

	public void OpenMapWindow(WorldClient c, MabiPC cr, MabiProp pr)
	{
		var gate = _gates.Values.FirstOrDefault(a => a.Region == cr.Region);
		if (gate == null || gate.Prop.State == "closed")
			return;
		
		if(!cr.Keywords.Contains(gate.KeywordId))
			cr.Keywords.Add(gate.KeywordId);

		var mygates = _gates.Values.Where(a => cr.Keywords.Contains(a.KeywordId) || _freeRoaming || cr.Keywords.Contains(10142));
		
		var p = new MabiPacket(Op.MoonGateMap, cr.Id);
		p.PutInt(2);
		p.PutString(gate.Ident);
		p.PutByte((byte)mygates.Count());
		foreach (var g in mygates)
		{
			p.PutShort(g.KeywordId);
			p.PutByte(1);
			p.PutInts(g.Region, g.X, g.Y);
		}
		c.Send(p);
	}

	private void HandleMoonGateUse(WorldClient client, MabiPacket packet)
	{
		var character = client.GetCreatureOrNull(packet.Id) as MabiPC;
		if(character == null)
			goto L_Fail;
			
		var source = packet.GetString();
		var target = packet.GetString();
		
		// Check gates
		MoonGate sGate, tGate;
		_gates.TryGetValue(source, out sGate);
		_gates.TryGetValue(target, out tGate);
		if (sGate == null || tGate == null || sGate.Prop.State == "closed")
			goto L_Fail;
			
		// Check range to source
		if (character.Region != sGate.Region || !WorldManager.InRange(character, sGate.X, sGate.Y, 1000))
			goto L_Fail;
		
		// Check if char has target
		if (!character.Keywords.Contains(tGate.KeywordId) && !_freeRoaming && !character.Keywords.Contains(10142))
			goto L_Fail;
			
		client.Warp(tGate.Region, tGate.X, tGate.Y);
			
		client.Send(new MabiPacket(Op.MoonGateUseR, client.Character.Id).PutByte(true));
		return;
	
	L_Fail:
		client.Send(new MabiPacket(Op.MoonGateUseR, client.Character.Id).PutByte(false));
	}
	
	public class MoonGate
	{
		public ulong PropId { get; set; }
		public ushort KeywordId { get; set; }
		public string Ident { get; set; }
		public uint Region { get; set; }
		public uint X { get; set; }
		public uint Y { get; set; }
		public float Direction { get; set; }

		public MabiProp Prop { get; set; }

		public MoonGate(ulong propId, ushort keywordId, string ident, uint region, uint x, uint y, float direction)
		{
			this.PropId = propId;
			this.KeywordId = keywordId;
			this.Ident = ident;
			this.Region = region;
			this.X = x;
			this.Y = y;
			this.Direction = direction;
		}
	}
}
