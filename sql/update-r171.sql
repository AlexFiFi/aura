ALTER TABLE `characters`
  DROP `strBoost`,
  DROP `dexBoost`,
  DROP `intBoost`,
  DROP `willBoost`,
  DROP `luckBoost`;
  
UPDATE `characters` SET `life`=0,`mana`=0,`stamina`=0;

ALTER TABLE  `characters` 
CHANGE  `life`  `lifeDelta` DOUBLE NOT NULL,
CHANGE `mana`  `manaDelta` DOUBLE NOT NULL,
CHANGE `stamina`  `staminaDelta` DOUBLE NOT NULL;