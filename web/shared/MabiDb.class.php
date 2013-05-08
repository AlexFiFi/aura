<?php
// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

require_once('MySQL.class.php');
require_once('Bcrypt.class.php');

class MabiDb
{
	private $mysql;
	
	public function __construct()
	{
		$this->mysql = new MySQL();
	}
	
	public function init($h, $u, $p, $db)
	{
		$this->mysql->connect($h, $u, $p);
		$this->mysql->select($db);
	}
	
	public function getAccount($accName)
	{
		$accName = mysql_real_escape_string($accName);
		
		$res = $this->mysql->get('SELECT accountId AS id, points, lastlogin, lastip FROM accounts WHERE accountId = "'. $accName .'"');
		return (count($res) > 0 ? $res[0] : null);
	}
	
	public function getAccountBySession($id)
	{
		$id = mysql_real_escape_string($id);
		
		$res = $this->mysql->get('SELECT accountId AS id, points, lastlogin, lastip FROM accounts WHERE session = "'. $id .'"');
		return (count($res) > 0 ? $res[0] : null);
	}
	
	public function accountExists($accName)
	{
		return ($this->getAccount($accName) !== null);
	}
	
	public function createAccount($name, $pass)
	{
		$name = $this->mysql->escape($name);
		$pass = $this->mysql->escape($pass);
		$creation = date('Y-m-d H:i:s', time());
		
		$pass = Bcrypt::hash(strtoupper(md5($pass)), 12);
		
		$this->mysql->query('INSERT INTO `accounts` (`accountId`, `password`, `creation`) VALUES ("'.$name.'", "'.$pass.'", "'.$creation.'")');
	}
	
	public function getCharacter($accountName, $sessionId, $characterId)
	{
		$accountName = mysql_real_escape_string($accountName);
		$sessionId = mysql_real_escape_string($sessionId);
		$characterid = mysql_real_escape_string($characterId);
		
		$res = $this->mysql->get('
			SELECT c.characterId AS id, gm.guildId AS guildId, gm.rank AS guildRank
			FROM characters AS c
			INNER JOIN accounts AS a ON c.accountId = a.accountId
			LEFT JOIN guild_members AS gm ON gm.characterId = c.characterId
			WHERE c.characterId = '. $characterId .' AND c.accountId = "'. $accountName .'" AND a.session = '. $sessionId .'
		');
		return (count($res) > 0 ? $res[0] : null);
	}
	
	public function getGuild($id)
	{
		$id = mysql_real_escape_string($id);
		
		$res = $this->mysql->get('
			SELECT g.*, gm.characterId AS leaderId
			FROM guilds AS g
			LEFT JOIN guild_members AS gm ON gm.guildId = g.guildId
			WHERE g.guildId =  '. $id .' AND g.guildId = gm.guildId AND gm.rank = 0
		');
		if(empty($res))
			return null;
		
		$res[0]['members'] = $this->getGuildMembers($id);
			
		return $res[0];
	}
	
	public function getGuildMembers($id)
	{
		$id = mysql_real_escape_string($id);
		
		$res = $this->mysql->get('
			SELECT gm.*, c.name AS name, IF(gm.rank > 5, 5, gm.rank) AS sortOrder
			FROM guild_members AS gm
			INNER JOIN characters AS c ON gm.characterId = c.characterId
			WHERE gm.guildId = '. $id .' AND gm.rank < 255
			ORDER BY sortOrder ASC, c.name ASC 
		');
		return $res;
	}
	
	public function acceptGuildApplication($guildId, $characterId)
	{
		$guildId = $this->mysql->escape($guildId);
		$characterId = $this->mysql->escape($characterId);
		
		$this->mysql->query('
			UPDATE guild_members
			SET rank = 5, messageFlags = messageFlags|1
			WHERE guildId = '. $guildId .' AND characterId = '. $characterId
		);
	}
	
	public function changeGuildMemberRank($guildId, $characterId, $rank)
	{
		$guildId = $this->mysql->escape($guildId);
		$characterId = $this->mysql->escape($characterId);
		$rank = $this->mysql->escape($rank);
		
		$this->mysql->query('
			UPDATE guild_members
			SET rank = '. $rank .', messageFlags = messageFlags|16
			WHERE guildId = '. $guildId .' AND characterId = '. $characterId
		);
	}
	
	public function removeGuildMember($guildId, $characterId)
	{
		$guildId = $this->mysql->escape($guildId);
		$characterId = $this->mysql->escape($characterId);
		
		$this->mysql->query('
			UPDATE guild_members
			SET messageFlags = messageFlags|IF(rank=254,2,8), rank = 255
			WHERE guildId = '. $guildId .' AND characterId = '. $characterId
		);
		
		return $this->mysql->affected();
	}
	
	public function updateGuild($guild)
	{
		$guild['intro'] = $this->mysql->escape($guild['intro']);
		$guild['welcome'] = $this->mysql->escape($guild['welcome']);
		$guild['leaving'] = $this->mysql->escape($guild['leaving']);
		$guild['rejection'] = $this->mysql->escape($guild['rejection']);
		
		$this->mysql->query('
			UPDATE guilds
			SET intro = "'. $guild['intro'] .'", welcome = "'. $guild['welcome'] .'",
				leaving = "'. $guild['leaving'] .'", rejection = "'. $guild['rejection'] .'"
			WHERE guildId = '. $guild['guildId']
		);
		
		return $this->mysql->affected();
	}
	
	public function getGuildList($orderby, $ordertype, $level, $members, $type, $search)
	{
		$orderby = $this->mysql->escape($ordertype);
		$ordertype = $this->mysql->escape($ordertype);
		$level = $this->mysql->escape($level);
		$members = $this->mysql->escape($members);
		$type = $this->mysql->escape($type);
		$search = $this->mysql->escape($search);
		
		$ordertype = ($ordertype == 1 ? 'DESC' : 'ASC');
		
		switch($orderby)
		{
			case 1: $orderby = 'g.level'; break;
			case 2: $orderby = 'memberCount'; break;
			case 3: $orderby = 'g.type'; break;
			default:
			case 4: $orderby = 'guildName'; break;
		}
		
		// members
		// type
		
		return $this->mysql->get('
			SELECT g.guildId, g.name AS guildName, g.type,
				   IF(g.level=0,5,IF(g.level=1,10,IF(g.level=2,20,IF(g.level=3,50,250)))) AS maxMembers,
				   COUNT(gm.characterId) AS memberCount, c.name AS leaderName
			FROM guilds AS g
			LEFT JOIN guild_members AS gm ON gm.guildId = g.guildId
			LEFT JOIN guild_members AS gm2 ON gm2.guildId = g.guildId
			LEFT JOIN characters AS c ON c.characterId = gm2.characterId
			WHERE gm.rank < 254 AND gm2.rank = 0 AND g.name LIKE "%'. $search .'%"
				'. ($level > 0 ? ' AND g.level = '. $level : '') .'
			GROUP BY g.guildId
			ORDER BY '. $orderby .' '. $ordertype .'
		');
	} 
}
