using HDFacturacionDTE.Business;
using HDFacturacionDTE.Entity;
using Portal_HDFacturacion_V2.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Portal_HDFacturacion_V2.Controllers
{
    public class HomeController : Controller
    {
        string sRUCEmisor = "";
        BLTipoDocumento oBLTipoDocumento = null;
        BLDocumentoDTE oBLDocumentoDTE = null;
        List<BETipoDocumento> lstTiposDocumentos = null;
        string sDocumentoPDF = "", sDocumentoXML = "";

        public ActionResult Index()
        {
            //oBLTipoDocumento = new BLTipoDocumento();

            //lstTiposDocumentos = oBLTipoDocumento.getListaDocumentos();

            //var Oficinas = lstTiposDocumentos;
            //ViewBag.Oficinas = Oficinas;
            Session["bValidado"] = "0";
            Session["sRUCEmisor"] = "";
            Session["sDocumentoPDF"] = "";
            Session["sDocumentoXML"] = "";

            return View();
        }
        public ActionResult Report(int id)
        {
            // Usas id para saber que report (pdf) debes usar
            string sURL_DTE = Server.MapPath("~/DTE");
            var filename = sURL_DTE;
            return File(filename, "application/pdf");
        }

        [HttpPost]
        public JsonResult ConsultaSunat(ComprobanteViewModel objSunat)
        {
            //objSunat.NumeroRuc = "20518000951";
            //objSunat.CodigoComp = "01";
            //objSunat.CodigoComp = "FAC";
            //objSunat.NumeroSerie = "F001";
            //objSunat.Numero = "1";
            //objSunat.FechaEmision = "05/01/2018";
            //objSunat.Monto = "298018.500";
            var fec = Convert.ToDateTime(objSunat.FechaEmision).ToString("dd/MM/yyyy");

            String response = "";
            Boolean isOK = false;

            string sRutaPDF;
            sRUCEmisor = objSunat.NumeroRuc;

            string sURL_DTE = ConfigurationManager.AppSettings["URLDTE"].ToString();
            string ruta_DTE = Server.MapPath("~/DTE");
            Session["sRUCEmisor"] = sRUCEmisor;

            bool bExisteRuc = false;
            string sMensaje = "";

            string sFechaEmi = fec;
            string sTipoDoc = objSunat.CodigoComp;
            string sSerieDoc = objSunat.NumeroSerie.ToUpper();
            string sNroDoc = objSunat.Numero;
            string sMontoDoc = objSunat.Monto;
            DateTime dFechaValida;
            bool bFechaValida = false;
            bool bTodoOK = true;
            double dMonto;
            bool bMontoValida = false;

            try
            {
                bFechaValida = DateTime.TryParse(sFechaEmi, out dFechaValida);
                sFechaEmi = dFechaValida.ToShortDateString();
                //txtFechaEmision.Text = sFechaEmi;
            }
            catch
            { bFechaValida = false; }
            try
            {
                bMontoValida = double.TryParse(sMontoDoc, out dMonto);
            }
            catch
            { bMontoValida = false; }


            Session["bValidado"] = "0";

            /*
             * Validaciones de controles antes de llamar a URL
             */
            if (!bFechaValida)
            {
                isOK = false;
                sMensaje = "Fecha de Emisión no tiene formato válido";
                bTodoOK = false;
            }

            if (!bMontoValida)
            {
                if (sMensaje.Equals(""))
                {
                    isOK = false;
                    sMensaje = "Monto de documento no tiene formato válido 9999.99";
                }
                else
                {
                    isOK = false;
                    sMensaje = sMensaje + "|Monto de documento no tiene formato válido 9999.99";
                }
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

                        sRutaPDF = sURL_DTE + sRUCEmisor + "/" + sDocumentoPDF;
                        //iframePDF.Attributes.Add("src", sRutaPDF);
                        //response = sRutaPDF;
                        //ruta_DTE = ruta_DTE + sRUCEmisor + "/" + sDocumentoPDF;
                        isOK = true;
                        response = sRutaPDF;
                    }
                    else
                    {
                        isOK = false;
                        sMensaje = "Documento no Existe o datos ingresados no corresponden. !Rectifique valores de búsqueda.!";
                        //iframePDF.Attributes.Add("src", "verMensajeError.aspx?Mensaje=" + sMensaje);

                    }
                }
                else
                {
                    isOK = false;
                    sMensaje = "No existen registros con ese Numero de RUC.";
                    //iframePDF.Attributes.Add("src", "verMensajeError.aspx?Mensaje=" + sMensaje);
                }
            }
            else
            {
                isOK = false;
                //iframePDF.Attributes.Add("src", "verMensajeError.aspx?Mensaje=" + sMensaje);
            } 

            //return response;
            return Json(new Response {IsSuccess=isOK, Action = "", Result = response,Message=sMensaje }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult VerificaSesion()
        {

            String response = "";
            Boolean isOK = true;

            string sValor = Convert.ToString(Session["bValidado"]);
            string sRUC = Convert.ToString(Session["sRUCEmisor"]);
            string sDocumento = Convert.ToString(Session["sDocumentoPDF"]);
            string sURL_DTE = ConfigurationManager.AppSettings["URLDTE"].ToString();
            string sMensaje = "";
            if (sValor == "0")
            {
                isOK = false;
                sMensaje = "La Session ha vencido. Primero debe realizar la consulta.";
            }
            if (sRUC == "")
            {
                isOK = false;
                sMensaje = "La Session ha vencido. Primero debe realizar la consulta.";
            }
            if (sDocumento == "")
            {
                isOK = false;
                sMensaje = "La Session ha vencido. Primero debe realizar la consulta.";
            }

            return Json(new Response {IsSuccess=isOK, Action = "", Result = response,Message=sMensaje }, JsonRequestBehavior.AllowGet);
        }
        public FileResult DownloadPDF(string FileName)
        {
            ////Session.Timeout= Session.Timeout + 20;
            //var FileVirtualPath = "~/DTE/20518000951/" + Session["sDocumentoPDF"];
            //return File(FileVirtualPath, "application/force~download", Path.GetFileName(FileVirtualPath));
            string sValor = Convert.ToString(Session["bValidado"]);
            string sRUC = Convert.ToString(Session["sRUCEmisor"]);
            string sDocumento = Convert.ToString(Session["sDocumentoPDF"]);
            string sURL_DTE = ConfigurationManager.AppSettings["URLDTE"].ToString();
            string strURL = "";
           
            if (!sRUC.Equals(""))
            {
                if (sValor.Equals("1"))
                {

                    try
                    {
                        //string strURL = @"http://comprobantes.helpdesk.com.pe/dte/" + sRUC + "/" + sDocumento;

                        //strURL = @sURL_DTE + sRUC + "/" + sDocumento;
                        //var FileVirtualPath="~/DTE/" + sRUC + "/" + sDocumento;
                        strURL = "~/DTE/" + sRUC + "/" + sDocumento;

                        //HttpResponse response =HttpContext.Current.Response;
                        //response.Clear();
                        //response.ClearContent();
                        //response.ClearHeaders();
                        //response.Buffer = true;
                        //response.AddHeader("Content-Disposition", "attachment;filename=\"" + sDocumento + "\"");
                        //byte[] data = req.DownloadData(strURL);
                        //response.BinaryWrite(data);
                        //response.End();
                        //DownloadPDF(strURL);
                        //WebClient req = new WebClient();


                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            //else
            //{
            //    ViewBag.Message = "Primero debe hacer la busqueda o ya se termino su sesion.";

            //}
            return File(strURL, "application/force~download", Path.GetFileName(strURL));
        }
        public FileResult DownloadXML (string rutaArchivo)
        {
            string sValor = Convert.ToString(Session["bValidado"]);
            string sRUC = Convert.ToString(Session["sRUCEmisor"]);
            string sDocumento = Convert.ToString(Session["sDocumentoXML"]);
            string sURL_DTE = ConfigurationManager.AppSettings["URLDTE"].ToString();
            string strURL = "";

            if (!sRUC.Equals(""))
            {
                if (sValor.Equals("1"))
                {
                    try
                    {
                        strURL = "~/DTE/" + sRUC + "/" + sDocumento;
                        //WebClient req = new WebClient();
                        //HttpResponse response = HttpContext.Current.Response;
                        //response.Clear();
                        //response.ClearContent();
                        //response.ClearHeaders();
                        //response.Buffer = true;
                        //response.AddHeader("Content-Disposition", "attachment;filename=\"" + sDocumento + "\"");
                        //byte[] data = req.DownloadData(strURL);
                        //response.BinaryWrite(data);
                        //response.End();

                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            return File(strURL, "application/force~download", Path.GetFileName(strURL));
        }

        [HttpPost]
        public JsonResult ConsultaSunatAnterior(ComprobanteViewModel objSunat)
        {
            String response = "";
            Boolean isOK = false;
            //objSunat.NumeroRuc = "20518000951";
            //objSunat.CodigoComp = "01";
            //objSunat.CodigoComp = "FAC";
            //objSunat.NumeroSerie = "F001";
            //objSunat.Numero = "1";
            //objSunat.FechaEmision = "05/01/2018";
            //objSunat.Monto = "298018.500";

            var fec = Convert.ToDateTime(objSunat.FechaEmision).ToString("dd/MM/yyyy");
            

            string sRutaPDF;
            sRUCEmisor = objSunat.NumeroRuc;

            string sURL_DTE = ConfigurationManager.AppSettings["URLDTE"].ToString();
            string ruta_DTE = Server.MapPath("~/DTE");
            Session["sRUCEmisor"] = sRUCEmisor;

            bool bExisteRuc = false;
            string sMensaje = "";

            string sFechaEmi = fec;
            string sTipoDoc = objSunat.CodigoComp;
            string sSerieDoc = objSunat.NumeroSerie;
            string sNroDoc = objSunat.Numero;
            string sMontoDoc = objSunat.Monto;
            DateTime dFechaValida;
            bool bFechaValida = false;
            bool bTodoOK = true;
            double dMonto;
            bool bMontoValida = false;

            try
            {
                bFechaValida = DateTime.TryParse(sFechaEmi, out dFechaValida);
                sFechaEmi = dFechaValida.ToShortDateString();
                //txtFechaEmision.Text = sFechaEmi;
            }
            catch
            { bFechaValida = false; }
            try
            {
                bMontoValida = double.TryParse(sMontoDoc, out dMonto);
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

                        sRutaPDF = sURL_DTE + sRUCEmisor + "/" + sDocumentoPDF;
                        //iframePDF.Attributes.Add("src", sRutaPDF);
                        response = sRutaPDF;
                        ruta_DTE = ruta_DTE + sRUCEmisor + "/" + sDocumentoPDF;

                        response = sRutaPDF;
                    }
                    else
                    {
                        sMensaje = "Documento no Existe o datos ingresados no corresponden. !Rectifique valores de búsqueda.!";
                        //iframePDF.Attributes.Add("src", "verMensajeError.aspx?Mensaje=" + sMensaje);
                        response = "";
                    }
                }
                else
                    isOK = false;
                    response= ""; //iframePDF.Attributes.Add("src", "verMensajeError.aspx?Mensaje=" + sMensaje);
            }
            else
            //iframePDF.Attributes.Add("src", "verMensajeError.aspx?Mensaje=" + sMensaje);
                isOK = false;
                response = "";

            //return response;
            return Json(new Response { Action = "", Result = response }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}