using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Data;
using System.Configuration;
using System.Globalization;

using HDFacturacionDTE.Entity;
using HDFacturacionDTE.Business;
using HDFacturacionDTE.Common;

namespace Portal_HDFacturacion
{
    public partial class verFacturasElectronicasHD : System.Web.UI.Page
    {

        string sRUCEmisor = "";
        BLTipoDocumento oBLTipoDocumento = null;
        BLDocumentoDTE oBLDocumentoDTE = null;
        List<BETipoDocumento> lstTiposDocumentos = null;
        string sDocumentoPDF = "", sDocumentoXML = "";

        

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            if (!IsPostBack)//CARGA INICIAL
            {
                Session["bValidado"] = "0";
                Session["sRUCEmisor"] = "";
                Session["sDocumentoPDF"] = "";
                Session["sDocumentoXML"] = ""; 

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
                        if (nextKey.Equals("RUC"))
                            displayValues.Append(postedValues[i]);
                    }
                }

                sRUCEmisor = displayValues.ToString();
                Session["sRUCEmisor"] = sRUCEmisor;

                if (!sRUCEmisor.Equals(""))
                {
                    txtRucEmisor.Text = sRUCEmisor;
                    txtRucEmisor.Enabled = false;
                }
                else
                    txtRucEmisor.Enabled = true;


