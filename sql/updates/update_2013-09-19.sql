CREATE TABLE IF NOT EXISTS `vars` (
  `varId` bigint(20) NOT NULL AUTO_INCREMENT,
  `accountId` varchar(50) NOT NULL,
  `characterId` bigint(20) NOT NULL DEFAULT '0',
  `name` varchar(64) NOT NULL,
  `type` char(2) NOT NULL,
  `value` mediumtext NOT NULL,
  PRIMARY KEY (`varId`),
  KEY `accountId` (`accountId`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

ALTER TABLE `vars`
  ADD CONSTRAINT `vars_ibfk_1` FOREIGN KEY (`accountId`) REFERENCES `accounts` (`accountId`) ON DELETE CASCADE ON UPDATE CASCADE;
