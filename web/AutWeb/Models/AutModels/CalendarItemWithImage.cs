using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutWeb.Models.AutModels
{
    public class CalendarItemWithImage
    {
        public DateTime TimeSlot { get; set; }
        public string Description { get; set; }
        public byte[] ItemImage { get; set; }
        public string ImageUrl { get; set; }
    }
}
