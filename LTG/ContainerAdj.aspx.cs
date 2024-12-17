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
    public partial class ContainerAdj : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindCustomer();
                // FillGrid();
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
                        ddlCustomer.Items.Insert(0, new ListItem("Select customer", ""));
                        ddlNewCustomer.DataSource = dt;
                        ddlNewCustomer.DataBind();
                        ddlNewCustomer.DataTextField = "CustomerName";
                        ddlNewCustomer.DataValueField = "CustomerCode";
                        ddlNewCustomer.DataBind();
                        ddlNewCustomer.Items.Insert(0, new ListItem("Select customer", ""));
                        // ddlBranch.da
                    }
                }
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
            if (ddlNewCustomer.SelectedIndex == 0)
            {
                string script = "alert(\"Please select customer\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                return;
            }
            if (checkBin())
            {

            }
            else
            {
                string script = "alert(\"Container is not available\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                txtHU.Text = "";
                return;
            }

            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "";
                qry = "Update Outbound set CustomerCode='" + ddlNewCustomer.SelectedValue + "',CustomerName='" + ddlNewCustomer.SelectedItem.Text + "' where HU in ( select HU from Inbound where ContainerId='" + txtHU.Text + "')";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                cmd1.ExecuteNonQuery();
                qry = "Update WarehouseProcess set CustomerCode='" + ddlNewCustomer.SelectedValue + "',CustomerName='" + ddlNewCustomer.SelectedItem.Text + "' where HU in ( select HU from Inbound where ContainerId='" + txtHU.Text + "')";
                 cmd1 = new SqlCommand(qry, con);
                cmd1.ExecuteNonQuery();
                qry = "Update Inbound set CustomerCode='" + ddlNewCustomer.SelectedValue + "',CustomerName='" + ddlNewCustomer.SelectedItem.Text + "' where ContainerId='" + txtHU.Text + "'";
                 cmd1 = new SqlCommand(qry, con);
                cmd1.ExecuteNonQuery();
                string script = "alert(\"Successfully Updated\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                txtHU.Text = "";
               // txtBin.Text = "";
                //txtNewBin.Text = "";
            }
        }
        private bool checkBin()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from Inbound where ContainerId='" + txtHU.Text + "'";
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
       
        protected void btnSave_Click(object sender, EventArgs e)
        {

        }
        protected void FillGrid()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "SELECT  distinct ContainerId  FROM Inbound where  CustomerCode='" + ddlCustomer.SelectedValue + "'";
                if (txtSearchContainer.Text != "")
                    qry += " and ContainerId like '%" + txtSearchContainer.Text + "%'";
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
        protected void txtSearchContainer_TextChanged(object sender, EventArgs e)
        {
            FillGrid();
        }

        protected void imgSearch_Click(object sender, ImageClickEventArgs e)
        {
            FillGrid();
        }
        protected void grdScans_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = grdScans.SelectedRow;
            txtHU.Text = row.Cells[1].Text;
           // txtBin.Text = row.Cells[1].Text;
            popup.Visible = false;
        }

        protected void txtHU_TextChanged(object sender, EventArgs e)
        {
            
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            popup.Visible = false;
        }
    }
}