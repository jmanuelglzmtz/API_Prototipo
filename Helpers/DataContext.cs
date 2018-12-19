using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebApi.Entities;

namespace WebApi.Helpers
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {  }
        public DbSet<User> Users { get; set; }

        public DbSet<UserRole> UserRole { get; set; }

        public DbSet<RoleModule> RoleModule { get; set; }

        public DbSet<RolePermission> RolePermission { get; set; }

        public DbSet<Product> Product { get; set; }        
        public DbSet<Role> Role { get; set; }
        public DbSet<Module> Module { get; set; }
        public DbSet<Permission> Permission { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { /* 
            modelBuilder.Entity<UserRole>()
                .HasKey(pc => new { pc.RoleId, pc.UserId });
            
            modelBuilder.Entity<RoleModule>()
                .HasKey(pc => new { pc.RoleId, pc.ModuleId });
            
            modelBuilder.Entity<RolePermission>()
            .HasKey(pc => new { pc.RoleId, pc.PermissionId });    
            */
        }

    }
}