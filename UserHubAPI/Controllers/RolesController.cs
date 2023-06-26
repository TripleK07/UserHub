using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserHubAPI.Entities;
using UserHubAPI.Services;

namespace UserHubAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/role")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAll();
            return Ok(roles);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var roles = await _roleService.GetById(id);
            return Ok(roles);
        }

        [HttpGet]
        [Route("GetRoleByRoleName/{roleName}")]
        public async Task<IActionResult> GetRoleByRoleName(String roleName)
        {
            var user = await _roleService.GetRoleByRoleName(roleName);
            if (user == null) {
                return NoContent();
            }

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(Roles role)
        {
            var result = await _roleService.Create(role);
            return CreatedAtAction("GetById", new { id = result.ID }, result);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(Roles role) {
            var result = await _roleService.Update(role);
            if (result)
                return Ok();
            else
                return NotFound();
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(Guid id) {
            var result = await _roleService.Delete(id);

            if (result)
                return Ok();
            else
                return NotFound();
        }
    }
}