using Azure.Data.AppConfiguration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppConfigurationWriteDemo
{
    //
    // Summary:
    //     Represents a configuration setting that stores a feature flag value. Feature
    //     flags allow you to activate or deactivate functionality in your application.
    //     A simple feature flag is either on or off. The application always behaves the
    //     same way. For example, you could roll out a new feature behind a feature flag.
    //     When the feature flag is enabled, all users see the new feature. Disabling the
    //     feature flag hides the new feature. In contrast, a conditional feature flag allows
    //     the feature flag to be enabled or disabled dynamically. The application may behave
    //     differently, depending on the feature flag criteria. Suppose you want to show
    //     your new feature to a small subset of users at first. A conditional feature flag
    //     allows you to enable the feature flag for some users while disabling it for others.
    //     Feature filters determine the state of the feature flag each time it's evaluated.
    //     NOTE: The Azure.Data.AppConfiguration doesn't evaluate feature flags on retrieval.
    //     It's the responsibility of the library consumer to interpret feature flags and
    //     determine whether a feature is enabled.
    public class FeatureFlagConfigurationSettings : FeatureFlagConfigurationSetting
    {
        //
        // Summary:
        //     Initializes an instance of the Azure.Data.AppConfiguration.FeatureFlagConfigurationSetting
        //     using a provided feature id and the enabled value.
        //
        // Parameters:
        //   featureId:
        //     The identified of the feature flag.
        //
        //   isEnabled:
        //     The value indicating whether the feature flag is enabled.
        //
        //   label:
        //     A label used to group this configuration setting with others.
        public FeatureFlagConfigurationSettings(string featureId, bool isEnabled, string label = null):base(featureId, isEnabled, label)
        {
            FeatureId = featureId;
            IsEnabled = isEnabled;
            Label = label;
        }

        //
        // Summary:
        //     The prefix used for Azure.Data.AppConfiguration.FeatureFlagConfigurationSetting
        //     setting keys.
        public new static string KeyPrefix { get; }
        //
        // Summary:
        //     Gets or sets an ID used to uniquely identify and reference the feature
        public new string FeatureId { get; set; }
        //
        // Summary:
        //     Gets or sets a description of the feature.
        public new string Description { get; set; }
        //
        // Summary:
        //     Gets or sets a display name for the feature to use for display rather than the
        //     ID.
        public new string DisplayName { get; set; }
        //
        // Summary:
        //     Gets or sets a value indicating whether the features is enabled. A feature is
        //     OFF if enabled is false. If enabled is true, then the feature is ON if there
        //     are no conditions or if all conditions are satisfied.
        public new bool IsEnabled { get; set; }
        //
        // Summary:
        //     Filters that must run on the client and be evaluated as true for the feature
        //     to be considered enabled.
        public new IList<FeatureFlagFilter> ClientFilters { get; set; }
    }
    class Program
    {
        static async Task Main(string[] args)
        {

            int value;
            var client = new ConfigurationClient("");
            Console.WriteLine("Hello!");
            Console.WriteLine("Enter 1 for app configuration and 2 for feature flags");
            Console.WriteLine("Enter:");
            value = Convert.ToInt32(Console.ReadLine());
            if (value == 1)
            {
                Console.WriteLine("Enter 1 to write the following configuration values to Azure App Configuration");
                Console.WriteLine("{'InspectionBookings':true, 'Inspections':true, 'InspectionPlans':true, 'InspectionTasks':true}");
                Console.WriteLine("Enter:");
                value = Convert.ToInt32(Console.ReadLine());
                if (value == 1)
                {
                    var inspectionBookings = new ConfigurationSetting("InspectionBookings", "true", "SubDemo");
                    var inspections = new ConfigurationSetting("Inspections", "true", "SubDemo");
                    var inspectionPlans = new ConfigurationSetting("InspectionPlans", "true", "SubDemo");
                    var inspectionTasks = new ConfigurationSetting("InspectionTasks", "true", "SubDemo");
                    await client.AddConfigurationSettingAsync(inspectionBookings);
                    await client.AddConfigurationSettingAsync(inspections);
                    await client.AddConfigurationSettingAsync(inspectionPlans);
                    await client.AddConfigurationSettingAsync(inspectionTasks);

                    Console.WriteLine();

                    Console.WriteLine("Enter 2 to update the value of Inspections and InspectionPlans to false"); ;
                    Console.WriteLine("Enter:");
                    ConfigurationSetting inspectionToUpdate = await client.GetConfigurationSettingAsync(inspections.Key, inspections.Label);
                    ConfigurationSetting inspectionPlansToUpdate = await client.GetConfigurationSettingAsync(inspectionPlans.Key, inspectionPlans.Label);
                    inspectionToUpdate.Value = "false";
                    inspectionPlansToUpdate.Value = "false";
                    value = Convert.ToInt32(Console.ReadLine());
                    if (value == 2)
                    {
                        await client.SetConfigurationSettingAsync(inspectionToUpdate);
                        await client.SetConfigurationSettingAsync(inspectionPlansToUpdate);

                        Console.WriteLine();

                        Console.WriteLine("Enter 3 to delete added configuration values");
                        Console.WriteLine("Enter:");
                        value = Convert.ToInt32(Console.ReadLine());
                        if (value == 3)
                        {
                            await client.DeleteConfigurationSettingAsync(inspectionBookings.Key, inspectionBookings.Label);
                            await client.DeleteConfigurationSettingAsync(inspections.Key, inspections.Label);
                            await client.DeleteConfigurationSettingAsync(inspectionPlans.Key, inspectionPlans.Label);
                            await client.DeleteConfigurationSettingAsync(inspectionTasks.Key, inspectionTasks.Label);
                        }
                    }
                }
            }
            else if (value == 2)
            {
                Console.WriteLine("Enter 1 to write the following feature flags to Azure App Configuration feature manager");
                Console.WriteLine("{'Subscriptions':true, 'Inspections':true, 'Registry':true, 'Schools':true}");
                Console.WriteLine("Enter:");
                value = Convert.ToInt32(Console.ReadLine());
                if (value == 1)
                {
                    var featurefilters = new List<FeatureFlagFilter>
                    {
                        new FeatureFlagFilter("BrowserFeatureFilter", new Dictionary<string, object>{{"Allowed", new string[]{"Edg"} } }),
                        new FeatureFlagFilter("PercentageFilter",new Dictionary<string, object>{ { "Value", 55} })
                    };
                    var xxx = new { 
                        id= "Subscriptions",
                        description="",
                        enabled= true,
                        conditions= new
                        {
                            client_filters= new []{
                                new {name="Microsoft.Percentage", parameters= new {Value=55}},
                                new {name="Browser", parameters= new {Value=55}},
                            }
                        }
                    };
                    var stinxxx = xxx.ToString();
                    var strinxxx = JsonConvert.SerializeObject(xxx);
                    var subscriptions = new ConfigurationSetting($"{FeatureFlagConfigurationSetting.KeyPrefix}Subscriptions", strinxxx, "SubDemo") { ContentType= "application/vnd.microsoft.appconfig.ff+json;charset=utf-8" };
                    var inspections = new FeatureFlagConfigurationSetting("Inspections", true, "SubDemo");
                    var registry = new FeatureFlagConfigurationSetting("Registry", true, "SubDemo");
                    var schools = new FeatureFlagConfigurationSetting("Schools", true, "SubDemo");
                    await client.AddConfigurationSettingAsync(subscriptions);
                    await client.AddConfigurationSettingAsync(inspections);
                    await client.AddConfigurationSettingAsync(registry);
                    await client.AddConfigurationSettingAsync(schools);

                    Console.WriteLine();

                    Console.WriteLine("Enter 2 to update the value of Inspections and Registry to false"); ;
                    Console.WriteLine("Enter:");
                    inspections.IsEnabled = false;
                    registry.IsEnabled = false;
                    value = Convert.ToInt32(Console.ReadLine());
                    if (value == 2)
                    {
                        await client.SetConfigurationSettingAsync(inspections);
                        await client.SetConfigurationSettingAsync(registry);

                        Console.WriteLine();

                        Console.WriteLine("Enter 3 to delete added configuration values");
                        Console.WriteLine("Enter:");
                        value = Convert.ToInt32(Console.ReadLine());
                        if (value == 3)
                        {
                            await client.DeleteConfigurationSettingAsync(subscriptions);
                            await client.DeleteConfigurationSettingAsync(inspections);
                            await client.DeleteConfigurationSettingAsync(registry);
                            await client.DeleteConfigurationSettingAsync(schools);
                        }
                    }
                }
            }
        }
    }
}
