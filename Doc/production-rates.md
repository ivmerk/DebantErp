# Производственные операции и расценки

Сущности **Производственные операции** (`production_operations`) и **Расценки**
(`production_rates`).

> **Статус:**
> - **Операции** — ✅ полностью: UI + BL + DAL (раздел `/workspace/operations`).
> - **Расценки** — 🧩 каркас: схема, модель и DAL есть, но BL-интерфейс
>   `IProductionRate` пуст, в DI не зарегистрированы, UI нет. Используются как
>   справочник для трудозатрат (`order_labor_costs.production_rate_id`).

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

Маршрут `[Route("workspace/operations")]`, экшены `Index/Create/Edit/Delete`.
DAL зарегистрирован как `IProductionOperationDAL`, BL — `IProductionOperation`.
Плитка «Операции» есть на дашборде.

## Расценки (каркас)

| Слой | Файлы | Состояние |
|------|-------|-----------|
| Модель | `DAL/Models/ProductionRateModel.cs` | есть |
| DAL | `DAL/Implementations/ProductionRate.cs` (+ интерфейс) | есть, **не в DI** |
| BL | `BL/ProductionRate/IProductionRate.cs` | **интерфейс пуст**, реализации нет |
| DTO / RDO / Контроллер / View | — | нет (раздел «Расценки» — заглушка) |

## Модель данных

`Db/004_create_productuion_rates.sql`:

```sql
create table if not exists production_operations (
  id serial primary key,
  name varchar(50) not null,
  is_actual boolean default true,
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
```

- **Операция** — справочник наименований работ (`name`), мягко деактивируется
  через `is_actual`.
- **Расценка** привязана к операции, хранит норму времени (`operation_timeframe`)
  и ставку (`rate`); мягко деактивируется через `is_actual`.

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

> ⚠️ Текущий `ProductionRateDAL.Update` меняет строку **на месте** (теряет
> историю). Когда будет раздел «Расценки», изменение нужно делать
> версионированием: деактивировать старую версию + вставить новую.

## Что нужно для раздела «Расценки»

1. Описать методы в `IProductionRate` и реализовать BL.
2. Зарегистрировать DAL и BL в `Program.cs`.
3. Добавить DTO/RDO, `RatesController : WorkspaceBaseController`
   (`[Route("workspace/rates")]`) и представления `Views/Rates/` (расценка
   привязана к операции — нужен выбор операции + норма времени + ставка).
4. Заменить заглушку в `WorkspaceController` и плитку «Расценки» в
   `Views/Workspace/Index.cshtml`.

Паттерн — см. [employees-specialties.md](employees-specialties.md) и
[specialties.md](specialties.md).
