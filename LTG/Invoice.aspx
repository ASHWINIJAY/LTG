<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Invoice.aspx.cs" Inherits="LTG.Invoice" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <title>Periodic Invoice</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f8f8f8;
            margin: 0;
            padding: 20px;
        }
        .invoice-box {
            max-width: 800px;
            margin: auto;
            padding: 30px;
            border: 1px solid #eee;
            background-color: #fff;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.15);
        }
        .invoice-box table {
            width: 100%;
            line-height: inherit;
            text-align: left;
            border-collapse: collapse;
            font-size:12px;
        }
        .invoice-box table td {
            padding: 3px;
            vertical-align: top;
        }
        .invoice-box table tr td:nth-child(2),
        .invoice-box table tr td:nth-child(3) {
            text-align: right;
        }
        .invoice-box table tr.top table td {
        }
        .invoice-box table tr.top table td.title {
            font-weight:bold;
        }
        .invoice-box table tr.information table td {
        }
        .invoice-box table tr.heading td {
            background: #eee;
            border-bottom: 1px solid #ddd;
            font-weight: bold;
            font-size:12px;
        }
        .invoice-box table tr.details td {
        }
        .invoice-box table tr.item td {
            border-bottom: 1px solid #eee;
        }
        .invoice-box table tr.item.last td {
            border-bottom: none;
        }
        .invoice-box table tr.total td:nth-child(3),
        .invoice-box table tr.total td:nth-child(4) {
            border-top: 2px solid #eee;
            font-weight: bold;
        }
        .invoice-box p, .invoice-box h3 {
            margin: 20px 0 0 0;
            line-height: 1.5;
        }
        .header img {
            max-width: 100%;
            height: auto;
        }
    </style>
    
