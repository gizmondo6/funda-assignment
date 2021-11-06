using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FundaAssignment.Web.Infrastructure;
using FundaAssignment.Web.Models;
using FundaAssignment.Web.Services;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using Shouldly;

namespace FundaAssignment.Web.UnitTests.Services
{
    public class ObjectForSaleServiceTests
    {
        [Test]
        public async Task Get()
        {
            var options = Substitute.For<IOptions<AppSettings>>();
            options.Value.Returns(new AppSettings
            {
                FundaApiBaseUrl = "http://test.fake.api",
                FundaApiKey = "c6a4717bdebd4c01a77638c495251495",
                FundaApiPageSize = 2
            });
            var fundaApiClient = Substitute.For<IFundaApiHttpClient>();
            fundaApiClient.GetStringAsync(
                "http://test.fake.api/feeds/Aanbod.svc/json/c6a4717bdebd4c01a77638c495251495?type=koop&zo=/amsterdam/&page=1&pagesize=2",
                CancellationToken.None
            ).Returns(
                File.ReadAllTextAsync("Services/ObjectForSaleServiceTests.Get.1.json")
            );
            fundaApiClient.GetStringAsync(
                "http://test.fake.api/feeds/Aanbod.svc/json/c6a4717bdebd4c01a77638c495251495?type=koop&zo=/amsterdam/&page=2&pagesize=2",
                CancellationToken.None
            ).Returns(
                File.ReadAllTextAsync("Services/ObjectForSaleServiceTests.Get.2.json")
            );

            var objectForSaleService = new ObjectForSaleService(fundaApiClient, options);

            var objectsForSale = await objectForSaleService.Get("/amsterdam/", CancellationToken.None);

            objectsForSale.ShouldBe(new[]
            {
                new ObjectForSale
                    {Id = Guid.Parse("3F4E0D7E-332A-44D8-933D-0D88EA4908D7"), AgentId = 1, AgentName = "Agent 1"},
                new ObjectForSale
                    {Id = Guid.Parse("A7108368-5B71-4704-BF9B-1AB8AAD0B92E"), AgentId = 1, AgentName = "Agent 1"},
                new ObjectForSale
                    {Id = Guid.Parse("7F45620C-4C05-46D1-B85A-B003B3DE58FF"), AgentId = 2, AgentName = "Agent 2"},
                new ObjectForSale
                    {Id = Guid.Parse("8E71DEEC-E0FC-4629-9F8C-D711882F3996"), AgentId = 3, AgentName = "Agent 3"}
            });
        }
    }
}