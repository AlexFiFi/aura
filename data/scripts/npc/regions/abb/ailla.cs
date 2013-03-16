using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class AillaScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_ailla");
		SetRace(9018);
		SetBody(height: 1.2f, fat: 1f, upper: 1.2f, lower: 1f);
		SetFace(skin: 20, eye: 38, eyeColor: 98, lip: 48);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x170C, 0xF3A842, 0xE1D952, 0xA8279);
		EquipItem(Pocket.Hair, 0x23F0, 0xF3713F, 0xF3713F, 0xF3713F);
		EquipItem(Pocket.Armor, 0x3C88, 0xA37109, 0xE0C53A, 0xF2DDCE);
		EquipItem(Pocket.Head, 0x488F, 0x1890EE, 0xCBCADB, 0x4F5256);

		SetLocation(region: 302, x: 125993, y: 85100);

		SetDirection(20);
		SetStand("human/anim/emotion/female_natural_emotion_dance01");
        
		Phrases.Add("Hmph.");
		Phrases.Add("I missed you, Milletian!");
		Phrases.Add("La la la...");
		Phrases.Add("What's going on here?");
		Phrases.Add("Where's my love?");

	}
}
