using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace HDFacturacionDTE.Data
{
    class BaseDatos
    {

        static BaseDatos()
        {
            BDHDDte = "HELPDESKDTE";
        }

        public static string BDHDDte { get; private set; }
    }
}
