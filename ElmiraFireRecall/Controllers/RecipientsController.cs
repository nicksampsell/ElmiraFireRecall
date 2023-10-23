using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElmiraFireRecall.Data;
using ElmiraFireRecall.Models;
using System.Collections;
using NuGet.Packaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ElmiraFireRecall.Controllers
{
    [Authorize(Policy = "Admin")]
    public class RecipientsController : Controller
    {
        private readonly FireDBContext _context;

        public RecipientsController(FireDBContext context)
        {
            _context = context;
        }

        // GET: Recipients
        public async Task<IActionResult> Index(string searchFor="", string searchBy="")
        {
            var fireDBContext = _context.Recipients.Include(x => x.FireGroups).Include(f => f.PhoneProvider).AsQueryable();

            if(!String.IsNullOrEmpty(searchFor))
            {
                if(searchBy == "name")
                {
                    fireDBContext = fireDBContext.Where(x => x.FirstName.Contains(searchFor) || x.LastName.Contains(searchFor));
                }
                else if(searchBy == "phone")
                {
                    fireDBContext = fireDBContext.Where(x => x.PhoneNumber.Contains(searchFor));
                }
                else if(searchBy == "group")
                {
                    fireDBContext = fireDBContext.Where(x => x.FireGroups.Any(y => y.Title.Contains(searchFor)));
                }
                else
                {
                    fireDBContext = fireDBContext.Where(x => x.FirstName.Contains(searchFor) || x.LastName.Contains(searchFor) || x.PhoneNumber.Contains(searchFor) || x.FireGroups.Any(y => y.Title.Contains(searchFor)));
                }
            }

            ViewBag.SearchFor = searchFor;
            ViewBag.SearchBy = searchBy;
            return View(await fireDBContext.ToListAsync());
        }

        // GET: Recipients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Recipients == null)
            {
                return NotFound();
            }

            var fireRecipient = await _context.Recipients
                .Include(f => f.PhoneProvider)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fireRecipient == null)
            {
                return NotFound();
            }

            return View(fireRecipient);
        }

        // GET: Recipients/Create
        public IActionResult Create()
        {
            ViewData["PhoneProviderId"] = new SelectList(_context.PhoneProviders, "Id", "Name");
            GetGroupMembership(null);
            return View();
        }

        // POST: Recipients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string[] selectedGroups, [Bind("Id,FirstName,LastName,Email,PhoneNumber,PhoneProviderId")] FireRecipient fireRecipient)
        {
            if(selectedGroups != null)
            {
                fireRecipient.FireGroups = new List<FireGroup>();
                var allGroups = _context.Groups.ToList();
                foreach(var group in selectedGroups)
                {
                    fireRecipient.FireGroups.Add(allGroups.FirstOrDefault(x => x.Id == int.Parse(group)));
                }
            }
            
            if (ModelState.IsValid)
            {
                _context.Add(fireRecipient);

                await _context.SaveChangesAsync();
                TempData["success"] = "The recipient has been added.";
                return RedirectToAction(nameof(Index));
            }
            ViewData["PhoneProviderId"] = new SelectList(_context.PhoneProviders, "Id", "Name", fireRecipient.PhoneProviderId);
            GetGroupMembership(null);
            return View(fireRecipient);
        }

        // GET: Recipients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Recipients == null)
            {
                return NotFound();
            }

            var fireRecipient = await _context.Recipients.Include(x => x.FireGroups).Where(x => x.Id == id).FirstOrDefaultAsync();
            if (fireRecipient == null)
            {
                return NotFound();
            }
            ViewData["PhoneProviderId"] = new SelectList(_context.PhoneProviders, "Id", "Name", fireRecipient.PhoneProviderId);
            GetGroupMembership(fireRecipient);
            return View(fireRecipient);
        }

        // POST: Recipients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string[] selectedGroups, [Bind("Id,FirstName,LastName,Email,PhoneNumber,PhoneProviderId")] FireRecipient fireRecipient)
        {

            if (id != fireRecipient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fireRecipient);
                    UpdateGroupMembership(selectedGroups, fireRecipient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FireRecipientExists(fireRecipient.Id))
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
            ViewData["PhoneProviderId"] = new SelectList(_context.PhoneProviders, "Id", "Name", fireRecipient.PhoneProviderId);
            GetGroupMembership(fireRecipient);
            return View(fireRecipient);
        }

        // GET: Recipients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Recipients == null)
            {
                return NotFound();
            }

            var fireRecipient = await _context.Recipients
                .Include(f => f.PhoneProvider)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fireRecipient == null)
            {
                return NotFound();
            }

            return View(fireRecipient);
        }

        // POST: Recipients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Recipients == null)
            {
                return Problem("Entity set 'FireDBContext.Recipients'  is null.");
            }
            var fireRecipient = await _context.Recipients.FindAsync(id);
            if (fireRecipient != null)
            {
                _context.Recipients.Remove(fireRecipient);
            }
            
            await _context.SaveChangesAsync();
            TempData["success"] = "The recipient has been deleted.";
            return RedirectToAction(nameof(Index));
        }

        private bool FireRecipientExists(int id)
        {
          return (_context.Recipients?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        private void GetGroupMembership(FireRecipient? recipient = null)
        {
            var allGroups = _context.Groups;
            var recipientGroups = new HashSet<int>();
            if (recipient != null)
            {
                recipientGroups.AddRange(recipient.FireGroups.Select(x => x.Id));   
            }
            var viewModel = new List<AssignedGroupsDTO>();
            foreach (var group in allGroups)
            {
                viewModel.Add(new AssignedGroupsDTO
                {
                    GroupId = group.Id,
                    Title = group.Title,
                    IsMember = recipientGroups.Contains(group.Id)
                });
            }
            ViewData["Groups"] = viewModel;
        }

        private void UpdateGroupMembership(string[]? selectedGroups, FireRecipient? recipient)
        {
            _context.Entry(recipient).Collection(x => x.FireGroups).Load();

            if(selectedGroups == null)
            {
                recipient.FireGroups = new List<FireGroup>();
                return;
            }

            var allGroups = _context.Groups.ToList();
            var selectedGroupsHS = new HashSet<string>(selectedGroups);
            var currentGroups = new HashSet<int>(recipient.FireGroups.Select(x => x.Id));
            
            foreach(var group in _context.Groups)
            {
                if(selectedGroupsHS.Contains(group.Id.ToString()))
                {
                    if(!currentGroups.Contains(group.Id))
                    {
                        recipient.FireGroups.Add(allGroups.FirstOrDefault(x => x.Id == group.Id));
                    }
                }
                else
                {
                    if(currentGroups.Contains(group.Id))
                    {
                        recipient.FireGroups.Remove(allGroups.FirstOrDefault(x => x.Id == group.Id));
                    }
                }
            }


        }


    }

    class AssignedGroupsDTO
    {
        public int GroupId { get; set; }
        public string Title { get; set; }
        public bool IsMember { get; set; }
    }
}
