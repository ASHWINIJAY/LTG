<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="InitStockTake.aspx.cs" Inherits="LTG.InitStockTake" %>
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
    
            <style>
        /* Basic styling for the page */
       .headercenter{
           text-align:center;
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
         <script>
             function openPopup() {
                 document.getElementById("popup").style.display = "block";
             }

             function closePopup() {
                 document.getElementById("popup").style.display = "none";
             }
    </script>
    <section class="section dashboard">
          <div class="popup" id="binPopup" visible="false" style="height:500px;overflow:scroll;" runat="server" >
        <asp:Button ID="btncloseBin" runat="server" Text="X" class="close-popup-btn" OnClick="btncloseBin_Click" ValidationGroup="2"/>
        <h2>Select Bin's</h2>
        <div class="row">

        <div class="popup" id="popupcancel" visible="false" runat="server" style="height:450px;overflow:scroll;" >
        <asp:Button ID="btnCancelPopup" runat="server" Text="X" class="close-popup-btn" OnClick="btnCancelPopup_Click" ValidationGroup="new"/>
        <h2>Cancel Current Stock Take</h2>
        <div class="row">

       

        <!-- Right side columns -->
        <div class="col">

          <!-- Recent Activity -->
          <div class="card">
           

           <section class="section error-404 d-flex flex-column align-items-center justify-content-center">
                <div class="row">
                      <div class="col-9">
                        <label for="txtContainer" class="form-label">Cancellation Reason</label>
                    </div>
                   <div class="col-12" style="margin-top: -9px;">
                      
                     <asp:TextBox id="txtCancelReason" runat="server" ValidationGroup="TimeSlot12" autocomplete="new-password" TextMode="MultiLine" ClientIDMode="Static" class="form-control"></asp:TextBox>
                       <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator2" controltovalidate="txtCancelReason" ErrorMessage="Please enter stock take cancellation reason" ForeColor="OrangeRed" />
                     
                        </div>
                     <div class="col-9">
                        <label for="txtContainer" class="form-label">Stock Take Password</label>
                    </div>
                   <div class="col-12" style="margin-top: -9px;">
                      
                     <asp:TextBox id="txtstockcancelpwd" runat="server" ValidationGroup="TimeSlot12" autocomplete="new-password" TextMode="Password" ClientIDMode="Static" class="form-control"></asp:TextBox>
                       <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator1" controltovalidate="txtstockcancelpwd" ErrorMessage="Please enter stock take password" ForeColor="OrangeRed" />
                     
                        </div>
                    <div class="col-6" style="margin-top: 3px;">
                         <asp:Button ID="btnConfirmCancel" class="btn btn-secondary" OnClick="btnConfirmCancel_Click" ValidationGroup="TimeSlot12" OnClientClick="return confirm('Are you sure you want to cancel the ongoing stock take process?');" Text="Confirm and cancel the process" runat="server" />
                       
                    </div>
                    <div class="col-6" style="margin-top: 3px;">
                        <asp:Button ID="btnCloseCancel" class="btn btn-danger" ValidationGroup="no"  OnClientClick="return confirm('Are you sure to cancel?');" OnClick="btnCloseCancel_Click" Text="Close" runat="server" />    
                
                    </div>
                  </div>
       
      </section>

          </div><!-- End Recent Activity -->

         
      </div>
          </div>
    </div>

        <!-- Right side columns -->
        <div class="col">

          <!-- Recent Activity -->
          <div class="card">
           

           <section class="section error-404 d-flex flex-column align-items-center justify-content-center">

                <div class="row">
                       <div class="col-12">
                         <asp:Button ID="btnOk" runat="server" CssClass="btn btn-custom" style="background-color:#12b6fc;" Text="Allocate" OnClick="btnOk_Click" />
                        
                       <asp:Button ID="Button2" runat="server" CssClass="btn btn-custom" style="background-color:#518f56;" Text="Cancel" OnClick="Button2_Click" />
                         </div>
                    <div class="col-12">
                   
                     <asp:TextBox id="txtBins" runat="server" ValidationGroup="TimeSlot21" AutoPostBack="true" OnTextChanged="txtBins_TextChanged"  ClientIDMode="Static" style="padding-top: 14px;" Placeholder="Enter HU or Bin" Width="70%"></asp:TextBox>
                        <asp:ImageButton ID="imgsearchBin" runat="server" OnClick="imgsearchBin_Click" ValidationGroup="testabc" ImageUrl="~/assets/img/search.png" Width="35" />
                         </div>
                    <asp:GridView ID="grdBins" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover" 
                              EmptyDataText="No Data Found" OnRowDataBound="grdBins_RowDataBound" DataKeyNames="HU" HeaderStyle-CssClass="bg-dark text-light">
                    <Columns>
                     <asp:TemplateField HeaderText="<input type='checkbox' id='chkHeaderBin' onclick='toggleSelectAllBin(this)' />">
              
            <ItemTemplate>
                <asp:CheckBox ID="chkSelect" runat="server" />
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" CssClass="headercenter"/>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SNo">
                            <ItemTemplate>
                                <asp:Label ID="lblRowNumber" runat="server" Text='<%# Container.DataItemIndex + 1 %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Bin" HeaderText="Bin" />
                        <asp:BoundField DataField="HU" HeaderText="HU" />
                    </Columns>
                </asp:GridView>
                     
                    </div>
     
      </section>

          </div><!-- End Recent Activity -->

         
      </div>
          </div>
    </div>
      <div class="row">

       

        <!-- Right side columns -->
        <div class="col">

          <!-- Recent Activity -->
          <div class="card">
              <h5 class="card-title" style="text-align:center;background-color: #4090ce;color:white">Initiate Stock Take</h5>
           <section class="section error-404 d-flex flex-column align-items-center justify-content-center">
                <div class="row g-3 needs-validation">
                     <div style="text-align:right;"><asp:ImageButton ID="imgExcel" runat="server" ToolTip="Export To Excel" OnClick="imgExcel_Click" ImageUrl="~/assets/img/excel.png" Width="30" /></div>
                  <div class="col-12">
                      <asp:RadioButton ID="radCycle" OnCheckedChanged="radCycle_CheckedChanged" AutoPostBack="true" runat="server" Text="Cycle Count" Checked="false" GroupName="Test" />
                      <asp:RadioButton ID="radFullStockTake" runat="server" OnCheckedChanged="radFullStockTake_CheckedChanged" AutoPostBack="true" Text="Full Stock Take" Checked="true" GroupName="Test" />

                      </div>
                    <div class="col-12">

                       <asp:Button ID="btnBrowseBin" runat="server" CssClass="btn btn-custom" style="background-color:#12b6fc;" Text="Select HU for Cycle Count" Visible="false" OnClick="btnBrowseBin_Click" />
                        
                       <asp:Button ID="btnReset" runat="server" CssClass="btn btn-custom" Text="Initiate Full Stock Take" OnClick="btnReset_Click" />
               <asp:Button ID="btnConfirm" runat="server" OnClick="btnConfirm_Click" Style="display:none;" />
<asp:HiddenField ID="hfUserConfirmed" runat="server" />
                   </div>
                     
                    <div class="col-12">
                         <asp:GridView ID="grdBin" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover" 
                              EmptyDataText="No Data Found" OnRowDataBound="grdBin_RowDataBound" HeaderStyle-CssClass="bg-dark text-light">
                    <Columns>
                        <asp:TemplateField HeaderText="SNo">
                            <ItemTemplate>
                                <asp:Label ID="lblRowNumber" runat="server" Text='<%# Container.DataItemIndex + 1 %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CustomerCode" HeaderText="Customer" />
                        <asp:BoundField DataField="ContainerId" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderText="Container" />
                        <asp:BoundField DataField="Bin" HeaderText="Bin" />
                        <asp:BoundField DataField="HU" HeaderText="HU" />
                        
                        <asp:BoundField DataField="Qty" HeaderText="Qty" />
                    </Columns>
                </asp:GridView>
                     
                    </div>
                   
                   
                  </div>
       
      </section>

          </div><!-- End Recent Activity -->

         
      </div>
          </div>
    </section>

  </main><!-- End #main -->
   
</asp:Content>
