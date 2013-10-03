using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class KarpfenScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_karpfen");
		SetRace(44);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 17, eye: 0, eyeColor: 0, lip: 0);

		SetColor(0x808080, 0x0, 0x0);

		EquipItem(Pocket.RightHand2, 0x9CF1, 0x6D613F, 0x6B6759, 0x0);

		SetLocation(region: 300, x: 205354, y: 191446);

		SetDirection(7);
		SetStand("chapter3/giant/female/anim/giant_female_c3_npc_karpfen");
        
		Phrases.Add("Everything in Uladh is so...small.");
		Phrases.Add("Hahaha, who would dare get in my way?");
		Phrases.Add("My beauty and strength transcend race!");
		Phrases.Add("Taillteann is so dull compared to Physis.");

	}
}
