<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="CustomerSetup.aspx.cs" Inherits="LTG.CustomerSetup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <main id="main" class="main">

    

    <section class="section dashboard">
      <div class="row">

       

        <!-- Right side columns -->
        <div class="col">

          <!-- Recent Activity -->
          <div class="card">
              <div id="divHeader" runat="server" class="card-title" style="text-align:center;background-color: #4090ce;color:white">Customer Creation</div>
           <section class="section error-404 d-flex flex-column align-items-center justify-content-center">
                <div class="row g-3 needs-validation">
                    <div class="col-12">
                      <label for="ddlBranch" class="form-label">Branch</label>
                        <asp:DropDownList ID ="ddlBranch" runat="server" class="form-select">
                            
                        </asp:DropDownList>
                        
                    </div>
                    <div class="col-12">
                      <label for="txtCode" class="form-label">Customer Code</label>
                        <asp:TextBox id="txtCode" runat="server" ValidationGroup="TimeSlot" MaxLength="10" ToolTip="Maximum 10 characters allowed" ClientIDMode="Static" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="reqName" controltovalidate="txtCode" ForeColor="OrangeRed" errormessage="Please enter a customercode!" />
                      
                    </div>

                    <div class="col-12">
                      <label for="txtSurName" class="form-label">Customer Name</label>
                     <asp:TextBox id="txtSurName" runat="server" MaxLength="50" ToolTip="Maximum 50 characters allowed" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator1" ForeColor="OrangeRed" controltovalidate="txtSurName" errormessage="Please enter a customername!" />
                      
                    </div>
                  <div class="col-12">
                      <label for="ddlBin" class="form-label">Default Bin</label>
                     <asp:DropDownList ID ="ddlBin" runat="server" class="form-select">
                           
                        </asp:DropDownList>
                    </div>
                     <div class="col-12">
                      <label for="txtSurName" class="form-label">Address 1</label>
                     <asp:TextBox id="txtAddr1" runat="server"  class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator2" ForeColor="OrangeRed" controltovalidate="txtAddr1" errormessage="Please enter a Address1!" />
                      
                    </div>
                     <div class="col-12">
                      <label for="txtSurName" class="form-label">Address 2</label>
                     <asp:TextBox id="txtAddr2" runat="server"  class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator3" ForeColor="OrangeRed" controltovalidate="txtAddr2" errormessage="Please enter a Address2!" />
                      
                    </div>
                     <div class="col-12">
                      <label for="txtSurName" class="form-label">Address 3</label>
                     <asp:TextBox id="txtAddr3" runat="server"  class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator4" ForeColor="OrangeRed" controltovalidate="txtAddr3" errormessage="Please enter a Address3!" />
                      
                    </div>
                     <div class="col-12">
                      <label for="txtSurName" class="form-label">Address 4</label>
                     <asp:TextBox id="txtAddr4" runat="server"  class="form-control"></asp:TextBox>
                       
                    </div>
                     <div class="col-12">
                      <h2>Delivery Address</h2>
                     <asp:CheckBox id="chkDelivery" runat="server" Text="Same as above" AutoPostBack="true" OnCheckedChanged="chkDelivery_CheckedChanged"></asp:CheckBox>
                       
                    </div>

                     <div class="col-12">
                      <label for="txtSurName" class="form-label">Address 1</label>
                     <asp:TextBox id="txtDelAddr1" runat="server"  class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator5" ForeColor="OrangeRed" controltovalidate="txtDelAddr1" errormessage="Please enter a Address1!" />
                      
                    </div>
                     <div class="col-12">
                      <label for="txtSurName" class="form-label">Address 2</label>
                     <asp:TextBox id="txtDelAddr2" runat="server"  class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator6" ForeColor="OrangeRed" controltovalidate="txtDelAddr2" errormessage="Please enter a Address2!" />
                      
                    </div>
                     <div class="col-12">
                      <label for="txtSurName" class="form-label">Address 3</label>
                     <asp:TextBox id="txtDelAddr3" runat="server"  class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator7" ForeColor="OrangeRed" controltovalidate="txtDelAddr3" errormessage="Please enter a Address3!" />
                      
                    </div>
                     <div class="col-12">
                      <label for="txtSurName" class="form-label">Address 4</label>
                     <asp:TextBox id="txtDelAddr4" runat="server"  class="form-control"></asp:TextBox>
                       
                    </div>
                     <div class="col-12">
                     
                     <asp:CheckBox id="chkActive" runat="server" Text="Active" AutoPostBack="false" OnCheckedChanged="chkDelivery_CheckedChanged"></asp:CheckBox>
                       
                    </div>
                    <div class="col-12">
                        <asp:Button ID="btnCreate" class="btn btn-primary w-100" OnClick="btnCreate_Click" Text="Create Customer" runat="server" />
                    </div>
                   
                  </div>
       
      </section>

          </div><!-- End Recent Activity -->

         
      </div>
          </div>
    </section>

  </main><!-- End #main -->

</asp:Content>
