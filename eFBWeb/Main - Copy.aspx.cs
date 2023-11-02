using eFootBallClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eFBWeb
{
    public partial class Main : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsPostBack) { return; }
            lstMatch3.DataBind();
            
        }

        protected void lstMatch_DataBinding(object sender, EventArgs e)
        {
            lstMatch3.DataSource = Common.LoadMatch(6).Tables[0];
        }
    }
}