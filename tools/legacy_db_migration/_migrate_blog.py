import psycopg2
import uuid_utils as uuid

blogid = "0193eaba-51c5-77b8-9f1e-181e2b818831"

conn = psycopg2.connect(
    database = "maw_website",
    user = "postgres",
    host= 'localhost',
    port = 5432
)

cur = conn.cursor()
cur.execute("SELECT id, blog_id, title, description, publish_date FROM blog.post")
records = cur.fetchall()

cur.close()
conn.close()

f = open("/output/blog.post.sql", "w")

for row in records:
    values = (uuid.uuid7().hex, blogid, row[2], row[3], row[4], row[4])
    sql = cur.mogrify("INSERT INTO blog.post (id, blog_id, title, content, published, modified) VALUES (%s, %s, %s, %s, %s, %s)", values)
    f.write(str(sql, 'utf-8'))
    f.write(";\n")

f.close()
