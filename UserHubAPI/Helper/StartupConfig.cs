using System;
using System.Reflection;
using System.Security.Cryptography;

namespace UserHubAPI.Helper
{
	public class StartupConfig
	{
        private static bool ImplementsServiceInterface(Type type)
        {
            var implementedInterfaces = type.GetInterfaces();
            return implementedInterfaces.Any(interfaceType =>
                interfaceType.Name.StartsWith("I") && interfaceType.Name.EndsWith(type.Name));
        }

        public static void RegisterServices(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // Scan the assembly for types that implement the IService interface
            var serviceTypes = assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && ImplementsServiceInterface(type))
                .ToList();

            // Register the services
            foreach (var serviceType in serviceTypes)
            {
                if (serviceType.Name.ToLower() != "unitofwork")
                {
                    var implementedInterface = serviceType.GetInterface($"I{serviceType.Name}");

                    if(implementedInterface != null)
                        services.AddScoped(implementedInterface, serviceType);
                }
            }
        }

        public static string GenerateSecretKey(int keySizeInBytes = 32)
        {
            byte[] secretKeyBytes = new byte[keySizeInBytes];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(secretKeyBytes);
            }

            var temp = Convert.ToBase64String(secretKeyBytes);
            return temp;
        }
    }
}

