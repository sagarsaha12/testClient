using Microsoft.EntityFrameworkCore;
using testClient.Entities;

namespace testClient.Data
{
    public class testClientContext : DbContext
    {
        protected override void OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-VG8KTFE;Initial Catalog=T729947_testClient;Persist Security Info=True;user id=sa;password=admin!234;Integrated Security=false;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=true;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInRole>().HasKey(a => a.Id);
            modelBuilder.Entity<UserToken>().HasKey(a => a.Id);
            modelBuilder.Entity<RoleEntitlement>().HasKey(a => a.Id);
            modelBuilder.Entity<Entity>().HasKey(a => a.Id);
            modelBuilder.Entity<Tenant>().HasKey(a => a.Id);
            modelBuilder.Entity<User>().HasKey(a => a.Id);
            modelBuilder.Entity<Role>().HasKey(a => a.Id);
            modelBuilder.Entity<Author>().HasKey(a => a.Id);
            modelBuilder.Entity<Books>().HasKey(a => a.Id);
            modelBuilder.Entity<UserInRole>().HasOne(a => a.Tenant).WithMany(b => b.UserInRoleInfo).HasForeignKey(c => c.TenantId);
            modelBuilder.Entity<UserInRole>().HasOne(a => a.Role).WithMany(b => b.UserInRoleInfo).HasForeignKey(c => c.RoleId);
            modelBuilder.Entity<UserInRole>().HasOne(a => a.User).WithMany(b => b.UserInRoleInfo).HasForeignKey(c => c.UserId);
            modelBuilder.Entity<UserInRole>().HasOne(a => a.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
            modelBuilder.Entity<UserInRole>().HasOne(a => a.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            modelBuilder.Entity<UserToken>().HasOne(a => a.Tenant).WithMany(b => b.UserTokenInfo).HasForeignKey(c => c.TenantId);
            modelBuilder.Entity<UserToken>().HasOne(a => a.User).WithMany(b => b.UserTokenInfo).HasForeignKey(c => c.UserId);
            modelBuilder.Entity<RoleEntitlement>().HasOne(a => a.Tenant).WithMany(b => b.RoleEntitlementInfo).HasForeignKey(c => c.TenantId);
            modelBuilder.Entity<RoleEntitlement>().HasOne(a => a.Role).WithMany(b => b.RoleEntitlementInfo).HasForeignKey(c => c.RoleId);
            modelBuilder.Entity<RoleEntitlement>().HasOne(a => a.Entity).WithMany(b => b.RoleEntitlementInfo).HasForeignKey(c => c.EntityId);
            modelBuilder.Entity<RoleEntitlement>().HasOne(a => a.CreatedByUser).WithMany(b => b.RoleEntitlementInfo).HasForeignKey(c => c.CreatedBy);
            modelBuilder.Entity<RoleEntitlement>().HasOne(a => a.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            modelBuilder.Entity<Entity>().HasOne(a => a.Tenant).WithMany(b => b.EntityInfo).HasForeignKey(c => c.TenantId);
            modelBuilder.Entity<Entity>().HasOne(a => a.CreatedByUser).WithMany(b => b.EntityInfo).HasForeignKey(c => c.CreatedBy);
            modelBuilder.Entity<Entity>().HasOne(a => a.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            modelBuilder.Entity<User>().HasOne(a => a.Tenant).WithMany(b => b.UserInfo).HasForeignKey(c => c.TenantId);
            modelBuilder.Entity<Role>().HasOne(a => a.Tenant).WithMany(b => b.RoleInfo).HasForeignKey(c => c.TenantId);
            modelBuilder.Entity<Role>().HasOne(a => a.CreatedByUser).WithMany(b => b.RoleInfo).HasForeignKey(c => c.CreatedBy);
            modelBuilder.Entity<Role>().HasOne(a => a.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            modelBuilder.Entity<Books>().HasOne(a => a.Author).WithMany(b => b.BooksInfo).HasForeignKey(c => c.AuthorId);
        }

        public DbSet<UserInRole> UserInRole { get; set; }
        public DbSet<UserToken> UserToken { get; set; }
        public DbSet<RoleEntitlement> RoleEntitlement { get; set; }
        public DbSet<Entity> Entity { get; set; }
        public DbSet<Tenant> Tenant { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Author> Author { get; set; }
        public DbSet<Books> Books { get; set; }
    }
}