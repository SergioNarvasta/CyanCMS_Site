namespace HDFacturacionDTE.Entity
{
    public class BEItemGenerico
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Codigo2 { get; set; }
        public string Estado { get; set; }
        public int Orden { get; set; }
        public string Numero { get; set; }
        public string Tipo { get; set; }
        public string Descripcion2 { get; set; }
        public double Monto { get; set; }
        public string Fecha2 { get; set; }
        public string Fecha { get; set; }
        public string Codigo3 { get; set; }
        public double Valor { get; set; }
        public string Codigo4 { get; set; }
        public string FlagSistema { get; set; }
        public string Orden1 { get; set; }
        public string Nombre { get; set; }
        public string CodigoDistrito { get; set; }
        public string CodigoDepart { get; set; }
        public string CodigoProvincia { get; set; }
        public string DescDistrito { get; set; }

        public BEItemGenerico()
        {

        }
        public BEItemGenerico(string vCodigo, string vDescripcion)
        {
            Codigo = vCodigo;
            Descripcion = vDescripcion;
            Codigo2 = "";
            Estado = "";
        }

        public BEItemGenerico(string vCodigo, string vDescripcion, string vEstado)
        {
            Codigo = vCodigo;
            Descripcion = vDescripcion;
            Estado = vEstado;
        }

        public BEItemGenerico(string vCodigo, string vCodigo2, string vDescripcion, string vEstado)
        {
            Codigo = vCodigo;
            Codigo2 = vCodigo2;
            Descripcion = vDescripcion;
            Estado = vEstado;
        }

    }
}
