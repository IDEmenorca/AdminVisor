using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Silme.Sig.BDUtils;
using System.Data;
using System.Text;
using System.Configuration;

namespace AdminVisor
{
    public partial class _default : System.Web.UI.Page
    {
        protected string msgIni;
        protected string usuari;
        protected string password;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                msgIni = "";
                usuari = Request.Form["usuari"];
                password = Request.Form["password"];
                if (usuari != null && usuari != "" &&
                    password != null && password != "")
                {
                    postgis oPostGis = new postgis();
                    string servidor = "";
                    string user= "";
                    string bbdd = "";
                    string pwd = "";
                    string port = "";
                    for (int i = 0; i < ConfigurationManager.ConnectionStrings.Count; i++)
                    {
                        string cs = ConfigurationManager.ConnectionStrings[i].ConnectionString;
                        string[] dadesCs = cs.Split(';');
                        if (ConfigurationManager.ConnectionStrings[i].Name.Contains("Conexion_app_visor"))
                        {
                            servidor = dadesCs[0].Split('=')[1];
                            user = dadesCs[2].Split('=')[1];
                            bbdd = dadesCs[1].Split('=')[1];
                            pwd = dadesCs[3].Split('=')[1];
                            port = dadesCs[4].Split('=')[1];
                        }
                    }

                    oPostGis.ConnString = oPostGis.BD_ConnString(servidor, port, user, pwd, bbdd);
                    string query = "select * from usuaris where usuari='" + usuari + "' and pwd='" + password + "'";
                    DataTable resUsuari = oPostGis.BD_Query(query).Tables[0];


                    if (resUsuari.Rows.Count > 0) // l'usuari i contrasenya correctes
                    {
                        
                            
                        Response.Clear();

                        StringBuilder sb = new StringBuilder();
                        sb.Append("<html>");
                        sb.AppendFormat(@"<body onload='document.forms[""form""].submit()'>");
                        sb.AppendFormat("<form name='form' action='panel.aspx' method='post'>");
                        sb.AppendFormat("<input type='hidden' name='nomUsuari' value='" + resUsuari.Rows[0]["usuari"] + "'>");
                        sb.Append("</form>");
                        sb.Append("</body>");
                        sb.Append("</html>");

                        Response.Write(sb.ToString());

                        Response.End();

                    }
                    else
                    {
                        msgIni = "'{\"missatge\":\"Usuari i password incorrectes. Tornar-ho a provar.\",\"correcte\":\"no\"}'";
                    }
                }
            }
            catch (ArgumentNullException)
            {
                Response.Redirect("default.aspx");
            }
        }
    }
}