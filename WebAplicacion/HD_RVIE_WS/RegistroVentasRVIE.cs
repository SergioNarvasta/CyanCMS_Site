using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace HD_RVIE_WS
{
   public class RegistroVentasRVIE
    {
        public string GeneraToken()
        {
            var grant_type = "password";
            var scope = "https://api.sunat.gob.pe";
            var request = (HttpWebRequest)WebRequest.Create("https://api-seguridad.sunat.gob.pe/v1/clientessol/" + this.Client_id + "/oauth2/token/");
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
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
        public string Ws07_ConsultaResumenCPE(int Ps_Periodo, int Ps_TipoResumen, int Ps_TipoArchivo )
        {
            //var client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/controlcpe/comprobantes?indFechaFiltro=FE&codCpe=01&fecInicio=" + FecIniPND + "&fecFin=" + FecFinPND + "&numPag=1&numRegPag=50&codestado="+ cod_estado + "&codTipTransaccion=&codMoneda=" + codMonedaPND + "&numSerie=&numCpe=&numRuc=&codTipoDocAdqui=&numDocAdqui=&indContribuyente="+ ind_contribuyente);
            var client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/migeigv/libros/ventas/resumen/"+ Ps_Periodo + "/"+ Ps_TipoResumen + "/"+Ps_TipoArchivo+"/exporta");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer " + this.Token);
            IRestResponse response = client.Execute(request);
            StringBuilder cadena = new StringBuilder();


            try
            {
                    cadena.Clear();
                    cadena.Append( response.Content);
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

        public string WS01_ConsultaDetalleCPE(int ps_periodo, int ps_codTipoOpe, string ps_mtoDesde, string ps_mtoHasta, 
            string ps_fecEmisionIni, string ps_fecEmisionFin,
            string ps_numDocAdquiriente, string ps_codCar,
            string ps_codTipoCDP, string ps_codInconsistencia)
        {
             
            int n_numpag = 1;

            //var client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/controlcpe/comprobantes?indFechaFiltro=FE&codCpe=01&fecInicio=" + FecIni + "&fecFin=" + FecFin + "&numPag=" + cnumpag + "&numRegPag=100&codEstado=" + cod_estado + "&codTipTransaccion=&codMoneda=" + codMonedaPND + "&numSerie=&numCpe=" + numCpe + "&numRuc=" + numRuc + "&codTipoDocAdqui=&numDocAdqui=&indContribuyente=" + ind_contribuyente);
            var client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/migeigv/libros/ventas/propuesta/" + ps_periodo + "/comprobantes?codTipoOpe=" + ps_codTipoOpe + "&mtoDesde=" + ps_mtoDesde + "&mtoHasta=" + ps_mtoHasta + "&fecEmisionIni=" + ps_fecEmisionIni + "&fecEmisionFin=" + ps_fecEmisionFin + "&numDocAdquiriente=" + ps_numDocAdquiriente + "&codCar=" + ps_codCar + "&codTipoCDP=" + ps_codTipoCDP + "&codInconsistencia=" + ps_codInconsistencia + "&page=" + n_numpag + "&perPage=100");
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

        public string WS01_ConsultaDetalleCPE2(int ps_periodo, int ps_codTipoOpe)
        {
            int n_numpag = 1;

            //var client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/migeigv/libros/ventas/propuesta/" + ps_periodo + "/comprobantes?codTipoOpe=" + ps_codTipoOpe + "&mtoDesde=" + ps_mtoDesde + "&mtoHasta=" + ps_mtoHasta + "&fecEmisionIni=" + FechaEmisionInicio + "&fecEmisionFin=" + ps_fecEmisionFin + "&numDocAdquiriente=" + ps_numDocAdquiriente + "&codCar=" + ps_codCar + "&codTipoCDP=" + ps_codTipoCDP + "&codInconsistencia=" + ps_codInconsistencia + "&page=" + n_numpag + "&perPage=100");
            //var client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/migeigv/libros/ventas/propuesta/" + ps_periodo + "/comprobantes?codTipoOpe=" + ps_codTipoOpe + "&mtoDesde=&mtoHasta=&fecEmisionIni=&fecEmisionFin=&numDocAdquiriente=&codCar=&codTipoCDP=&codInconsistencia=&page=" + n_numpag + "&perPage=1000");

            var client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/migeigv/libros/ventas/propuesta/202201/comprobantes?codTipoOpe=1&mtoDesde=&mtoHasta=&fecEmisionIni=&fecEmisionFin=&numDocAdquiriente=&codCar=2060164764907F0010000002195&codTipoCDP=&codInconsistencia=&page=1&perPage=500");

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
                    client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/migeigv/libros/ventas/propuesta/" + ps_periodo + "/comprobantes?codTipoOpe=" + ps_codTipoOpe + "&mtoDesde=&mtoHasta=&fecEmisionIni=&fecEmisionFin=&numDocAdquiriente=&codCar=&codTipoCDP=&codInconsistencia=&page=" + n_numpag + "&perPage=100");

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



    }
}



