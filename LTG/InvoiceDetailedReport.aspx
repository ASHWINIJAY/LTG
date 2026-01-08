<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InvoiceDetailedReport.aspx.cs" Inherits="LTG.InvoiceDetailedReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>

<body>
     <style>
        /* Basic styling for the page */
       
        .storage-details-table {
            width: 100%;
            border-collapse: collapse;
            margin: 20px 0;
        }
        .storage-details-table th, .storage-details-table td {
            border: 1px solid #ddd;
            padding: 10px;
            text-align: left;
        }
        .storage-details-table th {
            background-color: #f4f4f4;
        }
        .storage-details-table tbody tr:nth-child(odd) {
            background-color: #f9f9f9; /* Light gray */
        }
        .storage-details-table tbody tr:nth-child(even) {
            background-color: #ffffff; /* White */
        }
        .storage-details-table tbody td:nth-child(2) {
            font-size: 0.9em; /* Smaller font size for the description column */
            color: #555555;  /* Custom color for the description column */

        }
        .storage-details-container {
            margin-top: 20px;
        }
        .storage-details-container caption {
            font-size: 1.2em;
            margin-bottom: 10px;
        }
        /* Popup styling */
        .popup {
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            width: 700px;
            padding: 20px;
            background-color: white;
            box-shadow: 0 5px 15px rgba(0,0,0,0.3);
            border-radius: 10px;
            z-index: 1000;
        }

        /* Close button styling */
        .close-popup-btn {
            background-color: red;
            color: white;
            border: none;
            cursor: pointer;
            border-radius: 5px;
            float: right;
        }

        /* Background overlay */
        .popup-overlay {
            display: none; /* Hidden by default */
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0,0,0,0.5);
            z-index: 999;
        }
    </style>
    <form id="form1" runat="server">
       <div id="divHeader" runat="server">
            <table style="width:100%;">
                <tr>
                    
                    <td style="width:30px;padding-left:20px;">
                         <asp:ImageButton ID="btnExcel"  Width="30" ToolTip="Export To Excel" runat="server" ImageUrl="~/assets/img/excel.png" OnClick="btnExcel_Click" />
                        </td>
                    <td style="width:30px;padding-left:20px;">
                         <asp:ImageButton ID="btnCancel" Width="30" runat="server" ToolTip="Click here to see report logic" OnClick="btnCancel_Click" ImageUrl="~/assets/img/info.png" />
                    </td>
                    <td style="height:30px;text-align:center;">
                        <img src="assets/img/ltgpanel_new2.png" style="height:100px;" />
                    </td>
                </tr>
               
            </table>
        </div>
        <div>
             <div class="popup" id="popupAdd" visible="false" style="height:500px;overflow:scroll;" runat="server" >
        <asp:Button ID="btnAddclose" runat="server" Text="X" class="close-popup-btn" OnClick="btnAddclose_Click" ValidationGroup="2"/>
                 <div class="row">

       

        <!-- Right side columns -->
        <div class="col">

          <!-- Recent Activity -->
          <div class="card">
               <table class="storage-details-table">
                   <caption>This report displays the HUs that were inbounded within the selected period, as well as those inbounded before the 'FromDate' and outbounded/not outbounded between the 'FromDate' and 'ToDate'. </caption>
            <thead>
                <tr>
                    <th>Field</th>
                    <th>Description</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>DocDate (GRN)</td>
                    <td>HU’s Inbounded Date</td>
                </tr>
                <tr>
                    <td>Binned Date</td>
                    <td>HU’s Warehoused Date</td>
                </tr>
                <tr>
                    <td>Outbound Date</td>
                    <td>HU’s Outbounded Date</td>
                </tr>
                <tr>
                    <td>StoreQty</td>
                    <td>No of HU’s are warehoused</td>
                </tr>
                <tr>
                    <td>NoOfDays</td>
                    <td>Total Warehoused days (OutboundDate - InBoundDate)</td>
                </tr>
                <tr>
                    <td>UnitStorageCost</td>
                    <td>Per Day Storage Cost</td>
                </tr>
                <tr>
                    <td>TotalStorageCost</td>
                    <td>Total Warehoused days × Per Day Storage Cost</td>
                </tr>
                <tr>
                    <td>QtyInRate</td>
                    <td>Inbound Cost</td>
                </tr>
                <tr>
                    <td>TotQtyInRate</td>
                    <td>Number Qty × Inbound Cost</td>
                </tr>
                <tr>
                    <td>QtyOutRate</td>
                    <td>Outbound Cost</td>
                </tr>
                <tr>
                    <td>TotQtyOutRate</td>
                    <td>Number Qty × Outbound Cost</td>
                </tr>
            </tbody>
        </table>
              </div>
            </div>
                     </div>
                 </div>
        </div>
        <div>
            <hr />
            <asp:Label ID="lblHeader" Font-Size="Larger" Font-Bold="true"  ForeColor="#4191d1" runat="server"></asp:Label>
            <br />
          <asp:GridView ID="grdDetails" Width="100%" runat="server" HeaderStyle-BackColor="LightGray" OnRowCreated="grdDetails_RowCreated" OnDataBound="grdDetails_DataBound" AutoGenerateColumns="False">
                 <Columns>
                  <asp:TemplateField HeaderText="SNo" HeaderStyle-Width="5%" >
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
            
            <asp:BoundField DataField="ContainerId" HeaderStyle-HorizontalAlign="Left"  HeaderStyle-Width="100" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Left" HeaderText="ContainerId"/>
              <asp:BoundField DataField="Kolli" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="UOP"/>
                   <asp:BoundField DataField="HU" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="HU"/>
           <asp:BoundField DataField="DocDate" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="DocDate(GRN)"/>
                      <asp:BoundField DataField="FromDate" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Binned Date"/>
                      <asp:BoundField DataField="ToDate" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Outbound Date"/>
              <asp:BoundField DataField="FromDate1" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="FromDate"/>
           <asp:BoundField DataField="ToDate1" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="ToDate"/>
              <asp:BoundField DataField="StoreQty" HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="#FFFF00" ItemStyle-HorizontalAlign="Left" HeaderText="StoreQty"/>
           <asp:BoundField DataField="NoodfDays" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-BackColor="#FFFF00" HeaderText="NoOfDays"/>
              <asp:BoundField DataField="UnitStorageCost" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="#FFFF00" ItemStyle-HorizontalAlign="Left" HeaderText="UnitStorageCost"/>
              
                  <asp:BoundField DataField="TotalStorageCost" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="#FFFF00" ItemStyle-HorizontalAlign="Left" HeaderText="TotalStorageCost"/>
       
                     <asp:BoundField DataField="GRN" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-BackColor="#00B050" HeaderText="GRN"/>
            
                  <asp:BoundField DataField="QtyIn" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-BackColor="#00B050" HeaderText="QtyIn"/>
              <asp:BoundField DataField="QtyInRate" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="#00B050" ItemStyle-HorizontalAlign="Left" HeaderText="QtyInRate"/>
           <asp:BoundField DataField="TotQtyInRate" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="#00B050" ItemStyle-HorizontalAlign="Left" HeaderText="TotQtyInRate"/>
          
                      <asp:BoundField DataField="GDN" HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="#92D050" ItemStyle-HorizontalAlign="Left" HeaderText="GDN"/>
            
                         <asp:BoundField DataField="QtyOut" HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="#92D050" ItemStyle-HorizontalAlign="Left" HeaderText="QtyOut"/>
              <asp:BoundField DataField="QtyOutRate" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="#92D050" ItemStyle-HorizontalAlign="Left" HeaderText="QtyOutRate"/>
           <asp:BoundField DataField="TotOutRate" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="#92D050" ItemStyle-HorizontalAlign="Left" HeaderText="TotOutRate"/>
              
              
              </Columns>
          </asp:GridView> </div>
    </form>
</body>
</html>
