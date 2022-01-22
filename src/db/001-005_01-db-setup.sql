drop database if exists scheduling;
create database if not exists scheduling;
use scheduling;

drop user if exists schedule_app_user;
create user if not exists schedule_app_user identified by 'l3ts-make-a-@DaT3';
grant select on scheduling.* to schedule_app_user;
grant insert on scheduling.* to schedule_app_user;
grant update on scheduling.* to schedule_app_user;
grant delete on scheduling.* to schedule_app_user;