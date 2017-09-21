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
    [Route("api/CalendarBot")]
    public class CalendarBotController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CalendarBotController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Calendar
        [HttpPost]
        public async Task<IActionResult> PostCalendarItemWithImage(
            [FromBody] CalendarItemWithImage calendarItemWithImage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Is image included?
            bool imageIncluded = (calendarItemWithImage.ItemImage != null);
            
            if (imageIncluded)
            {
                // store in blob, store URI for DB record
                calendarItemWithImage.Description = "[IMG]" + calendarItemWithImage.Description;
            } else
            {
                // no image in request
                calendarItemWithImage.Description = "[NO-IMG]" + calendarItemWithImage.Description;
                // get image via Bing image search
                // store Bing image URI for DB record
            }

            // create calendarItem
            var calendarItem = new CalendarItem();
            calendarItem.Description = calendarItemWithImage.Description;
            calendarItem.ImageUrl = calendarItemWithImage.ImageUrl;
            calendarItem.TimeSlot = calendarItemWithImage.TimeSlot;

            // insert DB calendar item (calendarItem.Id, TimeSlot, ImageUrl, Description)
            _context.CalendarItems.Add(calendarItem);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetCalendarItem", new { id = calendarItem.Id }, calendarItem);
            return Ok(calendarItem);
        }

    }
}