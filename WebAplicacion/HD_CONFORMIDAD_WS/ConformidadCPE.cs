using RestSharp;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace HD_CONFORMIDAD_WS
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("HD_CONFORMIDAD_WS.ConformidadCPE")]

       
    public class ConformidadCPE : ServicedComponent
    {
        //PLATAFORMA CONFORMIDAD 
        public string ps_verificaHD = "HD-20220430-11:00";

        public string GeneraToken()
        {
            var grant_type = "password";
            var scope = "https://api.sunat.gob.pe";
            var request = (HttpWebRequest)WebRequest.Create("https://api-seguridad.sunat.gob.pe/v1/clientessol/" + this.Client_id + "/oauth2/token/");
            System.Net.ServicePointManager.SecurityProtocol =  SecurityProtocolType.Tls | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

            var postData = "grant_type=" + grant_type + "&scope=" + scope + "&client_id=" + this.Client_id + "&client_secret=" + this.Client_secret + "&username=" + this.Username + "&password=" + this.Password;
            var data = Encoding.ASCII.GetBytes(postData);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            string resp = "";
            try
            {
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                string responseString = "";
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null)
                        {
                            return "Error: " + resp;
                        }

                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            responseString = objReader.ReadToEnd();
                        }
                    }
                }

                var token = new JavaScriptSerializer().Deserialize<dynamic>(responseString);
                resp = token["access_token"];
               
            }
            catch (ArgumentException ae)
            {
                resp = "Error: " + ae.Message;
            }
            catch (ProtocolViolationException pve)
            {
                resp = "Error: " + pve.Message;
            }
            catch (InvalidOperationException ioe)
            {
                resp = "Error: " + ioe.Message;
            }
            catch (NotSupportedException nse)
            {
                resp = "Error: " + nse.Message;
            }

            this.Token = resp;

            return resp;

        }
        public string ConsultaComprobantesPND(DateTime FecIniPND, DateTime FecFinPND, string codMonedaPND, string cod_estado, string ind_contribuyente, string numCpe, string numRuc)
        {
            string cnumpag = "1";

            string FecIni = FecIniPND.ToString("yyyy-MM-dd");
            string FecFin = FecFinPND.ToString("yyyy-MM-dd");

            //var client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/controlcpe/comprobantes?indFechaFiltro=FE&codCpe=01&fecInicio=" + FecIniPND + "&fecFin=" + FecFinPND + "&numPag=1&numRegPag=50&codestado="+ cod_estado + "&codTipTransaccion=&codMoneda=" + codMonedaPND + "&numSerie=&numCpe=&numRuc=&codTipoDocAdqui=&numDocAdqui=&indContribuyente="+ ind_contribuyente);
            var client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/controlcpe/comprobantes?indFechaFiltro=FE&codCpe=01&fecInicio=" + FecIni + "&fecFin=" + FecFin + "&numPag="+ cnumpag + "&numRegPag=100&codEstado=" + cod_estado + "&codTipTransaccion=&codMoneda=" + codMonedaPND + "&numSerie=&numCpe=" + numCpe + "&numRuc=" + numRuc + "&codTipoDocAdqui=&numDocAdqui=&indContribuyente=" + ind_contribuyente);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer " + this.Token);
            IRestResponse response = client.Execute(request);
            StringBuilder cadenaTot = new StringBuilder();
            StringBuilder cadena = new StringBuilder();

            try
            { 
                do
                {
                    cadena.Clear();
                    var obj1 = new JavaScriptSerializer().Deserialize<dynamic>(response.Content);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        foreach (var item in obj1["comprobantes"])
                        {
                            cadena.Append(item["codCpe"] + "|");
                            cadena.Append(item["numSerie"] + "|");
                            cadena.Append(item["numCpe"] + "|");
                            cadena.Append(item["codMoneda"] + "|");

                            foreach (var item2 in item["datosEmisor"])
                            {
                                if (item2.Key == "numRuc")
                                {
                                    cadena.Append(item2.Value + "|");
                                }
                                if (item2.Key == "desRazonSocialEmis")
                                {
                                    cadena.Append(item2.Value + "|");
                                }

                            }


                            foreach (var item3 in item["datosReceptor"])
                            {
                                if (item3.Key == "numDocIdeRecep")
                                {
                                    cadena.Append(item3.Value + "|");

                                }
                                if (item3.Key == "desRazonSocialRecep")
                                {
                                    cadena.Append(item3.Value + "|");

                                }

                            }

                            foreach (var item4 in item["procedenciaIndividual"])
                            {
                                if (item4.Key == "mtoImporteTotal")
                                {
                                    cadena.Append(Convert.ToDouble(item4.Value) + "|");
                                }
                            }

                            foreach (var item5 in item["procedenciaMasiva"])
                            {
                                if (item5.Key == "mtoImporteTotal")
                                {
                                    cadena.Append(Convert.ToDouble(item5.Value) + "|");
                                }
                            }

                            foreach (var item6 in item["transacciones"])
                            {
                                cadena.Append(Convert.ToDouble(item6["mtoPagoPendiente"]) + "|");

                                cadena.Append(Convert.ToInt32(item6["numCuotas"]) + "|");

                            }

                            cadena.Append(Convert.ToDateTime(item["fecEmision"]) + "|");
                            cadena.Append(Convert.ToDateTime(item["fecVencimiento"]) + "|");
                            cadena.Append(Convert.ToDateTime(item["fecRegistro"]) + "|");
                            cadena.Append(item["codTipTransaccion"] + "|");

                            var fecReg = Convert.ToDateTime(item["fecRegistro"]) - DateTime.Now;
                            var numDias = item["numDiasAtencion"];

                            int diasRest = 0;
                            if (numDias + fecReg.Days < 0)
                            {
                                diasRest = 0;
                            }
                            else
                            {
                                diasRest = numDias + fecReg.Days;
                            }


                            cadena.Append(diasRest + "|");
                            cadena.Append(item["indEstadoCpe"] + "|");
                            cadena.Append(Environment.NewLine);


                        }


                    }
                    else
                    {
                        cadena.Append(obj1["cod"] + "|");
                        cadena.Append(obj1["msg"] + "|");
                        cadena.Append(obj1["exc"] + "|");

                        foreach (var error in obj1["errors"])
                        {

                            cadena.Append(error["codError"] + "|");
                            cadena.Append(error["desError"] + "|");

                        }

                        //docPendientes1.IsOk = false;
                        //docPendientes1.MensajeError_HDS = "Error al Generar el método ConsultaComprobantesPND , avisar a sistemas -> 1 | ";
                        //docPendientesList.Add(docPendientes1);
                    }

                    cadenaTot.Append(cadena);
                    cnumpag = Convert.ToString(Convert.ToInt32(cnumpag) + 1);
                    client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/controlcpe/comprobantes?indFechaFiltro=FE&codCpe=01&fecInicio=" + FecIni + "&fecFin=" + FecFin + "&numPag="+ cnumpag + "&numRegPag=100&codEstado=" + cod_estado + "&codTipTransaccion=&codMoneda=" + codMonedaPND + "&numSerie=&numCpe=" + numCpe + "&numRuc=" + numRuc + "&codTipoDocAdqui=&numDocAdqui=&indContribuyente=" + ind_contribuyente);
                    response = client.Execute(request);

                } while (cadena.Length > 0);


                 
                return cadenaTot.ToString();

            }
            catch (ArgumentException arg)
            {
                cadena.Append(arg.Message + "|");
                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = "Error al Generar el método ConsultaComprobantesPND , avisar a sistemas -> 2 | " + ex.Message;
            }
            catch (ProtocolViolationException vio)
            {
                cadena.Append(vio.Message + "|");

                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = "Error al Generar el método ConsultaComprobantesPND , avisar a sistemas -> 3 | " + ex.Message;
            }
            catch (InvalidOperationException inv)
            {
                cadena.Append(inv.Message + "|");

                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = "Error al Generar el método ConsultaComprobantesPND , avisar a sistemas -> 4 | " + ex.Message;
            }
            catch (NotSupportedException not)
            {
                cadena.Append(not.Message + "|");

                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = "Error al Generar el método ConsultaComprobantesPND , avisar a sistemas -> 5 | " + ex.Message;
            }
            catch (Exception ex)
            {
                cadena.Append(ex.Message + "|");

                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = ex.Message;
            }

            return cadena.ToString();
        }

        public string ConsultaComprobantesPNDVE(DateTime FecIniPND, DateTime FecFinPND, string codMonedaPND, string cod_estado, string ind_contribuyente, string numCpe, string numRuc)
        {
            string cnumpag = "1";
            string FecIni = FecIniPND.ToString("yyyy-MM-dd");
            string FecFin = FecFinPND.ToString("yyyy-MM-dd");
 
            //var client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/controlcpe/comprobantes?indFechaFiltro=FE&codCpe=01&fecInicio=" + FecIniPND + "&fecFin=" + FecFinPND + "&numPag=1&numRegPag=50&codestado="+ cod_estado + "&codTipTransaccion=&codMoneda=" + codMonedaPND + "&numSerie=&numCpe=&numRuc=&codTipoDocAdqui=&numDocAdqui=&indContribuyente="+ ind_contribuyente);
            var client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/controlcpe/comprobantes?indFechaFiltro=FE&codCpe=01&fecInicio=" + FecIni + "&fecFin=" + FecFin + "&numPag="+ cnumpag + "&numRegPag=1000&codEstado=" + cod_estado + "&codTipTransaccion=&codMoneda=" + codMonedaPND + "&numSerie=&numCpe=" + numCpe + "&numRuc=&codTipoDocAdqui=&numDocAdqui="+numRuc+"&indContribuyente=" + ind_contribuyente);

            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer " + this.Token);
            IRestResponse response = client.Execute(request);
            StringBuilder cadenaTot = new StringBuilder();
            StringBuilder cadena = new StringBuilder();


            try
            {


                do
                {
                    cadena.Clear();
                    var obj1 = new JavaScriptSerializer().Deserialize<dynamic>(response.Content);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        foreach (var item in obj1["comprobantes"])
                        {
                            cadena.Append(item["codCpe"] + "|");
                            cadena.Append(item["numSerie"] + "|");
                            cadena.Append(item["numCpe"] + "|");
                            cadena.Append(item["codMoneda"] + "|");

                            foreach (var item2 in item["datosEmisor"])
                            {
                                if (item2.Key == "numRuc")
                                {
                                    cadena.Append(item2.Value + "|");
                                }
                                if (item2.Key == "desRazonSocialEmis")
                                {
                                    cadena.Append(item2.Value + "|");
                                }

                            }


                            foreach (var item3 in item["datosReceptor"])
                            {
                                if (item3.Key == "numDocIdeRecep")
                                {
                                    cadena.Append(item3.Value + "|");

                                }
                                if (item3.Key == "desRazonSocialRecep")
                                {
                                    cadena.Append(item3.Value + "|");

                                }

                            }

                            foreach (var item4 in item["procedenciaIndividual"])
                            {
                                if (item4.Key == "mtoImporteTotal")
                                {
                                    cadena.Append(Convert.ToDouble(item4.Value) + "|");
                                }
                            }

                            foreach (var item5 in item["procedenciaMasiva"])
                            {
                                if (item5.Key == "mtoImporteTotal")
                                {
                                    cadena.Append(Convert.ToDouble(item5.Value) + "|");
                                }
                            }

                            foreach (var item6 in item["transacciones"])
                            {
                                cadena.Append(Convert.ToDouble(item6["mtoPagoPendiente"]) + "|");

                                cadena.Append(Convert.ToInt32(item6["numCuotas"]) + "|");

                            }

                            cadena.Append(Convert.ToDateTime(item["fecEmision"]) + "|");
                            cadena.Append(Convert.ToDateTime(item["fecVencimiento"]) + "|");
                            cadena.Append(Convert.ToDateTime(item["fecRegistro"]) + "|");
                            cadena.Append(item["codTipTransaccion"] + "|");

                            var fecReg = Convert.ToDateTime(item["fecRegistro"]) - DateTime.Now;
                            var numDias = item["numDiasAtencion"];

                            int diasRest = 0;
                            if (numDias + fecReg.Days < 0)
                            {
                                diasRest = 0;
                            }
                            else
                            {
                                diasRest = numDias + fecReg.Days;
                            }


                            cadena.Append(diasRest + "|");

                            //cadena.Append(item["numDiasAtencion"] + "|");

                            cadena.Append(item["indEstadoCpe"] + "|");
                            cadena.Append(Environment.NewLine);

                            //docPendientes1.IsOk = true;


                            //docPendientesList.Add(docPendientes1);


                        }


                    }
                    else
                    {
                        cadena.Append(obj1["cod"] + "|");
                        cadena.Append(obj1["msg"] + "|");
                        cadena.Append(obj1["exc"] + "|");

                        foreach (var error in obj1["errors"])
                        {

                            cadena.Append(error["codError"] + "|");
                            cadena.Append(error["desError"] + "|");

                        }

                        //docPendientes1.IsOk = false;
                        //docPendientes1.MensajeError_HDS = "Error al Generar el método ConsultaComprobantesPND , avisar a sistemas -> 1 | ";
                        //docPendientesList.Add(docPendientes1);
                    }

                    cadenaTot.Append(cadena);
                    cnumpag = Convert.ToString(Convert.ToInt32(cnumpag) + 1);
                    client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/controlcpe/comprobantes?indFechaFiltro=FE&codCpe=01&fecInicio=" + FecIni + "&fecFin=" + FecFin + "&numPag="+ cnumpag + "&numRegPag=1000&codEstado=" + cod_estado + "&codTipTransaccion=&codMoneda=" + codMonedaPND + "&numSerie=&numCpe=" + numCpe + "&numRuc=&codTipoDocAdqui=&numDocAdqui=" + numRuc + "&indContribuyente=" + ind_contribuyente);
                    response = client.Execute(request);

                } while (cadena.Length > 0);



                return cadenaTot.ToString();

            }
            catch (ArgumentException arg)
            {
                cadena.Append(arg.Message + "|");
                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = "Error al Generar el método ConsultaComprobantesPND , avisar a sistemas -> 2 | " + ex.Message;
            }
            catch (ProtocolViolationException vio)
            {
                cadena.Append(vio.Message + "|");

                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = "Error al Generar el método ConsultaComprobantesPND , avisar a sistemas -> 3 | " + ex.Message;
            }
            catch (InvalidOperationException inv)
            {
                cadena.Append(inv.Message + "|");

                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = "Error al Generar el método ConsultaComprobantesPND , avisar a sistemas -> 4 | " + ex.Message;
            }
            catch (NotSupportedException not)
            {
                cadena.Append(not.Message + "|");

                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = "Error al Generar el método ConsultaComprobantesPND , avisar a sistemas -> 5 | " + ex.Message;
            }
            catch (Exception ex)
            {
                cadena.Append(ex.Message + "|");

                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = ex.Message;
            }

            return cadena.ToString();
        }

        public string ConsultaComprobantesConf( DateTime FecIniPND, DateTime FecFinPND, string codMonedaPND, string cod_estado, string ind_contribuyente , string numCpe , string numRuc , string codAdqui)
        {
            string FecIni = FecIniPND.ToString("yyyy-MM-dd");
            string FecFin = FecFinPND.ToString("yyyy-MM-dd");
            string cnumpag="1";
            //List<DocPendientes> docPendientesList = new List<DocPendientes>();
            //DocPendientes docPendientes1 = new DocPendientes();
            //var client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/controlcpe/comprobantes?indFechaFiltro=FE&codCpe=01&fecInicio=" + FecIniPND + "&fecFin=" + FecFinPND + "&numPag=1&numRegPag=50&codestado="+ cod_estado + "&codTipTransaccion=&codMoneda=" + codMonedaPND + "&numSerie=&numCpe=&numRuc=&codTipoDocAdqui=&numDocAdqui=&indContribuyente="+ ind_contribuyente);
            var client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/controlcpe/comprobantes?indFechaFiltro=FE&codCpe=01&fecInicio="+ FecIni + "&fecFin="+ FecFin+ "&numPag=" +cnumpag+"&numRegPag=50&codEstado="+ cod_estado+"&codTipTransaccion=&codMoneda="+ codMonedaPND+"&numSerie=&numCpe="+ numCpe +"&numRuc="+numRuc+"&codTipoDocAdqui="+codAdqui+"&numDocAdqui=&indContribuyente="+ ind_contribuyente);
            //var client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/controlcpe/comprobantes?indfechafiltro=fe&codcpe=00&fecinicio=" + FecIniPND + "&fecfin=" + FecFinPND + "&numpag=1&numregpag=50&codestado=" + cod_estado + "&codtiptransaccion=&codmoneda=" + codMonedaPND + "&numserie=&numcpe=" + numCpe + "&numruc=" + numRuc + "&codtipodocadqui=" + codAdqui + "&numdocadqui=&indcontribuyente=" + ind_contribuyente);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer " + this.Token);
            IRestResponse response = client.Execute(request);

            StringBuilder cadenaTot = new StringBuilder();
            StringBuilder cadena = new StringBuilder();

            try
            {

                
                do
                {
                    cadena.Clear();
                    var obj1 = new JavaScriptSerializer().Deserialize<dynamic>(response.Content);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        foreach (var item in obj1["comprobantes"])
                        {
                            cadena.Append(item["codCpe"] + "|");
                            cadena.Append(item["numSerie"] + "|");
                            cadena.Append(item["numCpe"] + "|");
                            cadena.Append(item["codMoneda"] + "|");

                            foreach (var item2 in item["datosEmisor"])
                            {
                                if (item2.Key == "numRuc")
                                {
                                    cadena.Append(item2.Value + "|");
                                }
                                if (item2.Key == "desRazonSocialEmis")
                                {
                                    cadena.Append(item2.Value + "|");
                                }

                            }


                            foreach (var item3 in item["datosReceptor"])
                            {
                                if (item3.Key == "numDocIdeRecep")
                                {
                                    cadena.Append(item3.Value + "|");

                                }
                                if (item3.Key == "desRazonSocialRecep")
                                {
                                    cadena.Append(item3.Value + "|");

                                }

                            }

                            foreach (var item4 in item["procedenciaIndividual"])
                            {
                                if (item4.Key == "mtoImporteTotal")
                                {
                                    cadena.Append(Convert.ToDouble(item4.Value) + "|");
                                }
                            }

                            foreach (var item5 in item["procedenciaMasiva"])
                            {
                                if (item5.Key == "mtoImporteTotal")
                                {
                                    cadena.Append(Convert.ToDouble(item5.Value) + "|");
                                }
                            }

                            foreach (var item6 in item["transacciones"])
                            {
                                cadena.Append(Convert.ToDouble(item6["mtoPagoPendiente"]) + "|");

                                cadena.Append(Convert.ToInt32(item6["numCuotas"]) + "|");

                            }

                            cadena.Append(Convert.ToDateTime(item["fecEmision"]) + "|");
                            cadena.Append(Convert.ToDateTime(item["fecVencimiento"]) + "|");
                            cadena.Append(Convert.ToDateTime(item["fecRegistro"]) + "|");
                            cadena.Append(item["codTipTransaccion"] + "|");

                            var fecReg = Convert.ToDateTime(item["fecRegistro"]) - DateTime.Now;
                            var numDias = item["numDiasAtencion"];

                            int diasRest = 0;
                            if (numDias + fecReg.Days < 0)
                            {
                                diasRest = 0;
                            }
                            else
                            {
                                diasRest = numDias + fecReg.Days;
                            }


                            cadena.Append(diasRest + "|");

                            //cadena.Append(item["numDiasAtencion"] + "|");

                            cadena.Append(item["indEstadoCpe"] + "|");
                            cadena.Append(Environment.NewLine);

                            //docPendientes1.IsOk = true;


                            //docPendientesList.Add(docPendientes1);


                        }


                    }
                    else
                    {
                        cadena.Append(obj1["cod"] + "|");
                        cadena.Append(obj1["msg"] + "|");
                        cadena.Append(obj1["exc"] + "|");

                        foreach (var error in obj1["errors"])
                        {

                            cadena.Append(error["codError"] + "|");
                            cadena.Append(error["desError"] + "|");

                        }

                        //docPendientes1.IsOk = false;
                        //docPendientes1.MensajeError_HDS = "Error al Generar el método ConsultaComprobantesPND , avisar a sistemas -> 1 | ";
                        //docPendientesList.Add(docPendientes1);
                    }

                    cadenaTot.Append(cadena);
                    cnumpag = Convert.ToString(Convert.ToInt32(cnumpag) + 1);
                    client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/controlcpe/comprobantes?indFechaFiltro=FE&codCpe=01&fecInicio=" + FecIni + "&fecFin=" + FecFin + "&numPag=" + cnumpag + "&numRegPag=50&codEstado=" + cod_estado + "&codTipTransaccion=&codMoneda=" + codMonedaPND + "&numSerie=&numCpe=" + numCpe + "&numRuc=" + numRuc + "&codTipoDocAdqui=" + codAdqui + "&numDocAdqui=&indContribuyente=" + ind_contribuyente);
                    response = client.Execute(request);

                } while (cadena.Length>0);



                return cadenaTot.ToString();

            }
            catch (ArgumentException arg)
            {
                cadena.Append(arg.Message + "|");
                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = "Error al Generar el método ConsultaComprobantesPND , avisar a sistemas -> 2 | " + ex.Message;
            }
            catch (ProtocolViolationException vio)
            {
                cadena.Append(vio.Message + "|");

                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = "Error al Generar el método ConsultaComprobantesPND , avisar a sistemas -> 3 | " + ex.Message;
            }
            catch (InvalidOperationException inv)
            {
                cadena.Append(inv.Message + "|");

                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = "Error al Generar el método ConsultaComprobantesPND , avisar a sistemas -> 4 | " + ex.Message;
            }
            catch (NotSupportedException not)
            {
                cadena.Append(not.Message + "|");

                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = "Error al Generar el método ConsultaComprobantesPND , avisar a sistemas -> 5 | " + ex.Message;
            }
            catch (Exception ex)
            {
                cadena.Append(ex.Message + "|");

                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = ex.Message;
            }

            return cadena.ToString();
        }

        public string ConsultaComprobantesConfVE(DateTime FecIniPND, DateTime FecFinPND, string codMonedaPND, string cod_estado, string ind_contribuyente, string numCpe, string numRuc, string codAdqui)
        {
            string FecIni = FecIniPND.ToString("yyyy-MM-dd");
            string FecFin = FecFinPND.ToString("yyyy-MM-dd");
            string cnumpag = "1";
            //var client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/controlcpe/comprobantes?indFechaFiltro=FE&codCpe=01&fecInicio=" + FecIniPND + "&fecFin=" + FecFinPND + "&numPag=1&numRegPag=50&codestado="+ cod_estado + "&codTipTransaccion=&codMoneda=" + codMonedaPND + "&numSerie=&numCpe=&numRuc=&codTipoDocAdqui=&numDocAdqui=&indContribuyente="+ ind_contribuyente);
            var client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/controlcpe/comprobantes?indFechaFiltro=FE&codCpe=01&fecInicio="+ FecIni + "&fecFin="+ FecFin + "&numPag="+ cnumpag + "&numRegPag=&codEstado="+ cod_estado+"&codTipTransaccion=&codMoneda="+ codMonedaPND+"&numSerie=&numCpe="+ numCpe +"&numRuc=&codTipoDocAdqui="+codAdqui+"&numDocAdqui="+ numRuc + "&indContribuyente="+ ind_contribuyente);
           // var client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/controlcpe/comprobantes?indfechafiltro=fe&codcpe=00&fecinicio=" + FecIni + "&fecfin=" + FecFin + "&numpag="+ cnumpag + "&numregpag=100&codestado=" + cod_estado + "&codtiptransaccion=&codmoneda=" + codMonedaPND + "&numserie=&numcpe=" + numCpe + "&numruc=&codtipodocadqui=" + codAdqui + "&numdocadqui="+numRuc+ "&indContribuyente=" + ind_contribuyente);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer " + this.Token);
            IRestResponse response = client.Execute(request);

            StringBuilder cadenaTot = new StringBuilder();
            StringBuilder cadena = new StringBuilder();

                try
            {


                do
                {
                    cadena.Clear();
                    var obj1 = new JavaScriptSerializer().Deserialize<dynamic>(response.Content);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        foreach (var item in obj1["comprobantes"])
                        {
                            cadena.Append(item["codCpe"] + "|");
                            cadena.Append(item["numSerie"] + "|");
                            cadena.Append(item["numCpe"] + "|");
                            cadena.Append(item["codMoneda"] + "|");

                            foreach (var item2 in item["datosEmisor"])
                            {
                                if (item2.Key == "numRuc")
                                {
                                    cadena.Append(item2.Value + "|");
                                }
                                if (item2.Key == "desRazonSocialEmis")
                                {
                                    cadena.Append(item2.Value + "|");
                                }

                            }


                            foreach (var item3 in item["datosReceptor"])
                            {
                                if (item3.Key == "numDocIdeRecep")
                                {
                                    cadena.Append(item3.Value + "|");

                                }
                                if (item3.Key == "desRazonSocialRecep")
                                {
                                    cadena.Append(item3.Value + "|");

                                }

                            }

                            foreach (var item4 in item["procedenciaIndividual"])
                            {
                                if (item4.Key == "mtoImporteTotal")
                                {
                                    cadena.Append(Convert.ToDouble(item4.Value) + "|");
                                }
                            }

                            foreach (var item5 in item["procedenciaMasiva"])
                            {
                                if (item5.Key == "mtoImporteTotal")
                                {
                                    cadena.Append(Convert.ToDouble(item5.Value) + "|");
                                }
                            }

                            foreach (var item6 in item["transacciones"])
                            {
                                cadena.Append(Convert.ToDouble(item6["mtoPagoPendiente"]) + "|");

                                cadena.Append(Convert.ToInt32(item6["numCuotas"]) + "|");

                            }

                            cadena.Append(Convert.ToDateTime(item["fecEmision"]) + "|");
                            cadena.Append(Convert.ToDateTime(item["fecVencimiento"]) + "|");
                            cadena.Append(Convert.ToDateTime(item["fecRegistro"]) + "|");
                            cadena.Append(item["codTipTransaccion"] + "|");

                            var fecReg = Convert.ToDateTime(item["fecRegistro"]) - DateTime.Now;
                            var numDias = item["numDiasAtencion"];

                            int diasRest = 0;
                            if (numDias + fecReg.Days < 0)
                            {
                                diasRest = 0;
                            }
                            else
                            {
                                diasRest = numDias + fecReg.Days;
                            }


                            cadena.Append(diasRest + "|");

                            //cadena.Append(item["numDiasAtencion"] + "|");

                            cadena.Append(item["indEstadoCpe"] + "|");
                            cadena.Append(Environment.NewLine);

                            //docPendientes1.IsOk = true;


                            //docPendientesList.Add(docPendientes1);


                        }


                    }
                    else
                    {
                        cadena.Append(obj1["cod"] + "|");
                        cadena.Append(obj1["msg"] + "|");
                        cadena.Append(obj1["exc"] + "|");

                        foreach (var error in obj1["errors"])
                        {

                            cadena.Append(error["codError"] + "|");
                            cadena.Append(error["desError"] + "|");

                        }

                        //docPendientes1.IsOk = false;
                        //docPendientes1.MensajeError_HDS = "Error al Generar el método ConsultaComprobantesPND , avisar a sistemas -> 1 | ";
                        //docPendientesList.Add(docPendientes1);
                    }

                    cadenaTot.Append(cadena);
                    cnumpag = Convert.ToString(Convert.ToInt32(cnumpag) + 1);
                     client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/controlcpe/comprobantes?indFechaFiltro=FE&codCpe=01&fecInicio=" + FecIni + "&fecFin=" + FecFin + "&numPag="+ cnumpag + "&numRegPag=&codEstado=" + cod_estado + "&codTipTransaccion=&codMoneda=" + codMonedaPND + "&numSerie=&numCpe=" + numCpe + "&numRuc=&codTipoDocAdqui=" + codAdqui + "&numDocAdqui="+ numRuc + "&indContribuyente=" + ind_contribuyente);
                    response = client.Execute(request);

                } while (cadena.Length > 0);



                return cadenaTot.ToString();

            }
            catch (ArgumentException arg)
            {
                cadena.Append(arg.Message + "|");
                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = "Error al Generar el método ConsultaComprobantesPND , avisar a sistemas -> 2 | " + ex.Message;
            }
            catch (ProtocolViolationException vio)
            {
                cadena.Append(vio.Message + "|");

                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = "Error al Generar el método ConsultaComprobantesPND , avisar a sistemas -> 3 | " + ex.Message;
            }
            catch (InvalidOperationException inv)
            {
                cadena.Append(inv.Message + "|");

                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = "Error al Generar el método ConsultaComprobantesPND , avisar a sistemas -> 4 | " + ex.Message;
            }
            catch (NotSupportedException not)
            {
                cadena.Append(not.Message + "|");

                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = "Error al Generar el método ConsultaComprobantesPND , avisar a sistemas -> 5 | " + ex.Message;
            }
            catch (Exception ex)
            {
                cadena.Append(ex.Message + "|");

                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = ex.Message;
            }

            return cadena.ToString();
        }

        public string EnviaConformidad(string archivo)
        {
            RestClient restClient = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/controlcpe/enviosmasivo/01");
            restClient.Timeout = -1;
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer " + this.Token);
            request.AddFile("archivo", archivo);
            IRestResponse restResponse = restClient.Execute((IRestRequest)request);
            StringBuilder cadena = new StringBuilder();


            Repuesta_Conformidad objres = new Repuesta_Conformidad();
            try
            {
                var obj1 = new JavaScriptSerializer().Deserialize<dynamic>(restResponse.Content);
                if (restResponse.StatusCode == HttpStatusCode.OK)
                {
                   
                    cadena.Append(obj1["codOperacion"] + "|");
                    cadena.Append(obj1["desRespuesta"]+ "|");
                    cadena.Append(obj1["codTicketMasivo"] + "|");
                    cadena.Append(Environment.NewLine);



                }
                else
                {
                    cadena.Append(obj1["cod"] + "|");
                    cadena.Append(obj1["msg"] + "|");
                    cadena.Append(obj1["exc"] + "|");

                    foreach (var error in obj1["errors"])
                    {

                        cadena.Append(error["codError"]);
                        cadena.Append(error["desError"]);

                    }

                    //objres.IsOk = false;
                    //objres.MensajeError_HDS = "Error al Generar el método EnviaConformidad , avisar a sistemas -> 1 | ";
                 
                }

                return cadena.ToString();

            }

            catch (ArgumentException arg)
            {
                //objres.IsOk = false;
                //objres.MensajeError_HDS = "Error al Generar el método EnviaConformidad , avisar a sistemas -> 2 | " + ex.Message;
                cadena.Append(arg.Message + "|");
            }
            catch (ProtocolViolationException viola)
            {
                //objres.IsOk = false;
                //objres.MensajeError_HDS = "Error al Generar el método EnviaConformidad , avisar a sistemas -> 3 | " + ex.Message;
                cadena.Append(viola.Message + "|");
            }
            catch (InvalidOperationException inva)
            {
                //objres.IsOk = false;
                //objres.MensajeError_HDS = "Error al Generar el método EnviaConformidad , avisar a sistemas -> 4 | " + ex.Message;
                cadena.Append(inva.Message + "|");
            }
            catch (NotSupportedException not)
            {
                //objres.IsOk = false;
                //objres.MensajeError_HDS = "Error al Generar el método EnviaConformidad , avisar a sistemas -> 5 | " + ex.Message;
                cadena.Append(not.Message + "|");
            }
            catch (Exception ex)
            {
                //objres.IsOk = false;
                //objres.MensajeError_HDS = ex.Message;
                cadena.Append(ex.Message + "|");
            }

            return cadena.ToString();


        }


        public string ConsultaTicketsPND(DateTime fecEnvioInicial,DateTime fecEnvioFinal,string codTicketMasivo)
        {
            string FecIni = fecEnvioInicial.ToString("yyyy-MM-dd");
            string FecFin = fecEnvioFinal.ToString("yyyy-MM-dd");

            var client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/controlcpe/enviosmasivo?indTipoRegistro=01&fecEnvioInicial=" + FecIni + "&fecEnvioFinal=" + FecFin + "&numPag=1&numRegPag=50&codTicketMasivo=" + codTicketMasivo);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer " + this.Token);
            IRestResponse response = client.Execute(request);

            StringBuilder cadena = new StringBuilder();

            try
            {
                var obj1 = new JavaScriptSerializer().Deserialize<dynamic>(response.Content);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    foreach (var item in obj1["registros"])
                    {
                        cadena.Append(item["indTipoRegistro"]+ "|");
                        cadena.Append(item["codTicketMasivo"]+"|");
                        cadena.Append(item["indEstado"] + "|");
                        cadena.Append(item["desArchivo"] + "|");
                        cadena.Append(Convert.ToDateTime(item["fecRegis"])+"|");
                        cadena.Append(Convert.ToDateTime(item["fecInicioProceso"]) + "|");
                        cadena.Append(Convert.ToDateTime(item["fecFinProceso"]) + "|");
                        cadena.Append(item["cntCpe"] + "|");
                        cadena.Append(item["cntCorrectos"] + "|");
                        cadena.Append(item["cntErrores"] + "|");
                       //var arError = item["arcErrores"];

                       // if (!string.IsNullOrEmpty(item["arcErrores"])  )
                       // {
                       //     byte[] bytes = Convert.FromBase64String(arError);
                       //     arError = Encoding.ASCII.GetString(bytes);
                       // }

                       // cadena.Append(arError + "|");

                        cadena.Append(Environment.NewLine);


                    }


                }
                else
                {
                    cadena.Append(obj1["cod"]+"|");
                    cadena.Append(obj1["msg"]+ "|");
                    cadena.Append(obj1["exc"] + "|");

                    foreach (var error in obj1["errors"])
                    {

                        cadena.Append(error["codError"]+"|");
                        cadena.Append(error["desError"] + "|");

                    }

                    //objTicket.IsOk = false;
                    //objTicket.MensajeError_HDS = "Error al Generar el método ConsultaTicketsPND , avisar a sistemas -> 1 | ";
                    //tickPendientesList.Add(objTicket);
                }


                return cadena.ToString();
 

            }
            catch (ArgumentException arg)
            {
                cadena.Append(arg.Message + "|");
                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = "Error al Generar el método ConsultaComprobantesPND , avisar a sistemas -> 2 | " + ex.Message;
            }
            catch (ProtocolViolationException vio)
            {
                cadena.Append(vio.Message + "|");

                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = "Error al Generar el método ConsultaComprobantesPND , avisar a sistemas -> 3 | " + ex.Message;
            }
            catch (InvalidOperationException inv)
            {
                cadena.Append(inv.Message + "|");

                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = "Error al Generar el método ConsultaComprobantesPND , avisar a sistemas -> 4 | " + ex.Message;
            }
            catch (NotSupportedException not)
            {
                cadena.Append(not.Message + "|");

                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = "Error al Generar el método ConsultaComprobantesPND , avisar a sistemas -> 5 | " + ex.Message;
            }
            catch (Exception ex)
            {
                cadena.Append(ex.Message + "|");

                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = ex.Message;
            }

            return cadena.ToString();
        }

        //PLATAFORMA RVIE
        public string Ws07_ConsultaResumenCPE(int Ps_Periodo, int Ps_TipoResumen, int Ps_TipoArchivo)
        {


            //var client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/controlcpe/comprobantes?indFechaFiltro=FE&codCpe=01&fecInicio=" + FecIniPND + "&fecFin=" + FecFinPND + "&numPag=1&numRegPag=50&codestado="+ cod_estado + "&codTipTransaccion=&codMoneda=" + codMonedaPND + "&numSerie=&numCpe=&numRuc=&codTipoDocAdqui=&numDocAdqui=&indContribuyente="+ ind_contribuyente);
            var client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/migeigv/libros/ventas/resumen/" + Ps_Periodo + "/" + Ps_TipoResumen + "/" + Ps_TipoArchivo + "/exporta");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer " + this.Token);
            IRestResponse response = client.Execute(request);
            StringBuilder cadena = new StringBuilder();


            try
            {

                cadena.Clear();

                cadena.Append(response.Content);



            }
            catch (ArgumentException arg)
            {
                cadena.Append(arg.Message + "|");

            }
            catch (ProtocolViolationException vio)
            {
                cadena.Append(vio.Message + "|");

            }
            catch (InvalidOperationException inv)
            {
                cadena.Append(inv.Message + "|");

            }
            catch (NotSupportedException not)
            {
                cadena.Append(not.Message + "|");

            }
            catch (Exception ex)
            {
                cadena.Append(ex.Message + "|");

            }

            return cadena.ToString();
        }

        //CONSULTA CON PARAMETROS
        public string WS01_ConsultaDetalleCPE(int ps_periodo, int ps_codTipoOpe, int? ps_mtoDesde, int? ps_mtoHasta,
       DateTime? ps_fecEmisionIni, DateTime? ps_fecEmisionFin,
       int? ps_numDocAdquiriente, int? ps_codCar,
       int? ps_codTipoCDP, int? ps_codInconsistencia)
        {

            string FechaEmisionInicio = "";
            int n_numpag = 1;

            if (ps_fecEmisionIni == null)
            {
                FechaEmisionInicio = "";
            }
            else
            {
                FechaEmisionInicio = Convert.ToString(ps_fecEmisionIni);
            }

            //if (string.IsNullOrEmpty(ps_fecEmisionIni))
            //{
            //    FechaEmisionInicio = "";
            //}
            //else
            //{// yyyyMMdd

            //    FechaEmisionInicio = ps_fecEmisionIni;
            //}


            //string FecIni = ps_fecEmisionIni.ToString("yyyy-MM-dd");
            string FecFin = "";// ps_fecEmisionFin.ToString("yyyy-MM-dd");
            //var client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/controlcpe/comprobantes?indFechaFiltro=FE&codCpe=01&fecInicio=" + FecIni + "&fecFin=" + FecFin + "&numPag=" + cnumpag + "&numRegPag=100&codEstado=" + cod_estado + "&codTipTransaccion=&codMoneda=" + codMonedaPND + "&numSerie=&numCpe=" + numCpe + "&numRuc=" + numRuc + "&codTipoDocAdqui=&numDocAdqui=&indContribuyente=" + ind_contribuyente);
            var client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/migeigv/libros/ventas/propuesta/" + ps_periodo + "/comprobantes?codTipoOpe=" + ps_codTipoOpe + "&mtoDesde=" + ps_mtoDesde + "&mtoHasta=" + ps_mtoHasta + "&fecEmisionIni=" + FechaEmisionInicio + "&fecEmisionFin=" + ps_fecEmisionFin + "&numDocAdquiriente=" + ps_numDocAdquiriente + "&codCar=" + ps_codCar + "&codTipoCDP=" + ps_codTipoCDP + "&codInconsistencia=" + ps_codInconsistencia + "&page=" + n_numpag + "&perPage=100");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer " + this.Token);
            IRestResponse response = client.Execute(request);
            StringBuilder cadenaTot = new StringBuilder();
            StringBuilder cadena = new StringBuilder();


            try
            {

                do
                {
                    cadena.Clear();
                    var obj1 = new JavaScriptSerializer().Deserialize<dynamic>(response.Content);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        foreach (var item in obj1["registros"])
                        {
                            cadena.Append(item["id"] + "|");
                            cadena.Append(item["numRuc"] + "|");
                            cadena.Append(item["nomRazonSocial"] + "|");
                            cadena.Append(item["perPeriodoTributario"] + "|");
                            cadena.Append(item["codCar"] + "|");
                            cadena.Append(item["codTipoCDP"] + "|");
                            cadena.Append(item["numSerieCDP"] + "|");
                            cadena.Append(item["numCDP"] + "|");
                            cadena.Append(item["codTipoCarga"] + "|");
                            cadena.Append(item["codSituacion"] + "|");
                            cadena.Append(Convert.ToDateTime(item["fecEmision"]) + "|");
                            cadena.Append(Convert.ToDateTime(item["fecVencPag"]) + "|");
                            cadena.Append(item["codTipoDocIdentidad"] + "|");
                            cadena.Append(item["numDocIdentidad"] + "|");
                            cadena.Append(item["nomRazonSocialCliente"] + "|");
                            cadena.Append(Convert.ToDouble(item["mtoValFactExpo"]) + "|");
                            cadena.Append(Convert.ToDouble(item["mtoBIGravada"]) + "|");
                            cadena.Append(Convert.ToDouble(item["mtoDsctoBI"]) + "|");
                            cadena.Append(Convert.ToDouble(item["mtoIGV"]) + "|");
                            cadena.Append(Convert.ToDouble(item["mtoDsctoIGV"]) + "|");
                            cadena.Append(Convert.ToDouble(item["mtoExonerado"]) + "|");
                            cadena.Append(Convert.ToDouble(item["mtoInafecto"]) + "|");
                            cadena.Append(Convert.ToDouble(item["mtoISC"]) + "|");
                            cadena.Append(Convert.ToDouble(item["mtoBIIvap"]) + "|");
                            cadena.Append(Convert.ToDouble(item["mtoIvap"]) + "|");
                            cadena.Append(Convert.ToDouble(item["mtoIcbp"]) + "|");
                            cadena.Append(Convert.ToDouble(item["mtoOtrosTrib"]) + "|");
                            cadena.Append(Convert.ToDouble(item["mtoTotalCP"]) + "|");
                            cadena.Append(item["codMoneda"] + "|");
                            cadena.Append(Convert.ToDouble(item["mtoTipoCambio"]) + "|");
                            cadena.Append(item["codEstadoComprobante"] + "|");
                            cadena.Append(Convert.ToDouble(item["mtoValorOpGratuitas"]) + "|");
                            cadena.Append(Convert.ToDouble(item["mtoValorFob"]) + "|");
                            cadena.Append(item["indTipoOperacion"] + "|");
                            cadena.Append(Convert.ToInt32(item["numInconsistencias"]) + "|");
                            cadena.Append(item["indEditable"] + "|");



                            cadena.Append(Environment.NewLine);


                        }


                    }
                    else
                    {
                        cadena.Append(obj1["cod"] + "|");
                        cadena.Append(obj1["msg"] + "|");
                        cadena.Append(obj1["exc"] + "|");

                        foreach (var error in obj1["errors"])
                        {

                            cadena.Append(error["codError"] + "|");
                            cadena.Append(error["desError"] + "|");

                        }

                        //docPendientes1.IsOk = false;
                        //docPendientes1.MensajeError_HDS = "Error al Generar el método ConsultaComprobantesPND , avisar a sistemas -> 1 | ";
                        //docPendientesList.Add(docPendientes1);
                    }

                    cadenaTot.Append(cadena);
                    n_numpag = n_numpag + 1;
                    //client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/controlcpe/comprobantes?indFechaFiltro=FE&codCpe=01&fecInicio=" + FecIni + "&fecFin=" + FecFin + "&numPag=" + n_numpag + "&numRegPag=100&codEstado=" + cod_estado + "&codTipTransaccion=&codMoneda=" + codMonedaPND + "&numSerie=&numCpe=" + numCpe + "&numRuc=" + numRuc + "&codTipoDocAdqui=&numDocAdqui=&indContribuyente=" + ind_contribuyente);
                    client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/migeigv/libros/ventas/propuesta/" + ps_periodo + "/comprobantes?codTipoOpe=" + ps_codTipoOpe + "&mtoDesde=" + ps_mtoDesde + "&mtoHasta=" + ps_mtoHasta + "&fecEmisionIni=" + ps_fecEmisionIni + "&fecEmisionFin=" + ps_fecEmisionFin + "&numDocAdquiriente=" + ps_numDocAdquiriente + "&codCar=" + ps_codCar + "&codTipoCDP=" + ps_codTipoCDP + "&codInconsistencia=" + ps_codInconsistencia + "&page=" + n_numpag + "&perPage=100");

                    response = client.Execute(request);

                } while (cadena.Length > 0);



                return cadenaTot.ToString();

            }
            catch (ArgumentException arg)
            {
                cadena.Append(arg.Message + "|");
                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = "Error al Generar el método ConsultaComprobantesPND , avisar a sistemas -> 2 | " + ex.Message;
            }
            catch (ProtocolViolationException vio)
            {
                cadena.Append(vio.Message + "|");

                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = "Error al Generar el método ConsultaComprobantesPND , avisar a sistemas -> 3 | " + ex.Message;
            }
            catch (InvalidOperationException inv)
            {
                cadena.Append(inv.Message + "|");

                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = "Error al Generar el método ConsultaComprobantesPND , avisar a sistemas -> 4 | " + ex.Message;
            }
            catch (NotSupportedException not)
            {
                cadena.Append(not.Message + "|");

                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = "Error al Generar el método ConsultaComprobantesPND , avisar a sistemas -> 5 | " + ex.Message;
            }
            catch (Exception ex)
            {
                cadena.Append(ex.Message + "|");

                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = ex.Message;
            }

            return cadena.ToString();
        }
        //CONSULTA SIN PARAMETROS
        public string WS01_ConsultaDetalleCPE2(int ps_periodo, int ps_codTipoOpe)
        {
            int n_numpag = 1;

            //var client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/migeigv/libros/ventas/propuesta/" + ps_periodo + "/comprobantes?codTipoOpe=" + ps_codTipoOpe + "&mtoDesde=" + ps_mtoDesde + "&mtoHasta=" + ps_mtoHasta + "&fecEmisionIni=" + FechaEmisionInicio + "&fecEmisionFin=" + ps_fecEmisionFin + "&numDocAdquiriente=" + ps_numDocAdquiriente + "&codCar=" + ps_codCar + "&codTipoCDP=" + ps_codTipoCDP + "&codInconsistencia=" + ps_codInconsistencia + "&page=" + n_numpag + "&perPage=100");
            var client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/migeigv/libros/ventas/propuesta/"+ ps_periodo + "/comprobantes?codTipoOpe="+ ps_codTipoOpe + "&mtoDesde=&mtoHasta=&fecEmisionIni=&fecEmisionFin=&numDocAdquiriente=&codCar=&codTipoCDP=&codInconsistencia=&page="+ n_numpag + "&perPage=100");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer " + this.Token);
            IRestResponse response = client.Execute(request);
            StringBuilder cadenaTot = new StringBuilder();
            StringBuilder cadena = new StringBuilder();
 
            try
            {

                do
                {
                    cadena.Clear();
                    var obj1 = new JavaScriptSerializer().Deserialize<dynamic>(response.Content);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        foreach (var item in obj1["registros"])
                        {

                            cadena.Append(item["id"] + "|");
                            cadena.Append(item["numRuc"] + "|");
                            cadena.Append(item["nomRazonSocial"] + "|");
                            cadena.Append(item["perPeriodoTributario"] + "|");
                            cadena.Append(item["codCar"] + "|");
                            cadena.Append(item["codTipoCDP"] + "|");
                            cadena.Append(item["numSerieCDP"] + "|");
                            cadena.Append(item["numCDP"] + "|");
                            cadena.Append(item["codTipoCarga"] + "|");
                            cadena.Append(item["codSituacion"] + "|");
                            cadena.Append(Convert.ToDateTime(item["fecEmision"]) + "|");
                            cadena.Append(item["codTipoDocIdentidad"] + "|");
                            cadena.Append(item["numDocIdentidad"] + "|");
                            cadena.Append(item["nomRazonSocialCliente"] + "|");
                            cadena.Append(Convert.ToDouble(item["mtoValFactExpo"]) + "|");
                            cadena.Append(Convert.ToDouble(item["mtoBIGravada"]) + "|");
                            cadena.Append(Convert.ToDouble(item["mtoDsctoBI"]) + "|");
                            cadena.Append(Convert.ToDouble(item["mtoIGV"]) + "|");
                            cadena.Append(Convert.ToDouble(item["mtoDsctoIGV"]) + "|");
                            cadena.Append(Convert.ToDouble(item["mtoExonerado"]) + "|");
                            cadena.Append(Convert.ToDouble(item["mtoInafecto"]) + "|");
                            cadena.Append(Convert.ToDouble(item["mtoISC"]) + "|");
                            cadena.Append(Convert.ToDouble(item["mtoBIIvap"]) + "|");
                            cadena.Append(Convert.ToDouble(item["mtoIvap"]) + "|");
                            cadena.Append(Convert.ToDouble(item["mtoIcbp"]) + "|");
                            cadena.Append(Convert.ToDouble(item["mtoOtrosTrib"]) + "|");
                            cadena.Append(Convert.ToDouble(item["mtoTotalCP"]) + "|");
                            cadena.Append(item["codMoneda"] + "|");
                            cadena.Append(Convert.ToDouble(item["mtoTipoCambio"]) + "|");
                            cadena.Append(item["codEstadoComprobante"] + "|");
                            cadena.Append(Convert.ToDouble(item["mtoValorOpGratuitas"]) + "|");
                            cadena.Append(Convert.ToDouble(item["mtoValorFob"]) + "|");
                            cadena.Append(Convert.ToInt32(item["numInconsistencias"]) + "|");
                            cadena.Append(item["indEditable"] + "|");




                            cadena.Append(Environment.NewLine);


                        }


                    }
                    else
                    {
                        cadena.Append(obj1["cod"] + "|");
                        cadena.Append(obj1["msg"] + "|");
                        cadena.Append(obj1["exc"] + "|");

                        foreach (var error in obj1["errors"])
                        {

                            cadena.Append(error["codError"] + "|");
                            cadena.Append(error["desError"] + "|");

                        }

                        //docPendientes1.IsOk = false;
                        //docPendientes1.MensajeError_HDS = "Error al Generar el método ConsultaComprobantesPND , avisar a sistemas -> 1 | ";
                        //docPendientesList.Add(docPendientes1);
                    }

                    cadenaTot.Append(cadena);
                    n_numpag = n_numpag + 1;
                    //client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/controlcpe/comprobantes?indFechaFiltro=FE&codCpe=01&fecInicio=" + FecIni + "&fecFin=" + FecFin + "&numPag=" + n_numpag + "&numRegPag=100&codEstado=" + cod_estado + "&codTipTransaccion=&codMoneda=" + codMonedaPND + "&numSerie=&numCpe=" + numCpe + "&numRuc=" + numRuc + "&codTipoDocAdqui=&numDocAdqui=&indContribuyente=" + ind_contribuyente);
                     client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/migeigv/libros/ventas/propuesta/" + ps_periodo + "/comprobantes?codTipoOpe=" + ps_codTipoOpe + "&mtoDesde=&mtoHasta=&fecEmisionIni=&fecEmisionFin=&numDocAdquiriente=&codCar=&codTipoCDP=&codInconsistencia=&page=" + n_numpag + "&perPage=1000");

                    response = client.Execute(request);

                } while (cadena.Length > 0);



                return cadenaTot.ToString();

            }
            catch (ArgumentException arg)
            {
                cadena.Append(arg.Message + "|");
                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = "Error al Generar el método ConsultaComprobantesPND , avisar a sistemas -> 2 | " + ex.Message;
            }
            catch (ProtocolViolationException vio)
            {
                cadena.Append(vio.Message + "|");

                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = "Error al Generar el método ConsultaComprobantesPND , avisar a sistemas -> 3 | " + ex.Message;
            }
            catch (InvalidOperationException inv)
            {
                cadena.Append(inv.Message + "|");

                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = "Error al Generar el método ConsultaComprobantesPND , avisar a sistemas -> 4 | " + ex.Message;
            }
            catch (NotSupportedException not)
            {
                cadena.Append(not.Message + "|");

                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = "Error al Generar el método ConsultaComprobantesPND , avisar a sistemas -> 5 | " + ex.Message;
            }
            catch (Exception ex)
            {
                cadena.Append(ex.Message + "|");

                //docPendientes1.IsOk = false;
                //docPendientes1.MensajeError_HDS = ex.Message;
            }

            return cadena.ToString();
        }

        //ACEPTAR PROPUESTA RVIE CPE

        public string AceptaPropuesta(int ps_periodo)
        {
            var client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/migeigv/libros/ventas/propuesta/"+ps_periodo+"/aceptapropuesta");
            client.Timeout = -1;
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer " + this.Token);
            IRestResponse restResponse = client.Execute((IRestRequest)request);
            StringBuilder cadena = new StringBuilder();


            Repuesta_Conformidad objres = new Repuesta_Conformidad();
            try
            {
                var obj1 = new JavaScriptSerializer().Deserialize<dynamic>(restResponse.Content);
                if (restResponse.StatusCode == HttpStatusCode.OK)
                {

                    cadena.Append(obj1["numTicket"] + "|");

                    cadena.Append(Environment.NewLine);



                }
                else
                {
                    cadena.Append(obj1["status"] + "|");
                    cadena.Append(obj1["message"] + "|");

             

                    //objres.IsOk = false;
                    //objres.MensajeError_HDS = "Error al Generar el método EnviaConformidad , avisar a sistemas -> 1 | ";

                }

                return cadena.ToString();

            }

            catch (ArgumentException arg)
            {
                //objres.IsOk = false;
                //objres.MensajeError_HDS = "Error al Generar el método EnviaConformidad , avisar a sistemas -> 2 | " + ex.Message;
                cadena.Append(arg.Message + "|");
            }
            catch (ProtocolViolationException viola)
            {
                //objres.IsOk = false;
                //objres.MensajeError_HDS = "Error al Generar el método EnviaConformidad , avisar a sistemas -> 3 | " + ex.Message;
                cadena.Append(viola.Message + "|");
            }
            catch (InvalidOperationException inva)
            {
                //objres.IsOk = false;
                //objres.MensajeError_HDS = "Error al Generar el método EnviaConformidad , avisar a sistemas -> 4 | " + ex.Message;
                cadena.Append(inva.Message + "|");
            }
            catch (NotSupportedException not)
            {
                //objres.IsOk = false;
                //objres.MensajeError_HDS = "Error al Generar el método EnviaConformidad , avisar a sistemas -> 5 | " + ex.Message;
                cadena.Append(not.Message + "|");
            }
            catch (Exception ex)
            {
                //objres.IsOk = false;
                //objres.MensajeError_HDS = ex.Message;
                cadena.Append(ex.Message + "|");
            }

            return cadena.ToString();


        }

        private string token;
        private string client_id;
        private string client_secret;
        private string username;
        private string password;

        public string Token { get => token; set => token = value; }
        public string Client_id { get => client_id; set => client_id = value; }
        public string Client_secret { get => client_secret; set => client_secret = value; }
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }

        public class Ticket_Masivos
        {
            public bool IsOk { get; set; }

            public string ind_TipoRegistro { get; set; }

            public string cod_TicketMasivo { get; set; }

            public string cod_estado_proceso { get; set; }

            public string des_Archivo { get; set; }

            public DateTime fec_Regis { get; set; }

            public DateTime fec_InicioProceso { get; set; }

            public DateTime fec_FinProceso { get; set; }

            public int Cnt_RegCpe { get; set; }

            public int Cnt_RegCorrectos { get; set; }

            public int Cnt_RegErrores { get; set; }

            public string archivo_errores { get; set; }

            public string MensajeError_HDS { get; set; }

            public string Cod_Error_Servidor { get; set; }

            public string msg_Error_servidor { get; set; }

            public object exc_servidor { get; set; }

            public string codError_sunat { get; set; }

            public string desError_sunat { get; set; }
        }
        public class Repuesta_Conformidad
        {
            public bool IsOk { get; set; }

            public string codOperacion { get; set; }

            public string desRespuesta { get; set; }

            public object codTicketMasivo { get; set; }

            public string MensajeError_HDS { get; set; }

            public string Cod_Error_Servidor { get; set; }

            public string msg_Error_servidor { get; set; }

            public object exc_servidor { get; set; }

            public string codError_sunat { get; set; }

            public string desError_sunat { get; set; }
        }
        public class DocPendientes
            {
                public bool IsOk { get; set; }
                public string codCpe_PND { get; set; }
                public string Serie_doc { get; set; }
                public int Numero_doc { get; set; }
                public string Cod_monPND { get; set; }
                public string Emi_NumRuc { get; set; }
                public string Emi_RazSocial { get; set; }
                public string Rec_NumRuc { get; set; }
                public string Rec_RazSocial { get; set; }
                public double ProcIndividual_mtoImporteTotal { get; set; }
                public double ProcMasiva_mtoImporteTotal { get; set; }
                public double mtoPagoPendiente { get; set; }
                public int num_Cuota { get; set; }
                public DateTime Fec_emision { get; set; }
                public DateTime Fec_vencimiento { get; set; }
                public DateTime Fec_registro { get; set; }
                public DateTime Fec_PuestDisposicion { get; set; }
                public string Cod_tipo_transaccion { get; set; }
                public int num_dias_atencion { get; set; }
                public string ind_estadoCPE { get; set; }
                public string MensajeError_HDS { get; set; }
                public string Cod_Error_Servidor { get; set; }
                public string msg_Error_servidor { get; set; }
                public object exc_servidor { get; set; }
                public string codError_sunat { get; set; }
                public string desError_sunat { get; set; }
        }




    }
}
