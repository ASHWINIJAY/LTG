<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="UserRoles.aspx.cs" Inherits="LTG.UserRoles" %>
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
            <div class="card-title" id="divTest" runat="server" style="text-align:center;background-color: #4090ce;color:white;padding-top:5px;padding-bottom:5px;">Roles Assignment</div>

           <section class="section error-404 d-flex flex-column align-items-center justify-content-center">
                <div class="row">                   
                   
                    <div class="col-12">
                      <label for="txtUserName" class="form-label">Username</label>
                      <div class="input-group has-validation">
                          <asp:TextBox id="txtUserName" runat="server" class="form-control"></asp:TextBox>
                           </div>
                       
                     
                    </div>
                   <div class="card-title" id="div1" runat="server" style="text-align:left;background-color: transparent;color:black;padding-top:1px;padding-bottom:1px;">Process Forms  <asp:CheckBox id="chkProcessAll" AutoPostBack="true" OnCheckedChanged="chkProcessAll_CheckedChanged" runat="server" /></div>

                   <div class="col-12">
                       <asp:CheckBox id="CheckBoxInboundProcess" runat="server" Text="Inbound Process" style="padding-left:50px;" />
        <asp:CheckBox id="CheckBoxWarehouseProcess" runat="server" Text="Binning Process" style="padding-left:50px;" />
        <asp:CheckBox id="CheckBoxPickingProcess" runat="server" Text="Picking Process" style="padding-left:50px;" />
        <asp:CheckBox id="CheckBoxOutboundProcess" runat="server" Text="Outbound Process" style="padding-left:50px;" />
   
                       </div>
                   <hr />
                    <div class="card-title" id="div2" runat="server" style="text-align:left;background-color: transparent;color:black;padding-top:1px;padding-bottom:1px;">Exception Reports Forms <asp:CheckBox id="chkExceptionAll" AutoPostBack="true" OnCheckedChanged="chkExceptionAll_CheckedChanged" runat="server" /></div>

                   <div class="col-12">
                        <asp:CheckBox id="CheckBoxInboundException" runat="server" Text="Inbound Exception" style="padding-left:50px;" />
        <asp:CheckBox id="CheckBoxWarehousedException" runat="server" Text="Binned Exception" style="padding-left:50px;" />
        <asp:CheckBox id="CheckBoxPickedException" runat="server" Text="Picked Exception" style="padding-left:50px;" />
   
                       </div>
                      <hr />
                    <div class="card-title" id="div3" runat="server" style="text-align:left;background-color: transparent;color:black;padding-top:2px;padding-bottom:2px;">Adjustment Forms <asp:CheckBox id="chkAdjustAll" AutoPostBack="true" OnCheckedChanged="chkAdjustAll_CheckedChanged" runat="server" /></div>

                   <div class="col-12">
                       <asp:CheckBox id="CheckBoxBinToBin" runat="server" Text="Bin To Bin" style="padding-left:50px;" />
        <asp:CheckBox id="CheckBoxContainerAdjustment" runat="server" Text="Container Adjustment" style="padding-left:50px;" />
        <asp:CheckBox id="CheckBoxChangeContainerToAnotherCustomer" runat="server" Text="Change Container To Another Customer" style="padding-left:50px;" />
  <asp:CheckBox id="chkUpdateContainer" runat="server" Text="Update Container" style="padding-left:50px;" />
                       </div>
                      <hr />
                     <div class="card-title" id="div4" runat="server" style="text-align:left;background-color: transparent;color:black;padding-top:1px;padding-bottom:1px;">Fee Setup Forms <asp:CheckBox id="chkFeeAll" AutoPostBack="true" OnCheckedChanged="chkFeeAll_CheckedChanged" runat="server" /></div>

                   <div class="col-12">
                       <asp:CheckBox id="CheckBoxInboundFeeSetup" runat="server" Text="Inbound Fee Setup" style="padding-left:50px;" />
        <asp:CheckBox id="CheckBoxStorageFeeSetup" runat="server" Text="Storage Fee Setup" style="padding-left:50px;" />
        <asp:CheckBox id="CheckBoxOutboundFeeSetup" runat="server" Text="Outbound Fee Setup" style="padding-left:50px;" />
  
                       </div>
                      <hr />
                     <div class="card-title" id="div5" runat="server" style="text-align:left;background-color: transparent;color:black;padding-top:1px;padding-bottom:1px;">Creation Forms <asp:CheckBox id="chkCreationAll" AutoPostBack="true" OnCheckedChanged="chkCreationAll_CheckedChanged" runat="server" /></div>

                     <div class="col-12">
                       <asp:CheckBox id="CheckBoxBinCreation" runat="server" Text="Bin Creation" style="padding-left:50px;" />
        <asp:CheckBox id="CheckBoxCustomerCreation" runat="server" Text="Customer Creation" style="padding-left:50px;" />
        <asp:CheckBox id="chkUOPCreation" runat="server" Text="UOP Creation" style="padding-left:50px;" />
       
                         <asp:CheckBox id="CheckBoxUserCreation" runat="server" Text="User Creation" style="padding-left:50px;" />
        <asp:CheckBox id="CheckBoxTransporterCreation" runat="server" Text="Transporter Creation" style="padding-left:50px;" />
  <asp:CheckBox id="chkReasonCreate" runat="server" Text="Return Reason Creation" style="padding-left:50px;" />
                      
                       </div>
                      <hr />
                    <div class="card-title" id="div6" runat="server" style="text-align:left;background-color: transparent;color:black;padding-top:1px;padding-bottom:1px;">Maintenance Forms <asp:CheckBox id="chkMaintainAll" AutoPostBack="true" OnCheckedChanged="chkMaintainAll_CheckedChanged" runat="server" /></div>

                    <div class="col-12">
        <asp:CheckBox id="CheckBoxCustomerMaintenance" runat="server" Text="Customer Maintenance" style="padding-left:50px;" />
        <asp:CheckBox id="chkUOPMaintain" runat="server" Text="UOP Maintenance" style="padding-left:50px;" />
       
                        <asp:CheckBox id="CheckBoxUserMaintenance" runat="server" Text="User Maintenance" style="padding-left:50px;" />
        <asp:CheckBox id="CheckBoxTransporterMaintenance" runat="server" Text="Transporter Maintenance" style="padding-left:50px;" />
                         <asp:CheckBox id="chkBinMaintain" runat="server" Text="Bin Maintenance" style="padding-left:50px;" />
                        <asp:CheckBox id="chkReturnMaintain" runat="server" Text="Return Reason Maintenance" style="padding-left:50px;" />
    </div>   <hr />
