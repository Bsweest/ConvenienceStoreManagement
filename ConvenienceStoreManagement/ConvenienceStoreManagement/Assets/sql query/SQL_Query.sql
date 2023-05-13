DROP SCHEMA public CASCADE;
CREATE SCHEMA public;
GRANT ALL ON SCHEMA public TO postgres;
GRANT ALL ON SCHEMA public TO public;
COMMENT ON SCHEMA public IS 'standard public schema';

CREATE EXTENSION if not exists "uuid-ossp";

create table employee (
	id SERIAL primary key,
	name text,
	person_id text unique,
	username text unique,
	password text
);

create table customer (
	id SERIAL primary key,
	fullname text not null,
	phonenum text not null unique,
	person_id text unique,
	total_spending integer,
	balance integer
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
	purchase_time timestamptz
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