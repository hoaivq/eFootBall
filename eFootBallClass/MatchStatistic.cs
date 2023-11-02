using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFootBallClass
{
    public class MatchStatistic
    {
        public int id { get; set; }
        public int match_id { get; set; }
        public int type_id { get; set; }
        public int team1 { get; set; }
        public int team2 { get; set; }
        public DateTime update_time { get; set; }
        public int period { get; set; }
        public string type_name { get; set; }
    }
}
