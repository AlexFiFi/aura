CREATE TABLE IF NOT EXISTS `aura`.`mail` (
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
    REFERENCES `aura`.`characters` (`characterId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `senderId`
    FOREIGN KEY (`senderId` )
    REFERENCES `aura`.`characters` (`characterId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `itemId`
    FOREIGN KEY (`itemId` )
    REFERENCES `aura`.`items` (`itemId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);
    
INSERT INTO `aura`.`accounts` (`accountId`, `password`, `authority`, `creation`, `lastlogin`, `lastip`, `bannedexpiration`, `points`) VALUES ('Aura System', '0', 0, '0001-01-01 00:00:00', '0001-01-01 00:00:00', '', '0001-01-01 00:00:00', 0);

INSERT INTO `aura`.`characters` (`characterId`, `server`, `accountId`, `name`, `type`, `race`, `skinColor`, `eyeType`, `eyeColor`, `mouthType`, `status`, `height`, `fatness`, `upper`, `lower`, `region`, `x`, `y`, `direction`, `battleState`, `weaponSet`, `life`, `injuries`, `lifeMax`, `mana`, `manaMax`, `stamina`, `staminaMax`, `food`, `level`, `totalLevel`, `experience`, `age`, `strength`, `dexterity`, `intelligence`, `will`, `luck`, `abilityPoints`, `attackMin`, `attackMax`, `wattackMin`, `wattackMax`, `critical`, `protect`, `defense`, `rate`, `strBoost`, `dexBoost`, `intBoost`, `willBoost`, `luckBoost`, `birthday`, `title`, `deletionTime`, `maxLevel`, `rebirthCount`, `jobId`, `color1`, `color2`, `color3`, `lastTown`, `lastDungeon`) VALUES (1, 'Aura', 'Aura System', 'Mail System', 'CHARACTER', 10001, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 10, 0, 10, 10, 10, 10, 10, 0, 1, 0, 0, 17, 10, 10, 10, 10, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, '0001-01-01 00:00:00', 0, '0001-01-01 00:00:00', 200, 0, 0, 0, 0, 0, '', '');

INSERT INTO `aura`.`items` (`characterId`, `itemID`, `class`, `pocketId`, `pos_x`, `pos_y`, `varint`, `color_01`, `color_02`, `color_03`, `price`, `bundle`, `linked_pocket`, `figure`, `flag`, `durability`, `durability_max`, `origin_durability_max`, `attack_min`, `attack_max`, `wattack_min`, `wattack_max`, `balance`, `critical`, `defence`, `protect`, `effective_range`, `attack_speed`, `experience`, `exp_point`, `upgraded`, `upgraded_max`, `grade`, `prefix`, `suffix`, `data`, `option`, `sellingprice`, `expiration`, `update_time`) VALUES (1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, '', '', 0, 0, '2012-12-23 00:00:00');

