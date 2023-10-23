using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElmiraFireRecall.Data;
using ElmiraFireRecall.Models;
using Microsoft.AspNetCore.Authorization;

namespace ElmiraFireRecall.Controllers
{
    [Authorize(Policy = "Admin")]
    public class FireGroupsController : Controller
    {
        private readonly FireDBContext _context;

        public FireGroupsController(FireDBContext context)
        {
            _context = context;
        }

        // GET: FireGroups
        public async Task<IActionResult> Index()
        {
              return _context.Groups != null ? 
                          View(await _context.Groups.ToListAsync()) :
                          Problem("Entity set 'FireDBContext.Groups'  is null.");
        }

        // GET: FireGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var fireGroup = await _context.Groups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fireGroup == null)
            {
                return NotFound();
            }

            return View(fireGroup);
        }

        // GET: FireGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FireGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description")] FireGroup fireGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fireGroup);
                await _context.SaveChangesAsync();
                TempData["success"] = "The group was created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(fireGroup);
        }

        // GET: FireGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var fireGroup = await _context.Groups.FindAsync(id);
            if (fireGroup == null)
            {
                return NotFound();
            }
            return View(fireGroup);
        }

        // POST: FireGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description")] FireGroup fireGroup)
        {
            if (id != fireGroup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fireGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FireGroupExists(fireGroup.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["success"] = "Your changes have been saved.";
                return RedirectToAction(nameof(Index));
            }
            return View(fireGroup);
        }

        // GET: FireGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var fireGroup = await _context.Groups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fireGroup == null)
            {
                return NotFound();
            }

            return View(fireGroup);
        }

        // POST: FireGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Groups == null)
            {
                return Problem("Entity set 'FireDBContext.Groups'  is null.");
            }
            var fireGroup = await _context.Groups.FindAsync(id);
            if (fireGroup != null)
            {
                _context.Groups.Remove(fireGroup);
            }
            
            await _context.SaveChangesAsync();
            TempData["success"] = "The group has been deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        private bool FireGroupExists(int id)
        {
          return (_context.Groups?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
