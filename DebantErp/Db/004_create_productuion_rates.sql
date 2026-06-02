create table if not exists production_operations (
  id serial primary key,
  name varchar(50) not null,
  created_at timestamptz default now()
);

create table if not exists production_rates (
  id serial primary key,
  production_operation_id int references production_operations(id) on delete cascade,
  is_actual boolean default true,
  operation_timeframe numeric not null,
  rate numeric not null,
  created_at timestamptz default now(),
  updated_at timestamptz default now()
);

