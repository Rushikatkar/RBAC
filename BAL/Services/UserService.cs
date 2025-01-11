using DAL.Models;
using DAL.Models.Entities;
using DAL.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        // Method for user registration
        public async Task<bool> RegisterUserAsync(User user, string password)
        {
            // Check if the username or email already exists
            var existingUser = await _userRepository.GetUserByUsernameAsync(user.Username);
            if (existingUser != null)
                return false; // Username already exists

            var existingEmail = await _userRepository.GetUserByEmailAsync(user.Email);
            if (existingEmail != null)
                return false; // Email already exists

            // Validate RoleId
            var roleExists = await _userRepository.RoleExistsAsync(user.RoleId);
            if (!roleExists)
                throw new Exception("Invalid RoleId.");

            // Hash the password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);

            await _userRepository.AddUserAsync(user);
            return true; // Registration successful
        }


        // Method for user login
        public async Task<string> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetUserForLoginAsync(username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null; // Invalid credentials

            // Generate JWT token if login is successful
            return GenerateJwtToken(user);
        }

        // Method to generate JWT token
        private string GenerateJwtToken(User user)
        {
            var role = user.RoleId.ToString(); // Assuming you are using RoleId. You can modify this to fetch the role name from the database if needed.

            var claims = new[]
            {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        new Claim(ClaimTypes.Role, role)  // Adding role to the claims
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(int.Parse(_configuration["Jwt:ExpiresInMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
