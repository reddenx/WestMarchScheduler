use scheduling

ALTER TABLE `scheduling`.`sessioninfo` 
ADD COLUMN `Resolution` VARCHAR(10) NOT NULL DEFAULT 'day' AFTER `Description`;