// RoleController.cs
using BAL.Services;
using DAL.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Presentation_Layer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")] // Restrict access to Admin only
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetRoleById(int roleId)
        {
            if (roleId <= 0)
                return BadRequest("Invalid Role ID.");

            var role = await _roleService.GetRoleByIdAsync(roleId);

            if (role == null)
                return NotFound("Role not found.");

            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole([FromBody] string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return BadRequest("Role name cannot be empty.");

            await _roleService.AddRoleAsync(roleName);
            return Created("", "Role added successfully.");
        }

        [HttpPut("{roleId}")]
        public async Task<IActionResult> UpdateRole(int roleId, [FromBody] string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return BadRequest("Role name cannot be empty.");

            await _roleService.UpdateRoleAsync(roleId, roleName);
            return Ok("Role updated successfully.");
        }

        [HttpDelete("{roleId}")]
        public async Task<IActionResult> DeleteRole(int roleId)
        {
            await _roleService.DeleteRoleAsync(roleId);
            return Ok("Role deleted successfully.");
        }
    }
}
