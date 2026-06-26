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

-- Быстрый поиск действующих трудозатрат по заказу и по работнику.
-- Частичные индексы (только is_deleted = false) — мягко удалённые в выборки
-- не попадают, индексы меньше и точнее под запросы.
create index if not exists idx_order_labor_costs_order_id
  on order_labor_costs (order_id) where is_deleted = false;

create index if not exists idx_order_labor_costs_employee_id
  on order_labor_costs (employee_id) where is_deleted = false;
