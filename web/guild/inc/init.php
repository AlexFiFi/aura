<?php
// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder
//
// Initilization of configuration and database class.

include '../shared/Conf.class.php';
include '../shared/MabiDb.class.php';

// Get database config
$conf = new Conf();
$conf->load('../../conf/database.conf');

// Db init
$db = new MabiDb();
$db->init($conf->get('database_host'), $conf->get('database_user'), $conf->get('database_pass'), $conf->get('database_db'));
