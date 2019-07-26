using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClaimReq.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClaimReq.Controllers
{
    public class HomeController : Controller
    {
        private readonly ClaimsRepo _claimsRepo;

        public HomeController(ClaimsRepo claimsRepo)
        {
            _claimsRepo = claimsRepo;
        }

        public ActionResult Index()
        {
            var list = _claimsRepo.GetAll();
            return View(list);
        }

        public ActionResult Add()
        {
            return View("Edit", new KeyValuePair<string, Claim>("", new Claim()));
        }

        [HttpPost]
        public ActionResult Save(string key, Claim value)
        {
            _claimsRepo.Save(new KeyValuePair<string, Claim>(key, value));
            return RedirectToAction("Index");
        }

        public ActionResult Edit(string id)
        {
            var profile = _claimsRepo.GetProfileByKey(id);
            return View("Edit", profile);
        }

        public ActionResult Delete(string id)
        {
            _claimsRepo.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
