using Analyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace BullyConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var apikey = ConfigurationManager.AppSettings["RiotApiKey"].ToString();
            

            var api = new ApiService(apikey);
            var sdr = new SummonerDataRequest(api);

            var sd = sdr.GetSummonerByName(ApiRegion.NA, "n3ac3y");

            var cgdr = new CurrentGameDataRequest(api);
            var cgd = cgdr.GetCurrentGameData(ApiRegion.NA, sd.Id);

            Console.WriteLine("Data for " + sd.Name);
            if (cgd == null)
            {
                Console.WriteLine(sd.Name + " is not currently in a game.");
                End();
            }

            var blues = cgd.TeamMembers.Where(t => t.IsBlueTeam).ToList();
            var reds = cgd.TeamMembers.Where(t => !t.IsBlueTeam).ToList();

            Console.WriteLine("Blue Team");
            blues.ForEach(t => PrintMember(t));
            Console.WriteLine("\nRed Team");
            reds.ForEach(t => PrintMember(t));
            End();
        }

        static void PrintMember(TeamMember member)
        {
            Console.WriteLine(member.SummonerName);
        }

        static void End()
        {
            Console.WriteLine("\nPress any key to exit.");
            Console.ReadLine();
        }
    }
}
