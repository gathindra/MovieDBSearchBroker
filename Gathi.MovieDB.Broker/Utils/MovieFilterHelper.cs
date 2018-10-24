using Gathi.MovieDB.Broker.Models.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gathi.MovieDB.Broker.Utils
{
    public static class MovieFilterHelper
    {
        public static List<Result> OrderByAcesnding(this MovieResponse movieResponse, string query)
        {
            if (movieResponse.Results == null) return new List<Result>();

            var resultList = movieResponse.Results
                .Where(m => m.Title.ToLower().Contains(query.ToLower()))
                .OrderBy(m => m.Title)
                .ToList();

            return resultList;

        }

        public static void SetAbsoluteFilmPosterUrlPath(this List<Result> movieList, string urlFormat)
        {
            foreach (var movie in movieList)
            {
                // Some movie result doesn't contain poster path
                if (!String.IsNullOrWhiteSpace(movie.PosterPath))
                {
                    movie.PosterPath = String.Format(urlFormat, movie.PosterPath);
                }
            }
            
        }
    }
}
