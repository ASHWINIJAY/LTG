<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="LTG.Test" %>

<!DOCTYPE html>

<!DOCTYPE html>
<html lang="en">
<head>
    <style>
        /* Full screen overlay for the loader */
        #loadingDiv {
            position: fixed;
            width: 100%;
            height: 100%;
            top: 0;
            left: 0;
            background-color: rgba(0, 0, 0, 0.7);
            z-index: 9999;
            display: none; /* Initially hidden */
        }

        /* Spinner */
        #spinner {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            border: 8px solid #f3f3f3;
            border-radius: 50%;
            border-top: 8px solid #3498db;
            width: 60px;
            height: 60px;
            animation: spin 1s linear infinite;
        }

        /* Spinner animation */
        @keyframes spin {
            0% { transform: rotate(0deg); }
            100% { transform: rotate(360deg); }
        }
    </style>

    <script type="text/javascript">
        // Show the loader when the page starts loading
        window.onload = function() {
            document.getElementById("loadingDiv").style.display = "none";
        }

        // Show the loader when the page starts loading
        window.addEventListener("load", function () {
            document.getElementById("loadingDiv").style.display = "none";
        });

        // Show loader when navigating away (useful for full-page postbacks)
        window.addEventListener("beforeunload", function () {
            document.getElementById("loadingDiv").style.display = "block";
        });
    </script>
</head>
<body>

    <!-- Loader HTML -->
    <div id="loadingDiv">
        <div id="spinner"></div>
    </div>

    <!-- Your page content -->
    <form id="form1" runat="server">
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" />
    </form>

</body>
</html>

