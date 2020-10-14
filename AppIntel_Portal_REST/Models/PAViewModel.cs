using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppIntel_Portal_REST.Models
{
    public class PAViewModel
    {
        //public string WebAddress = "http://localhost:8080/PA/Index.htm";
        public string WebAddress = Program.GetPAReportAddress();
    }
}
