
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace HDLIbreriaSUNAT
{
   public class HDConformidadSUNAT
    {
        #region Genera Token
        public string GeneraToken()
        {
            var grant_type = "password";
            var scope = "https://api.sunat.gob.pe";
            var client_id = "eb405a79-cedb-494b-9ae3-7cc133761410";
            var client_secret = "Erzio/UwDohK6bwaxSMP8Q==";
            var username = "20601464927HDSOFT21";
            var password = "Sunat@2021";
            var request = (HttpWebRequest)WebRequest.Create("https://api-seguridad.sunat.gob.pe/v1/clientessol/" + client_id + "/oauth2/token/");
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
            var postData = "grant_type=" + grant_type + "&scope=" + scope + "&client_id=" + client_id + "&client_secret=" + client_secret + "&username=" + username + "&password=" + password;
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
        #endregion


    }
}
