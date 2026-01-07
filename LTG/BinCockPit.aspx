<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="BinCockPit.aspx.cs" Inherits="LTG.BinCockPit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <main id="main" class="main">

   <script type="text/javascript">
       function toggleSelectAll(headerCheckbox) {
           var grid = document.getElementById('<%= grdBin.ClientID %>');
           var checkboxes = grid.querySelectorAll('input[type="checkbox"]');

           // Check or uncheck all rows
           for (var i = 1; i < checkboxes.length; i++) {
               checkboxes[i].checked = headerCheckbox.checked;
           }
       }
       function toggleSelectAllBin(headerCheckbox) {
           var grid = document.getElementById('<%= grdBins.ClientID %>');
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
             function openPopup() {
                 document.getElementById("popup").style.display = "block";
             }

             function closePopup() {
                 document.getElementById("popup").style.display = "none";
             }
    </script>
    <section class="section dashboard">
      <div class="row">

         <div class="popup" id="popupAdd" visible="false" style="height:500px;overflow:scroll;" runat="server" >
        <asp:Button ID="btnAddclose" runat="server" Text="X" class="close-popup-btn" OnClick="btnAddclose_Click" ValidationGroup="2"/>
        <h2>Select Counter</h2>
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
                     <asp:BoundField DataField="Username" HeaderText="Username"  HeaderStyle-Width="20%" />
                     <asp:BoundField DataField="FirstName" HeaderText="Name"  HeaderStyle-Width="30%" />
                    <asp:TemplateField>
            <ItemTemplate>
                <asp:Button ID="SelectButton" runat="server" ValidationGroup="2" class="btn btn-primary" Text="Allocate" CommandName="Select" />
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
          <div class="popup" id="binPopup" visible="false" style="height:500px;overflow:scroll;" runat="server" >
        <asp:Button ID="btncloseBin" runat="server" Text="X" class="close-popup-btn" OnClick="btncloseBin_Click" ValidationGroup="2"/>
        <h2>Select Bin's</h2>
        <div class="row">

       

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
                   
                     <asp:TextBox id="txtBins" runat="server" ValidationGroup="TimeSlot21" AutoPostBack="true" OnTextChanged="txtBins_TextChanged"  ClientIDMode="Static" style="padding-top: 14px;" Width="70%"></asp:TextBox>
                        <asp:ImageButton ID="imgsearchBin" runat="server" OnClick="imgsearchBin_Click" ValidationGroup="testabc" ImageUrl="~/assets/img/search.png" Width="35" />
                         </div>
                    <asp:GridView ID="grdBins" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover" 
                              EmptyDataText="No Data Found" OnRowDataBound="grdBins_RowDataBound" DataKeyNames="Bin" HeaderStyle-CssClass="bg-dark text-light">
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
                    </Columns>
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
          <div class="card">
              <h5 class="card-title" style="text-align:center;background-color: #4090ce;color:white">CockPit-Allocate Stock Count By Bin</h5>
           <section class="section error-404 d-flex flex-column align-items-center justify-content-center">
                <div class="row g-3 needs-validation">
                     <div style="text-align:right;"><asp:ImageButton ID="imgExcel" runat="server" ToolTip="Export Grid Data To Excel" OnClick="imgExcel_Click" ImageUrl="~/assets/img/excel.png" Width="30" /></div>
                  <div class="col-12">
                   
                     <asp:TextBox id="txtSearchContainer" runat="server" ValidationGroup="TimeSlot21" placeholder="Enter Bin" AutoPostBack="true" OnTextChanged="txtSearchContainer_TextChanged"  ClientIDMode="Static" style="padding-top: 14px;" Width="70%"></asp:TextBox>
                        <asp:ImageButton ID="imgSearch" runat="server" OnClick="imgSearch_Click" ValidationGroup="testabc" ImageUrl="~/assets/img/search.png" Width="35" />
                         </div>
                    <div class="col-12">
                         <asp:Button ID="btnBrowseBin" runat="server" CssClass="btn btn-custom" style="background-color:#12b6fc;" Text="Browse Bin" OnClick="btnBrowseBin_Click" />
                        
                       <asp:Button ID="btnReset" runat="server" CssClass="btn btn-custom" style="background-color:#518f56;" Text="Click here to allocate" OnClick="btnReset_Click" />
                         <asp:Button ID="btnShowAllocate" runat="server" CssClass="btn btn-custom" Text="View Selected Bin's" OnClick="btnShowAllocate_Click" />
             <asp:Button ID="btnShowAll" runat="server" CssClass="btn btn-custom" Text="Show All" style="background-color:#518b8f;" OnClick="btnShowAll_Click" />
             
<asp:HiddenField ID="hfUserConfirmed" runat="server" />
                   </div>
                     
                    <div class="col-12">
                         <asp:GridView ID="grdBin" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover" 
                              EmptyDataText="No Data Found" OnRowDataBound="grdBin_RowDataBound" DataKeyNames="Bin" HeaderStyle-CssClass="bg-dark text-light">
                    <Columns>
                     <asp:TemplateField HeaderText="<input type='checkbox' id='chkHeader' onclick='toggleSelectAll(this)' />">
              
            <ItemTemplate>
                <asp:CheckBox ID="chkSelect" runat="server" />
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" CssClass="headercenter"/>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Line#">
                            <ItemTemplate>
                                <asp:Label ID="lblRowNumber" runat="server" Text='<%# Container.DataItemIndex + 1 %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                       
                       <asp:BoundField DataField="Bin" HeaderText="Bin" />
                         <asp:BoundField DataField="CustomerCode" HeaderText="Customer" />
                       
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
