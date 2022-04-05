using Microsoft.FeatureManagement;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AzureFeatureManagerDemo.Models
{
    [FilterAlias("Browser")]
    public class BrowserFeatureFilter : IFeatureFilter
    {
        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            return Task.FromResult(true);
        }
    }
    public class BrowserFilterSettings
    {
        public string[] Allowed { get; set; }
    }

}
