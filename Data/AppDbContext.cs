using Microsoft.EntityFrameworkCore;
using ApiRestNetNoxun.Models;

namespace ApiRestNetNoxun.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Procedure> Procedures { get; set; }
        public DbSet<DataSet> DataSets { get; set; }
        public DbSet<Field> Fields { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

             modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserID);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleID);





            modelBuilder.Entity<Procedure>()
                .HasOne(p => p.CreatedByUser)
                .WithMany(u => u.ProceduresCreated)
                .HasForeignKey(p => p.CreatedByUserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Procedure>()
                .HasOne(p => p.LastModifiedUser)
                .WithMany(u => u.ProceduresModified)
                .HasForeignKey(p => p.LastModifiedUserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DataSet>()
                .HasOne(ds => ds.Procedure)
                .WithMany(p => p.DataSets)
                .HasForeignKey(ds => ds.ProcedureID);

            modelBuilder.Entity<DataSet>()
                .HasOne(ds => ds.Field)
                .WithMany(f => f.DataSets)
                .HasForeignKey(ds => ds.FieldID);

            modelBuilder.Entity<Role>().HasData(
                new Role { RoleID = 1, RoleName = "Admin", Description = "Administrador" },
                new Role { RoleID = 2, RoleName = "User", Description = "Usuario común" },
                new Role { RoleID = 3, RoleName = "Editor", Description = "Puede editar datos" }
            );

        }
    }
}





