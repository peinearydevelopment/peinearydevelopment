﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PeinearyDevelopment.Config;

namespace PeinearyDevelopment
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        public Startup(IHostingEnvironment env)
        {
            Configuration = env.ConfigureConfigurationRoot();
            HostingEnvironment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureContainer(Configuration, HostingEnvironment);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.Configure(Configuration);

            app.UseStaticFiles()
               .ConfigureErrorPage(env)
               .UseMvc(routes => routes.MapRoute(name: "default", template: "{controller=Blog}/{action=Index}/{id?}"));
        }
    }
}
