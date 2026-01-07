<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ReturnReason.aspx.cs" Inherits="LTG.ReturnReason" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main id="main" class="main">

    

    <section class="section dashboard">
      <div class="row">

       

        <!-- Right side columns -->
        <div class="col">

          <!-- Recent Activity -->
          <div class="card">
              <div id="divTest" runat="server" class="card-title" style="text-align:center;background-color: #4090ce;color:white">Return Reason Creation</div>
           <section class="section error-404 d-flex flex-column align-items-center justify-content-center">
                <div class="row g-3 needs-validation">
                    <div class="col-12">
                      <label for="txtCode" class="form-label">Return Type</label>
                       <asp:DropDownList ID ="ddlReturnType" runat="server" class="form-select">
                          <asp:ListItem Text="GRN Return" Value="1"></asp:ListItem>
                           <asp:ListItem Text="Binned Return" Value="2"></asp:ListItem>
                           <asp:ListItem Text="Pickup Return" Value="3"></asp:ListItem>
                           <asp:ListItem Text="GDN Return" Value="4"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-12">
                      <label for="txtCode" class="form-label">Return Reason Code</label>
                        <asp:TextBox id="txtCode" runat="server" ValidationGroup="TimeSlot" MaxLength="50" ToolTip="Maximum 50 characters allowed"  ClientIDMode="Static" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="reqName" controltovalidate="txtCode" ForeColor="OrangeRed" errormessage="Please enter a reason code!" />
                      
                    </div>

                    <div class="col-12">
                      <label for="txtSurName" class="form-label">Return Reason Name</label>
                     <asp:TextBox id="txtSurName" runat="server" MaxLength="50" ToolTip="Maximum 50 characters allowed"  class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator1" ForeColor="OrangeRed" controltovalidate="txtSurName" errormessage="Please enter a reason name!" />
                      
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

