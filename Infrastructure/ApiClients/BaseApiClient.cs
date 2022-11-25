using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using System;
using System.Net.Http;
using System.Text;
using static Shared.Constants.SystemConstants;

namespace ApiIntegration
{
    public class BaseApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<BaseApiClient> _logger;

        public BaseApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<BaseApiClient> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<TResponse> GetAsync<TResponse>(string clientName, string url)
        {
            var client = _httpClientFactory.CreateClient(clientName);
            var response = await client.GetAsync(url);

            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                //_logger.LogInformation($"GET API {client.BaseAddress}{url} successfully");
                TResponse myDeserializedObjList = (TResponse)JsonConvert.DeserializeObject(body,typeof(TResponse));

                return myDeserializedObjList;
            }

            //_logger.LogWarning($"Cannot GET API {client.BaseAddress}{url}");
            return JsonConvert.DeserializeObject<TResponse>(body);
        }
        public async Task<TResponse> PostAsync<TResponse, T>(string clientName, string url, T request)
        {
            var client = _httpClientFactory.CreateClient(clientName);
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, httpContent);

            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                //_logger.LogInformation($"POST API {client.BaseAddress}{url} successfully");
                TResponse myDeserializedObjList = (TResponse)JsonConvert.DeserializeObject(body,typeof(TResponse));

                return myDeserializedObjList;
            }

            //_logger.LogWarning($"Cannot POST API {client.BaseAddress}{url}");
            return JsonConvert.DeserializeObject<TResponse>(body);
        }
        public async Task<TResponse> PutAsync<TResponse, T>(string clientName, string url, T request)
        {
            var client = _httpClientFactory.CreateClient(clientName);
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(url, httpContent);

            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                //_logger.LogInformation($"PUT API {client.BaseAddress}{url} successfully");
                TResponse myDeserializedObjList = (TResponse)JsonConvert.DeserializeObject(body,typeof(TResponse));

                return myDeserializedObjList;
            }

            //_logger.LogWarning($"Canknot PUT API {client.BaseAddress}{url}");
            return JsonConvert.DeserializeObject<TResponse>(body);
        }
        public async Task<bool> Delete(string clientName, string url)
        {
            var client = _httpClientFactory.CreateClient(clientName);

            var response = await client.DeleteAsync(url);

            return response.IsSuccessStatusCode;
        }
    }
}
