create table if not exists orders (
  id serial primary key,
  number varchar(50) unique not null,
  is_deleted bool default false,
  created_at timestamptz default now(),
  updated_at timestamptz default now()
);
