using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class KrugScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_krug");
		SetRace(28);
		SetBody(height: 1.3f, fat: 1f, upper: 1.2f, lower: 1f);
		SetFace(skin: 0, eye: 0, eyeColor: 0, lip: 0);

		SetColor(0x808080, 0x808080, 0x808080);


		SetLocation(region: 3200, x: 289307, y: 212562);

		SetDirection(76);
		SetStand("giant/male/anim/giant_npc_krug");
        
		Phrases.Add("Don't try to escape death.  Death tries harder against those who elude it.");
		Phrases.Add("Everyone wears a mask, hiding their dirty faces behind a thick layer of leather.");
		Phrases.Add("Giants and Elves are different in every way, even down to the air they breathe.");
		Phrases.Add("I am willing to bet my right arm concerning my wife's fidelity.");
		Phrases.Add("I think it's time to trim Waschbar's feathers.");
		Phrases.Add("I think Waschbar is shedding already.");
		Phrases.Add("Now is the time for us to follow the light of the ancient Giants.");
		Phrases.Add("Oh, my Kirine. Your name is pure as the shining moon above the whitest field of snow.");
		Phrases.Add("Oh, my queen, Kirine.");
		Phrases.Add("Waschbar may only be an eagle, but he is one my closest friends.");
	}
}
