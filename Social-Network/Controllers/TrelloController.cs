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

        public async Task<IActionResult> Index()
        {
            return View(await _context.Images.ToListAsync());
        }

        public async Task<IActionResult> ShowColumns(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Postse = _context.Posts.First(b => b.Id == id);

            ViewData["Title"] = Postse.CreateAt;
            ViewData["ParallaxTitle"] = Postse.Text;
            ViewData["ParallaxText"] = "Informatiom e.t.c";

            ViewData["Post"] = Postse.Id;

            var img = _context.Images
                      .First(c => c.PostId == id);

            if (img == null)
            {
                return NotFound();
            }

            return View(await img.ToListAsync());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost([Bind("Id,Text,CreateAt")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.Id = Guid.NewGuid();
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ShowColumns), new { Id = post.Id });
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateImage([Bind("Id, Post, CreateAt")] Images images, IFormFile fileToPost)
        {
            if (ModelState.IsValid)
            {
                images.Id = Guid.NewGuid();
                images.URL = await Helpers.Media.UploadImage(fileToPost, "img_post");
                _context.Add(images);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ShowColumns), new { Id = images.Post });
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
