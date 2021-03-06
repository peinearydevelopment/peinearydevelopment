﻿using DataAccess.Contracts;
using DataAccess.Contracts.Blog;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class PdDbContext : DbContext
    {
        public DbSet<CommentDto> Comments{ get; set; }
        public DbSet<PostDto> Posts { get; set; }
        public DbSet<TagDto> Tags{ get; set; }
        public DbSet<IpInformationDto> IpInformations { get; set; }
        public DbSet<ActionTakenDto> ActionsTaken { get; set; }

        private PdDbContext()
        {
        }

        public PdDbContext(DbContextOptions<PdDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommentDto>(entity => entity.ToTable("Comments", "blog"));
            modelBuilder.Entity<PostDto>(entity => entity.ToTable("Posts", "blog"));
            modelBuilder.Entity<TagDto>(entity => entity.ToTable("Tags", "blog"));
            modelBuilder.Entity<IpInformationDto>(entity => entity.ToTable("IpInformation", "stats"));
            modelBuilder.Entity<ActionTakenDto>(entity => entity.ToTable("ActionsTaken", "stats"));

            modelBuilder.Entity<PostTagDto>(entity =>
            {
                entity.ToTable("PostsTags", "blog")
                      .HasKey(pt => new { pt.PostId, pt.TagId });

                entity.HasOne(pt => pt.Post)
                      .WithMany(pt => pt.Tags)
                      .HasForeignKey(pt => pt.PostId);

                entity.HasOne(pt => pt.Tag)
                      .WithMany(pt => pt.Posts)
                      .HasForeignKey(pt => pt.TagId);
            });
        }
    }
}
