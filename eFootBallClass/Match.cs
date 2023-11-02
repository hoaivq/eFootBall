using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFootBallClass
{
    public class Aggregated
    {
        public int? team1 { get; set; }
        public int? team2 { get; set; }
    }

    public class Current
    {
        public int? team1 { get; set; }
        public int? team2 { get; set; }
    }

    public class Extra1
    {
        public int? team1 { get; set; }
        public int? team2 { get; set; }
    }

    public class Extra2
    {
        public int? team1 { get; set; }
        public int? team2 { get; set; }
    }

    public class Normaltime
    {
        public int? team1 { get; set; }
        public int? team2 { get; set; }
    }

    public class Ot
    {
        public int? team1 { get; set; }
        public int? team2 { get; set; }
    }

    public class Overtime
    {
        public int? team1 { get; set; }
        public int? team2 { get; set; }
    }

    public class Penalties
    {
        public int? team1 { get; set; }
        public int? team2 { get; set; }
    }

    public class Period1
    {
        public int? team1 { get; set; }
        public int? team2 { get; set; }
    }

    public class Period2
    {
        public int? team1 { get; set; }
        public int? team2 { get; set; }
    }

    public class Periods
    {
        public Current Current { get; set; }
        public Normaltime Normaltime { get; set; }
        public Period1 Period1 { get; set; }
        public Period2 Period2 { get; set; }
        public Penalties Penalties { get; set; }
        public Pt pt { get; set; }
        public Ot ot { get; set; }
        public Overtime Overtime { get; set; }
        public Extra1 Extra1 { get; set; }
        public Extra2 Extra2 { get; set; }
        public Aggregated Aggregated { get; set; }
    }

    public class Pt
    {
        public int? team1 { get; set; }
        public int? team2 { get; set; }
    }

    public class Match
    {
        public int? id { get; set; }
        public int? tournament_id { get; set; }
        public int? sport_id { get; set; }
        public DateTime match_time { get; set; }
        public DateTime? start_time { get; set; }
        public DateTime? end_time { get; set; }
        public int? home_team_id { get; set; }
        public int? away_team_id { get; set; }
        public int? home_team_ranking { get; set; }
        public int? away_team_ranking { get; set; }
        public int? home_team_score { get; set; }
        public int? away_team_score { get; set; }
        public int? home_team_half_time_score { get; set; }
        public int? away_team_half_time_score { get; set; }
        public int? home_team_normal_time_score { get; set; }
        public int? away_team_normal_time_score { get; set; }
        public int? status { get; set; }
        public int? status_code { get; set; }
        public int? winner { get; set; }
        public int? aggregate_winner { get; set; }
        public int? season_id { get; set; }
        public string season { get; set; }
        public int? time_played { get; set; }
        public int? time_remaining { get; set; }
        public int? time_running { get; set; }
        public DateTime? time_update { get; set; }
        public int? lmt_mode { get; set; }
        public int? has_live { get; set; }
        public object game_score { get; set; }
        public object attendance { get; set; }
        public int? stadium_id { get; set; }
        public string weather { get; set; }
        public object period { get; set; }
        public object side { get; set; }
        public int? is_visible { get; set; }
        public int? neutral { get; set; }
        public object source_type { get; set; }
        public int? has_lineup { get; set; }
        public int? has_fragment { get; set; }
        public int? category_id { get; set; }
        public int? tournament_level { get; set; }
        public int? period_type { get; set; }
        public int? reverse { get; set; }
        public int? is_hot { get; set; }
        public string ranking_cal_mode { get; set; }
        public int? home_team_corner { get; set; }
        public int? away_team_corner { get; set; }
        public int? home_team_yellow { get; set; }
        public int? away_team_yellow { get; set; }
        public int? home_team_red { get; set; }
        public int? away_team_red { get; set; }
        public int? home_team_overtime_score { get; set; }
        public int? away_team_overtime_score { get; set; }
        public int? home_team_penalty_score { get; set; }
        public int? away_team_penalty_score { get; set; }
        public int? previous_home_team_id { get; set; }
        public int? previous_away_team_id { get; set; }
        public int? previous_home_team_score { get; set; }
        public int? previous_away_team_score { get; set; }
        public object next_match_id { get; set; }
        public string continent { get; set; }
        public string home_team_logo { get; set; }
        public string away_team_logo { get; set; }
        public string tournament_logo { get; set; }
        public string country_logo { get; set; }
        public string category_logo { get; set; }
        public string sport_name { get; set; }
        public string tournament_name { get; set; }
        public string tournament_alias { get; set; }
        public string home_team_name { get; set; }
        public string away_team_name { get; set; }
        public string status_name { get; set; }
        public string stadium_name { get; set; }
        public string previous_home_team_name { get; set; }
        public string previous_away_team_name { get; set; }
        public string country_name { get; set; }
        public string round { get; set; }
        public string round_type { get; set; }
        public string referee_name { get; set; }
        public string previous_round { get; set; }
        public string previous_round_type { get; set; }
        public string category_name { get; set; }
        public bool has_standings { get; set; }
        public bool has_knockouts { get; set; }
        public Periods periods { get; set; }
        public Odds odds { get; set; }
    }




    public class OddsType
    {
        public object id { get; set; }
        public string ovalue { get; set; }
        public string ovalue0 { get; set; }
        public int type_id { get; set; }
        public int active { get; set; }
        public string type { get; set; }
        public object outcome { get; set; }
        public double value { get; set; }
        public double value0 { get; set; }
    }

    public class Odds_2
    {
        public object id { get; set; }
        public object ovalue { get; set; }
        public object ovalue0 { get; set; }
        public int type_id { get; set; }
        public int active { get; set; }
        public string type { get; set; }
        public object outcome { get; set; }
        public double value { get; set; }
        public double value0 { get; set; }
    }

    public class Odds_3
    {
        public object id { get; set; }
        public string ovalue { get; set; }
        public string ovalue0 { get; set; }
        public int type_id { get; set; }
        public int active { get; set; }
        public string type { get; set; }
        public object outcome { get; set; }
        public double value { get; set; }
        public double value0 { get; set; }
    }

    public class Odds_121
    {
        public object id { get; set; }
        public string ovalue { get; set; }
        public string ovalue0 { get; set; }
        public int type_id { get; set; }
        public int active { get; set; }
        public string type { get; set; }
        public object outcome { get; set; }
        public double value { get; set; }
        public double value0 { get; set; }
    }

    public class Odds_122
    {
        public object id { get; set; }
        public string ovalue { get; set; }
        public string ovalue0 { get; set; }
        public int type_id { get; set; }
        public int active { get; set; }
        public string type { get; set; }
        public object outcome { get; set; }
        public double value { get; set; }
        public double value0 { get; set; }
    }

    public class Odds
    {
        [JsonProperty("1")]
        public List<OddsType> _1 { get; set; }

        [JsonProperty("2")]
        public List<OddsType> _2 { get; set; }

        [JsonProperty("3")]
        public List<OddsType> _3 { get; set; }

        [JsonProperty("121")]
        public List<OddsType> _121 { get; set; }

        [JsonProperty("122")]
        public List<OddsType> _122 { get; set; }

        [JsonProperty("128")]
        public List<OddsType> _128 { get; set; }
    }


}
