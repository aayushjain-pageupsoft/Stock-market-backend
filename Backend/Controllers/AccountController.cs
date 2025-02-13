using Backend.Dtos.Account;
using Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        public AccountController(UserManager<AppUser> userManager) {
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var appUser = new AppUser
                {
                    UserName = model.UserName,
                    Email = model.Email
                };
                var createdUser = await _userManager.CreateAsync(appUser, model.Password);

                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.CreateAsync(appUser, "User");
                    if (roleResult.Succeeded) return Ok("User Created Successfully");
                    else return StatusCode(500, $"Failed to create user role :- {roleResult.Errors}");
                }
                else return StatusCode(500, $"Failed to create user :- {createdUser.Errors}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred :- {ex.Message}");
            };
        }
    }
}