</head>
<body>
    <script>
        function printPage() {
            document.getElementById("divHeader").style.display = "none";
            window.print();
            sleep(5000, myFunction);
           
        }
        function myFunction() {
            document.getElementById("divHeader").style.display = "block";
        }
    </script>
    <Form id="frm" runat="server">
        <div id="divHeader" runat="server">
            <table>
                <tr>
                    <td style="width:30px;padding-left:20px;">
                       <asp:ImageButton ID="btnSave" Width="30" ToolTip="Save" runat="server" ImageUrl="~/assets/img/save.png" OnClick="btnSave_Click" />
                        </td>
                    <td style="width:30px;padding-left:20px;">
                         <asp:ImageButton ID="btnPrint"  Width="30" ToolTip="Print" runat="server" ImageUrl="~/assets/img/printer.png" OnClick="btnPrint_Click" />
                        </td>
                    <td style="width:30px;padding-left:20px;">
                         <asp:ImageButton ID="btnExcel"  Width="30" ToolTip="Export To Excel" runat="server" ImageUrl="~/assets/img/excel.png" OnClick="btnExcel_Click" />
                        </td>
                    <td style="width:30px;padding-left:20px;display:none;">
                         <asp:ImageButton ID="btnCancel" Width="30" runat="server" ImageUrl="~/assets/img/cancel.png" OnClick="btnCancel_Click" />
                    </td>
                </tr>
               
            </table>
        </div>
    <div class="invoice-box" id="divPrint" runat="server">
        <div class="header">
            <img src="logo1.png" alt="Logo">
        </div>
        <table style="border:1px solid;">
            <tr class="top">
                <td colspan="4">
                    <table style="border:1px solid;">
                        <tr>
                            <td class="title">
                                TAX INVOICE
                            </td>
                            <td>
                                
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="information">
                <td colspan="4">
                    <table style="border:1px solid;">
                        <tr>
                            <td>
                                BMW (SOUTH AFRICA) PTY LTD<br>
                                P.O. BOX 2955<br>
                                6 FRANS DU TOIT STR, ROSSLYN<br>
                                PRETORIA, 0001<br>
                                VAT #: 4680227651
                            </td>
                            <td>
                                <table style="border:1px solid;">
                                    <tr style="border:1px solid;">
                                        <td style="font-weight:bold;">
                                            Invoice #:
                                        </td>
                                        <td>
                                            I00001380
                                        </td>
                                    </tr>
                                   <tr style="border:1px solid;">
                                        <td style="font-weight:bold;">
                                            Invoice Date:
                                        </td>
                                        <td>
                                           <asp:Label ID="lblInvDate" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="border:1px solid;">
                                       <td style="font-weight:bold;">
                                            Customer Id:
                                        </td>
                                        <td>
                                            BMWSOU_ZA
                                        </td>
                                    </tr>
                                    <tr style="border:1px solid;">
                                        <td style="font-weight:bold;">
                                            Client VAT #:
                                        </td>
                                        <td>
                                           4300107432
                                        </td>
                                    </tr>
                                    <tr style="border:1px solid;">
                                       <td style="font-weight:bold;">
                                            Due Date:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDueDatee" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="border:1px solid;">
                                        <td style="font-weight:bold;">
                                            Terms:
                                        </td>
                                        <td>
                                            7 days from Inv. Date
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="information">
                <td colspan="4">
                    <table style="border:1px solid;">
                        <tr>
                            <td>
                                WAREHOUSE<br>
                                LTG ROSSLYN WAREHOUSE (R01)<br>
                                LTG LOGISTICS TRANSPORT GLOBALLY (PTY) LTD<br>
                                28 VAN EDEN CRESCENT, ROSSLYN<br>
                                PRETORIA, 0200<br>
                                SOUTH AFRICA
                            </td>
                            <td style="text-align:right;font-weight:bold">
                                Billing Period:
                            </td>
                            <td><asp:Label ID="lblDate" runat="server"></asp:Label></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="heading">
                <td>Description</td>
                <td>VAT(15%)</td>
                 <td>Charges in ZAR</td>
                <td style="width:20%;text-align:right">Charges in ZAR(Inc.VAT)</td>
            </tr>
            <tr class="item">
                <td>Storage</td>
                <td><asp:Label ID="lblStorageVAT" runat="server"></asp:Label></td>
                <td><asp:Label ID="lblStorageSub" runat="server"></asp:Label></td>
                <td style="width:20%;text-align:right"><asp:Label ID="lblStorageTot" runat="server"></asp:Label></td>
            </tr>
            <tr class="item">
                <td>Warehouse Handling Out</td>
                <td><asp:Label ID="lblOutVat" runat="server"></asp:Label></td>
                <td><asp:Label ID="lblOutSub" runat="server"></asp:Label></td>
                <td style="width:20%;text-align:right"><asp:Label ID="lblOutTot" runat="server"></asp:Label></td>
            </tr>
            <tr class="item last">
                <td>Warehouse Handling In</td>
                <td><asp:Label ID="lblInVat" runat="server"></asp:Label></td>
                <td><asp:Label ID="lblinSub" runat="server"></asp:Label></td>
                <td style="width:20%;text-align:right"><asp:Label ID="lblInTot" runat="server"></asp:Label></td>
            </tr>
             <tr class="item last">
                 <td colspan="4"></td>
                 </tr>
            <tr class="total" style="margin-top:50px;">
                <td></td>
                 <td></td>
                <td>Subtotal:</td>
                <td style="width:20%;text-align:right"><asp:Label ID="lblSub" runat="server"></asp:Label></td>
            </tr>
            <tr class="total">
                <td></td>
                <td></td>
                <td>VAT:</td>
                <td style="width:20%;text-align:right"><asp:Label ID="lblvat" runat="server"></asp:Label></td>
            </tr>
            <tr class="total">
                <td></td>
                <td></td>
                <td>Total:</td>
                <td style="width:20%;text-align:right"><asp:Label ID="lblTot" runat="server"></asp:Label></td>
            </tr>
            <tr class="information">
                <td colspan="4" style="border: 1px solid;text-align:left;font-size:8px;">
                    Please contact us within 7 days should there be any discrepancies.<br>
            Additional charges to follow if applicable.<br>
            All business is transacted in accordance with our standard trading terms and conditions.
                    </td>

            </tr>
            <tr>
                <td colspan="2" style="border: 1px solid;text-align:left;">
                     <h3 style="background-color:gray;color:white;">Transfer Funds To:</h3>
       
       
            Bank SWIFT: FIRNZAJJXXX<br>
            Account: 62402187315<br>
            FIRST NATIONAL BANK, ROSSLYN
        <br />
            Pay Ref: BMWSOU_ZA I00001380 00030460
        
                </td>
                 <td colspan="2" style="border: 1px solid;text-align:left;">
                     <h3 style="background-color:gray;color:white;">Address:</h3>
                     LTG LOGISTICS TRANSPORT GLOBALLY (PTY) LTD<br>
