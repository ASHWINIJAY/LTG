<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="InBoundProcess.aspx.cs" Inherits="LTG.InBoundProcess" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <main id="main" class="main">
          <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
         <script src="https://cdn.jsdelivr.net/jsbarcode/3.11.0/JsBarcode.all.min.js"></script>
         <!-- Bootstrap CSS -->
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
<!-- Bootstrap JS Bundle -->
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>

    <script>
function myFunction() {
  confirm("Are you sure complete the InBound Scan !");
        }
        
            function openNewWindow(url) {
                window.open(url, '_blank');
             }
    </script>
       
          <style>
        /* Basic styling for the page */
       

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
 <script type="text/javascript">
     <%--   $(document).ready(function () {
     $('#<%= txtHU.ClientID %>').on({
         keypress: function () { typed_into = true; },
         input: function () {
             if (typed_into) {
                 alert('type');
                 typed_into = false; //reset type listener
             } else {
                 alert('not type');
             }
         }
     });
     });
     $(document).ready(function () {
        
         var type = false;
           $('#<%= txtHU.ClientID %>').on('input', function () {
               var inputLength = $('#<%= txtHU.ClientID %>').val().length;
               if (inputLength == 1 || inputLength == 2 ) {
                   type=true;
               }
               if(type==false)
                autoSave();
           });
         $('#<%= txtHU.ClientID %>').on('blur', function () {
             
              if (type == true)
                  autoSave();
          });
         $('#<%= txtHU.ClientID %>').on('keydown', function (event) {
             
             if (event.which == 13) { // 13 is the Enter key
                 if (type == true)
                     autoSave();
             }
         });

            function autoSave() {
                $('#<%= btnSave.ClientID %>').click();
            }
     });
     --%>
 </script>
         <style type="text/css">
             tbody, td, tfoot, th, thead, tr {
                 border:1px solid black;
             }
         </style>
    <section class="section dashboard">
      <div class="row">

              <div class="popup" id="popup" visible="false" runat="server" style="height:450px;overflow:scroll;" >
        <asp:Button ID="btnClose" runat="server" Text="X" class="close-popup-btn" OnClick="btnClose_Click" ValidationGroup="new"/>
        <h2>Change Entry Date</h2>
        <div class="row">

       

        <!-- Right side columns -->
        <div class="col">

          <!-- Recent Activity -->
          <div class="card">
           

           <section class="section error-404 d-flex flex-column align-items-center justify-content-center">
                <div class="row">
                      <div class="col-9">
                        <label for="txtContainer" class="form-label">New Date</label>
                    </div>
                    <div class="col-12" style="margin-top: -9px;">
                      
                     <asp:TextBox id="txtNewdate" runat="server" ValidationGroup="TimeSlot12" autocomplete="off" TextMode="Date" ClientIDMode="Static" class="form-control"></asp:TextBox>
                         <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator5" controltovalidate="txtNewdate" ErrorMessage="Please enter new entry date" ForeColor="OrangeRed" />
                    
                        </div>
                     <div class="col-9">
                        <label for="txtContainer" class="form-label">Supervisor Password</label>
                    </div>
                   <div class="col-12" style="margin-top: -9px;">
                      
                     <asp:TextBox id="txtSupPassword" runat="server" ValidationGroup="TimeSlot12" autocomplete="new-password" TextMode="Password" ClientIDMode="Static" class="form-control"></asp:TextBox>
                       <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator6" controltovalidate="txtSupPassword" ErrorMessage="Please enter supervisor password" ForeColor="OrangeRed" />
                     
                        </div>
                    <div class="col-6" style="margin-top: 3px;">
                         <asp:Button ID="btnSaveDate" class="btn btn-secondary" OnClick="btnSaveDate_Click" ValidationGroup="TimeSlot12" OnClientClick="return confirm('Are you sure you want to change the date?');" Text="Change" runat="server" />
                       
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
          <div class="card" id="divCustomer" runat="server">
           
               <h5 class="card-title" style="text-align:center;background-color: #4090ce;color:white">InBound Process</h5>
           <div class="section error-404 d-flex flex-column align-items-center justify-content-center">
                <div class="row g-3 needs-validation">
                    <div class="col-9">
                        <label for="txtContainer" class="form-label">GRN Date</label>
                    </div>
                    <div class="col-10" style="margin-top: -9px;">
                      
                     <asp:TextBox id="txtDate" runat="server" ValidationGroup="TimeSlot" autocomplete="off" ReadOnly="true" BackColor="#dfe8f0" ClientIDMode="Static" class="form-control"></asp:TextBox>
                        
                        </div>
                    <div class="col-2" style="margin-top: -9px;">
                       <asp:ImageButton ID="btnDateChange" runat="server" ValidationGroup="5" ImageUrl="~/assets/img/datechange.png" OnClick="btnDateChange_Click" ToolTip="Change Date" Width="50" CssClass="form-control" />
                       
            
        
                    </div>
                    <div class="col-12">
                      <label for="ddlBranch" class="form-label">Branch</label>
                        <asp:DropDownList ID ="ddlBranch" runat="server" class="form-select">
                          
                        </asp:DropDownList>
                        
                    </div>

                    <div class="col-12">
                      <label for="txtSurName" class="form-label">Customer</label>
                    <asp:DropDownList ID ="ddlCustomer"  runat="server" class="form-select">
                            
                        </asp:DropDownList>
                      
                    </div>
                   <div class="col-12">
                      <label for="txtContainer" class="form-label">Scan or Type Container Number</label>
                     <asp:TextBox id="txtContainer" runat="server" ValidationGroup="TimeSlot" autocomplete="off"   ClientIDMode="Static" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator2" controltovalidate="txtContainer" ErrorMessage="Please type Container Number" ForeColor="OrangeRed" />
                    </div>
                    <div class="col-12">
                      <label for="txtGRN" class="form-label">GRN</label>
                     <asp:TextBox id="txtGRN" runat="server" ValidationGroup="TimeSlot"   ClientIDMode="Static"  ReadOnly="true" BackColor="#dfe8f0" class="form-control"></asp:TextBox>
                         </div>
                    <div class="col-12" style="text-align:center;">
                        <asp:ImageButton ID="btnCusNext"  class="btn btn-primary" OnClick="btnCusNext_Click" BackColor="Transparent" Width="120" ImageUrl="~/assets/img/next.png" Text="Next" runat="server" />
                    </div>
                   
                  </div>
       
      </div>

          </div><!-- End Recent Activity -->
             <div class="card" id="divScan" visible="false" runat="server">
           
               <div class="card-title" id="divHeader" runat="server" style="text-align:center;background-color: #4090ce;color:white">InBound Process</div>
           <div class="section error-404 d-flex flex-column align-items-center justify-content-center">
                <div class="row g-3 needs-validation">
                   
                    <div class="col-12">
                      <label for="txtContainer1" class="form-label">Container Number</label>
                     <asp:TextBox id="txtContainer1" runat="server" ValidationGroup="TimeSlot"   ClientIDMode="Static"  ReadOnly="true" BackColor="#dfe8f0" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator3" controltovalidate="txtContainer1" ErrorMessage="Please type Container Number" ForeColor="OrangeRed" />
                    </div>
                    <div id="divBMW" runat="server">
                    <div class="col-12">
                      <label for="txtHU" class="form-label">Scan or Type HU Number</label>
                     <asp:TextBox id="txtHU" runat="server" ValidationGroup="TimeSlot" ClientIDMode="Static" autocomplete="off" AutoPostBack="true" OnTextChanged="txtHU_TextChanged" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="reqName" controltovalidate="txtHU" ErrorMessage="Please type HU Number" ForeColor="OrangeRed" />
                    </div>
                     <div class="col-12" runat="server" id="divBar" visible="false">
                      <label for="txtHU" class="form-label">Scan or Type Barcode</label>
                     <asp:TextBox id="txtBarcode" runat="server" ValidationGroup="TimeSlot" ClientIDMode="Static" autocomplete="off" AutoPostBack="true" OnTextChanged="txtBarcode_TextChanged" class="form-control"></asp:TextBox>
                   
                     </div>
                     <div class="col-12" id="divSeq" runat="server" visible="false">
                      <label for="txtHU" class="form-label">Sequential Number</label>
                     <asp:TextBox id="txtSeqn" runat="server" ValidationGroup="TimeSlot" Text="000000001" ReadOnly="true" BackColor="#dfe8f0" ClientIDMode="Static" autocomplete="off" AutoPostBack="false" OnTextChanged="txtHU_TextChanged" class="form-control"></asp:TextBox>
                    </div>
                     <div class="col-12">
                      <label for="txtQty" class="form-label">Default Bin</label>
                     <asp:TextBox id="txtDefaultBin" runat="server" ValidationGroup="TimeSlot" Text="Zreceiving" ReadOnly="true" BackColor="#dfe8f0"  ClientIDMode="Static" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator4" controltovalidate="txtDefaultBin" ErrorMessage="Please Enter Qty" ForeColor="OrangeRed" />
                    </div>
                     <div class="col-12">
                      <label for="txtQty" class="form-label">Quantity</label>
                     <asp:TextBox id="txtQty" runat="server" ValidationGroup="TimeSlot" Text="1" ReadOnly="true" BackColor="#dfe8f0"  ClientIDMode="Static" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator1" controltovalidate="txtQty" ErrorMessage="Please Enter Qty" ForeColor="OrangeRed" />
                    </div>
                        </div>
                    <div id="divALP" visible="false" runat="server">
                         <div class="col-12">
                      <label for="txtQty" class="form-label">Total Quantity</label>
                     <asp:TextBox id="txtALPQty" runat="server" ValidationGroup="TimeSlot" AutoPostBack="true" OnTextChanged="txtALPQty_TextChanged" ClientIDMode="Static" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator7" controltovalidate="txtALPQty" ErrorMessage="Please Enter Qty" ForeColor="OrangeRed" />
                    </div>
                         <div class="col-12">
                      <label for="txtHU" class="form-label">Scan or Type Part Number</label>
                     <asp:TextBox id="txtSpare" runat="server" ValidationGroup="TimeSlot" ClientIDMode="Static" autocomplete="off" AutoPostBack="true" OnTextChanged="txtSpare_TextChanged" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator8" controltovalidate="txtSpare" ErrorMessage="Please type HU Number" ForeColor="OrangeRed" />
                    </div>
                    </div>
                    <div class="col-12" style="display:none;">
                        <asp:HiddenField ID="hdnContainer" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnBin" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnInboundFee" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnTransportFee" runat="server" Value="0" />
                        <asp:HiddenField ID="hdninitalNumber" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnFeeDays" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnContractDate" runat="server" Value="" />
                          <asp:Button ID="btnSave" class="btn btn-primary" OnClick="btnSave_Click" Text="Save" runat="server" />
                         </div>
                   <div class="col-12">
                        <asp:Button ID="btnPrintBarcode" class="btn btn-secondary" OnClick="btnPrintBarcode_Click" ValidationGroup="2" Text="Print Barcode" runat="server" />
                        
                         <asp:Button ID="btnComplete" class="btn btn-primary" OnClick="btnComplete_Click" OnClientClick="return myFunction();" Text="Scanning Complete" BackColor="#bc623c" runat="server" />
                   <asp:Button ID="btnBack" class="btn btn-secondary" OnClick="btnBack_Click" ValidationGroup="2" OnClientClick="return confirm('Are you sure you want to go back to the previous screen?');" Text="Back" runat="server" />
                                             <asp:Button ID="btnCancel" class="btn btn-danger" ValidationGroup="2" OnClientClick="return confirm('Are you sure to cancel the inbound process?');" OnClick="btnCancel_Click1" Text="Cancel" runat="server" />    
                   </div>
                   
                     <div class="col-12">
                        <asp:GridView ID="grdScans" runat="server" class="form-control" AutoGenerateColumns="False" Width="100%">
                <Columns>
                     <asp:TemplateField HeaderText="SNo" HeaderStyle-Width="10%" >
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="HU" HeaderText="HU"  HeaderStyle-Width="27%" />
                     <asp:TemplateField HeaderText="HU/Part Number" HeaderStyle-Width="37%" >
            <ItemTemplate>
                <img src='data:image/png;base64,<%# Eval("BarcodeBase64") %>' alt="HU/Part Number" style="max-width:100%" width="250" height="100" />
            </ItemTemplate>
        </asp:TemplateField>
                   
                     <asp:BoundField DataField="GRN" HeaderText="GRN"  HeaderStyle-Width="23%" />
                    <asp:BoundField DataField="DefaultBin" HeaderText="Default Bin" Visible="false" HeaderStyle-Width="10%" />
                    
                    <asp:BoundField DataField="Qty" HeaderText="Qty"  HeaderStyle-Width="5%" />
                </Columns>
                            <HeaderStyle BackColor="#4090ce" ForeColor="White" />
            </asp:GridView>
                        </div>
                  </div>
               
        
      </div>

          </div>
         
      </div>
          </div>
        <!-- Warning Modal -->
<div class="modal fade" id="warningModal" tabindex="-1" aria-labelledby="warningModalLabel" aria-hidden="true">
  <div class="modal-dialog">
    <div class="modal-content border-warning">
      <div class="modal-header bg-warning text-dark">
        <h5 class="modal-title" id="warningModalLabel">⚠ Warning</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body">
      Customer contract expired. Are you sure want to proceed further?
      </div>
      <div class="modal-footer">
        <asp:Button ID="btnYes" runat="server" Text="Yes" OnClick="btnYes_Click" class="btn btn-outline-warning" data-bs-dismiss="modal"/>
          <asp:Button ID="btnNo" runat="server" Text="No" class="btn btn-outline-secondary" data-bs-dismiss="modal"/>
      </div>
    </div>
  </div>
</div>

    </section>

  </main><!-- End #main -->

</asp:Content>
