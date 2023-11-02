using System;
namespace wsGetFBInfo
{
    public class Stat
    {
        public string stat_name { get; set; }
        public string home_value { get; set; }
        public string away_value { get; set; }
    }
    public class DMatch_BDL
    {
        private int _id;
        public int id { get { return _id; } set { _id = value; } }

        private DateTime? _match_time;
        public DateTime? match_time { get { return _match_time; } set { _match_time = value; } }

        private int? _position;
        public int? position { get { return _position; } set { _position = value; } }

        private int? _state_id;
        public int? state_id { get { return _state_id; } set { _state_id = value; } }

        private int? _league_id;
        public int? league_id { get { return _league_id; } set { _league_id = value; } }

        private int? _home_id;
        public int? home_id { get { return _home_id; } set { _home_id = value; } }

        private int? _away_id;
        public int? away_id { get { return _away_id; } set { _away_id = value; } }

        private string _league_name;
        public string league_name { get { return _league_name; } set { _league_name = value; } }

        private string _home_name;
        public string home_name { get { return _home_name; } set { _home_name = value; } }

        private string _away_name;
        public string away_name { get { return _away_name; } set { _away_name = value; } }

        private int? _home_rank;
        public int? home_rank { get { return _home_rank; } set { _home_rank = value; } }

        private int? _away_rank;
        public int? away_rank { get { return _away_rank; } set { _away_rank = value; } }

        private int? _home_score;
        public int? home_score { get { return _home_score; } set { _home_score = value; } }

        private int? _home_corner;
        public int? home_corner { get { return _home_corner; } set { _home_corner = value; } }

        private int? _home_red;
        public int? home_red { get { return _home_red; } set { _home_red = value; } }

        private int? _home_yellow;
        public int? home_yellow { get { return _home_yellow; } set { _home_yellow = value; } }

        private int? _home_score_h1;
        public int? home_score_h1 { get { return _home_score_h1; } set { _home_score_h1 = value; } }

        private int? _home_corner_h1;
        public int? home_corner_h1 { get { return _home_corner_h1; } set { _home_corner_h1 = value; } }

        private int? _away_score;
        public int? away_score { get { return _away_score; } set { _away_score = value; } }

        private int? _away_corner;
        public int? away_corner { get { return _away_corner; } set { _away_corner = value; } }

        private int? _away_red;
        public int? away_red { get { return _away_red; } set { _away_red = value; } }

        private int? _away_yellow;
        public int? away_yellow { get { return _away_yellow; } set { _away_yellow = value; } }

        private int? _away_score_h1;
        public int? away_score_h1 { get { return _away_score_h1; } set { _away_score_h1 = value; } }

        private int? _away_corner_h1;
        public int? away_corner_h1 { get { return _away_corner_h1; } set { _away_corner_h1 = value; } }


        private double? _hdc;
        public double? hdc { get { return _hdc; } set { _hdc = value; } }

        private double? _tx;
        public double? tx { get { return _tx; } set { _tx = value; } }
    }
}