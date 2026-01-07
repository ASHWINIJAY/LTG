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
    public partial class ManagerBinStockTake : System.Web.UI.Page
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
                string qry = "select count(distinct(MSTN))+1 FROM BinAccuracyHistory";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        string output = FormatNumberWithLeadingZeros(dt.Rows[0][0].ToString(), 9);
                        txtGRN.Text = "MSTAN-" + dtMonth + "-" + output;
                        
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
                string qry = "SELECT  distinct CountedBin as Bin  FROM  BinAccuracyCount where isnull(Msaved,0) = 0 and isnull(MCompleted,0)=0 and StockTakeCompleted is null and  MAllocatedTo='" + hdUserName.Value + "'";
                if (txtStock.Text != "")
                    qry += " and CountedBin like '%" + txtStock.Text + "%' ";

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
            if (Session["Username"] == null)
                Response.Redirect("Login.aspx");
            HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");
            hdUserName.Value = Session["Username"].ToString();
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "SELECT CountedBin as  Bin, HU,MSTN as STN  FROM BinAccuracyCount where isnull(Msaved,0) = 1 and isnull(MCompleted,0)=0 and  MCountedBy='" + hdUserName.Value + "'";
               
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
            txtBin.Text = row.Cells[1].Text;
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
                string qry = "Select * from BinAccuracyCount where  HU='" + txtAddHU.Text + "' and MAllocatedCompleted=1 and isnull(MSaved,0)=1";
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
        private void getActaulBin()
        {
            HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");

            var userName = hdUserName.Value;
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select Bin from WarehouseProcess where HU='" + txtAddHU.Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        hdnActualBin.Value = dt.Rows[0][0].ToString();
                    }
                    else
                        hdnActualBin.Value = "";
                }
            }
        }
        private bool checkInvalidHU()
        {
            HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");

            var userName = hdUserName.Value;
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from BinCounterInstruction where AllocatedTo<> '" + userName +"' and Bin='" + txtBin.Text + "'";
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
        private bool checkNewHU()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from BinAccuracyCount where  HU='" + txtAddHU.Text + "'";
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
        private bool checkValidHU()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from Warehouseprocess where  HU='" + txtAddHU.Text + "'";
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
            if (!checkValidHU())
            {
                string script = "alert(\"HU not found\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                txtAddHU.Text = "";
                txtAddHU.Focus();
                return;
            }
            //if (checkInvalidHU())
            //{
            //    string script = "alert(\"Bin not allocated for you\");";
            //    ScriptManager.RegisterStartupScript(this, GetType(),
            //                          "ServerControlScript", script, true);
            //    txtAddHU.Text = "";
            //    txtAddHU.Focus();
            //    return;
            //}
            getActaulBin();

            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                HiddenField hdLoginId = (HiddenField)this.Master.FindControl("hdnLoginId");

                var userid = hdLoginId.Value;
               
                string qry = "Update BinAccuracyHistory set  MSTN='" + txtGRN.Text + "',ManagerCountedBin='" + txtBin.Text + "',MSaved=1,MCountedBy='" + userName + "' Where HU='" + txtAddHU.Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                cmd1.ExecuteNonQuery();
                qry = "Update BinAccuracyCount set MSTN='" + txtGRN.Text + "', ManagerCountedBin='" + txtBin.Text + "',MSaved=1,MCountedBy='" + userName + "' Where HU='" + txtAddHU.Text + "'";
                cmd1 = new SqlCommand(qry, con);
                cmd1.ExecuteNonQuery();
                if (!checkNewHU())
                {
                    qry = "Insert into BinAccuracyCount(MSTN,HU,CountedBin,ManagerCountedBin,ActualBin,MSaved,MCountedBy)Values('" + txtGRN.Text + "','" + txtAddHU.Text + "','" + txtBin.Text + "','" + txtBin.Text + "','" + hdnActualBin.Value + "',1,'" + userName + "')";
                    cmd1 = new SqlCommand(qry, con);
                    cmd1.ExecuteNonQuery();
                }
                FillGrid();
            }
            txtAddHU.Text = "";
           // txtBin.Text = "";
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
                string qry = "Update BinAccuracyCount SET MCompleted = 1 WHERE CountedBin = @HU ";

                using (SqlCommand cmd2 = new SqlCommand(qry, con))
                {
                    // Add the parameter once (value will be updated in each iteration)
                    cmd2.Parameters.Add("@HU", SqlDbType.VarChar);

                    // Loop through the GridView rows
                    foreach (GridViewRow row in grdScans.Rows)
                    {
                        // Assuming the "HU" column is the second column (index 1)
                        string huValue = row.Cells[1].Text.Trim();

                        // Set the parameter value for each iteration
                        cmd2.Parameters["@HU"].Value = huValue;

                        // Execute the query
                        cmd2.ExecuteNonQuery();
                        
                    }
                }
                // string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;
                FillInbound();


                    HiddenField hdLoginId = (HiddenField)this.Master.FindControl("hdnLoginId");

                    var userid = hdLoginId.Value;
                HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");

                var userName = hdUserName.Value;
                
                string script = "alert(\"Bin Accuracy Stock Take Completed Successfully\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                GenerateGRN();
                FillGrid();
                if (grdScans.Rows.Count > 0)
                {
                    txtBin.Text = grdScans.Rows[0].Cells[1].Text;
                }
                else
                    txtBin.Text = "";
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