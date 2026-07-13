-- ---------- Разряды (grades) ----------
-- Разряд — числовой тарифный разряд. У каждой производственной операции есть
-- разряд (номер). В таблице разрядов ведётся история дневных ставок: на один
-- номер разряда допускается несколько строк с датами введения; действующая — одна
-- (is_actual = true). Смена ставки = новая действующая строка, старая уходит в историю.
create table if not exists grades (
    id serial primary key,
    grade int not null,                       -- номер разряда (1, 2, 3, …)
    daily_rate numeric(12, 2) not null,       -- дневная ставка, деньги
    effective_date date not null,             -- дата введения
    is_actual boolean not null default true,  -- флаг актуальности
    created_at timestamptz default now()
);

-- Не более одной действующей ставки на номер разряда.
create unique index if not exists uq_grades_actual
    on grades (grade) where is_actual = true;

-- У операции появляется разряд (номер). Nullable: у ранее заведённых операций
-- разряд ещё не проставлен.
alter table production_operations
    add column if not exists grade int;
