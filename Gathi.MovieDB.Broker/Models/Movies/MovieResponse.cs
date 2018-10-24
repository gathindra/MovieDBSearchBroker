using Newtonsoft.Json;

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
        public Result[] Results { get; set; }
    }
}
