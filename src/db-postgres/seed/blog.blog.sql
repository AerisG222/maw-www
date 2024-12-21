DO
$$
BEGIN

    IF NOT EXISTS (SELECT 1 FROM blog.blog) THEN
        INSERT INTO blog.blog
            (
                id,
                title,
                copyright,
                description
            )
        VALUES
            (
                '0193eaba-51c5-77b8-9f1e-181e2b818831',
                'mikeandwan.us Blog',
                '@Copyright mikeandwan.us',
                'Blog of the mikeandwan.us website'
            );
    END IF;

END
$$
