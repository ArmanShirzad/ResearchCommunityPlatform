using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

using ResearchCommunityPlatform.Models;
using ResearchCommunityPlatform.Services.AuthorizationService;

namespace ResearchCommunityPlatform.Controllers
{
    [Authorize]
    public class PublicationsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<PublicationsController> _logger;

        public PublicationsController(AppDbContext context,
                                      IAuthorizationService authorizationService,
                                      UserManager<User> userManager,
                                      ILogger<PublicationsController> logger)
        {
            _context = context;
            _authorizationService = authorizationService;
            _userManager = userManager;
            _logger = logger;
        }
        [Authorize (Policy = "EditPolicy")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var publication = await _context.Publications
                .Include(p => p.Files)
                .Include(p => p.PublicationCreators)
                .FirstOrDefaultAsync(p => p.PublicationId == id);

            if (publication == null)
            {
                return NotFound();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, publication, AuthorizationConstants.Update);

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
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
                Authors = publication.Authors,
                Files = publication.Files.Select(f => new FileViewModel
                {
                    FileId = f.FileID,
                    FileName = f.FileName,
                    FilePath = f.FilePath,
                    Size = f.Size
                }).ToList()
            };
            return View("edit",model);
        }

        
        [HttpPost]
        [Authorize(Policy = "EditPolicy")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PublicationId,Title,DOI,Description,Authors,DateOfPublish,UploadDate,PubType")] AuthorizePublicationViewModel publication)
        {
            _logger.LogInformation("Edit action started.");
            _logger.LogInformation($"User authenticated: {User.Identity.IsAuthenticated}");
            _logger.LogInformation($"User: {User.Identity.Name}");
            if (id != publication.PublicationId)
            {
                return NotFound();
            }
            var match = await _context.Publications.FirstOrDefaultAsync(p => p.PublicationId == id);

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, match, AuthorizationConstants.Update);

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(publication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PublicationExists(publication.PublicationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("MainDash", nameof(DashboardController));
            }
            return View(publication);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var publication = await _context.Publications.FindAsync(id);
            if (publication == null)
            {
                return NotFound();
            }
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, publication, AuthorizationConstants.Delete);

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            _context.Publications.Remove(publication);
            await _context.SaveChangesAsync();
            return RedirectToAction("MainDash" ,nameof(DashboardController));
        }

        private bool PublicationExists(int id)
        {
            return _context.Publications.Any(e => e.PublicationId == id);
        }
    }

}
