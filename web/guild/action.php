<?php
// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder
// 
// To be called from the site dynamically to update the database.

require_once 'inc/init.php';
require_once 'inc/infoquery.php';

if($guild === null || $character === null)
{
	echo json_encode(array('success' => false));
	exit;
}

// Functions
{
	$success = false;
	
	// Deleting/Leaving
	if(isset($_GET['delete']))
	{
		$toDelete = $_GET['delete'];
		
		// Leaders may delete everybody but themselves
		if($isLeader && $character['id'] !== $toDelete && $toDelete !== $guild['leaderId'])
		{
			//echo 'DELETE BY LEADER: ' . $toDelete;
			if($db->removeGuildMember($guildId, $toDelete) > 0)
				$success = true;
		}
		// Officers may delete everybody but the leader
		elseif($isOfficer && $toDelete !== $guild['leaderId'])
		{
			//echo 'DELETE BY OFFICER: ' . $toDelete;
			if($db->removeGuildMember($guildId, $toDelete) > 0)
				$success = true;
		}
		// Members may only delete themselves
		elseif($character['id'] === $toDelete)
		{
			//echo 'LEAVING: ' . $toDelete;
			if($db->removeGuildMember($guildId, $toDelete) > 0)
				$success = true;
		}
	}

	// Accepting
	elseif(isset($_GET['accept']) && ($isLeader || $isOfficer))
	{
		$toAccept = $_GET['accept'];
		
		$db->acceptGuildApplication($guild['guildId'], $toAccept);
		
		$success = true;
	}

	// Change rank
	elseif(isset($_GET['rank']) && $isLeader)
	{
		$rank = $_GET['rank'];
		$toChange = $_GET['id'];
		
		$db->changeGuildMemberRank($guild['guildId'], $toChange, $rank);
		
		$success = true;
	}

	// Settings
	elseif(isset($_POST['save']) && ($isLeader || $isOfficer))
	{
		$guild['intro'] =     substr($_POST['intro'], 0, 500);
		$guild['welcome'] =   substr($_POST['welcome'], 0, 100);
		$guild['leaving'] =   substr($_POST['leaving'], 0, 100);
		$guild['rejection'] = substr($_POST['rejection'], 0, 100);
	
		$db->updateGuild($guild);
		
		$success = true;
	}
	
	echo json_encode(array('success' => $success));
}
