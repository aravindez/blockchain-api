DROP DATABASE IF EXISTS `Blockchain`;
CREATE DATABASE `Blockchain`;
USE `Blockchain`;

-- ---
-- Globals
-- ---

-- SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";
-- SET FOREIGN_KEY_CHECKS=0;

-- ---
-- Table 'users'
-- 
-- ---

DROP TABLE IF EXISTS `users`;
		
CREATE TABLE `users` (
  `id` INTEGER NOT NULL AUTO_INCREMENT,
  `email` VARCHAR(255) NOT NULL,
  `password` CHAR(128) NOT NULL,
  `first_name` VARCHAR(255) NOT NULL,
  `last_name` VARCHAR(255) NOT NULL,
  `admin` BIT NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`)
);

-- ---
-- Table 'blocks'
-- 
-- ---

DROP TABLE IF EXISTS `blocks`;
		
CREATE TABLE `blocks` (
  `id` INTEGER NOT NULL AUTO_INCREMENT,
  `previous_hash` CHAR(128) NOT NULL,
  `timestamp` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `created_by` INTEGER NOT NULL,
  `data` INTEGER NOT NULL,
  `hash` CHAR(128) NOT NULL,
  `nonce` INTEGER NOT NULL,
  `isValid` bit NULL DEFAULT NULL,
  PRIMARY KEY (`id`)
);

-- ---
-- Table 'chains'
-- 
-- ---

DROP TABLE IF EXISTS `chains`;
		
CREATE TABLE `chains` (
  `id` INTEGER NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(255) NOT NULL,
  `created_by` INTEGER NOT NULL,
  PRIMARY KEY (`id`)
);

-- ---
-- Table 'block_chain_user'
-- 
-- ---

DROP TABLE IF EXISTS `block_chain_user`;
		
CREATE TABLE `block_chain_user` (
  `id` INTEGER NOT NULL AUTO_INCREMENT,
  `block_id` INTEGER DEFAULT NULL,
  `chain_id` INTEGER NOT NULL,
  `user_id` INTEGER DEFAULT NULL,
  `block` bit NOT NULL DEFAULT 1,
  PRIMARY KEY (`id`)
);

-- ---
-- Table 'pending_blocks'
-- 
-- ---

DROP TABLE IF EXISTS `pending_blocks`;
		
CREATE TABLE `pending_blocks` (
  `id` INTEGER NOT NULL AUTO_INCREMENT,
  `block_id` INTEGER NOT NULL,
  `chain_id` INTEGER NOT NULL,
  `authorizer_id` INTEGER NOT NULL,
  `isValid` bit NULL DEFAULT NULL,
  PRIMARY KEY (`id`)
);

-- ---
-- Table 'keys'
-- 
-- ---

DROP TABLE IF EXISTS `keys`;
		
CREATE TABLE `keys` (
  `id` INTEGER NOT NULL AUTO_INCREMENT,
  `user_id` INTEGER NOT NULL,
  `public_key` VARCHAR(255) NOT NULL,
  `private_key` VARCHAR(255) NOT NULL,
  `salt` CHAR(8) NOT NULL,
  PRIMARY KEY (`id`)
);

-- ---
-- Table 'users_groups'
-- 
-- ---

DROP TABLE IF EXISTS `users_groups`;
		
CREATE TABLE `users_groups` (
  `id` INTEGER NOT NULL AUTO_INCREMENT,
  `user_id` INTEGER NOT NULL,
  `group_id` INTEGER NOT NULL,
  PRIMARY KEY (`id`)
);

-- ---
-- Table 'groups'
-- 
-- ---

DROP TABLE IF EXISTS `groups`;
		
CREATE TABLE `groups` (
  `id` INTEGER NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`id`)
);

-- ---
-- Foreign Keys 
-- ---

ALTER TABLE `blocks` ADD FOREIGN KEY (created_by) REFERENCES `users` (`id`);
ALTER TABLE `chains` ADD FOREIGN KEY (created_by) REFERENCES `users` (`id`);
ALTER TABLE `block_chain_user` ADD FOREIGN KEY (block_id) REFERENCES `blocks` (`id`);
ALTER TABLE `block_chain_user` ADD FOREIGN KEY (chain_id) REFERENCES `chains` (`id`);
ALTER TABLE `block_chain_user` ADD FOREIGN KEY (user_id) REFERENCES `users` (`id`);
ALTER TABLE `pending_blocks` ADD FOREIGN KEY (block_id) REFERENCES `blocks` (`id`);
ALTER TABLE `pending_blocks` ADD FOREIGN KEY (chain_id) REFERENCES `chains` (`id`);
ALTER TABLE `pending_blocks` ADD FOREIGN KEY (authorizer_id) REFERENCES `users` (`id`);
ALTER TABLE `keys` ADD FOREIGN KEY (id) REFERENCES `users` (`id`);
ALTER TABLE `users_groups` ADD FOREIGN KEY (user_id) REFERENCES `users` (`id`);
ALTER TABLE `users_groups` ADD FOREIGN KEY (group_id) REFERENCES `groups` (`id`);

-- ---
-- Table Properties
-- ---

-- ALTER TABLE `users` ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;
-- ALTER TABLE `blocks` ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;
-- ALTER TABLE `chains` ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;
-- ALTER TABLE `block_chain_user` ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;
-- ALTER TABLE `pending_blocks` ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;
-- ALTER TABLE `keys` ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;
-- ALTER TABLE `users_groups` ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;
-- ALTER TABLE `groups` ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- ---
-- Test Data
-- ---

-- INSERT INTO `users` (`id`,`email`,`password`,`first_name`,`last_name`) VALUES
-- ('','','','','');
-- INSERT INTO `blocks` (`id`,`previous_hash`,`timestamp`,`created_by`,`data`,`hash`,`nonce`,`isValid`) VALUES
-- ('','','','','','','','');
-- INSERT INTO `chains` (`id`,`name`,`created_by`) VALUES
-- ('','','');
-- INSERT INTO `block_chain_user` (`id`,`block_id`,`chain_id`,`user_id`,`block`) VALUES
-- ('','','','','');
-- INSERT INTO `pending_blocks` (`id`,`block_id`,`chain_id`,`authorizer_id`,`isValid`) VALUES
-- ('','','','','');
-- INSERT INTO `keys` (`id`,`user_id`,`public_key`,`private_key`,`salt`) VALUES
-- ('','','','','');
-- INSERT INTO `users_groups` (`id`,`user_id`,`group_id`) VALUES
-- ('','','');
-- INSERT INTO `groups` (`id`,`name`) VALUES
-- ('','');