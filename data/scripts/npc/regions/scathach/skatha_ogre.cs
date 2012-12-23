using Common.Constants;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class Skatha_ogreScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_skatha_ogre");
		SetRace(323);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 30, eye: 0, eyeColor: 0, lip: 0);

		NPC.ColorA = 0x9CB4D0;
		NPC.ColorB = 0x264870;
		NPC.ColorC = 0x343952;		

		EquipItem(Pocket.RightHand1, 0xA03B, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 4014, x: 31870, y: 43840);

		SetDirection(189);
		SetStand("chapter4/monster/anim/ogre/ogre_c4_npc_commerce");
	}
}
