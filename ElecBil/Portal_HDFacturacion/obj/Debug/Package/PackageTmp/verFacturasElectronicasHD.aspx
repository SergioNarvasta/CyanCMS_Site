<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="verFacturasElectronicasHD.aspx.cs" Inherits="Portal_HDFacturacion.verFacturasElectronicasHD" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="Form1" runat="server">
        <div class="main">
            <table width="100%">
            <tbody>
                <tr valign="top">
                    <td width="10%">RUC Emisor:</td>
                    <td width="25%">
                        <asp:TextBox ID="txtRucEmisor" runat="server" Width="120"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="reqRUC" CssClass="failureNotification" controltovalidate="txtRucEmisor" errormessage="Por favor, ingrese el RUC del Emisor!" />
                    </td>
                    <td width="10%">Tipo Doc.:</td>
                    <td width="25%">
                        <asp:DropDownList ID="ddlTipoDocumento" runat="server" Width="220px">
                        </asp:DropDownList>
                    </td>
                    <td width="10%">Serie Doc.:</td>
                    <td width="20%">
                        <asp:TextBox ID="txtSerieDocumento" runat="server" Width="50px"></asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="failureNotification" runat="server" id="reqSerie" controltovalidate="txtSerieDocumento" errormessage="Por favor, ingrese la Serie!" />
                    </td>
                </tr>
                <tr valign="top">
                    <td >Nro Doc.:</td>
                    <td >
                        <asp:TextBox ID="txtNroDocumento" runat="server" Width="120"></asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="failureNotification" runat="server" id="reqNroDocumento" controltovalidate="txtNroDocumento" errormessage="Por favor, ingrese el Nro de documento!" />
                    </td>
                    <td>Monto Factura:</td>
                    <td>
                        <asp:TextBox ID="txtMontoFactura" runat="server" Width="120"></asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="failureNotification" runat="server" id="reqMonto" controltovalidate="txtMontoFactura" errormessage="Por favor, ingrese el Monto del documento!" />
                    </td>
                    <td >Fecha Emisión:</td>
                    <td >
                        <asp:TextBox ID="txtFechaEmision" runat="server" Width="100px"></asp:TextBox>
                        <asp:ImageButton ID="imgbCalendario" runat="server"  
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
                        </div>
                    </td>
                </tr>
                <tr valign="top" align="right">
                    <td colspan="6"><asp:Button ID="btnVerFactura" runat="server" Text="Ver Factura" 
                            onclick="btnVerFactura_Click" /></td>
                </tr>
            </tbody>
            </table>
            <div class="main">
                <iframe src="" width="100%" height="415" style="border: none;" runat="server" id="iframePDF"></iframe>
            </div>
            <div class="footer">
                <table width="100%">
                    <tr>
                        <td width="50%">
                            <asp:Button ID="btnExportarXML" runat="server" Text="Exportar XML" 
                                onclick="btnExportarXML_Click" />
                        </td>
                        <td width="50%">
                            <asp:Button ID="btbExportarPDF" runat="server" Text="Exportar PDF" 
                                onclick="btbExportarPDF_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            
        </div>
    </form>
</body>
</html>