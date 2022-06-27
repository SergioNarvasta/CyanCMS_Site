using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDFacturacionDTE.Configuracion
{
    public abstract class HDConfiguracionBase : IConfiguracionBase
    {
        private HDBDConfiguracion _objParametros;

        public HDConfiguracionBase(string aplicacion)
        {
            try
            {
                DatoProduccion(aplicacion);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DatoProduccion(string aplicacion)
        {
            if (_objParametros == null)
            {

                _objParametros = new HDBDConfiguracion();
            }

            Base.Configuracion cfgConexion = new Base.Configuracion(aplicacion);

            LeerValores(cfgConexion);
        }

        public abstract void LeerValores(Base.Configuracion configuracion);


        public HDBDConfiguracion Parametros
        {
            get
            {

                return _objParametros;
            }
        }
    }
}
