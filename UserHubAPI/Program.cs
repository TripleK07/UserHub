using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using UserHubAPI.Entities.Data;
using UserHubAPI.Repositories;
using UserHubAPI.Repositories.IRepositories;
using UserHubAPI.Services;
using System.Security.Cryptography;
using UserHubAPI.Helper;

var builder = WebApplication.CreateBuilder(args);

// Add dbContext middleware
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<UserHubContext>(options =>
    options.UseSqlServer(connectionString));

//    options.UseNpgsql(connectionString));
// Datetime column for postgres
//AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Register repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

// Register unit of work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//dynamatically setup DI for services
StartupConfig.RegisterServices(builder.Services);

// Add JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        };
        });

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( c=> c.EnableAnnotations() );

//logging
builder.Host.UseNLog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();


