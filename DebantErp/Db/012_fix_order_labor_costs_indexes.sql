-- Фикс дрейфа миграций: индексы order_labor_costs были доведены до финального
-- вида (частичные, + employee_id) правкой миграции 006 от 26.06.2026. В
-- окружениях, применивших 006 раньше, индексов нет либо order_id-индекс не
-- частичный. `create index if not exists` сверяет только имя, поэтому не
-- переопределит существующий индекс — делаем drop + create, чтобы гарантировать
-- нужное определение. На данных это не сказывается (индексы производные).
drop index if exists idx_order_labor_costs_order_id;
drop index if exists idx_order_labor_costs_employee_id;

create index if not exists idx_order_labor_costs_order_id
  on order_labor_costs (order_id) where is_deleted = false;

create index if not exists idx_order_labor_costs_employee_id
  on order_labor_costs (employee_id) where is_deleted = false;
