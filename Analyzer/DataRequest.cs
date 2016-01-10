using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Analyzer
{
    public class DataRequest
    {
        protected IApiService api;

        public DataRequest (IApiService apiService)
        {
            api = apiService;
        }

        public SummonerData GetSummonerByName(ApiRegion region, string name)
        {
             const string urlPattern = "api/lol/na/v1.4/summoner/by-name/";

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

        public CurrentGameData GetCurrentGameData(ApiRegion region, long summonerId)
        {
            var data = api.GetData(ApiRegion.NA, BuildUrlPattern(region, summonerId));
            if (data == null)
                return null;

            var participants = data["participants"];

            return new CurrentGameData
            {
                TeamMembers = participants
                .Children()
                .Select(t => new TeamMember
                {
                    SummonerId = t["summonerId"].Value<long>(),
                    SummonerName = t["summonerName"].Value<string>(),
                    IsBlueTeam = t["teamId"].Value<long>() == 100
                }).ToList()
            };
        }

        private string BuildUrlPattern(ApiRegion region, long summonerId)
        {
            const string urlPattern = "observer-mode/rest/consumer/getSpectatorGameInfo/";

            string basePattern = null;
            switch (region)
            {
                case ApiRegion.NA:
                    basePattern = urlPattern + "NA1";
                    break;
                case ApiRegion.EUW:
                    basePattern = urlPattern + "EUW1";
                    break;
            }
            return basePattern + "/" + summonerId + "/";
        }
    }
}
