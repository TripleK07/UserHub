using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserHubAPI.Entities;
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
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var users = await _userService.GetById(id);
            return Ok(users);
        }

        [HttpGet]
        [Route("GetUserByUserName/{username}")]
        public async Task<IActionResult> GetUserByUserName(String username)
        {
            var user = await _userService.GetUserByUsername(username);
            if (user == null) {
                return NoContent();
            }

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(Users user)
        {
            var result = await _userService.Create(user);
            return CreatedAtAction("GetById", new { id = result.ID }, result);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(Users user) {
            var result = await _userService.Update(user);
            if (result)
                return Ok();
            else
                return NotFound();
        }

        [HttpDelete]
        [Route("Delete")]
        //[ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid id) {
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