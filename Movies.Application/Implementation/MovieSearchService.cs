using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Movies.Application.Interface;
using Movies.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Implementation
{
    public class MovieSearchService: IMovieSearchService
    {
        private readonly ILogger<MovieSearchService> _logger;
        private readonly IConfiguration _config;
        private readonly IHttpClientService _client;
        private readonly IMemoryCache _memoryCache;
        public MovieSearchService(ILogger<MovieSearchService> logger, IConfiguration config, IHttpClientService client, IMemoryCache memoryCache)
        {
            _logger = logger;
            _config = config;
            _client = client;
            _memoryCache = memoryCache;
        }

        public async Task<ApiResponse<Movie>> GetMoviesByTitle(string title)
        {
            try
            {
                if (string.IsNullOrEmpty(title)) return new ApiResponse<Movie> { message = "title can not be empty string", responseCode = ResponseCodesEnum.NotPermitted };
                
                var searchResult = await SearchMovie(title);
                if (searchResult != null) {

                    return new ApiResponse<Movie> { message = (searchResult.Response.ToLower() == "false") ? "No movie found" : "Movie found", payload = searchResult, responseCode = ResponseCodesEnum.Success, status = true };
                }
                else
                {
                    return new ApiResponse<Movie> { payload = searchResult, responseCode = ResponseCodesEnum.NotFound, message = searchResult.Response };
                }
               
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"An error occurred: {ex.Message}");
                return new ApiResponse<Movie> { responseCode = ResponseCodesEnum.Error };
            }
        }   
        
        private async Task<Movie> SearchMovie(string title)
        {
            try
            {
                var BaseUrl = _config.GetSection("BaseUrl").Value;
                var apiKey = _config.GetSection("ApiKey").Value;
                var UrlPath = $"{BaseUrl}{apiKey}&t={title}";

                return await _client.GetRequest<Movie>(UrlPath);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"An error occurred: {ex.Message}");
                return null;
            }
        }
        private async Task SaveLast5SearchQueries(string query)
        {
            try
            {                
                var key = DateTime.Now.ToString("yyyyMMdd") + query;
                var isFound = _memoryCache.TryGetValue(key, out List<string> queries);

                if (isFound && queries.Count == 5)
                {
                    queries.RemoveAt(0);
                }

                var cacheExpirationOption = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddHours(24),
                    Priority = CacheItemPriority.Normal,
                    //SlidingExpiration = TimeSpan.FromHours(23)
                };

               
                queries.Add(query);

                _memoryCache.Set(key, query, cacheExpirationOption);                

            }
            catch (Exception)
            {
               _logger.LogInformation($"AN error occured") ;
            }
        }

    }
}
