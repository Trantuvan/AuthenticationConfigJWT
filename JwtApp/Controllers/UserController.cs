using JwtApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace JwtApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public IActionResult Public()
        {
            return Ok("Hi, you're on public property");
        }

        [Authorize(Roles = "Administration")]
        public IActionResult Admins()
        {
            var currentUser = GetCurrentUser();

            return Ok($"Hi {currentUser.GivenName}, you're an {currentUser.Roles}");
        }

        [Authorize(Roles = "Seller")]
        public IActionResult Sellers()
        {
            var currentUser = GetCurrentUser();

            return Ok($"Hi {currentUser.GivenName}, you're an {currentUser.Roles}");
        }

        [Authorize(Roles = "Administration,Seller")]
        public IActionResult AdminSellers()
        {
            var currentUser = GetCurrentUser();

            return Ok($"Hi {currentUser.GivenName}, you're an {currentUser.Roles}");
        }
        private UserModel GetCurrentUser()
        {
            var identity = (ClaimsIdentity)HttpContext.User.Identity;

            if (identity != null)
            {
                var userClaims = identity.Claims;
                return new UserModel
                {
                    EmailAddress = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                    GivenName = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value,
                    Surname = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value,
                    Username = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
                    Roles = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value,
                };
            }
            return null;
        }
    }
}
