using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SampleMVVM_WPF.Interfaces
{
    public interface IWebApi
    {
        public Task<HttpResponseMessage?> GetTAsync(Dictionary<string, string>? queries, string endpoint);
        public Task<HttpResponseMessage?> PostTAsync<T>(T postValue, string endpoint);
        public Task<HttpResponseMessage?> PutTAsync<T>(Dictionary<string, string>? queries,
                                                       T putValue,
                                                       string endpoint);
    }
}
