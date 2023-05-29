﻿using System;
using UserHubAPI.Entities;
using UserHubAPI.Helper;

namespace UserHubAPI.Services
{
	public class AuthenticationService
	{
        private readonly IUserService _userService;

        public AuthenticationService(IUserService userService)
        {
            _userService = userService;
        }

        public Users? AuthenticateUser(String loginID, string password)
        {
            // Retrieve the user from the user repository
            return _userService.ValidateUserCredential(loginID, password).Result;
        }
    }
}