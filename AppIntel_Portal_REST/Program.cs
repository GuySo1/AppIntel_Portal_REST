using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AppIntel_Portal_REST
{
    public class Program
    {        
        static string serverURL = Environment.GetEnvironmentVariable("serverURL");

        

        static string serverPort = Environment.GetEnvironmentVariable("serverPort");
        const string RESTAPIHead = "/api";
        static string workspace = Environment.GetEnvironmentVariable("workspace"); // "MicroFocusDemo";
        static string workspaceURL = serverURL + ":" + serverPort + RESTAPIHead + "/workspaces/" + workspace;        
        static HttpClient client = new HttpClient();

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        public static string GetWorkspaceURL()
        {      
            return workspaceURL;
        }

        static public async Task<string> GetEAWebAddress()
        {
            // REST call to: http://mfeadns4mrtj5sd5o6hq.westeurope.cloudapp.azure.com:1248/api/workspaces/MicroFocusDemo/EAWeb
            // returns the URL for the web client (EAWeb)
            var content = await client.GetStringAsync(GetWorkspaceURL() + "/EAWeb");
            return JObject.Parse(content).SelectToken("url").Value<string>();
        }

        static public string GetExecReportAddress()
        {
            // http://mfeadns4mrtj5sd5o6hq.westeurope.cloudapp.azure.com:8080/exec/Index.htm is where I placed the executive report 
            return serverURL + ":8080/exec/Index.htm";
        }

        internal static string GetPAReportAddress()
        {
            // http://mfeadns4mrtj5sd5o6hq.westeurope.cloudapp.azure.com:8080/PA/index.html is where I placed the migration report 
            return serverURL + ":8080/PA/index.html";
        }

        static public string GetRESTAPIAddress()
        {
            // http://mfeadns4mrtj5sd5o6hq.westeurope.cloudapp.azure.com:1248/api
            return serverURL + ":" + serverPort + RESTAPIHead;
        }
    }
}
