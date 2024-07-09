using FormulaOne.Entities.Dtos.Responses;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using RestSharp;
using System.Net;
using System.Text.Json;

namespace FormulaOne.Api.Services
{
    public class FlightService : IFlightService
    {
        // Retry Policy
        private static readonly AsyncRetryPolicy<RestResponse> RetryPolicy =
            Policy.HandleResult<RestResponse>(resp =>
                resp.StatusCode == HttpStatusCode.TooManyRequests || (int)resp.StatusCode >= 500)
                .WaitAndRetryAsync(4, retryAttempt =>
                {
                    Console.WriteLine($"Attempt {retryAttempt} Retrying dur to error.");
                    return TimeSpan.FromSeconds(15 + retryAttempt);
                });


        private static readonly AsyncCircuitBreakerPolicy<RestResponse> CBPolicy =
            Policy.HandleResult<RestResponse>(resp => (int)resp.StatusCode >= 500)
            .CircuitBreakerAsync(2, TimeSpan.FromMinutes(1));

        private static readonly AsyncCircuitBreakerPolicy<RestResponse> AdvanceCBPolicy =
            Policy.HandleResult<RestResponse>(resp => (int)resp.StatusCode >= 500)
            .CircuitBreakerAsync(2, TimeSpan.FromMinutes(1));

        public async Task<List<FlightDto>> GetAllAvailableFlights()
        {
            if (CBPolicy.CircuitState == CircuitState.Open)
            {
                throw new Exception("Service is not availabe");
            }

            const string url = "http://localhost:5087/api/FlightsCalendar";
            var client = new RestClient();
            var request = new RestRequest(url);

            //var response = await client.ExecuteAsync(request);

            //var response = await RetryPolicy.ExecuteAsync(async () => await client.ExecuteAsync(request)); // Retry policy

            //var response = await CBPolicy.ExecuteAsync(
            //                        async () => await RetryPolicy.ExecuteAsync(
            //                            async () => await client.ExecuteAsync(request))); // Circuit breaker policy

            var response = await AdvanceCBPolicy.ExecuteAsync(
                                    async () => await RetryPolicy.ExecuteAsync(
                                        async () => await client.ExecuteAsync(request))); // Advance Circuit breaker policy

            if (!response.IsSuccessful)
            {
                throw new Exception("somehing went wrong");
            }

            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };

            return JsonSerializer.Deserialize<List<FlightDto>>(response.Content, options);
        }
    }
}
