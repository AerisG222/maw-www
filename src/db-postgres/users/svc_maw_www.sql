DO
$$
BEGIN
    IF NOT EXISTS (SELECT 1
                     FROM pg_catalog.pg_user
                    WHERE usename = 'svc_maw_www') THEN

        CREATE USER svc_maw_www;
        GRANT website TO svc_maw_www;

    END IF;
END
$$;
