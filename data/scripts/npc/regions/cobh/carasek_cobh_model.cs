// Aura Script
// --------------------------------------------------------------------------
// Carasek - Captain
// --------------------------------------------------------------------------

using System;
using System.Collections;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class Carasek_cobh_modelScript : NPCScript
{
	public override void OnLoad()
	{
		base.OnLoad();
		SetName("_carasek_cobh_model");
		SetRace(10002);
		SetBody(height: 1.2f, fat: 1f, upper: 1.29f, lower: 1f);
		SetFace(skin: 26, eye: 9, eyeColor: 46, lip: 18);

		SetColor(0x0, 0x0, 0x0);

		EquipItem(Pocket.Face, 0x1324, 0xDBFEA, 0x9BB286, 0x5F004C);
		EquipItem(Pocket.Hair, 0xFB5, 0xFFE9DBC2, 0xFFE9DBC2, 0xFFE9DBC2);
		EquipItem(Pocket.Armor, 0x3A9F, 0xFF021520, 0xFF06182D, 0xFF000000);
		EquipItem(Pocket.Shoe, 0x429F, 0xFF030B12, 0xFF000000, 0xFFFFFF);

		SetLocation(region: 23, x: 34811, y: 43369);

		SetDirection(205);
		SetStand("");
	}
    public override IEnumerable OnTalk(WorldClient c)
    {
        Msg(c, Options.FaceAndName, "He has the provocative complexion of dark rum and has a detailed tattoo on him<br/>And with his white hair like the foam of the ocean waters<br/>he smiles a gentle smile that is much like the wet sand on the beach.");
        MsgSelect(c, "Seems like you want to talk to me about something.", Button("Start Conversation", "@talk"), Button("Disembark", "@disembark"));

        var r = Wait();
        switch (r)
        {
            case "@talk":
            {
                Msg(c, "I'm Carasek, the captain of this ship. Welcome aboard. Let's set sail!");

            L_Keywords:
                Msg(c, Options.Name, "(Carasek is paying attention to me.)");
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
