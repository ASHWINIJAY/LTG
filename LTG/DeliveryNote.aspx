<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeliveryNote.aspx.cs" Inherits="LTG.DeliveryNote" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href='https://fonts.googleapis.com/css?family=Libre Barcode 39' rel='stylesheet' />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/2.5.1/jspdf.umd.min.js"></script>

    <script type="text/javascript">
        function printPage() {
            window.print();
        }
        </script>
     <style type="text/css">
        .auto-style1 {
            width: 70%;
            height: 23px;
        }
    </style>
    <style>
        
       @page {
    margin: 0; /* Removes the default margins */
}


@media print {
    /* Optionally, hide elements */
    .no-print {
        display: none;
    }
}
@page {
            size: A4;
            margin: 0;
        }
    </style>
   
    </head>
   <body>
       <button id="download">Download PDF</button>
        <div id="divPrint" runat="server">
       <form id="frm" runat="server">
          
        <div style="text-align:center;">
            <table style="width:99%;height:230px;border-spacing:0;border-collapse:collapse;border:1px solid blue;">
                <tr>
                    <td style="vertical-align:top;">
                        <table style="width:100%;height:65px;border:1px solid blue;border-left:hidden;border-top:hidden;border-right:hidden;">
                            <tr>
                                <td colspan="2" style="border:0px solid blue;border-left:hidden;border-top:hidden;border-right:hidden;text-align:right;font-size:9px;display:none;">
                                     Page 1 of 1
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="border:1px solid blue;border-left:hidden;border-top:hidden;border-right:hidden;">
                                   <asp:Image ID="img" runat="server" ImageUrl="~/assets/img/ltgpanel_new2.png" Height="170" />
                                </td>
                            </tr>
                            <tr>
                                 <td style="width:50%;border:1px solid blue;border-left:hidden;border-top:hidden;border-bottom:hidden;">
                                     <table style="width:100%">
                                         <tr>
                                             <td style="text-align:left;width:100%;vertical-align:top;">
                                                 <span style="font-size:x-large;font-weight: bold;">Delivery Note</span></td>
                                                 <td style="text-align:left;width:100%;vertical-align:top;">
                                                 <asp:Image ID="BarcodeImage" runat="server" />
                                             <asp:Label ID="lblSlip" Visible="false" style="font-size:large;" runat="server"></asp:Label></td>
                                         </tr>
                                        
                                          <tr>
                                             <td style="width:70%;text-align:left">
                                                 <span style="font-size:12px;">Date:         </span><asp:Label ID="lblDate" runat="server" style="font-size:12px;" Text="19/Jun/2024 10:20 AM"></asp:Label>
                                             </td>
                                             <td style="text-align:left;"> <span style="font-size:12px;"> </span></td>
                                         </tr>
                                          <tr>
                                             <td style="width:70%;text-align:left">
                                                 <span style="font-size:12px;"> </span>  </td>
                                             <td style="text-align:left;"> <span style="font-size:12px;"></span></td>
                                         </tr>
                                     </table>
                                                                                                                              </td>
                    <td style="width:50%;">
                         <table style="width:100%">
                                         <tr>
                                             <td style="width:80%;text-align:left;">
                                                 <span style="font-size:large;font-weight: bold;">LTG LOGISTICS TRANSPORT GLOBALLY (PTY) LTD</span>
                                             </td>
                                             <td><span style="font-size:large;"></span></td>
                                         </tr>
                                         <tr>
                                             <td style="width:80%;text-align:left">
                                                 <span style="font-size:12px;">28 VAN EDEN CRESCENT</span>
                                             </td>
                                             <td></td>
                                            </tr>
                             <tr>
                                             <td style="text-align:left;"><span style="font-size:12px;">Tel:087 234-9757</span></td>
                                 <td></td>
                                         </tr>
                                          <tr>
                                             <td style="width:70%;text-align:left">
                                                 <span style="font-size:12px;">ROSSYLN EAST         </span></td>
                                             <td style="text-align:left;"> 
                                   
                                </td>
                                         </tr>
                              <tr>
                                             <td style="width:70%;text-align:left">
                                                 <span style="font-size:12px;">0200         </span></td>
                                             <td style="text-align:left;"> <span style="font-size:12px;">         </span></td>
                                         </tr>
                                         
                                     </table>
                    </td>
                            </tr>
                        </table>
                    </td>
                   
                </tr>
                <tr>
                    <td style="vertical-align:top;">
                        <table style="width:100%;height:50px;border-bottom:1px solid blue;">
                            <tr>
                                <td style="width:33%;border-right:1px solid blue;">
                                     <table style="width:100%">
                                         <tr>
                                             <td style="width:70%;text-align:left;font-size:12px;font-weight:bold;">
                                                 Receiver
                                                
                                             </td>
                                            
                                             
                                         </tr>
                                          <tr>
                                             
                                             <td style="width:70%;text-align:left;font-size:12px;">
                                                  <asp:Label ID="lblCustomer"  runat="server"></asp:Label>
                                                
                                             </td>
                                             
                                         </tr>
                                          <tr>
                                             
                                             <td style="width:70%;text-align:left;font-size:12px;">
                                                 <asp:Label ID="lblAddr1"  runat="server"></asp:Label>
                                                
                                             </td>
                                             
                                         </tr>
                                          <tr>
                                             
                                             <td style="width:70%;text-align:left;font-size:12px;">
                                                  <asp:Label ID="lblAddr2"  runat="server"></asp:Label>
                                                
                                             </td>
                                             
                                         </tr>
                                         <tr>
                                             
                                             <td style="width:70%;text-align:left;font-size:12px;">
                                                 <asp:Label ID="lblAddr3"  runat="server"></asp:Label>
                                                 <asp:Label ID="lblAddr4"  runat="server"></asp:Label>
                                             </td>
                                             
                                         </tr>
                                     </table>
                                </td>
                                 <td style="width:33%;border-right:1px solid blue;">
                                      <table style="width:100%">
                                         <tr>
                                             <td style="width:70%;text-align:left;font-size:12px;font-weight:bold;">
                                                 Transporter
                                                
                                             </td>
                                            
                                             
                                         </tr>
                                          <tr>
                                             <td style="width:70%;text-align:left;font-size:12px;">
                                                  <span style="font-size:12px;font-weight:bold">Transporter Name:  </span>
                                                 <asp:Label ID="lblTransName" runat="server"></asp:Label>
                                                
                                             </td>
                                            
                                             
                                         </tr>
                                         <tr>
                                             
                                             <td style="width:100%;text-align:left;font-size:12px;">
                                                <span style="font-size:12px;font-weight:bold">Mode:  </span>
                                                 <asp:Label ID="lblTransMode" style="font-size:12px;" runat="server"></asp:Label>
                                             </td>
                                             
                                         </tr>
                                          <tr>
                                             
                                             <td style="width:70%;text-align:left;font-size:12px;">
                                                <span style="font-size:12px;font-weight:bold">Vehicle Reg.No.:  </span>
                                                 <asp:Label ID="lblRegno" style="font-size:12px;" runat="server"></asp:Label>
                                                
                                             </td>
                                             
                                         </tr>
                                          <tr>
                                             
                                             <td style="width:70%;text-align:left;font-size:12px;display:none;">
                                                <span style="font-size:12px;font-weight:bold">Delivery No:  </span>
                                                 <asp:Label ID="lblDelivery" style="font-size:12px;" runat="server"></asp:Label>
                                                
                                                
                                             </td>
                                             
                                         </tr>
                                     </table>
                                </td>
                                 <td style="width:33%;">
