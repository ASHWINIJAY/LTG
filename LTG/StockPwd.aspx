<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="StockPwd.aspx.cs" Inherits="LTG.StockPwd" %>
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
              <div id="divTest" runat="server" class="card-title" style="text-align:center;background-color: #4090ce;color:white">Stock Take Password Setup</div>
           <section class="section error-404 d-flex flex-column align-items-center justify-content-center">
                <div class="row g-3 needs-validation">
                   
                     <div class="col-12" id="divPassword" runat="server">
                      <label for="txtPassword" class="form-label">Password</label>
                       <asp:TextBox id="txtPassword" runat="server" TextMode="Password" autocomplete="new-password" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator4" ForeColor="OrangeRed" controltovalidate="txtPassword" errormessage="Please enter a password!" />
                      
                    </div>
                    <div class="col-12" id="divConPassword" runat="server" style="margin-top: -9px;">
                      <label for="txtConfirmPassword" class="form-label">Confirm Password</label>
                       <asp:TextBox id="txtConfirmPassword" runat="server" autocomplete="new-password" TextMode="Password" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator5" ForeColor="OrangeRed" controltovalidate="txtConfirmPassword" errormessage="Please enter a confirm password!" />
                      
                    </div>
                  <div id="passwordStrength" class="progress">
    <div id="strengthBar" class="progress-bar" role="progressbar" style="width: 0;"></div>
</div>
                     <div class="col-12" id="div1" runat="server">
                      <asp:Label ID="lblStrength" runat="server" Text=""></asp:Label>

                       
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
         <script type="text/javascript">
    $(document).ready(function () {
        $('#<%=txtPassword.ClientID%>').on('keyup', function () {
            var password = $(this).val();
            var strength = getPasswordStrength(password);

            // Update the progress bar
            var progressBar = $('#strengthBar');
            progressBar.css('width', strength.percent + '%');

            // Set the strength bar color based on strength score
            if (strength.score === 4) {
                progressBar.removeClass().addClass('progress-bar bg-success');
            } else if (strength.score === 3) {
                progressBar.removeClass().addClass('progress-bar bg-info');
            } else if (strength.score === 2) {
                progressBar.removeClass().addClass('progress-bar bg-danger');
            } else {
                progressBar.removeClass().addClass('progress-bar bg-danger');
            }

            // Display strength text
            $('#<%=lblStrength.ClientID%>').text(strength.text);
        });

        function getPasswordStrength(password) {
            var score = 0;
            // Check length
            if (password.length >= 8) score++;
            // Check for lowercase letter
            if (/[a-z]/.test(password)) score++;
            // Check for uppercase letter
            if (/[A-Z]/.test(password)) score++;
            // Check for numbers and special characters
            if (/\d/.test(password) || /[!@#$%^&*]/.test(password)) score++;

            // Calculate the percentage for the progress bar
            var percent = (score / 4) * 100;

            // Define text for each score
            var strengthText;
            switch (score) {
                case 4:
                    strengthText = 'Very Strong';
                    break;
                case 3:
                    strengthText = 'Strong';
                    break;
                case 2:
                    strengthText = 'Medium';
                    break;
                default:
                    strengthText = 'Weak';
                    break;
            }

            return { score: score, percent: percent, text: strengthText };
        }
    });
         </script>

  </main><!-- End #main -->

</asp:Content>
