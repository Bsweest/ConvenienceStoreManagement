DROP SCHEMA public CASCADE;
CREATE SCHEMA public;
GRANT ALL ON SCHEMA public TO postgres;
GRANT ALL ON SCHEMA public TO public;
COMMENT ON SCHEMA public IS 'standard public schema';

CREATE EXTENSION if not exists "uuid-ossp";

create table employee (
	id SERIAL primary key,
	name text not null,
	phonenum text not null unique,
	salary integer DEFAULT 0,
	person_id text unique,
	username text unique,
	password text
);

create table customer (
	id SERIAL primary key,
	fullname text not null,
	phonenum text not null unique,
	person_id text unique,
	total_spending integer DEFAULT 0,
	balance integer DEFAULT 0
);

create table shopitem (
	id uuid DEFAULT uuid_generate_v4(),
	name text not null unique,
	price integer,
	image_path text,
	
	primary key (id)
);

create table discount (
	id SERIAL primary key,
	itemid uuid references shopitem,
	percent int,
	start_time timestamptz,
	end_time timestamptz
);

create table invoice (
	id SERIAL primary key,
	cus_id integer references customer,
	staff_id integer references employee,
	total_cost int,
	purchase_time timestamptz DEFAULT now()
);

create table good (
	id integer primary key,
	itemid uuid references shopitem,
	import_date date DEFAULT now(),
	mfg_date date,
	expired_date date,
	cost integer,
	invoice_id integer references invoice
);

create table transaction (
	id SERIAL primary key,
	cus_id integer references customer,
	staff_id integer references employee,
	trans_time timestamptz DEFAULT now(),
	amount integer
);

CREATE or REPLACE FUNCTION check_auth_pass(check_string text, auth_name text)
RETURNS BOOLEAN language plpgsql
as $$
	DECLARE 
		auth_pass text;
	BEGIN
		select password into auth_pass from employee
		where username=auth_name;
		IF (auth_pass is not null) then
			IF (auth_pass != check_string) then RETURN false;
			ELSE RETURN true;
			END IF;
		ELSE RETURN false;
		END IF;
	END
$$;

insert into customer (id, fullname, phonenum) values (0, 'GUEST', '000000000');
insert into employee (id, name, phonenum) values (0, 'ADMIN', '000000000');