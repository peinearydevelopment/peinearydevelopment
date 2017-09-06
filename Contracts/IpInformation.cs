using Newtonsoft.Json;

namespace Contracts
{
    public class IpInformation
    {
        public int Id { get; set; }
        public string BusinessName { get; set; }
        public string BusinessWebsite { get; set; }
        public string City { get; set; }
        public string Continent { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string IpName { get; set; }
        public IpType IpType { get; set; }
        [JsonProperty("isp")]
        public string InternetServiceProvider { get; set; }
        [JsonProperty("lat")]
        public string Latitude { get; set; }
        [JsonProperty("lon")]
        public string Longitude { get; set; }
        [JsonProperty("org")]
        public string OrganizationName { get; set; }
        [JsonProperty("query")]
        public string Ip { get; set; }
        public string Region { get; set; }
        public string Status { get; set; }
    }
}
