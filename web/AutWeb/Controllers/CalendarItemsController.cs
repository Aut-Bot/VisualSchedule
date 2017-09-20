using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutWeb.Data;
using AutWeb.Models.AutModels;

namespace AutWeb.Controllers
{
    public class CalendarItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CalendarItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CalendarItems
        public async Task<IActionResult> Index()
        {
            return View(await _context.CalendarItems.ToListAsync());
        }

        // GET: CalendarItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calendarItem = await _context.CalendarItems
                .SingleOrDefaultAsync(m => m.Id == id);
            if (calendarItem == null)
            {
                return NotFound();
            }

            return View(calendarItem);
        }

        // GET: CalendarItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CalendarItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TimeSlot,Description,ImageUrl")] CalendarItem calendarItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(calendarItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(calendarItem);
        }

        // GET: CalendarItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calendarItem = await _context.CalendarItems.SingleOrDefaultAsync(m => m.Id == id);
            if (calendarItem == null)
            {
                return NotFound();
            }
            return View(calendarItem);
        }

        // POST: CalendarItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TimeSlot,Description,ImageUrl")] CalendarItem calendarItem)
        {
            if (id != calendarItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(calendarItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CalendarItemExists(calendarItem.Id))
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
            return View(calendarItem);
        }

        // GET: CalendarItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calendarItem = await _context.CalendarItems
                .SingleOrDefaultAsync(m => m.Id == id);
            if (calendarItem == null)
            {
                return NotFound();
            }

            return View(calendarItem);
        }

        // POST: CalendarItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var calendarItem = await _context.CalendarItems.SingleOrDefaultAsync(m => m.Id == id);
            _context.CalendarItems.Remove(calendarItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CalendarItemExists(int id)
        {
            return _context.CalendarItems.Any(e => e.Id == id);
        }
    }
}
