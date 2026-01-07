using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LTG
{
    public partial class UserRoles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               
                if (Request.QueryString["Code"] != null)
                {
                    txtUserName.Text = Request.QueryString["Code"].ToString();
                    LoadData();
                }
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            save();
        }
        private void save()
        {
            string username = txtUserName.Text; // Get this from your form or session
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            // Get checkbox values from the form
            int inboundProcess = CheckBoxInboundProcess.Checked ? 1 : 0;
            int warehouseProcess = CheckBoxWarehouseProcess.Checked ? 1 : 0;
            int pickingProcess = CheckBoxPickingProcess.Checked ? 1 : 0;
            int outboundProcess = CheckBoxOutboundProcess.Checked ? 1 : 0;
            int inboundException = CheckBoxInboundException.Checked ? 1 : 0;
            int warehousedException = CheckBoxWarehousedException.Checked ? 1 : 0;
            int pickedException = CheckBoxPickedException.Checked ? 1 : 0;
            int binToBin = CheckBoxBinToBin.Checked ? 1 : 0;
            int containerAdjustment = CheckBoxContainerAdjustment.Checked ? 1 : 0;
            int changeContainerToAnotherCustomer = CheckBoxChangeContainerToAnotherCustomer.Checked ? 1 : 0;
            int inboundFeeSetup = CheckBoxInboundFeeSetup.Checked ? 1 : 0;
            int storageFeeSetup = CheckBoxStorageFeeSetup.Checked ? 1 : 0;
            int outboundFeeSetup = CheckBoxOutboundFeeSetup.Checked ? 1 : 0;
            int binCreation = CheckBoxBinCreation.Checked ? 1 : 0;
            int customerCreation = CheckBoxCustomerCreation.Checked ? 1 : 0;
            int userCreation = CheckBoxUserCreation.Checked ? 1 : 0;
            int transporterCreation = CheckBoxTransporterCreation.Checked ? 1 : 0;
            int customerMaintenance = CheckBoxCustomerMaintenance.Checked ? 1 : 0;
            int userMaintenance = CheckBoxUserMaintenance.Checked ? 1 : 0;
            int transporterMaintenance = CheckBoxTransporterMaintenance.Checked ? 1 : 0;
            int huTrackingReportsDetails = CheckBoxHUTrackingReportsDetails.Checked ? 1 : 0;
            int huTrackingReportsSummary = CheckBoxHUTrackingReportsSummary.Checked ? 1 : 0;
            int binMaintain = chkBinMaintain.Checked ? 1 : 0;
            int deliveryNote = chkDelivery.Checked ? 1 : 0;
            int detailMonth = chkDetailMonth.Checked ? 1 : 0;
            int summaryMonth = chkSummaryMonth.Checked ? 1 : 0;
            int inboundReport = chkInboundReport.Checked ? 1 : 0;
            int binnedReport = chkBinnedReport.Checked ? 1 : 0;
            int pickedReport = chkPickedReport.Checked ? 1 : 0;
            int outboundReport = chkOutboundReport.Checked ? 1 : 0;
            int audit = chkAudit.Checked ? 1 : 0;
            int supervisor = chkSup.Checked ? 1 : 0;
            int reDeliveryNote = chkReDN.Checked ? 1 : 0;
            int monthEnd = chkMonthEnd.Checked ? 1 : 0;
            int monthEndPwd = chkMonthEndPwd.Checked ? 1 : 0;
            int stockOnHand = chkStockOnHand.Checked ? 1 : 0;
            int updateContainer = chkUpdateContainer.Checked ? 1 : 0;
            int GDNReturn = chkGDNReturn.Checked ? 1 : 0;
            int GRNReturn = chkGRNReturn.Checked ? 1 : 0;
            int PickupReturn = chkPickReturn.Checked ? 1 : 0;
            int GDNReturnRpt = chkGDNReturnReport.Checked ? 1 : 0;
            int GRNReturnRpt = chkGRNReturnReport.Checked ? 1 : 0;
            int binReturn = chkBinReturn.Checked ? 1 : 0;
            int binReturnRpt = chkBinReturnReport.Checked ? 1 : 0;
            int PickupReturnRpt = chkPickReturnReport.Checked ? 1 : 0;
            int returnReason = chkReasonCreate.Checked ? 1 : 0;
            int reasonMaintain = chkReturnMaintain.Checked ? 1 : 0;
            int barcodeRePrint = chkBarcodeReprint.Checked ? 1 : 0;
            int DispatchedRpt = chkDispatchedReport.Checked ? 1 : 0;
            int stockTakeReprint = chkStockTakeReprint.Checked ? 1 : 0;
            int stockPwd = chkStockPwd.Checked ? 1 : 0;
            int mailNotification = chkMailNotification.Checked ? 1 : 0;
            int stockRoles = 0;
            if (radManager.Checked)
                stockRoles = 1;
            if (radAdmin.Checked)
                stockRoles = 2;

            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();

                // Check if the record exists
                string checkQuery = "SELECT COUNT(*) FROM UserRoles WHERE Username = @Username";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@Username", username);

                int recordExists = (int)checkCmd.ExecuteScalar();

                string query;

                if (recordExists > 0)
                {
                    // Update existing record
                    query = @"
                    UPDATE UserRoles
                    SET MailNotification=@MailNotification,StockTakeReprint=@StockTakeReprint,StockPwd=@StockPwd,
StockRoles=@StockRoles,GRNReturn=@GRNReturn,PickupReturn=@PickupReturn,
                        InboundProcess = @InboundProcess,
ReverseWarehouseReport = @ReverseWarehouseReport,
ReverseWarehouse = @ReverseWarehouse,
                        WarehouseProcess = @WarehouseProcess,
                        PickingProcess = @PickingProcess,
                        OutboundProcess = @OutboundProcess,
                        InboundException = @InboundException,
                        WarehousedException = @WarehousedException,
                        PickedException = @PickedException,
                        BinToBin = @BinToBin,
                        ContainerAdjustment = @ContainerAdjustment,
                        ChangeContainerToAnotherCustomer = @ChangeContainerToAnotherCustomer,
                        InboundFeeSetup = @InboundFeeSetup,
                        StorageFeeSetup = @StorageFeeSetup,
                        OutboundFeeSetup = @OutboundFeeSetup,
                        BinCreation = @BinCreation,
                        CustomerCreation = @CustomerCreation,
                        UserCreation = @UserCreation,
                        TransporterCreation = @TransporterCreation,
                        CustomerMaintenance = @CustomerMaintenance,
                        UserMaintenance = @UserMaintenance,
                        TransporterMaintenance = @TransporterMaintenance,
                        HUTrackingReportsDetails = @HUTrackingReportsDetails,
                        HUTrackingReportsSummary = @HUTrackingReportsSummary,
 BinMaintain = @BinMaintain,
DeliveryNote = @DeliveryNote,
DetailMonth = @DetailMonth,
SummaryMonth = @SummaryMonth,
InboundReport = @InboundReport,
BinnedReport = @BinnedReport,
PickedReport = @PickedReport,
OutboundReport = @OutboundReport,
Supervisor = @Supervisor,
Audit = @Audit,
MonthEndSetup = @MonthEndSetup,
UpdateMonthEndPwd = @UpdateMonthEndPwd,
StockOnHand = @StockOnHand,
ReDeliveryNote=@ReDeliveryNote,
UpdateContainer=@UpdateContainer,
GDNReturn=@GDNReturn,
GDNReturnReport=@GDNReturnReport,GRNReturnReport=@GRNReturnReport,PickupReturnReport=@PickupReturnReport,
ReturnReason=@ReturnReason,
ReasonMaintain=@ReasonMaintain,
BarcodeRePrint=@BarcodeRePrint ,DispatchedReport=@DispatchedReport
                    WHERE Username = @Username";
                }
                else
                {
                    // Insert new record
                    query = @"
                    INSERT INTO UserRoles (MailNotification,StockTakeReprint,StockPwd,ReverseWarehouseReport,ReverseWarehouse,DispatchedReport,PickupReturnReport,GRNReturnReport,GRNReturn,PickupReturn,BarcodeRePrint,ReturnReason,ReasonMaintain,StockRoles,MonthEndSetup,UpdateMonthEndPwd,GDNReturn,GDNReturnReport,
                        Username, InboundProcess, WarehouseProcess, PickingProcess, OutboundProcess, 
                        InboundException, WarehousedException, PickedException, BinToBin, 
                        ContainerAdjustment, ChangeContainerToAnotherCustomer, InboundFeeSetup, 
                        StorageFeeSetup, OutboundFeeSetup, BinCreation, CustomerCreation, 
                        UserCreation, TransporterCreation, CustomerMaintenance, UserMaintenance, 
                        TransporterMaintenance, HUTrackingReportsDetails, HUTrackingReportsSummary,BinMaintain,DeliveryNote,DetailMonth,SummaryMonth,InboundReport,
BinnedReport,PickedReport,OutboundReport,Audit,Supervisor,ReDeliveryNote,StockOnHand,UpdateContainer)
                    VALUES (@MailNotification,@StockTakeReprint,@StockPwd,@ReverseWarehouseReport,@ReverseWarehouse,@DispatchedReport,@PickupReturnReport,@GRNReturnReport,@GRNReturn,@PickupReturn,@BarcodeRePrint,@ReturnReason,@ReasonMaintain,@StockRoles,@MonthEndSetup,@UpdateMonthEndPwd,@GDNReturn,@GDNReturnReport,
                        @Username, @InboundProcess, @WarehouseProcess, @PickingProcess, @OutboundProcess, 
                        @InboundException, @WarehousedException, @PickedException, @BinToBin, 
                        @ContainerAdjustment, @ChangeContainerToAnotherCustomer, @InboundFeeSetup, 
                        @StorageFeeSetup, @OutboundFeeSetup, @BinCreation, @CustomerCreation, 
                        @UserCreation, @TransporterCreation, @CustomerMaintenance, @UserMaintenance, 
                        @TransporterMaintenance, @HUTrackingReportsDetails, @HUTrackingReportsSummary,@BinMaintain,@DeliveryNote,@DetailMonth,@SummaryMonth,@InboundReport,
@BinnedReport,@PickedReport,@OutboundReport,@Audit,@Supervisor,@ReDeliveryNote,@StockOnHand,@UpdateContainer)";
                }

                SqlCommand cmd = new SqlCommand(query, conn);

                // Add parameters to command
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@StockRoles", stockRoles);
                cmd.Parameters.AddWithValue("@InboundProcess", inboundProcess);
                cmd.Parameters.AddWithValue("@WarehouseProcess", warehouseProcess);
                cmd.Parameters.AddWithValue("@PickingProcess", pickingProcess);
                cmd.Parameters.AddWithValue("@OutboundProcess", outboundProcess);
                cmd.Parameters.AddWithValue("@InboundException", inboundException);
                cmd.Parameters.AddWithValue("@WarehousedException", warehousedException);
                cmd.Parameters.AddWithValue("@PickedException", pickedException);
                cmd.Parameters.AddWithValue("@BinToBin", binToBin);
                cmd.Parameters.AddWithValue("@ContainerAdjustment", containerAdjustment);
                cmd.Parameters.AddWithValue("@ChangeContainerToAnotherCustomer", changeContainerToAnotherCustomer);
                cmd.Parameters.AddWithValue("@InboundFeeSetup", inboundFeeSetup);
                cmd.Parameters.AddWithValue("@StorageFeeSetup", storageFeeSetup);
                cmd.Parameters.AddWithValue("@OutboundFeeSetup", outboundFeeSetup);
                cmd.Parameters.AddWithValue("@BinCreation", binCreation);
                cmd.Parameters.AddWithValue("@CustomerCreation", customerCreation);
                cmd.Parameters.AddWithValue("@UserCreation", userCreation);
                cmd.Parameters.AddWithValue("@TransporterCreation", transporterCreation);
                cmd.Parameters.AddWithValue("@CustomerMaintenance", customerMaintenance);
                cmd.Parameters.AddWithValue("@UserMaintenance", userMaintenance);
                cmd.Parameters.AddWithValue("@TransporterMaintenance", transporterMaintenance);
                cmd.Parameters.AddWithValue("@HUTrackingReportsDetails", huTrackingReportsDetails);
                cmd.Parameters.AddWithValue("@HUTrackingReportsSummary", huTrackingReportsSummary);
                cmd.Parameters.AddWithValue("@BinMaintain", binMaintain);
                cmd.Parameters.AddWithValue("@DeliveryNote", deliveryNote);
                cmd.Parameters.AddWithValue("@DetailMonth", detailMonth);
                cmd.Parameters.AddWithValue("@SummaryMonth", summaryMonth);
                cmd.Parameters.AddWithValue("@InboundReport", inboundReport);
                cmd.Parameters.AddWithValue("@BinnedReport", binnedReport);
                cmd.Parameters.AddWithValue("@PickedReport", pickedReport);
                cmd.Parameters.AddWithValue("@OutboundReport", outboundReport);
                cmd.Parameters.AddWithValue("@Audit", audit);
                cmd.Parameters.AddWithValue("@Supervisor", supervisor);
                cmd.Parameters.AddWithValue("@ReDeliveryNote", reDeliveryNote);
                cmd.Parameters.AddWithValue("@MonthEndSetup", monthEnd);
                cmd.Parameters.AddWithValue("@UpdateMonthEndPwd", monthEndPwd);
                cmd.Parameters.AddWithValue("@StockOnHand", stockOnHand);
                cmd.Parameters.AddWithValue("@UpdateContainer", updateContainer);
                cmd.Parameters.AddWithValue("@GDNReturn", GDNReturn);
                cmd.Parameters.AddWithValue("@GRNReturn", GRNReturn);
                cmd.Parameters.AddWithValue("@PickupReturn", PickupReturn);
                cmd.Parameters.AddWithValue("@GDNReturnReport", GDNReturnRpt);
                cmd.Parameters.AddWithValue("@ReverseWarehouse", binReturn);
                cmd.Parameters.AddWithValue("@ReverseWarehouseReport", binReturnRpt);
                cmd.Parameters.AddWithValue("@GRNReturnReport", GRNReturnRpt);
                cmd.Parameters.AddWithValue("@PickupReturnReport", PickupReturnRpt);
                cmd.Parameters.AddWithValue("@DispatchedReport", DispatchedRpt);
                cmd.Parameters.AddWithValue("@ReturnReason", returnReason);
                cmd.Parameters.AddWithValue("@ReasonMaintain", reasonMaintain);
                cmd.Parameters.AddWithValue("@BarcodeRePrint", barcodeRePrint);
                cmd.Parameters.AddWithValue("@StockTakeReprint", stockTakeReprint);
                cmd.Parameters.AddWithValue("@StockPwd", stockPwd);
                cmd.Parameters.AddWithValue("@MailNotification", mailNotification);
                cmd.ExecuteNonQuery();
                string script = "alert('Roles Assigned Successfully');" +
                 "window.setTimeout(function(){ window.location.href = 'UserMaintenance.aspx'; }, 1000);"; // 1-second delay before redirect
                ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);

            }
        }
        private void LoadData()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(constr))
            {
                string query = @"
        SELECT 
MailNotification,
StockTakeReprint,StockPwd,
            InboundProcess,
            WarehouseProcess,
            PickingProcess,
            OutboundProcess,
            InboundException,
            WarehousedException,
            PickedException,
            BinToBin,
            ContainerAdjustment,
            ChangeContainerToAnotherCustomer,
            InboundFeeSetup,
            StorageFeeSetup,
            OutboundFeeSetup,
            BinCreation,
            CustomerCreation,
            UserCreation,
            TransporterCreation,
            CustomerMaintenance,
            UserMaintenance,
            TransporterMaintenance,
            HUTrackingReportsDetails,
            HUTrackingReportsSummary,
BinMaintain,PickupReturnReport,GDNReturn,GDNReturnReport,
DeliveryNote,DetailMonth,SummaryMonth,InboundReport,
BinnedReport,PickedReport,OutboundReport,Audit,Supervisor,ReDeliveryNote,MonthEndSetup,UpdateMonthEndPwd,StockOnHand,UpdateContainer,StockRoles,ReturnReason,
ReasonMaintain,BarcodeRePrint,GRNReturn,PickupReturn,GRNReturnReport ,DispatchedReport ,ReverseWarehouse,ReverseWarehouseReport 
        FROM UserRoles
        WHERE Username = @Username";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", txtUserName.Text);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable userValuesTable = new DataTable();

                conn.Open();
                adapter.Fill(userValuesTable);

                if (userValuesTable.Rows.Count > 0)
                {
                    DataRow userRow = userValuesTable.Rows[0];
                    CheckBoxInboundProcess.Checked = Convert.ToBoolean(userRow["InboundProcess"]);
                    CheckBoxWarehouseProcess.Checked = Convert.ToBoolean(userRow["WarehouseProcess"]);
                    CheckBoxPickingProcess.Checked = Convert.ToBoolean(userRow["PickingProcess"]);
                    CheckBoxOutboundProcess.Checked = Convert.ToBoolean(userRow["OutboundProcess"]);
                    CheckBoxInboundException.Checked = Convert.ToBoolean(userRow["InboundException"]);
                    CheckBoxWarehousedException.Checked = Convert.ToBoolean(userRow["WarehousedException"]);
                    CheckBoxPickedException.Checked = Convert.ToBoolean(userRow["PickedException"]);
                    CheckBoxBinToBin.Checked = Convert.ToBoolean(userRow["BinToBin"]);
                    CheckBoxContainerAdjustment.Checked = Convert.ToBoolean(userRow["ContainerAdjustment"]);
                    CheckBoxChangeContainerToAnotherCustomer.Checked = Convert.ToBoolean(userRow["ChangeContainerToAnotherCustomer"]);
                    CheckBoxInboundFeeSetup.Checked = Convert.ToBoolean(userRow["InboundFeeSetup"]);
                    CheckBoxStorageFeeSetup.Checked = Convert.ToBoolean(userRow["StorageFeeSetup"]);
                    CheckBoxOutboundFeeSetup.Checked = Convert.ToBoolean(userRow["OutboundFeeSetup"]);
                    CheckBoxBinCreation.Checked = Convert.ToBoolean(userRow["BinCreation"]);
                    CheckBoxCustomerCreation.Checked = Convert.ToBoolean(userRow["CustomerCreation"]);
                    CheckBoxUserCreation.Checked = Convert.ToBoolean(userRow["UserCreation"]);
                    CheckBoxTransporterCreation.Checked = Convert.ToBoolean(userRow["TransporterCreation"]);
                    CheckBoxCustomerMaintenance.Checked = Convert.ToBoolean(userRow["CustomerMaintenance"]);
                    CheckBoxUserMaintenance.Checked = Convert.ToBoolean(userRow["UserMaintenance"]);
                    CheckBoxTransporterMaintenance.Checked = Convert.ToBoolean(userRow["TransporterMaintenance"]);
                    CheckBoxHUTrackingReportsDetails.Checked = Convert.ToBoolean(userRow["HUTrackingReportsDetails"]);
                    CheckBoxHUTrackingReportsSummary.Checked = Convert.ToBoolean(userRow["HUTrackingReportsSummary"]);
                    if(userRow["BinMaintain"] != DBNull.Value)
                    chkBinMaintain.Checked = Convert.ToBoolean(userRow["BinMaintain"]);
                    if (userRow["OutboundReport"] != DBNull.Value)
                        chkOutboundReport.Checked = Convert.ToBoolean(userRow["OutboundReport"]);
                    if (userRow["DetailMonth"] != DBNull.Value)
                        chkDetailMonth.Checked = Convert.ToBoolean(userRow["DetailMonth"]);
                    if (userRow["SummaryMonth"] != DBNull.Value)
                        chkSummaryMonth.Checked = Convert.ToBoolean(userRow["SummaryMonth"]);
                    if (userRow["InboundReport"] != DBNull.Value)
                        chkInboundReport.Checked = Convert.ToBoolean(userRow["InboundReport"]);
                    if (userRow["BinnedReport"] != DBNull.Value)
                        chkBinnedReport.Checked = Convert.ToBoolean(userRow["BinnedReport"]);
                    if (userRow["PickedReport"] != DBNull.Value)
                        chkPickedReport.Checked = Convert.ToBoolean(userRow["PickedReport"]);
                    if (userRow["DeliveryNote"] != DBNull.Value)
                        chkDelivery.Checked = Convert.ToBoolean(userRow["DeliveryNote"]);
                    if (userRow["Audit"] != DBNull.Value)
                        chkAudit.Checked = Convert.ToBoolean(userRow["Audit"]);
                    if (userRow["Supervisor"] != DBNull.Value)
                        chkSup.Checked = Convert.ToBoolean(userRow["Supervisor"]);
                    if (userRow["ReDeliveryNote"] != DBNull.Value)
                        chkReDN.Checked = Convert.ToBoolean(userRow["ReDeliveryNote"]);
                    if (userRow["MonthEndSetup"] != DBNull.Value)
                        chkMonthEnd.Checked = Convert.ToBoolean(userRow["MonthEndSetup"]);
                    if (userRow["UpdateMonthEndPwd"] != DBNull.Value)
                        chkMonthEndPwd.Checked = Convert.ToBoolean(userRow["UpdateMonthEndPwd"]);
                    if (userRow["StockOnHand"] != DBNull.Value)
                        chkStockOnHand.Checked = Convert.ToBoolean(userRow["StockOnHand"]);
                    if (userRow["ReturnReason"] != DBNull.Value)
                        chkReasonCreate.Checked = Convert.ToBoolean(userRow["ReturnReason"]);
                    if (userRow["ReasonMaintain"] != DBNull.Value)
                        chkReturnMaintain.Checked = Convert.ToBoolean(userRow["ReasonMaintain"]);
                    if (userRow["BarcodeReprint"] != DBNull.Value)
                        chkBarcodeReprint.Checked = Convert.ToBoolean(userRow["BarcodeReprint"]);
                    if (userRow["GRNReturn"] != DBNull.Value)
                        chkGRNReturn.Checked = Convert.ToBoolean(userRow["GRNReturn"]);
                    if (userRow["GRNReturnReport"] != DBNull.Value)
                        chkGRNReturnReport.Checked = Convert.ToBoolean(userRow["GRNReturnReport"]);
                    if (userRow["ReverseWarehouse"] != DBNull.Value)
                        chkBinReturn.Checked = Convert.ToBoolean(userRow["ReverseWarehouse"]);
                    if (userRow["ReverseWarehouseReport"] != DBNull.Value)
                        chkBinReturnReport.Checked = Convert.ToBoolean(userRow["ReverseWarehouseReport"]);
                    if (userRow["GDNReturn"] != DBNull.Value)
                        chkGDNReturn.Checked = Convert.ToBoolean(userRow["GDNReturn"]);
                    if (userRow["GDNReturnReport"] != DBNull.Value)
                        chkGDNReturnReport.Checked = Convert.ToBoolean(userRow["GDNReturnReport"]);
                    if (userRow["PickupReturnReport"] != DBNull.Value)
                        chkPickReturnReport.Checked = Convert.ToBoolean(userRow["PickupReturnReport"]);
                    if (userRow["DispatchedReport"] != DBNull.Value)
                        chkDispatchedReport.Checked = Convert.ToBoolean(userRow["DispatchedReport"]);
                    if (userRow["PickupReturn"] != DBNull.Value)
                        chkPickReturn.Checked = Convert.ToBoolean(userRow["PickupReturn"]);
                    if (userRow["StockTakeReprint"] != DBNull.Value)
                        chkStockTakeReprint.Checked = Convert.ToBoolean(userRow["StockTakeReprint"]);
                    if (userRow["StockPwd"] != DBNull.Value)
                        chkStockPwd.Checked = Convert.ToBoolean(userRow["StockPwd"]);
                    if (userRow["MailNotification"] != DBNull.Value)
                        chkMailNotification.Checked = Convert.ToBoolean(userRow["MailNotification"]);
                    if (userRow["StockRoles"] != DBNull.Value)
                    {
                        if(Convert.ToInt32(userRow["StockRoles"])==0)
                        {
                            radCounter.Checked = true;
                        }
                        else if (Convert.ToInt32(userRow["StockRoles"]) == 1)
                        {
                            radManager.Checked = true;
                        }
                        else if (Convert.ToInt32(userRow["StockRoles"]) == 2)
                        {
                            radAdmin.Checked = true;
                        }
                    }

                }
            }
        }

        protected void chkProcessAll_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = chkProcessAll.Checked;

            // Set the state of all checkboxes based on the "Select All" checkbox
            CheckBoxInboundProcess.Checked = isChecked;
            CheckBoxWarehouseProcess.Checked = isChecked;
            CheckBoxPickingProcess.Checked = isChecked;
            CheckBoxOutboundProcess.Checked = isChecked;
        }

        protected void chkExceptionAll_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = chkExceptionAll.Checked;

            // Apply the state to all other checkboxes in the "Exception Reports" section
            CheckBoxInboundException.Checked = isChecked;
            CheckBoxWarehousedException.Checked = isChecked;
            CheckBoxPickedException.Checked = isChecked;
        }

        protected void chkAdjustAll_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = chkAdjustAll.Checked;

            // Apply the state to all other checkboxes in the "Adjustment Forms" section
            CheckBoxBinToBin.Checked = isChecked;
            CheckBoxContainerAdjustment.Checked = isChecked;
            CheckBoxChangeContainerToAnotherCustomer.Checked = isChecked;
            chkUpdateContainer.Checked = isChecked;
        }

        protected void chkFeeAll_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = chkFeeAll.Checked;

            // Apply the state to all other checkboxes in the "Fee Setup Forms" section
            CheckBoxInboundFeeSetup.Checked = isChecked;
            CheckBoxStorageFeeSetup.Checked = isChecked;
            CheckBoxOutboundFeeSetup.Checked = isChecked;
        }

        protected void chkCreationAll_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = chkCreationAll.Checked;

            // Apply the state to all other checkboxes in the "Creation Forms" section
            CheckBoxBinCreation.Checked = isChecked;
            CheckBoxCustomerCreation.Checked = isChecked;
            CheckBoxUserCreation.Checked = isChecked;
            CheckBoxTransporterCreation.Checked = isChecked;
            chkReasonCreate.Checked = isChecked;
        }

        protected void chkMaintainAll_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = chkMaintainAll.Checked;

            // Apply the state to all other checkboxes in the "Maintenance Forms" section
            CheckBoxCustomerMaintenance.Checked = isChecked;
            CheckBoxUserMaintenance.Checked = isChecked;
            CheckBoxTransporterMaintenance.Checked = isChecked;
            chkBinMaintain.Checked = isChecked;
            chkGDNReturn.Checked = isChecked;
            chkReturnMaintain.Checked = isChecked;
            chkMaintainAll.Focus();
        }

        protected void chkReportAll_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = chkReportAll.Checked;

            // Apply the state to all other checkboxes in the "Reports" section
            CheckBoxHUTrackingReportsDetails.Checked = isChecked;
            CheckBoxHUTrackingReportsSummary.Checked = isChecked;
            chkGDNReturnReport.Checked = isChecked;
            chkGRNReturnReport.Checked = isChecked;
            chkBinReturnReport.Checked = isChecked;
            chkPickReturnReport.Checked = isChecked;
            chkDetailMonth.Checked = isChecked;
            chkSummaryMonth.Checked = isChecked;
            chkInboundReport.Checked = isChecked;
            chkBinnedReport.Checked = isChecked;
            chkPickedReport.Checked = isChecked;
            chkOutboundReport.Checked = isChecked;
            chkStockOnHand.Checked = isChecked;
            chkStockTakeReprint.Checked = isChecked;
            chkReportAll.Focus();
        }

        protected void chkAuditAll_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = chkAuditAll.Checked;

            // Apply the state to all other checkboxes in the "Reports" section
            chkAudit.Checked = isChecked;
        }

        protected void chkSetupAll_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = chkSetupAll.Checked;

            // Apply the state to all other checkboxes in the "Reports" section
            chkSup.Checked = isChecked;
            chkMonthEnd.Checked = isChecked;
            chkMonthEndPwd.Checked = isChecked;
            chkStockPwd.Checked = isChecked;
        }

        protected void chkDeliveryAll_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = chkDeliveryAll.Checked;
            chkDelivery.Checked = isChecked;
            chkReDN.Checked = isChecked;
            chkBarcodeReprint.Checked = isChecked;
        }

        protected void chkRoles_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void chkReturn_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = chkReturn.Checked;
            chkGRNReturn .Checked = isChecked;
            chkBinReturn.Checked = isChecked;
            chkGDNReturn.Checked = isChecked;
            chkPickReturn.Checked = isChecked;
        }
    }
}