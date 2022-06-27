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
                    <td width="8%" style="font-family: 'Times New Roman', Times, serif; font-size: x-small">RUC Emisor:</td>
                    <td width="12%">
                        <asp:TextBox ID="txtRucEmisor" runat="server" Width="70px" Font-Size="X-Small"></asp:TextBox>
                    </td>
                    <td width="8%" style="font-family: 'Times New Roman', Times, serif; font-size: x-small">Tipo Doc.:</td>
                    <td width="17%">
                        <asp:DropDownList ID="ddlTipoDocumento" runat="server" Width="150px" Font-Size="X-Small">
                        </asp:DropDownList>
                    </td>
                    <td width="8%" style="font-family: 'Times New Roman', Times, serif; font-size: x-small">Serie Doc.:</td>
                    <td width="17%">
                        <asp:TextBox ID="txtSerieDocumento" runat="server" Width="50px" Font-Size="X-Small"></asp:TextBox>
                        
                    </td>
                    <td width="30%" align="right">
                        <asp:RequiredFieldValidator runat="server" id="reqRUC" CssClass="failureNotification" controltovalidate="txtRucEmisor" errormessage="Por favor, ingrese el RUC del Emisor!" Font-Size="XX-Small" />
                        <asp:RequiredFieldValidator CssClass="failureNotification" runat="server" id="reqSerie" controltovalidate="txtSerieDocumento" errormessage="Por favor, ingrese la Serie!" Font-Size="XX-Small" /> 
                    </td>
                </tr>
                <tr valign="top">
                    <td style="font-family: 'Times New Roman', Times, serif; font-size: x-small">Nro Doc.:</td>
                    <td >
                        <asp:TextBox ID="txtNroDocumento" runat="server" Width="70px" Font-Size="X-Small"></asp:TextBox>
                        
                    </td>
                    <td style="font-family: 'Times New Roman', Times, serif; font-size: x-small">Monto Factura:</td>
                    <td >
                        <asp:TextBox ID="txtMontoFactura" runat="server" Width="100" Font-Size="X-Small"></asp:TextBox>
                        
                    </td>
                    <td style="font-family: 'Times New Roman', Times, serif; font-size: x-small">Fecha Emisión:</td>
                    <td >
                        <asp:TextBox ID="txtFechaEmision" runat="server" Width="70px" Font-Size="X-Small"></asp:TextBox>
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
                    <td align="right">
                        <asp:RequiredFieldValidator CssClass="failureNotification" runat="server" id="reqNroDocumento" controltovalidate="txtNroDocumento" errormessage="Por favor, ingrese el Nro de documento!" Font-Size="XX-Small" />
                        <asp:RequiredFieldValidator CssClass="failureNotification" runat="server" id="reqMonto" controltovalidate="txtMontoFactura" errormessage="Por favor, ingrese el Monto del documento!" Font-Size="XX-Small" />
                        
                    </td>
                </tr>
                <tr valign="top" align="left">
                    <td colspan="7">
                        <asp:Button ID="btnVerFactura"  runat="server" Text="Ver Factura"   OnClick="btnVerFactura_Click" Width="100px" BackColor="Aqua" Font-Size="X-Small" />
                        <asp:Button ID="btnExportarXML" runat="server" Text="Exportar XML" onclick="btnExportarXML_Click" Width="100px" BackColor="Blue" Font-Size="X-Small" />
                        <asp:Button ID="btbExportarPDF" runat="server" Text="Exportar PDF" onclick="btbExportarPDF_Click" Width="100px" BackColor="#0066FF" Font-Size="X-Small" />
                        
                    </td>

                </tr>
            </tbody>
            </table>
            <div class="main">
                <iframe src="" width="100%" height="400" style="border: none;" runat="server" id="iframePDF"></iframe>
            </div>
            <div class="footer">
                <table width="100%">
                    <tr><td>(c) 2018 HelpDesk. All Rights Reserved.</td>
                    </tr>
                </table>
            </div>
            
        </div>
    </form>
</body>
</html>