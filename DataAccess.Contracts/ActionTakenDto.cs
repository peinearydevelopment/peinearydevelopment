using System;

namespace DataAccess.Contracts
{
    public class ActionTakenDto
    {
        public int Id { get; set; }
        public int IpInformationId { get; set; }
        public DateTimeOffset ViewedOn { get; set; }
        public string Url { get; set; }
        /*
         * https://stackoverflow.com/questions/4734248/how-to-know-if-http-request-is-a-bot
         * https://stackoverflow.com/questions/2776013/how-do-i-detect-bots-programatically
         */
        public string UserAgent { get; set; }
        public string Referer { get; set; }
        public string Uid { get; set; }
        public string Body { get; set; }
        public string ActionType { get; set; }

        public virtual IpInformationDto IpInformation { get; set; }
    }
}
