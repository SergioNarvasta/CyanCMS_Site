using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using HDFacturacionDTE.Configuracion;
using HDFacturacionDTE.IData;

namespace HDFacturacionDTE.Data
{
    public class BDHDDTE: BDConexion 
    {
        public BDHDDTE(string aplicacion)
            : base(aplicacion) { }

        protected override IHDBDConfiguracion Configuracion
        {
            get
            {
                return Conexion.GeneraConfiguracion<ConfigConexionHDDTE>(this.Aplicacion);
            }
        }

    }
}
