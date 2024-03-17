using Microsoft.AspNetCore.Mvc;

using ResearchCommunityPlatform.Models;

using System.Diagnostics;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace ResearchCommunityPlatform.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1)
        {

            int pageSize = 3; // Set the number of records per page
            int skipAmount = pageSize * (page - 1);

            var totalRecords = await _context.Publications.CountAsync();
            var publications = await _context.Publications
                .OrderByDescending(p => p.UploadDate)
                .Skip(skipAmount)
                .Take(pageSize)
                .Select(p => new PublicationViewModel
                {
                    PublicationId = p.PublicationId,
                    Title = p.Title,
                    Description = p.Description,
                    DateOfPublish = p.DateOfPublish,
                    UploadDate = p.UploadDate,
                    PubType = p.PubType,
                    DOI = p.DOI,
                    Authors = JsonConvert.DeserializeObject<List<string>>(p.AuthorsSerialized) // Make sure to include Newtonsoft.Json
                })
                .ToListAsync();

            var model = new PublicationsViewModel
            {
                Publications = publications,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = totalRecords
                }
            };


            return View(model);
        }
        [HttpGet]
        [HttpGet]
        public async Task<IActionResult> LoadMorePublications(int lastLoadedId)
        {
            int pageSize = 50; // Adjust the page size if you like

            var publications = await _context.Publications
                .Where(p => p.PublicationId > lastLoadedId) // This ensures we start after the last loaded ID
                .OrderBy(p => p.PublicationId) // Ensure records are ordered by ID
                .Take(pageSize) // Take the next set of records
                .Select(p => new PublicationViewModel
                {
            // Property mappings
                    PublicationId = p.PublicationId,
                    Title = p.Title,
                    Description = p.Description,
                    DateOfPublish = p.DateOfPublish,
                    UploadDate = p.UploadDate,
                    PubType = p.PubType,
                    DOI = p.DOI,
                    Authors = JsonConvert.DeserializeObject<List<string>>(p.AuthorsSerialized)
                })
                .ToListAsync();

            if (!publications.Any())
            {
                return Content(""); // Returns an empty string if no more records
            }

            return PartialView("_PublicationPartial", publications);
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




        public async Task<IActionResult> GetPublicationById(int id)
        {

            var publication = await _context.Publications
                             .Include(p => p.Files) // If you have files associated with the publication
                             .Include(p => p.PublicationCreators) // If you want to show creators' details
                             .FirstOrDefaultAsync(x => x.PublicationId == id);

            if (publication == null)
            {
                return NotFound();
            }

            var model = new PublicationViewModel
            {
                PublicationId = publication.PublicationId,
                Title = publication.Title,
                Description = publication.Description,
                DateOfPublish = publication.DateOfPublish,
                UploadDate = publication.UploadDate,
                PubType = publication.PubType,
                DOI = publication.DOI,
                Authors = JsonConvert.DeserializeObject<List<string>>(publication.AuthorsSerialized), // Deserialize the authors
                  Files = publication.Files.Select(f => new FileViewModel
                  {
                      FileId = f.FileID,
                      FileName = f.FileName,
                      FilePath = f.FilePath,
                      Size = f.Size // Make sure the File entity has a Size property
                  }).ToList()
            };

            return View("Publication", model);
        }
    }
}   