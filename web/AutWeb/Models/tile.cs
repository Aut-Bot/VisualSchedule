using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutWeb.Models
{
    public class Tile
    {
        public Tile(string imgurl, string description)
        {
            Imgurl = imgurl;
            Description = description;

        }
        public string Imgurl { get; }
        public string Description { get; }
    }
}
