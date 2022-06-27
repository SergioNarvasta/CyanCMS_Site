using System;
using Microsoft.Win32;

namespace HDFacturacionDTE.Base
{
    public class Configuracion
    {
        private string _strLlaveNombre;

        public Configuracion(string llaveNombre)
        {
            if (string.IsNullOrEmpty(llaveNombre))
            {
                throw new Exception("No se ha especificado la llave del registro.");
            }

            this._strLlaveNombre = llaveNombre;
        }

        #region LeerValor

        private string LeerValor(string valorNombre)
        {
            //return Registry.LocalMachine.OpenSubKey(@"SOFTWARE\HDServ\" + this._strLlaveNombre).GetValue(valorNombre, "").ToString();
            string sRutaKey = @"SOFTWARE\HDServ\" + this._strLlaveNombre + @"\";
            //string sRutaKey = @"W:\Proyectos NET\Portal_HDFacturacion\RegEdit\" + this._strLlaveNombre + @"\";
            RegistryKey rkLocalMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey rkKey = rkLocalMachine.OpenSubKey(sRutaKey);
            string sValorKey = rkKey.GetValue(valorNombre, "").ToString();
            return sValorKey;
        }

        #endregion


        #region LeerProveedor

        public string LeerProveedor()
        {
            return LeerValor("Provider");
        }

        #endregion

        #region LeerBaseDatos

        public string LeerBaseDatos()
        {
            return LeerValor("BD_Activa");
        }

        #endregion

        #region LeerServidor

        public string LeerServidor()
        {
            return LeerValor("Server");
        }

        #endregion

        #region LeerUsuario

        public string LeerUsuario()
        {
            return LeerValor("User");
        }

        #endregion

        #region LeerContrasena

        public string LeerContrasena()
        {
            return LeerValor("Password");
        }

        #endregion

        #region LeerPuerto

        public string LeerPuerto()
        {
            return LeerValor("Port");
        }

        #endregion

    }
}
