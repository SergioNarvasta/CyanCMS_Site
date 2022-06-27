using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HDFacturacionDTE.Data;
using HDFacturacionDTE.Entity;
using System.Configuration;


namespace HDFacturacionDTE.Business
{
    public class BLDocumentoDTE
    {

        public bool ValidarRUCEmisor(string pisRUCEmisor, ref string posMensajeResultado)
        {
            DADocumentoDTE obj = null;
            int iExiste;

            obj = new DADocumentoDTE();
            iExiste = obj.ValidarRUCEmisor(pisRUCEmisor, ref posMensajeResultado);

            return (iExiste > 0);
        }

        public BEDocumentosDTE ObtenerDocumentosDTE(string pisRUCEmisor, string pisTipoDoc, string pisSerieDoc, string pisNroDoc,
                    string pisFechaEmision, string pidImporteDoc)
        {
            DADocumentoDTE obj = null;

            obj = new DADocumentoDTE();
            return obj.getDocumentosDTE(pisRUCEmisor, pisTipoDoc, pisSerieDoc, pisNroDoc, pisFechaEmision, pidImporteDoc);
        }
    }
}
