using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Common
{
    public class MyApp
    {
        public static AppCommon Common { get; set; } = new AppCommon();
        public static AppSetting Setting { get; set; } = new AppSetting();
        public static AppLog Log { get; set; } = new AppLog();
        public static AppDao Dao { get; set; } = new AppDao();

       
    }
}