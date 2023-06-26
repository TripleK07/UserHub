using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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

        //entities
        public DbSet<Users> Users { get; set; } = null!;
        public DbSet<Roles> Roles { get; set; } = null!;
    }
}