<?php
// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder
// 
// This is a simple page to create accounts.
// Due to the need of reading conf files this script has to be located
// inside the Aura web folder, or you have to adjust some paths.
// --------------------------------------------------------------------------

include '../shared/Conf.class.php';
include '../shared/MabiDb.class.php';

$conf = new Conf();
$conf->load('../../conf/database.conf');

$db = new MabiDb();
$db->init($conf->get('database.host'), $conf->get('database.user'), $conf->get('database.pass'), $conf->get('database.db'));

$succs = false;
$error = '';
$user  = '';
$pass  = '';
$pass2 = '';
if(isset($_POST['register']))
{
	$user  = $_POST['user'];
	$pass  = $_POST['pass'];
	$pass2 = $_POST['pass2'];
	
	if(!preg_match('~^[a-z0-9]{4,20}$~i', $user))
		$error = 'Invalid username (4-20 characters).';
	else if($pass !== $pass2)
		$error = 'Passwords don\'t match.';
	else if(!preg_match('~^[a-z0-9]{6,24}$~i', $pass))
		$error = 'Invalid password (6-24 characters).';
	else if($db->accountExists($user))
		$error = 'Username already exists.';
	else
	{
		$db->createAccount($user, $pass);
		$succs = true;
	}
}

?>
<!DOCTYPE HTML>
<html lang="en-US">
	<head>
		<meta charset="UTF-8">
		<title>Register - Aura</title>
		
		<link rel="stylesheet" href="../shared/css/reset.css" media="all" />
		<link rel="stylesheet" href="style.css" media="all" />
		
		<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
		<!--[if IE]>
		<script src="../shared/js/ie.js"></script>
		<![endif]-->
	</head>
	<body>
		<a href="<?php echo basename(__FILE__) ?>"><img src="../shared/images/logo_white.png" alt="Aura"/></a>
		
		<?php if(!$succs): ?>
			<?php if(!empty($error)): ?>
			<div class="notice">
				<?php echo $error ?>
			</div>
			<?php endif; ?>
			
			<form method="post" action="<?php echo basename(__FILE__) ?>">
				<table>
					<tr>
						<td class="desc">Username</td>
						<td class="inpt"><input type="text" name="user" value="<?php echo $user ?>"/></td>
					</tr>
					<tr>
						<td class="desc">Password</td>
						<td class="inpt"><input type="password" name="pass" value="<?php echo $pass ?>"/></td>
					</tr>
					<tr>
						<td class="desc">Password Repeat</td>
						<td class="inpt"><input type="password" name="pass2" value="<?php echo $pass2 ?>"/></td>
					</tr>
					<tr>
						<td class="desc"></td>
						<td class="inpt"><input type="submit" name="register" value="Register"/></td>
					</tr>
				</table>
			</form>
		<?php else: ?>
			<div class="notice">
				<?php echo 'Account created successfully!' ?>
			</div>
		<?php endif; ?>
	</body>
</html>
