using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazine_Store.Models
{
    public class Magazines
    {
        public List<Magazine> data { get; set; }
        public bool success { get; set; }
        public string token { get; set; }
    }

    public class Magazine
    {
        public int id { get; set; }
        public string name { get; set; }
        public string category { get; set; }

    }
}
