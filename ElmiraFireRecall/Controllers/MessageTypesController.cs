using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElmiraFireRecall.Data;
using ElmiraFireRecall.Models;

namespace ElmiraFireRecall.Controllers
{
    public class MessageTypesController : Controller
    {
        private readonly FireDBContext _context;

        public MessageTypesController(FireDBContext context)
        {
            _context = context;
        }

        // GET: MessageTypes
        public async Task<IActionResult> Index()
        {
              return _context.MessageTypes != null ? 
                          View(await _context.MessageTypes.ToListAsync()) :
                          Problem("Entity set 'FireDBContext.MessageTypes'  is null.");
        }

        // GET: MessageTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MessageTypes == null)
            {
                return NotFound();
            }

            var messageType = await _context.MessageTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (messageType == null)
            {
                return NotFound();
            }

            return View(messageType);
        }

        // GET: MessageTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MessageTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] MessageType messageType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(messageType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(messageType);
        }

        // GET: MessageTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MessageTypes == null)
            {
                return NotFound();
            }

            var messageType = await _context.MessageTypes.FindAsync(id);
            if (messageType == null)
            {
                return NotFound();
            }
            return View(messageType);
        }

        // POST: MessageTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] MessageType messageType)
        {
            if (id != messageType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(messageType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageTypeExists(messageType.Id))
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
            return View(messageType);
        }

        // GET: MessageTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MessageTypes == null)
            {
                return NotFound();
            }

            var messageType = await _context.MessageTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (messageType == null)
            {
                return NotFound();
            }

            return View(messageType);
        }

        // POST: MessageTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MessageTypes == null)
            {
                return Problem("Entity set 'FireDBContext.MessageTypes'  is null.");
            }
            var messageType = await _context.MessageTypes.FindAsync(id);
            if (messageType != null)
            {
                _context.MessageTypes.Remove(messageType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MessageTypeExists(int id)
        {
          return (_context.MessageTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
