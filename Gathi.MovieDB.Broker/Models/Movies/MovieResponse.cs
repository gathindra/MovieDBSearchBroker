using Newtonsoft.Json;
using System.Collections.Generic;

namespace Gathi.MovieDB.Broker.Models.Movies
{
    public class MovieResponse
    {
        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("total_results")]
        public int TotalResults { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("results")]
        public List<Result> Results { get; set; }
    }
}
