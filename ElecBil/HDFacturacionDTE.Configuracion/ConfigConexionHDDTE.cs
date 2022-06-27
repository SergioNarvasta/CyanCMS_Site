using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace HDFacturacionDTE.Configuracion
{
    public class ConfigConexionHDDTE: HDConfiguracionBase
    {
        public ConfigConexionHDDTE(string aplicacion)
            : base(aplicacion) { }

        public override void LeerValores(Base.Configuracion configuracion)
        {
            Parametros.BaseDatos = configuracion.LeerBaseDatos();
            Parametros.Usuario = configuracion.LeerUsuario();
            Parametros.Password = configuracion.LeerContrasena();
            Parametros.Servidor = configuracion.LeerServidor();
            Parametros.Provider = configuracion.LeerProveedor();
            Parametros.Port = configuracion.LeerPuerto();

        }
    }
}
