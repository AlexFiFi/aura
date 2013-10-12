using System.Collections;
using Aura.Data;
using Aura.Shared.Const;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.World;

public class NaoScript : NPCScript
{
	private const bool SelectTalent = false;

	public override void OnLoad()
	{
		// Nao is the only NPC that needs a specific id so far.
		SetId(Id.Nao);
		SetName("_nao");
		SetRace(1);
		SetLocation(1000, 0, 0);
	}

	// Ending the talk with Nao means leaving the Soul Stream.
	// The client does that automatically.
	public override IEnumerable OnTalk(WorldClient c)
	{
		Bgm(c, "Nao_talk.mp3");

		Intro(c,
			"A beautiful girl in a black dress with intricate patterns.",
			"Her deep azure eyes remind everyone of an endless blue sea full of mystique.",
			"With her pale skin and her distinctively sublime silhouette, she seems like she belongs in another world."
		);

		Msg(c, "Hello, there... You are <username/>, right?<br/>I have been waiting for you.<br/>It's good to see a " + (c.Character.IsMale ? "gentleman" : "lady") + " like you here.<p/>My name is Nao. <br/>It is my duty to lead pure souls like yours to Erinn.");
		Msg(c, "<username/>, we have some time before I guide you to Erinn.<br/>Do you have any questions for me?", Button("No"), Button("Yes"));
		var r = Select(c);

		// Information about Mabi
		if (r == "@yes")
		{
		L_Info:
			//Do not hesitate to ask questions. I am more than happy to answer them for you.
			//If you have any questions before heading off to Erinn, please feel free to ask.
			Msg(c, "If there is something you'd like to know more of, please ask me now.",
				Button("End Conversation"),
				List("Talk to Nao", 4, "@endconv",
					Button("About Mabinogi", "@mabinogi"),
					Button("About Erinn", "@erinn"),
					Button("What to do?", "@what"),
					Button("About Adventures", "@adventures")
				)
			);

			var info = Select(c);
			switch (info)
			{
				case "@mabinogi":
					Msg(c, "Mabinogi can be defined as the songs of bards, although in some cases, the bards themselves are referred to as Mabinogi.<br/>To the residents at Erinn, music is a big part of their lives and nothing brings joy to them quite like music and Mabinogi.<br/>Once you get there, I highly recommend joining them in composing songs and playing musical instruments.");
					goto L_Info;
				case "@erinn":
					Msg(c, "Erinn is the name of the place you will be going to, <username/>.<br/>The place commonly known as the world of Mabinogi is called Erinn.<br/>It has become so lively since outsiders such as yourself began to come.");
					Msg(c, "Some time ago, adventurers discovered a land called Iria,<br/>and others even conquered Belvast Island, between the continents.<br/>Now, these places have become home to adventurers like yourself, <username/>.<p/>You can go to Tir Chonaill of Uladh now,<br/>but you should try catching a boat from Uladh and<br/>crossing the ocean to Iria or Belvast Island.");
					goto L_Info;
				case "@what":
					Msg(c, "That purely depends on what you wish to do.<br/>You are not obligated to do anything, <username/>.<br/>You set your own goals in life, and pursue them during your adventures in Erinn.<p/>Sure, it may be nice to be recognized as one of the best, be it the most powerful, most resourceful, etc., but <br/>I don't believe your goal in life should necessarily have to be becoming 'the best' at everything.<br/>Isn't happiness a much better goal to pursue?<p/>I think you should experience what Erinn has to offer <br/>before deciding what you really want to do there.");
					goto L_Info;
				case "@adventures":
					Msg(c, "There are so many things to do and adventures to go on in Erinn.<br/>Hunting and exploring dungeons in Uladh...<br/>Exploring the ruins of Iria...<br/>Learning the stories of the Fomors in Belvast...<p/>Explore all three regions to experience brand new adventures!<br/>Whatever you wish to do, <username/>, if you follow your heart,<br/>I know you will become a great adventurer before you know it!");
					goto L_Info;
			}
		}

		// Talent selection
		//if (SelectTalent)
		//{
		//	Msg(c, "<username/>, you have the freedom to do whatever you wish in life.<br/>But having a goal will make life more meaningful, don't you think?<br/>This goal, also known as a Talent, will help you train specific skills.<br/>It will also grant bonuses to related stats.<p/><username/>, would you like to choose an active Talent?<br/>You can change this Talent anytime you rebirth.<br/>So take a look, and see which Talent you'd like. <talent_select />");
		//
		//	var talent = Select(c);
		//	//switch(talent)
		//	//{
		//	//    case "@talent7": ...
		//	//}
		//
		//	Msg(c, "<talent_select hide='true' />");
		//	Msg(c, "You've selected Music as your active Talent.<br/>Here, take this instrument as a gift from me.<p/>Remember, you can change your active Talent anytime<br/>you rebirth. I wish you the best on your new journey, <username/>.");
		//}

		// End
		Msg(c, "Soon, you'll be able to go to Erinn.");
		Msg(c, "When you arrive in Erinn, please go see Chief Duncan<br/>and show him my letter of introduction.<br/>I'm sure he will be a great help to you.");
		Msg(c, Options.FaceAndName, "(Received Bread, Traveler's Guide, and Soul Stones from Nao.)", Image("Novice_item_g9Korea"));
		Msg(c, "I wish you the best of luck in Erinn.<br/>See you around.", Button("End Conversation"));
		r = Select(c);

		// Move to Uladh Beginner Area
		c.Character.SetLocation(125, 21489, 76421);
		c.Character.Direction = 233;
		
		GiveItem(c, 1000,  1); // Traveler's Guide
		GiveItem(c, 50004, 1); // Bread
		GiveItem(c, 85539, 3); // Nao's Soul Stone for Beginners
		
		StartQuest(c, 200501); // Nao's Letter of Introduction
		
		Close(c);

		Return();
	}
}

