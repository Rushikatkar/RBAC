using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.DTOs
{
    public class UserDTO
    {
        public class RegisterDto
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Role { get; set; }
        }

        public class LoginDto
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

    }
}
