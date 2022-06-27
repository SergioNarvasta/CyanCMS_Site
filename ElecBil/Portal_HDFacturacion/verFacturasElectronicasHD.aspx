<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="verFacturasElectronicasHD.aspx.cs" Inherits="Portal_HDFacturacion.verFacturasElectronicasHD" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <!-- Font Awesome -->
    <link
      href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css"
      rel="stylesheet"
    />
    <!-- Google Fonts -->
    <link
      href="https://fonts.googleapis.com/css?family=Roboto:300,400,500,700&display=swap"
      rel="stylesheet"
    />
    <!-- MDB -->
    <link
      href="https://cdnjs.cloudflare.com/ajax/libs/mdb-ui-kit/3.3.0/mdb.min.css"
      rel="stylesheet"
    />
</head>
<body>
<div class="all-content-wrapper">
   <%-- <div class="row">
    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
        <div class="panel panel-primary">
            <div class="panel-heading">
                Monitor Gerencial Copper
                <div class="panel-controls">
                    <a href="javascript:void(0);" data-panel-loading="true" data-loading-effect="timer" data-toggle="tooltip" data-title="Refresh" data-placement="bottom" data-original-title="" title=""><i class="fa fa-refresh"></i></a>
                </div>
            </div>
            <div class="panel-body">
                <div class="form-group col-sm-6 col-xs-12 text-center">
                    <br />
                    <a id="btnCapturar" class="m-w-150 btn btn-rounded btn-primary" data-panel-loading="false" data-loading-effect="timer" data-loading-color="#16a085" data-toggle="tooltip" data-placement="bottom">Captura</a>
                    <a id="btnEnviarCorreo" class="m-w-150 btn btn-rounded btn-primary">Enviar</a>
                    <a id="btnConsultaHoy" class="m-w-150 btn btn-rounded btn-default">Consultar</a>
                </div>
            </div>

        </div>
    </div>
</div>--%>

     <div class="card">
      <div class="card-body">
        <h5 class="card-title">PORTAL FACTURACION</h5>
            <form id="Form1" runat="server">
                <div class="row">
                     <div class="form-group col-sm-4">
                        <label>RUC Emisor:</label>
                         <asp:TextBox ID="txtRucEmisor" CssClass="form-control"  runat="server" Width="50%"></asp:TextBox>
                    </div>
                     <div class="form-group col-sm-4">
                        <label>Tipo Doc.:</label>
                         <asp:DropDownList ID="ddlTipoDocumento" CssClass="form-control" runat="server" Width="50%">
                                    </asp:DropDownList>
                    </div>
                    <div class="form-group col-sm-4">
                        <label>Nro Doc.:</label>
                        <asp:TextBox ID="txtNroDocumento" CssClass="form-control"  runat="server" Width="50%"></asp:TextBox>
                    </div>
                </div>
                 <div class="row">
                    <div class="form-group col-sm-4">
                        <label>Serie Doc.:</label>
                        <asp:TextBox ID="txtSerieDocumento" CssClass="form-control"  runat="server" Width="50%"></asp:TextBox>
                        
                    </div>
                    <div class="form-group col-sm-4">
                        <label>Monto Factura:</label>
                        <asp:TextBox ID="txtMontoFactura" CssClass="form-control"  runat="server" Width="50%" ></asp:TextBox>
                    </div>
                    <div class="form-group col-sm-4">
                        <label>Fecha Emisión:</label>
                        <asp:TextBox type="date" ID="txtFechaEmision" CssClass="form-control"  runat="server" Width="50%" ></asp:TextBox>
                    </div>

               </div>
                <div class="row">
                    <asp:RequiredFieldValidator CssClass="failureNotification" runat="server" id="reqNroDocumento" controltovalidate="txtNroDocumento" errormessage="Por favor, ingrese el Nro de documento!" Font-Size="XX-Small" />
                    <asp:RequiredFieldValidator CssClass="failureNotification" runat="server" id="reqMonto" controltovalidate="txtMontoFactura" errormessage="Por favor, ingrese el Monto del documento!" Font-Size="XX-Small" />
                </div>
                <div class="row">
                     <asp:Button ID="btnVerFactura"  CssClass="btn btn-primary"   runat="server" Text="Ver Factura"   OnClick="btnVerFactura_Click" Width="100px" Font-Size="X-Small" />
                     <asp:Button ID="btnExportarXML" CssClass="btn btn-info"  runat="server" Text="Exportar XML" onclick="btnExportarXML_Click" Width="100px" Font-Size="X-Small" />
                     <asp:Button ID="btbExportarPDF" CssClass="btn btn-primary"  runat="server" Text="Exportar PDF" onclick="btbExportarPDF_Click" Width="100px" Font-Size="X-Small" />
                        
                </div>     
                <div class="main">
                        <iframe src="" width="100%" height="400" style="border: none;" runat="server" id="iframePDF"></iframe>
                    </div>
                    <div class="footer">
                        <table width="100%">
                            <tr><td>(c) 2018 HelpDesk. All Rights Reserved.</td>
                            </tr>
                        </table>
                    </div>
                <div class="main">
                    <table width="100%">
                    <tbody>
                        <tr valign="top">
                            
                            <td width="30%" align="right">
                                <asp:RequiredFieldValidator runat="server" id="reqRUC" CssClass="failureNotification" controltovalidate="txtRucEmisor" errormessage="Por favor, ingrese el RUC del Emisor!" Font-Size="XX-Small" />
                                <asp:RequiredFieldValidator CssClass="failureNotification" runat="server" id="reqSerie" controltovalidate="txtSerieDocumento" errormessage="Por favor, ingrese la Serie!" Font-Size="XX-Small" /> 
                            </td>
                        </tr>
                        <tr valign="top">
                            <td style="font-family: 'Times New Roman', Times, serif; font-size: x-small">Nro Doc.:</td>
                            <td >
                                
                        
                            </td>
                            <td style="font-family: 'Times New Roman', Times, serif; font-size: x-small">Monto Factura:</td>
                            <td >
                                
                        
                            </td>
                            <td style="font-family: 'Times New Roman', Times, serif; font-size: x-small">Fecha Emisión:</td>
                            <td >
                                
                                <%--<asp:ImageButton ID="imgbCalendario" runat="server"  
                                    ImageUrl="~/images/calendar_1.jpg" Height="20px" 
                                    onclick="imgbCalendario_Click" />
                                <div id="divCalendario" style="display:none" runat="server">
                                    <br />
                                    <asp:Calendar ID="cldFechaEmision"  SelectionMode="Day"
                                            ShowGridLines="True" runat="server" 
                                        onselectionchanged="cldFechaEmision_SelectionChanged" >
                                        <SelectedDayStyle BackColor="Yellow"
                                               ForeColor="Red">
                                         </SelectedDayStyle>
                                    </asp:Calendar>
                                </div>--%>
                            </td>
                            <td align="right">
                               
                            </td>
                        </tr>
                        <tr valign="top" align="left">
                            <td colspan="7">
                               
                            </td>

                        </tr>
                    </tbody>
                    </table>
                    
            
                </div>
            </form>
      </div>
    </div>
    </div>
   
</body>
</html>