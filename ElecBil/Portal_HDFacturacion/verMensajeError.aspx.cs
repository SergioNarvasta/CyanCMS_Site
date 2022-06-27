using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Portal_HDFacturacion
{
    public partial class verMensajeError : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sRespuesta;
            if (!IsPostBack)//CARGA INICIAL
            {
                System.Text.StringBuilder displayValues =
                    new System.Text.StringBuilder();

                System.Collections.Specialized.NameValueCollection
                    postedValues = Request.QueryString;

                String nextKey;
                for (int i = 0; i < postedValues.AllKeys.Length; i++)
                {
                    nextKey = postedValues.AllKeys[i];
                    nextKey = nextKey.ToUpper();
                    if (nextKey.Substring(0, 2) != "__")
                    {
                        if (nextKey.Equals("MENSAJE"))
                            displayValues.Append(postedValues[i]);
                    }
                }

                sRespuesta = displayValues.ToString();
                sRespuesta = sRespuesta.Replace("|", "<br/>");

                divRespuesta.InnerHtml = "<P>" + sRespuesta + "</P>";
            }
        }
    }
}