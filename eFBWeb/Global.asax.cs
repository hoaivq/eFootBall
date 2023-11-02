using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace eFBWeb
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            //DevExpress.Web.ASPxWebControl.CallbackError += ASPxWebControl_CallbackError;
        }

        private void ASPxWebControl_CallbackError(object sender, EventArgs e)
        {
            //var exception = HttpContext.Current.Server.GetLastError();
            //DevExpress.Web.ASPxWebControl.SetCallbackErrorMessage(exception.Message);
        }

        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue";
        }
    }
}