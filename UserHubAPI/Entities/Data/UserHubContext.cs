using System;
using Microsoft.EntityFrameworkCore;
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
            //if you want customize table name
            //modelBuilder.Entity<User>().ToTable("tblUser");
        }

        //entities
        public DbSet<Users> Users { get; set; } = null!;
    }
}