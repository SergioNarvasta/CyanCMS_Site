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

namespace HDLIbreriaSUNAT
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("HDLIbreriaSUNAT.HDValidarCPESunat")]
    public class HDValidarCPESunat:
       System.EnterpriseServices.ServicedComponent
    {
        /// VALIDEZ DE COMPROBANTES ELÉCTRONICOS
        public string ps_verificaHD = "HD-20220430";

        public string GeneraTokenSunat()
        {
            var grant_type = "client_credentials";
            var scope = "https://api.sunat.gob.pe/v1/contribuyente/contribuyentes";
            var client_id = "ed950066-8132-4be7-84ae-80ece888c0a7";
            var client_secret = "UmzfDQM145lNju91kR293w==";

            var request = (HttpWebRequest)WebRequest.Create("https://api-seguridad.sunat.gob.pe/v1/clientesextranet/ed950066-8132-4be7-84ae-80ece888c0a7/oauth2/token/");
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

            var postData = "grant_type=" + grant_type + "&scope=" + scope + "&client_id=" + client_id + "&client_secret=" + client_secret;
            var data = Encoding.ASCII.GetBytes(postData);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded;charset=utf8";
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
                            return "";
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
                resp = ae.Message;
            }
            catch (ProtocolViolationException pve)
            {
                resp = pve.Message;
            }
            catch (InvalidOperationException ioe)
            {
                resp = ioe.Message;
            }
            catch (NotSupportedException nse)
            {
                resp = nse.Message;
            }


            return resp;
        }

        public string ConsultaSunat(string NumeroRuc, string CodigoComp, string NumeroSerie,
                                    string Numero, string FechaEmision, string Monto, string tokens)

        {

            // string tokens = GeneraTokenSunat();

            var request = (HttpWebRequest)WebRequest.Create("https://api.sunat.gob.pe/v1/contribuyente/contribuyentes/" + NumeroRuc + "/validarcomprobante");
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

            var json = "{\r\n \"numRuc\":\"" + NumeroRuc + "\",\r\n\"codComp\":\"" + CodigoComp + "\",\r\n\"numeroSerie\":\"" + NumeroSerie.ToUpper() + "\",\r\n\"numero\":\"" + Numero + "\",\r\n\"fechaEmision\":\"" + Convert.ToDateTime(FechaEmision).ToString("dd/MM/yyyy") + "\",\r\n\"monto\":\"" + Monto + "\"\r\n}";

            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            request.Headers.Add("Authorization", "Bearer " + tokens);

            string resp = "";
            try
            {
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null)
                        {
                            return "Respuesta Nulo";
                        }

                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseString = objReader.ReadToEnd();
                            var token = new JavaScriptSerializer().Deserialize<dynamic>(responseString);

                            string ret = "";
                            ret = "success:" + token["success"] + "|" + "message:" + token["message"] + "|";
                            string valor = "";
                            foreach (var item in token["data"])
                            {
                                if (item.Key == "observaciones")
                                {
                                    foreach (var item2 in item.Value)
                                    {
                                        valor = item2 + "|";
                                        ret = ret + valor;
                                    }
                                }
                                else
                                {
                                    valor = item.Key + ":" + item.Value + "|";
                                    ret = ret + valor;
                                }

                            }
                            resp = ret;
                            //resp = objReader.ReadToEnd();
                        }
                    }
                }
            }
            catch (ArgumentException ae)
            {
                resp = ae.Message;
            }
            catch (ProtocolViolationException pve)
            {
                resp = pve.Message;
            }
            catch (InvalidOperationException ioe)
            {
                resp = ioe.Message;
            }
            catch (NotSupportedException nse)
            {
                resp = nse.Message;
            }

            return resp;
        }

        /// <summary>
        /// VALIDEZ DE CONFORMIDAD DE COMPROBANTES DE PAGO ELÉCTRONICO
        /// </summary>
        /// 
        public string GeneraTokenSunatConformidad()
        {
            return  "";
        }

        public string ListaPendientesDocumentos()
        {
            return "";
        }



    }
}
