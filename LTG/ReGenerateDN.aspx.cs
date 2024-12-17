using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LTG
{
    public partial class ReGenerateDN : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // FillGrid();
            }
        }

        protected void btnBrowse_Click(object sender, EventArgs e)
        {
            FillGrid();
            popup.Visible = true;
        }
        private void bindCustomerAddr()
        {
            try
            {
                string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();
                    string qry = "Select * from Customers where CustomerCode='" + hdnCustomer.Value + "'";
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
        protected void txtSearchContainer_TextChanged(object sender, EventArgs e)
        {
            FillGrid();
        }

        protected void imgSearch_Click(object sender, ImageClickEventArgs e)
        {
            FillGrid();
        }
        protected void imgRefresh_Click(object sender, ImageClickEventArgs e)
        {
            bindTransport();
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (ddlTransport.SelectedIndex == 0)
            {
                string script = "alert(\"You must select a transporter\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                ddlTransport.Focus();
                return;
            }
            if (txtReg.Text == "")
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

                string qry = "Insert into DeliveryDetails(TransportName,TransportMode,RegNo,DelNote,Address,SpecialIns,CreatedBy,DelAddr2,DelAddr3,DelAddr4)output INSERTED.OutSlip values('" + ddlTransport.SelectedItem.Text + "','" + txtMode.Text + "','" + txtReg.Text + "','" + txtDelivery.Text + "','" + txtAddr1.Text + "','" + txtSpl.Text + "','" + userName + "','" + txtAddr2.Text + "','" + txtAddr3.Text + "','" + txtAddr4.Text + "')";
                cmd1 = new SqlCommand(qry, con);
                var id = cmd1.ExecuteScalar();
               
                qry = "update Outbound set OutSlip=" + id + " where GDN='" + txtBin.Text + "'";
                cmd1 = new SqlCommand(qry, con);
                cmd1.ExecuteNonQuery();
                
                txtBin.Text = "";
               
                //divScan.Visible = false;
                popup.Visible = false;
                divTransport.Visible = false;
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
        protected void btnComplete_Click(object sender, EventArgs e)
        {
            popup.Visible = false;

            if (checkBin())
            {
                divTransport.Visible = true;
                bindTransport();
                bindCustomerAddr();
            }
            else
            {
                string script = "alert(\"GDN is not available\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                txtBin.Text = "";
                return;
            }

            
        }
        private void bindTransport()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from Transport where BranchId=" + hdnBranch.Value + " and CustomerCode='" + hdnCustomer.Value + "'";
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
        protected void btnClse_Click(object sender, EventArgs e)
        {
            divTransport.Visible = false;
        }
        private bool checkBin()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select *  from Outbound where GDN='" + txtBin.Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        hdnBin.Value = txtBin.Text;
                        hdnBranch.Value = dt.Rows[0]["BranchId"].ToString();
                        hdnCustomer.Value = dt.Rows[0]["CustomerCode"].ToString();
                        return true;
                        // ddlBranch.da
                    }
                }
            }
            return false;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {

        }
        protected void FillGrid()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = " select distinct top 1000  GDN,CustomerCode,0 as OutSlip FROM Outbound as O ";
                if (txtSearchContainer.Text != "")
                    qry += " where GDN like '%" + txtSearchContainer.Text + "%'";

                qry += " order by GDN desc";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    grdScans.DataSource = dt;
                    grdScans.DataBind();
                }
            }
        }

        protected void grdScans_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = grdScans.SelectedRow;

            txtBin.Text = row.Cells[2].Text;
            popup.Visible = false;
        }

        protected void txtHU_TextChanged(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;


        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            popup.Visible = false;
        }
    }
}