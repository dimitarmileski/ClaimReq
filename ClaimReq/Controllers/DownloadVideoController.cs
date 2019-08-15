using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ClaimReq.Models;
using System.Diagnostics;

namespace ClaimReq.Controllers
{
    public class DownloadVideoController : Controller
    {
        private static bool success;
        //GET Download
        public IActionResult Index()
        {
            ViewBag.successDownload = false;


            if (success)
            {
                ViewBag.successDownload = true;
            }
            success = false;
            return View();
        }

        //POST Download
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DownloadVideo(string videoUrl, string formatSelected)
        {
            if (videoUrl == null || videoUrl.Equals(""))
            {
                success = false;
                return RedirectToAction(nameof(Index));
            }

            Video.SaveVideoToDisk(videoUrl, formatSelected);
            success = true;

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}