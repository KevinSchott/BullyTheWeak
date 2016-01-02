using Newtonsoft.Json.Linq;

namespace Analyzer
{
    public class SummonerDataRequest
    {
        private IApiService api;
        private const string urlPattern = "api/lol/na/v1.4/summoner/by-name/";

        public SummonerDataRequest(IApiService apiService)
        {
            api = apiService;
        }

        public SummonerData GetSummonerByName(ApiRegion region, string name)
        {
            var fetch = api.GetData(ApiRegion.NA, urlPattern + name);
            if (fetch == null)
                return null;

            var data = fetch[name.Replace(" ", "").ToLower()];
            return new SummonerData
            {
                Id = data["id"].Value<long>(),
                Name = data["name"].Value<string>(),
                ProfileIconId = data["profileIconId"].Value<int>(),
                RevisionDate = data["revisionDate"].Value<long>(),
                Level = data["summonerLevel"].Value<int>()
            };
        }
    }
}