                Inicio();
            }
        }

        protected void Inicio()
        {
            oBLTipoDocumento = new BLTipoDocumento();            

            lstTiposDocumentos = oBLTipoDocumento.getListaDocumentos();

            ddlTipoDocumento.DataTextField = "sDESC_TIPODOCUMENTO";
            ddlTipoDocumento.DataValueField = "sID_TIPODOCUMENTO";
            ddlTipoDocumento.DataSource = lstTiposDocumentos;
            ddlTipoDocumento.DataBind();

        }

        protected void cldFechaEmision_SelectionChanged(object sender, EventArgs e)
        {
            txtFechaEmision.Text = cldFechaEmision.SelectedDate.ToShortDateString();
            divCalendario.Style.Value = "display:none";
        }

        protected void imgbCalendario_Click(object sender, ImageClickEventArgs e)
        {
            string sStyle = divCalendario.Style.Value.ToUpper();

            if (sStyle.Equals("DISPLAY:NONE"))
                divCalendario.Style.Value = "display:block";
            else
                divCalendario.Style.Value = "display:none";
        }

        protected void btnVerFactura_Click(object sender, EventArgs e)
        {
            string sRutaPDF;
            sRUCEmisor = txtRucEmisor.Text;

            Session["sRUCEmisor"] = sRUCEmisor;

            bool bExisteRuc = false;
            string sMensaje = "";
            
            string sFechaEmi = txtFechaEmision.Text;
            string sTipoDoc = ddlTipoDocumento.SelectedValue.ToString();
            string sSerieDoc = txtSerieDocumento.Text;
            string sNroDoc = txtNroDocumento.Text;
            string sMontoDoc = txtMontoFactura.Text;
            DateTime dFechaValida;
            bool bFechaValida = false;
            bool bTodoOK = true;
            double dMonto;
            bool bMontoValida =  false;

            try
            {
                bFechaValida = DateTime.TryParse(sFechaEmi, out dFechaValida);
                sFechaEmi = dFechaValida.ToShortDateString();
                txtFechaEmision.Text = sFechaEmi;
            }
            catch
            { bFechaValida = false; }
            try
            {
                bMontoValida  = double.TryParse(sMontoDoc, out dMonto);
            }
            catch
            { bMontoValida = false; }


            Session["bValidado"] = "0";

            /*
             * Validaciones de controles antes de llamar a URL
             */
            if (!bFechaValida)
            {
                sMensaje = "Fecha de Emisión no tiene formato válido";
                bTodoOK = false;
            }

            if (!bMontoValida)
            {
                if (sMensaje.Equals(""))
                    sMensaje = "Monto de documento no tiene formato válido 9999.99";
                else
                    sMensaje = sMensaje + "|Monto de documento no tiene formato válido 9999.99";

                bTodoOK = false;
            }

            if (bTodoOK)
            {

                /*
                 * Validamos existencia de RUC EMISOR
                 */

                oBLDocumentoDTE = new BLDocumentoDTE();
                bExisteRuc = oBLDocumentoDTE.ValidarRUCEmisor(sRUCEmisor, ref sMensaje);
                if (bExisteRuc)
                {

                    /*
                     * Obtener el nombre del archivoS segun base de datos
                     */
                    BEDocumentosDTE oBEDocumentoDTE = new BEDocumentosDTE();
                    oBEDocumentoDTE = oBLDocumentoDTE.ObtenerDocumentosDTE(sRUCEmisor, sTipoDoc, sSerieDoc, sNroDoc, sFechaEmi, sMontoDoc);

                    if (oBEDocumentoDTE != null)
                    {
                        sDocumentoPDF = oBEDocumentoDTE.sDOCUMENTOPDF;
                        sDocumentoXML = oBEDocumentoDTE.sDOCUMENTOXML;

                        Session["sDocumentoPDF"] = sDocumentoPDF;
                        Session["sDocumentoXML"] = sDocumentoXML; 

                        Session["bValidado"] = "1";

                        sRutaPDF = "http://192.168.1.111/dte/" + sRUCEmisor + "/" + sDocumentoPDF;
                        iframePDF.Attributes.Add("src", sRutaPDF);
                    }
                    else
                    {
                        sMensaje = "Documento no Existe.  Caso contario rectifique valores de búsqueda.!";
                        iframePDF.Attributes.Add("src", "verMensajeError.aspx?Mensaje=" + sMensaje);
                    }
                }
                else
                    iframePDF.Attributes.Add("src", "verMensajeError.aspx?Mensaje=" + sMensaje);
            }
            else
                iframePDF.Attributes.Add("src", "verMensajeError.aspx?Mensaje=" + sMensaje);
        }

        protected void btnExportarXML_Click(object sender, EventArgs e)
        {
            string sValor = Convert.ToString(Session["bValidado"]);
            string sRUC = Convert.ToString(Session["sRUCEmisor"]);
            string sDocumento = Convert.ToString(Session["sDocumentoXML"]); 

            if (!sRUC.Equals("")) 
            {
                if (sValor.Equals("1"))
                {
                    try
                    {
                        string strURL = @"http://192.168.1.111/dte/" + sRUC + "/" + sDocumento;
                        WebClient req = new WebClient();
                        HttpResponse response = HttpContext.Current.Response;
                        response.Clear();
                        response.ClearContent();
                        response.ClearHeaders();
                        response.Buffer = true;
                        response.AddHeader("Content-Disposition", "attachment;filename=\"" + sDocumento + "\"");
                        byte[] data = req.DownloadData(strURL);
                        response.BinaryWrite(data);
                        response.End();
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }

        protected void btbExportarPDF_Click(object sender, EventArgs e)
        {
            string sValor = Convert.ToString(Session["bValidado"]);
            string sRUC = Convert.ToString(Session["sRUCEmisor"]);
            string sDocumento = Convert.ToString(Session["sDocumentoPDF"]);

            if (!sRUC.Equals(""))
            {
                if (sValor.Equals("1"))
                {

                    try
                    {
                        string strURL = @"http://192.168.1.111/dte/" + sRUC + "/" + sDocumento;
                        WebClient req = new WebClient();
                        HttpResponse response = HttpContext.Current.Response;
                        response.Clear();
                        response.ClearContent();
                        response.ClearHeaders();
                        response.Buffer = true;
                        response.AddHeader("Content-Disposition", "attachment;filename=\"" + sDocumento + "\"");
                        byte[] data = req.DownloadData(strURL);
                        response.BinaryWrite(data);
                        response.End();
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }


    }
}