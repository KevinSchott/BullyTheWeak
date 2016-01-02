using Newtonsoft.Json.Linq;
using System.Linq;

namespace Analyzer
{
    public class CurrentGameDataRequest
    {
        private IApiService api;
        private const string urlPattern = "observer-mode/rest/consumer/getSpectatorGameInfo/";

        public CurrentGameDataRequest(IApiService apiService)
        {
            api = apiService;
        }

        public CurrentGameData GetCurrentGameData(ApiRegion region, long summonerId)
        {
            var data = api.GetData(ApiRegion.NA, GetFullUrlPattern(region, summonerId));
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

        private string GetFullUrlPattern(ApiRegion region, long summonerId)
        {
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
