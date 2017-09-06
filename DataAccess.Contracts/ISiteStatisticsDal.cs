using System.Threading.Tasks;

namespace DataAccess.Contracts
{
    public interface ISiteStatisticsDal
    {
        Task<IpInformationDto> EnsureExistsAndGet(IpInformationDto ipInformation);
        Task SaveAction(ActionTakenDto action);
    }
}
