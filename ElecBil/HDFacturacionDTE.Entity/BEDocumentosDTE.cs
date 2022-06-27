using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDFacturacionDTE.Entity
{
    public class BEDocumentosDTE
    {
        public string sTIPO_DOCUMENTO { get; set; }
        public string sSERIE_DOCUMENTO { get; set; }
        public string sNRO_DOCUMENTO { get; set; }
        public string sFECHA_DOCUMENTO { get; set; }
        public double dIMPORTE_DOCUMENTO { get; set; }
        public string sDOCUMENTOPDF { get; set; }
        public string sDOCUMENTOXML { get; set; }
    }
}
