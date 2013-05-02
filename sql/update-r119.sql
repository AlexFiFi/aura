CREATE  TABLE `aura`.`guilds` (
  `id` BIGINT(20) UNSIGNED NOT NULL AUTO_INCREMENT ,
  `name` VARCHAR(12) NOT NULL ,
  `intro` VARCHAR(500) NOT NULL ,
  `welcome` VARCHAR(100) NOT NULL ,
  `leaving` VARCHAR(100) NOT NULL ,
  `rejection` VARCHAR(100) NOT NULL ,
  `level` TINYINT(3) NOT NULL DEFAULT 0 ,
  `type` TINYINT(3) NOT NULL DEFAULT 0 ,
  `region` BIGINT(10) UNSIGNED NOT NULL ,
  `x` BIGINT(10) UNSIGNED NOT NULL ,
  `y` BIGINT(10) UNSIGNED NOT NULL ,
  `rotation` TINYINT(3) UNSIGNED NOT NULL DEFAULT 0,  `gp` INT(10) UNSIGNED NOT NULL DEFAULT 0 ,
  `gold` INT(10) UNSIGNED NOT NULL DEFAULT 0 ,
  `stone_type` BIGINT(10) UNSIGNED NOT NULL DEFAULT 211  ,
 `title` VARCHAR(50) NOT NULL DEFAULT '',
  `options` TINYINT(3) UNSIGNED NOT NULL DEFAULT 0  
,  PRIMARY KEY (`id`) ,
  UNIQUE INDEX `name_UNIQUE` (`name` ASC) ,
  UNIQUE INDEX `id_UNIQUE` (`id` ASC) );

CREATE  TABLE `aura`.`guild_members` (
  `character_id` BIGINT(20) UNSIGNED NOT NULL ,
  `guild_id` BIGINT(20) UNSIGNED NOT NULL,
  `rank` TINYINT(3) UNSIGNED NOT NULL DEFAULT 0 ,
  `joined` DATETIME NOT NULL ,
  `guild_points` BIGINT(10) UNSIGNED NOT NULL DEFAULT 0 ,
  `message_flags` TINYINT(3) UNSIGNED NOT NULL DEFAULT 0,  `app_message` VARCHAR(100) NULL  ,  UNIQUE INDEX `character_id_UNIQUE` (`character_id` ASC) ,
  PRIMARY KEY (`character_id`) ,
  CONSTRAINT `character_id`
    FOREIGN KEY (`character_id` )
    REFERENCES `aura`.`characters` (`characterId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  INDEX `guild_id_idx` (`guild_id` ASC) ,
  CONSTRAINT `guild_id`
   FOREIGN KEY (`guild_id` )
   REFERENCES `aura`.`guilds` (`id` )
   ON DELETE NO ACTION
   ON UPDATE NO ACTION);