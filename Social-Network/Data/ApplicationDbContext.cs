using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Social_Network.Data.social_network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Social_Network.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Images> Images { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        //public DbSet<Friend> Friends { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>()
                .Property(b => b.CreateAt)
                .HasDefaultValueSql("getdate()");
            builder.Entity<Post>()
                .Property(b => b.CreateAt)
                .HasDefaultValueSql("getdate()");
            builder.Entity<Comment>()
                .Property(b => b.CreateAt)
                .HasDefaultValueSql("getdate()");
            builder.Entity<Like>()
                .Property(b => b.CreateAt)
                .HasDefaultValueSql("getdate()");
            builder.Entity<Images>()
                .Property(b => b.CreateAt)
                .HasDefaultValueSql("getdate()");
            builder.Entity<User>()
                .Property(b => b.AvatarURL)
                .HasDefaultValueSql("'/storage/img/avatar.png'");

            /*
            builder.Entity<User>()
                .HasMany(p1 => p1.Posts)
                .WithOne(p2 => p2.Author)
                .HasForeignKey(p3 => p3.Author.Id);
            builder.Entity<User>()
               .HasMany(p1 => p1.Comments)
               .WithOne(p2 => p2.Author)
               .HasForeignKey(p3 => p3.Author.Id);
            builder.Entity<User>()
               .HasMany(p1 => p1.Likes)
               .WithOne(p2 => p2.Author)
               .HasForeignKey(p3 => p3.Author.Id);
            */

            builder.Entity<User>()
                .HasMany(f1 => f1.Folowers)
                .WithMany(s => s.Folowing)
                .UsingEntity(u => u.ToTable("Folowers"));

        }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