public class TinScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_tin");
		SetRace(10002);
		SetBody(height: .1f);
		SetFace(skin: 15, eye: 15, eyeColor: 47, lip: 0);
		SetStand("human/male/anim/male_stand_Tarlach_anguish");
		SetLocation("tir_beginner", 22211, 74946, 44);

		EquipItem(Pocket.Face, 4900);
		EquipItem(Pocket.Hair, 4021, 0xA64742);
		EquipItem(Pocket.Armor, 15069, 0xFFF7E1, 0xBC3412, 0x40460F);
		EquipItem(Pocket.Shoe, 17010, 0xAC6122, 0xFFFFFF, 0xFFFFFF);
		EquipItem(Pocket.Head, 18518, 0xA58E74, 0xFFFFFF, 0xFFFFFF);
	}

	public override IEnumerable OnTalk(WorldClient c)
	{
		Msg(c, "Hey, who are you?");
		Msg(c, "You don't look like you're from this world. Am I right?<br/>Did you make your way down here from Soul Stream?<br/>Ahhh, so Nao sent you here!");
		Msg(c, "She's way too obedient to the Goddess' wishes.<br/>Anyway, she's a good girl, so be nice to her.");
		//Msg(c, "And this... is just for you.");
		//Msg(c, "The weapon I gave you is a Spirit Weapon.<br/>If you hold it in your hand, you can talk to the spirit in the sword.<br/>I'm lending it to make your stay here much easier,<br/>so use it wisely.");
		//Msg(c, "You're wondering how to give it back to me, aren't you?<br/>Don't worry. When the right time comes, it will leave you of its own accord.");
		//Msg(c, "Oh my! I almost forgot.<br/>I just gave you the Spirit Weapon and almost forgot to introduce you to the spirit. <p/>The spirit's name is Eiry.<br/> If you want to talk to her, simply click on the wing-shaped button on the bottom right side of your screen.");

		Return();
	}
}

public class TinPortalTir : BaseScript
{
	public override void OnLoad()
	{
		DefineProp(45036533145010200, 125, 27651, 72620, (c, cr, pr) =>
		{
			c.Warp(1, 15250, 38467);
		});
	}
}
