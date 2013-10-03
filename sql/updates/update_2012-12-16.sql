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


ALTER TABLE `contacts`
  ADD CONSTRAINT `contacts_ibfk_1` FOREIGN KEY (`characterId`) REFERENCES `characters` (`characterId`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `notes`
  ADD CONSTRAINT `notes_ibfk_1` FOREIGN KEY (`contactId`) REFERENCES `contacts` (`contactId`) ON DELETE CASCADE ON UPDATE CASCADE;