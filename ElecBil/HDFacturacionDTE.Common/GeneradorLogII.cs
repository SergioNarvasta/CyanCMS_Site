using System;
using System.Configuration;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Data;

using System.Collections.Generic;
using System.Reflection;
using System.Collections;

namespace HDFacturacionDTE.Common
{
    public class GeneradorLogII
    {
        private readonly static string strPath = ConfigurationManager.AppSettings["strDirectorioLogSISACT"].ToString();

        private string _usuario;
        private string _idIdentificador;
        private string _idTransaccion;
        private string _archivo;
        private string _archivoCompleto;
        public bool _flgUsuarioActual;

        public GeneradorLogII(string usuario, string idIdentificador, string idIdTansaccion, string archivo)
        {
            _usuario = usuario;
            _idIdentificador = idIdentificador;
            _idTransaccion = idIdTansaccion;
            _archivo = archivo;

            if (_usuario != null)
            {
                _flgUsuarioActual = true;
                _archivo += "_" + _usuario;
            }

            _archivoCompleto = string.Format("{0}\\{1}_{2}.{3}", strPath, _archivo, DateTime.Now.ToString("dd-MM-yyyy"), "log");

            DirectoryInfo dir = new DirectoryInfo(strPath);
            if (!dir.Exists) dir.Create();
        }

        public void CrearArchivologSap(string evento, string rfc, object obj, Exception objException)
        {
            StreamWriter swWriter = null;
            StringBuilder sblog = new StringBuilder();
            StringBuilder sblogDetalle = new StringBuilder();
            string strIdGeneral = string.Empty;
            string strId = string.Empty;

            try
            {
                if (_idTransaccion == null) _idTransaccion = DateTime.Now.ToString("hhmmssfff");

                if (string.IsNullOrEmpty(_idIdentificador))
                    strIdGeneral = string.Format("[{0}][{1}]-", _idTransaccion, _usuario);
                else
                    strIdGeneral = string.Format("[{0}][{1}][{2}]-", _idTransaccion, _usuario, _idIdentificador);

                strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);

                if (evento != null)
                {
                    sblog.AppendLine(strId + evento);
                }

                if (rfc != null)
                {
                    sblog.AppendLine(string.Format("{0}[RFC][{1}]", strId, rfc));
                }

                sblogDetalle = EstructuraLog(strIdGeneral, strId, objException, obj);
                sblog.Append(sblogDetalle.ToString());

                swWriter = File.AppendText(_archivoCompleto);
                swWriter.Write(sblog);
            }
            catch (Exception)
            {

            }
            finally
            {
                if (swWriter != null)
                    swWriter.Close();
            }
        }

