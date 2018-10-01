using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCore_Template.Data.Model;
using System.Threading.Tasks;

namespace WebApp1.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            var u = new ApplicationUser
            {
                UserName = "kkk",
                Email = "ggg@com.pl",
            };

            var p = _userManager.PasswordHasher.HashPassword(u, "password");
            u.PasswordHash = p;

            return View();
        }

        /// <summary>
        /// Auto login page
        /// </summary>
        /// <param name="returnUrl">Url to return to if succesfully logged in</param>
        /// <returns></returns>
        [Route("/login")]
        public async Task<IActionResult> LoginAsync(string returnUrl)
        {
            return Content("Logged in", "text/html");
        }

        /// <summary>
        /// Auto logout page
        /// </summary>
        /// <returns></returns>
        [Route("/logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            return Content("Logged out", "text/html");
        }

        public IActionResult Private()
        {
            return View();
        }
    }
}
