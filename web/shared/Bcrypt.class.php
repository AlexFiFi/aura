<?php

class Bcrypt
{
	public static function hash($password, $work_factor = 7)
	{
		if(CRYPT_BLOWFISH !== 1)
			throw new Exception('Bcrypt requires PHP 5.3 or above');

		if($work_factor < 4 || $work_factor > 31)
			$work_factor = 7;
		
		$salt  = '$2a$';
		$salt .= str_pad($work_factor, 2, '0', STR_PAD_LEFT) . '$';
		$salt .= substr(strtr(base64_encode(self::getRandomBytes(16)), '+', '.'), 0, 22);
		
		return crypt($password, $salt);
	}

	public static function verify($password, $stored_hash)
	{
		if(CRYPT_BLOWFISH !== 1)
			throw new Exception('Bcrypt requires PHP 5.3 or above');

		if(substr($hash, 0, 4) != '$2a$')
			throw new Exception('Unsupported hash format');

		return crypt($password, $stored_hash) === $stored_hash;
	}

	private static function getRandomBytes($count)
	{
		$bytes = '';

		if(function_exists('openssl_random_pseudo_bytes') && (strtoupper(substr(PHP_OS, 0, 3)) !== 'WIN'))
		{
			$bytes = openssl_random_pseudo_bytes($count);
		}

		if($bytes === '' && is_readable('/dev/urandom') && ($hRand = @fopen('/dev/urandom', 'rb')) !== FALSE)
		{
			$bytes = fread($hRand, $count);
			fclose($hRand);
		}

		if(strlen($bytes) < $count)
		{
			$bytes = '';

			$randomState = microtime();
			if(function_exists('getmypid'))
				$randomState .= getmypid();

			for($i = 0; $i < $count; $i += 16)
			{
				$randomState = md5(microtime() . $randomState);

				if (PHP_VERSION >= '5')
					$bytes .= md5($randomState, true);
				else
					$bytes .= pack('H*', md5($randomState));
			}

			$bytes = substr($bytes, 0, $count);
		}

		return $bytes;
	}
}
