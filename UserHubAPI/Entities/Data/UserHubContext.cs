using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;

namespace UserHubAPI.Entities.Data
{
    public class UserHubContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public UserHubContext(DbContextOptions<UserHubContext> options, IConfiguration configuration)
        : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /// <summary>
            /// you can defined connection here. But you cannot run ef commands in terminal.
            /// So we defined the config in program.cs
            /// </summary>
            //optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            //optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //role-menu setup
            modelBuilder.Entity<RoleMenu>().HasKey(rm => new { rm.RoleId, rm.MenuId });
            modelBuilder.Entity<RoleMenu>().HasOne<Roles>(r => r.Role).WithMany(m => m.RoleMenu).HasForeignKey(rm => rm.RoleId);
            modelBuilder.Entity<RoleMenu>().HasOne<Menus>(m => m.Menu).WithMany(r => r.RoleMenu).HasForeignKey(rm => rm.MenuId);

            //user-role setup
            modelBuilder.Entity<UserRole>().HasKey(ur => new { ur.UserId, ur.RoleId });
            modelBuilder.Entity<UserRole>().HasOne<Users>(u => u.User).WithMany(r => r.UserRole).HasForeignKey(ur => ur.UserId);
            modelBuilder.Entity<UserRole>().HasOne<Roles>(r => r.Role).WithMany(r => r.UserRole).HasForeignKey(ur => ur.RoleId);

            //modelBuilder.Entity<Base>().Property(e => e.Autokey)
            //    .Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var createdDate = entityType.FindProperty("CreatedDate");

                if (createdDate != null && createdDate.ClrType == typeof(DateTime))
                {
                    createdDate.SetDefaultValueSql("GETDATE()");
                    //createdDate.SetDefaultValueSql("NOW()"); //for postgres
                }

                var modifiedDate = entityType.FindProperty("ModifiedDate");

                if (modifiedDate != null && modifiedDate.ClrType == typeof(DateTime))
                {
                    modifiedDate.SetDefaultValueSql("GETDATE()");
                    //modifiedDate.SetDefaultValueSql("NOW()");
                }
            }
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ChangeTracker.DetectChanges();

            // Exclude auto-incrementing column from updates
            foreach (var entry in ChangeTracker.Entries<Base>())
            {
                if (entry.State == EntityState.Modified)
                {
                    //prevent updating column in Edit condition
                    entry.Property(e => e.Autokey).IsModified = false;
                    entry.Property(e => e.CreatedDate).IsModified = false;
                    entry.Property(e => e.CreatedBy).IsModified = false;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public void InitialData()
        {
            Guid menuId = Guid.NewGuid();
            Guid roleId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            if (!Users.Any())
            {
                Users.Add(new Users
                {
                    ID = userId,
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now,
                    ModifiedBy = "Admin",
                    ModifiedDate = DateTime.Now,
                    Email = "triplek07@gmail.com",
                    IsActive = true,
                    RecordStatus = 1,
                    LoginID = "kkk",
                    Password = "1qb1pHE8EwvGI6dRUQIu4ShK/hAZi16wASvI6DedV6M=",
                    UserName = "Khant Ko Ko",
                });
            }

            if (!Menus.Any())
            {
                Menus.Add(new Menus
                {
                    ID = menuId,
                    MenuName = "Setup",
                    MenuDescription = "Setup",
                    ParentId = Guid.Empty,
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now,
                    ModifiedBy = "Admin",
                    ModifiedDate = DateTime.Now,
                    IsActive = true,
                    RecordStatus = 1,
                });
            }

            if (!Roles.Any())
            {
                Roles.Add(new Roles
                {
                    ID = roleId,
                    RoleName = "Admin",
                    RoleDescription = "Admin",
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    ModifiedBy = "Admin",
                    ModifiedDate = DateTime.Now,
                    RecordStatus = 1,
                });
            }

            if (!RoleMenu.Any())
            {
                RoleMenu.Add(new RoleMenu
                {
                    MenuId = menuId,
                    RoleId = roleId,
                });
            }

            if (!UserRole.Any())
            {
                UserRole.Add(new UserRole
                {
                    UserId = userId,
                    RoleId = roleId,
                });
            }

            SaveChanges();

        }

        //entities
        public DbSet<Users> Users { get; set; } = null!;
        public DbSet<Roles> Roles { get; set; } = null!;
        public DbSet<Menus> Menus { get; set; } = null!;
        public DbSet<RoleMenu> RoleMenu { get; set; } = null!;
        public DbSet<UserRole> UserRole { get; set; } = null!;
    }
}