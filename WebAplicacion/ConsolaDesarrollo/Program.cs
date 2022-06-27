using HD_CONFORMIDAD_WS;
using HD_RVIE_WS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolaDesarrollo
{
    class Program
    {
        static void Main(string[] args)
        {
            //HDConformidadSUNAT objSunat = new HDConformidadSUNAT();
            //var token = objSunat.ConsultaConformidadPago(objSunat.GeneraToken());
           
            //PARA TOKENS
            string client_id = "97dee99e-984b-4369-a72e-fb03a6c3defa";
            string client_secret = "caWpscwQt8zFaK5t1kgftg==";
            string username = "20601647649EFACT001";
            string password = "factu2018";

            //Datos a ingresar para consultar pendientes 
            DateTime FecIniPND = Convert.ToDateTime("25/01/2022");
            DateTime FecFinPND = DateTime.Now;
            string codMonedaPND = "";
            string cod_estado = "04";
            string ind_contribuyente = "V";
            string numCpe = "";
            string numRuc = "";
            string codAdqui = "6";


            //Datos a ingresar para enviar conformidad (ruta + archivo valido)
            string direccorio = @"D:\Datos\wgnegron\Escritorio\CONFORMIDAD\collection SAC - 20220000006\";
            string rut_archivo = "20601647649-PND-20220119-10.txt";

            //Datos a ingresar para Consultar tickets

            string fecEnvioInicial = "2022-01-01";
            string fecEnvioFinal = "2022-12-31";
            string codTicketMasivo = "";


            RegistroVentasRVIE objSunat = new RegistroVentasRVIE();
            //ConformidadCPE objSunat = new ConformidadCPE();
            objSunat.Client_id = "97dee99e-984b-4369-a72e-fb03a6c3defa";
            objSunat.Client_secret = "caWpscwQt8zFaK5t1kgftg==";
            objSunat.Username = "20601647649EFACT001";
            objSunat.Password = "factu2018";

            //----------------RIVE---------------

            int Ps_Periodo = 202204;
            int Ps_TipoResumen = 1;
            int Ps_TipoArchivo = 0;


            //Datos para Consultar Ws01
            int ps_periodo = 202204;
            int ps_codTipoOpe = 1;
            string ps_mtoDesde = "";
            string ps_mtoHasta = "";
            string ps_fecEmisionIni ="";
            string ps_fecEmisionFin = "";
            string ps_numDocAdquiriente  = "";
            string ps_codCar             = "";
            string ps_codTipoCDP         = "";
            string ps_codInconsistencia  = "";


           

            //var token = objSunat.GeneraToken(client_id, client_secret, username, password);
            var token = objSunat.GeneraToken();
            var docresumen = objSunat.Ws07_ConsultaResumenCPE(Ps_Periodo, Ps_TipoResumen, Ps_TipoArchivo);
            //var DocPND = objSunat.ConsultaComprobantesPND(objSunat.GeneraToken(client_id, client_secret, username, password), FecIniPND, FecFinPND, codMonedaPND);
            //var DocPND = objSunat.EnviaConformidad(direccorio, rut_archivo);
            //var ticket = objSunat.ConsultaTicketsPND(fecEnvioInicial, fecEnvioFinal, codTicketMasivo);
            //-- var DocPND1 = objSunat.RegistroVentasRVIE( FecIniPND,  FecFinPND,  codMonedaPND,  cod_estado,  ind_contribuyente,  numCpe,  numRuc , codAdqui);
            //var DocPND = objSunat.EnviaConformidad(direccorio, rut_archivo);

           // var doc = objSunat.WS01_ConsultaDetalleCPE(ps_periodo, ps_codTipoOpe, ps_mtoDesde, ps_mtoHasta, ps_fecEmisionIni, ps_fecEmisionFin, ps_numDocAdquiriente, ps_codCar, ps_codTipoCDP, ps_codInconsistencia);
           var doc = objSunat.WS01_ConsultaDetalleCPE2(ps_periodo, ps_codTipoOpe);

            Console.WriteLine(doc);



        }
    }
}
