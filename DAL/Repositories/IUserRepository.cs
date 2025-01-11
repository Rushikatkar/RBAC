using DAL.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByEmailAsync(string email);
        Task AddUserAsync(User user);
        Task<User> GetUserForLoginAsync(string username);
        Task<bool> RoleExistsAsync(int roleId);
    }
}
