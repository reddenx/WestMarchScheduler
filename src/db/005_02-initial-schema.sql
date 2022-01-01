use scheduling;
drop table if exists ScheduleInfo;
drop table if exists Player;
drop table if exists SessionInfo;

create table if not exists SessionInfo
(
	`HostKey` varchar(100) primary key not null,
    `LeadKey` varchar(100) not null,
    `PlayerKey` varchar(100) not null,
    `Title` varchar(200) not null,
    `Description` nvarchar(2400) not null
);

create table Player
(
	`HostKey` varchar(100) not null,
    `Name` varchar(200) not null,
    `Role` varchar(20) not null
);

create table ScheduleInfo
(
	`HostKey` varchar(100) not null,
    `Name` varchar(200) not null,
    `Start` datetime not null,
    `End` datetime not null
);