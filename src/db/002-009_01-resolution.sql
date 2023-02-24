use scheduling

ALTER TABLE `scheduling`.`SessionInfo` 
ADD COLUMN `Resolution` VARCHAR(10) NOT NULL DEFAULT 'day' AFTER `Description`;