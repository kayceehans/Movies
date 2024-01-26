using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Movies.Application.Interface;
using Movies.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly ILogger<MovieController> _logger;
        private readonly IMovieSearchService _searchMovie;

        public MovieController(ILogger<MovieController> logger, IMovieSearchService searchMovie)
        {
            _logger = logger;
            _searchMovie = searchMovie;
        }
        [HttpGet("/api/movies/search")]
        [Produces(typeof(ApiResponse<Movie>))]
        public async Task<IActionResult> Search(string title)
        {
            try
            {
                _logger.LogInformation($"Movies searched title: {title}");               

                var response = await _searchMovie.GetMoviesByTitle(title);

                _logger.LogInformation($"Response got from title: {title}  search Result: {response}");

                return Ok(response);
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation($"An error occurred: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}
