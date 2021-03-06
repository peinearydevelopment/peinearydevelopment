﻿using DataAccess;
using DataAccess.Contracts;
using DataAccess.Contracts.Blog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PeinearyDevelopment.Services;
using PeinearyDevelopment.Utilities;
using System.Net.Http;

namespace PeinearyDevelopment.Config
{
    public static class ContainerConfig
    {
        public static void ConfigureContainer(this IServiceCollection services, IConfigurationRoot configuration, IHostingEnvironment hostingEnvironment)
        {
            if (!hostingEnvironment.IsDevelopment())
            {
                services.Configure<MvcOptions>(options => options.Filters.Add(new RequireHttpsAttribute()));
            }

            services.AddMvc();
            services.AddMemoryCache();

            services.AddDbContextPool<PdDbContext>(options => options.UseSqlServer(configuration["ConnectionStrings:PdDbContextConnectionString"]))
                    .AddSingleton(AutoMapperConfig.Configure())
                    .AddSingleton(configuration)
                    .AddSingleton(new HttpClient())
                    .AddScoped<IFilesService, FilesService>()
                    .AddScoped<IPostsDal, PostsDal>()
                    .AddScoped<ICommentsDal, CommentsDal>()
                    .AddScoped<IRssFeed, RssFeed>()
                    .AddScoped<ISitemap, Sitemap>()
                    .AddScoped<ISiteStatisticsDal, SiteStatisticsDal>();
        }
    }
}
