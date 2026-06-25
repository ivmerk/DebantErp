# Заказы и трудозатраты

Сущности **Заказы** (`orders`) и **Трудозатраты по заказу** (`order_labor_costs`).

> **Статус: ✅ готово (UI + BL + DAL).** Раздел `/workspace/orders` — мастер-деталь:
> список заказов и детальная страница заказа с трудозатратами.

## UI

- **Список заказов** (`/workspace/orders`): номер + дата, создание / переименование /
  мягкое удаление, режим просмотра/редактирования (`?edit=true`), пагинация.
  Кнопка «Открыть» ведёт на страницу заказа.
- **Страница заказа** (`/workspace/orders/{id}`): таблица трудозатрат
  (работник · операция · ставка · количество · сумма) с итогом. В режиме правки —
  добавление (выбор работника + расценки + количество), изменение количества и
  мягкое удаление трудозатраты.

Сумма строки = ставка расценки × количество; расценка резолвится по **конкретной
версии** (`production_rate_id`), поэтому трудозатрата сохраняет ставку, которая
действовала на момент её создания (см. версионирование расценок в
[production-rates.md](production-rates.md)).

| Слой | Заказы | Трудозатраты |
|------|--------|--------------|
| Контроллер | `Controllers/OrdersController.cs` (общий для обеих сущностей) | — |
| Представления | `Views/Orders/Index.cshtml` | `Views/Orders/Details.cshtml` |
| ViewModel | `ViewModels/OrderListViewModel.cs` | `ViewModels/OrderDetailsViewModel.cs` (`LaborCostView`) |
| Бизнес-логика | `BL/Order/Order.cs` / `IOrder.cs` | `BL/OrderLaborCost/OrderLostCost.cs` / `IOrderLaborCost.cs` |
| Доступ к данным | `DAL/Implementations/OrderDAL.cs` / `IOrderDAL.cs` | `DAL/Implementations/OrderLaborCostDAL.cs` / `IOrderLaborCostDAL.cs` |
| DTO | `Dtos/CreateOrderDto.cs`, `UpdateOrderDto.cs` | `Dtos/CreateOrderLaborCostDto.cs`, `UpdateOrderLaborCostDto.cs` |
| RDO | `Rdos/OrderRdo.cs` | `Rdos/OrderLaborCostRdo.cs` |

Зарегистрированы в `Program.cs`: `IOrder → Order`, `IOrderLaborCost → OrderLostCost`,
`IOrderDAL → OrderDAL`, `IOrderLaborCostDAL → OrderLaborCostDAL`. Маршрут раздела —
`[Route("workspace/orders")]`. Изменение трудозатраты ограничено количеством
(`UpdateOrderLaborCostDto`); сменить работника/расценку — удалить и добавить заново.

## Модель данных

`Db/005_create_orders.sql`:

```sql
create table if not exists orders (
  id serial primary key,
  number varchar(50) unique not null,
  is_deleted bool default false,
  created_at timestamptz default now(),
  updated_at timestamptz default now()
);
```

`Db/006_create_order_labor_costs.sql`:

```sql
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
```

Трудозатрата связывает **заказ**, **работника** и **расценку**
(`production_rate`) с количеством (`quantity`). Удаление мягкое (`is_deleted`).

## DTO

- `CreateOrderDto`: `Number`.
- `CreateOrderLaborCostDto`: `EmployeeId`, `ProductionRateId`, `Quantity`, `OrderId`.

Паттерн UI — см. [employees-specialties.md](employees-specialties.md).
