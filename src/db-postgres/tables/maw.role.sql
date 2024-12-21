CREATE TABLE IF NOT EXISTS maw.role (
    id UUID NOT NULL,
    name TEXT NOT NULL,
    created TIMESTAMP NOT NULL,
    deleted TIMESTAMP NULL,

    CONSTRAINT pk_maw_role PRIMARY KEY (id),

    CONSTRAINT uq_maw_role$name UNIQUE (name)
);

GRANT SELECT, INSERT, UPDATE, DELETE
   ON maw.role
   TO website;
