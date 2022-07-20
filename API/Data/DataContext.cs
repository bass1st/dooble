using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        // Constructor class
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<UserLike> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }

        // Overriding OnModelCreating method from the DbContext class
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Create table primary key derived as a combination of source user and liked user IDs
            builder.Entity<UserLike>().HasKey(k => new {k.SourceUserId, k.LikedUserId});
            // Configure relationships
            builder.Entity<UserLike>().HasOne(s => s.SourceUser).WithMany(l => l.LikedUsers).HasForeignKey(s => s.SourceUserId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<UserLike>().HasOne(s => s.LikedUser).WithMany(l => l.LikedByUsers).HasForeignKey(s => s.LikedUserId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Message>().HasOne(u => u.Recipient).WithMany(m => m.MessagesReceived).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Message>().HasOne(u => u.Sender).WithMany(m => m.MessagesSent).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
