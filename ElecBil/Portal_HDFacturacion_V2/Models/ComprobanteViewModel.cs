using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal_HDFacturacion_V2.Models
{
    public class ComprobanteViewModel
    {
        public string NumeroRuc { get; set; }
        public string CodigoComp { get; set; }
        public string NumeroSerie { get; set; }
        public string Numero { get; set; }
        public string FechaEmision { get; set; }
        public string Monto { get; set; }
    }
}