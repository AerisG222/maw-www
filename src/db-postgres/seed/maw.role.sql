DO
$$
BEGIN

    IF NOT EXISTS (SELECT 1 FROM maw.role) THEN
        INSERT INTO maw.role
            (
                id,
                name,
                created
            )
        VALUES
            (
                '01940dab-5005-7c3c-bfa0-1cfc0a798831',
                'Administrator',
                NOW()
            );
    END IF;

END
$$
