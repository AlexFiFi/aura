<?php
// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder
// 
// Simple guild management site, to accept, delete, leave, change ranks, etc.

require_once 'inc/init.php';
require_once 'inc/infoquery.php';

if($guild === null)
	error('Unknown guild.');
	
if($character === null)
	error('Unknown character.');

// Left joined rank is null if no guild
if($character['guildRank'] === null)
	error('You are not a member of any guild.');

// 254 = Application
if($character['guildRank'] == 254)
	error('Your application is being reviewed.');

// 255 = Deleted, Left, or Declined.
if($character['guildRank'] == 255)
	error('You are no longer a member of this guild, or your application has been denied.');

// Render
include 'templates/layout.htm';

// Called if errors occure. Renders layout with the given msg
// and exits afterwards.
function error($error)
{
	include 'templates/layout.htm';
	exit;
}

// Returns guild type as string
function getGuildType($id)
{
	switch($id)
	{
		case 0: return 'Battle';
		case 1: return 'Adventure';
		case 2: return 'Manufacturing';
		case 3: return 'Commerce';
		case 4: return 'Social';
		default:return 'Other';
	}
}

// Returns guild level as string
function getGuildLevel($id)
{
	switch($id)
	{
		case 0: return 'Beginner';
		case 1: return 'Basic';
		case 2: return 'Advanced';
		case 3: return 'Great';
		case 4: return 'Grand';
		default:return 'Unknown';
	}
}

// Returns a short description of the rank
function getRankShort($rank)
{
	switch($rank)
	{
		case 0:   return 'leader';
		case 1:   return 'officer';
		case 2:   return 'bronze';
		case 3:   return 'senior';
		case 4:   return 'unkmember';
		case 5:   return 'member';
		case 254: return 'applied';
		case 255: return 'declined';
		default:return 'unk';
	}
}
