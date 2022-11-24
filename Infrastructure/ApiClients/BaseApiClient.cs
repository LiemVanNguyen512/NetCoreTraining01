using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using System.Net.Http;
using System.Text;
using static Shared.Constants.SystemConstants;

namespace ApiIntegration
{
    public class BaseApiClient
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly ILogger<BaseApiClient> _logger;
        private readonly AsyncRetryPolicy _retryPolicy;

        public BaseApiClient(HttpClient client, IConfiguration configuration, ILogger<BaseApiClient> logger)
        {
            _client = client;
            _configuration = configuration;
            _logger = logger;
            _retryPolicy = Policy.Handle<Exception>()
                    .WaitAndRetryAsync(
                        int.Parse(_configuration[AppSettings.RetryCount])-1, 
                        retryAttempt => TimeSpan.FromSeconds(int.Parse(_configuration[AppSettings.RetryAttemptSeconds])));
            _logger.LogInformation($"Retry count: {_configuration[AppSettings.RetryCount]}, Retry Attempt Seconds: {_configuration[AppSettings.RetryAttemptSeconds]}");
        }

        public async Task<TResponse> GetAsync<TResponse>(string baseAddress, string url)
        {
            _client.BaseAddress = new Uri(_configuration[baseAddress]);
            var response = await _retryPolicy.ExecuteAsync(async () =>
            {
                _logger.LogInformation($"Retry GET API {_client.BaseAddress}{url}");
                return await _client.GetAsync(url);
            });

            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"GET API {_client.BaseAddress}{url} successfully");
                TResponse myDeserializedObjList = (TResponse)JsonConvert.DeserializeObject(body,typeof(TResponse));

                return myDeserializedObjList;
            }

            _logger.LogWarning($"Cannot GET API {_client.BaseAddress}{url}");
            return JsonConvert.DeserializeObject<TResponse>(body);
        }
        public async Task<TResponse> PostAsync<TResponse, T>(string baseAddress, string url, T request)
        {
            _client.BaseAddress = new Uri(_configuration[baseAddress]);
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _retryPolicy.ExecuteAsync(async () =>
            {
                _logger.LogInformation($"Retry POST API {_client.BaseAddress}{url}");
                return await _client.PostAsync(url, httpContent);
            });

            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"POST API {_client.BaseAddress}{url} successfully");
                TResponse myDeserializedObjList = (TResponse)JsonConvert.DeserializeObject(body,typeof(TResponse));

                return myDeserializedObjList;
            }

            _logger.LogWarning($"Cannot POST API {_client.BaseAddress}{url}");
            return JsonConvert.DeserializeObject<TResponse>(body);
        }
        public async Task<TResponse> PutAsync<TResponse, T>(string baseAddress, string url, T request)
        {
            _client.BaseAddress = new Uri(_configuration[baseAddress]);
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _retryPolicy.ExecuteAsync(async () =>
            {
                _logger.LogInformation($"Retry PUT API {_client.BaseAddress}{url}");
                return await _client.PutAsync(url, httpContent);
            });

            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"PUT API {_client.BaseAddress}{url} successfully");
                TResponse myDeserializedObjList = (TResponse)JsonConvert.DeserializeObject(body,typeof(TResponse));

                return myDeserializedObjList;
            }

            _logger.LogWarning($"Cannot PUT API {_client.BaseAddress}{url}");
            return JsonConvert.DeserializeObject<TResponse>(body);
        }
        public async Task<bool> Delete(string baseAddress, string url)
        {
            _client.BaseAddress = new Uri(_configuration[baseAddress]);

            var response = await _retryPolicy.ExecuteAsync(async () =>
            {
                _logger.LogInformation($"Retry DELETE API {_client.BaseAddress}{url}");
                return await _client.DeleteAsync(url);
            });

            return response.IsSuccessStatusCode;
        }
    }
}
