using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Xml;
using System;
using System.IO;
using System.Linq;
using HDFacturacionDTE.Entity;

namespace HDFacturacionDTE.IData
{
    public static class Comunes
    {
        public static void LlenaCombo(List<BEItemGenerico> datos, DropDownList ddlCombo, bool blnInsertarSeleccionar = false)
        {
            if (datos == null) { return; }
            if (ddlCombo == null) { return; }
            if (blnInsertarSeleccionar)
            {
                string seleccionar = ConfigurationManager.AppSettings["Seleccionar"];
                BEItemGenerico item = new BEItemGenerico() { Codigo = "00", Descripcion = seleccionar, Descripcion2 = seleccionar };
                datos.Insert(0, item);
            }
            ddlCombo.DataSource = datos;
            ddlCombo.DataValueField = "Codigo";
            ddlCombo.DataTextField = "Descripcion";
            ddlCombo.DataBind();
        }

        //PROY-24740
        public static string GetUrlByCodigoPortal(string id_portal)
        {
            string strRutaSite = ConfigurationManager.AppSettings["RutaSite"];
            strRutaSite = strRutaSite.Substring(0, strRutaSite.LastIndexOf("/"));
            string strRutas = ConfigurationManager.AppSettings["contRutaSitePortal"];
            string url = "";
            List<string> arrRutas = strRutas.Split(';').ToList();
            url = string.Format("{0}/{1}", strRutaSite, (arrRutas.Single(s => s.Split(',')[0] == id_portal)).Split(',')[1]);
            return url;
        }

    }
}
