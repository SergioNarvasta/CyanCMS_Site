using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HDFacturacionDTE.Data;
using HDFacturacionDTE.Entity;

namespace HDFacturacionDTE.Business
{
    public class BLTipoDocumento
    {
        public List<BETipoDocumento> getListaDocumentos()
        {
            DATipoDocumento obj = null;

            obj = new DATipoDocumento();
            return obj.getListaDocumentos();
        }
    }
}
