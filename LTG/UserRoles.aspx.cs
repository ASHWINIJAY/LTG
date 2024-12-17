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
                    SET 
                        InboundProcess = @InboundProcess,
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
ReDeliveryNote=@ReDeliveryNote
                    WHERE Username = @Username";
                }
                else
                {
                    // Insert new record
                    query = @"
                    INSERT INTO UserRoles (
                        Username, InboundProcess, WarehouseProcess, PickingProcess, OutboundProcess, 
                        InboundException, WarehousedException, PickedException, BinToBin, 
                        ContainerAdjustment, ChangeContainerToAnotherCustomer, InboundFeeSetup, 
                        StorageFeeSetup, OutboundFeeSetup, BinCreation, CustomerCreation, 
                        UserCreation, TransporterCreation, CustomerMaintenance, UserMaintenance, 
                        TransporterMaintenance, HUTrackingReportsDetails, HUTrackingReportsSummary,BinMaintain,DeliveryNote,DetailMonth,SummaryMonth,InboundReport,
BinnedReport,PickedReport,OutboundReport,Audit,Supervisor,ReDeliveryNote)
                    VALUES (
                        @Username, @InboundProcess, @WarehouseProcess, @PickingProcess, @OutboundProcess, 
                        @InboundException, @WarehousedException, @PickedException, @BinToBin, 
                        @ContainerAdjustment, @ChangeContainerToAnotherCustomer, @InboundFeeSetup, 
                        @StorageFeeSetup, @OutboundFeeSetup, @BinCreation, @CustomerCreation, 
                        @UserCreation, @TransporterCreation, @CustomerMaintenance, @UserMaintenance, 
                        @TransporterMaintenance, @HUTrackingReportsDetails, @HUTrackingReportsSummary,@BinMaintain,@DeliveryNote,@DetailMonth,@SummaryMonth,@InboundReport,
@BinnedReport,@PickedReport,@OutboundReport,@Audit,@Supervisor,@ReDeliveryNote)";
                }

                SqlCommand cmd = new SqlCommand(query, conn);

                // Add parameters to command
                cmd.Parameters.AddWithValue("@Username", username);
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
BinMaintain,
DeliveryNote,DetailMonth,SummaryMonth,InboundReport,
BinnedReport,PickedReport,OutboundReport,Audit,Supervisor,ReDeliveryNote
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
        }

        protected void chkMaintainAll_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = chkMaintainAll.Checked;

            // Apply the state to all other checkboxes in the "Maintenance Forms" section
            CheckBoxCustomerMaintenance.Checked = isChecked;
            CheckBoxUserMaintenance.Checked = isChecked;
            CheckBoxTransporterMaintenance.Checked = isChecked;
            chkBinMaintain.Checked = isChecked;
            chkMaintainAll.Focus();
        }

        protected void chkReportAll_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = chkReportAll.Checked;

            // Apply the state to all other checkboxes in the "Reports" section
            CheckBoxHUTrackingReportsDetails.Checked = isChecked;
            CheckBoxHUTrackingReportsSummary.Checked = isChecked;
           
            chkDetailMonth.Checked = isChecked;
            chkSummaryMonth.Checked = isChecked;
            chkInboundReport.Checked = isChecked;
            chkBinnedReport.Checked = isChecked;
            chkPickedReport.Checked = isChecked;
            chkOutboundReport.Checked = isChecked;
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
        }

        protected void chkDeliveryAll_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = chkDeliveryAll.Checked;
            chkDelivery.Checked = isChecked;
            chkReDN.Checked = isChecked;
        }
    }
}