PO BOX 911-1882<br>
ROSSLYN<br>
0200<br>
SOUTH AFRICA<br />
                </td>

            </tr>
            <tr class="heading">
                <td colspan="2">
                   
                    Invoiced ZAR: <asp:Label ID="lblInvoice" runat="server"></asp:Label>
                </td>
                <td colspan="2" style="text-align:left;">
                    Balance Due: <asp:Label ID="lblDueAmount" runat="server"></asp:Label>
                
                    Due Date: <asp:Label ID="lblDueDate" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
       
      <br />
        <br />
        <br />
         <br />
        <br />
        <br />
         <br />
         <br />
        <br />
        <br />
        <br />

        
    <div>
        <table>
            <tr>
                <td>
                    <h3>InBounds:</h3>
                </td>
            </tr>
        </table>
        <br />
    <asp:GridView ID="grdInbound" runat="server" HeaderStyle-CssClass="heading" ShowFooter="true" ShowHeaderWhenEmpty="true" OnRowDataBound="grdInbound_RowDataBound" AutoGenerateColumns="False" Width="100%">
        <Columns>
             <asp:TemplateField HeaderText="SNo" HeaderStyle-Width="5%" >
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
            
            <asp:BoundField DataField="DateTimeofScan" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Date"  HeaderStyle-Width="10%" />
             <asp:BoundField DataField="CustomerCode" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="CustomerCode"  HeaderStyle-Width="15%" />
            <asp:BoundField DataField="ContainerId" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Container#"  HeaderStyle-Width="15%" />
                    <asp:BoundField DataField="HU" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="HU"  HeaderStyle-Width="35%" />
                    <asp:BoundField DataField="TotalInBoundCost" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Charges Exc.Vat"  HeaderStyle-Width="15%" />
        </Columns>
        <FooterStyle HorizontalAlign="Left" />
    </asp:GridView>
        <table>
            <tr>
                <td>
                    <h3>OutBounds:</h3>
                </td>
            </tr>
        </table>
        <br />
    <asp:GridView ID="grdOutBound" runat="server" HeaderStyle-CssClass="heading" ShowFooter="true" ShowHeaderWhenEmpty="true" OnRowDataBound="grdOutBound_RowDataBound" AutoGenerateColumns="False" Width="100%">
        <Columns>
             <asp:TemplateField HeaderText="SNo" HeaderStyle-Width="5%" >
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
            
            <asp:BoundField DataField="DateTimeofScan" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Date"  HeaderStyle-Width="10%" />
             <asp:BoundField DataField="CustomerCode" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="CustomerCode"  HeaderStyle-Width="15%" />
            <asp:BoundField DataField="ContainerId" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Container#"  HeaderStyle-Width="15%" />
                    <asp:BoundField DataField="HU" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="HU"  HeaderStyle-Width="35%" />
                    <asp:BoundField DataField="TotalOutBoundCost" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Charges Exc.Vat"  HeaderStyle-Width="15%" />
        </Columns>
        <FooterStyle HorizontalAlign="Left" />
    </asp:GridView>
       <table>
            <tr>
                <td>
                    <h3>Storage:</h3>
                </td>
            </tr>
        </table>
        <br />
    <asp:GridView ID="grdStorage" runat="server" HeaderStyle-CssClass="heading" ShowFooter="true" ShowHeaderWhenEmpty="true" OnRowDataBound="grdStorage_RowDataBound" AutoGenerateColumns="False" Width="100%">
        <Columns>
             <asp:TemplateField HeaderText="SNo" HeaderStyle-Width="5%" >
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
            
            <asp:BoundField DataField="DateTimeofScan" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Date"  HeaderStyle-Width="10%" />
             <asp:BoundField DataField="CustomerCode" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="CustomerCode"  HeaderStyle-Width="15%" />
              <asp:BoundField DataField="HU" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="HU"  HeaderStyle-Width="35%" />
                    <asp:BoundField DataField="TotalStorageCost" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderText="Charges Exc.Vat"  HeaderStyle-Width="15%" />
        </Columns>
        <FooterStyle HorizontalAlign="Right" />
    </asp:GridView>
        </div>
         </div>
        </Form>
   
</body>
</html>

