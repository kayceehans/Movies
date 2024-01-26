using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Movies.Application.Implementation;
using Movies.Application.Interface;
using Movies.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Movies.Test
{
    public class MovieSearchByTitleTest
    {
        private readonly Mock<ILogger<MovieSearchService>> _logger;
        private readonly Mock<IConfiguration> _config;
        private readonly Mock<IHttpClientService> _client;
        private readonly Mock<IMemoryCache> _cache;

        public MovieSearchByTitleTest()
        {
            _logger = new Mock<ILogger<MovieSearchService>>();
            _config = new Mock<IConfiguration>();
            _client = new Mock<IHttpClientService>();
            _cache = new Mock<IMemoryCache>();
        }

        [Fact]
        public void Search_For_Movies_With_Empty_String()
        {
            // Arrange 

            var searchTitle = string.Empty;
            // Act
            var service = new MovieSearchService(_logger.Object, _config.Object, _client.Object, _cache.Object).GetMoviesByTitle(searchTitle);

            var resp = service.Result;
            // Assert

            Assert.Equal(ResponseCodesEnum.NotPermitted, resp.responseCode);

        }

        [Fact]
        public void Search_For_Movies_With_Null_Response()
        {
            // Arrange 
            var searchTitle = "ToyStory";

            var config = new Mock<IConfigurationSection>();
            config.Setup(c => c.Value).Returns("http://www.omdbapi.com/?apikey=");

            config.Setup(c=>c.GetSection("BaseUrl")).Returns(config.Object);

            var configA = new Mock<IConfigurationSection>();
            configA.Setup(c => c.Value).Returns("1234");

            config.Setup(c => c.GetSection("ApiKey")).Returns(configA.Object);

            _client.Setup(c=>c.GetRequest<Data.Movie>(It.IsAny<string>())).Returns(Task.FromResult(new Data.Movie() ));
            // Act

            var service = new MovieSearchService(_logger.Object, _config.Object, _client.Object, _cache.Object).GetMoviesByTitle(searchTitle);
              var resp = service.Result;
            // Assert

            Assert.Equal(ResponseCodesEnum.NotFound, resp.responseCode);
        }

        [Fact]
        public void Search_For_Movies_With_Response()
        {
            // Arrange 
            var searchTitle = "ToyStory";

            var config = new Mock<IConfigurationSection>();
            config.Setup(c => c.Value).Returns("http://www.omdbapi.com/?apikey=");

            config.Setup(c => c.GetSection("BaseUrl")).Returns(config.Object);

            var configA = new Mock<IConfigurationSection>();
            configA.Setup(c => c.Value).Returns("1234");

            config.Setup(c => c.GetSection("ApiKey")).Returns(configA.Object);

            _client.Setup(c => c.GetRequest<Data.Movie>(It.IsAny<string>())).Returns(Task.FromResult(new Data.Movie()));
            // Act

            var service = new MovieSearchService(_logger.Object, _config.Object, _client.Object, _cache.Object).GetMoviesByTitle(searchTitle);
            var resp = service.Result;
            // Assert

            Assert.Equal(ResponseCodesEnum.Success, resp.responseCode); 


        }
    }
}
