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


namespace Social_Network.Controllers
{
    public class MainHomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        IWebHostEnvironment _appEnvironment;

        public MainHomeController(ApplicationDbContext context,
               IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> MainHome(string id)
        {
            var autersID = _context.Users.First(a => a.Id == id);

            ViewData["AuthorId"] = autersID.Id;

            var post = _context.Posts
               .Include(c => c.Image)
               .Include(c => c.Author)
               .Include(c => c.Likes)
               .Include(c => c.Comments);
         

            if (post == null)
            {
                return NotFound();
            }

            return View(await post.ToListAsync());
        } 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLike([Bind("Id,PostId, AuthorId, LikeInt,CreateAt")] Like like)
        {
            if (ModelState.IsValid)
            {
                like.Id = Guid.NewGuid();
                _context.Add(like);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MainHome), new { Id = like.AuthorId });
            }
            return RedirectToAction(nameof(MainHome));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComents([Bind("Id,Text,PostId, AuthorId, CreateAt")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.Id = Guid.NewGuid();
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MainHome), new { Id = comment.AuthorId });
            }
            return RedirectToAction(nameof(MainHome));
        }
    }
}
