using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wsGetFBInfo
{
   

    public class List
    {
        public int id { get; set; }
        public int time { get; set; }
        public int type_id { get; set; }
        public string type_name { get; set; }
        public string scores { get; set; }
    }

    public class MatchTimeLine
    {
        public int position { get; set; }
        public int team { get; set; }
        public int type_id { get; set; }
        public int status_code { get; set; }
        public long riskFactor { get; set; }
        public List<List> list { get; set; }
    }
}
