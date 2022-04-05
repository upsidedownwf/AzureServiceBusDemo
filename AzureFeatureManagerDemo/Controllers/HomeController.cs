using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AzureFeatureManagerDemo.Models;
using Microsoft.FeatureManagement.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace AzureFeatureManagerDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFeatureManager featureManger;
        private readonly IConfiguration configuration;

        public HomeController(ILogger<HomeController> logger, IFeatureManager featureManger, IConfiguration configuration)
        {
            _logger = logger;
            this.featureManger = featureManger;
            this.configuration = configuration;
        }

        [FeatureGate("FeatureReady")]
        public IActionResult Index()
        {
            return View();
        }

        [FeatureGate("Home")]
        public async Task<IActionResult> Privacy()
        {
            if (await featureManger.IsEnabledAsync("Privacy"))
            {

                return View();
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet("{statusCode}")]
        public IActionResult Notfound(int statusCode)
        {
            var statuscoderequest = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (statusCode)
            {
                case 404:
                    break;
            }
            return Content("<p style='color:red'>Sorry, what you are looking for is not found. Thanks</p>", "text/html");
        }
        public IActionResult NoFeature()
        {
            return View();
        }
    }
}
