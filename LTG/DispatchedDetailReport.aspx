<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DispatchedDetailReport.aspx.cs" Inherits="LTG.DispatchedDetailReport" %>

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
                         <asp:ImageButton ID="btnCancel" Width="30" Visible="false" runat="server" ToolTip="Click here to see report logic" OnClick="btnCancel_Click" ImageUrl="~/assets/img/info.png" />
                    </td>
                    <td style="height:30px;text-align:center;">
                        <img src="assets/img/ltgpanel_new2.png" style="height:100px;" />
                    </td>
                </tr>
               
            </table>
        </div>
      
        <div>
            <hr />
            <asp:Label ID="lblHeader" Font-Size="Larger" Font-Bold="true"  ForeColor="#4191d1" runat="server"></asp:Label>
            <asp:HiddenField ID="hdnQty" runat="server" />
            <br />
          <asp:GridView ID="grdDetails" Width="100%" runat="server" HeaderStyle-BackColor="LightGray" OnRowCreated="grdDetails_RowCreated" OnDataBound="grdDetails_DataBound" AutoGenerateColumns="False">
                 <Columns>
                  <asp:TemplateField HeaderText="SNo" HeaderStyle-Width="5%" >
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:BoundField DataField="CustomerCode" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-BackColor="#FFFF00" HeaderText="Customer"/>
             
            <asp:BoundField DataField="GDN" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-BackColor="#FFFF00" HeaderText="GDN"/>
            
         
                     <asp:BoundField DataField="Bin" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-BackColor="#FFFF00" HeaderText="Bin"/>
             
         
                   <asp:BoundField DataField="HU" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="HU"/>
          
              <asp:BoundField DataField="Qty" HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="#00B050" ItemStyle-HorizontalAlign="Left" HeaderText="Despatched Qty"/>
          
                     
                 
              </Columns>
          </asp:GridView> </div>
    </form>
</body>
</html>

