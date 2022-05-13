using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using MSGraphMVC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MSGraphMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var tenantId = _configuration.GetValue<string>("AzureAd:TenantId");
            var clientSecret = _configuration.GetValue<string>("AzureAd:ClientSecret");
            var clientId = _configuration.GetValue<string>("AzureAd:ClientId");

            var applicationPermissions = new ClientSecretCredential(tenantId, clientId, clientSecret);
            GraphServiceClient graphService = new GraphServiceClient(applicationPermissions);

            var users = graphService.Users.Request()
                .Select(x => x.DisplayName);
            
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
    }
}
