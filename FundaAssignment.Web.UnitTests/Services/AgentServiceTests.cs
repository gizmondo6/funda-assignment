using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FundaAssignment.Web.Models;
using FundaAssignment.Web.Services;
using NSubstitute;
using NUnit.Framework;
using Shouldly;

namespace FundaAssignment.Web.UnitTests.Services
{
    public class AgentServiceTests
    {
        [Test]
        public async Task GetTopSellers()
        {
            var objectForSaleService = Substitute.For<IObjectForSaleService>();

            objectForSaleService.Get("/amsterdam/", CancellationToken.None).Returns(
                Task.FromResult<IEnumerable<ObjectForSale>>(new[]
                {
                    new ObjectForSale
                        {Id = Guid.Parse("4A56984E-6686-4664-90BD-B99E5C9165A7"), AgentId = 1, AgentName = "Agent 1"},

                    new ObjectForSale
                        {Id = Guid.Parse("A9D1094F-CBFF-4C5A-B8BD-C48510723988"), AgentId = 2, AgentName = "Agent 2"},
                    new ObjectForSale
                        {Id = Guid.Parse("2D1E6999-8E9D-468D-AFCA-E3E9856AB504"), AgentId = 2, AgentName = "Agent 2"},

                    new ObjectForSale
                        {Id = Guid.Parse("2A54EAED-5ADD-4588-B6B8-D441DC8576FA"), AgentId = 3, AgentName = "Agent 3"},
                    new ObjectForSale
                        {Id = Guid.Parse("8C64477E-22F5-41B0-86C5-26608B6ADA7D"), AgentId = 3, AgentName = "Agent 3"},
                    new ObjectForSale
                        {Id = Guid.Parse("A51BB860-7376-4694-90AE-8F1D604E2A88"), AgentId = 3, AgentName = "Agent 3"}
                }));

            var agentService = new AgentService(objectForSaleService);

            var top = (await agentService.GetTopSellers("/amsterdam/", 2, CancellationToken.None)).ToList();

            top.ShouldBe(new[]
            {
                new TopSellingAgent {AgentId = 3, AgentName = "Agent 3", ObjectCount = 3},
                new TopSellingAgent {AgentId = 2, AgentName = "Agent 2", ObjectCount = 2}
            });
        }
    }
}