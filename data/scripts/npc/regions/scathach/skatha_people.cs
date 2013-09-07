using Aura.Shared.Const;
using Aura.Shared.Util;
using Aura.World.Events;
using System;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Skatha_peopleScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_skatha_people");
		SetRace(10001);
		SetBody(height: 1f, fat: 1f, upper: 1f, lower: 1f);
		SetFace(skin: 17, eye: 162, eyeColor: 114, lip: 2);

		EquipItem(Pocket.Face, 0xF52, 0x116A94, 0x6C75, 0xE5B354);
		EquipItem(Pocket.Hair, 0xC3F, 0x8B6559, 0x8B6559, 0x8B6559);
		EquipItem(Pocket.Armor, 0x3E2D, 0x808080, 0x808080, 0x808080);
		EquipItem(Pocket.Head, 0x48F2, 0x808080, 0x808080, 0x808080);

		SetLocation(region: 4015, x: 32951, y: 40325);

		SetDirection(194);
        
		SetStand("chapter4/human/female/anim/female_c4_npc_skatha_human_stand");
        
        Phrases.Add("I'll miss the sounds of the ocean...");
        Phrases.Add("Owen, I miss you.");
        Phrases.Add("You don't have to fear me. Really!");

	}
	
	protected override void Subscribe()
	{
        EventManager.TimeEvents.ErinnDaytimeTick += OnErinnDaytimeTick;
	}
	
	protected override void Unsubscribe()
	{
		EventManager.TimeEvents.ErinnDaytimeTick -= OnErinnDaytimeTick;
	}
    
    private void OnErinnDaytimeTick(MabiTime time)
	{
		if (!time.IsNight)
			WarpNPC(region: 15, x: 100, y: 0);
		else
            WarpNPC(region: 4015, x: 32951, y: 40325);
	}
}
