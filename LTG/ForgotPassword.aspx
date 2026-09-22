<%@ Page Language="C#" AutoEventWireup="true"
CodeBehind="ForgotPassword.aspx.cs"
Inherits="LTG.ForgotPassword" %>
<!DOCTYPE html>

<html lang="en">
    <head>
  <meta charset="utf-8">
  <meta content="width=device-width, initial-scale=1.0" name="viewport">

  <title>LTG|Forgot Password</title>
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

                        <div class="d-flex justify-content-center" style="padding-top: 1.5rem !important;">
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
                                        Forgot Password
                                    </h5>

                                    <p class="text-center small">
                                        Enter your username to request a password reset.
                                    </p>
                                </div>

                                <asp:Label ID="lblMessage"
                                    runat="server"
                                    CssClass="text-danger" />

                                <div class="col-12">
                                    <label class="form-label">
                                        Username
                                    </label>

                                    <asp:TextBox
                                        ID="txtUsername"
                                        runat="server"
                                        CssClass="form-control">
                                    </asp:TextBox>
                                </div>

                                <br />

                                <div class="col-12">

                                    <asp:Button
                                        ID="btnSubmit"
                                        runat="server"
                                        Text="Submit Request"
                                        CssClass="btn btn-primary w-100"
                                        OnClick="btnSubmit_Click"
                                        OnClientClick="return confirm('Are you sure you want to reset your password?');" />

                                </div>

                                <br />

                                <div class="text-center">
                                    <a href="Login.aspx">
                                        Back to Login
                                    </a>
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
</body>
</html>