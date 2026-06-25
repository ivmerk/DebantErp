# Пользователи, аутентификация и доступ

Учётные записи (`users`), вход по cookie, регистрация и управление
пользователями администратором.

## Сущность «Пользователь»

`Db/001_create_users.sql`:

```sql
create table if not exists users (
    id serial primary key,
    first_name varchar(50),
    last_name varchar(50),
    phone varchar(50),
    role int,
    email varchar(50) unique not null,
    password varchar(100),
    salt varchar(50),
    status int,
    created_at timestamptz default now(),
    updated_at timestamptz default now()
);
```

Перечисления (`DAL/Models/DalEnums.cs`):

| Enum | Значения |
|------|----------|
| `UserRoleEnum` | `Admin = 1`, `User = 2` |
| `UserStatusEnum` | `Active = 1`, `Inactive = 2`, `Deleted = 3`, `NeedToApprove = 4` |
| `GenderEnum` | `Male = 1`, `Female = 2` |

## Аутентификация

- **Cookie-аутентификация** (`AddAuthentication().AddCookie()`, схема `Cookies`).
  При входе `Auth.Authenticate` проверяет пароль и вызывает `SignInAsync` с
  `ClaimsPrincipal`: `NameIdentifier` (userid), `Name` (email), `Role`.
- **Пароли**: SHA + индивидуальная соль (`BL/Auth/Encrypt.cs`).
- Сервер **без серверной сессии** — состояние в зашифрованной cookie.
  `ICurrentUser.IsLoggedIn()` смотрит только `User.Identity.IsAuthenticated`.
- Ключи Data Protection хранятся в `.data-protection/`, чтобы cookie переживала
  перезапуски. При ошибке расшифровки cookie после ротации ключей — очистить
  cookies браузера.

| Поток | Маршрут | Контроллер |
|-------|---------|------------|
| Вход | `GET/POST /login` | `LoginController` |
| Выход | `POST /logout` | `LoginController` (`[ValidateAntiForgeryToken]`) |
| Регистрация | `GET/POST /register` | `RegisterController` |

Новые пользователи создаются со статусом `NeedToApprove` — активирует админ.

`RegisterUserDto`: `FirstName`, `LastName`, `Phone`, `Email`, `Password`
(все обязательны, email валидируется).

## Авторизация

- **Deny-by-default**: в `Program.cs` задан `FallbackPolicy`, требующий
  аутентификации, поэтому каждый эндпойнт требует входа, кроме помеченных
  `[AllowAnonymous]` (`Home`, `Login`, `Register`, `/health`).
- Значение claim роли — имя из `UserRoleEnum` (`"Admin"` / `"User"`,
  с заглавной). Проверки чувствительны к регистру — так же писать в
  `[Authorize(Roles = ...)]`.

## Управление пользователями (админ)

`Controllers/AdminController.cs` — `[Authorize(Roles = "Admin")]`,
`[Route("admin")]`:

| Действие | Маршрут |
|----------|---------|
| Список пользователей | `GET /admin/users` |
| Изменить роль/статус | `POST /admin/users/{id}` (`UpdateUser`) |

`UpdateUserDto`: `FirstName`, `LastName`, `Phone`, `Role`, `Email`, `Status`
(в UI меняются `Role` и `Status`).

Представления: `Views/Admin/Users.cshtml`, формы входа/регистрации —
`Views/Login`, `Views/Register`.

## Мок-данные

`MockDataSeeder.SeedAsync()` вызывается при старте и засевает пользователей,
работников, специальности, заказы и расценки, если таблицы пусты. Сид-админ для
проверки: `s2P7D@example.com` / `123456`.
