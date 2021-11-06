using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FundaAssignment.Web.Infrastructure;
using FundaAssignment.Web.Models;
using Microsoft.Extensions.Options;

namespace FundaAssignment.Web.Services
{
    /// <summary>
    /// Provides information about the objects that are listed on funda.nl which are for sale.
    /// </summary>
    public class ObjectForSaleService : IObjectForSaleService
    {
        private readonly IFundaApiHttpClient _httpClient;
        private readonly string _fundaApiBaseUrl;
        private readonly string _fundaApiKey;
        private readonly int _fundaApiPageSize;

        public ObjectForSaleService(IFundaApiHttpClient httpClient, IOptions<AppSettings> appSettings)
        {
            _httpClient = httpClient;
            _fundaApiBaseUrl = appSettings.Value.FundaApiBaseUrl;
            _fundaApiKey = appSettings.Value.FundaApiKey;
            _fundaApiPageSize = appSettings.Value.FundaApiPageSize;
        }

        /// <summary>
        /// Returns objects for sale that correspond to provided query.
        /// </summary>
        /// <param name="searchQuery">Query for filtering objects.</param>
        /// <param name="cancellationToken">Signals when the operations should be canceled.</param>
        /// <returns>The objects for sale that correspond to the query.</returns>
        public async Task<IEnumerable<ObjectForSale>> Get(string searchQuery, CancellationToken cancellationToken)
        {
            var allObjects = new List<ObjectForSale>();
            ObjectsForSaleResponse pageResponse;
            var currentPage = 1;
            do
            {
                cancellationToken.ThrowIfCancellationRequested();
                pageResponse = await GetPageOfObjects(searchQuery, currentPage, cancellationToken);
                allObjects.AddRange(pageResponse.Objects);
                currentPage++;
            } while (currentPage <= pageResponse.Paging.TotalPages);

            return allObjects;
        }

        private async Task<ObjectsForSaleResponse> GetPageOfObjects(string searchQuery, int page,
            CancellationToken cancellationToken)
        {
            var uri = BuildUriForPage(searchQuery, page);
            var responseString = await _httpClient.GetStringAsync(uri, cancellationToken);
            var response = JsonSerializer.Deserialize<ObjectsForSaleResponse>(responseString);
            return response;
        }

        private string BuildUriForPage(string searchQuery, int page)
        {
            var parameters =
                $"?type=koop&zo={searchQuery}&page={page.ToString()}&pagesize={_fundaApiPageSize.ToString()}";
            return $"{_fundaApiBaseUrl}/feeds/Aanbod.svc/json/{_fundaApiKey}{parameters}";
        }
    }
}