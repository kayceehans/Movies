using Movies.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Interface
{
    public interface IMovieSearchService
    {
        Task<ApiResponse<Movie>> GetMoviesByTitle(string title);
    }
}