<table style="width:100%">
                                         <tr>
                                             <td style="width:70%;text-align:left;font-size:12px;font-weight:bold;">
                                                 Delivery Address
                                                
                                             </td>
                                            
                                             
                                         </tr>
                                         
                                          <tr>
                                             
                                             <td style="width:70%;text-align:left;font-size:12px;">
                                                 <asp:Label ID="lblDelAddr1"  runat="server"></asp:Label></td>
                                             
                                         </tr>
                                          <tr>
                                             
                                             <td style="width:70%;text-align:left;font-size:12px;">
                                                  <asp:Label ID="lbldelAddr2"  runat="server"></asp:Label>
                                                
                                             </td>
                                             
                                         </tr>
                                         <tr>
                                             
                                             <td style="width:70%;text-align:left;font-size:12px;">
                                                  <asp:Label ID="lbldelAddr3"  runat="server"></asp:Label>
                                                
                                             </td>
                                             
                                         </tr>
    <tr>
                                             
                                             <td style="width:70%;text-align:left;font-size:12px;">
                                                  <asp:Label ID="lbldelAddr4"  runat="server"></asp:Label>
                                                
                                             </td>
                                             
                                         </tr>
                                     </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                 <tr>
                    <td style="vertical-align:top;" colspan="2">
                        <table style="width:100%;height:50px;border-bottom:1px solid blue;">
                            <tr>
                                <td style="width:33%;border-right:1px solid blue;">
                                     <table style="width:100%">
                                         <tr>
                                             <td style="width:70%;text-align:left;font-size:12px;font-weight:bold;">
                                                Special Instructions
                                                
                                             </td>
                                            
                                             
                                         </tr>
                                         
                                          <tr>
                                             
                                             <td style="width:70%;text-align:left;font-size:12px;">
                                              <asp:Label ID="lblSplIns" runat="server"></asp:Label>   
                                                
                                             </td>
                                             
                                         </tr>
                                          <tr>
                                             
                                             <td style="width:70%;text-align:left;font-size:12px;">
                                                
                                                
                                             </td>
                                             
                                         </tr>
                                         <tr>
                                             
                                             <td style="width:70%;text-align:left;font-size:12px;">
                                                
                                                
                                             </td>
                                             
                                         </tr>
                                        
                                     </table>
                                </td>
                                
                                 <td style="width:33%;">
                                     <table style="width:100%">
                                         <tr>
                                             
                                             <td style="text-align:left;font-size:12px;font-weight:bold;" class="auto-style1">
                                                Total Picked Quantity
                                                
                                             </td>
                                              </tr>
                                          <tr>
                                              <td style="text-align:left;font-size:20px;" class="auto-style1">
                                               
                                                <asp:Label ID="lblTotalQty" runat="server"></asp:Label>
                                             </td>                                             
                                         </tr>
                                     </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
