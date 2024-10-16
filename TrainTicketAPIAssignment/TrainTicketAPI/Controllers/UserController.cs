using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainTicketAPI.Interfaces;
using TrainTicketAPI.Models;

namespace TrainTicketAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUP([FromBody] SignUpData signupdata)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                UserName = signupdata.UserName,
                Email = signupdata.Email
            };

            var result = await _userManager.CreateAsync(user, signupdata.Password);

            if (result.Succeeded)
            {
                return Ok(
                    new AuthenticatedUser
                    {
                        Username = user.UserName,
                        Email = user.Email,
                        Token = _tokenService.CreateToken(user)
                    }
                );
            }
            else
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new { Errors = errors });
            }
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInData signindata)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(userObject => userObject.UserName == signindata.UserName.ToLower());

            if (user == null) return Unauthorized("Can't find the user.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, signindata.Password, false);

            if (!result.Succeeded) return Unauthorized("Username or password is incorrect.");

            return Ok(
                    new AuthenticatedUser
                    {
                        Username = user.UserName,
                        Email = user.Email,
                        Token = _tokenService.CreateToken(user)
                    }
            );
        }
    }

    public class SignUpData
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class SignInData
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class AuthenticatedUser
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
