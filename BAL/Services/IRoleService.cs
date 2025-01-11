using DAL.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role> GetRoleByIdAsync(int roleId);
        Task AddRoleAsync(string roleName);
        Task UpdateRoleAsync(int roleId, string roleName);
        Task DeleteRoleAsync(int roleId);
    }
}
