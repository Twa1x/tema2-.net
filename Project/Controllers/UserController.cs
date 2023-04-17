using Core.Dtos;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Project.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private UserService userService { get; set; }

        public UsersController(UserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("/register")]
        [AllowAnonymous]
        public IActionResult Register(RegisterDto payload)
        {
            userService.Register(payload);
            return Ok();
        }




        [HttpPost("/login")]
        [AllowAnonymous]
        public IActionResult Login(LoginDto payload)
        {
            var jwtToken = userService.Validate(payload);

            return Ok(new { token = jwtToken });
        }

        [HttpGet("test-auth")]
        public IActionResult TestLogin()
        {

            ClaimsPrincipal user = User;

            var result = "";

            foreach (var claim in user.Claims)
            {
                result += claim.Type + " : " + claim.Value + "\n";
            }



            var hasRole_student = user.IsInRole("Student");
            var hasRole_teacher = user.IsInRole("Teacher");

            return Ok(result);
        }

        [HttpGet("students-only")]
        [Authorize(Roles = "Student")]
        public ActionResult<string> HelloStudents()
        {
            return Ok("Hello students!");
        }
    }
}
