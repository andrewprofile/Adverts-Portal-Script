using System;
using System.Collections.Generic;
using System.Linq;
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
        UserRepository userRepository = new UserRepository(new DatabaseContext());

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
            var db = new DatabaseContext();
            Login login = new Login(HttpContext);

            if (login.CheckAuthentication())
            {
                return RedirectToAction("Profile", "User");
            }
            else
            {
                if (!String.IsNullOrEmpty(model.Username) && !String.IsNullOrEmpty(model.Password))
                {
                    if (login.CreateAuthentication(model.Username, model.Password))
                    {
                        return RedirectToAction("Profile", "User");
                    }
                }
            }

            return RedirectToAction("Index", "User");
        }

        [HttpPost]
        public async Task<IActionResult> Register(User model)
        {
            Login login = new Login(HttpContext);
            Register register = new Register();

            if (login.CheckAuthentication())
            {
                return RedirectToAction("Profile", "User");
            }
            else
            {
                if (!string.IsNullOrEmpty(model.Username) && !string.IsNullOrEmpty(model.Password) && !string.IsNullOrEmpty(model.Email))
                {
                    bool result = await register.CreateAccount(model.Username, model.Password, model.Email);
                }
            }

            return RedirectToAction("Index", "User");
        }

        public IActionResult Logout()
        {
            Login login = new Login(HttpContext);

            if (login.CheckAuthentication())
            {
                login.DestroyAuthentication();
            }

            return RedirectToAction("Index", "User");
        }
    }
}
