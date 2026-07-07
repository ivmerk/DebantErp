-- Норма времени — дробное положительное число с двумя знаками после запятой.
-- Фиксируем масштаб (scale = 2) на уровне БД: значения округляются до сотых.
alter table production_rates
  alter column operation_timeframe type numeric(12, 2);
