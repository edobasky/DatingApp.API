using DatingApp.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected DataContext()
        {
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<UserLike> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserLike>()
                .HasKey(k => new { k.SourceUserId, k.TargetUserId });

            builder.Entity<UserLike>()
                .HasOne(s => s.SourceUser)
                .WithMany(l => l.LikedUsers)
                .HasForeignKey(s => s.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserLike>()
               .HasOne(s => s.TargetUser)
               .WithMany(l => l.LikedByUsers)
               .HasForeignKey(s => s.TargetUserId)
               .OnDelete(DeleteBehavior.Cascade);  //  .OnDelete(DeleteBehavior.NoAction); for sql server database
           
            builder.Entity<Message>()
                .HasOne(x => x.Recipient)
                .WithMany(x => x.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
               .HasOne(x => x.Sender)
               .WithMany(x => x.MessagesSent)
               .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
