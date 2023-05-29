using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UserHubAPI.Helper
{
    public static class JwtTokenGenerator
    {
        private static readonly IConfiguration _configuration;

        static JwtTokenGenerator()
        {
            // Set up the configuration
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json") // Path to your appsettings.json file
                .Build();

            _configuration = configurationBuilder;
        }

        public static string GenerateJwtToken(string userId, string userRole)
        {
            var claims = new[]
            {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Role, userRole)
                    // Add additional claims as needed
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), // Token expiration time
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}