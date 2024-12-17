<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="UserCreation.aspx.cs" Inherits="LTG.UserCreation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main id="main" class="main">

      <style>
        /* Basic styling for the page */
       

        /* Popup styling */
        .popup {
            display: none; /* Hidden by default */
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            width: 300px;
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

    <section class="section dashboard">
      <div class="row">

       

        <!-- Right side columns -->
        <div class="col">

          <!-- Recent Activity -->
          <div class="card">
           

           <section class="section error-404 d-flex flex-column align-items-center justify-content-center">
                <div class="row g-3 needs-validation">
                    <div style="text-align:right;">
                        <asp:ImageButton ID="imgClose" runat="server" ToolTip="Close" OnClick="imgClose_Click" ImageUrl="~/assets/img/cancel.png" Width="30" /></div>
         
                    <div class="col-12" style="margin-top: -9px;">
                      <label for="txtName" class="form-label">First Name</label>
                        <asp:TextBox id="txtName" runat="server" ValidationGroup="TimeSlot"  ClientIDMode="Static" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="reqName" controltovalidate="txtName" ForeColor="OrangeRed" errormessage="Please enter a firstname!" />
                      
                    </div>

                    <div class="col-12" style="margin-top: -9px;">
                      <label for="txtSurName" class="form-label">Sur Name</label>
                     <asp:TextBox id="txtSurName" runat="server"  class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator1" ForeColor="OrangeRed" controltovalidate="txtSurName" errormessage="Please enter a surname!" />
                      
                    </div>
                    <div class="col-12" style="margin-top: -9px;">
                      <label for="txtEmail" class="form-label">Email</label>
                     <asp:TextBox id="txtEmail" runat="server" TextMode="Email" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator2" ForeColor="OrangeRed" controltovalidate="txtEmail" errormessage="Please enter a email!" />
                      
                    </div>
                    <div class="col-12" style="margin-top: -9px;">
                      <label for="ddlBranch" class="form-label">Branch</label>
                        <asp:DropDownList ID ="ddlBranch" runat="server" class="form-select">
                            <asp:ListItem  Text="Rosslyn" Value="1"></asp:ListItem>
                             <asp:ListItem  Text="Kempton Park" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                        
                    </div>
                     <div class="col-12">
                      <label for="ddlDepartment" class="form-label">Department</label>
                    <asp:DropDownList ID="ddlRoles" runat="server" class="form-select">
                        <asp:ListItem Text="Admin" Value="1">

                        </asp:ListItem>
                        <asp:ListItem Text="Others" Value="2">

                        </asp:ListItem>
                        </asp:DropDownList>
                    </div>
                     <div class="col-12">
                      <label for="txtMobile" class="form-label">Mobile</label>
                     <asp:TextBox id="txtMobile" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    <div class="col-12">
                      <label for="txtUserName" class="form-label">Username</label>
                      <div class="input-group has-validation">
                        <span class="input-group-text" id="inputGroupPrepend">@</span>
                          <asp:TextBox id="txtUserName" runat="server" class="form-control"></asp:TextBox>
                           </div>
                        <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator3" ForeColor="OrangeRed" controltovalidate="txtUserName"  errormessage="Please enter a username!" />
                      
                     
                    </div>
                     <div class="col-12" id="divResetPassword" runat="server">
                     
                     <asp:CheckBox id="chkReset" runat="server" Text="Reset Password" Visible="false" OnCheckedChanged="chkReset_CheckedChanged" AutoPostBack="true"></asp:CheckBox>
                       
                    </div>
                    <div class="col-12" id="divPassword" runat="server">
                      <label for="txtPassword" class="form-label">Password</label>
                       <asp:TextBox id="txtPassword" runat="server" TextMode="Password" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator4" ForeColor="OrangeRed" controltovalidate="txtPassword" errormessage="Please enter a password!" />
                      
                    </div>
                    <div class="col-12" id="divConPassword" runat="server" style="margin-top: -9px;">
                      <label for="txtConfirmPassword" class="form-label">Confirm Password</label>
                       <asp:TextBox id="txtConfirmPassword" runat="server" TextMode="Password" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator5" ForeColor="OrangeRed" controltovalidate="txtConfirmPassword" errormessage="Please enter a confirm password!" />
                      
                    </div>
                    <div class="col-12">
                     
                     <asp:CheckBox id="chkActive" runat="server" Text="Active" AutoPostBack="false"></asp:CheckBox>
                       
                    </div>
                   
                    <div class="col-12">
                        <asp:Button ID="btnCreate" OnClick="btnCreate_Click" class="btn btn-primary w-100" Text="Submit" runat="server" />
                    </div>
                   
                  </div>
       
      </section>

          </div><!-- End Recent Activity -->

         
      </div>
          </div>
    </section>

  </main><!-- End #main -->

</asp:Content>
