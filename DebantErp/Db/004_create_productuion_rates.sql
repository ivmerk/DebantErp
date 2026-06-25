create table if not exists production_operations (
  id serial primary key,
  name varchar(50) not null,
  is_actual boolean default true,
  created_at timestamptz default now()
);

-- Расценки версионируются в этой же таблице: текущая версия — is_actual = true,
-- прошлые версии (история) — is_actual = false. Изменение расценки = пометить
-- старую версию is_actual = false и вставить новую активную (а не UPDATE на месте).
-- order_labor_costs ссылается на конкретную версию (production_rate_id), поэтому
-- прошлые трудозатраты остаются привязаны к своей версии расценки.
create table if not exists production_rates (
  id serial primary key,
  production_operation_id int references production_operations(id) on delete cascade,
  is_actual boolean default true,
  operation_timeframe numeric not null,
  rate numeric not null,
  created_at timestamptz default now(),
  updated_at timestamptz default now()
);

