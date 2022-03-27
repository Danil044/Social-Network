using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Social_Network.Data;
using Social_Network.Data.social_network;

namespace Social_Network.Controllers.StandartController
{
    public class ImagesStandartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ImagesStandartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ImagesStandart
        public async Task<IActionResult> Index()
        {
            return View(await _context.Images.ToListAsync());
        }

        // GET: ImagesStandart/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var images = await _context.Images
                .FirstOrDefaultAsync(m => m.Id == id);
            if (images == null)
            {
                return NotFound();
            }

            return View(images);
        }

        // GET: ImagesStandart/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ImagesStandart/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Post, CreateAt")] Images images, IFormFile fileToPost)
        {
            if (ModelState.IsValid)
            {
                images.Id = Guid.NewGuid();
                images.URL = await Helpers.Media.UploadImage(fileToPost, "img_post");
                _context.Add(images);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Post"] = new SelectList(_context.Posts, "Id", "CreateAt", images.Post);
            return View(images);
        }

        // GET: ImagesStandart/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var images = await _context.Images.FindAsync(id);
            if (images == null)
            {
                return NotFound();
            }
            return View(images);
        }

        // POST: ImagesStandart/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,URL,CreateAt")] Images images)
        {
            if (id != images.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(images);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImagesExists(images.Id))
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
            return View(images);
        }

        // GET: ImagesStandart/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var images = await _context.Images
                .FirstOrDefaultAsync(m => m.Id == id);
            if (images == null)
            {
                return NotFound();
            }

            return View(images);
        }

        // POST: ImagesStandart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var images = await _context.Images.FindAsync(id);
            _context.Images.Remove(images);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImagesExists(Guid id)
        {
            return _context.Images.Any(e => e.Id == id);
        }
    }
}
