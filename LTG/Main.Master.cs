using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace LTG
{
    public partial class Main : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                // setSession();
                if (getLoginType() == null)
                    Response.Redirect("Login");
                checkRoles();
                if (lblRole.Text == "Admin")
                {
                    LiAdmin.Visible = true;
                    LiUserMaintenance.Visible = true;
                }
               
            }

        }
        private void checkRoles()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;
            string query = "SELECT * FROM UserRoles WHERE Username = @username"; // Adjust query as needed

            using (SqlConnection connection = new SqlConnection(constr))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", hdnUserName.Value);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable userValuesTable = new DataTable();

                connection.Open();
                adapter.Fill(userValuesTable);
                var liElements = new Dictionary<string, string>
            {
                { "LiInboundProcess", "InboundProcess" },
                { "LiTransportProcess", "TransportProcess" },
                { "LiDeliveryProcess", "DeliveryProcess" },
                { "LiWarehouseProcess", "WarehouseProcess" },
                { "LiPickingProcess", "PickingProcess" },
                { "LiOutboundProcess", "OutboundProcess" },
                { "LiBinToBin", "BinToBin" },
                { "LiInvoiceReport", "HUTrackingReportsDetails" },
                { "LiInvoiceSummaryReport", "HUTrackingReportsSummary" },
                { "LiInboundException", "InboundException" },
                { "LiWarehousedException", "WarehousedException" },
                { "LiPickedException", "PickedException" },
                { "LiCustomerCreation", "CustomerCreation" },
                { "LiCustomerMaintenance", "CustomerMaintenance" },
                { "LiInboundFeeSetup", "InboundFeeSetup" },
                 { "LiTransportFeeSetup", "TransportFeeSetup" },
                { "LiDeliveryFeeSetup", "DeliveryFeeSetup" },
                { "LiOutboundFeeSetup", "OutboundFeeSetup" },
                { "LiStorageFeeSetup", "StorageFeeSetup" },
                { "LiBinCreation", "BinCreation" },
                { "LiTransportCreation", "TransporterCreation" },
                { "LiTransportMaintenance", "TransporterMaintenance" },
                { "LiUserCreation", "UserCreation" },
                { "LiUserMaintenance", "UserMaintenance" },
                { "LiContainerAdjustment", "ContainerAdjustment" },
                { "LiChangeContainerToAnotherCustomer", "ChangeContainerToAnotherCustomer" },
                { "LiBinMaintain", "BinMaintain" },
                { "LiDeliveryNoteReprint", "DeliveryNote" },
                { "LiDetailedReport", "DetailMonth" },
                { "LiSummaryReport", "SummaryMonth" },
                { "LiInboundReport", "InboundReport" },
                { "LiTransportReport", "TransportReport" },
                { "LiDeliveryReport", "TransportReport" },
                { "LiBinnedReport", "BinnedReport" },
                { "LiPickedReport", "PickedReport" },
                { "LiOutboundReport", "OutboundReport" },
                { "LiAudit", "Audit" },
                { "LiSupervisor", "Supervisor" },
                { "LiMonthEndSetup", "MonthEndSetup" },
                { "LiUpdateMonthEndPwd", "UpdateMonthEndPwd" },
                { "LiReGenerateDN", "ReDeliveryNote" },
                { "LiStockOnHandReport", "StockOnHand" },
                { "LiUpdateContainer", "UpdateContainer" },
                { "LiGDNReturn", "GDNReturn" },
                { "LiGRNReturn", "GRNReturn" },
                { "LiPickupReturn", "PickupReturn" },
                { "LiGDNReturnReport", "GDNReturnReport" },
                 { "LiGRNReturnReport", "GRNReturnReport" },
                 { "LiWarehouseReturnReport", "ReverseWarehouseReport" },
                 { "LiWarehouseReturn", "ReverseWarehouse" },
                 { "LiPickupReturnReport", "PickupReturnReport" },
                  { "LiDispatchedReport", "DispatchedReport" },
                { "LiReturnReason", "ReturnReason" },
                { "LiReasonMaintain", "ReasonMaintain" },
                { "LiStockTakeReprint", "StockTakeReprint" },
                { "LiMailNotification", "MailNotification" },
                { "LiStockPwd", "StockPwd" },
                { "LiBarcodeRePrint", "BarcodeRePrint" },
                { "LiUOPCreation", "UOPCreation" },
                { "LiUOPMaintain", "UOPMaintain" }

          
                // Add more mappings as needed
            };

                if (userValuesTable.Rows.Count > 0)
                {
                    DataRow userRow = userValuesTable.Rows[0];
                    // List of IDs corresponding to the <li> elements
                    if(userRow["StockRoles"] != DBNull.Value)
                    {
                        var roles = Convert.ToInt32(userRow["StockRoles"].ToString());
                        LiInitStock.Visible = false;
                        LiCounterCockPit.Visible = false;
                        LiCounter.Visible = false;
                        LiManagerCockPit.Visible = false;
                        LiManager.Visible = false;
                        LiStockConfirm.Visible = false;
                        LiStockAdj.Visible = false;
                        LiInitStockTakeAccuracy.Visible = false;
                        LiCancelStockTakeAccuracy.Visible = false;
                        LiBinCockPit.Visible = false;
                        LiBinStockTake.Visible = false;
                        LiManagerBinCockPit.Visible = false;
                        LiManagerBinStockTake.Visible = false;
                        LiStockAccVariance.Visible = false;
                        LiStockAccConfirm.Visible = false;
                        if (roles==0)
                        {
                           // LiCounterCockPit.Visible = true;
                            LiCounter.Visible = true;
                            LiBinStockTake.Visible = true;
                        }
                        else if (roles == 1)
                        {
                            //LiManagerCockPit.Visible = true;
                            LiManager.Visible = true;
                            LiManagerBinStockTake.Visible = true;
                        }
                        else if (roles == 2)
                        {
                            LiInitStockTakeAccuracy.Visible = true;
                            LiCancelStockTakeAccuracy.Visible = true;
                            LiBinCockPit.Visible = true;
                            LiManagerBinCockPit.Visible = true;
                            LiStockAccVariance.Visible = true;
                            LiStockAccConfirm.Visible = true;
                            LiInitStock.Visible = true;
                            LiCounterCockPit.Visible = true;
                            LiManagerCockPit.Visible = true;
                            LiStockConfirm.Visible = true;
                            LiStockAdj.Visible = true;

                        }
                    }
                        foreach (var li in liElements)
                    {
                        string liId = li.Key;
                        string columnName = li.Value;

                        // Check if the column exists and get the value
                        //int columnIndex = reader.GetOrdinal(columnName);
                        bool isVisible = false;
                        if (userRow[columnName] !=DBNull.Value)
                         isVisible = Convert.ToBoolean(userRow[columnName]); 

                        // Set the visibility of the <li> element
                        Control liControl = FindControl(liId);
                        if (liControl != null && liControl is HtmlGenericControl liElement)
                        {
                            liElement.Visible = isVisible;
                        }
                    }
                }
                else
                {
                    foreach (var li in liElements)
                    {
                        string liId = li.Key;
                        string columnName = li.Value;

                        // Check if the column exists and get the value
                        //int columnIndex = reader.GetOrdinal(columnName);
                        bool isVisible = false;

                        // Set the visibility of the <li> element
                        Control liControl = FindControl(liId);
                        if (liControl != null && liControl is HtmlGenericControl liElement)
                        {
                            liElement.Visible = isVisible;
                        }
                    }
                }
               // reader.Close();
            }
        }
        private string getLoginType()
        {
            HttpCookie LoginType = Request.Cookies["LoginId"];
            HttpCookie firstName = Request.Cookies["firstName"];
            HttpCookie loginRole = Request.Cookies["loginRole"];

            // Read the cookie information and display it.
            if (loginRole == null)
                return null;
            hdnLoginId.Value = LoginType.Value;
            Session["LoginId"] = LoginType.Value;
            LoginType = Request.Cookies["Username"];
            hdnUserName.Value = LoginType.Value;
            Session["Username"] = LoginType.Value;
            lblFirst.Text = firstName.Value;
            lblRole.Text = loginRole.Value;
            
            return "";
        }
    }
}