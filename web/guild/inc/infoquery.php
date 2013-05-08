<?php
// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder
// 
// Querying of basic information required for index and action.

$req = $_GET + $_POST;

// Check required parameters
if(!isset($req['guildid']) || !isset($req['userid']) || !isset($req['userchar']) || !isset($req['key']))
	die('<!-- missing info -->');

// Check data validity
if(!preg_match('~[0-9]+~', $req['guildid']) || !preg_match('~[0-9]+~', $req['userchar']) || !preg_match('~[0-9]+~', $req['key']))
	die('<!-- data error -->');

// Get parameters
$guildId = $req['guildid'];
$accountName = $req['userid'];
$characterId = $req['userchar'];
$sessionId = $req['key'];

$guild = null;
$character = null;
$isLeader  = false;
$isOfficer = false;

// Get guild
$guild = $db->getGuild($guildId);
if($guild !== null)
{
	// Get character
	$character = $db->getCharacter($accountName, $sessionId, $characterId);
	if($character !== null)
	{
		// Check if this is the character's guild
		if($character['guildId'] === $guildId)
		{
			// Leader (0)? Officer (1)?
			$isLeader  = ($character['guildRank'] == 0);
			$isOfficer = ($character['guildRank'] == 1);
		}
	}
}