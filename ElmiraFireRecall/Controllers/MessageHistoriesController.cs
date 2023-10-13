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
    public class MessageHistoriesController : Controller
    {
        private readonly FireDBContext _context;

        public MessageHistoriesController(FireDBContext context)
        {
            _context = context;
        }

        // GET: MessageHistories
        public async Task<IActionResult> Index()
        {
            var fireDBContext = _context.MessageHistory.Include(m => m.MessageType);
            return View(await fireDBContext.ToListAsync());
        }

        // GET: MessageHistories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MessageHistory == null)
            {
                return NotFound();
            }

            var messageHistory = await _context.MessageHistory
                .Include(m => m.MessageType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (messageHistory == null)
            {
                return NotFound();
            }

            return View(messageHistory);
        }

        // GET: MessageHistories/Create
        public IActionResult Create()
        {
            ViewData["MessageTypeId"] = new SelectList(_context.MessageTypes, "Id", "Id");
            return View();
        }

        // POST: MessageHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MessageTypeId,Subject,Message")] MessageHistory messageHistory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(messageHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MessageTypeId"] = new SelectList(_context.MessageTypes, "Id", "Id", messageHistory.MessageTypeId);
            return View(messageHistory);
        }

        // GET: MessageHistories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MessageHistory == null)
            {
                return NotFound();
            }

            var messageHistory = await _context.MessageHistory.FindAsync(id);
            if (messageHistory == null)
            {
                return NotFound();
            }
            ViewData["MessageTypeId"] = new SelectList(_context.MessageTypes, "Id", "Id", messageHistory.MessageTypeId);
            return View(messageHistory);
        }

        // POST: MessageHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MessageTypeId,Subject,Message")] MessageHistory messageHistory)
        {
            if (id != messageHistory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(messageHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageHistoryExists(messageHistory.Id))
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
            ViewData["MessageTypeId"] = new SelectList(_context.MessageTypes, "Id", "Id", messageHistory.MessageTypeId);
            return View(messageHistory);
        }

        // GET: MessageHistories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MessageHistory == null)
            {
                return NotFound();
            }

            var messageHistory = await _context.MessageHistory
                .Include(m => m.MessageType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (messageHistory == null)
            {
                return NotFound();
            }

            return View(messageHistory);
        }

        // POST: MessageHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MessageHistory == null)
            {
                return Problem("Entity set 'FireDBContext.MessageHistory'  is null.");
            }
            var messageHistory = await _context.MessageHistory.FindAsync(id);
            if (messageHistory != null)
            {
                _context.MessageHistory.Remove(messageHistory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MessageHistoryExists(int id)
        {
          return (_context.MessageHistory?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
