<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ManagerStockTake.aspx.cs" Inherits="LTG.ManagerStockTake" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <main id="main" class="main">

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
             <script>
function myFunction() {
    var c = confirm("Are you sure complete to the stock take process !");
   
    return c;
                 }
                 function confirmSave() {
                     if (confirm('Stock Take already in progress. Do you want to start again?')) {
                         document.getElementById('<%= hfUserConfirmed.ClientID %>').value = 'true';
        } else {
            document.getElementById('<%= hfUserConfirmed.ClientID %>').value = 'false';
        }
                     __doPostBack('<%= btnConfirm.ClientID %>', '');
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
           
        <div class="row">

       

        <!-- Right side columns -->
        <div class="col">

          <!-- Recent Activity -->
          <div class="card">
           <h5 class="card-title" style="text-align:center;background-color: #4090ce;color:white">Stock Take Capture(Manager)</h5>

           <section class="section error-404 d-flex flex-column align-items-center justify-content-center">

                <div class="row">
                    
                    <div class="col-12">
                      <label for="txtHU" class="form-label">Type Or Scan HU Number</label>
                     <asp:TextBox id="txtAddHU" runat="server" ValidationGroup="TimeSlot7" AutoPostBack="false" ClientIDMode="Static" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator1" controltovalidate="txtAddHU" ErrorMessage="Please type HU Number" ForeColor="OrangeRed" />
                    </div>
                     <div class="col-12">
                      <label for="txtQty" class="form-label">Quantity</label>
                     <asp:TextBox id="txtQty" runat="server" AutoPostBack="false" Text="1" OnTextChanged="txtQty_TextChanged" ValidationGroup="TimeSlot"  ClientIDMode="Static" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator2" controltovalidate="txtQty" ErrorMessage="Please Enter Qty" ForeColor="OrangeRed" />
                    </div>
                     <div class="col-12">
                      <label for="txtGRN" class="form-label">STN</label>
                     <asp:TextBox id="txtGRN" runat="server" ValidationGroup="TimeSlot"   ClientIDMode="Static"  ReadOnly="true" BackColor="#dfe8f0" class="form-control"></asp:TextBox>
                         </div>
                    <div class="col-12">
                    <asp:Button ID="btnAddSave" Visible="true" class="btn btn-primary" OnClick="btnAddSave_Click" ValidationGroup="TimeSlot7" Text="Save" BackColor="#518f56" runat="server" />
                  <asp:Button ID="btrnViewPending" class="btn btn-primary" OnClick="btrnViewPending_Click" ValidationGroup="TimeSlot8" Text="View Pending Hu's" BackColor="#bc623c" runat="server" />
                   <asp:Button ID="btnComplete" class="btn btn-primary" OnClick="btnComplete_Click" ValidationGroup="TimeSlot8" OnClientClick="return myFunction();" Text="Complete the process" BackColor="#518b8f" runat="server" />
      <asp:HiddenField ID="hfUserConfirmed" runat="server" />
                        <asp:Button ID="btnConfirm" runat="server" OnClick="btnConfirm_Click" Style="display:none;" />
                    </div>

                    <div class="col-12">
                        <asp:GridView ID="grdScans" runat="server" class="form-control" AutoGenerateColumns="False" Width="100%">
                <Columns>
                     <asp:TemplateField HeaderText="SNo" HeaderStyle-Width="10%" >
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="HU" HeaderText="HU"  HeaderStyle-Width="57%" />                  
                     
                    
                    <asp:BoundField DataField="Qty" HeaderText="Qty"  HeaderStyle-Width="10%" />
                </Columns>
                            <HeaderStyle BackColor="#4090ce" ForeColor="White" />
            </asp:GridView>
                        </div>
                </div>
       
      </section>

          </div><!-- End Recent Activity -->

         
      </div>
          </div>
    </div>
          
           <div class="popup" id="popupdelete" visible="false" style="height:500px;overflow:scroll;" runat="server" >
        <asp:Button ID="btnDeleteClosw" runat="server" Text="X" class="close-popup-btn" OnClick="btnDeleteClosw_Click" ValidationGroup="2"/>
                 <h2>List of HU to count</h2>
        <div class="row">

       

        <!-- Right side columns -->
        <div class="col">

          <!-- Recent Activity -->
          <div class="card">
           

           <section class="section error-404 d-flex flex-column align-items-center justify-content-center">
                <div class="row">
                     <div class="col-12">
                   
                     <asp:TextBox id="txtStock" runat="server" ValidationGroup="TimeSlot21" AutoPostBack="true" OnTextChanged="txtStock_TextChanged"  ClientIDMode="Static" style="padding-top: 14px;" Width="70%"></asp:TextBox>
                        <asp:ImageButton ID="imgStock" runat="server" OnClick="imgStock_Click" ValidationGroup="testabc" ImageUrl="~/assets/img/search.png" Width="35" />
                         </div>
                    <asp:GridView ID="GridView1" runat="server" class="form-control" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" AutoGenerateColumns="False" Width="100%">
                <Columns>
                     <asp:TemplateField HeaderText="SNo" HeaderStyle-Width="10%" >
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:BoundField DataField="HU" HeaderText="HU"  HeaderStyle-Width="80%" />
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
       
          
    </section>

  </main><!-- End #main -->

</asp:Content>

