-- Код операции: набор до 15 символов, обязательный и уникальный.
alter table production_operations add column if not exists code varchar(15);

-- Backfill существующих строк, чтобы удовлетворить NOT NULL + UNIQUE
-- (уже заведённым операциям код проставляется по их id, потом его можно
-- отредактировать вручную).
update production_operations set code = 'OP-' || id where code is null;

alter table production_operations alter column code set not null;
alter table production_operations
  add constraint uq_production_operations_code unique (code);
