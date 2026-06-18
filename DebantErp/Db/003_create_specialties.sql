create table if not exists specialties (
    id serial primary key,
    name varchar(50) unique not null,
    is_actual boolean default true,
    created_at timestamptz default now()
);

create table if not exists employee_specialty_assignments (
    id serial primary key,
    employee_id int references employees(id) on delete cascade,
    specialty_id int references specialties(id) on delete cascade,
    date_from date not null,
    is_actual boolean default true,
    created_at timestamptz default now(),
    updated_at timestamptz default now(),
    unique(employee_id, specialty_id)
);

create or replace function update_modified_column_specialties()
returns trigger as $$
begin
    new.updated_at = now();
    return new;
end;
$$ language 'plpgsql';

drop trigger if exists set_timestamp on employee_specialty_assignments;

create trigger set_timestamp
before update on employee_specialty_assignments
for each row
execute function update_modified_column_specialties();
