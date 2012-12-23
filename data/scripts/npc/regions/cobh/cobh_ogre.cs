using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Cobh_ogreScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_cobh_ogre");
		SetRace(323);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 205, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0xBC8942;
		NPC.ColorB = 0x444444;
		NPC.ColorC = 0x243954;		

		EquipItem(Pocket.RightHand1, 0xA03B, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 23, x: 22400, y: 41150);

		SetDirection(46);
		SetStand("chapter4/monster/anim/ogre/ogre_c4_npc_commerce");
	}
}
