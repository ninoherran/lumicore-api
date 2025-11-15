CREATE TABLE if not exists role (id UUID PRIMARY KEY, name VARCHAR(255));
CREATE TABLE if not exists role_user (role_id UUID REFERENCES role(id), user_id UUID REFERENCES users(id) UNIQUE);