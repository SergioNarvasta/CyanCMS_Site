using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ConsultaSunat
{
 
    public class HDConsultaSunat
    {        //PRUEBAS CON EQUILIBRA

        public string GeneraToken()
        {
            var ps_grant_type = "password";
            var ps_scope = "https://api.sunat.gob.pe";
            var ps_client_id = "97dee99e-984b-4369-a72e-fb03a6c3defa";
            var ps_client_secret = "caWpscwQt8zFaK5t1kgftg==";
            var ps_username = "20601647649EFACT001";
            var ps_password = "factu2018";

            var request = (HttpWebRequest)WebRequest.Create("https://api-seguridad.sunat.gob.pe/v1/clientessol/" + ps_client_id + "/oauth2/token/");
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

            var postData = "grant_type=" + ps_grant_type + "&scope=" + ps_scope + "&client_id=" + ps_client_id + "&client_secret=" + ps_client_secret + "&username=" + ps_username + "&password=" + ps_password;
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
                            return resp;
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

            return resp;

        }

        public string ConsultaConformidadPago(string pu_FEC_INI, string pu_FEC_FIN, string pu_COD_ESTADO,
                                    string pu_COD_MONEDA, string token)
        {
            var client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/controlcpe/comprobantes?indFechaFiltro=FE&codCpe=01&fecInicio=" + pu_FEC_INI + "&fecFin=" + pu_FEC_FIN + "&numPag=1&numRegPag=50&codEstado=" + pu_COD_ESTADO + "&codTipTransaccion=&codMoneda=" + pu_COD_MONEDA + "&numSerie=&numCpe=&numRuc=&codTipoDocAdqui=&numDocAdqui=&indContribuyente=C");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer " + token);
            IRestResponse response = client.Execute(request);

            string resp = "";

            return "oki";
         }

        public string EnviaConformidad(string token)
        {
            var archivo = "20601647649-PND-20211221-99.zip";
            var valHash = "77bea2c141a8971c09e013ddf3c28199e7d3729c8fc4877ba4d2b0800087c2fa";
        var client = new RestClient("https://api-cpe.sunat.gob.pe/v1/contribuyente/controlcpe/enviosmasivoext/registro?archivo="+archivo+ "&valHash="+valHash);
        client.Timeout = -1;
        var request = new RestRequest(Method.POST);
        request.AddHeader("Authorization", "Bearer" + token);
        IRestResponse response = client.Execute(request);
        Console.WriteLine(response.Content);
            return "ok";
        }
    }

    public class Encrypt
    {
        public static string GetSHA256(string txt)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(txt));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

    }

    public class Comprobante
    {
        public string FEC_INI { get; set; }
        public string FEC_FIN { get; set; }
        public string COD_ESTADO { get; set; }
        public string COD_MONEDA { get; set; }
        public string token { get; set; }

    }

}
