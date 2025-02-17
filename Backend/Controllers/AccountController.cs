using Backend.Dtos.Account;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager) {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Check if email is already registered
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> IsEmailAlreadyRegistered(string email) { 
            AppUser user = await _userManager.FindByEmailAsync(email);
            return user == null ? Ok(true) : Ok(false);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            try
            {
                //Check Model state
                if (!ModelState.IsValid) { 
                    string errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return Problem(errorMessage, statusCode: 400); 
                }

                var appUser = new AppUser
                {
                    UserName = model.UserName,
                    Email = model.Email
                };
                // Create user
                var createdUser = await _userManager.CreateAsync(appUser, model.Password);

                if (createdUser.Succeeded)
                {
                    // Add user to role
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if (roleResult.Succeeded) return Ok(
                            new NewUserDto
                            {
                                UserName = appUser.UserName,
                                Email = appUser.Email,
                                Token = _tokenService.CreateToken(appUser)
                            }
                        );
                    else return StatusCode(500, $"Failed to create user role line 35:- {roleResult.Errors}");
                }
                else return StatusCode(500, $"Failed to create user :- {createdUser.Errors}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred :- {ex.Message}");
            };
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            try
            {
                if(!ModelState.IsValid) return BadRequest(ModelState);
                //find user by email
                //var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == model.UserName.ToLower());
                var user = await _userManager.FindByEmailAsync(model.UserName.ToLower());
                //check if user exists
                if (user == null) return Unauthorized("Invalid Email");
                //check if password is correct
                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                if (!result.Succeeded) return Unauthorized("Invalid Password");
                return Ok(
                    new NewUserDto
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        Token = _tokenService.CreateToken(user)
                    }
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred :- {ex.Message}");
            };
        }
    }
}
