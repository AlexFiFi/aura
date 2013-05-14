CREATE DATABASE `aura` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci;
USE `aura`;

CREATE TABLE IF NOT EXISTS `accounts` (
  `accountId` varchar(50) NOT NULL,
  `password` varchar(64) NOT NULL,
  `secPassword` VARCHAR(64) NULL DEFAULT NULL,
  `authority` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `creation` datetime DEFAULT NULL,
  `lastlogin` datetime DEFAULT NULL,
  `lastip` varchar(16) NOT NULL DEFAULT '',
  `bannedreason` varchar(255) NOT NULL DEFAULT '',
  `bannedexpiration` datetime DEFAULT NULL,
  `points` int(11) NOT NULL DEFAULT '0',
  `session` BIGINT UNSIGNED NOT NULL DEFAULT '0',
  `loggedIn` BOOLEAN NOT NULL DEFAULT FALSE,
  PRIMARY KEY (`accountId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `cards` (
  `cardId` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `accountId` varchar(50) NOT NULL,
  `type` int(11) unsigned NOT NULL,
  `race` INT UNSIGNED NOT NULL DEFAULT '0' ,
  `isGift` BOOLEAN NOT NULL DEFAULT FALSE ,
  `message` VARCHAR( 200 ) NULL DEFAULT NULL ,
  `sender` VARCHAR( 50 ) NULL DEFAULT NULL ,
  `senderServer` VARCHAR( 100 ) NULL DEFAULT NULL ,
  `receiver` VARCHAR( 50 ) NULL DEFAULT NULL ,
  `receiverServer` VARCHAR( 100 ) NULL DEFAULT NULL ,
  `added` DATETIME NULL DEFAULT NULL ,
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
  `optionTitle` SMALLINT( 5 ) unsigned NOT NULL DEFAULT '0',
  `talentTitle` SMALLINT( 5 ) unsigned NOT NULL DEFAULT  '0',
  `grandmasterTalent` TINYINT( 3 ) unsigned NOT NULL DEFAULT '255',
  `deletionTime` datetime DEFAULT NULL,
  `maxLevel` smallint(5) unsigned NOT NULL,
  `rebirthCount` smallint(5) unsigned NOT NULL,
  `jobId` tinyint(3) unsigned NOT NULL,
  `color1` int(10) unsigned NOT NULL DEFAULT '8421504',
  `color2` int(10) unsigned NOT NULL DEFAULT '8421504',
  `color3` int(10) unsigned NOT NULL DEFAULT '8421504',
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
  `tags` TEXT NOT NULL,
  PRIMARY KEY (`itemID`),
  KEY `characterId` (`characterId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `keywords` (
  `keywordId` tinyint(3) unsigned NOT NULL,
  `characterId` bigint(20) unsigned NOT NULL,
  PRIMARY KEY (`keywordId`,`characterId`),
  KEY `keywords_ibfk_1` (`characterId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `skills` (
  `skillId` smallint(5) unsigned NOT NULL,
  `characterId` bigint(20) unsigned NOT NULL,
  `rank` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `exp` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`skillId`,`characterId`),
  KEY `skills_ibfk_1` (`characterId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `contacts` (
  `contactId` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `characterId` bigint(20) unsigned NOT NULL,
  PRIMARY KEY (`contactId`),
  KEY `characterId` (`characterId`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

CREATE TABLE IF NOT EXISTS `notes` (
  `noteId` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `contactId` int(10) unsigned NOT NULL,
  `from` varchar(50) NOT NULL,
  `msg` text NOT NULL,
  `time` datetime NOT NULL,
  `read` tinyint(1) NOT NULL,
  PRIMARY KEY (`noteId`),
  KEY `contactId` (`contactId`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

CREATE TABLE IF NOT EXISTS `quests` (
  `characterId` bigint(20) unsigned NOT NULL,
  `questId` bigint(20) unsigned NOT NULL,
  `questClass` int(10) unsigned NOT NULL,
  `state` tinyint(4) NOT NULL,
  PRIMARY KEY (`questId`),
  KEY `characterId` (`characterId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `quest_progress` (
  `progressId` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `characterId` bigint(20) unsigned NOT NULL,
  `questId` bigint(20) unsigned NOT NULL,
  `questClass` int(10) unsigned NOT NULL,
  `objective` varchar(100) NOT NULL,
  `count` int(11) unsigned NOT NULL,
  `done` tinyint(1) NOT NULL,
  `unlocked` tinyint(1) NOT NULL,
  PRIMARY KEY (`progressId`),
  KEY `questId` (`questId`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

CREATE TABLE IF NOT EXISTS `guilds` (
  `guildId` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(12) NOT NULL,
  `intro` varchar(500) NOT NULL,
  `welcome` varchar(100) NOT NULL,
  `leaving` varchar(100) NOT NULL,
  `rejection` varchar(100) NOT NULL,
  `level` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `type` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `region` bigint(10) unsigned NOT NULL,
  `x` bigint(10) unsigned NOT NULL,
  `y` bigint(10) unsigned NOT NULL,
  `rotation` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `gp` int(10) unsigned NOT NULL DEFAULT '0',
  `gold` int(10) unsigned NOT NULL DEFAULT '0',
  `stone_type` bigint(10) unsigned NOT NULL DEFAULT '211',
  `title` varchar(50) NOT NULL,
  `options` tinyint(3) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`guildId`),
  UNIQUE KEY `name` (`name`),
  UNIQUE KEY `guildId` (`guildId`),
  KEY `guildId_2` (`guildId`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=216172782119026688 ;

CREATE TABLE IF NOT EXISTS `guild_members` (
  `characterId` bigint(20) unsigned NOT NULL,
  `guildId` bigint(20) unsigned NOT NULL,
  `rank` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `joined` datetime NOT NULL,
  `guildPoints` bigint(10) unsigned NOT NULL DEFAULT '0',
  `messageFlags` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `appMessage` varchar(100) NOT NULL DEFAULT '',
  PRIMARY KEY (`characterId`),
  UNIQUE KEY `characterId` (`characterId`),
  KEY `characterId_2` (`characterId`),
  KEY `guildId` (`guildId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `cooldowns` (
  `characterId` bigint(20) unsigned NOT NULL,
  `type` enum('ITEM','SKILL','QUEST','MISSION','PARTNER_ACTION') NOT NULL,
  `id` int(10) NOT NULL,
  `expires` datetime NOT NULL,
  `errorMessage` varchar(250) DEFAULT NULL,
  KEY `characterId` (`characterId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `titles` (
  `characterId` bigint(20) unsigned NOT NULL,
  `titleId` smallint(5) unsigned NOT NULL,
  `usable` tinyint(1) NOT NULL DEFAULT '0',
  KEY `characterId` (`characterId`),
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

ALTER TABLE `cards`
  ADD CONSTRAINT `cards_ibfk_1` FOREIGN KEY (`accountId`) REFERENCES `accounts` (`accountId`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `characters`
  ADD CONSTRAINT `characters_ibfk_1` FOREIGN KEY (`accountId`) REFERENCES `accounts` (`accountId`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `items`
  ADD CONSTRAINT `items_ibfk_1` FOREIGN KEY (`characterId`) REFERENCES `characters` (`characterId`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `keywords`
  ADD CONSTRAINT `keywords_ibfk_1` FOREIGN KEY (`characterId`) REFERENCES `characters` (`characterId`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `skills`
  ADD CONSTRAINT `skills_ibfk_1` FOREIGN KEY (`characterId`) REFERENCES `characters` (`characterId`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `contacts`
  ADD CONSTRAINT `contacts_ibfk_1` FOREIGN KEY (`characterId`) REFERENCES `characters` (`characterId`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `notes`
  ADD CONSTRAINT `notes_ibfk_1` FOREIGN KEY (`contactId`) REFERENCES `contacts` (`contactId`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `quests`
  ADD CONSTRAINT `quests_ibfk_1` FOREIGN KEY (`characterId`) REFERENCES `characters` (`characterId`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `quest_progress`
  ADD CONSTRAINT `quest_progress_ibfk_1` FOREIGN KEY (`questId`) REFERENCES `quests` (`questId`) ON DELETE CASCADE ON UPDATE CASCADE;
  
ALTER TABLE `guild_members`
  ADD CONSTRAINT `guild_members_ibfk_2` FOREIGN KEY (`guildId`) REFERENCES `guilds` (`guildId`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `guild_members_ibfk_1` FOREIGN KEY (`characterId`) REFERENCES `characters` (`characterId`) ON DELETE CASCADE ON UPDATE CASCADE;
  
ALTER TABLE `cooldowns`
  ADD CONSTRAINT `cooldowns_ibfk_1` FOREIGN KEY (`characterId`) REFERENCES `characters` (`characterId`) ON DELETE CASCADE ON UPDATE CASCADE;
  
ALTER TABLE `titles`
  ADD CONSTRAINT `titles_ibfk_1` FOREIGN KEY (`characterId`) REFERENCES `characters` (`characterId`) ON DELETE CASCADE ON UPDATE CASCADE;

CREATE TABLE IF NOT EXISTS `mail` (
  `messageId` BIGINT(20) UNSIGNED NOT NULL AUTO_INCREMENT ,
  `senderId` BIGINT(20) UNSIGNED NOT NULL ,
  `senderName` VARCHAR(50) NOT NULL ,
  `recipientId` BIGINT(20) UNSIGNED NOT NULL ,
  `recipientName` VARCHAR(50) NOT NULL ,
  `text` VARCHAR(150) NOT NULL ,
  `itemId` BIGINT(20) UNSIGNED NOT NULL ,
  `cashOnDemand` INT UNSIGNED NOT NULL ,
  `dateSent` DATETIME NOT NULL ,
  `read` TINYINT(1) UNSIGNED NOT NULL DEFAULT 1 ,
  `type` TINYINT(1) UNSIGNED NOT NULL DEFAULT 1 ,
  PRIMARY KEY (`messageId`) ,
  UNIQUE INDEX `messageId_UNIQUE` (`messageId` ASC) ,
  INDEX `recipientId` (`recipientId` ASC) ,
  INDEX `senderId` (`senderId` ASC) ,
  CONSTRAINT `recipientId`
    FOREIGN KEY (`recipientId` )
    REFERENCES `characters` (`characterId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `senderId`
    FOREIGN KEY (`senderId` )
    REFERENCES `characters` (`characterId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `itemId`
    FOREIGN KEY (`itemId` )
    REFERENCES `items` (`itemId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);
    
INSERT INTO `accounts` (`accountId`, `password`, `authority`, `creation`, `lastlogin`, `lastip`, `bannedexpiration`, `points`) VALUES ('Aura System', '0', 0, '0001-01-01 00:00:00', '0001-01-01 00:00:00', '', '0001-01-01 00:00:00', 0);

INSERT INTO `characters` (`characterId`, `server`, `accountId`, `name`, `type`, `race`, `skinColor`, `eyeType`, `eyeColor`, `mouthType`, `status`, `height`, `fatness`, `upper`, `lower`, `region`, `x`, `y`, `direction`, `battleState`, `weaponSet`, `life`, `injuries`, `lifeMax`, `mana`, `manaMax`, `stamina`, `staminaMax`, `food`, `level`, `totalLevel`, `experience`, `age`, `strength`, `dexterity`, `intelligence`, `will`, `luck`, `abilityPoints`, `attackMin`, `attackMax`, `wattackMin`, `wattackMax`, `critical`, `protect`, `defense`, `rate`, `strBoost`, `dexBoost`, `intBoost`, `willBoost`, `luckBoost`, `birthday`, `title`, `deletionTime`, `maxLevel`, `rebirthCount`, `jobId`, `color1`, `color2`, `color3`, `lastTown`, `lastDungeon`) VALUES (1, 'Aura', 'Aura System', 'Mail System', 'CHARACTER', 10001, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 10, 0, 10, 10, 10, 10, 10, 0, 1, 0, 0, 17, 10, 10, 10, 10, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, '0001-01-01 00:00:00', 0, '0001-01-01 00:00:00', 200, 0, 0, 0, 0, 0, '', '');

INSERT INTO `items` (`characterId`, `itemID`, `class`, `pocketId`, `pos_x`, `pos_y`, `varint`, `color_01`, `color_02`, `color_03`, `price`, `bundle`, `linked_pocket`, `figure`, `flag`, `durability`, `durability_max`, `origin_durability_max`, `attack_min`, `attack_max`, `wattack_min`, `wattack_max`, `balance`, `critical`, `defence`, `protect`, `effective_range`, `attack_speed`, `experience`, `exp_point`, `upgraded`, `upgraded_max`, `grade`, `prefix`, `suffix`, `data`, `option`, `sellingprice`, `expiration`, `update_time`, `tags`) VALUES (1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, '', '', 0, 0, '2012-12-23 00:00:00', '');
