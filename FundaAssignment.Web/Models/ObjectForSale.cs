using System;
using System.Text.Json.Serialization;

namespace FundaAssignment.Web.Models
{
    public record ObjectForSale
    {
        public Guid Id { get; init; }
        
        [JsonPropertyName("MakelaarId")]
        public long AgentId { get; init; }
        
        [JsonPropertyName("MakelaarNaam")]
        public string AgentName { get; init; }
    }
}