using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Interface
{
    public interface IHttpClientService
    {
        Task<T> GetRequest<T>(string uri) where T: class;
        Task<TOut> PostRequest<TIn, TOut>(string uri, TIn content) where TOut : class;
    }
}
