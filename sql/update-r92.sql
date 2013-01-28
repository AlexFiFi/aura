CREATE TABLE IF NOT EXISTS `quests` (
  `characterId` bigint(20) unsigned NOT NULL,
  `questId` bigint(20) unsigned NOT NULL,
  `questClass` int(10) unsigned NOT NULL,
  `state` tinyint(4) NOT NULL,
  PRIMARY KEY (`questId`),
  KEY `characterId` (`characterId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `quest_progress` (
  `characterId` bigint(20) unsigned NOT NULL,
  `questId` bigint(20) unsigned NOT NULL,
  `questClass` int(10) unsigned NOT NULL,
  `objective` varchar(100) NOT NULL,
  `count` int(11) unsigned NOT NULL,
  `done` tinyint(1) NOT NULL,
  `unlocked` tinyint(1) NOT NULL,
  PRIMARY KEY (`questId`,`objective`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


ALTER TABLE `quests`
  ADD CONSTRAINT `quests_ibfk_1` FOREIGN KEY (`characterId`) REFERENCES `characters` (`characterId`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `quest_progress`
  ADD CONSTRAINT `quest_progress_ibfk_1` FOREIGN KEY (`questId`) REFERENCES `quests` (`questId`) ON DELETE CASCADE ON UPDATE CASCADE;
