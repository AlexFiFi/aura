ALTER TABLE `quest_progress` DROP FOREIGN KEY `quest_progress_ibfk_1` ;
ALTER TABLE quest_progress DROP PRIMARY KEY;
ALTER TABLE `quest_progress` ADD `progressId` BIGINT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY FIRST ;
ALTER TABLE `quest_progress` ADD INDEX ( `questId` ) ;
ALTER TABLE `quest_progress` ADD FOREIGN KEY ( `questId` ) REFERENCES `aura`.`quests` ( `questId` ) ON DELETE CASCADE ON UPDATE CASCADE ;
