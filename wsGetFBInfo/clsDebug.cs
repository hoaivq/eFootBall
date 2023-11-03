using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace wsGetFBInfo
{
    public class clsDebug
    {
        static void Main(string[] args)
        {
            Service sv = new Service();
            sv.TmrStart_BDL_Elapsed(null, null);
            //sv.TmrUpdateHistory_BDL_Elapsed(null, null);
        }
    }
}
