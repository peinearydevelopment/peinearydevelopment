using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using PeinearyDevelopment.Services;
using System;
using System.Threading.Tasks;

namespace PeinearyDevelopment.Middleware
{
    public class ImagesMiddleware
    {
        private const string ImagesUrlPart = "/images/";
        private readonly RequestDelegate _next;

        public ImagesMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context, IFilesService filesService, ILoggerFactory loggerFactory)
        {
            var requestUrl = GetRequestUrl(context);
            if (requestUrl.Contains(ImagesUrlPart))
            {
                var fileReferenceId = GetFileName(requestUrl);
                var fileContents = await filesService.GetFileContents(fileReferenceId).ConfigureAwait(false);
                var contentType = GetContentType(fileReferenceId);
                context.Response.Headers.Append("Cache-Control", "max-age=31536000");
                await new FileContentResultExecutor(loggerFactory).ExecuteAsync(new ActionContext(context, new RouteData(), new ActionDescriptor()), new FileContentResult(fileContents, contentType)).ConfigureAwait(false);
            }
            else
            {
                await _next(context).ConfigureAwait(false);
            }
        }

        private string GetRequestUrl(HttpContext context) => string.Concat(context.Request.Scheme, "://", context.Request.Host.ToUriComponent(), context.Request.PathBase.ToUriComponent(), context.Request.Path.ToUriComponent(), context.Request.QueryString.ToUriComponent());

        private string GetFileName(string url)
        {
            var startIndex = url.IndexOf(ImagesUrlPart) + ImagesUrlPart.Length;
            return url.Substring(startIndex);
        }

        private string GetContentType(string fileName)
        {
            var startIndex = fileName.LastIndexOf('.') + 1;

            switch (fileName.Substring(startIndex))
            {
                case "png":
                    return "image/png";
                default:
                    throw new Exception();
            }
        }
    }
}
