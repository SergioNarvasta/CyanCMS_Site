using System.Data;

namespace HDFacturacionDTE.IData
{
    public class DAABOracleDataReader : DAABDataReader
    {
        IDataReader m_oReturnedDataReader;

        public override System.Data.IDataReader ReturnDataReader
        {
            get
            {
                return m_oReturnedDataReader;
            }
            set
            {
                m_oReturnedDataReader = value;
            }
        }
    }
}
