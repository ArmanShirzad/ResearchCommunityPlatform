using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using ResearchCommunityPlatform.Models;
using ResearchCommunityPlatform.Services.AuthorizationService;

using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using File = ResearchCommunityPlatform.Models.File;

namespace ResearchCommunityPlatform.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly AuthorizationService _authorizationService;
        private readonly IWebHostEnvironment _environment;

        public DashboardController(AppDbContext context, UserManager<User> userManager, AuthorizationService authorizationService, IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _authorizationService = authorizationService;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> MainDash()
        {
            var userID = _userManager.GetUserId(User);
            var userPublications = _context.Publications.Where(p => p.UserId == userID).ToList();
            var authorizePublicationViewModel = new List<AuthorizePublicationViewModel>();

            foreach (var publication in userPublications)
            {
                var canEdit = await _authorizationService.CanEditPublicationAsync(User, publication);
                authorizePublicationViewModel.Add(new AuthorizePublicationViewModel
                {
                    PublicationId = publication.PublicationId,
                    Title = publication.Title,
                    Description = publication.Description,
                    Authors = publication.Authors,
                    DateOfPublish = publication.DateOfPublish,
                    DOI = publication.DOI,
                    PubType = publication.PubType,
                    UploadDate = publication.UploadDate,
                    Files = publication.Files?.Select(f => new FileViewModel
                    {
                        FileId = f.FileID,
                        FileName = f.FileName,
                        FilePath = f.FilePath,
                        Size = f.Size
                    }).ToList(),
                    CanEdit = canEdit
                });
            }

            return View(authorizePublicationViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_CreatePartialViewName"); // Replace "_CreatePartialViewName" with the actual name of your partial view
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePublicationViewModel model, IFormFile[] uploadedFiles)
        {
            if (ModelState.IsValid)
            {
                var userID = _userManager.GetUserId(User);
                var publication = new Publication
                {
                    Title = model.Title,
                    DOI = model.DOI,
                    Description = model.Description,
                    Authors = model.Authors,
                    DateOfPublish = model.DateOfPublish,
                    UploadDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    PubType = model.PubType,
                    UserId = userID,
                    Files = new List<File>()

                };
                if (model.Files != null && model.Files.Count > 0)
                {
                    foreach (var uploadedFile in model.Files)
                    {
                        if (uploadedFile != null && uploadedFile.Length > 0)
                        {
                            var filePath = Path.Combine(_environment.WebRootPath, "uploads", uploadedFile.FileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await uploadedFile.CopyToAsync(stream);
                            }

                            var fileEntity = new File
                            {
                                FileName = uploadedFile.FileName,
                                FilePath = filePath,
                                Size = uploadedFile.Length / (1024f * 1024f), // Size in MB
                                PublicationId = publication.PublicationId // This will be set after the publication is saved
                            };

                            publication.Files.Add(fileEntity); // Add the new File entity to the Files collection of the Publication
                        }
                    }
                }

                // Save the publication to the database
                _context.Publications.Add(publication);
                await _context.SaveChangesAsync();
                // Add the user-publication relationship
                var userId = _userManager.GetUserId(User);
                var userPublication = new PublicationCreator
                {
                    UserID = userId,
                    PublicationId = publication.PublicationId
                };
                _context.PublicationCreators.Add(userPublication);

                return RedirectToAction("MainDash"); // Redirect to the main dashboard or another appropriate action
            }
            foreach (var modelState in ViewData.ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
                }
            }
            return View(model);
        }
    }
}
