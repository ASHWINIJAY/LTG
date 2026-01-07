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
    public partial class BarcodeRePrint : System.Web.UI.Page
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

        protected void btnComplete_Click(object sender, EventArgs e)
        {
            popup.Visible = false;

            if (checkBin())
            {

            }
            else
            {
                string script = "alert(\"GRN is not available\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                txtBin.Text = "";
                return;
            }
            var id = hdnBin.Value;
            CallExecutable(id.ToString());
            string relativeUrl = "Delivery.aspx?source=2&id=" + id;

            // Register the JavaScript to open a new window with the relative URL
            string script1 = $"openNewWindow('{ResolveUrl(relativeUrl)}');";
            ClientScript.RegisterStartupScript(this.GetType(), "openNewWindowScript", script1, true);
        }
        public void CallExecutable(string id)
        {
            // Specify the path to the executable
            //string exePath = @"C:\DEVELOPMENT\LTGBarcode\bin\Debug\LTGBarcode.exe";
            string exePath = @"C:\LTG\LTGBarcode\LTGBarcode.exe";
            //string exePath = @"C:\LTG\LTGLiveReports\LTGReports.exe";

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
        private bool checkBin()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select GRN from Inbound where GRN='" + txtBin.Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        hdnBin.Value = dt.Rows[0][0].ToString();
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
                string qry = " select distinct top 1000  GRN,CustomerCode,DateTimeofScan as CreatedDate FROM Inbound where CustomerCode like 'ALP%' or CustomerCode like 'AUT%' ";
                if (txtSearchContainer.Text != "")
                    qry += " and GRN like '%" + txtSearchContainer.Text + "%'";

                qry += " order by DateTimeofScan desc";

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
        protected void txtSearchContainer_TextChanged(object sender, EventArgs e)
        {
            FillGrid();
        }

        protected void imgSearch_Click(object sender, ImageClickEventArgs e)
        {
            FillGrid();
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