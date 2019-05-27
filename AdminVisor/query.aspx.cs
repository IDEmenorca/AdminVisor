using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Silme.Sig.BDUtils;
using System.Data;
using System.Configuration;

namespace AdminVisor
{
    public partial class query : System.Web.UI.Page
    {
        public string accio = "";
        public string resultat = "";
        private string tot = "";
        private string totSobreElegit = "";
        private string grups = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                accio = Request.QueryString["accio"];
            }
            catch (ArgumentNullException)
            {
                Response.Redirect("default.aspx");
            }

            
            if (accio.IndexOf("llistar") != -1) llistar();
            else if (accio.IndexOf("insertar") != -1) insertar();
            else if (accio.IndexOf("modificar") != -1) modificar();
            else if(accio.IndexOf("Eines") != -1) eines();
            else if (accio.IndexOf("Mapes") != -1) mapes();
            else if (accio.IndexOf("Serveis") != -1) serveis();
            else if(accio.IndexOf("afegir") != -1)afegir();
            else if (accio.IndexOf("eliminar") != -1) eliminar();
            Response.Write(resultat);
        }

        private void llistar()
        {
            string servidor = "";
            string user= "";
            string bbdd = "";
            string pwd = "";
            string port = "";

            postgis oPostGis = new postgis();
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
            string query;
            if (accio.IndexOf("Projectes") != -1)
            {
                // Agafem el llistat de tots els projectes
                query = "select row_to_json(t) as files from (select * from projecte order by id) t";
            }
            else if (accio.IndexOf("Grups") != -1)
            {
                query = "select row_to_json(t) as files from (select * from grup order by \"idGrup\") t";
            }
            else if (accio.IndexOf("Serveis") != -1)
            {
                query = "select row_to_json(t) as files from (select id, title, url, \"idServei\", \"estaDisponible\" from servei order by \"idServei\") t";
            }
            else // mapes
            {
                query = "select row_to_json(t) as files from (select * from mapa_de_fons order by id) t";
            }
            DataTable resProj = oPostGis.BD_Query(query).Tables[0];
            resultat = "[";
            foreach (DataRow fila in resProj.Rows)
            {
                resultat += fila["files"].ToString().Trim() + ",";
            }

            resultat = resultat.Substring(0, resultat.Length - 1);

            resultat += "]";
        }

        private void insertar()
        {
            string user= "";
            string bbdd = "";
            string pwd = "";
            string port = "";
            string servidor = "";

            postgis oPostGis = new postgis();
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
            string query = "";
            if (accio.IndexOf("Projectes") != -1)
            {
                string nom = Request.QueryString["nom"];
                query = "insert into projecte (nom) values ('"+nom+"');";
            }
            else if (accio.IndexOf("Grups") != -1)
            {
                query = "SELECT id FROM grup ORDER BY \"idGrup\" DESC LIMIT 1";
                DataTable resProj = oPostGis.BD_Query(query).Tables[0];
                foreach (DataRow fila in resProj.Rows)
                {
                    resultat += fila["id"].ToString().Trim();
                }
                string id = "node"+(Int32.Parse(System.Text.RegularExpressions.Regex.Match(resultat, @"\d+").Value)+1);
                string title = Request.QueryString["title"];
                query = "insert into grup (id, title) values ( '" + id + "', '" + title + "')";
            }
            else if (accio.IndexOf("Serveis") != -1)
            {
                string id = Request.QueryString["id"];
                string title = Request.QueryString["title"];
                string type = "SITNA.Consts.layerType.WMS";
                string url = Request.QueryString["url"];
                string estaDisponible = Request.QueryString["estaDisponible"];
                query = "insert into servei (id, title, type, url, \"estaDisponible\") values ('"+id+"', '"+title+"', '"+type+"', '"+url+"', "+estaDisponible+")";
            }
            else // mapes
            {
                string nom = Request.QueryString["nom"];
                string url = Request.QueryString["url"];
                string layerNames = Request.QueryString["layerNames"];
                string type = Request.QueryString["type"];
                string title = Request.QueryString["title"];
                string format = Request.QueryString["format"];
                string matrixSet = Request.QueryString["matrixSet"];
                string thumbnail = Request.QueryString["thumbnail"];
                string estaDisponible = Request.QueryString["estaDisponible"];
                query = "insert into mapa_de_fons (nom, url, \"layerNames\", type, title, format, \"matrixSet\", thumbnail, \"estaDisponible\") values ('"+nom+"', '"+url+"', '"+layerNames+"', '"+type+"', '"+title+"', '"+format+"', '"+matrixSet+"', '"+thumbnail+"', "+estaDisponible+")";
            }

            oPostGis.BD_NonQuery(query);
            
        }

        private void modificar()
        {
            string user= "";
            string bbdd = "";
            string pwd = "";
            string port = "";
            string servidor = "";

            postgis oPostGis = new postgis();
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
            string query = "";
            string id = Request.QueryString["id"];
            if (accio.IndexOf("Projectes") != -1)
            {
                string nom = Request.QueryString["nom"];
                query = "update projecte set nom = '" + nom + "' where id=" + id;
            }
            else if (accio.IndexOf("Grups") != -1)
            {
                string title = Request.QueryString["title"];
                query = "update grup set title = '" + title + "' where id= '" + id + "'";
            }
            else if (accio.IndexOf("Serveis") != -1)
            {
                string title = Request.QueryString["title"];
                string url = Request.QueryString["url"];
                string idServei = Request.QueryString["idServei"];
                string estaDisponible = Request.QueryString["estaDisponible"];
                query = "update servei set id='" + id + "', title='" + title + "', url='" + url + "', \"estaDisponible\"=" + estaDisponible + " where \"idServei\"=" + idServei;
            }
            else
            {
                string nom = Request.QueryString["nom"];
                string url = Request.QueryString["url"];
                string layerNames = Request.QueryString["layerNames"];
                string type = Request.QueryString["type"];
                string title = Request.QueryString["title"];
                string format = Request.QueryString["format"];
                string matrixSet = Request.QueryString["matrixSet"];
                string thumbnail = Request.QueryString["thumbnail"];
                string estaDisponible = Request.QueryString["estaDisponible"];
                query = "update mapa_de_fons set nom='" + nom + "', url='" + url + "', \"layerNames\"='" + layerNames + "', type='" + type + "', title='" + title + "', format='" + format + "', \"matrixSet\"='" + matrixSet + "', thumbnail='" + thumbnail + "', \"estaDisponible\"=" + estaDisponible+" where id="+id;
            }

            oPostGis.BD_NonQuery(query);
        }

        private void eines()
        {
            string idProjecte = Request.QueryString["id"];
            string user= "";
            string bbdd = "";
            string pwd = "";
            string port = "";
            string servidor = "";

            postgis oPostGis = new postgis();
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
            //string query = "select row_to_json(t) as files from (select id, div as nom from eina order by div) t";
            string query = "select row_to_json(t) as files from (select E.id, E.div as nom from eina E inner join projecte_eina PE on E.id = PE.id_eina where E.id not in (select id_eina from projecte_eina where id_projecte = "+idProjecte+") group by E.id) t";
            DataTable resProj = oPostGis.BD_Query(query).Tables[0];
            if (resProj.Rows.Count == 0)
            {
                tot = "cap";
            }
            else
            {
                tot = "[";
                foreach (DataRow fila in resProj.Rows)
                {
                    tot += fila["files"].ToString().Trim() + ",";
                }

                tot = tot.Substring(0, tot.Length - 1);

                tot += "]";
            }


            query = "select row_to_json(t) as files from (select PE.id, E.div as nom from projecte_eina PE join eina E on PE.id_eina = E.id where PE.id_projecte = "+idProjecte+") t";

            resProj = oPostGis.BD_Query(query).Tables[0];
            if (resProj.Rows.Count == 0)
            {
                totSobreElegit = "cap";
            }
            else
            {
                totSobreElegit = "[";
                foreach (DataRow fila in resProj.Rows)
                {
                    totSobreElegit += fila["files"].ToString().Trim() + ",";
                }

                totSobreElegit = totSobreElegit.Substring(0, totSobreElegit.Length - 1);

                totSobreElegit += "]";
            }
            

            resultat = tot + "|" + totSobreElegit;
        }

        private void mapes()
        {
            string idProjecte = Request.QueryString["id"];
            string user= "";
            string bbdd = "";
            string pwd = "";
            string port = "";
            string servidor = "";

            postgis oPostGis = new postgis();
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
            string query = "select row_to_json(t) as files from (select MF.id, MF.nom as nom from mapa_de_fons MF where MF.id not in (select id_mapa_de_fons from projecte_mapa_de_fons where id_projecte = "+idProjecte+")) t";
            DataTable resProj = oPostGis.BD_Query(query).Tables[0];
            if (resProj.Rows.Count == 0)
            {
                tot = "cap";
            }
            else
            {
                tot = "[";
                foreach (DataRow fila in resProj.Rows)
                {
                    tot += fila["files"].ToString().Trim() + ",";
                }

                tot = tot.Substring(0, tot.Length - 1);

                tot += "]";
            }

            query = "select row_to_json(t) as files from (select PM.id, MF.nom as nom from projecte_mapa_de_fons PM join mapa_de_fons MF on MF.id = PM.id_mapa_de_fons where PM.id_projecte = " + idProjecte + " ) t";

            resProj = oPostGis.BD_Query(query).Tables[0];
            if (resProj.Rows.Count == 0)
            {
                totSobreElegit = "cap";
            }
            else
            {
                totSobreElegit = "[";
                foreach (DataRow fila in resProj.Rows)
                {
                    totSobreElegit += fila["files"].ToString().Trim() + ",";
                }

                totSobreElegit = totSobreElegit.Substring(0, totSobreElegit.Length - 1);

                totSobreElegit += "]";
            }


            resultat = tot + "|" + totSobreElegit;
        }

        private void serveis()
        {
            string idProjecte = Request.QueryString["id"];
            string user= "";
            string bbdd = "";
            string pwd = "";
            string port = "";
            string servidor = "";

            postgis oPostGis = new postgis();
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
            // Obtenir tots els serveis que no té aquell projecte
            string query = "select row_to_json(t) as files from (select \"idServei\" as id, title as nom from servei where \"idServei\" NOT IN (select id_servei from projecte_servei_grup where id_projecte = "+idProjecte+")) t";
            DataTable resProj = oPostGis.BD_Query(query).Tables[0];
            if (resProj.Rows.Count == 0)
            {
                tot = "cap";
            }
            else
            {
                tot = "[";
                foreach (DataRow fila in resProj.Rows)
                {
                    tot += fila["files"].ToString().Trim() + ",";
                }

                tot = tot.Substring(0, tot.Length - 1);

                tot += "]";
            }


            // Obtenir tots els grups
            query = "select row_to_json(t) as files from (select \"idGrup\" as id, title as nom from grup) t";
            resProj = oPostGis.BD_Query(query).Tables[0];
            if (resProj.Rows.Count == 0)
            {
                grups = "cap";
            }
            else
            {
                grups = "[";
                foreach (DataRow fila in resProj.Rows)
                {
                    grups += fila["files"].ToString().Trim() + ",";
                }

                grups = grups.Substring(0, grups.Length - 1);

                grups += "]";
            }


            // Obtenir el nom del servei i grup dels que té el projecte
            query = "select row_to_json(t) as files from (select PSG.id as id, S.title as nom, G.title as nom_grup from projecte_servei_grup PSG join servei S on PSG.id_servei = S.\"idServei\" join grup G on PSG.id_grup = G.\"idGrup\" WHERE id_projecte ="+idProjecte+")t";
            resProj = oPostGis.BD_Query(query).Tables[0];
            if (resProj.Rows.Count == 0)
            {
                totSobreElegit = "cap";
            }
            else
            {
                totSobreElegit = "[";
                foreach (DataRow fila in resProj.Rows)
                {
                    totSobreElegit += fila["files"].ToString().Trim() + ",";
                }

                totSobreElegit = totSobreElegit.Substring(0, totSobreElegit.Length - 1);

                totSobreElegit += "]";
            }
            resultat = tot + "|" + totSobreElegit + "|" + grups;
        }

        private void afegir()
        {
            string user= "";
            string bbdd = "";
            string pwd = "";
            string port = "";
            string servidor = "";

            postgis oPostGis = new postgis();
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
            string query = "";
            string id = Request.QueryString["id"];
            string idProjecte = Request.QueryString["idProjecte"];
            if (accio.IndexOf("eina") != -1)
            {
                query = "insert into projecte_eina (id_projecte, id_eina) values (" + idProjecte + ", " + id + ")";
            }else if(accio.IndexOf("mapa") != -1){
                query = "insert into projecte_mapa_de_fons (id_projecte, id_mapa_de_fons) values (" + idProjecte + ", " + id + ")";
            }
            else if (accio.IndexOf("servei") != -1)
            {
                string idGrup = Request.QueryString["idGrup"];
                query = "insert into projecte_servei_grup (id_projecte, id_servei, id_grup) values ("+idProjecte+", "+id+", "+idGrup+")";
            }
            oPostGis.BD_NonQuery(query);
        }

        private void eliminar()
        {
            string user= "";
            string bbdd = "";
            string pwd = "";
            string port = "";
            string servidor = "";

            postgis oPostGis = new postgis();
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
            string query = "";
            string id = Request.QueryString["id"];
            if (accio.IndexOf("eina") != -1)
            {
                query = "delete from projecte_eina where id="+id;
            }
            else if (accio.IndexOf("mapa") != -1)
            {
                query = "delete from projecte_mapa_de_fons where id=" + id;
            }
            else if (accio.IndexOf("servei") != -1)
            {
                query = "delete from projecte_servei_grup where id="+id;
            }
            oPostGis.BD_NonQuery(query);
        }
    }
}