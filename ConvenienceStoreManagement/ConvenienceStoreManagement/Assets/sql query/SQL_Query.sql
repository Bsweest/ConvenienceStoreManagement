drop table if exists app_auth;

create table app_auth (
	username varchar(20),
	pass varchar(20) not null,
	staff_id uuid,
	
	primary key (username),
	constraint username_length check (char_length(username) >= 5),
 	constraint pass_length check (char_length(pass) >= 5)
);