using Announcer.Classes.Helpers;
using Announcer.Db;
using Announcer.Models;
using Announcer.Models.Responses.GiantBomb;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Announcer.Classes.Services
{
    public class GiantBombService : ApiBase
    {
        const string API_URL = "http://www.giantbomb.com/api/{0}/";

        private static GiantBombService _instance;

        public static GiantBombService Instance
        {
            get { return _instance ?? (_instance = new GiantBombService()); }
        }

        public async Task<List<GameResponse>> FindGame(string title, string game)
        {
            return await FindGame(title);
        }

        public async Task<List<GameResponse>> FindGame(string title, uint? platform = null)
        {
            return await FindGame(title, platform != null ? new List<uint?> { platform } : new List<uint?>());
        }

        public async Task<List<GameResponse>> FindGame(string title, List<uint?> platforms)
        {
            var parameters = new NameValueCollection
            {
                { "field_list", "id,name,image" },
                { "filter", "name:" + StringHelper.EncodeGiantBombSpecialCharacters(title) },
                { "limit", "20" }
            };

            if (platforms?.Count > 0)
            {
                parameters["filter"] += ",platforms:" + string.Join("|", platforms);
            }

            var response = await GetRequest<GameSearchResponse>("games", parameters);

            return response?.Results ?? new List<GameResponse>();
        }

        public async Task CachePlatforms()
        {
            Dictionary<string, List<string>> cachedAliases;
            try
            {
                var path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "aliases.json");
                cachedAliases = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(File.ReadAllText(path));
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString(), Logger.Severity.error);

                return;
            }

            uint addedPlatforms = 0;
            uint addedAliases = 0;
            using (var context = new Context())
            {
                var existingPlatforms = await context.Platforms
                    .Include(x => x.Aliases)
                    .ToListAsync();

                uint limit = 100;
                uint offset = 0;
                bool fetchFinished = false;
                do
                {
                    var response = await GetRequest<PlatformListResponse>("platforms", new NameValueCollection
                    {
                        { "limit", limit.ToString() },
                        { "offset", offset.ToString() }
                    });

                    foreach (var platform in response.Results)
                    {
                        var aliases = new List<string>
                        {
                            platform.Name
                        };
                        var cachedPlatform = cachedAliases.FirstOrDefault(p => p.Value.Contains(platform.Name));
                        if (cachedPlatform.Equals(default(KeyValuePair<string, List<string>>)) == false)
                        {
                            aliases.AddRange(cachedPlatform.Value);
                        }
                        aliases.Add(platform.Abbreviation);
                        aliases = aliases.Distinct(StringComparer.OrdinalIgnoreCase).ToList();

                        var existingPlatform = await context.Platforms.FirstOrDefaultAsync(x => x.Abbreviation == platform.Abbreviation);
                        if (existingPlatform == null)
                        {
                            var newPlatform = new Platform
                            {
                                Abbreviation = platform.Abbreviation,
                                Name = platform.Name,
                                GiantBombId = platform.Id,
                                Aliases = new List<PlatformAlias>()
                            };

                            foreach (var alias in aliases)
                            {
                                newPlatform.Aliases.Add(new PlatformAlias
                                {
                                    Alias = alias
                                });
                            }

                            addedPlatforms++;
                            addedAliases += (uint)aliases.Count;

                            context.Platforms.Add(newPlatform);
                        }
                        else
                        {
                            foreach (var alias in aliases)
                            {
                                if (existingPlatform.Aliases.Any(x => x.Alias.ToLower() == alias.ToLower()) == false) // Skip exisitng aliases
                                {
                                    existingPlatform.Aliases.Add(new PlatformAlias
                                    {
                                        Alias = alias
                                    });
                                    addedAliases += (uint)aliases.Count;
                                }
                            }
                        }
                        await context.SaveChangesAsync();
                    }

                    offset += limit;
                    fetchFinished = response.TotalResultsOnPage == 0;
                }
                while (fetchFinished == false);

                Logger.Log($"- Added {addedPlatforms} new platform(s) and {addedAliases} new alias(es).", Logger.Severity.info);
            }
        }

        #region Overrides

        public override HttpClient GetClient()
        {
            var client = base.GetClient();

            client.DefaultRequestHeaders.Add("User-Agent", "MustardBombsAwesomeBot");

            return client;
        }

        public override NameValueCollection GetParameters(NameValueCollection parameters)
        {
            parameters = base.GetParameters(parameters);

            parameters.Add("format", "json");
            parameters.Add("api_key", Config.GetServiceConfig(DataType.giantbomb.ToString()).Token);

            return parameters;
        }

        public override async Task<T> GetRequest<T>(string method, NameValueCollection parameters)
        {
            return await base.GetRequest<T>(string.Format(API_URL, method), parameters);
        }

        #endregion
    }
}
