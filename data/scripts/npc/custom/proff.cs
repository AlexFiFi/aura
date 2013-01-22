using System;
using System.Collections;
using Common.World;
using World.Network;
using World.Scripting;
using World.Tools;
using World.World;
using Common.Constants;

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
		Msg(c, Options.FaceAndName,
			"A handsome young man stands before you. He has an air of intelligence and sophistication about him.",
			"His clear blue eyes are focused intently on the bottles in his hands, constantly pouring one into the other.",
			"He glances at you as you approach."
		);
		
	L_Start:
		MsgSelect(c, "Oh, hello there, " + c.Character.Name + ". What can I do for you today?", "Where am I?", "@whereami", "What commands can I use?", "@whatcommands", "About commands", "@aboutcmd", "End Conversation", "@endconvo");			
		
		var r = Wait();
		switch (r)
		{
			case "@whereami":
			{
				MsgSelect(c, "Heh, heh, I've often wondered that myself. However, I can tell you that you're on an Aura powered server.", "Continue", "@startingpoint");
				goto L_Start;
			}
			
			case "@whatcommands":
			{
				var commands = string.Join(", ", CommandHandler.Instance.GetAllCommandsForAuth(c.Account.Authority));
				MsgSelect(c, "Well, " + c.Character.Name + "... Let's see... At your current rank, the following commands are available to you: " + commands + ".<br/>Does that help to clear things up?", "Continue", "@startingpoint");
				goto L_Start;
			}
			
			case "@aboutcmd":
			{
				Msg(c,
					"Commands are special chat messages that let you interact with the server. Some, like the \"where\" command, give you information. Others actually affect the behavior of the server.",
					"<p/>You can execute a command by typing a '" + WorldConf.CommandPrefix + "' followed by the command name, and any arguments into the general chat, and pressing enter.",
					"<p/>For example, to execute a \"where\" command on this server, you'd enter the following into General Chat:", "", WorldConf.CommandPrefix + "where"
				);
				MsgSelect(c, "But remember, " + c.Character.Name + "... With great power comes great responsibility. " + ((c.Account.Authority > 0) ? "You've been given some extra powers, so " : "Should you ever get extra powers, ") + "use them wisely. Ranks can be removed as easily as they're given.", "Continue", "@startingpoint");
				goto L_Start;
			}
				
			case "@endconvo":
			{
				MsgSelect(c, "Is that all for now? Well, thanks for stopping by. Feel free to return any time.", "Continue", "@end");
				End();
			}
		}
	}
}
