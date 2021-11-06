using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FundaAssignment.Web.Models;

namespace FundaAssignment.Web.Services
{
    public interface IAgentService
    {
        Task<IEnumerable<TopSellingAgent>> GetTopSellers(string searchQuery, int take, CancellationToken cancellationToken);
    }
}