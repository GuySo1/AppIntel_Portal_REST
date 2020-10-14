using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

using System.Threading.Tasks;

namespace AppIntel_Portal_REST.Models
{
    
    public class EAWebModel
    {      
        public string EAWebAddress = Program.GetEAWebAddress().Result;
        //public string EAWebAddress = "https://localhost";        
    }
}
