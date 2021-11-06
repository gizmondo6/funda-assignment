namespace FundaAssignment.Web.Models
{
    public record TopSellingAgent
    {
        public long AgentId { get; init; }
                
        public string AgentName { get; init; }
        
        public int ObjectCount { get; init; }
    }
}