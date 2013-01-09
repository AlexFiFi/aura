using World.Scripting;
using World.Network;

public class SpawnTreeSample : BaseScript
{
	public override void OnLoad()
	{
		// Specify behavior for a tree near the Dugald portal in Tir.
		// 1: First parameter is the id of the prop (simply hit an
		//    unimplemented prop to see the id in the world server console.
		// 2: The region (takes numbers or region names from data/db/maps.txt).
		// 3: X coordinate
		// 4: Y coordinate
		// 5: Just your average LINQ delegate.
		//    Delegates in this form are like anonymous functions, that can be
		//    passed around to others. This function will automatically be used
		//    if the prop is used, aka hit, touched, etc. The three variables
		//    (cl, cr, pr), that can be used inside the function body, are a
		//    WorldClient, a MabiCreature, and a MabiProp object, so you have
		//    access to all the information you need.
		// The region and coordinates are required for Aura to check the
		// distance to the prop when it's being used. The information doesn't
		// have to be 100% correct, juse use >where or the GMCP while standing
		// by the prop.
		DefineProp(45036000568868929, "tir", 4700, 18300, (cl, cr, pr) =>
		{
			// Spawns between 5 and 10 spiders in a 300 radius around the
			// position of the creature, in the region the creature is in.
			Spawn(30003, (uint)Rnd(5,10), cr.Region, cr.GetPosition(), 300);
			
			// Shows a notice in the middle of the screen.
			Notice(cl, "You're surrounded!");
		});
	}
}
