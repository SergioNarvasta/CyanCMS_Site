<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="verFacturasElectronicasHD_V2.aspx.cs" Inherits="Portal_HDFacturacion.verFacturasElectronicasHD_V2" %>

<!DOCTYPE html>
<html lang="en">

<!-- Mirrored from adminbsb-sensitive.firebaseapp.com/pages/examples/blank-page.html by HTTrack Website Copier/3.x [XR&CO'2014], Fri, 03 May 2019 16:28:24 GMT -->
<!-- Added by HTTrack -->
<meta http-equiv="content-type" content="text/html;charset=utf-8" /><!-- /Added by HTTrack -->
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>HDSoft | Consultas</title>

    <!-- Favicon -->
    <link rel="icon" href="favicon.ico" type="image/x-icon">

    <!-- Google Fonts -->
    <link href="https://fonts.googleapis.com/css?family=Open+Sans:400,600,700" rel="stylesheet">
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet" type="text/css">
    <%--<link href="~/Assets/css/icon.css" rel="stylesheet" />--%>

    <!-- Bootstrap Core Css -->
    <link href="Assets/plugins/bootstrap/dist/css/bootstrap.css" rel="stylesheet" />

    <!-- Animate.css Css -->
    <link href="Assets/plugins/animate-css/animate.css" rel="stylesheet" />

    <!-- Font Awesome Css -->
    <link href="Assets/plugins/font-awesome/css/font-awesome.min.css" rel="stylesheet" />

    <!-- iCheck Css -->
    <link href="Assets/plugins/iCheck/skins/flat/_all.css" rel="stylesheet" />

    <!-- Switchery Css -->
    <link href="Assets/plugins/switchery/dist/switchery.css" rel="stylesheet" />

    <!-- Metis Menu Css -->
    <link href="Assets/plugins/metisMenu/dist/metisMenu.css" rel="stylesheet" />

    <!-- Pace Loader Css -->
    <link href="Assets/plugins/pace/themes/white/pace-theme-flash.css" rel="stylesheet" />

    <!-- Toastr Css -->
    <link href="Assets/plugins/toastr/toastr.css" rel="stylesheet" />

    <!-- WaitMe Css -->
    <link href="Assets/plugins/wait-me/src/waitMe.css" rel="stylesheet" />

    <!-- Custom Css -->
    <link href="Assets/css/style.css" rel="stylesheet" />

    <!-- AdminBSB Themes. You can choose a theme from css/themes instead of get all themes -->
    <link href="Assets/css/themes/allthemes.css" rel="stylesheet" />

</head>
<body class="theme-blue">
<div class="all-content-wrapper">

            <div class="page-body">
               
                <div class="row clearfix">
                    <div class="col-sm-12">
                        <div class="panel panel-default">
                            <div class="panel-body panel-body-success">
                                <div class="align-center">
                                    <h3>PORTAL DE FACTURACION ELECTRONICA</h3> 
                                </div>
                                <div class="form-group col-sm-6 col-xs-12 text-center">
                                                         <br />
                                                         <a id="btnCapturar" class="m-w-150 btn btn-rounded btn-primary" data-panel-loading="false" data-loading-effect="timer" data-loading-color="#16a085" data-toggle="tooltip" data-placement="bottom">Captura</a>
                                                         <a id="btnEnviarCorreo" class="m-w-150 btn btn-rounded btn-primary">Enviar</a>
                                                         <a id="btnConsultaHoy" class="m-w-150 btn btn-rounded btn-default">Consultar</a>
                                                     </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="panel panel-default">
                            <div class="panel-body panel-body-danger">
                                <div class="align-justify">
                                    Lorem ipsum dolor sit amet, in nam laudem euismod, democritum omittantur et per, et lobortis conclusionemque mei. Officiis maiestatis in mei, in justo saperet nonumes mea. Nam putant nusquam eu.
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row clearfix">
                    <div class="col-sm-6">
                        <div class="panel panel-default">
                            <div class="panel-body panel-body-primary">
                                <div class="align-justify">
                                    Lorem ipsum dolor sit amet, in nam laudem euismod, democritum omittantur et per, et lobortis conclusionemque mei. Officiis maiestatis in mei, in justo saperet nonumes mea. Nam putant nusquam eu.
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="panel panel-default">
                            <div class="panel-body panel-body-warning">
                                <div class="align-justify">
                                    Lorem ipsum dolor sit amet, in nam laudem euismod, democritum omittantur et per, et lobortis conclusionemque mei. Officiis maiestatis in mei, in justo saperet nonumes mea. Nam putant nusquam eu.
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
    








</div>


  <!-- Jquery Core Js -->
    <script src="Assets/plugins/jquery/dist/jquery.min.js"></script>

    <!-- Bootstrap Core Js -->
    <script src="Assets/plugins/bootstrap/dist/js/bootstrap.min.js"></script>

    <!-- Pace Loader Js -->
    <script src="Assets/plugins/pace/pace.js"></script>

    <!-- Screenfull Js -->
    <script src="Assets/plugins/screenfull/src/screenfull.js"></script>

    <!-- Metis Menu Js -->
    <script src="Assets/plugins/metisMenu/dist/metisMenu.js"></script>

    <!-- Jquery Slimscroll Js -->
    <script src="Assets/plugins/jquery-slimscroll/jquery.slimscroll.js"></script>

    <!-- waitMe Js -->
    <script src="Assets/plugins/wait-me/src/waitMe.js"></script>

    <!-- Switchery Js -->
    <script src="Assets/plugins/switchery/dist/switchery.js"></script>

    <!-- Toastr Js -->
    <script src="Assets/plugins/toastr/toastr.js"></script>

    <!-- Custom Js -->
    <script src="Assets/js/admin.js"></script>
    <script src="Assets/js/pages/examples/blank-page.js"></script>
    <script src="Assets/js/pages/typography.js"></script>
    <script src="Assets/js/pages/widgets/panel.js"></script>
    <script src="Assets/js/pages/widgets/panel-with-loading.js"></script>

    <script src="~/Assets/js/MessageConfirmation.js"></script>
    <script src="~/Assets/js/Funciones.js"></script>
    <script src="~/Scripts/jquery.validate.min.js"></script>
    <script src="~/Scripts/jquery.validate.unobtrusive.min.js"></script>
    <!-- Google Analytics Code -->
    <script src="Assets/js/google-analytics.js"></script>

   <%-- <!-- Demo Purpose Only -->
    <script src="Assets/js/demo.js"></script>--%>
</body>

    <!-- Mirrored from adminbsb-sensitive.firebaseapp.com/pages/examples/blank-page.html by HTTrack Website Copier/3.x [XR&CO'2014], Fri, 03 May 2019 16:28:25 GMT -->
</html>