        public void CrearArchivologWS(string evento, string url, object obj, Exception objException)
        {
            StreamWriter swWriter = null;
            StringBuilder sblog = new StringBuilder();
            StringBuilder sblogDetalle = new StringBuilder();
            string strIdGeneral = string.Empty;
            string strId = string.Empty;

            try
            {
                if (_idTransaccion == null) _idTransaccion = DateTime.Now.ToString("hhmmssfff");

                if (string.IsNullOrEmpty(_idIdentificador))
                    strIdGeneral = string.Format("[{0}][{1}]-", _idTransaccion, _usuario);
                else
                    strIdGeneral = string.Format("[{0}][{1}][{2}]-", _idTransaccion, _usuario, _idIdentificador);

                strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);

                if (evento != null)
                {
                    sblog.AppendLine(strId + evento);
                }

                if (url != null)
                {
                    sblog.AppendLine(string.Format("{0}[WS][{1}]", strId, url));
                }

                sblogDetalle = EstructuraLog(strIdGeneral, strId, objException, obj);
                sblog.Append(sblogDetalle.ToString());

                swWriter = File.AppendText(_archivoCompleto);
                swWriter.Write(sblog);
            }
            catch (Exception)
            {

            }
            finally
            {
                if (swWriter != null)
                    swWriter.Close();
            }
        }

        public void CrearArchivolog(string evento, object obj, Exception objException)
        {
            StreamWriter swWriter = null;
            StringBuilder sblog = new StringBuilder();
            StringBuilder sblogDetalle = new StringBuilder();
            string strIdGeneral = string.Empty;
            string strId = string.Empty;

            try
            {
                if (_idTransaccion == null) _idTransaccion = DateTime.Now.ToString("hhmmssfff");

                if (string.IsNullOrEmpty(_idIdentificador))
                    strIdGeneral = string.Format("[{0}][{1}]-", _idTransaccion, _usuario);
                else
                    strIdGeneral = string.Format("[{0}][{1}][{2}]-", _idTransaccion, _usuario, _idIdentificador);

                strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);

                if (evento != null)
                {
                    sblog.AppendLine(strId + evento);
                }

                sblogDetalle = EstructuraLog(strIdGeneral, strId, objException, obj);
                if (sblogDetalle.Length > 0)
                    sblog.AppendLine(sblogDetalle.ToString());

                swWriter = File.AppendText(_archivoCompleto);
                swWriter.Write(sblog);
            }
            catch (Exception)
            {

            }
            finally
            {
                if (swWriter != null)
                    swWriter.Close();
            }
        }

        public void CrearArchivolog(string evento, List<String> lstLog, Exception objException)
        {
            StreamWriter swWriter = null;
            StringBuilder sblog = new StringBuilder();
            StringBuilder sblogDetalle = new StringBuilder();
            string strIdGeneral = string.Empty;
            string strId = string.Empty;

            try
            {
                if (_idTransaccion == null) _idTransaccion = DateTime.Now.ToString("hhmmssfff");

                if (string.IsNullOrEmpty(_idIdentificador))
                    strIdGeneral = string.Format("[{0}][{1}]-", _idTransaccion, _usuario);
                else
                    strIdGeneral = string.Format("[{0}][{1}][{2}]-", _idTransaccion, _usuario, _idIdentificador);

                strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);

                if (evento != null)
                {
                    sblog.AppendLine(strId + evento);
                }

                if (lstLog != null)
                {
                    foreach (string strLog in lstLog)
                    {
                        strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);
                        sblog.AppendLine(strId + strLog);
                    }
                }

                sblogDetalle = EstructuraLog(strIdGeneral, strId, objException, null);
                sblog.Append(sblogDetalle.ToString());

                swWriter = File.AppendText(_archivoCompleto);
                swWriter.Write(sblog);
            }
            catch (Exception)
            {

            }
            finally
            {
                if (swWriter != null)
                    swWriter.Close();
            }
        }

        //private StringBuilder EstructuraLog(string strIdGeneral, string strId, List<String> lstLog, Exception objException, object obj)
        private StringBuilder EstructuraLog(string strIdGeneral, string strId, Exception objException, object obj)
        {
            StringBuilder sblog = new StringBuilder();

            try
            {
                if (objException != null)
                {
                    strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);
                    sblog.AppendLine(strId + "Message = " + objException.Message);
                    strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);
                    sblog.AppendLine(strId + "GetType = " + objException.GetType().ToString());
                    strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);
                    sblog.AppendLine(strId + "Source  = " + (String.IsNullOrEmpty(objException.Source) ? String.Empty : objException.Source));

                    if (objException.TargetSite != null)
                    {
                        strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);
                        sblog.AppendLine(strId + "TargetSite = " + objException.TargetSite.ToString());
                    }
                    if (objException.StackTrace != null)
                    {
                        strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);
                        sblog.AppendLine(strId + "StackTrace = " + objException.StackTrace.ToString());
                    }
                }

                if (obj != null)
                {
                    Type tyobjt = obj.GetType();

                    if (tyobjt.Name == "String")
                    {
                        strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);
                        sblog.Append(strId);
                        sblog.AppendLine(obj.ToString());
                    }
                    else if (tyobjt.Name == "DataSet")
                    {
                        int canttables = ((DataSet)(obj)).Tables.Count;

                        for (int i = 0; i < canttables; i++)
                        {
                            DataTable dt = (DataTable)((DataSet)(obj)).Tables[i];

                            strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);
                            sblog.Append(strId);
                            sblog.AppendLine(string.Format("[Tabla_{0}][nroRegistro={1}]", i, dt.Rows.Count));

                            foreach (DataRow row in dt.Rows)
                            {
                                strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);
                                sblog.Append(strId);

                                foreach (DataColumn column in dt.Columns)
                                {
                                    sblog.Append(string.Format("[{0}={1}]", column.ColumnName, row[column].ToString()));
                                }
                                sblog.AppendLine(" ");
                            }
                        }
                    }
                    else if (tyobjt.Name == "DataTable")
                    {
                        DataTable dt = (DataTable)obj;
                        foreach (DataRow row in dt.Rows)
                        {
                            strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);
                            sblog.Append(strId);

                            foreach (DataColumn column in dt.Columns)
                            {
                                sblog.Append(string.Format("[{0}={1}]", column.ColumnName, row[column].ToString()));
                            }
                            sblog.AppendLine(" ");
                        }
                    }
                    else if (tyobjt.Namespace == "HDFacturacionDTE.Entity")
                    {
                        String atributo = string.Empty;
                        System.Reflection.PropertyInfo[] propiedades = tyobjt.GetProperties();
                        object objvalue;
                        PropertyInfo objInfo;

                        strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);
                        sblog.Append(strId);

                        foreach (System.Reflection.PropertyInfo propiedad in propiedades)
                        {
                            atributo = propiedad.Name;
                            objInfo = obj.GetType().GetProperty(atributo);
                            objvalue = objInfo.GetValue(obj, null);

                            if (!(objInfo.PropertyType.Namespace == "HDFacturacionDTE.Entity" || objInfo.PropertyType.Namespace == "System.Collections.Generic" ||
                                objInfo.PropertyType.Name == "ArrayList" || objInfo.PropertyType.Name == "DataTable" || objInfo.PropertyType.Name == "DataSet"))
                            {
                                sblog.Append(string.Format("[{0}={1}]", propiedad.Name, Funciones.CheckStr(objvalue)));
                            }
                        }
                        //sblog.AppendLine(" ");
                    }
                    else if (tyobjt.Namespace == "System.Collections.Generic")
                    {
                        int cantidadfilas = (int)tyobjt.GetProperty("Count").GetValue(obj, null);

                        for (int i = 0; i < cantidadfilas; i++)
                        {
                            object[] index = { i };
                            object objEntidad = obj.GetType().GetProperty("Item").GetValue(obj, index);
                            PropertyInfo[] objpropentidad = objEntidad.GetType().GetProperties();

                            strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);
                            sblog.Append(strId);

                            foreach (PropertyInfo propiedad in objpropentidad)
                            {
                                object propertyValue = propiedad.GetValue(objEntidad, null);

                                if (!(propiedad.PropertyType.Namespace == "System.Collections.Generic" || propiedad.PropertyType.Name == "ArrayList" ||
                                    propiedad.PropertyType.Name == "DataTable" || propiedad.PropertyType.Name == "DataSet"))
                                {
                                    sblog.Append(string.Format("[{0}={1}]", propiedad.Name, propertyValue));
                                }
                            }
                            //sblog.AppendLine(" ");
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

            return sblog;
        }

    }
}
