using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gathi.MovieDB.Broker.Config
{
    public class MoviesControllerConfig
    {
        public string MovieDBBaseUrl { get; set; }
        public string MovieDBQueryFromat { get; set; }
        public string MovieDBPosterUrlFormat { get; set; }
        public string MovieDBApiKey { get; set; }
    }
}
