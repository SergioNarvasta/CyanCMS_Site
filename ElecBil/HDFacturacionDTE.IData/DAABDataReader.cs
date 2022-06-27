using System.Data;

namespace HDFacturacionDTE.IData
{
     public abstract class DAABDataReader
     {
        public abstract IDataReader ReturnDataReader
        {
            get;
            set;
        }
    }
}
