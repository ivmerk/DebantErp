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

Вся схема лежит в одном скрипте `Db/001_baseline.sql` (консолидация прежних
`001`–`012`). Локально его нужно применить **вручную**; на деплое это делает
сервис `migrator` (`scripts/migrate.sh`, идемпотентно, с учётом в таблице
`_migrations`). Схема идемпотентна (`create … if not exists`). Данные засеваются
`MockDataSeeder` при старте, если таблицы пусты. О заметке про опечатку
`speciality` см.
[employees-specialties.md → База данных](employees-specialties.md#база-данных).

Таблицы, создаваемые `001_baseline.sql`:

| Таблица | Назначение |
|---------|-----------|
| `users` (+ триггер `updated_at`) | пользователи, вход, роли |
| `employees`, `employees_details` | работники и их реквизиты |
| `specialties`, `employee_specialty_assignments` | специальности и назначения |
| `production_operations`, `production_rates` | операции (с кодом) и расценки (с историей версий) |
| `orders` | заказы |
| `order_labor_costs` | трудозатраты по заказам |

> Таблица `auth_sessions` (прежние `007`/`008`) в итоговой схеме отсутствует —
> сессии заменены cookie-аутентификацией, поэтому в baseline её нет.
