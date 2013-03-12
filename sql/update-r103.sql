RENAME TABLE `character_cards` TO `cards` ;

ALTER TABLE `cards`
ADD `race` INT UNSIGNED NOT NULL DEFAULT '0' ,
ADD `isGift` BOOLEAN NOT NULL DEFAULT FALSE ,
ADD `message` VARCHAR( 200 ) NULL DEFAULT NULL ,
ADD `sender` VARCHAR( 50 ) NULL DEFAULT NULL ,
ADD `senderServer` VARCHAR( 100 ) NULL DEFAULT NULL ,
ADD `receiver` VARCHAR( 50 ) NULL DEFAULT NULL ,
ADD `receiverServer` VARCHAR( 100 ) NULL DEFAULT NULL ,
ADD `added` DATETIME NULL DEFAULT NULL ;

INSERT INTO `cards` (`accountId`, `type`, `race`)
SELECT `accountId`, 102, `type` FROM `pet_cards` ;

DROP TABLE `pet_cards` ;
