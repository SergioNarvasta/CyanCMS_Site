
using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Collections.Generic;
using HDFacturacionDTE.Common;
using System.Linq;


namespace HDFacturacionDTE.IData
{
    public class DAABOracleFactory : DAABAbstracFactory
    {
        private OracleConnection m_conecSQL;
        private OracleTransaction m_TranSQL;

        private void AttachParameters(OracleCommand command, OracleParameter[] commandParameters)
        {
            if ((command == null))
            {
                throw new ArgumentNullException("command");
            }
            if ((commandParameters != null))
            {
                ;
                foreach (OracleParameter p in commandParameters)
                {
                    if ((p != null))
                    {
                        if ((p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input) && p.Value == null)
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        private void AssignParameterValues(OracleParameter[] commandParameters, DataRow dataRow)
        {
            if (commandParameters == null || dataRow == null)
            {
                return;
            }

            int i = 0;
            foreach (OracleParameter commandParameter in commandParameters)
            {
                if ((commandParameter.ParameterName == null || commandParameter.ParameterName.Length <= 1))
                {
                    throw new Exception(string.Format("Please provide a valid parameter name on the parameter #{0}, the ParameterName property has the following value: ' {1}' .", i, commandParameter.ParameterName));
                }
                if (dataRow.Table.Columns.IndexOf(commandParameter.ParameterName) != -1)
                {
                    commandParameter.Value = dataRow[commandParameter.ParameterName];
                }
                i++;
            }
        }

        private void AssignParameterValues(OracleParameter[] commandParameters, object[] parameterValues)
        {
            int j;
            if ((commandParameters == null) && (parameterValues == null))
            {
                return;
            }
            if (commandParameters != null && commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }

            j = commandParameters.Length - 1;
            for (int i = 0; i <= j; i++)
            {
                if (parameterValues[i] is IDbDataParameter)
                {
                    IDbDataParameter paramInstance = ((IDbDataParameter)(parameterValues[i]));
                    if ((paramInstance.Value == null))
                    {
                        commandParameters[i].Value = DBNull.Value;
                    }
                    else
                    {
                        commandParameters[i].Value = paramInstance.Value;
                    }
                }
                else if ((parameterValues[i] == null))
                {
                    commandParameters[i].Value = DBNull.Value;
                }
                else
                {
                    commandParameters[i].Value = parameterValues[i];
                }
            }
        }

        private void PrepareCommand(OracleCommand command, OracleConnection connection, OracleTransaction transaction, CommandType commandType, string commandText, OracleParameter[] commandParameters, ref bool mustCloseConnection)
        {
            if ((command == null))
            {
                throw new ArgumentNullException("command");
            }
            if (string.IsNullOrEmpty(commandText))
            {
                throw new ArgumentNullException("commandText");
            }
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
                mustCloseConnection = true;
            }
            else
            {
                mustCloseConnection = false;
            }
            command.Connection = connection;
            command.CommandText = commandText;
            command.BindByName = true;   //Ordenar parametros del Store Procedure
            if (transaction != null)
            {
                if (transaction.Connection == null)
                {
                    throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                }
                command.Transaction = transaction;
            }
            command.CommandType = commandType;
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
            return;
        }

        //PROY-24740
        private List<OracleParameter> ConvertToOracleParameter(CommandType commandType, IDbDataParameter[] parametros)
        {            
            List<OracleParameter> oParametros = parametros.Select(s => new OracleParameter()
            {
                ParameterName = s.ParameterName,
                SourceColumn = s.SourceColumn,
                SourceVersion = s.SourceVersion,
                Direction = s.Direction,
                Value = s.Value,
                    DbType = CastOracleParameterDbType(s.DbType),
                OracleDbType = CastOracleDbType(s.DbType),
                Size = (s.Size == 0 && s.DbType == DbType.String) ? 50 : s.Size,
            }).ToList();
            return oParametros;
        }

        /// <summary>
        /// Conversion para datos no Soportados en Oracle System.Data.DbType
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public DbType CastOracleParameterDbType(DbType dbType)
        {
            if (DbType.Currency == dbType)
            {
                return DbType.Decimal;
            }
            else if (DbType.Boolean == dbType)
            {
                return DbType.Byte;
            }
            else if (DbType.Guid == dbType)
            {
                return DbType.String;
            }
            else if (DbType.SByte == dbType)
            {
                return DbType.Int32;
            }
            else if (DbType.UInt16 == dbType)
            {
                return DbType.Int16;
            }
            else if (DbType.UInt32 == dbType)
            {
                return DbType.Int32;
            }
            else if (DbType.UInt64 == dbType)
            {
                return DbType.Int64;
            }
            else if (DbType.VarNumeric == dbType)
            {
                return DbType.Decimal;
            }
            else
            {
                return dbType;
            }
        }

        public OracleDbType CastOracleDbType(DbType oDbType)
        {
            switch (oDbType)
            {
                case DbType.Object:
                    return OracleDbType.RefCursor;
                case DbType.Date:
                    return OracleDbType.Date;
                case DbType.DateTime:
                    return OracleDbType.Date;
                case DbType.Currency:
                    return OracleDbType.Decimal;
                case DbType.Double:
                    return OracleDbType.Double;
                case DbType.Single:
                    return OracleDbType.Single;
                case DbType.Decimal:
                    return OracleDbType.Decimal;
                case DbType.VarNumeric:
                    return OracleDbType.Decimal;
                case DbType.Byte:
                    return OracleDbType.Byte;
                case DbType.AnsiString:
                    return OracleDbType.Varchar2;
                case DbType.AnsiStringFixedLength:
                    return OracleDbType.Varchar2;
                case DbType.Binary:
                    return OracleDbType.Blob;
                case DbType.Boolean:
                    return OracleDbType.Byte;
                case DbType.Guid:
                    return OracleDbType.Raw;
                case DbType.SByte:
                    return OracleDbType.Int16;
                case DbType.Int16:
                    return OracleDbType.Int16;
                case DbType.UInt16:
                    return OracleDbType.Int16;
                case DbType.Int32:
                    return OracleDbType.Int32;
                case DbType.UInt32:
                    return OracleDbType.Int32;
                case DbType.Int64:
                    return OracleDbType.Int64;
                case DbType.UInt64:
                    return OracleDbType.Int64;
                case DbType.String:
                    return OracleDbType.Varchar2;
                case DbType.StringFixedLength:
                    return OracleDbType.Char;
                case DbType.Time:
                    return OracleDbType.Date;
                default:
                    return OracleDbType.Varchar2;
            }
        }

        public object GetOutputValue(OracleParameter oParameter)
        {
			if (oParameter.Value == System.DBNull.Value)
			{
				return oParameter.Value;
			}            
			switch (oParameter.DbType)
            {
                case DbType.Decimal:
                    return ((OracleDecimal)oParameter.Value).IsNull ? 0 : ((OracleDecimal)oParameter.Value).ToDouble();
                case DbType.String:
                    return ((OracleString)oParameter.Value).IsNull ? null : ((OracleString)oParameter.Value).ToString();
                case DbType.Int32:
                    return (( OracleDecimal)oParameter.Value).IsNull ? 0 : ((OracleDecimal)oParameter.Value).ToInt32();
                case DbType.Int64:
                    return ((OracleDecimal)oParameter.Value).IsNull ? 0 : ((OracleDecimal)oParameter.Value).ToInt64();
                case DbType.Int16:
                    return ((OracleDecimal)oParameter.Value).IsNull ? 0 : ((OracleDecimal)oParameter.Value).ToInt16();
                case DbType.Double:
                    return ((OracleDecimal)oParameter.Value).IsNull ? 0 : ((OracleDecimal)oParameter.Value).ToDouble();
                case DbType.Currency:
                    return ((OracleDecimal)oParameter.Value).IsNull ? 0 : ((OracleDecimal)oParameter.Value).ToDouble();
                case DbType.Single:
                    return ((OracleDecimal)oParameter.Value).IsNull ? 0 : ((OracleDecimal)oParameter.Value).ToSingle();
                default:
                    return oParameter.Value;
            }
        }

        private void AsingValueParameter(OracleParameter[] paranOracle, ref IDbDataParameter[] paramValues)
        {
            int i = 0;
            foreach (OracleParameter oParam in paranOracle)
            {
                if (oParam.Direction != ParameterDirection.Input)
                {
                    paramValues[i].ParameterName = oParam.ParameterName;
                    paramValues[i].SourceColumn = oParam.SourceColumn;
                    paramValues[i].SourceVersion = oParam.SourceVersion;
                    paramValues[i].Direction = oParam.Direction;
                    paramValues[i].Value = GetOutputValue(oParam);
                    paramValues[i].DbType = oParam.DbType;
                }
                i = i + 1;
            }
        }
        //PROY-24740

        private bool estableceConexion(bool pb_transaccional, string pc_cadConexion)
        {
            if (m_conecSQL == null || m_conecSQL.State != ConnectionState.Open)
            {                
                m_conecSQL = new OracleConnection(pc_cadConexion);
                m_conecSQL.Open();
            }
            if (pb_transaccional && m_TranSQL == null)
            {
                m_TranSQL = m_conecSQL.BeginTransaction(IsolationLevel.ReadCommitted);
            }
            return true;
        }

        public override DataSet ExecuteDataset(ref DAABRequest request)
        {
            string connectionString = request.ConnectionString;
            DataSet objDataSet = new DataSet();
            if ((connectionString == null || connectionString.Length == 0))
            {
                throw new ArgumentNullException("connectionString");
            }
            if ((request.Command == null || request.Command.Length == 0))
            {
                throw new ArgumentNullException("No ha ingresado el commando a ejecutar.");
            }
            try
            {
                Log(request);

                IDbDataParameter[] lista = request.Parameters.Cast<IDbDataParameter>().ToArray();
                estableceConexion(request.Transactional, connectionString);
                OracleParameter[] aparam = ConvertToOracleParameter(request.CommandType, lista).ToArray();
                if (request.Transactional)
                {
                    objDataSet = ExecuteDatasets(m_TranSQL, request.CommandType, request.Command, aparam, request.TableNames);
                }
                else
                {
                    objDataSet = ExecuteDatasets(m_conecSQL, request.CommandType, request.Command, aparam, request.TableNames);
                }
                AsingValueParameter(aparam, ref lista);
            }
            catch (Exception ex)
            {
                request.Exception = ex;
                throw ex;
            }
            finally
            {
                if (!(request.Transactional) & !(m_conecSQL == null))
                {
                    m_conecSQL.Dispose();
                }

                Log(request, objDataSet);
            }
            return objDataSet;
        }

        private DataSet ExecuteDatasets(OracleConnection connection, CommandType commandType, string commandText, OracleParameter[] commandParameters, string[] tableNames)
        {
            if ((connection == null))
            {
                throw new ArgumentNullException("connection");
            }
            OracleCommand cmd = new OracleCommand();
            DataSet ds = new DataSet();
            OracleDataAdapter dataAdatpter = new OracleDataAdapter();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters, ref mustCloseConnection);
            try
            {
                dataAdatpter = new OracleDataAdapter(cmd);
                if (tableNames == null)
                {
                    dataAdatpter.Fill(ds);
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        ds.Tables[0].TableName = "Tabla" + i.ToString();
                        ds.AcceptChanges();
                    }
                }
                else if (!(tableNames == null && tableNames.Length > 0))
                {
                    dataAdatpter.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {
                        for (int index = 0; index < tableNames.Length; index++)
                        {
                            if ((tableNames[index] == null || tableNames[index].Length == 0))
                            {
                                throw new ArgumentException("The tableNames parameter must contain a list of tables, a value was provided as null or empty string.", "tableNames");
                            }
                            ds.Tables[index].TableName = tableNames[index];
                        }
                    }
                }
                cmd.Parameters.Clear();
            }
            finally
            {
                if (dataAdatpter != null)
                {
                    dataAdatpter.Dispose();
                }
            }
            if ((mustCloseConnection))
            {
                connection.Close();
            }
            return ds;
        }

        private DataSet ExecuteDatasets(OracleTransaction transaction, CommandType commandType, string commandText, OracleParameter[] commandParameters, string[] tableNames)
        {
            if ((transaction == null))
            {
                throw new ArgumentNullException("transaction");
            }
            if (!((transaction == null)) && (transaction.Connection == null))
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            OracleCommand cmd = new OracleCommand();
            DataSet ds = new DataSet();
            OracleDataAdapter dataAdatpter = new OracleDataAdapter();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);
            try
            {
                dataAdatpter = new OracleDataAdapter(cmd);
                if (!(tableNames == null && tableNames.Length > 0))
                {
                    string tableName = "Table";

                    for (int index = 0; index <= tableNames.Length - 1; index++)
                    {
                        if ((tableNames[index] == null || tableNames[index].Length == 0))
                        {
                            throw new ArgumentException("The tableNames parameter must contain a list of tables, a value was provided as null or empty string.", "tableNames");
                        }
                        dataAdatpter.TableMappings.Add(tableName, tableNames[index]);
                        tableName = tableName + (index + 1).ToString();
                    }
                }
                dataAdatpter.Fill(ds);
                cmd.Parameters.Clear();
            }
            finally
            {
                if ((!(dataAdatpter == null)))
                {
                    dataAdatpter.Dispose();
                }
            }
            return ds;
        }

