CREATE TABLE IF NOT EXISTS maw.user_role (
    user_id UUID NOT NULL,
    role_id UUID NOT NULL,
    created TIMESTAMP NOT NULL,
    deleted TIMESTAMP NULL,

    CONSTRAINT pk_maw_user_role PRIMARY KEY (user_id, role_id),

    CONSTRAINT fk_maw_user_role$maw_user FOREIGN KEY (user_id) REFERENCES maw.user(id),
    CONSTRAINT fk_maw_user_role$maw_role FOREIGN KEY (role_id) REFERENCES maw.role(id)
);

GRANT SELECT, INSERT, UPDATE, DELETE
   ON maw.role
   TO website;
