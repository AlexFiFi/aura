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
}
