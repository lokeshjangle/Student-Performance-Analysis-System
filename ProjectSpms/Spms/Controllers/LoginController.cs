using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using spms.Models;

namespace spms.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly SiteDbContext _site;

        public LoginController(SiteDbContext site)
        {
            _site = site;
        }

        [HttpPost]
        public async Task<IActionResult> AdminLogin(Admin input)
        {
            var admin = await _site.Admins.FirstOrDefaultAsync(a => a.UserName == input.UserName);

            if (admin != null && admin.Password == input.Password)
            {
                var identity = new ClaimsIdentity("Admin");
                identity.AddClaim(new Claim(ClaimTypes.Name, input.UserName));

                await HttpContext.SignInAsync(new ClaimsPrincipal(identity));
                var selection = _site.Admins
                                .Where(a => a.UserName==input.UserName);

                return Ok(selection.ToList());
            }
            else
            {
                return NotFound("User Not Found");
            }
        }
        [HttpGet]
        public async Task<IActionResult> OnGetLogout(){
                Console.WriteLine();
                await HttpContext.SignOutAsync();
                return Ok("Successful Logout");
        }
    }
}
