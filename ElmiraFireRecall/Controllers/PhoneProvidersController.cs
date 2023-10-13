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
    public class PhoneProvidersController : Controller
    {
        private readonly FireDBContext _context;

        public PhoneProvidersController(FireDBContext context)
        {
            _context = context;
        }

        // GET: PhoneProviders
        public async Task<IActionResult> Index()
        {
              return _context.PhoneProviders != null ? 
                          View(await _context.PhoneProviders.ToListAsync()) :
                          Problem("Entity set 'FireDBContext.PhoneProviders'  is null.");
        }

        // GET: PhoneProviders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PhoneProviders == null)
            {
                return NotFound();
            }

            var phoneProvider = await _context.PhoneProviders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (phoneProvider == null)
            {
                return NotFound();
            }

            return View(phoneProvider);
        }

        // GET: PhoneProviders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PhoneProviders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,EmailSuffix")] PhoneProvider phoneProvider)
        {
            if (ModelState.IsValid)
            {
                _context.Add(phoneProvider);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(phoneProvider);
        }

        // GET: PhoneProviders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PhoneProviders == null)
            {
                return NotFound();
            }

            var phoneProvider = await _context.PhoneProviders.FindAsync(id);
            if (phoneProvider == null)
            {
                return NotFound();
            }
            return View(phoneProvider);
        }

        // POST: PhoneProviders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,EmailSuffix")] PhoneProvider phoneProvider)
        {
            if (id != phoneProvider.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phoneProvider);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhoneProviderExists(phoneProvider.Id))
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
            return View(phoneProvider);
        }

        // GET: PhoneProviders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PhoneProviders == null)
            {
                return NotFound();
            }

            var phoneProvider = await _context.PhoneProviders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (phoneProvider == null)
            {
                return NotFound();
            }

            return View(phoneProvider);
        }

        // POST: PhoneProviders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PhoneProviders == null)
            {
                return Problem("Entity set 'FireDBContext.PhoneProviders'  is null.");
            }
            var phoneProvider = await _context.PhoneProviders.FindAsync(id);
            if (phoneProvider != null)
            {
                _context.PhoneProviders.Remove(phoneProvider);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhoneProviderExists(int id)
        {
          return (_context.PhoneProviders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
