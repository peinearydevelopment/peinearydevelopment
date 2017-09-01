using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DataAccess.Contracts
{
    public class PageViewDto
    {
        public int Id { get; set; }
        public string BusinessName { get; set; }
        public string BusinessWebsite { get; set; }
        public string City { get; set; }
        public string Continent { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string IpName { get; set; }
        public string IpType { get; set; }
        [JsonProperty("isp")]
        public string InternetServiceProvider { get; set; }
        [JsonProperty("lat")]
        public string Latitude { get; set; }
        [JsonProperty("lon")]
        public string Longitude { get; set; }
        public string Org { get; set; }
        [JsonProperty("query")]
        public string Ip { get; set; }
        public string Region { get; set; }
        public string Status { get; set; }
        public DateTimeOffset ViewedOn { get; set; }
        public string ViewedUrl { get; set; }

        private HttpClient HttpClient { get; }

        public PageViewDto() => ViewedOn = DateTimeOffset.Now;

        public PageViewDto(HttpClient httpClient, string url)
        {
            HttpClient = httpClient;
            ViewedUrl = url;
        }

        public async Task<PageViewDto> Deserialize(string ip)
        {
            var response = await HttpClient.GetAsync($"http://extreme-ip-lookup.com/json/{ip}").ConfigureAwait(false);
            var visitorDto = JsonConvert.DeserializeObject<PageViewDto>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            visitorDto.ViewedUrl = ViewedUrl;
            return visitorDto;
        }
    }
}
