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
        protected Dictionary<ApiRegion, string> regionCodes;
        protected Dictionary<ApiRegion, string> regionCodesAlt;
        

        public DataRequest (IApiService apiService)
        {
            api = apiService;
            regionCodes = new Dictionary<ApiRegion, string>
            {
                {ApiRegion.NA, "na" },
                {ApiRegion.EUW, "euw"}
            };

            regionCodesAlt = new Dictionary<ApiRegion, string>
            {
                {ApiRegion.NA, "NA1" },
                {ApiRegion.EUW, "EUW1"}
            };
        }

        public SummonerData GetSummonerByName(ApiRegion region, string name)
        {
            string summonerByNameUrl = string.Format("api/lol/{0}/v1.4/summoner/by-name/{1}",
                regionCodes[region], name);

            var fetch = api.GetData(region, summonerByNameUrl);
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
            string currentGameDataUrl = string.Format("observer-mode/rest/consumer/getSpectatorGameInfo/{0}/{1}",
                regionCodesAlt[region], summonerId);

            var data = api.GetData(region, currentGameDataUrl);
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
                    ChampionId = t["championId"].Value<long>(),
                    IsBlueTeam = t["teamId"].Value<long>() == 100
                }).ToList()
            };
        }


        public RecentGameData GetRecentGameData(ApiRegion region, long summonerId)
        {
            string recentGameDataUrl = string.Format("/api/lol/{0}/v1.3/game/by-summoner/{1}/recent",
                regionCodes[region], summonerId);

            var data = api.GetData(region, recentGameDataUrl);
            if (data == null)
                return null;

            return new RecentGameData
            {
            };
        }
    }
}
