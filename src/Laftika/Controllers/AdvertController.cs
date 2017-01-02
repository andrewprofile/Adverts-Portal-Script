using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Laftika.Models;
using Laftika.DAL;

namespace Laftika.Controllers
{
    public class AdvertController : Controller
    {
        private readonly IGenericRepository<Advert> _advertRepository;

        public AdvertController(IGenericRepository<Advert> advertRepository)
        {
            _advertRepository = advertRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Advert(int id)
        {
            ViewBag.Advert = _advertRepository.GetById(id);

            return View();
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveAdvert(Advert model)
        {
            _advertRepository.Insert(model);
            await _advertRepository.Save();

            ViewBag.AlertMessage = "Poprawnie!";

            return RedirectToAction("Index", "Advert", new { message = ViewBag.AlertMessage });
        }
    }
}
