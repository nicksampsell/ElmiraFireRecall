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
using Pagination.EntityFrameworkCore.Extensions;

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
        public async Task<IActionResult> Index(int page = 1, int limit = 250)
        {
            List<MessageHistory> messageHistory = await _context.MessageHistory.Include(u => u.User).Include(m => m.MessageType).OrderByDescending(x => x.Created).Skip((page - 1)).Take(limit).ToListAsync();
            int totalItems = await _context.MessageHistory.Include(u => u.User).Include(m => m.MessageType).CountAsync();
            ViewBag.Limit = limit;
            return View(new Pagination<MessageHistory>(messageHistory, totalItems, page, limit));
        }



        private bool MessageHistoryExists(int id)
        {
          return (_context.MessageHistory?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
