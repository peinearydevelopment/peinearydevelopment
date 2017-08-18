using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace PeinearyDevelopment.Config
{
    public static class ConfigurationConfig
    {
        public static IConfigurationRoot ConfigureConfigurationRoot(this IHostingEnvironment env)
        {
            return new ConfigurationBuilder()
                                .SetBasePath(env.ContentRootPath)
                                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                                .AddEnvironmentVariables()
                                .Build();
        }
    }
}
