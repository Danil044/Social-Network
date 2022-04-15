using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Social_Network.Data;
using Social_Network.Data.social_network;

namespace Social_Network.Controllers.StandartController
{
    public class LikesStandartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LikesStandartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LikesStandart
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Likes.Include(l => l.Post);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: LikesStandart/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var like = await _context.Likes
                .Include(l => l.Post)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (like == null)
            {
                return NotFound();
            }

            return View(like);
        }

        // GET: LikesStandart/Create
        public IActionResult Create()
        {
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id");
            return View();
        }

        // POST: LikesStandart/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PostId,LikeInt,CreateAt")] Like like)
        {
            if (ModelState.IsValid)
            {
                like.Id = Guid.NewGuid();
                _context.Add(like);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id", like.PostId);
            return View(like);
        }

        // GET: LikesStandart/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var like = await _context.Likes.FindAsync(id);
            if (like == null)
            {
                return NotFound();
            }
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id", like.PostId);
            return View(like);
        }

        // POST: LikesStandart/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,PostId,LikeInt,CreateAt")] Like like)
        {
            if (id != like.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(like);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LikeExists(like.Id))
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
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id", like.PostId);
            return View(like);
        }

        // GET: LikesStandart/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var like = await _context.Likes
                .Include(l => l.Post)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (like == null)
            {
                return NotFound();
            }

            return View(like);
        }

        // POST: LikesStandart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var like = await _context.Likes.FindAsync(id);
            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LikeExists(Guid id)
        {
            return _context.Likes.Any(e => e.Id == id);
        }
    }
}
