<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RWarehouseDetailReport.aspx.cs" Inherits="LTG.RWarehouseDetailReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
       <div id="divHeader" runat="server">
            <table style="width:100%;">
                <tr>
                    <td style="width:30px;padding-left:20px;">
                       <asp:ImageButton ID="btnSave" Width="30" ToolTip="Save" runat="server" ImageUrl="~/assets/img/save.png" />
                        </td>
                    <td style="width:30px;padding-left:20px;">
                         <asp:ImageButton ID="btnPrint"  Width="30" ToolTip="Print" runat="server" ImageUrl="~/assets/img/printer.png" />
                        </td>
                    <td style="width:30px;padding-left:20px;">
                         <asp:ImageButton ID="btnExcel"  Width="30" ToolTip="Export To Excel" runat="server" ImageUrl="~/assets/img/excel.png" OnClick="btnExcel_Click" />
                        </td>
                    <td style="width:30px;padding-left:20px;display:none;">
                         <asp:ImageButton ID="btnCancel" Width="30" runat="server" ImageUrl="~/assets/img/cancel.png" />
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
            <br />
          <asp:GridView ID="grdDetails" Width="100%" runat="server" HeaderStyle-BackColor="LightGray" OnRowCreated="grdDetails_RowCreated" OnDataBound="grdDetails_DataBound" AutoGenerateColumns="False">
                 <Columns>
                  <asp:TemplateField HeaderText="SNo" HeaderStyle-Width="5%" >
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                   <asp:BoundField DataField="HU" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="HU"/>
           <asp:BoundField DataField="ReturnDateTimeofScan" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Returned Date"/>
             
              <asp:BoundField DataField="DateTimeofScan" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Binned Date"/>
            <asp:BoundField DataField="Duration" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-BackColor="#00B050" HeaderText="No of days"/>
                     
                     <asp:BoundField DataField="RBIN" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-BackColor="#00B050" HeaderText="Return Number"/>
            <asp:BoundField DataField="BinningNumber" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-BackColor="#00B050" HeaderText="Binning Number"/>
                  <asp:BoundField DataField="Qty" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-BackColor="#00B050" HeaderText="Returned Qty"/>
                     
                      <asp:BoundField DataField="Reason" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-BackColor="#00B050" HeaderText="Reason"/>
            
                     
              </Columns>
          </asp:GridView> </div>
    </form>
</body>
</html>