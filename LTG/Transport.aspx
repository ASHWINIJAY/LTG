<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Transport.aspx.cs" Inherits="LTG.Transport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <main id="main" class="main">

    

    <section class="section dashboard">
      <div class="row">

       

        <!-- Right side columns -->
        <div class="col">

          <!-- Recent Activity -->
          <div class="card">
              <div class="card-title" id="divTest" runat="server" style="text-align:center;background-color: #4090ce;color:white">Transporter Creation</div>
           <section class="section error-404 d-flex flex-column align-items-center justify-content-center">
                <div class="row g-3 needs-validation">
                   <div class="col-12">
                      <label for="ddlBranch" class="form-label">Branch</label>
                        <asp:DropDownList ID ="ddlBranch" AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" runat="server" class="form-select">
                        </asp:DropDownList>
                        
                    </div>
                     <div class="col-12">
                      <label for="ddlBranch" class="form-label">Customer</label>
                        <asp:DropDownList ID ="ddlCustomer" runat="server" AutoPostBack="false" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged" class="form-select">
                            
                        </asp:DropDownList>
                        
                    </div>
                   

                    <div class="col-12">
                      <label for="txtSurName" class="form-label">Transporter Name</label>
                     <asp:TextBox id="txtSurName" runat="server" autocomplete="off"  class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator1" ForeColor="OrangeRed" controltovalidate="txtSurName" errormessage="Please enter a transportername!" />
                      
                    </div>
                  
                    <div class="col-12">
                        <asp:Button ID="btnCreate" class="btn btn-primary w-100" OnClick="btnCreate_Click" Text="Save" runat="server" />
                    </div>
                   
                  </div>
       
      </section>

          </div><!-- End Recent Activity -->

         
      </div>
          </div>
    </section>

  </main><!-- End #main -->
</asp:Content>
