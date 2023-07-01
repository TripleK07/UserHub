using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserHubAPI.Entities;
using UserHubAPI.Services;

namespace UserHubAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/menu")]
    public class MenusController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenusController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var Menus = await _menuService.GetAll();
            return Ok(Menus);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var Menus = await _menuService.GetById(id);
            return Ok(Menus);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(Menus menu)
        {
            var result = await _menuService.Create(menu);
            return CreatedAtAction("GetById", new { id = result.ID }, result);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(Menus menu) {
            var result = await _menuService.Update(menu);
            if (result)
                return Ok();
            else
                return NotFound();
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(Guid id) {
            var result = await _menuService.Delete(id);

            if (result)
                return Ok();
            else
                return NotFound();
        }
    }
}