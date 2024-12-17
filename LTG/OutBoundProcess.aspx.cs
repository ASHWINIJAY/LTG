using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace LTG
{
    public partial class OutBoundProcess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                bindBranch();
                bindCustomer();
                GenerateGRN();
                if (Request.QueryString["id"] != null)
                {
                    var id = Request.QueryString["id"].ToString();
                    BindIncomplete(id);
                }
                else
                    CheckIncomplete();
            }
        }
        protected void CheckIncomplete()
        {
            if (Session["LoginId"] == null)
            {
                Response.Redirect("Dashboard.aspx");
            }
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "SELECT  *  FROM Outbound where Completed=0 and Loginname='" + Session["LoginId"].ToString() + "' order by DateTimeofScan desc";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        string redirectUrl = "OutboundProcess.aspx?id=" + dt.Rows[0]["GDN"].ToString(); // The URL to open in a new tab

                        // Inject the JavaScript confirmation dialog with a redirect in a new tab on confirmation
                        string script = "var userConfirmed = confirm('You have existing GDN is incomplete, Do you want to continue?');" +
                                        "if (userConfirmed) {" +
                                        "    window.open('" + redirectUrl + "', '_blank');" +
                                        "}";
                        ClientScript.RegisterStartupScript(this.GetType(), "confirmRedirectNewTab", script, true);
                    }
                }
            }
        }

        protected void BindIncomplete(string GRN)
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;
            HiddenField hdLoginId = (HiddenField)this.Master.FindControl("hdnLoginId");
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "SELECT  *  FROM Outbound where Completed=0 and GDN='" + GRN + "' order by DateTimeofScan";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        ddlBranch.SelectedValue = dt.Rows[0]["BranchId"].ToString();
                        ddlCustomer.SelectedValue = dt.Rows[0]["CustomerCode"].ToString();
                        txtDate.Text = Convert.ToDateTime(dt.Rows[0]["DateTimeOfScan"]).ToString("dd/MMM/yyyy");
                        txtContainer.Text = dt.Rows[0]["ContainerId"].ToString();
                        txtGRN.Text = GRN;
                        getOutboundFee();
                        divCustomer.Visible = false;
                        divScan.Visible = true;
                        divHeader.InnerText = "Outbound Process for - " + ddlCustomer.SelectedItem.Text;
                        //txtContainer1.Text = txtContainer.Text;
                        txtHU.Focus();
                        FillGrid();
                    }
                }
            }
        }
        private void bindCustomer()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from Customers";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        ddlCustomer.DataSource = dt;
                        ddlCustomer.DataBind();
                        ddlCustomer.DataTextField = "CustomerName";
                        ddlCustomer.DataValueField = "CustomerCode";
                        ddlCustomer.DataBind();
                        // ddlBranch.da
                    }
                    ddlCustomer.Items.Insert(0, new ListItem("Select customer", ""));
                }
            }
        }
        private void bindBranch()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from Branch";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        ddlBranch.DataSource = dt;
                        ddlBranch.DataBind();
                        ddlBranch.DataTextField = "BranchName";
                        ddlBranch.DataValueField = "BranchId";
                        ddlBranch.DataBind();
                        // ddlBranch.da
                    }
                }
            }
        }
        private void bindTransport()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from Transport where BranchId=" + ddlBranch.SelectedValue + " and CustomerCode='" + ddlCustomer.SelectedValue + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        ddlTransport.DataSource = dt;
                        ddlTransport.DataBind();
                        ddlTransport.DataTextField = "TransportName";
                        ddlTransport.DataValueField = "TransportId";
                        ddlTransport.DataBind();
                      
                        // ddlBranch.da
                    }
                    ddlTransport.Items.Insert(0, new ListItem("Select transport", ""));
                }
            }
        }
        private void getOutboundFee()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select OutboundFee,Bin from Customers where CustomerCode='" + ddlCustomer.SelectedValue + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0] != DBNull.Value)
                            hdnInboundFee.Value = dt.Rows[0][0].ToString();
                        else
                            hdnInboundFee.Value = "";
                        
                    }
                }
            }
        }
        protected void btnCusNext_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlCustomer.SelectedIndex == 0)
            {
                string script = "alert(\"Please select customer\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                return;
            }
           
            
           
            getOutboundFee();
            if (hdnInboundFee.Value == "")
            {
                string script = "alert(\"Please setup the outbound fee. \");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);

                return;
            }
            divCustomer.Visible = false;
            divScan.Visible = true;
            divHeader.InnerText = "Outbound Process for - " + ddlCustomer.SelectedItem.Text;
            txtBin.Focus();
            FillGrid();
            bindCustomerAddr();
        }
        private void bindCustomerAddr()
        {
            try
            {
                string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();
                    string qry = "Select * from Customers where CustomerCode='" + ddlCustomer.SelectedValue + "'";
                    SqlCommand cmd1 = new SqlCommand(qry, con);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            txtAddr1.Text = dt.Rows[0]["DelAddr1"].ToString();
                            txtAddr2.Text = dt.Rows[0]["DelAddr2"].ToString();

                            txtAddr3.Text = dt.Rows[0]["DelAddr3"].ToString();
                            txtAddr4.Text = dt.Rows[0]["DelAddr4"].ToString();
                            // ddlBranch.da
                        }
                    }
                }
            }
            catch
            {

            }
        }
        private bool checkRefNo()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from Outbound where ContainerId='" + txtContainer.Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        return true;
                        // ddlBranch.da
                    }
                }
            }
            return false;
        }
        private bool checkBin()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from Bin where BinName='" + txtBin.Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        return true;
                        // ddlBranch.da
                    }
                }
            }
            return false;
        }
        private bool checkHU()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from PickingProcess where HU='" + txtHU.Text + "' and Status='PICKED' and CustomerCode='" + ddlCustomer.SelectedValue + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        return true;
                        // ddlBranch.da
                    }
                }
            }
            return false;
        }
        private int IsHUAvailable()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from WarehouseProcess where HU='" + txtHU.Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["QtyOnHand"].ToString() == "1")
                            return 0;
                        else
                            return 2;
                        // ddlBranch.da
                    }
                }
            }
            return 1;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            save();
            FillGrid();
        }
        private bool checkValidCustomerHU()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from Inbound where HU='" + txtHU.Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["CustomerCode"].ToString() == ddlCustomer.SelectedValue)
                            return true;
                        else
                            return false;
                        // ddlBranch.da
                    }
                }
            }
            return false;
        }

        private void save()
        {
            txtHU.Text = txtHU.Text.Trim();
            if (txtHU.Text == "")
            {
                string script = "alert(\"HU Cannot be empty\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                txtHU.Text = "";
                txtHU.Focus();
                return;
            }
            if (!checkValidCustomerHU())
            {
                string script = "alert(\"This HU is not linked to this customer and cannot be scanned\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                txtHU.Text = "";
                txtHU.Focus();
                return;
            }
            if (checkHU())
            {

            }
            else
            {
                string script = "alert(\"HU is not available\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                txtHU.Text = "";
                return;
            }
            var HUAvailability = IsHUAvailable();
            if (HUAvailability == 2)
            {
                string script = "alert(\"HU is already Outbounded.\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                txtHU.Text = "";
                txtHU.Focus();
                return;
            }
            if (HUAvailability == 1)
            {
                string script = "alert(\"HU is not available for the bin!\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                txtHU.Text = "";
                txtHU.Focus();
                return;
            }
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                HiddenField hdLoginId = (HiddenField)this.Master.FindControl("hdnLoginId");

                var userid = hdLoginId.Value;
                HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");

                var userName = hdUserName.Value;
                string qry = "Update WarehouseProcess set QtyOut=1,QtyOnHand=0,ScannedOutTime='" + txtDate.Text + "',ModifiedBy='" + userName + "',ModifiedDate=getdate() where HU='" + txtHU.Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                cmd1.ExecuteNonQuery();
                qry = "Insert into Outbound(GDN,ContainerId,BranchId,BranchName,CustomerCode,CustomerName,HU,Qty,UnitOutBoundCost,TotalOutBoundCost,Loginname,DateTimeofScan,CreatedBy,CreatedDate,BinName)values('" + txtGRN.Text + "','" + txtContainer.Text + "'," + ddlBranch.SelectedValue + ",'" + ddlBranch.SelectedItem.Text + "','" + ddlCustomer.SelectedValue + "','" + ddlCustomer.SelectedItem.Text + "','" + txtHU.Text + "','" + txtQty.Text + "'," + hdnInboundFee.Value + "," + hdnInboundFee.Value + ",'" + userid + "','" + txtDate.Text + "','" + userName + "',getdate(),'" + txtBin.Text + "')";
                cmd1 = new SqlCommand(qry, con);
                cmd1.ExecuteNonQuery();
                qry = "Update PickingProcess set Status='Outbounded' where HU='" + txtHU.Text + "'";
                cmd1 = new SqlCommand(qry, con);
                cmd1.ExecuteNonQuery();
                // qry = "Insert into WarehouseProcess(Bin,BranchId,BranchName,CustomerCode,CustomerName,HU,QtyIn,QtyOnHand,UnitStorageCost,TotalStorageCost,UserName,ScannedInTime,CreatedBy,CreatedDate)values('" + hdnBin.Value + "'," + ddlBranch.SelectedValue + ",'" + ddlBranch.SelectedItem.Text + "','" + ddlCustomer.SelectedValue + "','" + ddlCustomer.SelectedItem.Text + "','" + txtHU.Text + "','" + txtQty.Text + "','" + txtQty.Text + "'," + hdnInboundFee.Value + "," + hdnInboundFee.Value + ",'" + userid + "',getdate(),'" + userName + "',getdate())";
                // cmd1 = new SqlCommand(qry, con);
                //cmd1.ExecuteNonQuery();
            }
            txtHU.Text = "";
            txtHU.Focus();
        }
        protected void btnComplete_Click(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();

                string qry = "Update Outbound set Completed=1 where GDN='" + txtGRN.Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                cmd1.ExecuteNonQuery();
            }
            bindTransport();
            popup.Visible = true;
            txtHU.Text = " TestPrint";
           
           
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtHU.Text = "";
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if(ddlTransport.SelectedIndex==0)
            {
                string script = "alert(\"You must select a transporter\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                ddlTransport.Focus();
                return;
            }
            if (txtReg.Text =="")
            {
                string script = "alert(\"Please enter Regno\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                txtReg.Focus();
                return;
            }
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                HiddenField hdLoginId = (HiddenField)this.Master.FindControl("hdnLoginId");

                var userid = hdLoginId.Value;
                HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");

                var userName = hdUserName.Value;
                   SqlCommand cmd1;// = new SqlCommand(qry, con);
               
               string  qry = "Insert into DeliveryDetails(TransportName,TransportMode,RegNo,DelNote,Address,SpecialIns,CreatedBy,DelAddr2,DelAddr3,DelAddr4)output INSERTED.OutSlip values('" + ddlTransport.SelectedItem.Text + "','" + txtMode.Text + "','" + txtReg.Text + "','" + txtDelivery.Text + "','" + txtAddr1.Text + "','" + txtSpl.Text + "','" + userName + "','" + txtAddr2.Text + "','" + txtAddr3.Text + "','" + txtAddr4.Text + "')";
                cmd1 = new SqlCommand(qry, con);
                var id = cmd1.ExecuteScalar();
                qry = "update Outbound set OutSlip=" + id + " where GDN='" + txtGRN.Text + "'";
                cmd1 = new SqlCommand(qry, con);
                cmd1.ExecuteNonQuery();
                GenerateGRN();
                txtHU.Text = "";
                divCustomer.Visible = true;
                txtBin.Text = "";
                txtContainer.Text = "";
                divScan.Visible = false;
                popup.Visible = false;
                CallExecutable(id.ToString());
               
                string relativeUrl = "Delivery.aspx?id=" + id;

                // Register the JavaScript to open a new window with the relative URL
                string script = $"openNewWindow('{ResolveUrl(relativeUrl)}');";
                ClientScript.RegisterStartupScript(this.GetType(), "openNewWindowScript", script, true);

                // qry = "Insert into WarehouseProcess(Bin,BranchId,BranchName,CustomerCode,CustomerName,HU,QtyIn,QtyOnHand,UnitStorageCost,TotalStorageCost,UserName,ScannedInTime,CreatedBy,CreatedDate)values('" + hdnBin.Value + "'," + ddlBranch.SelectedValue + ",'" + ddlBranch.SelectedItem.Text + "','" + ddlCustomer.SelectedValue + "','" + ddlCustomer.SelectedItem.Text + "','" + txtHU.Text + "','" + txtQty.Text + "','" + txtQty.Text + "'," + hdnInboundFee.Value + "," + hdnInboundFee.Value + ",'" + userid + "',getdate(),'" + userName + "',getdate())";
                // cmd1 = new SqlCommand(qry, con);
                //cmd1.ExecuteNonQuery();
            }
        }
        public void CallExecutable(string id)
        {
            // Specify the path to the executable
           // string exePath = @"C:\DEVELOPMENT\LTGReports\bin\Debug\LTGReports.exe";
            string exePath = @"C:\LTG\LTGLiveReports\LTGReports.exe";

            // Specify any arguments if needed
            string arguments = id;

            // Create a new process start info object
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = arguments,
                RedirectStandardOutput = true,
                UseShellExecute = false, // Important: this prevents redirection of streams
                CreateNoWindow = true // Optional: hides the console window
            };

            try
            {
                using (Process process = Process.Start(startInfo))
                {
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string result = reader.ReadToEnd();
                        // return result;
                    }
                }

            }
            catch (Exception ex)
            {
                // Handle exception
                Response.Write("An error occurred: " + ex.Message);
            }
        }
        private void GenerateGRN()
        {
            string dtMonth = DateTime.Now.ToString("dd-MM-yy").Replace("-", "");
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;
            txtDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "select count(distinct(GDN))+1 FROM Outbound";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        string output = FormatNumberWithLeadingZeros(dt.Rows[0][0].ToString(), 9);
                        txtGRN.Text = "GDN-" + dtMonth + "-" + output;
                        // ddlBranch.da
                    }
                }
            }

        }
        static string FormatNumberWithLeadingZeros(string number, int totalLength)
        {
            return number.PadLeft(totalLength, '0');
        }

        protected void FillGrid()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "SELECT  *  FROM Outbound where GDN='" + txtGRN.Text + "' order by CreatedDate";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    grdScans.DataSource = dt;
                    grdScans.DataBind();
                    divHeader.InnerText = "OutBound Process - Total Scans: " + dt.Rows.Count;
                }
            }
        }

        protected void txtBin_TextChanged(object sender, EventArgs e)
        {
            save();
            FillGrid();
        }

        protected void txtBin_TextChanged1(object sender, EventArgs e)
        {
            txtHU.Focus();
        }

        protected void txtHU_TextChanged(object sender, EventArgs e)
        {
            save();
            FillGrid();
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();

                string qry = "Update WarehouseProcess set QtyOut=0,QtyOnHand=1,ScannedOutTime=null where HU in(select HU from Outbound where GDN='" + txtGRN.Text + "')";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                cmd1.ExecuteNonQuery();
                
                qry = "Update PickingProcess set Status='PICKED' where HU in(Select HU from Outbound where GDN='" + txtGRN.Text + "')";
                cmd1 = new SqlCommand(qry, con);
                cmd1.ExecuteNonQuery();
                qry = "Delete  from Outbound where GDN='" + txtGRN.Text + "'";
                cmd1 = new SqlCommand(qry, con);

                cmd1.ExecuteNonQuery();
                divScan.Visible = false;
                divCustomer.Visible = true;
                FillGrid();
                // ddlBranch.da

            }
        }

        protected void btnCancel_Click1(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Update WarehouseProcess set QtyOut=0,QtyOnHand=1,ScannedOutTime=null where HU in(select HU from Outbound where GDN='" + txtGRN.Text + "')";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                cmd1.ExecuteNonQuery();
                qry = "Update PickingProcess set Status='PICKED' where HU in(Select HU from Outbound where GDN='" + txtGRN.Text + "')";
                cmd1 = new SqlCommand(qry, con);
                cmd1.ExecuteNonQuery();
                qry = "Delete  from Outbound where GDN='" + txtGRN.Text + "'";
                cmd1 = new SqlCommand(qry, con);

                cmd1.ExecuteNonQuery();
                Response.Redirect("outboundProcess.aspx");
                // ddlBranch.da

            }
        }

        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
           // bindTransport();
        }

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtContainer.Focus();
        }

        protected void btnCreateNew_Click(object sender, EventArgs e)
        {

        }

        protected void imgRefresh_Click(object sender, ImageClickEventArgs e)
        {
            bindTransport();
        }

        protected void btnClse_Click(object sender, EventArgs e)
        {
            popup.Visible = false;
        }
        protected void btnClose_Click(object sender, EventArgs e)
        {
            popup1.Visible = false;
        }

        protected void btnSaveDate_Click(object sender, EventArgs e)
        {
            if (txtNewdate.Text == "")
            {
                string script = "alert(\"Please select date\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);

                return;
            }
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();

                // Check if the table has any records
                string checkCountQry = "SELECT COUNT(*) FROM Supervisor where Password='" + txtSupPassword.Text + "'";
                SqlCommand checkCmd = new SqlCommand(checkCountQry, con);
                int recordCount = (int)checkCmd.ExecuteScalar();


                if (recordCount == 0)
                {
                    string script = "alert(\"Password is wrong, Please reenter\");";
                    ScriptManager.RegisterStartupScript(this, GetType(),
                                          "ServerControlScript", script, true);
                    txtSupPassword.Text = "";
                    return;
                }
            }
            txtDate.Text = txtNewdate.Text;
            txtSupPassword.Text = "";
            txtNewdate.Text = "";
            popup1.Visible = false;
        }

        protected void btnCancelDate_Click(object sender, EventArgs e)
        {
            popup1.Visible = false;
        }

        protected void btnDateChange_Click(object sender, ImageClickEventArgs e)
        {

            txtNewdate.Attributes["max"] = DateTime.Now.ToString("yyyy-MM-dd");
            // return;
            popup1.Visible = true;
        }
    }
}