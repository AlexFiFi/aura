<?php
// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

class MySQL
{
	private $connection = null;
	
	public function connect($h, $u, $p)
	{
		$this->connection = @mysql_connect($h, $u, $p);
		if($this->connection === false)
			throw new exception('Unable to connect to MySQL server ('. mysql_error() .').');
	}
	
	public function select($db)
	{
		if(!@mysql_selectDb($db, $this->connection))
			throw new exception('Unable to select database ('. mysql_error($this->connection) .').');
	}
	
	public function query($sql)
	{
		if($this->connection === null)
			throw new exception('No connection.');
			
		if(($res = mysql_query($sql, $this->connection)) === false)
			throw new exception('Query error ('. mysql_error($this->connection) .').');
			
		return $res;
	}
	
	public function get($sql, $returnSingle = false)
	{
		$return = array();
		$query = $this->query($sql);
		
		while($dataset = mysql_fetch_array($query, MYSQL_ASSOC))
			$return[] = $dataset;
		
		if($returnSingle && count($return) == 1)
			$return = $return[0];
		
		return $return;
	}
	
	public function escape($val)
	{
		return @mysql_real_escape_string($val, $this->connection);
	}
}
