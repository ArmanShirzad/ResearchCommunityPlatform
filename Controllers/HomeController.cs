using Microsoft.AspNetCore.Mvc;

using ResearchCommunityPlatform.Models;

using System.Diagnostics;

namespace ResearchCommunityPlatform.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult RecentUploads(int page = 1)
        {
            //int itemsPerPage = 4; // Set the number of items to display per page
            //var publications = _context.Publications
            //    .Include(p => p.PublicationCreators)
            //        .ThenInclude(pc => pc.User)
            //    .OrderByDescending(p => p.UploadDate) // Sort by upload date
            //    .Skip((page - 1) * itemsPerPage)
            //    .Take(itemsPerPage)
            //    .Select(p => new PublicationViewModel
            //    {
            //        PublicationId = p.PublicationId,
            //        Title = p.Title,
            //        Description = p.Description,
            //        DateOfPublish = p.DateOfPublish,
            //        UploadDate = p.UploadDate,
            //        PubType = p.PubType,
            //        Authors = string.Join(", ", p.PublicationCreators.Select(pc => pc.User.UserName)),
            //        DOI = p.DOI
            //    })
            //    .ToList();

            //var totalItems = _context.Publications.Count();
            //var totalPages = (int)Math.Ceiling((decimal)totalItems / itemsPerPage);

            //var model = new PublicationsViewModel
            //{
            //    Publications = publications,
            //    PagingInfo = new PagingInfo
            //    {
            //        CurrentPage = page,
            //        ItemsPerPage = itemsPerPage,
            //        TotalItems = totalItems,
            //        TotalPages = totalPages
            //    }
            //};

            return View();//model
        }
    }
}