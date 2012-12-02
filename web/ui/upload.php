<?php
// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder
// 
// This is a very simple upload script, that works with the data coming from
// the client, when saving hotkey and window settings.
// --------------------------------------------------------------------------

if(empty($_POST) || empty($_FILES) || !isset($_FILES['ui']))
	exit;

// Only allow plain text files named '################.xml'.
if(!preg_match('~[0-9]{16}.xml~', $_FILES['ui']['name']) || $_FILES['ui']['type'] !== 'text/plain')
	exit;

$charid = $_POST['char_id'];
$server = $_POST['name_server'];

// Check paramters.
if(!preg_match('~[0-9]{16}~', $charid) || !preg_match('~[0-9a-z_ ]~i', $server))
	exit;
	
$key = substr($charid, -3);

// Create folder structure.
$folder = 'storage/';
if(!is_dir($folder) && (!mkdir($folder) || !touch($folder . 'index.php')))
	exit;
$folder .= $server . '/';
if(!is_dir($folder) && (!mkdir($folder) || !touch($folder . 'index.php')))
	exit;
$folder .= $key . '/';
if(!is_dir($folder) && (!mkdir($folder) || !touch($folder . 'index.php')))
	exit;

// Try to move file.
$filepath = $folder . $charid . '.xml';
if(!@move_uploaded_file($_FILES['ui']['tmp_name'], $filepath))
	exit;

chmod($filepath, 0644);
