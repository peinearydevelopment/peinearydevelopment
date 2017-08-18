using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace PeinearyDevelopment.Config
{
    public static class ErrorPageConfig
    {
        public static IApplicationBuilder ConfigureErrorPage(this IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            return app;
        }
    }
}
