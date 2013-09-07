using System;
using System.Collections;
using Aura.World.Network;
using Aura.World.Scripting;
using Aura.World.Util;
using Aura.World.World;
using Aura.Shared.Const;

public class ProffScript : NPCScript
{
	public override void OnLoad()
	{
		SetName("_<mini>NPC</mini> The Proffessor");
		SetDialogName("The Proffessor");
		SetRace(10002);
		SetBody(height: 1f);
		SetFace(skin: 18, eye: 5, eyeColor: 54, lip: 12);
		SetStand("chapter4/human/anim/male_alchemists_stand_idle01.ani");
		SetLocation("tir", 13650, 38400, 135);

		EquipItem(Pocket.Hair, 4955, 0x1000005A, 0, 0);
		EquipItem(Pocket.Face, 4909, 18, 0, 0);
		EquipItem(Pocket.Robe, "Lava Cat Robe", 0x077acb, 0x001647, 0xFFFFFF);
		EquipItem(Pocket.Shoe, "Dustin Silver Knight Greaves", 0x676767);
		EquipItem(Pocket.RightHand1, "Bottled Water", 0x49006020);

		Phrases.Add("Hello there.");
		Phrases.Add("Come with me... I shall show you a future in ruins.");
		Phrases.Add("E = mc² actually stands for Enjoyment = (Modifying the Client)²");
		Phrases.Add("Hmm... I haven't checked in with FIONA lately...");
		Phrases.Add("My latest creation is almost ready!");
	}
	
	public override IEnumerable OnTalk(WorldClient c)
	{
		Intro(c,
			"A handsome young man stands before you. He has an air of intelligence and sophistication about him.",
			"His clear blue eyes are focused intently on the bottles in his hands, constantly pouring one into the other.",
			"He glances at you as you approach."
		);
		
	L_Start:
		Msg(c, "Oh, hello there, <username/>. What can I do for you today?", Button("Where am I?", "@whereami"), Button("What commands can I use?", "@whatcommands"), Button("About commands", "@aboutcmd"), Button("End Conversation", "@endconvo"));			
		
		var r = Select(c);
		switch (r)
		{
			case "@whereami":
			{
				Msg(c, "Heh, heh, I've often wondered that myself. However, I can tell you that you're on an Aura powered server.");
				goto L_Start;
			}
			
			case "@whatcommands":
			{
				var commands = string.Join(", ", CommandHandler.Instance.GetAllCommandsForAuth(c.Account.Authority));
				Msg(c, "Well, <username/>... Let's see... At your current rank, the following commands are available to you: " + commands + ".<br/>Does that help to clear things up?");
				goto L_Start;
			}
			
			case "@aboutcmd":
			{
				Msg(c, "Commands are special chat messages that let you interact with the server. Some, like the \"where\" command, give you information. Others actually affect the behavior of the server.");
				Msg(c, "You can execute a command by typing a '" + WorldConf.CommandPrefix + "' followed by the command name, and any arguments into the general chat, and pressing enter.");
				Msg(c, "For example, to execute a \"where\" command on this server, you'd enter the following into General Chat:<br/><br/>" + WorldConf.CommandPrefix + "where");
				Msg(c, "But remember, <username/>... With great power comes great responsibility. " + ((c.Account.Authority > 0) ? "You've been given some extra powers, so " : "Should you ever get extra powers, ") + "use them wisely. Ranks can be removed as easily as they're given.");
				goto L_Start;
			}
			
			case "@endconvo":
			{
				Msg(c, "Is that all for now? Well, thanks for stopping by. Feel free to return any time.");
				End();
			}
		}
	}
}
