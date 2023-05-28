using System.Reflection;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using UserHubAPI.Entities.Data;
using UserHubAPI.Repositories;
using UserHubAPI.Repositories.IRepositories;
using UserHubAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add dbContext middleware
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<UserHubContext>(options =>
    options.UseSqlServer(connectionString));

//    options.UseNpgsql(connectionString));
// datetime column for postgres
//AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Register repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

// Register unit of work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register other services
//builder.Services.AddScoped<IUserService, UserService>();

//dynamatically setup DI for services
RegisterServices(builder.Services); 

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

app.UseAuthorization();

app.MapControllers();

app.Run();

static void RegisterServices(IServiceCollection services)
{
    var assembly = Assembly.GetExecutingAssembly();

    // Scan the assembly for types that implement the IService interface
    var serviceTypes = assembly.GetTypes()
        .Where(type => type.IsClass && !type.IsAbstract && ImplementsServiceInterface(type))
        .ToList();

    // Register the services
    foreach (var serviceType in serviceTypes)
    {
        if (serviceType.Name.ToLower() != "unitofwork") {
            var implementedInterface = serviceType.GetInterface($"I{serviceType.Name}");
            services.AddScoped(implementedInterface, serviceType);
        }
    }
}

static bool ImplementsServiceInterface(Type type)
{
    var implementedInterfaces = type.GetInterfaces();
    return implementedInterfaces.Any(interfaceType =>
        interfaceType.Name.StartsWith("I") && interfaceType.Name.EndsWith(type.Name));
}
