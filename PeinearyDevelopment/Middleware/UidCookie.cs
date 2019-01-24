using AutoMapper;
using Contracts;
using DataAccess.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PeinearyDevelopment.Middleware
{
    public class UidCookieMiddleware
    {
        private readonly RequestDelegate _next;

        public UidCookieMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context, ISiteStatisticsDal siteStatisticsDal, IMapper mapper, IMemoryCache memoryCache, HttpClient httpClient)
        {
            // https://stackoverflow.com/questions/38098564/custom-aspcore-middleware-with-cancellation-token/38098986#38098986
            var cancellationToken = context?.RequestAborted ?? CancellationToken.None;
            string uid;
            if (!context.Request.Cookies.ContainsKey("uid"))
            {
                uid = Guid.NewGuid().ToString();
                context.Response.Cookies.Append("uid", uid, new CookieOptions { Expires = DateTimeOffset.Now.AddMonths(1) });
            }
            else
            {
                uid = context.Request.Cookies["uid"];
            }

            
            var requestUrl = GetRequestUrl(context);
            var userAgent = context.Request.Headers["User-Agent"];
            var referer = context.Request.Headers["Referer"];
            var ip = context.Connection.RemoteIpAddress.ToString();
            var ipInformation = await GetIpLookupInformation(siteStatisticsDal, mapper, memoryCache, httpClient, ip, cancellationToken).ConfigureAwait(false);
            await siteStatisticsDal.SaveAction(new ActionTakenDto
            {
                IpInformationId = ipInformation.Id,
                ActionType = nameof(ActionType.Request),
                Body = await GetRequestBody(context).ConfigureAwait(false),
                Referer = referer,
                Uid = uid,
                Url = requestUrl,
                UserAgent = userAgent,
                ViewedOn = DateTimeOffset.Now
            }, cancellationToken).ConfigureAwait(false);

            await _next(context).ConfigureAwait(false);
        }

        private string GetRequestUrl(HttpContext context) => string.Concat(context.Request.Scheme, "://", context.Request.Host.ToUriComponent(), context.Request.PathBase.ToUriComponent(), context.Request.Path.ToUriComponent(), context.Request.QueryString.ToUriComponent());

        private async Task<IpInformation> GetIpLookupInformation(ISiteStatisticsDal siteStatisticsDal, IMapper mapper, IMemoryCache memoryCache, HttpClient httpClient, string ip, CancellationToken cancellationToken = default(CancellationToken))
        {
            var cachedIpInformation = memoryCache.Get<IpInformation>(ip);
            if (cachedIpInformation != null)
            {
                return cachedIpInformation;
            }
            else
            {
                var response = await httpClient.GetAsync($"http://extreme-ip-lookup.com/json/{ip}", cancellationToken).ConfigureAwait(false);
                var ipInformation = JsonConvert.DeserializeObject<IpInformation>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                var ipInformationDto = await siteStatisticsDal.EnsureExistsAndGet(mapper.Map<IpInformationDto>(ipInformation), cancellationToken).ConfigureAwait(false);
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1));
                ipInformation = mapper.Map<IpInformation>(ipInformationDto);
                memoryCache.Set(ip, ipInformation, cacheEntryOptions);

                return ipInformation;
            }
        }

        private async Task<string> GetRequestBody(HttpContext context)
        {
            var body = await new StreamReader(context.Request.Body).ReadToEndAsync().ConfigureAwait(false);
            var memoryStream = new MemoryStream();
            var streamWriter = new StreamWriter(memoryStream);
            streamWriter.Write(body);
            streamWriter.Flush();
            memoryStream.Position = 0;
            context.Request.Body = memoryStream;
            return body;
        }
    }
}
