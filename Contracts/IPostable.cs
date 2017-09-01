using System;

namespace Contracts
{
    public interface IPostable
    {
        DateTimeOffset PostedOn { get; set; }
    }
}
