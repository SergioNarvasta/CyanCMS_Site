using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultaSunat
{
    class Program
    {
        static void Main(string[] args)
        {
            HDConsultaSunat objSunat = new HDConsultaSunat();
            var token = objSunat.ConsultaConformidadPago(objSunat.GeneraToken());

            string cadenaEncriptada = Encrypt.GetSHA256("patito");


        }
    }
}
