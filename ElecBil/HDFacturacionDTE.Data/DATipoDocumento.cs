using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using HDFacturacionDTE.Common;
using HDFacturacionDTE.Entity;
using HDFacturacionDTE.IData;
using System.Diagnostics;
using System.Reflection;

namespace HDFacturacionDTE.Data
{
    public class DATipoDocumento
    {

        public List<BETipoDocumento> getListaDocumentos()
        {
            //--parametro de salida

            BDHDDTE obj = new BDHDDTE(BaseDatos.BDHDDte);
            DAABRequest obRequest = obj.CreaRequest(new StackTrace(true));
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = Constantes.HDDTE_OBTENERLISTADOCUMENTOS;

            List<BETipoDocumento> filas = new List<BETipoDocumento>();
            IDataReader dr = null;
            try
            {
                dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BETipoDocumento item = new BETipoDocumento();

                    item.sID_TIPODOCUMENTO = Funciones.CheckStr(dr["ID_TIPO_DOCUMENTO"]);
                    item.sDESC_TIPODOCUMENTO = Funciones.CheckStr(dr["TIPO_DOCUMENTO"]);
                    
                    //--agrega item
                    filas.Add(item);
                }
            }

            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }

            return filas;
        }
    }
}
