using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Social_Network.Data;
using Social_Network.Data.social_network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Social_Network.Controllers.trello
{
    public class TrelloController : Controller
    {
        private readonly ApplicationDbContext _context;
        IWebHostEnvironment _appEnvironment;

        public TrelloController(ApplicationDbContext context,
               IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Index(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autersID = _context.Users.First(a => a.Id == id);

            ViewData["AuthorId"] = autersID.Id;

            var post = _context.Posts
                .Include(c => c.Image)
                .Where(c => c.AuthorId == id);
       
            if (post == null)
            {
                return NotFound();
            }

            return View(await post.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost([Bind("Id, AuthorId, Text, CreateAt")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.Id = Guid.NewGuid();
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { Id = post.AuthorId });
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateImage([Bind("Id, PostId, CreateAt")] Images images, IFormFile fileToPost, string AuthorId)
        {
            if (ModelState.IsValid)
            {
                images.Id = Guid.NewGuid();
                images.URL = await Helpers.Media.UploadImage(fileToPost, "img_post");
                _context.Add(images);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { Id = AuthorId });
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
