using System.Collections.Generic;


namespace Analyzer
{
    public class TeamMember
    {
        public long SummonerId { get; set; }
        public string SummonerName { get; set; }
        public bool IsBlueTeam { get; set; }
    }

    public class CurrentGameData
    {
        public List<TeamMember> TeamMembers { get; set; }
    }
}
