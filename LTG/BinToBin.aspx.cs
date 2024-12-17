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
    public partial class BinToBin : System.Web.UI.Page
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
            var HUAvailability = IsHUAvailable();
            if (HUAvailability == 2)
            {
                string script = "alert(\"HU is already Outbounded.We can not transfer the Bin\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                txtHU.Text = "";
                return;
            }
            if (HUAvailability == 1)
            {
                string script = "alert(\"HU is not available for the bin!\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                txtHU.Text = "";
                return;
            }
            if (checkBin())
            {

            }
            else
            {
                string script = "alert(\"Bin is not available\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                txtNewBin.Text = "";
                return;
            }

            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Update WarehouseProcess set Bin='" + txtNewBin.Text + "' where QtyOnHand>0 and HU='" + txtHU.Text +"' and Bin='" + txtBin.Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                cmd1.ExecuteNonQuery();
                string script = "alert(\"Successfully Updated\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                txtHU.Text = "";
                txtBin.Text = "";
                txtNewBin.Text = "";
            }
        }
        private bool checkBin()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from Bin where BinName='" + txtNewBin.Text + "' and Isnull(IsActive,1)<>0";
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
                string qry = "Select * from WarehouseProcess where Bin='" + txtBin.Text + "' and HU='" + txtHU.Text + "'";
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

        }
        protected void FillGrid()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "SELECT  *  FROM WarehouseProcess where QtyOnHand>0 and Bin='" + txtBin.Text + "'";
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
            txtHU.Text = row.Cells[2].Text;
            txtBin.Text = row.Cells[1].Text;
            popup.Visible = false;
        }

        protected void txtHU_TextChanged(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "SELECT  *  FROM WarehouseProcess where HU='" + txtHU.Text + "' and QtyOnHand>0";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
if(dt.Rows.Count>0)
                    {
                        txtBin.Text = dt.Rows[0]["Bin"].ToString();
                    }
                    else
                    {
                        string script = "alert(\"HU is not available for the bin to bin transfer!\");";
                        ScriptManager.RegisterStartupScript(this, GetType(),
                                              "ServerControlScript", script, true);
                    }
                }
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            popup.Visible = false;
        }
    }
}