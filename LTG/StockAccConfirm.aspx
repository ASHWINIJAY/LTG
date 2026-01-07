<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="StockAccConfirm.aspx.cs" Inherits="LTG.StockAccConfirm" %>
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
      <div class="row">
            <div class="popup" id="popup" visible="false" runat="server" style="height:450px;overflow:scroll;" >
        <asp:Button ID="btnClose" runat="server" Text="X" class="close-popup-btn" OnClick="btnClose_Click" ValidationGroup="new"/>
        <h2>Stock Take Password</h2>
        <div class="row">

       

        <!-- Right side columns -->
        <div class="col">

          <!-- Recent Activity -->
          <div class="card">
           

           <section class="section error-404 d-flex flex-column align-items-center justify-content-center">
                <div class="row">
                     
                     <div class="col-9">
                        <label for="txtContainer" class="form-label">Password</label>
                    </div>
                   <div class="col-12" style="margin-top: -9px;">
                      
                     <asp:TextBox id="txtSupPassword" runat="server" ValidationGroup="TimeSlot12" autocomplete="new-password" TextMode="Password" ClientIDMode="Static" class="form-control"></asp:TextBox>
                       <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator6" controltovalidate="txtSupPassword" ErrorMessage="Please enter stock take password" ForeColor="OrangeRed" />
                     
                        </div>
                    <div class="col-6" style="margin-top: 3px;">
                         <asp:Button ID="btnSaveDate" class="btn btn-secondary" OnClick="btnSaveDate_Click" ValidationGroup="TimeSlot12" OnClientClick="return confirm('Are you sure you want to complete stock take process?');" Text="Initiate" runat="server" />
                       
                    </div>
                    <div class="col-6" style="margin-top: 3px;">
                        <asp:Button ID="btnCancelDate" class="btn btn-danger" ValidationGroup="no"  OnClientClick="return confirm('Are you sure to cancel?');" OnClick="btnCancelDate_Click" Text="Cancel" runat="server" />    
                
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
              <h5 class="card-title" style="text-align:center;background-color: #4090ce;color:white">Confirm Stock Take Accuracy</h5>
           <section class="section error-404 d-flex flex-column align-items-center justify-content-center">
                <div class="row g-3 needs-validation" style="width:100%">
                     <div style="text-align:right;"><asp:ImageButton ID="imgExcel" runat="server" ToolTip="Export To Excel" OnClick="imgExcel_Click" ImageUrl="~/assets/img/excel.png" Width="30" /></div>
                   <div class="col-6">
                       <asp:Button ID="btnReset" runat="server" CssClass="btn btn-custom" Text="Confirm Stock Take Accuracy" OnClientClick="return confirm('Are you sure you want to confirm stock take accuracy?');" OnClick="btnReset_Click" />
               <asp:Button ID="btnConfirm" runat="server" OnClick="btnConfirm_Click" Style="display:none;" />
<asp:HiddenField ID="hfUserConfirmed" runat="server" />
                   </div>
                    <div class="row">
                     <div class="col-12">
                   
                     <asp:TextBox id="txtStock" runat="server" placeholder="Enter HU here..." ValidationGroup="TimeSlot21" AutoPostBack="true" OnTextChanged="txtStock_TextChanged"  ClientIDMode="Static" style="padding-top: 14px;" Width="70%"></asp:TextBox>
                        <asp:ImageButton ID="imgStock" runat="server" OnClick="imgStock_Click" ValidationGroup="testabc" ImageUrl="~/assets/img/search.png" Width="35" />
                         </div>
                    <div class="col-12">
                        <asp:Gridview ID="grdBin" runat="server" HeaderStyle-BackColor="#80808069" Width="100%" GridLines="Both" class="table" AutoGenerateColumns="false">
                       
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
