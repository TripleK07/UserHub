using System;
using Microsoft.AspNetCore.Mvc;
using UserHubAPI.Entities;
using UserHubAPI.Services;

namespace UserHubAPI.Controllers
{
    [ApiController]
    [Route("api/v1/user")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// to use custom route name
        /// </summary>
        /// <value>[Route("getUsers")]</value>
        [HttpGet]
        [Route("GetUsers")]
        [ProducesResponseType(200), ProducesResponseType(204)]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }


        [HttpGet]
        [Route("GetUserByID")]
        [ProducesResponseType(200), ProducesResponseType(204)]
        public async Task<IActionResult> GetUserByID(Guid ID)
        {
            var users = await _userService.GetById(ID);
            return Ok(users);
        }

        [HttpPost]
        [Route("AddUser")]
        [ProducesResponseType(201), ProducesResponseType(400)]
        public async Task<IActionResult> AddUser(Users user)
        {
            var entity = await _userService.Create(user);
            return CreatedAtAction("GetUserByID", entity.ID,entity);
        }

        [HttpPut]
        [Route("EditUser")]
        [ProducesResponseType(200), ProducesResponseType(400)]
        public async Task<IActionResult> EditUser(Users user) {
            await _userService.Update(user);
            return Ok();
        }
    }
}

