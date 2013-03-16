using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class MalleusScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_malleus");
		SetRace(8002);
		SetBody(height: 1.2f, fat: 1f, upper: 1.1f, lower: 1f);
		SetFace(skin: 15, eye: 50, eyeColor: 126, lip: 29);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x22C4, 0x9DDBEC, 0x53721F, 0x5520);
		EquipItem(Pocket.Armor, 0x32FF, 0x4F4F4F, 0x633C31, 0x808080);
		EquipItem(Pocket.Glove, 0x409D, 0x4F4F4F, 0x633C31, 0x808080);
		EquipItem(Pocket.Shoe, 0x4478, 0x4F4F4F, 0x633C31, 0x808080);
		EquipItem(Pocket.Head, 0x487D, 0x4F4F4F, 0x633C31, 0x808080);
		EquipItem(Pocket.RightHand1, 0x9D60, 0x633C31, 0x808080, 0x808080);

		SetLocation(region: 428, x: 67581, y: 107823);

		SetDirection(43);
		SetStand("");

		Phrases.Add("Could I interrupt you for a moment?");
		Phrases.Add("For the honor of Giants!");
		Phrases.Add("My name is Melor.");
	}
}
