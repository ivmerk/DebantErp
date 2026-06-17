-- AuthSessions заменена на cookie-аутентификацию (claims в зашифрованной cookie,
-- ключи Data Protection персистятся в .data-protection/).
-- Таблица была write-only — данные в неё писались на login/logout, но никогда
-- не читались, поэтому удаляется без потери полезной информации.
-- Индексы удаляются вместе с таблицей.
drop table if exists AuthSessions;
