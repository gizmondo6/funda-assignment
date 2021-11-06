using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FundaAssignment.Web.Models;

namespace FundaAssignment.Web.Services
{
    public interface IObjectForSaleService
    {
        Task<IEnumerable<ObjectForSale>> Get(string searchQuery, CancellationToken cancellationToken);
    }
}