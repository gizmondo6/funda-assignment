using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FundaAssignment.Web.Models
{
    public record ObjectsForSaleResponse
    {
        public IReadOnlyList<ObjectForSale> Objects { get; init; }

        public Paging Paging { get; init; }
    }

    public record Paging
    {
        [JsonPropertyName("AantalPaginas")]
        public int TotalPages { get; init; }
    }
}