<td style="vertical-align:top;" colspan="2">
    <asp:GridView ID="grdOutBound" runat="server" HeaderStyle-CssClass="heading" ShowFooter="true" ShowHeaderWhenEmpty="true" OnRowDataBound="grdOutBound_RowDataBound" AutoGenerateColumns="False" Width="100%">
        <Columns>
             <asp:TemplateField HeaderText="SNo" HeaderStyle-Width="5%" >
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
            
             <asp:BoundField DataField="HU" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="HU"  HeaderStyle-Width="35%" />
            <asp:BoundField DataField="BinName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Delivery Location"  HeaderStyle-Width="35%" />
                   <asp:TemplateField HeaderText="Qty Picked" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Left">
            <ItemTemplate >
                <%# Eval("Qty") %>
            </ItemTemplate>
            <FooterTemplate>
                <asp:Label ID="lblTotalQty" runat="server" />
            </FooterTemplate>
        </asp:TemplateField>
        </Columns>
        <FooterStyle HorizontalAlign="Left" />
    </asp:GridView>
    </td>
                </tr>

                   <tr>
                    <td style="vertical-align:top;" colspan="2">
                        <table style="width:100%;height:50px;border-bottom:1px solid blue;border-top:1px solid blue;">
                            <tr>
                                <td style="width:33%;border-right:1px solid blue;">
                                     <table style="width:100%">
                                         <tr>
                                             <td style="width:70%;text-align:left;font-size:12px;font-weight:bold;">
                                                <table>
                                                    <tr>
                                                        <td style="width:250px;">
                                                            Issued By
                                                        </td>
                                                        <td style="width:200px;">
                                                            Date
                                                        </td>
                                                        <td>
                                                            Time
                                                        </td>
                                                       
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                          <asp:TextBox ID="txtissuesBy" runat="server" Text="                                 " style="border:0px;border-bottom: 2px solid black;" />
                                                        </td>
                                                        <td>
                                                          <asp:TextBox ID="txtIssueDate" runat="server" Text="                   " style="border:0px;border-bottom: 2px solid black;" />
                                                        </td>
                                                        <td>
                                                           <asp:TextBox ID="txtIssueTime" runat="server" Text="                " style="border:0px;border-bottom: 2px solid black;" />
                                                        </td>
                                                       
                                                    </tr>
                                                </table>
                                                
                                             </td>
                                            
                                             
                                         </tr>
                                         
                                          <tr>
                                             
                                             <td style="width:70%;text-align:left;font-size:12px;">
                                              Signature: 
                                                
                                             </td>
                                             
                                         </tr>
                                          <tr>
                                             
                                             <td style="width:70%;text-align:left;font-size:12px;">
                                                <asp:TextBox ID="Label1" runat="server" Text="                                 " style="border:0px;border-bottom: 2px solid black;" />
                                                
                                             </td>
                                             
                                         </tr>
                                        
                                        
                                     </table>
                                </td>
                                
                                 <td style="width:33%;">
                                     <table style="width:100%">
                                         <tr>
                                             <td style="width:70%;text-align:left;font-size:12px;font-weight:bold;">
                                                <table>
                                                    <tr>
                                                        <td style="width:250px;">
                                                            Security Check
                                                        </td>
                                                        <td style="width:200px;">
                                                            Date
                                                        </td>
                                                        <td>
                                                            Time
                                                        </td>
                                                       
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                          <asp:TextBox ID="TextBox1" runat="server" Text="                                 " style="border:0px;border-bottom: 2px solid black;" />
                                                        </td>
                                                        <td>
                                                          <asp:TextBox ID="TextBox2" runat="server" Text="                   " style="border:0px;border-bottom: 2px solid black;" />
                                                        </td>
                                                        <td>
                                                           <asp:TextBox ID="TextBox3" runat="server" Text="                " style="border:0px;border-bottom: 2px solid black;" />
                                                        </td>
                                                       
                                                    </tr>
                                                </table>
                                                
                                             </td>
                                            
                                             
                                         </tr>
                                         
                                          <tr>
                                             
                                             <td style="width:70%;text-align:left;font-size:12px;">
                                              Signature: 
                                                
                                             </td>
                                             
                                         </tr>
                                          <tr>
                                             
                                             <td style="width:70%;text-align:left;font-size:12px;">
                                                <asp:TextBox ID="TextBox4" runat="server" Text="                                 " style="border:0px;border-bottom: 2px solid black;" />
                                                
                                             </td>
                                             
                                         </tr>
                                        
                                     </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                 <tr>
                    <td style="vertical-align:top;" colspan="2">
                        <table style="width:100%;height:50px;border-bottom:1px solid blue;border-top:1px solid blue;">
                            <tr>
                                <td style="width:33%;border-right:1px solid blue;">
                                     <table style="width:100%">
                                         <tr>
                                             <td style="width:70%;text-align:left;font-size:12px;font-weight:bold;">
                                                <table>
                                                    <tr>
                                                        <td style="width:250px;">
                                                            RECEIVED IN GOOD CONDITION BY
                                                        </td>
                                                        <td style="width:200px;">
                                                            
                                                        </td>
                                                        <td>
                                                            (Company  Stamp)
                                                        </td>
                                                       
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                    </tr>
                                                </table>
                                                
                                             </td>
                                            
                                             
                                         </tr>
                                         
                                          
                                        
                                        
                                     </table>
                                </td>
                                
                                 <td style="width:33%;">
                                     <table style="width:100%">
                                         <tr>
                                             <td style="width:70%;text-align:left;font-size:12px;font-weight:bold;">
                                                <table>
                                                    <tr>
                                                        <td style="width:250px;">
                                                            Received By
                                                        </td>
                                                        <td style="width:200px;">
                                                           Received Date
                                                        </td>
                                                        <td>
                                                           Received Time
                                                        </td>
                                                       
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                          <asp:TextBox ID="TextBox9" runat="server" Text="                                 " style="border:0px;border-bottom: 2px solid black;" />
                                                        </td>
                                                        <td>
                                                          <asp:TextBox ID="TextBox10" runat="server" Text="                   " style="border:0px;border-bottom: 2px solid black;" />
                                                        </td>
                                                        <td>
                                                           <asp:TextBox ID="TextBox11" runat="server" Text="                " style="border:0px;border-bottom: 2px solid black;" />
                                                        </td>
                                                       
                                                    </tr>
                                                </table>
                                                
                                             </td>
                                            
                                             
                                         </tr>
                                         
                                          <tr>
                                             
                                             <td style="width:70%;text-align:left;font-size:12px;">
                                              Signature: 
                                                
                                             </td>
                                             
                                         </tr>
                                          <tr>
                                             
                                             <td style="width:70%;text-align:left;font-size:12px;">
                                                <asp:TextBox ID="TextBox12" runat="server" Text="                                 " style="border:0px;border-bottom: 2px solid black;" />
                                                
                                             </td>
                                             
                                         </tr>
                                        
                                     </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
       <div class="print-footer" style="text-align:center;">
        <p>Page <span class="page-number">1</span> of <span class="total-pages">1</span></p>
    </div>
               
    </form></div>
        <script>
            document.getElementById('download').addEventListener('click', () => {
                const { jsPDF } = window.jspdf;
                const doc = new jsPDF();

                doc.text(document.getElementById('divPrint').innerText, 10, 10);
                doc.save('sample.pdf');
            });
        </script>
</body>
</html>
