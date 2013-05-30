using Aura.World.Scripting;

public class BangorPortals : BaseScript
{
	public override void OnLoad()
	{
		DefineProp(45036129417756692, "bangor", 13103, 24027, PropWarp("gairech", 39167, 17906));
		DefineProp(45036125123248153, "gairech", 39171, 16618, PropWarp("bangor", 13083, 23128));
		DefineProp(45036129417756782, "bangor", 13179, 15447, PropWarp("barri_altar", 3210, 2441));
		DefineProp(45036133712723974, "barri_altar", 3173, 1551, PropWarp("bangor", 13167, 15202));
		DefineProp(45036129417756810, "bangor", 12837, 2891, PropWarp("morva_aisle", 18358, 35028));
		DefineProp(45036408590565392, "morva_aisle", 18412, 36408, PropWarp("bangor", 12810, 5323));
	}
}
