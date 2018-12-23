using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gathi.MovieDB.Broker.Config;
using Gathi.MovieDB.Broker.ServiceClient;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Gathi.MovieDB.Broker.Utils;
using Gathi.MovieDB.Broker.Models.Movies;
using System.Net;

namespace Gathi.MovieDB.Broker.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [EnableCors("GathiPolicy")]
    public class MoviesController : ControllerBase
    {
        private readonly ILogger<MoviesController> _logger;

        private readonly IMovieDBClient _movieDBClient;

        private readonly MoviesControllerConfig _config;

        public MoviesController(IMovieDBClient movieDBClient,
            IOptions<MoviesControllerConfig> configOptions,
            ILogger<MoviesController> logger)
        {
            this._movieDBClient = movieDBClient;
            this._config = configOptions.Value;
            this._logger = logger;

        }

        [HttpGet]
        public async Task<ActionResult> Search(string query, int page)
        {
            try
            {
                // If query is empty send empty response to the caller
                if (String.IsNullOrWhiteSpace(query))
                {
                    return Ok(new MovieResponse
                    {
                        Page = 1,
                        TotalPages = 0,
                        TotalResults = 0,
                        Results = new List<Result>()
                    });
                }

                var search = WebUtility.UrlEncode(query);
                _logger.LogDebug("Initiating MusicDB service call with search token {0} and page {1}", 
                                query,
                                page);
                var moviesResponse = await this._movieDBClient
                                                .SearchMoviesAsync(search, page);

                // Step - 1
                // Get the result list ordered alphabetically
                // There are some movies doesn't contain the title that matches search but
                // other attribites contains the search token. We need
                // remove those too while ordering the list.

                // Step - 2
                // We need to create proper resouce path for the image
                // Result is something like /adw6Lq9FiC9zjYEpOqfq03ituwp.jpg
                // Actual resource path is https://image.tmdb.org/t/p/w185/adw6Lq9FiC9zjYEpOqfq03ituwp.jpg
                // results.SetAbsoluteFilmPosterUrlPath(this._config.MovieDBPosterUrlFormat);
   
                var sortedMovieResponse = new FluentDataHelper(moviesResponse)
                                            .SortAlphabetically(query)
                                            .SetUrlPath(this._config.MovieDBPosterUrlFormat)
                                            .Build();
                return Ok(sortedMovieResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Movies");
                return StatusCode(500, ex.Message);
            }
        }
    }
}