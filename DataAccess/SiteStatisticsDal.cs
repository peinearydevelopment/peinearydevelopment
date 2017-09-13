using System.Threading.Tasks;
using DataAccess.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;

namespace DataAccess
{
    public class SiteStatisticsDal : ISiteStatisticsDal
    {
        private PdDbContext DbContext { get; }

        public SiteStatisticsDal(PdDbContext dbContext) => DbContext = dbContext;

        public async Task<IpInformationDto> EnsureExistsAndGet(IpInformationDto ipInformation, CancellationToken cancellationToken = default(CancellationToken))
        {
            var dbIpInformation = await DbContext.IpInformations
                                                 .Where(ip => ip.Ip == ipInformation.Ip)
                                                 .Where(ip => ip.BusinessName == ipInformation.BusinessName)
                                                 .Where(ip => ip.BusinessWebsite == ipInformation.BusinessWebsite)
                                                 .Where(ip => ip.City == ipInformation.City)
                                                 .Where(ip => ip.Continent == ipInformation.Continent)
                                                 .Where(ip => ip.Country == ipInformation.Country)
                                                 .Where(ip => ip.CountryCode == ipInformation.CountryCode)
                                                 .Where(ip => ip.IpName == ipInformation.IpName)
                                                 .Where(ip => ip.IpType == ipInformation.IpType)
                                                 .Where(ip => ip.InternetServiceProvider == ipInformation.InternetServiceProvider)
                                                 .Where(ip => ip.Latitude == ipInformation.Latitude)
                                                 .Where(ip => ip.Longitude == ipInformation.Longitude)
                                                 .Where(ip => ip.OrganizationName == ipInformation.OrganizationName)
                                                 .Where(ip => ip.Region == ipInformation.Region)
                                                 .Where(ip => ip.Status == ipInformation.Status)
                                                 .FirstOrDefaultAsync(cancellationToken)
                                                 .ConfigureAwait(false);
            if (dbIpInformation != null)
            {
                return dbIpInformation;
            }
            else
            {
                var newIpInformationDto = await DbContext.IpInformations
                                                         .AddAsync(ipInformation, cancellationToken)
                                                         .ConfigureAwait(false);
                await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                return newIpInformationDto.Entity;
            }
        }

        public async Task SaveAction(ActionTakenDto action, CancellationToken cancellationToken = default(CancellationToken))
        {
            await DbContext.ActionsTaken.AddAsync(action, cancellationToken).ConfigureAwait(false);
            await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
