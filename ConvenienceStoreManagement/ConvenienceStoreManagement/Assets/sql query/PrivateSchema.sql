DROP SCHEMA private CASCADE;
CREATE SCHEMA private;

create table private.secret_key(
	name text not null primary key,
	value text not null
);

insert into private.secret_key values ('ADMIN', 'thisisadminkey');

CREATE FUNCTION grant_admin_control(check_string text)
RETURNS BOOLEAN language plpgsql
as $$
	DECLARE 
		admin_key text;
	BEGIN
		select value into admin_key from private.secret_key
		where name='ADMIN';
		IF (admin_key != check_string) then RETURN false;
		ELSE RETURN true;
		END IF;
	END
$$;