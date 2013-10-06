USE `aura`;

CREATE TABLE IF NOT EXISTS `bank_accounts` (
  `accountId` varchar(50) NOT NULL,
  `password` varchar(64) DEFAULT NULL, -- For bank lock functionality
  `gold` int(11) DEFAULT 0 NOT NULL,
  `lastuse` datetime, -- Unsure
  PRIMARY KEY (`accountId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `bank_pockets` (
  `accountId` varchar(50) NOT NULL,
  `_index` tinyint(3) NOT NULL,
  `name` varchar(30) NOT NULL, -- If null, find character name and use it
  `enabled` tinyint(3) DEFAULT 0 NOT NULL,
  `width` int(11) unsigned DEFAULT 12 NOT NULL, -- Default bank pocket width
  `height` int(11) unsigned DEFAULT 8 NOT NULL,  -- Default bank pocket height
  PRIMARY KEY (`accountId`,`_index`),
  KEY `bank_pockets_ibfk_1` (`accountId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

ALTER TABLE `bank_pockets`
  ADD CONSTRAINT `bank_pockets_ibfk_1` FOREIGN KEY (`accountId`) REFERENCES `accounts` (`accountId`) ON DELETE CASCADE ON UPDATE CASCADE;