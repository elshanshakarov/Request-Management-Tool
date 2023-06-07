using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using File = Entities.Concrete.File;
using Type = Entities.Concrete.Type;

namespace DataAccess.Context
{
    public class CICDbContext : DbContext
    {
        public CICDbContext(DbContextOptions<CICDbContext> contextOptions) : base(contextOptions) { }

        public CICDbContext() { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
            modelBuilder.Entity<Request>().Property(p => p.Date).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Request>().Property(p => p.StatusId).HasDefaultValue((short)1);
            modelBuilder.Entity<User>().Property(p => p.Active).HasDefaultValue(true).IsRequired();
            modelBuilder.Entity<User>().Property(p => p.NotificationPermit).HasDefaultValue(true).IsRequired();
            modelBuilder.Entity<User>().Property(p => p.CreatedDate).HasDefaultValueSql("getdate()").IsRequired();
            modelBuilder.Entity<Request>().HasOne(x => x.Creator).WithMany(x => x.CreatorRequests).HasForeignKey(x => x.CreatorId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Request>().HasOne(x => x.Executor).WithMany(x => x.ExecutorRequests).HasForeignKey(x => x.ExecutorId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Comment>().Property(p => p.Date).HasDefaultValueSql("getdate()");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestType> RequestTypes { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<CategoryUser> CategoryUsers { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<NonWorkingDay> NonWorkingDays { get; set; }
        public DbSet<File> Files { get; set; }
    }
}
