using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class BekardScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_bekard");
		SetRace(9002);
		SetBody(height: 0.8000003f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 16, eye: 141, eyeColor: 196, lip: 2);

		NPC.ColorA = 0x808080;
		NPC.ColorB = 0x808080;
		NPC.ColorC = 0x808080;		

		EquipItem(Pocket.Face, 0x1AF4, 0xF48854, 0xB90029, 0xF2567E);
		EquipItem(Pocket.Hair, 0x102B, 0xFFFFFF, 0xFFFFFF, 0xFFFFFF);
		EquipItem(Pocket.Armor, 0x3C76, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Shoe, 0x4319, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.RightHand1, 0x9DBC, 0x808080, 0x959595, 0x3C4155);

		SetLocation(region: 4014, x: 33040, y: 41920);

		SetDirection(82);
		SetStand("");

		Phrases.Add("Begone.");
		Phrases.Add("Do you not know who I am? I'm a Royal Alchemist!");
		Phrases.Add("On your knees.");
		Phrases.Add("The arrogance!");
		Phrases.Add("Ugh...disgusting.");
		Phrases.Add("Who do you think you are?");
	}
}
