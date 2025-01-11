using BAL.Services;
using DAL.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Presentation_Layer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Endpoint for user registration
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Username = request.Username,
                    Email = request.Email,
                    RoleId = request.RoleId // Set the RoleId from the request
                };

                var result = await _userService.RegisterUserAsync(user, request.Password);
                if (!result)
                {
                    return BadRequest("Username or Email already taken.");
                }

                return Ok("User registered successfully.");
            }

            return BadRequest("Invalid input.");
        }


        // Endpoint for user login
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            var token = await _userService.LoginAsync(request.Username, request.Password);
            if (token == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            return Ok(new { Token = token });
        }
    }

    // Models for request body
    public class RegisterRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public int RoleId { get; set; }
    }


    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
