using DataAccess.Contracts.Blog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace DataAccess.GhostDb.Loader
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var contextOptionsBuilder = new DbContextOptionsBuilder<PdDbContext>();
            contextOptionsBuilder.UseSqlServer("Server=ZACKPEINE\\SANDBOX;Database=Blog;Trusted_Connection=True;MultipleActiveResultSets=true");
            using (var context = new PdDbContext(contextOptionsBuilder.Options))
            using (var connection = new SQLiteConnection(@"Data Source=.\ghost.db"))
            {
                connection.Open();

                var posts = new Dictionary<int, EntityEntry<PostDto>>();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT id, created_at, updated_at, markdown, published_at, slug, title FROM posts";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var post = context.Posts.Add(new PostDto
                            {
                                CreatedOn = reader.GetUnixEpochAsDateTime("created_at").Value,
                                LastUpdatedOn = reader.GetUnixEpochAsDateTime("updated_at"),
                                MarkdownContent = reader.SafeGetString("markdown"),
                                PostedOn = reader.GetUnixEpochAsDateTime("published_at"),
                                Slug = reader.SafeGetString("slug"),
                                Title = reader.SafeGetString("title")
                            });

                            posts.Add(reader.GetInt32(reader.GetOrdinal("id")), post);
                        }
                    }
                }

                var tags = new Dictionary<int, EntityEntry<TagDto>>();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT id, slug, name, description FROM tags";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var tag = context.Tags.Add(new TagDto
                            {
                                Description = reader.SafeGetString("description"),
                                Name = reader.SafeGetString("name"),
                                Slug = reader.SafeGetString("slug")
                            });

                            tags.Add(reader.GetInt32(reader.GetOrdinal("id")), tag);
                        }
                    }
                }

                context.SaveChanges();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT post_id, tag_id FROM posts_tags";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var post = posts[reader.GetInt32(reader.GetOrdinal("post_id"))];
                            var tag = tags[reader.GetInt32(reader.GetOrdinal("tag_id"))];
                            var postTag = new PostTagDto { TagId = tag.Entity.Id, PostId = post.Entity.Id };
                            if (post.Entity.Tags == null)
                            {
                                post.Entity.Tags = new List<PostTagDto> { postTag };
                            }
                            else
                            {
                                post.Entity.Tags.Add(postTag);
                            }
                        }
                    }
                }

                context.SaveChanges();
            }
        }

        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime? GetUnixEpochAsDateTime(this SQLiteDataReader reader, string columnName)
        {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return null;
            var dateTime = epoch.AddMilliseconds(reader.GetInt64(reader.GetOrdinal(columnName)));
            dateTime.AddHours(-dateTime.Hour);
            dateTime.AddMinutes(-dateTime.Minute);
            dateTime.AddSeconds(-dateTime.Second);
            dateTime.AddMilliseconds(-dateTime.Millisecond);
            return dateTime;
        }

        public static string SafeGetString(this SQLiteDataReader reader, string columnName)
        {
            return reader.IsDBNull(reader.GetOrdinal(columnName)) ? null : reader.GetString(reader.GetOrdinal(columnName));
        }
    }
}