        public override int ExecuteNonQuery(ref DAABRequest request)
        {
            int iretval;
            string connectionString = request.ConnectionString;
            if ((connectionString == null || connectionString.Length == 0))
            {
                throw new ArgumentNullException("connectionString");
            }
            if ((request.Command == null || request.Command.Length == 0))
            {
                throw new ArgumentNullException("No ha ingresado el commando a ejecutar.");
            }
            try
            {
                IDbDataParameter[] lista = request.Parameters.Cast<IDbDataParameter>().ToArray();
                OracleParameter[] aparam = ConvertToOracleParameter(request.CommandType, lista).ToArray();
                estableceConexion(request.Transactional, connectionString);
                if (request.Transactional)
                {
                    iretval = ExecuteNonQuerys(m_TranSQL, request.CommandType, request.Command, aparam);
                }
                else
                {
                    iretval = ExecuteNonQuerys(m_conecSQL, request.CommandType, request.Command, aparam);
                }
                AsingValueParameter(aparam, ref lista);
            }
            catch (Exception ex)
            {
                request.Exception = ex;
                throw ex;
            }
            finally
            {
                if (!(request.Transactional) & !(m_conecSQL == null))
                {
                    m_conecSQL.Dispose();
                }

                Log(request);
            }
            return iretval;
        }

