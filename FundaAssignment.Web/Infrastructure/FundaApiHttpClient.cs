using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Polly.Extensions.Http;

namespace FundaAssignment.Web.Infrastructure
{
    /// <summary>
    /// Used to make HTTP calls to funda API and to hold retrying policy.
    /// The retrying policy required to overcome funda API rate limiting when requesting large amount of data.  
    /// Basically it's an abstraction around HttpClient to make unit-testing of code dependent on it easier.   
    /// </summary>
    public class FundaApiHttpClient : IFundaApiHttpClient
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Implement HTTP call retries with exponential backoff.
        /// https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/implement-http-call-retries-exponential-backoff-polly
        /// </summary>
        public static IAsyncPolicy<HttpResponseMessage> RetryPolicy
            => HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        public FundaApiHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<string> GetStringAsync(string requestUri, CancellationToken cancellationToken) =>
            _httpClient.GetStringAsync(requestUri, cancellationToken);
    }
}