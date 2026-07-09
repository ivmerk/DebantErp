# Раздел «Специальности»

Справочник специальностей в рабочей области (`/workspace/specialties`).
Полностью реализован (UI + BL + DAL), по той же схеме, что и «Работники».

## Обзор

Открывается из хаба **Данные** (`/workspace`). По умолчанию — режим просмотра;
добавление, переименование и удаление появляются после кнопки **«Изменить»**
(серверный флаг `?edit=true`). Сортировка по названию, пагинация по 20, мягко
удалённые (`is_actual = false`) не показываются.

## Архитектура

| Слой | Файлы |
|------|-------|
| Контроллер | `Controllers/SpecialtiesController.cs` (`: WorkspaceBaseController`) |
| Представление | `Views/Specialties/Index.cshtml` |
| ViewModel | `ViewModels/SpecialtyListViewModel.cs` (`Items`, `Page`, `TotalPages`, `Edit`) |
| Бизнес-логика | `BL/Specialty/Specialty.cs` / `ISpecialty.cs` |
| Доступ к данным | `DAL/Implementations/SpecialtyDAL.cs` / `ISpecialtyDAL.cs` |
| DTO / RDO | `Dtos/CreateUpdateSpecialtyDto.cs`, `Rdos/SpecialtyRdo.cs` |

Маршрут: `[Route("workspace/specialties")]`, экшены `Index` / `Create` / `Edit` /
`Delete`.

## Поведение бизнес-логики

- **Нормализация имени** (`Capitalize`): обрезка пробелов, первая буква
  заглавная, остальные строчные. `"ШВЕЯ"`, `"швея"`, `" шВеЯ "` → `"Швея"`.
- **Реактивация при повторном создании**: если специальность с таким именем уже
  есть и она мягко удалена — запись реактивируется (`is_actual = true`), а не
  создаётся дубликат (имя уникально). Если активна — ошибка «уже существует».
- **Проверка дубликата при обновлении** срабатывает только если имя реально
  меняется (иначе `IsExist` нашла бы саму редактируемую запись).
- **Удаление мягкое** (`is_actual = false`).

## Модель данных

`Db/001_baseline.sql` (таблицы `specialties`, `employee_specialty_assignments`):

```sql
create table if not exists specialties (
    id serial primary key,
    name varchar(50) unique not null,
    is_actual boolean default true,
    created_at timestamptz default now()
);
```

`SpecialtyRdo`: `Id`, `Name`, `IsActual`.

## Связь с работниками

Специальности назначаются работникам через таблицу
`employee_specialty_assignments` — см. [employees-specialties.md](employees-specialties.md),
этап «Специальности работника». Список активных специальностей используется в
выпадающем меню назначения.

## Проверка

1. «Данные» → «Специальности» → «Изменить».
2. Добавить «швея» → сохранится как «Швея».
3. Переименовать, удалить (мягко), снова добавить удалённую — реактивируется.
