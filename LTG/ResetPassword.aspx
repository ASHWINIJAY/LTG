<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="LTG.ResetPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
  <meta charset="utf-8">
  <meta content="width=device-width, initial-scale=1.0" name="viewport">

  <title>LTG|Reset Password</title>
  <meta content="" name="description">
  <meta content="" name="keywords">

  <!-- Favicons -->
  <link href="assets/img/favicon.png" rel="icon">
  <link href="assets/img/apple-touch-icon.png" rel="apple-touch-icon">

  <!-- Google Fonts -->
  <link href="https://fonts.gstatic.com" rel="preconnect">
  <link href="https://fonts.googleapis.com/css?family=Open+Sans:300,300i,400,400i,600,600i,700,700i|Nunito:300,300i,400,400i,600,600i,700,700i|Poppins:300,300i,400,400i,500,500i,600,600i,700,700i" rel="stylesheet">

  <!-- Vendor CSS Files -->
  <link href="assets/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet">
  <link href="assets/vendor/bootstrap-icons/bootstrap-icons.css" rel="stylesheet">
  <link href="assets/vendor/boxicons/css/boxicons.min.css" rel="stylesheet">
  <link href="assets/vendor/quill/quill.snow.css" rel="stylesheet">
  <link href="assets/vendor/quill/quill.bubble.css" rel="stylesheet">
  <link href="assets/vendor/remixicon/remixicon.css" rel="stylesheet">
  <link href="assets/vendor/simple-datatables/style.css" rel="stylesheet">

  <!-- Template Main CSS File -->
  <link href="assets/css/style.css" rel="stylesheet">

  <!-- =======================================================
  * Template Name: NiceAdmin
  * Template URL: https://bootstrapmade.com/nice-admin-bootstrap-admin-html-template/
  * Updated: Apr 20 2024 with Bootstrap v5.3.3
  * Author: BootstrapMade.com
  * License: https://bootstrapmade.com/license/
  ======================================================== -->
</head>
<body>
  <form id="form1" runat="server">

   <main>
    <div class="container">

        <section class="section register min-vh-100 d-flex flex-column align-items-center justify-content-center py-4">

            <div class="container">

                <div class="row justify-content-center">

                    <div class="col-lg-4 col-md-6 d-flex flex-column align-items-center justify-content-center">

                        <div class="d-flex justify-content-center" style="padding-top:1.5rem !important;">
                            <a href="#" class="logo d-flex align-items-center w-auto">
                                <img src="assets/img/ltglogo_new.png"
                                     style="max-height:140px;width:340px;" />
                            </a>
                        </div>

                        <p style="font-size:larger;color:#3e91cf;">
                            Warehouse Management System
                        </p>

                        <div class="card mb-3">

                           <div class="card-body">


<div class="pt-4 pb-2">
    <h5 class="card-title text-center pb-0 fs-4">
        Reset Your Password
    </h5>

    <p class="text-center small">
        Please enter and confirm your new password.
    </p>
</div>

<div class="row g-3">

    <div class="col-12">
        <asp:Label
            ID="lblMessage"
            runat="server"
            CssClass="text-danger fw-bold">
        </asp:Label>
    </div>

    <div class="col-12">
        <label class="form-label">
            New Password
        </label>

        <asp:TextBox
            ID="txtPassword"
            runat="server"
            TextMode="Password"
            CssClass="form-control">
        </asp:TextBox>

        <asp:RequiredFieldValidator
            ID="rfvPassword"
            runat="server"
            ControlToValidate="txtPassword"
            ErrorMessage="Please enter new password"
            ForeColor="OrangeRed"
            Display="Dynamic">
        </asp:RequiredFieldValidator>
    </div>

    <div class="col-12">
        <label class="form-label">
            Confirm Password
        </label>

        <asp:TextBox
            ID="txtConfirmPassword"
            runat="server"
            TextMode="Password"
            CssClass="form-control">
        </asp:TextBox>

        <asp:RequiredFieldValidator
            ID="rfvConfirmPassword"
            runat="server"
            ControlToValidate="txtConfirmPassword"
            ErrorMessage="Please confirm password"
            ForeColor="OrangeRed"
            Display="Dynamic">
        </asp:RequiredFieldValidator>

        <br />

        <asp:CompareValidator
            ID="cvPassword"
            runat="server"
            ControlToValidate="txtConfirmPassword"
            ControlToCompare="txtPassword"
            ErrorMessage="Passwords do not match"
            ForeColor="OrangeRed"
            Display="Dynamic">
        </asp:CompareValidator>
    </div>

    <div class="col-12">
        <small class="text-muted">
            Password must contain at least 8 characters.
        </small>
    </div>

    <div class="col-12">
        <asp:Button
            ID="btnReset"
            runat="server"
            Text="Reset Password"
            CssClass="btn btn-primary w-100"
            OnClick="btnReset_Click" />
    </div>

    <div class="col-12 text-center">
        <a href="Login.aspx">
            Back to Login
        </a>
    </div>

</div>


</div>

                        </div>

                    </div>

                </div>

            </div>

        </section>

    </div>
</main>

</form>
      <a href="#" class="back-to-top d-flex align-items-center justify-content-center"><i class="bi bi-arrow-up-short"></i></a>

  <!-- Vendor JS Files -->
  <script src="assets/vendor/apexcharts/apexcharts.min.js"></script>
  <script src="assets/vendor/bootstrap/js/bootstrap.bundle.min.js"></script>
  <script src="assets/vendor/chart.js/chart.umd.js"></script>
  <script src="assets/vendor/echarts/echarts.min.js"></script>
  <script src="assets/vendor/quill/quill.js"></script>
  <script src="assets/vendor/simple-datatables/simple-datatables.js"></script>
  <script src="assets/vendor/tinymce/tinymce.min.js"></script>
  <script src="assets/vendor/php-email-form/validate.js"></script>

  <!-- Template Main JS File -->
  <script src="assets/js/main.js"></script>
</body>
</html>
