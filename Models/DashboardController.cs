using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ResearchCommunityPlatform.Models
{
    public class DashboardController : Controller
    {

        [Authorize]
        public IActionResult MainDash()
        {
            return View();
        }
    }
}
