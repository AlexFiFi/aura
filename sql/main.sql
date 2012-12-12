CREATE DATABASE `aura` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci;
USE `aura`;

CREATE TABLE IF NOT EXISTS `accounts` (
  `accountId` varchar(50) NOT NULL,
  `password` varchar(64) NOT NULL,
  `authority` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `creation` datetime DEFAULT NULL,
  `lastlogin` datetime DEFAULT NULL,
  `lastip` varchar(16) NOT NULL DEFAULT '',
  `bannedreason` varchar(255) NOT NULL DEFAULT '',
  `bannedexpiration` datetime DEFAULT NULL,
  `points` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`accountId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `channels` (
  `server` varchar(50) NOT NULL,
  `name` varchar(12) NOT NULL,
  `ip` varchar(16) NOT NULL,
  `port` int(11) NOT NULL,
  `heartbeat` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE CURRENT_TIMESTAMP,
  `state` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `events` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `stress` tinyint(3) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`server`,`name`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

CREATE TABLE IF NOT EXISTS `character_cards` (
  `cardId` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `accountId` varchar(50) NOT NULL,
  `type` int(11) unsigned NOT NULL,
  PRIMARY KEY (`cardId`),
  KEY `account` (`accountId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

CREATE TABLE IF NOT EXISTS `characters` (
  `characterId` bigint(20) unsigned NOT NULL,
  `server` varchar(100) NOT NULL,
  `accountId` varchar(50) DEFAULT NULL,
  `name` varchar(50) NOT NULL,
  `type` enum('CHARACTER','PET') NOT NULL DEFAULT 'CHARACTER',
  `race` int(10) unsigned NOT NULL,
  `skinColor` tinyint(3) unsigned NOT NULL,
  `eyeType` tinyint(3) unsigned NOT NULL,
  `eyeColor` tinyint(3) unsigned NOT NULL,
  `mouthType` tinyint(3) unsigned NOT NULL,
  `status` int(10) unsigned NOT NULL,
  `height` double NOT NULL,
  `fatness` double NOT NULL,
  `upper` double NOT NULL,
  `lower` double NOT NULL,
  `region` int(10) unsigned NOT NULL,
  `x` int(10) unsigned NOT NULL,
  `y` int(10) unsigned NOT NULL,
  `direction` tinyint(3) unsigned NOT NULL,
  `battleState` tinyint(3) unsigned NOT NULL,
  `weaponSet` tinyint(3) unsigned NOT NULL,
  `life` double NOT NULL,
  `injuries` double NOT NULL,
  `lifeMax` double NOT NULL,
  `mana` double NOT NULL,
  `manaMax` double NOT NULL,
  `stamina` double NOT NULL,
  `staminaMax` double NOT NULL,
  `food` double NOT NULL,
  `level` smallint(5) unsigned NOT NULL DEFAULT '1',
  `totalLevel` int(10) unsigned NOT NULL DEFAULT '0',
  `experience` bigint(20) unsigned NOT NULL DEFAULT '0',
  `age` tinyint(3) unsigned NOT NULL,
  `strength` double NOT NULL,
  `dexterity` double NOT NULL,
  `intelligence` double NOT NULL,
  `will` double NOT NULL,
  `luck` double NOT NULL,
  `abilityPoints` smallint(5) unsigned NOT NULL,
  `attackMin` smallint(5) unsigned NOT NULL,
  `attackMax` smallint(5) unsigned NOT NULL,
  `wattackMin` smallint(5) unsigned NOT NULL,
  `wattackMax` smallint(5) unsigned NOT NULL,
  `critical` double NOT NULL,
  `protect` double NOT NULL,
  `defense` smallint(5) unsigned NOT NULL,
  `rate` smallint(5) unsigned NOT NULL,
  `strBoost` tinyint(3) unsigned NOT NULL,
  `dexBoost` tinyint(3) unsigned NOT NULL,
  `intBoost` tinyint(3) unsigned NOT NULL,
  `willBoost` tinyint(3) unsigned NOT NULL,
  `luckBoost` tinyint(3) unsigned NOT NULL,
  `lastTown` varchar(100) NOT NULL,
  `lastDungeon` varchar(100) NOT NULL,
  `birthday` datetime NOT NULL,
  `title` smallint(5) unsigned NOT NULL DEFAULT '0',
  `deletionTime` datetime DEFAULT NULL,
  `maxLevel` smallint(5) unsigned NOT NULL,
  `rebirthCount` smallint(5) unsigned NOT NULL,
  `jobId` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY (`characterId`),
  KEY `account` (`accountId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `id_pool` (
  `type` varchar(20) NOT NULL,
  `id` bigint(20) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`type`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `items` (
  `characterId` bigint(20) unsigned NOT NULL,
  `itemID` bigint(20) unsigned NOT NULL,
  `class` int(10) unsigned NOT NULL,
  `pocketId` tinyint(3) unsigned NOT NULL,
  `pos_x` int(10) unsigned NOT NULL,
  `pos_y` int(10) unsigned NOT NULL,
  `varint` int(10) unsigned NOT NULL,
  `color_01` int(10) unsigned NOT NULL,
  `color_02` int(10) unsigned NOT NULL,
  `color_03` int(10) unsigned NOT NULL,
  `price` int(10) unsigned NOT NULL,
  `bundle` smallint(5) unsigned NOT NULL,
  `linked_pocket` tinyint(3) unsigned NOT NULL,
  `figure` int(10) unsigned NOT NULL,
  `flag` tinyint(3) unsigned NOT NULL,
  `durability` int(10) unsigned NOT NULL,
  `durability_max` int(10) unsigned NOT NULL,
  `origin_durability_max` int(10) unsigned NOT NULL,
  `attack_min` smallint(5) unsigned NOT NULL,
  `attack_max` smallint(5) unsigned NOT NULL,
  `wattack_min` smallint(5) unsigned NOT NULL,
  `wattack_max` smallint(5) unsigned NOT NULL,
  `balance` tinyint(3) unsigned NOT NULL,
  `critical` tinyint(3) unsigned NOT NULL,
  `defence` int(10) unsigned NOT NULL,
  `protect` smallint(5) unsigned NOT NULL,
  `effective_range` smallint(5) unsigned NOT NULL,
  `attack_speed` tinyint(3) unsigned NOT NULL,
  `experience` smallint(5) unsigned NOT NULL,
  `exp_point` tinyint(3) unsigned NOT NULL,
  `upgraded` tinyint(3) unsigned NOT NULL,
  `upgraded_max` tinyint(3) unsigned NOT NULL,
  `grade` tinyint(3) unsigned NOT NULL,
  `prefix` smallint(5) unsigned NOT NULL,
  `suffix` smallint(5) unsigned NOT NULL,
  `data` varchar(3000) NOT NULL,
  `option` varchar(1000) NOT NULL,
  `sellingprice` int(10) unsigned NOT NULL,
  `expiration` int(10) unsigned NOT NULL,
  `update_time` datetime DEFAULT NULL,
  PRIMARY KEY (`itemID`),
  KEY `characterId` (`characterId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `keywords` (
  `keywordId` tinyint(3) unsigned NOT NULL,
  `characterId` bigint(20) unsigned NOT NULL,
  PRIMARY KEY (`keywordId`,`characterId`),
  KEY `keywords_ibfk_1` (`characterId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `pet_cards` (
  `cardId` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `accountId` varchar(50) DEFAULT NULL,
  `type` int(10) unsigned NOT NULL,
  PRIMARY KEY (`cardId`),
  KEY `account` (`accountId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

CREATE TABLE IF NOT EXISTS `sessions` (
  `accountId` varchar(50) NOT NULL,
  `session` bigint(20) unsigned NOT NULL,
  PRIMARY KEY (`accountId`,`session`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `skills` (
  `skillId` smallint(5) unsigned NOT NULL,
  `characterId` bigint(20) unsigned NOT NULL,
  `rank` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `exp` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`skillId`,`characterId`),
  KEY `skills_ibfk_1` (`characterId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


ALTER TABLE `character_cards`
  ADD CONSTRAINT `character_cards_ibfk_1` FOREIGN KEY (`accountId`) REFERENCES `accounts` (`accountId`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `characters`
  ADD CONSTRAINT `characters_ibfk_1` FOREIGN KEY (`accountId`) REFERENCES `accounts` (`accountId`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `items`
  ADD CONSTRAINT `items_ibfk_1` FOREIGN KEY (`characterId`) REFERENCES `characters` (`characterId`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `keywords`
  ADD CONSTRAINT `keywords_ibfk_1` FOREIGN KEY (`characterId`) REFERENCES `characters` (`characterId`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `pet_cards`
  ADD CONSTRAINT `pet_cards_ibfk_1` FOREIGN KEY (`accountId`) REFERENCES `accounts` (`accountId`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `skills`
  ADD CONSTRAINT `skills_ibfk_1` FOREIGN KEY (`characterId`) REFERENCES `characters` (`characterId`) ON DELETE CASCADE ON UPDATE CASCADE;
