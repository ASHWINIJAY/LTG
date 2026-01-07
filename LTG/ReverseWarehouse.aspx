<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ReverseWarehouse.aspx.cs" Inherits="LTG.ReverseWarehouse" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <main id="main" class="main">
         <script type="text/javascript">
             function toggleSelectAll(headerCheckbox) {
                 var grid = document.getElementById('<%= GridView1.ClientID %>');
                 var checkboxes = grid.querySelectorAll('input[type="checkbox"]');

                 // Check or uncheck all rows
                 for (var i = 1; i < checkboxes.length; i++) {
                     checkboxes[i].checked = headerCheckbox.checked;
                 }
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
function myFunction() {
    var c = confirm("Are you sure you want to update?");
    
    return c;
}
             </script>
         <script>
             function openPopup() {
                 document.getElementById("popup").style.display = "block";
             }

             function closePopup() {
                 document.getElementById("popup").style.display = "none";
             }
    </script>
         <script type="text/javascript">
             function openNewWindow(url) {
                 window.open(url, '_blank');
             }
    </script>
          <style type="text/css">
             tbody, td, tfoot, th, thead, tr {
                 border:1px solid black;
             }
         </style>
    <section class="section dashboard">
      <div class="row">

       <div class="popup" id="popup" visible="false" style="height:500px;overflow:scroll;" runat="server" >
        <asp:Button ID="btnClose" runat="server" Text="X" class="close-popup-btn" OnClick="btnClose_Click" ValidationGroup="2"/>
        <h2>Binned Details</h2>
        <div class="row">

       

        <!-- Right side columns -->
        <div class="col">

          <!-- Recent Activity -->
          <div class="card">
           

           <section class="section error-404 d-flex flex-column align-items-center justify-content-center">
                <div class="row">
                     <div class="col-12">
                   
                     <asp:TextBox id="txtSearchContainer" runat="server" placeholder="Search BinningNumber..." ValidationGroup="TimeSlot21" AutoPostBack="true" OnTextChanged="txtSearchContainer_TextChanged"  ClientIDMode="Static" style="padding-top: 14px;" Width="70%"></asp:TextBox>
                        <asp:ImageButton ID="imgSearch" runat="server" OnClick="imgSearch_Click" ValidationGroup="testabc" ImageUrl="~/assets/img/search.png" Width="35" />
                         </div>
                    <asp:GridView ID="grdScans" runat="server" class="form-control" OnSelectedIndexChanged="grdScans_SelectedIndexChanged" AutoGenerateColumns="False" Width="100%">
                <Columns>
                     <asp:TemplateField HeaderText="SNo" HeaderStyle-Width="10%" >
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:BoundField DataField="BinningNumber" HeaderText="GRN"  HeaderStyle-Width="30%" />
                    <asp:BoundField DataField="OutboundDate" HeaderText="Binned Date" DataFormatString="{0:dd/MMM/yyyy}" HtmlEncode="False"  HeaderStyle-Width="30%" />
                    <asp:TemplateField>
            <ItemTemplate>
                <asp:Button ID="SelectButton" runat="server" ValidationGroup="2" class="btn btn-primary" Text="Select" CommandName="Select" />
            </ItemTemplate>
        </asp:TemplateField>
                </Columns>
                            <HeaderStyle BackColor="#4090ce" ForeColor="White" />
            </asp:GridView>
                   
                  </div>
       
      </section>

          </div><!-- End Recent Activity -->

         
      </div>
          </div>
    </div>
             <div class="popup" id="divChangeDate" visible="false" runat="server" style="height:450px;overflow:scroll;" >
        <asp:Button ID="btnChangeDate" runat="server" Text="X" class="close-popup-btn" OnClick="btnChangeDate_Click" ValidationGroup="new"/>
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


            <div class="popup" id="divHUPopup" visible="false" style="height:500px;overflow:scroll;" runat="server" >
        <asp:Button ID="btnHuPopup" runat="server" Text="X" class="close-popup-btn" OnClick="btnHuPopup_Click" ValidationGroup="2"/>
        <h2>HU Details</h2>
        <div class="row">

       

        <!-- Right side columns -->
        <div class="col">

          <!-- Recent Activity -->
          <div class="card">
           

           <section class="section error-404 d-flex flex-column align-items-center justify-content-center">
                <div class="row">
                     <div class="col-12">
                   
                     <asp:TextBox id="txtSearchHU" runat="server" placeholder="Search HU..." ValidationGroup="TimeSlot21" AutoPostBack="true" OnTextChanged="txtSearchHU_TextChanged"  ClientIDMode="Static" style="padding-top: 14px;" Width="70%"></asp:TextBox>
                        <asp:ImageButton ID="imgHU" runat="server" OnClick="imgHU_Click" ValidationGroup="testabc" ImageUrl="~/assets/img/search.png" Width="35" />
                         </div>
                     <div class="col-12">
                    <asp:GridView ID="GridView1" runat="server" class="form-control"  OnSelectedIndexChanged="GridView1_SelectedIndexChanged" AutoGenerateColumns="False" Width="100%">
                <Columns>
                     <asp:TemplateField HeaderStyle-Width="10%" HeaderText="<input type='checkbox' id='chkHeader' onclick='toggleSelectAll(this)'/>">
              
            <ItemTemplate>
                <asp:CheckBox ID="chkSelect" runat="server" />
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" CssClass="headercenter"/>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
                     <asp:TemplateField HeaderText="SNo" HeaderStyle-Width="10%" >
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:BoundField DataField="HU" HeaderText="HU"  HeaderStyle-Width="80%" />
                   
                </Columns>
                            <HeaderStyle BackColor="#4090ce" ForeColor="White" />
            </asp:GridView>
                         </div>
                    <div class="col-12">
                         <asp:Button ID="btnSaveHU" class="btn btn-primary" OnClick="btnSaveHU_Click" ValidationGroup="TimeSlot" Text="Save" Width="100%" BackColor="#bc623c" runat="server" />
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
         <!-- End Recent Activity -->
             <div class="card" id="divScan" visible="true" runat="server">
           
               <div class="card-title" id="divHeader" runat="server" style="text-align:center;background-color: #4090ce;color:white">Binning Reverse Process</div>
           <div class="section error-404 d-flex flex-column align-items-center justify-content-center">
                <div class="row g-3 needs-validation">
                    
                   
                   
                     <div class="col-12">
                         <asp:Button ID="btnBrowse" class="btn btn-primary" OnClick="btnBrowse_Click" ValidationGroup="2" Text="Select Binning Number From List" runat="server" />
                       </div>
                    <div class="col-12">
                      <label for="txtGDN" class="form-label">Type Binning Number</label>
                     <asp:TextBox id="txtGDN" runat="server" ValidationGroup="TimeSlot" AutoPostBack="true" OnTextChanged="txtGDN_TextChanged"  ClientIDMode="Static" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="reqName" controltovalidate="txtGDN" ErrorMessage="Please type Binning Number" ForeColor="OrangeRed" />
                    </div>
                     <div class="col-9">
                        <label for="txtContainer" class="form-label">Return Date</label>
                    </div>
                    <div class="col-10" style="margin-top: -9px;">
                      
                     <asp:TextBox id="txtDate" runat="server" ValidationGroup="TimeSlot" autocomplete="off" ReadOnly="true" BackColor="#dfe8f0" ClientIDMode="Static" class="form-control"></asp:TextBox>
                        
                        </div>
                    <div class="col-2" style="margin-top: -9px;">
                       <asp:ImageButton ID="btnDateChange" runat="server" ValidationGroup="5" ImageUrl="~/assets/img/datechange.png" OnClick="btnDateChange_Click" ToolTip="Change Date" Width="50" CssClass="form-control" />
                       
            
        
                    </div>
                     <div class="col-12">
                      <label for="txtGRN" class="form-label">RBIN</label>
                     <asp:TextBox id="txtGRN" runat="server" ValidationGroup="TimeSlot"   ClientIDMode="Static"  ReadOnly="true" BackColor="#dfe8f0" class="form-control"></asp:TextBox>
                         </div>
                      <div class="col-12">
                      <label for="txtSurName" class="form-label">Reason for the return</label>
                   <asp:DropDownList id="ddlReason" runat="server" ValidationGroup="TimeSlot" AutoPostBack="false"   ClientIDMode="Static" class="form-select"></asp:DropDownList>
                       
                      
                    </div>
                     <div class="col-12">
                         <asp:Button ID="btnComplete" class="btn btn-primary" OnClick="btnComplete_Click" OnClientClick="return myFunction();" ValidationGroup="TimeSlot" Text="Submit" Width="100%" BackColor="#bc623c" runat="server" />
                       </div>
                        <div class="col-12">
                            <h2>Returned HU's</h2>
                    <asp:GridView ID="GridView2" runat="server" class="form-control"  AutoGenerateColumns="False" Width="100%">
                <Columns>
                    
                     <asp:TemplateField HeaderText="SNo" HeaderStyle-Width="10%" >
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:BoundField DataField="HU" HeaderText="HU"  HeaderStyle-Width="45%" />
                   <asp:BoundField DataField="BinningNumber" HeaderText="Binning Number"  HeaderStyle-Width="45%" />
                </Columns>
                            <HeaderStyle BackColor="#4090ce" ForeColor="White" />
            </asp:GridView>
                         </div>
                  </div>
               <div class="row" style="display:none;">
                    <div class="col-6">
                        <asp:HiddenField ID="hdnContainer" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnBin" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnInboundFee" runat="server" Value="0" />
                          <asp:Button ID="btnSave" class="btn btn-primary" OnClick="btnSave_Click" Text="Save" runat="server" />
                         </div>
                  
                  </div>
        
      </div>

          </div>
         
      </div>
          </div>
    </section>

  </main><!-- End #main -->

</asp:Content>
