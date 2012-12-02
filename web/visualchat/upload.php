<?php
// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder
// 
// This is a very simple upload script, that works with the data coming from
// the client, when using the visual chat.
// --------------------------------------------------------------------------

$conf = array(
// --------------------------------------------------------------------------
// Configuration
// --------------------------------------------------------------------------

// Delete old images automatically after x seconds.
// (Checked while uploading, set to 0 to keep images.)
'auto_delete' => 0,

// --------------------------------------------------------------------------
);

// Auto delete old files.
if($conf['auto_delete'] > 0 && is_dir('img'))
{
	$files = scandir('img');
	foreach($files as $file)
	{
		$file = 'img/' . $file;
		if(is_file($file) && time() > filemtime($file) + $conf['auto_delete'])
			@unlink($file);
	}
}

if(empty($_POST) || empty($_FILES) || !isset($_FILES['filename']))
	exit;

// Only allow png files, named 'visualchat.png'.
if($_FILES['filename']['name'] !== 'visualchat.png' || $_FILES['filename']['type'] !== 'image/png')
	exit;

$tmpfile = $_FILES['filename']['tmp_name'];

// Check image properties (200% size is max for the client).
if(($size = @getimagesize($tmpfile)) === false || $size[0] > 256 || $size[1] > 96 || $size['mime'] !== 'image/png')
	exit;

// Create 'img/'.
if(!is_dir('img') && (!mkdir('img') || !touch('img/index.php')))
	exit;

$charname = $_POST['charname'];

// Check paramters.
if(!preg_match('~[0-9a-z_ ]~i', $charname))
	exit;

// Try to move file (file name format expected by the client:
// chat_<date:yyyymmdd_hhmmss>_<charname>.png)
$filepath = sprintf('img/chat_%s_%s.png', date('Ymd_His'), $charname);
if(!@move_uploaded_file($tmpfile, $filepath))
	exit;
	
chmod($filepath, 0644);

// Build URL and print it.
$url = (isset($_SERVER['HTTPS']) && strtolower($_SERVER['HTTPS']) !== 'off') ? 'https' : 'http';
$url .= '://'. $_SERVER['HTTP_HOST'];
$url .= str_replace(basename($_SERVER['SCRIPT_NAME']), '', $_SERVER['SCRIPT_NAME']);
$url .= $filepath;
echo $url;
