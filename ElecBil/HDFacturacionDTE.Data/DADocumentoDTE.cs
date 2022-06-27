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
    public class DADocumentoDTE
    {
        public virtual int ValidarRUCEmisor(string pisRUCEmisor, ref string posMensajeResultado)
        {
            //--inicializa dato a  devolver
            int intExiste = 0; //- 0= NO existe

            posMensajeResultado = "";

            //--define parametros
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("RUC", DbType.String,20,ParameterDirection.Input)
											   };
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = pisRUCEmisor;

            BDHDDTE obj = new BDHDDTE(BaseDatos.BDHDDte);
            DAABRequest obRequest = obj.CreaRequest(new StackTrace(true));
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = Constantes.HDDTE_VALIDARRUCEMISOR;
            obRequest.Parameters.AddRange(arrParam);

            IDataReader dr = null;
            try
            {
                dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;
                while (dr.Read())
                {
                    intExiste = Funciones.CheckInt(dr["CODIGO"]);
                    posMensajeResultado = Funciones.CheckStr(dr["DESCRIPCION"]);
                }
            }

            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }

            return intExiste;
        }

        public BEDocumentosDTE getDocumentosDTE(string pisRUCEmisor, string pisTipoDoc, string pisSerieDoc, string pisNroDoc,
                    string pisFechaEmision, string pidImporteDoc)
        {

            DAABRequest.Parameter[] arrParam = {new DAABRequest.Parameter("RUC_EMI", DbType.String,20,ParameterDirection.Input),
												new DAABRequest.Parameter("TIPODOC", DbType.String,3,ParameterDirection.Input),
												new DAABRequest.Parameter("SERIE", DbType.String,4,ParameterDirection.Input),
												new DAABRequest.Parameter("NRODOC", DbType.String,20,ParameterDirection.Input),
											    new DAABRequest.Parameter("FECHA", DbType.DateTime,ParameterDirection.Input),
                                                new DAABRequest.Parameter("IMPORTE", DbType.Double,ParameterDirection.Input)
                                               };



            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = pisRUCEmisor;
            arrParam[1].Value = pisTipoDoc;
            arrParam[2].Value = pisSerieDoc;
            arrParam[3].Value = pisNroDoc;
            arrParam[4].Value = Funciones.CheckDate(pisFechaEmision).ToShortDateString();
            arrParam[5].Value = Funciones.CheckDbl(pidImporteDoc, 2);

            BDHDDTE obj = new BDHDDTE(BaseDatos.BDHDDte);
            DAABRequest obRequest = obj.CreaRequest(new StackTrace(true));
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = Constantes.HDDTE_OBTENERDOCUMENTOSDTE;
            obRequest.Parameters.AddRange(arrParam);

            IDataReader dr = null;
            BEDocumentosDTE oDocumentosDTE = null;
            try
            {
                dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;

                while (dr.Read())
                {
                    oDocumentosDTE = new BEDocumentosDTE();
                    oDocumentosDTE.sTIPO_DOCUMENTO = Funciones.CheckStr(dr["ID_TIPO_DOCUMENTO"]);
                    oDocumentosDTE.sSERIE_DOCUMENTO = Funciones.CheckStr(dr["SERIE"]);
                    oDocumentosDTE.sNRO_DOCUMENTO = Funciones.CheckStr(dr["NUMERO_DOCUMENTO"]);
                    oDocumentosDTE.sFECHA_DOCUMENTO = Funciones.CheckDate(dr["FECHA_DOCUMENTO"]).ToShortDateString();
                    oDocumentosDTE.dIMPORTE_DOCUMENTO = Funciones.CheckDbl(dr["IMPORTE_DOCUMENTO"], 2);
                    oDocumentosDTE.sDOCUMENTOPDF = Funciones.CheckStr(dr["ARCHIVO_PDF"]);
                    oDocumentosDTE.sDOCUMENTOXML = Funciones.CheckStr(dr["ARCHIVO_XML"]);
                }

            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();

                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }

            return oDocumentosDTE;

        }

    }
}
