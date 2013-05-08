<?php
// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder
// 
// Lists all guilds according to the search parameters, in XML.

require_once 'inc/init.php';

define('NL', "\n");

echo '<?xml version="1.0" encoding="utf-16"?>'.NL;

foreach(array('Page', 'SortField', 'SortType', 'GuildLevelIndex', 'GuildMemberIndex', 'GuildType', 'SearchWord') as $p)
	if(!isset($_GET[$p]))
		die('<Guildlist RowCount="0" NowPage="1"></Guildlist>.NL');

$page      = $_GET['Page'];
$orderby   = $_GET['SortField'];
$ordertype = $_GET['SortType'];
$level     = $_GET['GuildLevelIndex'];
$members   = $_GET['GuildMemberIndex'];
$type      = $_GET['GuildType'];
$search    = $_GET['SearchWord'];

$list = $db->getGuildList($orderby, $ordertype, $level, $members, $type, $search);
$count = count($list);
$start = ($page - 1) * 10;

echo sprintf('<Guildlist RowCount="%s" NowPage="%s">'.NL, $count, $page);
for($i = 0; $i < min($count,10); ++$i)
{
	if(!isset($list[$i]))
		break;
		
	$guild = $list[$i];
	echo sprintf(
		'<Guild guildid="%s" levelindex="%s" membercnt="%s" guildname="%s" guildtype="%s" mastername="%s" />'.NL,
		$guild['guildId'], $guild['maxMembers'], $guild['memberCount'], $guild['guildName'], $guild['type'], $guild['leaderName']
	);
}

echo '</Guildlist>'.NL;