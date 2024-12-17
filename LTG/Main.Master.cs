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
                { "LiBinnedReport", "BinnedReport" },
                { "LiPickedReport", "PickedReport" },
                { "LiOutboundReport", "OutboundReport" },
                { "LiAudit", "Audit" },
                { "LiSupervisor", "Supervisor" },
                { "LiReGenerateDN", "ReDeliveryNote" }
              

          
                // Add more mappings as needed
            };

                if (userValuesTable.Rows.Count > 0)
                {
                    DataRow userRow = userValuesTable.Rows[0];
                    // List of IDs corresponding to the <li> elements
                    
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
            lblFirst.Text = firstName.Value;
            lblRole.Text = loginRole.Value;
            
            return "";
        }
    }
}