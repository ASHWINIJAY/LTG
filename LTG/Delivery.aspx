<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Delivery.aspx.cs" Inherits="LTG.Delivery" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <style type="text/css">

    .loading
    {
        font-family: Arial;
        font-size: 10pt;
        border: 5px solid #67CFF5;
        width: 200px;
        height: 100px;
        display: none;
        position: fixed;
        top:50%;
        background-color: White;
        z-index: 999;
    }
</style>
      <script type="text/javascript">
          function showLoader() {
              document.getElementById("divLoader").style.display = "block";
          }
          function CloseLoader() {
              document.getElementById("divLoader").style.display = "none";
          }
          function openSearch() {
              window.open('frmSearchJob.aspx', 'Popup_Window', 'scrollbars=yes,width=700,height=520');
          }
      </script>
    <form id="form1" runat="server">
        <div>
             <iframe id="pdfIframe" runat="server" width="100%" height="600px"></iframe>
             <div class="loading" runat="server" id="divLoader"  align="center">
    Loading. Please wait.<br />
    <br />
    <img src="loader.gif" alt=""/>
</div>
        </div>
    </form>
</body>
</html>
