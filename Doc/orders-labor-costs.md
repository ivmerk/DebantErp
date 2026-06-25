# Заказы и трудозатраты

Сущности **Заказы** (`orders`) и **Трудозатраты по заказу** (`order_labor_costs`).

> **Статус: бэкенд готов, UI нет.** Бизнес-логика и доступ к данным реализованы и
> зарегистрированы в DI, но HTTP-эндпойнтов нет — прежние JSON API-контроллеры
> удалены при переходе на чистый серверный MVC, а раздел «Заказы» в рабочей
> области (`/workspace/orders`) пока заглушка (`View("Section", "Заказы")`).
> Чтобы раздел заработал, нужен контроллер + представления (по образцу
> `EmployeesController` / `Views/Employees`).

## Архитектура

| Слой | Заказы | Трудозатраты |
|------|--------|--------------|
| Бизнес-логика | `BL/Order/Order.cs` / `IOrder.cs` | `BL/OrderLaborCost/OrderLostCost.cs` / `IOrderLaborCost.cs` |
| Доступ к данным | `DAL/Implementations/OrderDAL.cs` / `IOrderDAL.cs` | `DAL/Implementations/OrderLaborCostDAL.cs` / `IOrderLaborCostDAL.cs` |
| DTO | `Dtos/CreateOrderDto.cs`, `UpdateOrderDto.cs` | `Dtos/CreateOrderLaborCostDto.cs`, `UpdateOrderLaborCostDto.cs` |
| RDO | `Rdos/OrderRdo.cs` | `Rdos/OrderLaborCostRdo.cs` |

Зарегистрированы в `Program.cs`: `IOrder → Order`, `IOrderLaborCost → OrderLostCost`,
`IOrderDAL → OrderDAL`, `IOrderLaborCostDAL → OrderLaborCostDAL`.

Интерфейс BL (CRUD), одинаковый для обеих сущностей: `Get()`, `Get(id)`,
`Create(dto)`, `Update(id, dto)`, `Delete(id)`.

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

## Что нужно для запуска раздела в UI

1. `OrdersController : WorkspaceBaseController` с `[Route("workspace/orders")]`,
   экшены `Index/Create/Edit/Delete` (внедрить `IOrder`).
2. Представления `Views/Orders/Index.cshtml` (просмотр/редактирование по образцу
   работников, флаг `?edit=true`).
3. Заменить заглушку в `WorkspaceController` и поправить плитку в
   `Views/Workspace/Index.cshtml` (`controller = "Orders"`).
4. Трудозатраты — отдельный подэкран/таблица внутри заказа (работник + расценка +
   количество).

См. общий паттерн в [employees-specialties.md](employees-specialties.md).
