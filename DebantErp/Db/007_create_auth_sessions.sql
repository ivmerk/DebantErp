create table if not exists AuthSessions (
    SessionId uuid primary key default gen_random_uuid(),
    UserId int not null references users(Id) on delete cascade,
    SessionToken text not null unique,
    IpAddress varchar(45),
    UserAgent text,
    CreatedAt timestamp not null default now(),
    LastAccessedAt timestamp not null default now(),
    ExpiresAt timestamp,
    IsActive boolean not null default true
);

create index idx_auth_sessions_user_id on AuthSessions(UserId);
create index idx_auth_sessions_token on AuthSessions(SessionToken);
create index idx_auth_sessions_expires_at on AuthSessions(ExpiresAt);
create index idx_auth_sessions_is_active on AuthSessions(IsActive);
