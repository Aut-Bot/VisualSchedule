using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutWeb.Data;
using AutWeb.Models.AutModels;

namespace AutWeb.Api
{
    [Produces("application/json")]
    [Route("api/Calendar")]
    public class CalendarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CalendarController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Calendar
        [HttpGet]
        public IEnumerable<CalendarItem> GetCalendarItems()
        {
            return _context.CalendarItems;
        }

        // GET: api/Calendar/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCalendarItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var calendarItem = await _context.CalendarItems.SingleOrDefaultAsync(m => m.Id == id);

            if (calendarItem == null)
            {
                return NotFound();
            }

            return Ok(calendarItem);
        }

        // PUT: api/Calendar/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCalendarItem([FromRoute] int id, [FromBody] CalendarItem calendarItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != calendarItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(calendarItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CalendarItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // POST: api/Calendar
        [HttpPost]
        public async Task<IActionResult> PostCalendarItem([FromBody] CalendarItem calendarItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.CalendarItems.Add(calendarItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCalendarItem", new { id = calendarItem.Id }, calendarItem);
        }
        
        // DELETE: api/Calendar/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCalendarItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var calendarItem = await _context.CalendarItems.SingleOrDefaultAsync(m => m.Id == id);
            if (calendarItem == null)
            {
                return NotFound();
            }

            _context.CalendarItems.Remove(calendarItem);
            await _context.SaveChangesAsync();

            return Ok(calendarItem);
        }

        private bool CalendarItemExists(int id)
        {
            return _context.CalendarItems.Any(e => e.Id == id);
        }
    }
}