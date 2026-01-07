<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BarCode.aspx.cs" Inherits="LTG.BarCode" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <title>Barcode</title>
    <style>
        table {
            border-collapse: collapse;
            width: 600px;
            text-align: center;
        }
        th, td {
            border: 1px solid black;
            padding: 8px;
        }
        .logo {
            width: 100px; /* Adjust size as needed */
            height: auto;
        }
    </style>
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
    <form id="form1" runat="server">
        <div style="text-align:center;">
            <table id="labelTable">
                <!-- Logo Row -->
                <tr>
                    <td colspan="3" style="height: 50px;">
                        <img src="assets/img/ltgpanel_new2.png" style="height:100px;" />
                    </td>
                </tr>
                <!-- Empty Row -->
                <tr>
                    <td colspan="2" style="height: 50px;">
                        <asp:Image ID="imgBarcode" runat="server" Width="300px" Height="80px" />
                    
                    </td>
                    <td style="text-align:right;"> <asp:Label id="lblCustomerName" Font-Bold="true" Font-Size="X-Small" runat="server"></asp:Label></td>
                </tr>
                
                <!-- Table Headers -->
                <tr>
                    <th>Date&Time</th>
                    <th>Client Code</th>
                    <th>WHCode</th>
                </tr>
                <tr>
                    <td><asp:Label id="lblDate" runat="server"></asp:Label> </td>
                    <td><asp:Label id="lblClient" runat="server"></asp:Label></td>
                    <td><asp:Label id="lblWH" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th>Part Number</th>
                    <th>Quantity</th>
                    <th>User</th>
                </tr>
                <tr>
                   <td><asp:Label id="lblPart" runat="server"></asp:Label> </td>
                    <td><asp:Label id="lblQty" runat="server"></asp:Label></td>
                    <td><asp:Label id="lbluser" runat="server"></asp:Label></td>
                </tr>
            </table>
            <br />
        </div>
    </form>
</body>
</html>

