using Common.Constants;
using Common.Events;
using Common.Tools;
using Common.World;
using System;
using World.Network;
using World.Scripting;
using World.World;

public class FletaScript : NPCScript
{
    private byte lastHour;
    private bool lastVisible;
    
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_fleta");
		SetRace(10001);
		SetBody(height: 0.1000001f, fat: 1.06f, upper: 1.09f, lower: 1f);
		SetFace(skin: 15, eye: 8, eyeColor: 155, lip: 2);

		NPC.ColorA = 0x0;
		NPC.ColorB = 0x0;
		NPC.ColorC = 0x0;		

		EquipItem(Pocket.Face, 0xF3C, 0x470036, 0xF79622, 0xF46F81);
		EquipItem(Pocket.Hair, 0xBBC, 0xFFBC8B63, 0xFFBC8B63, 0xFFBC8B63);
		EquipItem(Pocket.Armor, 0x3AE6, 0xFF301D16, 0xFF1B100E, 0xFF6D5034);
		EquipItem(Pocket.Shoe, 0x426F, 0x151515, 0xFFFFFF, 0xFFFFFF);
        
        lastHour = 255;
        lastVisible = false;
                
        ErinnTimeTick(null, new TimeEventArgs((byte)((DateTime.Now.Ticks / 900000000) % 24) ,0)); //Initialize Fleta

		SetDirection(240);
		SetStand("");
        
		Phrases.Add("...Ah, I'm bored.");
		Phrases.Add("...Are you scared?");
		Phrases.Add("...Hehe");
		Phrases.Add("Chirp Chirp.");
		Phrases.Add("La la la.");
		Phrases.Add("Should I go play somewhere else...");
	}
        
    protected override void ErinnTimeTick(object sender, TimeEventArgs e)
    {
        if (sender != null)
            base.ErinnTimeTick(sender, e);
    
        if (lastHour == e.Hour)
            return;
        
        lastHour = e.Hour;
        
        if ((9 <= e.Hour && e.Hour < 11) || (15 <= e.Hour && e.Hour < 17) || (19 <= e.Hour && e.Hour < 21))
        {
            if ((sender == null || lastVisible == false))
            {
                WarpNPC(region: 53, x: 104689, y: 109742);
                lastVisible = true;
                //Logger.Info("Fleta has appeared in Sen Mag");
            }
        }
        else if (sender == null || lastVisible == true)
        {
            WarpNPC(region: 15, x: 300, y: 0);
            lastVisible = false;
            //Logger.Info("Fleta has vanished from Sen Mag");
        }
    }
}
