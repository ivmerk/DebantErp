# Производственные операции и расценки

Сущности **Производственные операции** (`production_operations`) и **Расценки**
(`production_rates`).

> **Статус:**
> - **Операции** — ✅ полностью: UI + BL + DAL (раздел `/workspace/operations`).
> - **Расценки** — ✅ полностью: UI + BL + DAL (раздел `/workspace/rates`), с
>   историей версий. Используются как справочник для трудозатрат
>   (`order_labor_costs.production_rate_id`).

---

## Производственные операции (готово)

Справочник операций в рабочей области (`/workspace/operations`), по тому же
паттерну, что «Специальности»: режим просмотра/редактирования (`?edit=true`),
сортировка по названию, пагинация, **мягкое удаление** (`is_actual = false`).

| Слой | Файлы |
|------|-------|
| Контроллер | `Controllers/ProductionOperationsController.cs` |
| Представление | `Views/ProductionOperations/Index.cshtml` |
| ViewModel | `ViewModels/ProductionOperationListViewModel.cs` |
| Бизнес-логика | `BL/ProductionRate/ProductionOperation.cs` / `IProductionOperation.cs` |
| Доступ к данным | `DAL/Implementations/ProductionOperationDAL.cs` |
| DTO / RDO | `Dtos/CreateUpdateProductionOperationDto.cs`, `Rdos/ProductionOperationRdo.cs` |

Наименование нормализуется на сохранении (первая буква заглавная, остальные
строчные, без крайних пробелов): «ПОШИВ» → «Пошив».

Маршрут `[Route("workspace/operations")]`, экшены `Index/Create/Edit/Delete`.
DAL зарегистрирован как `IProductionOperationDAL`, BL — `IProductionOperation`.
Плитка «Операции» есть на дашборде.

## Расценки (готово)

Раздел `/workspace/rates`: список действующих расценок (операция · норма времени ·
ставка), добавление (для операций без действующей расценки), изменение и мягкое
удаление, режим просмотра/редактирования. **Изменение расценки версионируется** —
у каждой строки кнопка «История (N)» раскрывает прошлые версии.

| Слой | Файлы |
|------|-------|
| Контроллер | `Controllers/RatesController.cs` (`[Route("workspace/rates")]`) |
| Представление | `Views/Rates/Index.cshtml` |
| ViewModel | `ViewModels/ProductionRateListViewModel.cs` (`ProductionRateRow` + история) |
| Бизнес-логика | `BL/ProductionRate/ProductionRate.cs` / `IProductionRate.cs` |
| Доступ к данным | `DAL/Implementations/ProductionRate.cs` |
| DTO / RDO | `Dtos/CreateProductionRateDto.cs`, `ChangeProductionRateDto.cs`, `Rdos/ProductionRateRdo.cs` |

`Create`/`Change` в BL реализуют версионирование: при изменении старая версия
помечается `is_actual = false` (уходит в историю) и вставляется новая действующая.
Одна действующая расценка на операцию.

## Модель данных

`Db/001_baseline.sql` (таблицы `production_operations`, `production_rates`):

```sql
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
```

- **Операция** — справочник наименований работ (`name`) с уникальным кодом
  (`code`, до 15 символов); мягко деактивируется через `is_actual`.
- **Расценка** привязана к операции, хранит норму времени (`operation_timeframe`,
  дробное положительное с двумя знаками) и ставку (`rate`); мягко деактивируется
  через `is_actual`.

Сидируется `MockDataSeeder` при старте (если таблицы пусты).

## История изменений расценок (версионирование)

Отдельная таблица истории не нужна — история хранится в самой `production_rates`
как версии (это бы дублировало `rate` / `operation_timeframe`, уже лежащие в
строках). Подход — версионирование (SCD-2):

- **Текущая версия** расценки операции: `is_actual = true`.
- **История** (прошлые версии): строки той же операции с `is_actual = false`.
- **Изменение расценки** = пометить активную версию `is_actual = false` и вставить
  новую активную (а **не** `UPDATE` значений на месте).

```sql
-- текущая расценка операции
SELECT * FROM production_rates
WHERE production_operation_id = @op AND is_actual = true;

-- история (все версии операции, новые сверху)
SELECT * FROM production_rates
WHERE production_operation_id = @op
ORDER BY created_at DESC;
```

`order_labor_costs.production_rate_id` ссылается на **конкретную** версию, поэтому
прошлые трудозатраты остаются привязаны к расценке, действовавшей на момент их
создания, и не меняются при появлении новой версии.

Версионирование реализовано в `ProductionRate.Change` (BL): деактивирует текущую
версию (`ProductionRateDAL.Delete` → `is_actual = false`) и вставляет новую
(`Create`). На экране расценок прошлые версии видны по кнопке «История (N)».

> Примечание: `ProductionRateDAL.Update` (UPDATE на месте) в UI не используется —
> изменение идёт через версионирование. Метод оставлен в DAL как часть
> `IBaseDAL`.

Паттерн UI — см. [employees-specialties.md](employees-specialties.md) и
[specialties.md](specialties.md).
