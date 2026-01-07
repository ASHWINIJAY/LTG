<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="MonthEnd.aspx.cs" Inherits="LTG.MonthEnd" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <main id="main" class="main">
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <style>
    .progress {
        height: 25px;
    }
    .progress-bar {
        transition: width 0.5s;
    }
</style>

    <section class="section dashboard">
      <div class="row">

       

        <!-- Right side columns -->
        <div class="col">

          <!-- Recent Activity -->
          <div class="card">
              <div id="divTest" runat="server" class="card-title" style="text-align:center;background-color: #4090ce;color:white">Month-End Setup</div>
           <section class="section error-404 d-flex flex-column align-items-center justify-content-center">
                <div class="row g-3 needs-validation">
                   
                     <div class="col-12" id="divPassword" runat="server">
                      <label for="txtPassword" class="form-label">Previous Month-End Date</label>
                       <asp:TextBox id="txtPrevMonthEnd" runat="server" autocomplete="off" ReadOnly="true" BackColor="#dfe8f0" class="form-control"></asp:TextBox>
                        
                    </div>
                    <div class="col-12" id="divConPassword" runat="server">
                      <label for="txtConfirmPassword" class="form-label">New Month-End Date</label>
                       <asp:TextBox id="txtMonthEnd" runat="server" autocomplete="off" TextMode="Date" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator5" ForeColor="OrangeRed" controltovalidate="txtMonthEnd" errormessage="Please enter a Month-End Date!" />
                      
                    </div>
                  <div class="col-12" id="div1" runat="server">
                      <label for="txtConfirmPassword" class="form-label">Month-End Password</label>
                       <asp:TextBox id="txtMonthEndPwd" runat="server" autocomplete="off" TextMode="Password" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator1" ForeColor="OrangeRed" controltovalidate="txtMonthEndPwd" errormessage="Please enter a Month-End Password!" />
                      
                    </div>
                    <div class="col-12">
                        <asp:Button ID="btnCreate" class="btn btn-primary w-100" OnClick="btnCreate_Click" Text="Save" runat="server" />
                        <asp:HiddenField ID="hdnMonthendPwd" runat="server" Value="0" />
                    </div>
                   
                  </div>
       
      </section>

          </div><!-- End Recent Activity -->

         
      </div>
          </div>
    </section>
        

  </main><!-- End #main -->

</asp:Content>
