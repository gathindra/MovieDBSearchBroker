using Gathi.MovieDB.Broker.Config;
using Gathi.MovieDB.Broker.Models.Movies;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gathi.MovieDB.Broker.ServiceClient
{
    public class MovieDBClient : IMovieDBClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly MoviesControllerConfig _config;

        public MovieDBClient(IHttpClientFactory httpClientFactory,
            IOptions<MoviesControllerConfig> configOptions)
        {
            this._httpClientFactory = httpClientFactory;
            this._config = configOptions.Value;
        }

        public async Task<MovieResponse> SearchMoviesAsync(string query)
        {
            // Get client created. New HttpClientFactory resolves the following issues.
            var client = this._httpClientFactory.CreateClient("MovieDBClient");
            /*
                It’s inefficient as each one will have its own connection pool for the remote server. 
                This means you pay the cost of reconnecting to that remote server for every client you create.
                The bigger problem you can have if you create a lot of them is that you can run into socket exhaustion 
                where you have basically used up too many sockets too fast. 

                There is a limit on how many sockets you can have open at one time. 
                When you dispose of the HttpClient, 
                the connection it had open remains open for up to 240 seconds in a TIME_WAIT state 
                (in case any packets from the remote server still come through).

                A preferred approach therefore it to reuse HttpClient instances so that connections can also be reused.
                Using a singleton HttpClient in this way will keep connections open and not respect the DNS Time To Live 
                (TTL) setting. Now the connections will never get DNS updates so the server you are talking to will 
                never have its address updated. 
             */

            // Set the base URI
            client.BaseAddress = new Uri(this._config.MovieDBBaseUrl);

            // Construct the query with search token
            var queryString = String.Format(this._config.MovieDBQueryFromat,
                query,
                this._config.MovieDBApiKey);

            var response = await client.GetAsync(queryString);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<MovieResponse>(responseData);
            }

            // Just send empty object
            return new MovieResponse();

        }
    }
}
