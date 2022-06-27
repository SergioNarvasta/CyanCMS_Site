using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal_HDFacturacion_V2.Models
{
    public class Response
    {
        public bool IsSuccess { get; set; }
        public string Result { get; set; }
        public string Action { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public int Id { get; set; }
    }
}