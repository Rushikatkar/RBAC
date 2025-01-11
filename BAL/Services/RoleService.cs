using DAL.Models.Entities;
using DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task AddRoleAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Role name cannot be null or empty.", nameof(roleName));

            await _roleRepository.AddRoleAsync(roleName);
        }

        public async Task UpdateRoleAsync(int roleId, string roleName)
        {
            if (roleId <= 0)
                throw new ArgumentException("Invalid Role ID.", nameof(roleId));

            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Role name cannot be null or empty.", nameof(roleName));

            await _roleRepository.UpdateRoleAsync(roleId, roleName);
        }

        public async Task DeleteRoleAsync(int roleId)
        {
            if (roleId <= 0)
                throw new ArgumentException("Invalid Role ID.", nameof(roleId));

            await _roleRepository.DeleteRoleAsync(roleId);
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _roleRepository.GetAllRolesAsync();
        }

        /// <summary>
        /// Retrieves a specific role by its ID.
        /// </summary>
        /// <param name="roleId">The ID of the role to retrieve.</param>
        /// <returns>The role if found; otherwise, null.</returns>
        public async Task<Role> GetRoleByIdAsync(int roleId)
        {
            if (roleId <= 0)
                throw new ArgumentException("Invalid Role ID.", nameof(roleId));

            return await _roleRepository.GetRoleByIdAsync(roleId);
        }
    }
}
