-- Консолидированный baseline схемы (свёрнутые прежние миграции 001–012).
-- Отражает итоговое состояние БД. Для уже поднятых окружений все объекты
-- создаются через `if not exists` — это no-op (схема уже совпадает). Для новых
-- установок это единственная стартовая миграция.
--
-- Миграции append-only: НЕ редактировать этот файл после применения — любое
-- изменение схемы добавляется новым `NNN_*.sql` (см. CLAUDE.md).

-- ---------- users ----------
create table if not exists users (
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

create or replace function update_modified_column()
returns trigger as $$
begin
    new.updated_at = now();
    return new;
end;
$$ language plpgsql;

drop trigger if exists set_timestamp on users;
create trigger set_timestamp
before update on users
for each row
execute function update_modified_column();

-- ---------- employees ----------
create table if not exists employees (
    id serial primary key,
    first_name varchar(50) not null,
    middle_name varchar(50) not null,
    last_name varchar(50) not null,
    is_actual boolean default true,
    created_at timestamptz default now(),
    updated_at timestamptz default now()
);

create table if not exists employees_details (
    id serial primary key,
    tax_code varchar(50) unique not null,
    address text not null,
    email varchar(50) unique not null,
    phone varchar(50) not null,
    birth_date date not null,
    gender int not null,
    picture varchar(100) not null,
    created_at timestamptz default now(),
    updated_at timestamptz default now(),
    employee_id int not null references employees(id) on delete cascade
);

-- ---------- specialties ----------
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

-- ---------- production operations / rates ----------
-- Расценки версионируются в этой же таблице: текущая версия — is_actual = true,
-- прошлые (история) — is_actual = false. Изменение = пометить старую неактуальной
-- и вставить новую активную. order_labor_costs ссылается на конкретную версию.
create table if not exists production_operations (
    id serial primary key,
    name varchar(50) not null,
    is_actual boolean default true,
    created_at timestamptz default now(),
    code varchar(15) not null,
    constraint uq_production_operations_code unique (code)
);

create table if not exists production_rates (
    id serial primary key,
    production_operation_id int references production_operations(id) on delete cascade,
    is_actual boolean default true,
    operation_timeframe numeric(12, 2) not null,
    rate numeric not null,
    created_at timestamptz default now(),
    updated_at timestamptz default now()
);

-- ---------- orders / labor costs ----------
create table if not exists orders (
    id serial primary key,
    number varchar(50) unique not null,
    is_deleted bool default false,
    created_at timestamptz default now(),
    updated_at timestamptz default now()
);

create table if not exists order_labor_costs (
    id serial primary key,
    employee_id int not null references employees(id),
    production_rate_id int not null references production_rates(id),
    quantity int not null,
    order_id int not null references orders(id),
    is_deleted bool default false,
    created_at timestamptz default now(),
    updated_at timestamptz default now()
);

-- Частичные индексы (только is_deleted = false) под GetByOrderId / по работнику.
create index if not exists idx_order_labor_costs_order_id
    on order_labor_costs (order_id) where is_deleted = false;

create index if not exists idx_order_labor_costs_employee_id
    on order_labor_costs (employee_id) where is_deleted = false;