        private int ExecuteNonQuerys(OracleConnection connection, CommandType commandType, string commandText, OracleParameter[] commandParameters)
        {
            if ((connection == null))
            {
                throw new ArgumentNullException("connection");
            }
            OracleCommand cmd = new OracleCommand();
            int retval;
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, ((OracleTransaction)(null)), commandType, commandText, commandParameters, ref mustCloseConnection);
            retval = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            if ((mustCloseConnection))
            {
                connection.Close();
            }
            return retval;
        }

        private int ExecuteNonQuerys(OracleTransaction transaction, CommandType commandType, string commandText, OracleParameter[] commandParameters)
        {
            if ((transaction == null))
            {
                throw new ArgumentNullException("transaction");
            }
            if (!((transaction == null)) && (transaction.Connection == null))
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            OracleCommand cmd = new OracleCommand();
            int retval;
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);
            retval = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return retval;
        }

        public override object ExecuteScalar(ref DAABRequest Request)
        {
            string connectionString = Request.ConnectionString;
            Object objObject = new Object();
            if ((connectionString == null || connectionString.Length == 0))
            {
                throw new ArgumentNullException("connectionString");
            }
            if ((Request.Command == null || Request.Command.Length == 0))
            {
                throw new ArgumentNullException("No ha ingresado el commando a ejecutar.");
            }
            try
            {
                IDbDataParameter[] lista = Request.Parameters.Cast<IDbDataParameter>().ToArray();
                estableceConexion(Request.Transactional, connectionString);
                OracleParameter[] aparam = ConvertToOracleParameter(Request.CommandType, lista).ToArray();
                if (Request.Transactional)
                {
                    objObject = ExecuteScalares(m_TranSQL, Request.CommandType, Request.Command, aparam);
                }
                else
                {
                    objObject = ExecuteScalares(m_conecSQL, Request.CommandType, Request.Command, aparam);
                }
                AsingValueParameter(aparam, ref lista);
            }
            catch (Exception ex)
            {
                Request.Exception = ex;
                throw ex;
            }
            finally
            {
                if (!(Request.Transactional) & !(m_conecSQL == null))
                {
                    m_conecSQL.Dispose();
                }

                Log(Request);
            }
            return objObject;
        }

        private object ExecuteScalares(OracleConnection connection, CommandType commandType, string commandText, OracleParameter[] commandParameters)
        {
            if ((connection == null))
            {
                throw new ArgumentNullException("connection");
            }
            OracleCommand cmd = new OracleCommand();
            object retval;
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, ((OracleTransaction)(null)), commandType, commandText, commandParameters, ref mustCloseConnection);
            retval = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            if ((mustCloseConnection))
            {
                connection.Close();
            }
            return retval;
        }

        private object ExecuteScalares(OracleTransaction transaction, CommandType commandType, string commandText, OracleParameter[] commandParameters)
        {
            if ((transaction == null))
            {
                throw new ArgumentNullException("transaction");
            }
            if (!((transaction == null)) && (transaction.Connection == null))
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            OracleCommand cmd = new OracleCommand();
            object retval;
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);
            retval = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return retval;
        }

        public override DAABDataReader ExecuteReader(ref DAABRequest Request)
        {
            DAABOracleDataReader oDataReaderSQL;
            string connectionString = Request.ConnectionString;
            if ((connectionString == null || connectionString.Length == 0))
            {
                throw new ArgumentNullException("connectionString");
            }
            try
            {
                IDbDataParameter[] lista = Request.Parameters.Cast<IDbDataParameter>().ToArray();
                OracleParameter[] aparam = ConvertToOracleParameter(Request.CommandType, lista).ToArray();               
                estableceConexion(false, connectionString);
                OracleDataReader drSQL;
                drSQL = ExecuteReaders(m_conecSQL, ((OracleTransaction)(null)), Request.CommandType, Request.Command, aparam);
                oDataReaderSQL = new DAABOracleDataReader();
                oDataReaderSQL.ReturnDataReader = drSQL;
                AsingValueParameter(aparam, ref lista);
            }
            catch (Exception ex)
            {
                Request.Exception = ex;
                if (!(m_conecSQL == null))
                {
                    m_conecSQL.Dispose();
                }
                throw ex;
            }
            finally
            {
                Log(Request);
            }

            return oDataReaderSQL;
        }

        private OracleDataReader ExecuteReaders(OracleConnection connection, OracleTransaction transaction, CommandType commandType, string commandText, OracleParameter[] commandParameters)
        {
            if ((connection == null))
            {
                throw new ArgumentNullException("connection");
            }
            bool mustCloseConnection = false;
            OracleCommand cmd = new OracleCommand();
            try
            {
                OracleDataReader dataReader;
                PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);
                //Ordenar parametros del Store Procedure
                cmd.BindByName = true;
                dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                bool canClear = true;
                foreach (OracleParameter commandParameter in cmd.Parameters)
                {
                    if (commandParameter.Direction != ParameterDirection.Input)
                    {
                        canClear = false;
                    }
                }
                if ((canClear))
                {
                    cmd.Parameters.Clear();
                }
                return dataReader;
            }
            catch (Exception ex1)
            {
                if ((mustCloseConnection))
                {
                    connection.Close();
                }
                throw ex1;
            }
        }

        public override void FillDataset(ref DAABRequest request)
        {
            string connectionString = request.ConnectionString;
            if ((connectionString == null || connectionString.Length == 0))
            {
                throw new ArgumentNullException("connectionString");
            }
            if ((request.Command == null || request.Command.Length == 0))
            {
                throw new ArgumentNullException("No ha ingresado el commando a ejecutar.");
            }
            if ((request.RequestDataSet == null))
            {
                throw new ArgumentNullException("RequestDataSet");
            }
            try
            {
                IDbDataParameter[] lista = request.Parameters.Cast<IDbDataParameter>().ToArray();
                estableceConexion(request.Transactional, connectionString);
                OracleParameter[] aparam = ConvertToOracleParameter(request.CommandType, lista).ToArray();
                if (request.Transactional)
                {
                    FillDatasets(m_conecSQL, m_TranSQL, request.CommandType, request.Command, request.RequestDataSet, request.TableNames, aparam);
                }
                else
                {
                    FillDatasets(m_conecSQL, ((OracleTransaction)(null)), request.CommandType, request.Command, request.RequestDataSet, request.TableNames, aparam);
                }
                AsingValueParameter(aparam, ref lista);
            }
            catch (Exception ex1)
            {
                request.Exception = ex1;
                throw ex1;
            }
            finally
            {
                if (!(request.Transactional) & !(m_conecSQL == null))
                {
                    m_conecSQL.Dispose();
                }
            }
        }

        private void FillDatasets(OracleConnection connection, OracleTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, OracleParameter[] commandParameters)
        {
            if ((connection == null))
            {
                throw new ArgumentNullException("connection");
            }
            if ((dataSet == null))
            {
                throw new ArgumentNullException("dataSet");
            }
            OracleCommand command = new OracleCommand();
            bool mustCloseConnection = false;
            PrepareCommand(command, connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);
            OracleDataAdapter dataAdapter = new OracleDataAdapter(command);
            try
            {
                if (!(tableNames == null && tableNames.Length > 0))
                {
                    string tableName = "Table";
                    int index = 0;
                    for (index = 0; index <= tableNames.Length - 1; index++)
                    {
                        if ((tableNames[index] == null || tableNames[index].Length == 0))
                        {
                            throw new ArgumentException("The tableNames parameter must contain a list of tables, a value was provided as null or empty string.", "tableNames");
                        }
                        dataAdapter.TableMappings.Add(tableName, tableNames[index]);
                        tableName = tableName + (index + 1).ToString();
                    }
                }
                dataAdapter.Fill(dataSet);
                command.Parameters.Clear();
            }
            finally
            {
                if ((!(dataAdapter == null)))
                {
                    dataAdapter.Dispose();
                }
            }
            if ((mustCloseConnection))
            {
                connection.Close();
            }
        }

        public override void UpdateDataSet(ref DAABRequest RequestInsert, ref DAABRequest RequestUpdate, ref DAABRequest RequestDelete)
        {
            string connectionString = RequestInsert.ConnectionString;
            OracleCommand cmdCommandInsert;
            OracleCommand cmdCommandUpdate;
            OracleCommand cmdCommandDelete;
            if ((connectionString == null || connectionString.Length == 0))
            {
                throw new ArgumentNullException("connectionString");
            }
            if ((RequestInsert.Command == null || RequestInsert.Command.Length == 0))
            {
                throw new ArgumentNullException("No ha ingresado el commando a ejecutar.RequestInsert");
            }
            if ((RequestUpdate.Command == null || RequestUpdate.Command.Length == 0))
            {
                throw new ArgumentNullException("No ha ingresado el commando a ejecutar.RequestUpdate");
            }
            if ((RequestDelete.Command == null || RequestDelete.Command.Length == 0))
            {
                throw new ArgumentNullException("No ha ingresado el commando a ejecutar.RequestDelete");
            }
            if ((RequestInsert.RequestDataSet == null))
            {
                throw new ArgumentNullException("RequestDataSet:RequestInsert");
            }
            if (RequestInsert.TableNames == null)
            {
                throw new ArgumentNullException("Falta especificar el nombre de la tabla a actualizar");
            }
            try
            {
                bool cerrarCn = false;
                IDbDataParameter[] lista = RequestInsert.Parameters.Cast<IDbDataParameter>().ToArray();
                estableceConexion(RequestInsert.Transactional, connectionString);
                cmdCommandInsert = new OracleCommand();

                OracleParameter[] aparamInsert = ConvertToOracleParameter(RequestInsert.CommandType, lista).ToArray();
                PrepareCommand(cmdCommandInsert, m_conecSQL, m_TranSQL, RequestInsert.CommandType, RequestInsert.Command, aparamInsert, ref cerrarCn);

                cmdCommandUpdate = new OracleCommand();
                OracleParameter[] aparamUpdate = ConvertToOracleParameter(RequestUpdate.CommandType, lista).ToArray();
                PrepareCommand(cmdCommandUpdate, m_conecSQL, m_TranSQL, RequestUpdate.CommandType, RequestUpdate.Command, aparamUpdate, ref cerrarCn);

                cmdCommandDelete = new OracleCommand();
                OracleParameter[] aparamDelete = ConvertToOracleParameter(RequestUpdate.CommandType, lista).ToArray();
                PrepareCommand(cmdCommandDelete, m_conecSQL, m_TranSQL, RequestDelete.CommandType, RequestDelete.Command, aparamDelete, ref cerrarCn);
                UpdateDatasets(cmdCommandInsert, cmdCommandDelete, cmdCommandUpdate, RequestInsert.RequestDataSet, RequestInsert.TableNames[0]);

                AsingValueParameter(aparamInsert, ref lista);

            }
            catch (Exception ex1)
            {
                RequestInsert.Exception = ex1;
                RequestDelete.Exception = ex1;
                RequestUpdate.Exception = ex1;
                throw ex1;
            }
            finally
            {
                if (!(RequestInsert.Transactional) & !(m_conecSQL == null))
                {
                    m_conecSQL.Dispose();
                }
            }
        }

        public void UpdateDatasets(OracleCommand insertCommand, OracleCommand deleteCommand, OracleCommand updateCommand, DataSet dataSet, string tableName)
        {
            if ((insertCommand == null))
            {
                throw new ArgumentNullException("insertCommand");
            }
            if ((deleteCommand == null))
            {
                throw new ArgumentNullException("deleteCommand");
            }
            if ((updateCommand == null))
            {
                throw new ArgumentNullException("updateCommand");
            }
            if ((dataSet == null))
            {
                throw new ArgumentNullException("dataSet");
            }
            if ((tableName == null || tableName.Length == 0))
            {
                throw new ArgumentNullException("tableName");
            }
            OracleDataAdapter dataAdapter = new OracleDataAdapter();
            try
            {
                dataAdapter.UpdateCommand = updateCommand;
                dataAdapter.InsertCommand = insertCommand;
                dataAdapter.DeleteCommand = deleteCommand;
                dataAdapter.Update(dataSet, tableName);
                dataSet.AcceptChanges();
            }
            finally
            {
                if ((!(dataAdapter == null)))
                {
                    dataAdapter.Dispose();
                }
            }
        }

        public override void CommitTransaction()
        {
            if (!(m_conecSQL == null && m_conecSQL.State == ConnectionState.Open && !(m_TranSQL == null)))
            {
                m_TranSQL.Commit();
                m_TranSQL = null;
                Dispose();
            }
        }

        public override void RollBackTransaction()
        {
            if (!(m_conecSQL == null && m_conecSQL.State == ConnectionState.Open && !(m_TranSQL == null)))
            {
                m_TranSQL.Rollback();
                m_TranSQL = null;
                Dispose();
            }
        }

        public override void Dispose()
        {
            if (!(m_conecSQL == null && (!((m_conecSQL.State == ConnectionState.Closed)) | !((m_conecSQL.State == ConnectionState.Broken)))))
            {
                if (m_conecSQL.State == ConnectionState.Open && !(m_TranSQL == null))
                {
                    m_TranSQL.Commit();
                    m_TranSQL = null;
                }
                m_conecSQL.Dispose();
            }
        }

        private void Log(DAABRequest request)
        {
            GeneradorLogII _objLog = new GeneradorLogII(request.Usuario, request.IdAplicacion, request.IdTransaccion, "DATOS");

            try
            {
                if (request.SaveLog || _objLog._flgUsuarioActual || request.Exception != null)
                {
                    List<string> objLog = new List<string>();
                    string strFileSource = string.Empty;

                    strFileSource = request.Trace.GetFrame(0).GetFileName();
                    if (strFileSource != null)
                    {
                        strFileSource = strFileSource.Replace('\\', '/');
                        strFileSource = strFileSource.Substring(strFileSource.LastIndexOf('/') + 1, strFileSource.Length - strFileSource.LastIndexOf('/') - 1);
                    }

                    objLog.Add(string.Format("[{0}][{1}]", strFileSource, Funciones.CheckStr(request.Trace.GetFrame(0).GetMethod().Name)));
                    objLog.Add(string.Format("[BD][{0}][{1}]", Funciones.CheckStr(request.BaseDatos), Funciones.CheckStr(request.Command)).ToUpper());

                    foreach (var obj in request.Parameters)
                    {
                        dynamic objParametro = Convert.ChangeType(obj, obj.GetType());

                        objLog.Add(string.Format("[{0}={1}]", Funciones.CheckStr(objParametro.ParameterName).ToLower(), objParametro.Value));
                    }

                    _objLog.CrearArchivolog(null, objLog, request.Exception);
                }
            }
            catch
            {

            }
        }

        private void Log(DAABRequest request, DataSet ds)
        {
            GeneradorLogII _objLog = new GeneradorLogII(null, request.IdAplicacion, request.IdTransaccion, "DATOS");

            try
            {
                if (request.Exception != null)
                {
                    List<string> lstLog = new List<string>();
                    lstLog.Add(string.Format("[][{0}]", Funciones.CheckStr(request.Trace.GetFrame(0).GetMethod().Name)));
                    lstLog.Add(string.Format("[BD][{0}][{1}]", Funciones.CheckStr(request.BaseDatos), Funciones.CheckStr(request.Command)).ToUpper());

                    foreach (var obj in request.Parameters)
                    {
                        dynamic objParametro = Convert.ChangeType(obj, obj.GetType());
                        lstLog.Add(string.Format("[{0}={1}]", Funciones.CheckStr(objParametro.ParameterName).ToLower(), objParametro.Value));
                    }

                    _objLog.CrearArchivolog(null, lstLog, null);
                    _objLog.CrearArchivolog(null, ds, request.Exception);
                }
                else if (request.SaveLog && request.SaveLogDataSet && request.Trace != null)
                {
                    _objLog.CrearArchivolog(null, ds, null);
                }
            }
            catch
            {

            }
        }

    }
}
