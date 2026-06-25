# Документация DebantERP

ERP для управления работниками, специальностями, производственными операциями,
заказами и трудозатратами. ASP.NET Core 9.0 MVC + PostgreSQL (Dapper),
серверный рендеринг (Razor + формы).

## Документы по сущностям

| Документ | Сущность | Статус |
|----------|----------|--------|
| [employees-specialties.md](employees-specialties.md) | Работники + назначение специальностей | ✅ UI + BL + DAL |
| [specialties.md](specialties.md) | Специальности (справочник) | ✅ UI + BL + DAL |
| [users-auth.md](users-auth.md) | Пользователи, вход, роли, админка | ✅ UI + BL + DAL |
| [production-rates.md](production-rates.md) | Производственные операции | ✅ UI + BL + DAL |
| [production-rates.md](production-rates.md) | Расценки (с историей версий) | ✅ UI + BL + DAL |
| [orders-labor-costs.md](orders-labor-costs.md) | Заказы и трудозатраты | ✅ UI + BL + DAL |

## Общие сведения

- **Рабочая область** (`/workspace`) — хаб выбора сущности. Разделы вынесены в
  отдельные контроллеры по сущностям, наследующие `WorkspaceBaseController`
  (общий `PageSize`). Все разделы реализованы: «Работники», «Специальности»,
  «Операции», «Расценки», «Заказы».
- **Режим просмотра / редактирования**: справочные экраны по умолчанию только для
  чтения; правка появляется по кнопке «Изменить» (серверный флаг `?edit=true`).
- **Доступ**: deny-by-default; вход обязателен, кроме `[AllowAnonymous]`.
  Управление пользователями — только роль `Admin`.

## Запуск

```bash
# БД (из каталога DebantErp/)
docker-compose -f docker-compose.dev.yml up -d

# Сборка и запуск (из корня / каталога DebantErp/)
dotnet build DebantErp.sln
cd DebantErp && dotnet run            # http://localhost:5010
```

Вход (сид-админ): `s2P7D@example.com` / `123456`.

## База данных

Схема создаётся **вручную** скриптами `Db/001_*.sql` … `Db/008_*.sql` по порядку
(раннера миграций нет). Данные засеваются `MockDataSeeder` при старте, если
таблицы пусты. О пересоздании схемы и заметке про опечатку `speciality` см.
[employees-specialties.md → База данных](employees-specialties.md#база-данных).

| Скрипт | Таблицы |
|--------|---------|
| `001_create_users.sql` | `users` (+ триггер `updated_at`) |
| `002_create_employees.sql` | `employees`, `employees_details` |
| `003_create_specialties.sql` | `specialties`, `employee_specialty_assignments` |
| `004_create_productuion_rates.sql` | `production_operations`, `production_rates` |
| `005_create_orders.sql` | `orders` |
| `006_create_order_labor_costs.sql` | `order_labor_costs` |
| `007` / `008` | `auth_sessions` создаётся и затем удаляется (сессии не используются) |
