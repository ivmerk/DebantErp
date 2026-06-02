create table if not exists  users (
    id serial primary key,
    first_name varchar(50),
    last_name varchar(50),
    phone varchar(50),
    role int,
    email varchar(50) unique not null,
    password varchar(100),
    salt varchar(50),
    status int,
    created_at timestamptz default now(),
    updated_at timestamptz default now()
);

create function update_modified_column()
returns trigger as $$
begin
    new.updated_at = now();
    return new;
end;
$$ language plpgsql;

create trigger set_timestamp
before update on users
for each row
execute function update_modified_column();
