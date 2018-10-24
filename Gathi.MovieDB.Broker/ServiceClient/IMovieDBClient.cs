using Gathi.MovieDB.Broker.Models.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gathi.MovieDB.Broker.ServiceClient
{
    public interface IMovieDBClient
    {
        Task<MovieResponse> SearchMoviesAsync(string query);
    }
}
