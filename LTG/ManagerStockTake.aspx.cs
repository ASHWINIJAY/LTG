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
    public partial class ManagerStockTake : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GenerateGRN();
                // bindCustomer();
                FillGrid();
            }
        }
        private void GenerateGRN()
        {
            string dtMonth = DateTime.Now.ToString("dd-MM-yy").Replace("-", "");
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "select count(distinct(MStockTakeNumber))+1 FROM StockTakeHistory";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        string output = FormatNumberWithLeadingZeros(dt.Rows[0][0].ToString(), 9);
                        txtGRN.Text = "MSTN-" + dtMonth + "-" + output;

                        // ddlBranch.da
                    }
                }
            }

        }
        static string FormatNumberWithLeadingZeros(string number, int totalLength)
        {
            return number.PadLeft(totalLength, '0');
        }
        protected void FillInbound()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;
            HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "SELECT  distinct HU  FROM StocktakeCounterInstruction where isnull(Msaved,0) = 0 and  MAllocatedTo='" + hdUserName.Value + "'";
                if (txtStock.Text != "")
                    qry += " and HU like '%" + txtStock.Text + "%' ";

                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
                txtStock.Text = "";
            }
        }
        protected void FillGrid()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;
            HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "SELECT ManagerQty as Qty, HU,MSTN as STN FROM StocktakeCounterInstruction where isnull(Msaved,0) = 1 and isnull(MAllocatedCompleted,0)=0 and  MAllocatedTo='" + hdUserName.Value + "'";

                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    grdScans.DataSource = dt;
                    grdScans.DataBind();
                    if (dt.Rows.Count > 0)
                        txtGRN.Text = dt.Rows[0]["STN"].ToString();
                }
            }
        }



        protected void btnDelete_Click(object sender, EventArgs e)
        {
            FillInbound();
            popupdelete.Visible = true;
        }








        protected void btnDeleteClosw_Click(object sender, EventArgs e)
        {
            popupdelete.Visible = false;

        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // return;
            GridViewRow row = GridView1.SelectedRow;
            txtAddHU.Text = row.Cells[1].Text;
            popupdelete.Visible = false;
        }



        protected void imgStock_Click(object sender, ImageClickEventArgs e)
        {
            FillInbound();
        }

        protected void txtStock_TextChanged(object sender, EventArgs e)
        {
            FillInbound();
        }

        protected void btnCompleteScan_Click(object sender, EventArgs e)
        {

        }

        protected void btrnViewPending_Click(object sender, EventArgs e)
        {
            FillInbound();
            popupdelete.Visible = true;
        }

        protected void btnAddSave_Click(object sender, EventArgs e)
        {
            save();

        }
        private bool checkHU()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from StocktakeCounterInstruction where MAllocatedTo is not null and isnull(MSaved,0)=1 and HU='" + txtAddHU.Text + "'";
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
        private bool checkInvalidHU()
        {
            HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");

            var userName = hdUserName.Value;
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from StocktakeCounterInstruction where MAllocatedTo<> '" + userName + "' and HU='" + txtAddHU.Text + "'";
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
        private void save()
        {
            txtAddHU.Text = txtAddHU.Text.Trim();
            HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");

            var userName = hdUserName.Value;
            if (txtAddHU.Text == "")
            {
                string script = "alert(\"HU Cannot be empty\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                txtAddHU.Text = "";
                txtAddHU.Focus();
                return;
            }

            if (checkHU())
            {
                string script = "alert(\"HU Already Processed\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                txtAddHU.Text = "";
                txtAddHU.Focus();
                return;
            }
            if (checkInvalidHU())
            {
                string script = "alert(\"HU not allocated for you\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                txtAddHU.Text = "";
                txtAddHU.Focus();
                return;
            }

            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                HiddenField hdLoginId = (HiddenField)this.Master.FindControl("hdnLoginId");

                var userid = hdLoginId.Value;

                string qry = "Update StockTakeHistory set MStockTakeNumber='" + txtGRN.Text + "',ManagerQty='" + txtQty.Text + "',ManagerCountedDate=getDate(),ManagerCountBy='" + userName + "' Where HU='" + txtAddHU.Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                cmd1.ExecuteNonQuery();
                qry = "Update StocktakeCounterInstruction set ManagerQty='" + txtQty.Text + "',MSTN='" + txtGRN.Text + "',MSaved=1 where HU='" + txtAddHU.Text + "'";
                cmd1 = new SqlCommand(qry, con);
                cmd1.ExecuteNonQuery();
                qry = "Update StockMaster set MCounter1Qty='" + txtQty.Text + "' where HU='" + txtAddHU.Text + "'";
                cmd1 = new SqlCommand(qry, con);
                cmd1.ExecuteNonQuery();
                FillGrid();
            }
            txtAddHU.Text = "";
            txtAddHU.Focus();
           // txtQty.Text = "";
        }

        protected void btnComplete_Click(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();

                // Prepare the query with a parameter
                string qry = "UPDATE StocktakeCounterInstruction SET MAllocatedCompleted = 1 WHERE HU = @HU";

                using (SqlCommand cmd1 = new SqlCommand(qry, con))
                {
                    // Add the parameter once (value will be updated in each iteration)
                    cmd1.Parameters.Add("@HU", SqlDbType.VarChar);

                    // Loop through the GridView rows
                    foreach (GridViewRow row in grdScans.Rows)
                    {
                        // Assuming the "HU" column is the second column (index 1)
                        string huValue = row.Cells[1].Text.Trim();

                        // Set the parameter value for each iteration
                        cmd1.Parameters["@HU"].Value = huValue;

                        // Execute the query
                        cmd1.ExecuteNonQuery();
                    }
                    
                }
                FillInbound();
                HiddenField hdLoginId = (HiddenField)this.Master.FindControl("hdnLoginId");

                var userid = hdLoginId.Value;
                HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");

                var userName = hdUserName.Value;
                foreach (GridViewRow row in GridView1.Rows)
                {
                    string Hu = row.Cells[1].Text;
                     qry = "Update StockTakeHistory set MStockTakeNumber='" + txtGRN.Text + "',ManagerQty=0,ManagerCountedDate=getDate(),ManagerCountBy='" + userName + "' Where HU='" + Hu + "'";
                    SqlCommand cmd1 = new SqlCommand(qry, con);
                    cmd1.ExecuteNonQuery();
                    qry = "Update StocktakeCounterInstruction set MAllocatedCompleted = 1,ManagerQty=0,MSTN='" + txtGRN.Text + "',MSaved=1 where HU='" + Hu + "'";
                    cmd1 = new SqlCommand(qry, con);
                    cmd1.ExecuteNonQuery();
                    qry = "Update StockMaster set MCounter1Qty=0 where HU='" + Hu + "'";
                    cmd1 = new SqlCommand(qry, con);
                    cmd1.ExecuteNonQuery();
                }
                string script = "alert(\"Stock Take Completed Successfully\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                GenerateGRN();
                FillGrid();
            }

        }

        protected void txtQty_TextChanged(object sender, EventArgs e)
        {
            save();
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {

        }
    }
}