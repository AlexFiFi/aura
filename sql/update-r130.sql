CREATE TABLE IF NOT EXISTS `cooldowns` (
  `characterId` bigint(20) unsigned NOT NULL,
  `type` enum('ITEM','SKILL','QUEST','MISSION','PARTNER_ACTION') NOT NULL,
  `id` int(10) NOT NULL,
  `expires` datetime NOT NULL,
  `errorMessage` varchar(250) DEFAULT NULL,
  KEY `characterId` (`characterId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

ALTER TABLE `cooldowns`
  ADD CONSTRAINT `cooldowns_ibfk_1` FOREIGN KEY (`characterId`) REFERENCES `characters` (`characterId`) ON DELETE CASCADE ON UPDATE CASCADE;
