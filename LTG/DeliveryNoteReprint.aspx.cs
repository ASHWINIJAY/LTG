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
    public partial class DeliveryNoteReprint : System.Web.UI.Page
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
                string script = "alert(\"GDN is not available\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                txtBin.Text = "";
                return;
            }

            string relativeUrl = "Delivery.aspx?source=1&id=" + hdnBin.Value;

            // Register the JavaScript to open a new window with the relative URL
            string script1 = $"openNewWindow('{ResolveUrl(relativeUrl)}');";
            ClientScript.RegisterStartupScript(this.GetType(), "openNewWindowScript", script1, true);
        }
        private bool checkBin()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select OutSlip from Outbound where GDN='" + txtBin.Text + "'";
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
                string qry = " select distinct top 1000  GDN,CustomerCode,D.OutSlip,D.CreatedDate FROM Outbound as O inner join DeliveryDetails as D on D.OutSlip=O.OutSlip ";
                if (txtSearchContainer.Text != "")
                    qry += " where GDN like '%" + txtSearchContainer.Text + "%'";

                qry += " order by D.CreatedDate desc";

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