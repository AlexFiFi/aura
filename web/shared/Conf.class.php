<?php
// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder

class Conf
{
	var $options = array();
	
	public function load($filePath)
	{
		if(!is_file($filePath))
			throw new Exception('File not found.');

		$file = fopen($filePath,'r');
		$required = false;
		$base = dirname($filePath);
		while(!feof($file))
		{
			$line = trim(fgets($file));
			
			if (strlen($line) < 3 || $line[0] == '!' || $line[0] == ';' || $line[0] == '#' || substr($line, 0, 2) === '//' || substr($line, 0, 2) === '--')
				continue;
				
			if(!strncmp($line, 'include ', 8) || ($required = !strncmp($line, 'require ', 8)))
			{
				try
				{
					$this->load($base.'/'.trim(substr($line, 8)));
				}
				catch(Exception $ex)
				{
					if($required)
						throw $ex;
				}
			}
			else
			{
				$pos = -1;
				if (($pos = stripos($line, '=')) === false && ($pos = stripos($line, ':')) === false && ($pos = stripos($line, ' ')) === false)
					continue;
				
				$this->options[trim(substr($line, 0, $pos))] = trim(substr($line, $pos + 1));
			}
		}
		fclose($file);
	}
	
	public function get($key, $default = '')
	{
		if(!isset($this->options[$key]))
			return $default;
		return $this->options[$key];
	}
	
	public function getInt($key, $default = 0)
	{
		if(!isset($this->options[$key]))
			return $default;
		return (int)$this->options[$key];
	}
	
	public function getBool($key, $default = false)
	{
		if(!isset($this->options[$key]))
			return $default;
		$val = $this->options[$key];
		return ($val === "true" || $val === "yes" || $val === "1");
	}
}