using System;
using System.Collections.Generic;
using System.Linq;

using RestSharp;
using Newtonsoft.Json.Linq;

namespace HDT.Plugins.ArenaStats
{
    internal class HeroInfo
    {
        public string name;
        public int game_type;
        public double popularity;
        public int total_games;
        public double win_rate;
    }

    internal class HeroTier
    {
        // 2 = RANKED_STANDARD, 3 = ARENA, 30 = RANKED_WILD
        //GameType.GT_ARENA; // = 5
        public static List<HeroInfo> getHeroTiers(int game_type = 3)
        {
            var baseUrl = "https://hsreplay.net"; ;
            var client = new RestClient(baseUrl);
            var request = new RestRequest("analytics/query/player_class_performance_summary/");
            IRestResponse response = client.Execute(request);
            Dictionary<String, List<HeroInfo>> heroData = (Dictionary<String, List<HeroInfo>>)
                JObject.Parse(response.Content)["series"]["data"].ToObject(typeof(Dictionary<String, List<HeroInfo>>));
            List<HeroInfo> heroInfo = new List<HeroInfo>();
            foreach (string hero in heroData.Keys)
            {
                for (int i = 0; i < heroData[hero].Count; i++)
                {
                    if (heroData[hero][i].game_type == game_type)
                    {
                        HeroInfo h = new HeroInfo();
                        h.name = hero;
                        h.game_type = game_type;
                        h.popularity = heroData[hero][i].popularity;
                        h.total_games = heroData[hero][i].total_games;
                        h.win_rate = heroData[hero][i].win_rate;
                        heroInfo.Add(h);
                    }
                }
            }
            return heroInfo.OrderByDescending(x => x.win_rate).ToList();
        }
    }
}
