using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserHubAPI.Entities;
using UserHubAPI.Models;
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
        [Route("GetAllIncludeMenu")]
        public async Task<IActionResult> GetAllIncludeMenu()
        {
            var roles = await _roleService.GetAllIncludeMenu();
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
        [Route("GetByIdIncludeMenu/{id}")]
        public async Task<IActionResult> GetByIdIncludeMenu(Guid id)
        {
            var roles = await _roleService.GetByIdIncludeMenu(id);
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

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(RolesViewModel roleViewModel)
        {
            Roles role = ConvertRoleViewModelToRole(roleViewModel);

            var result = await _roleService.Create(role);
            return CreatedAtAction("GetById", new { id = result.ID }, result);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(RolesViewModel roleViewModel)
        {
            Roles role = ConvertRoleViewModelToRole(roleViewModel);

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

        private static Roles ConvertRoleViewModelToRole(RolesViewModel roleViewModel)
        {
            List<RoleMenu> menuList = new();
            foreach (Guid menu in roleViewModel.Menus)
            {
                 RoleMenu rm = new()
                 {
                    RoleId = roleViewModel.ID,
                    MenuId = menu,
                 };

                 menuList.Add(rm);
            }

            Roles role = new()
            {
                ID = roleViewModel.ID,
                RoleName = roleViewModel.RoleName,
                RoleDescription = roleViewModel.RoleDescription,
                IsActive = roleViewModel.IsActive,
                RecordStatus = roleViewModel.RecordStatus,
                RoleMenu = menuList
            };

            return role;
        }
    }
}