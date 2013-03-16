using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class ArnengScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_arneng");
		SetRace(10002);
		SetBody(height: 0.9999999f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 20, eye: 24, eyeColor: 8, lip: 0);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x1324, 0x7CC0, 0x7A44, 0x4027);
		EquipItem(Pocket.Hair, 0xFA5, 0x26180F, 0x26180F, 0x26180F);
		EquipItem(Pocket.Armor, 0x3BC3, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Shoe, 0x4285, 0x343436, 0xB8B223, 0x808080);

		SetLocation(region: 3400, x: 329238, y: 176236);

		SetDirection(178);
		SetStand("human/male/anim/male_natural_stand_npc_yoff");
        
		Phrases.Add("If I could get paid for it, I would've sold my soul a long time ago.");
		Phrases.Add("I'm so tired of not being able to sleep");
		Phrases.Add("There are those who come here to hand in their will before they meet the dragon.");
		Phrases.Add("This is the end. The end of the Aura.World...");
		Phrases.Add("You can say that this is like the final page in the Aura.World.");
		Phrases.Add("You know, right? This place is the end of the Aura.World.");
	}
}
