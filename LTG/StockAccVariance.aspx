<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="StockAccVariance.aspx.cs" Inherits="LTG.StockAccVariance" %>
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
</script>
    

    <section class="section dashboard">
      <div class="row">

       

        <!-- Right side columns -->
        <div class="col">

          <!-- Recent Activity -->
          <div class="card">
              <h5 class="card-title" style="text-align:center;background-color: #4090ce;color:white">Stock Accuracy Variance</h5>
           <section class="section error-404 d-flex flex-column align-items-center justify-content-center">
                <div class="row g-3 needs-validation" style="width:100%">
                     <div style="text-align:right;"><asp:ImageButton ID="imgExcel" runat="server" ToolTip="Export To Excel" OnClick="imgExcel_Click" ImageUrl="~/assets/img/excel.png" Width="30" /></div>
                   <div class="col-6" style="display:none;">
                       <asp:Button ID="btnReset" runat="server" CssClass="btn btn-custom" Text="Create Stock Take" OnClick="btnReset_Click" />
               <asp:Button ID="btnConfirm" runat="server" OnClick="btnConfirm_Click" Style="display:none;" />
<asp:HiddenField ID="hfUserConfirmed" runat="server" />
                       <asp:HiddenField ID="hdnExportExcel" Value="0" runat="server" />
                   </div>
                    <div class="row">
                     <div class="col-12">
                   
                     <asp:TextBox id="txtStock" runat="server" placeholder="Enter HU here..." ValidationGroup="TimeSlot21" AutoPostBack="true" OnTextChanged="txtStock_TextChanged"  ClientIDMode="Static" style="padding-top: 14px;" Width="70%"></asp:TextBox>
                        <asp:ImageButton ID="imgStock" runat="server" OnClick="imgStock_Click" ValidationGroup="testabc" ImageUrl="~/assets/img/search.png" Width="35" />
                         </div>
                    <div class="col-12">
                         <asp:GridView ID="grdBin" runat="server" AutoGenerateColumns="false" CssClass="table-bordered table-hover" 
                              EmptyDataText="No Data Found" OnRowDataBound="grdBin_RowDataBound" HeaderStyle-CssClass="bg-dark text-light">
                    <Columns>
                        <asp:TemplateField HeaderText="SNo">
                            <ItemTemplate>
                                <asp:Label ID="lblRowNumber" runat="server" Text='<%# Container.DataItemIndex + 1 %>' />
                            </ItemTemplate>
                        </asp:TemplateField> <asp:BoundField DataField="ActualBin" HeaderText="ActualBin" />
                        <asp:BoundField DataField="CountedBin" HeaderText="CountedBin by Counter" />
                        <asp:BoundField DataField="ManagerCountedBin" HeaderText="CountedBin by Manager" />
                        <asp:BoundField DataField="HU" HeaderText="HU" />
                        <asp:BoundField DataField="CountedBy" HeaderText="Counted By" />
                        <asp:BoundField DataField="MCountedBy" HeaderText="Manager" />                        
                        <asp:BoundField DataField="Variance" HeaderText="Variance" />
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
