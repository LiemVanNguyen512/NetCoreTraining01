using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using Serilog;

namespace Infrastructure.Policies
{
    public static class HttpClientRetryPolicy
    {
        public static IHttpClientBuilder UseImmediateHttpRetryPolicy(this IHttpClientBuilder builder,
        int retryCount = 3)
        {
            return builder.AddPolicyHandler(ConfigureImmediateHttpRetry(retryCount));
        }

        public static IHttpClientBuilder UseLinearHttpRetryPolicy(this IHttpClientBuilder builder,
            int retryCount = 3, int retryAttemptSeconds = 5)
        {
            return builder.AddPolicyHandler(ConfigureLinearHttpRetry(retryCount, retryAttemptSeconds));
        }

        public static IHttpClientBuilder UseExponentialHttpRetryPolicy(this IHttpClientBuilder builder,
            int retryCount = 3)
        {
            return builder.AddPolicyHandler(ConfigureExponentialHttpRetry(retryCount));
        }              
        private static IAsyncPolicy<HttpResponseMessage> ConfigureImmediateHttpRetry(int retryCount) =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<TimeoutRejectedException>()
                .RetryAsync(retryCount, (exception, retryCount, context) =>
                {
                    Log.Error($"Retry {retryCount} of {context.PolicyKey} at " +
                              $"{context.OperationKey}, due to: {exception.Exception.Message}");
                });

        private static IAsyncPolicy<HttpResponseMessage> ConfigureLinearHttpRetry(int retryCount, int retryAttemptSeconds)
        {
            Log.Information($"Retry {retryCount} times send request");
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(retryAttemptSeconds),
                    (exception, retryCount, context) =>
                    {
                        Log.Error($"Retry {retryCount} of {context.PolicyKey} at " +
                              $"{context.OperationKey}, due to: {exception.Exception.Message}");
                    });
        }
            


        private static IAsyncPolicy<HttpResponseMessage> ConfigureExponentialHttpRetry(int retryCount) =>
            // In this case will wait for
            //  2 ^ 1 = 2 seconds then
            //  2 ^ 2 = 4 seconds then
            //  2 ^ 3 = 8 seconds then
            //  2 ^ 4 = 16 seconds then
            //  2 ^ 5 = 32 seconds
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(retryCount, retryAttempt
                        => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, retryCount, context) =>
                    {
                        Log.Error($"Retry {retryCount} of {context.PolicyKey} at " +
                              $"{context.OperationKey}, due to: {exception.Exception.Message}");
                    });
    }
}
