using System.Threading;
using System.Threading.Tasks;

namespace FundaAssignment.Web.Infrastructure
{
    public interface IFundaApiHttpClient
    {
        Task<string> GetStringAsync(string requestUri, CancellationToken cancellationToken);
    }
}