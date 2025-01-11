using DAL.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(User user, string password);
        Task<string> LoginAsync(string username, string password);
    }
}
