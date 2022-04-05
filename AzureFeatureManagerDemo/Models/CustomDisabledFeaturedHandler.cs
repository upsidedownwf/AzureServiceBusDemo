using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.FeatureManagement.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureFeatureManagerDemo.Models
{
    public class CustomDisabledFeaturedHandler : IDisabledFeaturesHandler
    {
        public Task HandleDisabledFeatures(IEnumerable<string> features, ActionExecutingContext context)
        {
            //context.Result = new RedirectToActionResult("NoFeature", "Home", null);
            context.Result = new ContentResult {
                StatusCode = 200,
                Content= "<p style='color:red'>Sorry, This feature is not available now. Thanks</p>",
                ContentType="text/html"
            };
            return Task.CompletedTask;
        }
    }
}
