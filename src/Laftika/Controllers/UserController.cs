using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Laftika.Library.Authentication;
using Laftika.Models;
using Laftika.DAL;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Laftika.Controllers
{
    public class UserController : Controller
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly Login _login;

        public UserController(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
            _login = new Login(HttpContext, _userRepository);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User model)
        {
            if (_login.CheckAuthentication())
            {
                return RedirectToAction("Profile", "User");
            }

            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                return RedirectToAction("Index", "User");
            }

            return RedirectToAction(_login.CreateAuthentication(model.Username, model.Password).IsCompleted ? "Profile" : "Index", "User");
        }

        [HttpPost]
        public async Task<IActionResult> Register(User model)
        {
            Register register = new Register(_userRepository);

            if (_login.CheckAuthentication())
            {
                return RedirectToAction("Profile", "User");
            }

            if (!string.IsNullOrEmpty(model.Username) && !string.IsNullOrEmpty(model.Password) && !string.IsNullOrEmpty(model.Email))
            {
                await register.CreateAccount(model.Username, model.Password, model.Email);
            }

            return RedirectToAction("Index", "User");
        }

        public async Task<IActionResult> Logout()
        {
            if (_login.CheckAuthentication())
            {
                await _login.DestroyAuthentication();
            }

            return RedirectToAction("Index", "User");
        }
    }
}
