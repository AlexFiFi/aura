using Aura.Shared.Const;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class VoightScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_voight");
		SetRace(10002);
		SetBody(height: 1.1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 17, eye: 0, eyeColor: 0, lip: 0);

		SetColor(0x808080, 0x808080, 0x808080);

		EquipItem(Pocket.Face, 0x132E, 0x2C2869, 0xD00031, 0xF58974);
		EquipItem(Pocket.Hair, 0x135B, 0x677276, 0x677276, 0x677276);
		EquipItem(Pocket.Armor, 0x3B8A, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Head, 0x46F0, 0x969696, 0xBCBCBC, 0x948C8F);

		SetLocation(region: 3300, x: 251470, y: 183780);

		SetDirection(236);
		SetStand("");
        
		Phrases.Add("Aww... I think I drank too much last night...");
		Phrases.Add("Did you just ask when I close the bank? Well, I don't close it, ever.");
		Phrases.Add("Even garbage is recyclable nowadays. Why not life?");
		Phrases.Add("Jeez, it's so hot in this jungle.");
		Phrases.Add("Man, those creditors! There's no end to their hassling!");
		Phrases.Add("Pay me if you want my appraisal service.");
		Phrases.Add("Yeah, I'm too old to be told that I'm cute.");
		Phrases.Add("Yeah, this is real snake skin.");
	}
}
