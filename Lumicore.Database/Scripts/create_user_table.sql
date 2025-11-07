-- Example initial migration
-- This script will create a simple table to verify DbUp is working.

create table if not exists users (
    id UUID primary key,
    firstname text not null,
    lastname text not null,
    email text not null unique,
    password text not null,
    is_admin boolean not null default false
);
