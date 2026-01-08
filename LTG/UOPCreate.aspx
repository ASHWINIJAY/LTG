<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="UOPCreate.aspx.cs" Inherits="LTG.UOPCreate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main id="main" class="main">

    

    <section class="section dashboard">
      <div class="row">

       

        <!-- Right side columns -->
        <div class="col">

          <!-- Recent Activity -->
          <div class="card">
              <div id="divTest" runat="server" class="card-title" style="text-align:center;background-color: #4090ce;color:white">Unit Of Package(UOP) Creation</div>
           <section class="section error-404 d-flex flex-column align-items-center justify-content-center">
                <div class="row g-3 needs-validation">
                   
                    <div class="col-12">
                        <asp:TextBox id="txtCode" runat="server" ValidationGroup="TimeSlo4rt" MaxLength="50" ToolTip="Maximum 50 characters allowed" Visible="false"  ClientIDMode="Static" class="form-control"></asp:TextBox>
                      
                    </div>

                    <div class="col-12">
                      <label for="txtSurName" class="form-label">Unit Of Package</label>
                     <asp:TextBox id="txtSurName" runat="server" MaxLength="50" ToolTip="Maximum 50 characters allowed"  class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator1" ForeColor="OrangeRed" controltovalidate="txtSurName" errormessage="Please enter a Unit of Package!" />
                      
                    </div>
                   <div class="col-12">
                     
                     <asp:CheckBox id="chkActive" runat="server" Checked="true" Text="Active"></asp:CheckBox>
                       
                    </div>
                    <div class="col-12">
                        <asp:Button ID="btnCreate" class="btn btn-primary w-100" OnClick="btnCreate_Click" Text="Submit" runat="server" />
                    </div>
                   
                  </div>
       
      </section>

          </div><!-- End Recent Activity -->

         
      </div>
          </div>
    </section>

  </main><!-- End #main -->

</asp:Content>
