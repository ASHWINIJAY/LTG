<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="StockTakeConfirm.aspx.cs" Inherits="LTG.StockTakeConfirm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <main id="main" class="main">
<script type="text/javascript">
    function confirmSave() {
        if (confirm('Stock Take already in progress. Do you want to start again?')) {
            document.getElementById('<%= hfUserConfirmed.ClientID %>').value = 'true';
        } else {
            document.getElementById('<%= hfUserConfirmed.ClientID %>').value = 'false';
        }
        __doPostBack('<%= btnConfirm.ClientID %>', '');
    }
    function openNewWindow(url) {
        window.open(url, '_blank');
    }
</script>
    

    <section class="section dashboard">
      <div class="row">

       

        <!-- Right side columns -->
        <div class="col">

          <!-- Recent Activity -->
          <div class="card">
              <h5 class="card-title" style="text-align:center;background-color: #4090ce;color:white">Confirm Stock Take</h5>
           <section class="section error-404 d-flex flex-column align-items-center justify-content-center">
                <div class="row g-3 needs-validation" style="width:100%">
                     <div style="text-align:right;"><asp:ImageButton ID="imgExcel" runat="server" ToolTip="Export To Excel" OnClick="imgExcel_Click" ImageUrl="~/assets/img/excel.png" Width="30" /></div>
                   <div class="col-6">
                       <asp:Button ID="btnReset" runat="server" CssClass="btn btn-custom" Text="Confirm Stock Take" OnClientClick="return confirm('Are you sure you want to confirm stock take?');" OnClick="btnReset_Click" />
               <asp:Button ID="btnConfirm" runat="server" OnClick="btnConfirm_Click" Style="display:none;" />
<asp:HiddenField ID="hfUserConfirmed" runat="server" />
                   </div>
                    <div class="row">
                       
                     <div class="col-12">
                   
                     <asp:TextBox id="txtStock" runat="server" placeholder="Enter HU here..." ValidationGroup="TimeSlot21" AutoPostBack="true" OnTextChanged="txtStock_TextChanged"  ClientIDMode="Static" style="padding-top: 14px;" Width="70%"></asp:TextBox>
                        <asp:ImageButton ID="imgStock" runat="server" OnClick="imgStock_Click" ValidationGroup="testabc" ImageUrl="~/assets/img/search.png" Width="35" />
                         </div>
                    <div class="col-12">
                        <asp:Gridview ID="grdBin" Font-Size="X-Small" runat="server" HeaderStyle-BackColor="#80808069" Width="100%" GridLines="Both" class="table" AutoGenerateColumns="false">
                       
                                  <Columns>
                        <asp:TemplateField HeaderText="SNo">
                            <ItemTemplate>
                                <asp:Label ID="lblRowNumber" runat="server" Text='<%# Container.DataItemIndex + 1 %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CustomerCode" HeaderStyle-Width="5%" ItemStyle-Width="5%" HeaderText="Customer" />
                        <asp:BoundField DataField="ContainerId" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderText="Container" />
                        <asp:BoundField DataField="Bin" HeaderText="Bin" HeaderStyle-Width="5%" ItemStyle-Width="5%" />
                        <asp:BoundField DataField="HU" HeaderText="HU" HeaderStyle-Width="15%" ItemStyle-Width="15%" />
                        <asp:BoundField DataField="STN" HeaderText="STN#" HeaderStyle-Width="15%" ItemStyle-Width="15%"/>
                         <asp:BoundField DataField="QtyOnHand" HeaderText="QtyOnHand" HeaderStyle-Width="7%" ItemStyle-Width="7%" />
                         <asp:BoundField DataField="Qty" HeaderText="Counter1 Qty" HeaderStyle-Width="7%" ItemStyle-Width="7%" />
                        <asp:BoundField DataField="ManagerQty" HeaderText="Manager Qty" HeaderStyle-Width="7%" ItemStyle-Width="7%"  />
                        <asp:BoundField DataField="Variance" HeaderText="Variance" HeaderStyle-Width="7%" ItemStyle-Width="7%"  />
                         <asp:BoundField DataField="FinalQty" HeaderText="Final Qty" HeaderStyle-Width="7%" ItemStyle-Width="7%"  />
                    </Columns>
                </asp:GridView>
                     
                    </div>
                   </div>
                   
                  </div>
       
      </section>

          </div><!-- End Recent Activity -->

         
      </div>
          </div>
    </section>

  </main><!-- End #main -->
   
</asp:Content>
