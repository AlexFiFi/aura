using Aura.World.Scripting;
using Aura.World.Network;

public class TirPortals : BaseScript
{
	public override void OnLoad()
	{
		DefineProp(45036533145010804, "tir_tin", 27753, 72762, PropWarp("tir", 15388, 38706));

		DefineProp(45036000569263146, "tir", 11300, 39744, PropWarp("tir_bank", 2102, 1184));
		DefineProp(45036004863705089, "tir_bank", 2249, 1031, PropWarp("tir", 11475, 39606));

		DefineProp(45036000569262524, "tir", 16075, 37997, PropWarp("tir_duncan", 2351, 1880));
		DefineProp(45036009158672385, "tir_duncan", 2325, 2058, PropWarp("tir", 16061, 38154));

		DefineProp(45036000569262795, "tir", 5843, 37600, PropWarp("tir_church", 1693, 687));
		DefineProp(45036013453639682, "tir_church", 1779, 457, PropWarp("tir", 5888, 37471));

		DefineProp(45036000569262904, "tir", 11259, 37258, PropWarp("tir_grocery", 2310, 1712));
		DefineProp(45036017748606977, "tir_grocery", 2448, 1705, PropWarp("tir", 11482, 37274));
		
		DefineProp(45036000569262443, "tir", 13767, 44662, PropWarp("tir_healer", 664, 854));
		DefineProp(45036022043574273, "tir_healer", 516, 826, PropWarp("tir", 13575, 44611));

		DefineProp(45036000569262537, "tir", 15850, 33972, PropWarp("tir_inn", 1419, 685));
		DefineProp(45036026338541569, "tir_inn", 1416, 542, PropWarp("tir", 15814, 33691));
		
		DefineProp(45036000569262320, "tir", 13218, 36376, PropWarp("tir_general", 845, 2440));
		DefineProp(45036030633508867, "tir_general", 725, 2576, PropWarp("tir", 13113, 36454));

		DefineProp(45036000569262997, "tir", 3877, 32969, PropWarp("tir_magic", 2348, 886));
		DefineProp(45036034928476161, "tir_magic", 2479, 949, PropWarp("tir", 4093, 32909));

		DefineProp(45036000568999945, "tir", 27690, 30381, PropWarp("ciar_altar", 3886, 3297));
		DefineProp(45036043518410754, "ciar_altar", 4433, 3194, PropWarp("tir", 28761, 30725));

		DefineProp(45036000569196606, "tir", 9748, 60215, PropWarp("alby_altar", 3197, 2518));
		DefineProp(45036052108345350, "alby_altar", 3177, 1982, PropWarp("tir", 9756, 59227));

		DefineProp(45036000568868967, "tir", 4916, 15545, PropWarp("dugald_aisle", 28545, 96881));
		DefineProp(45036064993312808, "dugald_aisle", 28517, 98372, PropWarp("tir", 5067, 17156));

		DefineProp(45036000569196647, "tir", 1516, 59999, PropWarp("sidhe_north", 9985, 6522));
		DefineProp(45036198137233609, "sidhe_north", 10007, 5680, PropWarp("tir", 1748, 59187));

		DefineProp(45040308420935876, "beginner_tutorial", 22300, 20400, PropWarp("tir", 12785, 38383));
		DefineProp(45040308420936241, "beginner_tutorial", 22614, 20382, PropWarp("tir", 12785, 38383));
		DefineProp(45040308420935963, "beginner_tutorial", 17948, 14155, PropWarp("beginner_tutorial", 18517, 12543));
		DefineProp(45040308420935964, "beginner_tutorial", 18310, 13104, PropWarp("beginner_tutorial", 17654, 14772));
		
		DefineProp(45036515964813322, "ciar_hard_altar", 3202, 1795, PropWarp("ciar_altar", 3193, 4319));
		DefineProp(45036043518410789, "ciar_altar", 3197, 4595, (cl, cr, pr) =>
		{
			if(cr.LevelTotal >= 250)
				cl.Warp("ciar_hard_altar", 3206, 2085);
			else
				cl.Send(PacketCreator.Notice(cr, "You need a cumulative level of at least 250."));
		});

		// TNN indoor -> Tir ...?
		//DefineProp(45036150892593154, "tir_na_nog_duncan", 2325, 2058, PropWarp("tir", 16061, 38154));
		//DefineProp(45036155187560450, "tir_na_nog_church", 1779, 457, PropWarp("tir", 5888, 37471));
		//DefineProp(45036159482527745, "tir_na_nog_grocery", 2448, 1705, PropWarp("tir", 11482, 37274));
		//DefineProp(45036163777495042, "tir_na_nog_healer", 516, 826, PropWarp("tir", 13575, 44611));
		//DefineProp(45036168072462338, "tir_na_nog_inn", 1416, 542, PropWarp("tir", 15814, 33691));
		//DefineProp(45036172367429633, "tir_na_nog_general", 725, 2576, PropWarp("tir", 13113, 36454));
		//DefineProp(45036176662396930, "tir_na_nog_magic", 2479, 949, PropWarp("tir", 4093, 32909));
		//DefineProp(45036180957364226, "tir_na_nog_bank", 2249, 1031, PropWarp("tir", 11475, 39606));
	}
}
