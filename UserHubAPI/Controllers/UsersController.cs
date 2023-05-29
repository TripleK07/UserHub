using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserHubAPI.Entities;
using UserHubAPI.Helper;
using UserHubAPI.Services;

namespace UserHubAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/user")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("GetUsers")]
        [ProducesResponseType(200), ProducesResponseType(204)]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

        [HttpGet]
        [Route("GetUserByID/{id}")]
        [ProducesResponseType(200), ProducesResponseType(204)]
        public async Task<IActionResult> GetUserByID(Guid id)
        {
            var users = await _userService.GetById(id);
            return Ok(users);
        }

        [HttpGet]
        [Route("GetUserByUserName/{username}")]
        [ProducesResponseType(200), ProducesResponseType(204)]
        public async Task<IActionResult> GetUserByUserName(String username)
        {
            var user = await _userService.GetUserByUsername(username);
            if (user == null) {
                return NoContent();
            }

            return Ok(user);
        }

        [HttpPost]
        [Route("AddUser")]
        [ProducesResponseType(201), ProducesResponseType(400)]
        public async Task<IActionResult> AddUser(Users user)
        {
            var result = await _userService.Create(user);
            return CreatedAtAction("GetUserByID", new { id = result.ID }, result);
        }

        [HttpPut]
        [Route("EditUser")]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(404)]
        public async Task<IActionResult> EditUser(Users user) {
            var result = await _userService.Update(user);
            if (result)
                return Ok();
            else
                return NotFound();
        }

        [HttpDelete]
        [Route("DeleteUser")]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(404)]
        public async Task<IActionResult> DeleteUser(Guid id) {
            var result = await _userService.Delete(id);

            if (result)
                return Ok();
            else
                return NotFound();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(String loginID, String password) {
            var result = await _userService.Login(loginID, password);

            if (String.IsNullOrEmpty(result)){
                return Unauthorized();
            }

            return Ok(result);
        }
    }
}