using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Laftika.Models;
using Laftika.DAL;

namespace Laftika.Controllers
{
    public class AdvertController : Controller
    {
        AdvertRepository advertRepository = new AdvertRepository(new DatabaseContext());

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Advert(int id)
        {
            Advert advert = advertRepository.GetAdvertById(id);

            ViewBag.Advert = advert;
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveAdvert(Advert model)
        {
            advertRepository.InsertAdvert(model);
            await advertRepository.Save();

            ViewBag.AlertMessage = "Poprawnie!";

            return RedirectToAction("Index", "Advert", new { message = ViewBag.AlertMessage });
        }
    }
}
