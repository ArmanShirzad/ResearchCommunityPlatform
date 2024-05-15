using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

using ResearchCommunityPlatform.Models;

namespace ResearchCommunityPlatform.Controllers
{
    [Authorize]
    public class PublicationsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<User> _userManager;

        public PublicationsController(AppDbContext context,
                                      IAuthorizationService authorizationService,
                                      UserManager<User> userManager)
        {
            _context = context;
            _authorizationService = authorizationService;
            _userManager = userManager;
        }

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

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, publication, Constants.Update);

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            return View(publication);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PublicationId,Title,DOI,Description,AuthorsSerialized,DateOfPublish,UploadDate,PubType")] Publication publication)
        {
            if (id != publication.PublicationId)
            {
                return NotFound();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, publication, Constants.Update);

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
                return RedirectToAction(nameof(Index));
            }
            return View(publication);
        }

        private bool PublicationExists(int id)
        {
            return _context.Publications.Any(e => e.PublicationId == id);
        }
    }

}
