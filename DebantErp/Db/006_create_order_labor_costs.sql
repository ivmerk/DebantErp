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
