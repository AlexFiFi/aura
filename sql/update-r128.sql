ALTER TABLE  `characters` ADD  `optionTitle` SMALLINT( 5 ) UNSIGNED NOT NULL DEFAULT  '0' AFTER  `title`;

CREATE TABLE IF NOT EXISTS `titles` (
  `characterId` bigint(20) unsigned NOT NULL,
  `titleId` smallint(5) unsigned NOT NULL,
  `usable` tinyint(1) NOT NULL DEFAULT '0',
  KEY `characterId` (`characterId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

ALTER TABLE `titles`
  ADD CONSTRAINT `titles_ibfk_1` FOREIGN KEY (`characterId`) REFERENCES `characters` (`characterId`) ON DELETE CASCADE ON UPDATE CASCADE;