using System.Web;
using System.Web.Optimization;

namespace Portal_HDFacturacion_V2
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            /**CSS*/
            bundles.Add(new StyleBundle("~/Public/css").Include(
                //<!-- Bootstrap Core Css -->
                "~/Assets/plugins/bootstrap/dist/css/bootstrap.css",

                //<!-- Animate.css Css -->
                "~/Assets/plugins/animate-css/animate.css",

                //<!-- Font Awesome Css -->
                "~/Assets/plugins/font-awesome/css/font-awesome.min.css",

                //<!-- iCheck Css -->
                "~/Assets/plugins/iCheck/skins/flat/_all.css",

                //<!-- Switchery Css -->
                "~/Assets/plugins/switchery/dist/switchery.css",

                //<!-- Metis Menu Css -->
                "~/Assets/plugins/metisMenu/dist/metisMenu.css",

                //<!-- Pace Loader Css -->
                "~/Assets/plugins/pace/themes/white/pace-theme-flash.css",

                //<!-- Bootstrap Select Css -->
                "~/Assets/plugins/bootstrap-select/dist/css/bootstrap-select.css",

                //<!-- Toastr Css -->
                "~/Assets/plugins/toastr/toastr.css",

                //<!-- WaitMe Css -->
                "~/Assets/plugins/wait-me/src/waitMe.css",

                //<!-- Custom Css -->
                "~/Assets/css/style.css"
            ));


            /**Javascript*/
            bundles.Add(new ScriptBundle("~/Public/js").Include(
                 //<!-- Jquery Core Js -->
                "~/Assets/plugins/jquery/dist/jquery.min.js",

                //<!-- Bootstrap Core Js -->
                "~/Assets/plugins/bootstrap/dist/js/bootstrap.min.js",

                //<!-- Pace Loader Js -->
                "~/Assets/plugins/pace/pace.js",

                //<!-- Screenfull Js -->
                "~/Assets/plugins/screenfull/src/screenfull.js",

                //<!-- Metis Menu Js -->
                "~/Assets/plugins/metisMenu/dist/metisMenu.js",

                //<!-- Jquery Slimscroll Js -->
                "~/Assets/plugins/jquery-slimscroll/jquery.slimscroll.js",

                //<!-- waitMe Js -->
                "~/Assets/plugins/wait-me/src/waitMe.js",

                //<!-- Switchery Js -->
                "~/Assets/plugins/switchery/dist/switchery.js",

                //<!-- Bootstrap Select Js -->
                "~/Assets/plugins/bootstrap-select/dist/js/bootstrap-select.js",
                  
                //<!-- Masked Input Js -->
                "~/Assets/plugins/jquery.inputmask/dist/jquery.inputmask.bundle.js",
                //<!-- Toastr Js -->
                "~/Assets/plugins/toastr/toastr.js",

                //<!-- Custom Js -->
                "~/Assets/js/admin.js",

                "~/Assets/js/MessageConfirmation.js"
                //"~/Scripts/jquery.validate.min.js",
                //"~/Scripts/jquery.validate.unobtrusive.min.js"

            ));












            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //// Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            //// para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));
        }
    }
}
