using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Silme.Sig.BDUtils;
using System.Data;

namespace AdminVisor
{
    public partial class panel : System.Web.UI.Page
    {
        protected string title = "";
        protected string nomUsuari;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Form["nomUsuari"] != null)
            {
                nomUsuari = Request.Form["nomUsuari"];
            }
            else if (Request.QueryString["nomUsuari"] != null)
            {
                nomUsuari = Request.QueryString["nomUsuari"];
            }
            else
            {
                Response.Redirect("default.aspx");
            }
            title = Request.QueryString["panel"] == null ? "Projectes" : Request.QueryString["panel"];
            
        }
    }
}