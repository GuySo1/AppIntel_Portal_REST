using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AppIntel_Portal_REST.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace AppIntel_Portal_REST.Controllers
{
    
    public class HomeController : Controller
    {
        /// <summary>
        /// For Sending the REST requests
        /// </summary>
        static HttpClient client = new HttpClient() { Timeout = System.Threading.Timeout.InfiniteTimeSpan };
        
        /// <summary>
        /// Full URL to the workspace REST API endpoint
        /// </summary>
        string workspaceURL = Program.GetWorkspaceURL();

        /// <summary>
        /// For getting the environment variables that points to the right server and workspace
        /// Using the environment variables, different deploymants can point to different EA servers and workspaces
        /// </summary>
        private IHostingEnvironment Environment;

        public HomeController(IHostingEnvironment _environment)
        {
            Environment = _environment;
        }

        public IActionResult Index()
        {            
            ViewData["EAWebAddr"] = Program.GetEAWebAddress().Result;

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region Inventory Report
        public async Task<ActionResult<string>> inventory()
        {
            // REST call to: http://mfeadns4mrtj5sd5o6hq.westeurope.cloudapp.azure.com:1248/api/workspaces/MicroFocusDemo/Inventory?format=html
            var content = await client.GetStringAsync(workspaceURL + "/Inventory?format=html");

            // Update cache
            UpdateLatest(content, "Inventory.html");

            //Return view
            return View("HTMLReport", new ReportViewModel() { ReportHTML = content });
        }
        public async Task<ActionResult<string>> inventory_Download()
        {
            // REST call to: http://mfeadns4mrtj5sd5o6hq.westeurope.cloudapp.azure.com:1248/api/workspaces/MicroFocusDemo/Inventory?format=csv
            var content = await client.GetByteArrayAsync(workspaceURL + "/Inventory?format=csv");

            // return file (will be downloaded by the browser)
            return File(content, System.Net.Mime.MediaTypeNames.Application.Octet, "Inventory.csv");
        }

        public IActionResult inventory_latest()
        {
            // View latest Inventory Report
            return View("ComplexReport", new ExecViewModel() { WebAddress = "/Latest/Inventory.html" });
        }
        #endregion

        #region CRUD Report
        public async Task<ActionResult<string>> CRUD()
        {
            // REST call to: http://mfeadns4mrtj5sd5o6hq.westeurope.cloudapp.azure.com:1248/api/workspaces/MicroFocusDemo/Reports/CRUD?format=html
            var content = await client.GetStringAsync(workspaceURL + "/Reports/CRUD?format=html");

            // Update cache
            UpdateLatest(content, "CRUD.html");

            //Return view
            return View("HTMLReport", new ReportViewModel() { ReportHTML = content });
        }

        public async Task<ActionResult<string>> CRUD_Download()
        {
            // REST call to: http://mfeadns4mrtj5sd5o6hq.westeurope.cloudapp.azure.com:1248/api/workspaces/MicroFocusDemo/Reports/CRUD?format=csv
            var content = await client.GetByteArrayAsync(workspaceURL + "/Reports/CRUD?format=csv");

            // return file (will be downloaded by the browser)
            return File(content, System.Net.Mime.MediaTypeNames.Application.Octet, "CRUD.csv");
        }

        public IActionResult CRUD_latest()
        {
            // View latest CRUD Report
            return View("ComplexReport", new ExecViewModel() { WebAddress = "/Latest/CRUD.html" });
        }
        #endregion

        #region Cross-reference Report
        public async Task<ActionResult<string>> XREF()
        {
            // REST call to: http://mfeadns4mrtj5sd5o6hq.westeurope.cloudapp.azure.com:1248/api/workspaces/MicroFocusDemo/Reports/CrossRef?format=html
            var content = await client.GetStringAsync(workspaceURL + "/Reports/CrossRef?format=html");

            // Update cache
            UpdateLatest(content, "XREF.html");

            //Return view
            return View("HTMLReport", new ReportViewModel() { ReportHTML = content });
        }

        public async Task<ActionResult<string>> XREF_Download()
        {
            // REST call to: http://mfeadns4mrtj5sd5o6hq.westeurope.cloudapp.azure.com:1248/api/workspaces/MicroFocusDemo/Reports/CrossRef?format=csv
            var content = await client.GetByteArrayAsync(workspaceURL + "/Reports/CrossRef?format=csv");

            // return file (will be downloaded by the browser)
            return File(content, System.Net.Mime.MediaTypeNames.Application.Octet, "CrossRef.csv");
        }

        public IActionResult XREF_latest()
        {
            // View latest Cross-reference Report
            return View("ComplexReport", new ExecViewModel() { WebAddress = "/Latest/XREF.html" });
        }
        #endregion

        #region Executive Report
        public IActionResult execReport()
        {
            // View the report that was deployed on the EA server
            return View("ComplexReport", new ExecViewModel());
        }

        public async Task<ActionResult<string>> exec_Download()
        {
            // REST call to: http://mfeadns4mrtj5sd5o6hq.westeurope.cloudapp.azure.com:1248/api/workspaces/MicroFocusDemo/Reports/Executive
            var content = await client.GetByteArrayAsync(workspaceURL + "/Reports/Executive");

            // return file (will be downloaded by the browser). Executive report contains files and folders so is packaged in a zip file
            return File(content, System.Net.Mime.MediaTypeNames.Application.Octet, "Executive Report.zip");
        }
        #endregion

        #region Migration Report
        public IActionResult PAReport()
        {
            // View the report that was deployed on the EA server
            return View("ComplexReport2", new PAViewModel());
        }

        public async Task<ActionResult<string>> PA_Download()
        {
            // REST call to: http://mfeadns4mrtj5sd5o6hq.westeurope.cloudapp.azure.com:1248/api/workspaces/MicroFocusDemo/CodeSearchReports/Portability%20Assessment
            var content = await client.GetByteArrayAsync(workspaceURL + "/CodeSearchReports/Portability%20Assessment?format=html");

            // return file (will be downloaded by the browser). Executive report contains files and folders so is packaged in a zip file
            return File(content, System.Net.Mime.MediaTypeNames.Application.Octet, "Migration Report.zip");
        }
        #endregion  

        /// <summary>
        /// Downloads an RDP file for connection to the EA machine and the full Windows client
        /// Currently hard coded but can be dynamically adjusted to point to the right machine
        /// </summary>
        /// <returns>The RDP file</returns>
        public IActionResult DownloadRDP()
        {
            return File("~/AnalystAccess2.rdp", System.Net.Mime.MediaTypeNames.Application.Octet, "AnalystAccess2.rdp");
        }

        /// <summary>
        /// Switch to the web client (EAWeb) embedded view
        /// </summary>
        /// <returns>EA Web view</returns>
        public IActionResult EAWeb_Embedded()
        {
            return View("EAWeb", new EAWebModel());
        }

        /// <summary>
        /// A simple caching of the reports.
        /// Every report that is generated is copied to the wwwroot/Latest folder and can be accesses using the Open Latest buttons
        /// </summary>
        /// <param name="HTMLcontent">The report HTML content</param>
        /// <param name="fileName">The file name that will be written in the Latest folder</param>
        /// <returns></returns>
        private bool UpdateLatest(string HTMLcontent, string fileName)
        {
            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;

            string path = Path.Combine(this.Environment.WebRootPath, "Latest");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            System.IO.File.WriteAllText(Path.Combine(path, fileName), HTMLcontent);

            return true;
        }

    }
}
