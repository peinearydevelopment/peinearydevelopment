using System.Threading;
using System.Threading.Tasks;

namespace DataAccess.Contracts
{
    public interface ISiteStatisticsDal
    {
        Task<IpInformationDto> EnsureExistsAndGet(IpInformationDto ipInformation, CancellationToken cancellationToken = default(CancellationToken));
        Task SaveAction(ActionTakenDto action, CancellationToken cancellationToken = default(CancellationToken));
    }
}
