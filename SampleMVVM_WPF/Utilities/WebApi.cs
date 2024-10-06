using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SampleMVVM_WPF.Interfaces;

namespace SampleMVVM_WPF.Utilities
{
    public sealed class WebApi : IWebApi
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string connectionString;

        public WebApi(IHttpClientFactory httpClientFactory, 
                      IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            connectionString = configuration.GetSection("Users_API:ConnectionString").Value ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<HttpResponseMessage?> GetTAsync(Dictionary<string, string>? queries, string endpoint) 
        {
            var requestPath = new StringBuilder(connectionString + endpoint);
            if (queries is not null && queries.Count != 0)
            {
                requestPath.Append('?');
                foreach (var query in queries)
                    requestPath.Append($"{query.Key}={query.Value}");
            }

            try
            {
                using HttpClient client = _httpClientFactory.CreateClient(requestPath.ToString());

                var response = await client.GetAsync(requestPath.ToString())
                                            .ConfigureAwait(false);

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<HttpResponseMessage?> PostTAsync<T>(T postValue, string endpoint) 
        {
            var requestPath = new StringBuilder(connectionString + endpoint);

            try
            {
                using HttpClient client = _httpClientFactory.CreateClient(requestPath.ToString());

                var response = await client.PostAsJsonAsync(requestPath.ToString(),
                                                            value: postValue).ConfigureAwait(false);

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<HttpResponseMessage?> PutTAsync<T>(Dictionary<string, string> queries,
                                                             T putValue,
                                                             string endpoint) 
        {
            var requestPath = new StringBuilder(connectionString + endpoint);
            if (queries is not null && queries.Count != 0)
            {
                requestPath.Append('?');
                foreach (var query in queries)
                    requestPath.Append($"{query.Key}={query.Value}");
            }

            try
            {
                using HttpClient client = _httpClientFactory.CreateClient(requestPath.ToString());

                var response = await client.PutAsJsonAsync(requestPath.ToString(),
                                                           value: putValue).ConfigureAwait(false);

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
