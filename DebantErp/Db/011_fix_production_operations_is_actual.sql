-- Фикс дрейфа миграций: production_operations в ранних окружениях был создан
-- (миграция 004) ДО того, как в 004 добавили колонку is_actual (25.06.2026).
-- Т.к. 004 уже отмечен applied, а create table if not exists не пересоздаёт
-- таблицу, в таких окружениях колонки нет, и `SELECT ... WHERE is_actual = true`
-- падает — список операций отображается пустым. Добавляем колонку идемпотентно;
-- существующие строки получают значение по умолчанию (true).
alter table production_operations
  add column if not exists is_actual boolean default true;
