using Gathi.MovieDB.Broker.Models.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gathi.MovieDB.Broker.Utils
{
    public class FluentDataHelper
    {
        private MovieResponse _movieResponse;

        public FluentDataHelper(MovieResponse movieResponse)
        {
            this._movieResponse = movieResponse;
        }

        public FluentDataHelper SortAlphabetically(string query)
        {
            this._movieResponse.Results = this._movieResponse
                                            .OrderByAcesnding(query);

            return this;
        }

        public FluentDataHelper SetUrlPath(string urlFormat)
        {
            this._movieResponse.Results
                .SetAbsoluteFilmPosterUrlPath(urlFormat);

            return this;
        }

        public MovieResponse Build()
        {
            return this._movieResponse;
        }
    }
}
