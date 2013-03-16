using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Goblin_tacoScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_goblin_taco");
		SetRace(322);
		SetBody(height: 0.5f, fat: 1f, upper: 1.2f, lower: 1.1f);
		SetFace(skin: 23, eye: 3, eyeColor: 7, lip: 2);

		SetColor(0xDE7373, 0x5E5FA4, 0xAE8F8F);



		SetLocation(region: 4005, x: 46670, y: 40003);

		SetDirection(133);
		SetStand("chapter4/human/female/anim/female_c4_npc_cordelia");

		Phrases.Add("...");
		Phrases.Add("Everyone will pay for their actions someday.");
		Phrases.Add("Let's all be nice to each other.");
	}
}