<div class="card-title" id="divReturn" runat="server" style="text-align:left;background-color: transparent;color:black;padding-top:1px;padding-bottom:1px;">HU's Return <asp:CheckBox id="chkReturn" AutoPostBack="true" OnCheckedChanged="chkReturn_CheckedChanged" runat="server" /></div>
<div class="col-12">
     <asp:CheckBox id="chkGRNReturn" runat="server" Text="GRN Return" style="padding-left:50px;" />
    <asp:CheckBox id="chkBinReturn" runat="server" Text="Binned Return" style="padding-left:50px;" />
     <asp:CheckBox id="chkPickReturn" runat="server" Text="Pickup Return"  style="padding-left:50px;" />
        <asp:CheckBox id="chkGDNReturn" runat="server" Text="GDN Return" style="padding-left:50px;" />
                         </div>
                     <hr />
                     <div class="card-title" id="div9" runat="server" style="text-align:left;background-color: transparent;color:black;padding-top:1px;padding-bottom:1px;">Re-Print/Re-Generate <asp:CheckBox id="chkDeliveryAll" AutoPostBack="true" OnCheckedChanged="chkDeliveryAll_CheckedChanged" runat="server" /></div>
                    <div class="col-12">
                         <asp:CheckBox id="chkDelivery" runat="server" Text="DeliveryNote Re-print" style="padding-left:50px;" />
                         <asp:CheckBox id="chkReDN" runat="server" Text="Re-Generate DeliveryNote" style="padding-left:50px;" />
                       <asp:CheckBox id="chkBarcodeReprint" runat="server" Text="Re-Generate Barcode" style="padding-left:50px;" />
                        </div>
                      <hr />
                     <div class="card-title" id="div7" runat="server" style="text-align:left;background-color: transparent;color:black;padding-top:1px;padding-bottom:1px;">Reports <asp:CheckBox id="chkReportAll" AutoPostBack="true" OnCheckedChanged="chkReportAll_CheckedChanged" runat="server" /></div>

                     <div class="col-12">
        <asp:CheckBox id="CheckBoxHUTrackingReportsDetails" runat="server" Text="HU Tracking Reports(Details)" style="padding-left:50px;" />
        <asp:CheckBox id="CheckBoxHUTrackingReportsSummary" runat="server" Text="HU Tracking Reports(Summary)" style="padding-left:50px;" />
                        
                      </div>  <div class="col-12">  <asp:CheckBox id="chkDetailMonth" runat="server" Text="HU Tracking Reports(Details) - Per Month" style="padding-left:50px;" />
        <asp:CheckBox id="chkSummaryMonth" runat="server" Text="HU Tracking Reports(Summary) - Per Month" style="padding-left:50px;" />
                   </div>  <div class="col-12">     <asp:CheckBox id="chkInboundReport" runat="server" Text="Inbound Tracking Report" style="padding-left:50px;" />
        <asp:CheckBox id="chkBinnedReport" runat="server" Text="Binned Tracking Report" style="padding-left:50px;" />
                     </div> <div class="col-12">     <asp:CheckBox id="chkPickedReport" runat="server" Text="Picked Tracking Report" style="padding-left:50px;" />
        <asp:CheckBox id="chkOutboundReport" runat="server" Text="Outbound Tracking Report" style="padding-left:50px;" />
                         <asp:CheckBox id="chkStockOnHand" runat="server" Text="Stock-onhand Report" style="padding-left:50px;" />
                          <asp:CheckBox id="chkGRNReturnReport" runat="server" Text="GRN Return Report" style="padding-left:50px;" />
                          <asp:CheckBox id="chkBinReturnReport" runat="server" Text="Binned Return Report" style="padding-left:50px;" />
                          
                         <asp:CheckBox id="chkGDNReturnReport" runat="server" Text="GDN Return Report" style="padding-left:50px;" />
                         <asp:CheckBox id="chkPickReturnReport" runat="server" Text="Pickup Return Report" style="padding-left:50px;" />
                         <asp:CheckBox id="chkDispatchedReport" runat="server" Text="Stock Despatched Report" style="padding-left:50px;" />
    <asp:CheckBox id="chkStockTakeReprint" runat="server" Text="Stock Accuracy Report" style="padding-left:50px;" />

                     </div>  <hr />
                     <div class="card-title" id="divLogs" runat="server" style="text-align:left;background-color: transparent;color:black;padding-top:1px;padding-bottom:1px;">Logs <asp:CheckBox id="chkAuditAll" AutoPostBack="true" OnCheckedChanged="chkAuditAll_CheckedChanged" runat="server" /></div>

                     <div class="col-12">
        <asp:CheckBox id="chkAudit" runat="server" Text="Audit Logs" style="padding-left:50px;" />
                         </div>
                      <hr />
                    <div class="card-title" id="div8" runat="server" style="text-align:left;background-color: transparent;color:black;padding-top:1px;padding-bottom:1px;">Admin Tools <asp:CheckBox id="chkSetupAll" AutoPostBack="true" OnCheckedChanged="chkSetupAll_CheckedChanged" runat="server" /></div>
                   
                     <div class="col-12">
        <asp:CheckBox id="chkSup" runat="server" Text="Supervisor Setup" style="padding-left:50px;" />
                         </div>
                    <div class="col-12">
        <asp:CheckBox id="chkMonthEnd" runat="server" Text="Month-End Setup" style="padding-left:50px;" />
                         </div>
                    <div class="col-12">
        <asp:CheckBox id="chkMonthEndPwd" runat="server" Text="Update Month-End Password" style="padding-left:50px;" />
                         </div>
                     <div class="col-12">
        <asp:CheckBox id="chkStockPwd" runat="server" Text="Stock Take Password Setup" style="padding-left:50px;" />
                         </div>
                     <div class="col-12">
        <asp:CheckBox id="chkMailNotification" runat="server" Text="Mail Notifications Setup" style="padding-left:50px;" />
                         </div>
                      <hr />
                    
                    <div class="card-title" id="div10" runat="server" style="text-align:left;background-color: transparent;color:black;padding-top:1px;padding-bottom:1px;">Stock Take Roles</div>

                   <div class="col-12">
                        <asp:RadioButton id="radCounter" runat="server" Text="Counter" GroupName="Options" style="padding-left:50px;" />
        <asp:RadioButton id="radManager" runat="server" Text="Manager Counter" GroupName="Options" style="padding-left:50px;" />
        <asp:RadioButton id="radAdmin" runat="server" Text="Admin" GroupName="Options" style="padding-left:50px;" />
   
                       </div>
                     <hr />
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
