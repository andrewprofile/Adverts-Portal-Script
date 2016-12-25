using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Laftika.Models;

namespace Laftika.Controllers
{
    public class AdvertController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Advert(int id)
        {
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveAdvert(Advert model)
        {
            return RedirectToAction("Index", "Advert", new { message = ViewBag.AlertMessage });
        }
    }
}
