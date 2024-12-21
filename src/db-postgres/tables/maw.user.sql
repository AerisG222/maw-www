CREATE TABLE IF NOT EXISTS maw.user (
    id UUID NOT NULL,
    name TEXT NOT NULL,
    email TEXT NOT NULL,
    created TIMESTAMP NOT NULL,
    deleted TIMESTAMP NULL,

    CONSTRAINT pk_maw_user PRIMARY KEY (id)
);

GRANT SELECT, INSERT, UPDATE, DELETE
   ON maw.user
   TO website;
