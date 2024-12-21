CREATE OR REPLACE FUNCTION blog.add_post
(
    _id UUID,
    _blog_id UUID,
    _title TEXT,
    _content TEXT,
    _published TIMESTAMP
)
RETURNS UUID
LANGUAGE SQL
AS $$

    INSERT INTO blog.post
    (
        id,
        blog_id,
        title,
        content,
        published,
        modified
    )
    VALUES
    (
        _id,
        _blog_id,
        _title,
        _content,
        _published,
        _published
    );

    SELECT _id;

$$;

GRANT EXECUTE
   ON FUNCTION blog.add_post
   TO website;
