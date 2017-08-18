using DataAccess.Contracts.Blog;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class PdDbContext : DbContext
    {
        public DbSet<CommentDto> Comments{ get; set; }
        public DbSet<PostDto> Posts { get; set; }
        public DbSet<TagDto> Tags{ get; set; }

        public PdDbContext() {}

        public PdDbContext(DbContextOptions<PdDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommentDto>().ToTable("Comments", "blog");

            modelBuilder.Entity<PostDto>().ToTable("Posts", "blog");

            modelBuilder.Entity<TagDto>().ToTable("Tags", "blog");

            modelBuilder.Entity<PostTagDto>()
                        .ToTable("PostsTags", "blog")
                        .HasKey(pt => new { pt.PostId, pt.TagId });

            modelBuilder.Entity<PostTagDto>()
                        .HasOne(pt => pt.Post)
                        .WithMany(pt => pt.Tags)
                        .HasForeignKey(pt => pt.PostId);

            modelBuilder.Entity<PostTagDto>()
                        .HasOne(pt => pt.Tag)
                        .WithMany(pt => pt.Posts)
                        .HasForeignKey(pt => pt.TagId);

            modelBuilder.Entity<PostCommentDto>()
                        .ToTable("PostsComments", "blog")
                        .HasKey(pt => new { pt.PostId, pt.CommentId });

            modelBuilder.Entity<PostCommentDto>()
                        .HasOne(pt => pt.Post)
                        .WithMany(pt => pt.Comments)
                        .HasForeignKey(pt => pt.PostId);

            modelBuilder.Entity<PostCommentDto>()
                        .HasOne(pt => pt.Comment)
                        .WithMany(pt => pt.Posts)
                        .HasForeignKey(pt => pt.CommentId);
        }
    }
}
