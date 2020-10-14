using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppIntel_Portal_REST.Models
{
    public class ExecViewModel
    {
        //public string WebAddress = "http://localhost:8080/exec/Index.htm";
        public string WebAddress = Program.GetExecReportAddress();
    }
}
