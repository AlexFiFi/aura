// Aura Script
// --------------------------------------------------------------------------
// Carys - Captain to Port Quilla
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Carys_to_iria_modelScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_carys_to_iria_model");
		SetRace(10002);
		SetBody(height: 1.1f, fat: 1.2f, upper: 1.1f, lower: 1.25f);
		SetFace(skin: 26, eye: 9, eyeColor: 46, lip: 18);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0x1324, 0x1A715D, 0xBA0371, 0xF2365F);
		EquipItem(Pocket.Hair, 0xFB5, 0xFFE9DBC2, 0xFFE9DBC2, 0xFFE9DBC2);
		EquipItem(Pocket.Armor, 0x3A9F, 0xFF021520, 0xFF06182D, 0xFF000000);
		EquipItem(Pocket.Shoe, 0x429F, 0xFF030B12, 0xFF000000, 0xFFFFFF);

		SetLocation(region: 23, x: 35759, y: 30738);

		SetDirection(93);
		SetStand("");
	}
    
    public override IEnumerable OnTalk(WorldClient c)
    {
        Msg(c, Options.FaceAndName, "Many intricate tattoos cover his dark, tanned skin.<br/>Beneath his shimmering white hair beams a calm, friendly smile.");
        MsgSelect(c, "Do you have business with me?", Button("Start Conversation", "@talk"), Button("Disembark", "@disembark"));

        var r = Wait();
        switch (r)
        {
            case "@talk":
            {
                Msg(c, "I'm Karis, the younger brother of Captain Carasek.<br/>It's good to meet you.<br/>Shall we set sail?");

            L_Keywords:
                Msg(c, Options.Name, "(Karis is waiting for me to say something.)");
                ShowKeywords(c);

                var keyword = Wait();

                Msg(c, "Can we change the subject?");
                goto L_Keywords;
            }
            case "@disembark":
            {
                Msg(c, "(Unimplemented)");
                End();
            }
        }
    }
}
