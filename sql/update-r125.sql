DROP TABLE IF EXISTS `guilds`;
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

DROP TABLE IF EXISTS `guild_members`;
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


ALTER TABLE `guild_members`
  ADD CONSTRAINT `guild_members_ibfk_2` FOREIGN KEY (`guildId`) REFERENCES `guilds` (`guildId`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `guild_members_ibfk_1` FOREIGN KEY (`characterId`) REFERENCES `characters` (`characterId`) ON DELETE CASCADE ON UPDATE CASCADE;