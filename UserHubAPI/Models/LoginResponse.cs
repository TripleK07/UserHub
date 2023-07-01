using System;
using UserHubAPI.Entities;

namespace UserHubAPI.Models
{
	public class LoginResponse
	{
        public string Token { get; set; } = null!;

        public List<Menus> Menus { get; set; } = null!;
	}
}