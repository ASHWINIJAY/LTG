<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="BinToBin.aspx.cs" Inherits="LTG.BinToBin" %>
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
    var c = confirm("Are you sure complete the OutBound Scan !");
    if (c == true) {
        document.getElementById("popup").style.display = "block";
    }
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

       <div class="popup" id="popup" visible="false" runat="server" style="height:500px;overflow:scroll;" >
        <asp:Button ID="btnClose" runat="server" Text="X" class="close-popup-btn" OnClick="btnClose_Click" ValidationGroup="2"/>
        <h2>HU Details</h2>
        <div class="row">

       

        <!-- Right side columns -->
        <div class="col">

          <!-- Recent Activity -->
          <div class="card">
           

           <section class="section error-404 d-flex flex-column align-items-center justify-content-center">
                <div class="row">
                    <asp:GridView ID="grdScans" runat="server" class="form-control" OnSelectedIndexChanged="grdScans_SelectedIndexChanged" AutoGenerateColumns="False" Width="100%">
                <Columns>
                     <asp:TemplateField HeaderText="SNo" HeaderStyle-Width="10%" >
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:BoundField DataField="Bin" HeaderText="HU"  HeaderStyle-Width="30%" />
                    <asp:BoundField DataField="HU" HeaderText="HU"  HeaderStyle-Width="30%" />
                    <asp:BoundField DataField="QtyOnHand" HeaderText="QtyOnHand"  HeaderStyle-Width="20%" />
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


        <!-- Right side columns -->
        <div class="col">

          <!-- Recent Activity -->
         <!-- End Recent Activity -->
             <div class="card" id="divScan" visible="true" runat="server">
           
               <div class="card-title" id="divHeader" runat="server" style="text-align:center;background-color: #4090ce;color:white">Bin To Bin Transfer</div>
           <div class="section error-404 d-flex flex-column align-items-center justify-content-center">
                <div class="row g-3 needs-validation">
                    <div class="col-12">
                      <label for="txtBin" class="form-label">Existing Bin</label>
                     <asp:TextBox id="txtBin" runat="server" ValidationGroup="TimeSlot" Text=""  ClientIDMode="Static" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator3" controltovalidate="txtBin" ErrorMessage="Please type/scan bin" ForeColor="OrangeRed" />
                    </div>
                   
                     <div class="col-12">
                         <asp:Button ID="btnBrowse" class="btn btn-primary" OnClick="btnBrowse_Click" ValidationGroup="2" Text="Select HU From List" runat="server" />
                       </div>
                    <div class="col-12">
                      <label for="txtHU" class="form-label">Scan or Type HU Number</label>
                     <asp:TextBox id="txtHU" runat="server" ValidationGroup="TimeSlot" AutoPostBack="true" OnTextChanged="txtHU_TextChanged"  ClientIDMode="Static" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="reqName" controltovalidate="txtHU" ErrorMessage="Please type HU Number" ForeColor="OrangeRed" />
                    </div>
                    
                     <div class="col-12">
                      <label for="txtNewBin" class="form-label">New Bin</label>
                     <asp:TextBox id="txtNewBin" runat="server" ValidationGroup="TimeSlot" Text=""  ClientIDMode="Static" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator1" controltovalidate="txtNewBin" ErrorMessage="Please type/scan bin" ForeColor="OrangeRed" />
                    </div>
                     <div class="col-12">
                         <asp:Button ID="btnComplete" class="btn btn-primary" OnClick="btnComplete_Click" ValidationGroup="TimeSlot" Text="Update" BackColor="#bc623c" runat="server" />
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
