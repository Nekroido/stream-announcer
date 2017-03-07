using Announcer.Classes.Helpers;
using Announcer.Models.Responses.Tmdb;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Announcer.Classes.Services
{
    public class TmdbService : ApiBase
    {
        const string API_URL = "http://api.themoviedb.org/3/{0}";

        public const string IMAGE_URL = "http://image.tmdb.org/t/p/w500{0}";

        const string LANGUAGE = "ru";

        private static TmdbService _instance;

        public static TmdbService Instance
        {
            get { return _instance ?? (_instance = new TmdbService()); }
        }

        public async Task<List<MediaResponse>> FindMovie(string query)
        {
            var response = await GetRequest<SearchResponse>("search/movie", new NameValueCollection
            {
                { "query", query }
            });

            return response?.Results ?? new List<MediaResponse>();
        }

        public async Task<List<MediaResponse>> FindShow(string query)
        {
            var response = await GetRequest<SearchResponse>("search/tv", new NameValueCollection
            {
                { "query", query }
            });

            return response?.Results ?? new List<MediaResponse>();
        }

        #region Overrides

        public override NameValueCollection GetParameters(NameValueCollection parameters)
        {
            parameters = base.GetParameters(parameters);

            parameters.Add("format", "json");
            parameters.Add("api_key", Config.GetServiceConfig(DataType.tmdb.ToString()).Token);

            return parameters;
        }

        public override async Task<T> GetRequest<T>(string method, NameValueCollection parameters)
        {
            return await base.GetRequest<T>(string.Format(API_URL, method), parameters);
        }

        #endregion
    }
}
