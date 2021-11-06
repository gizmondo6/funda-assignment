using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FundaAssignment.Web.Models;

namespace FundaAssignment.Web.Services
{
    /// <summary>
    /// Provides information about agents that sell objects on funda.nl.
    /// </summary>
    public class AgentService : IAgentService
    {
        private readonly IObjectForSaleService _objectForSaleService;

        public AgentService(IObjectForSaleService objectForSaleService)
        {
            _objectForSaleService = objectForSaleService;
        }

        /// <summary>
        /// Determines agents have the most object listed for sale. 
        /// </summary>
        /// <param name="searchQuery">Query for filtering objects.</param>
        /// <param name="take">Number of agents to return.</param>
        /// <param name="cancellationToken">Signals when the operations should be canceled.</param>
        /// <returns>Agents have the most object listed for sale.</returns>
        public async Task<IEnumerable<TopSellingAgent>> GetTopSellers(string searchQuery, int take, CancellationToken cancellationToken)
        {
            var objectsForSale = await _objectForSaleService.Get(searchQuery, cancellationToken);
            return objectsForSale
                .GroupBy(ofs => (ofs.AgentId, ofs.AgentName))
                .OrderByDescending(g => g.Count())
                .Take(take)
                .Select(g => new TopSellingAgent {AgentId = g.Key.AgentId, AgentName = g.Key.AgentName, ObjectCount = g.Count()});
        }
    }
}