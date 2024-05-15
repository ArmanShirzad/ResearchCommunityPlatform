using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ResearchCommunityPlatform.Controllers
{
    public class DashboardController : Controller
    {

        [Authorize]
        [HttpGet]
        public IActionResult MainDash()
        {
            return View();
        }
        //[HttpPost]
        //public IActionResult MainDash( UploadPubViewModel)
        //{
        //    return View();
        //}
        [HttpGet]
        public IActionResult Create() 
        {
            return PartialView();
        }

    }
}
