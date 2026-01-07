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
    public partial class HuAdj : System.Web.UI.Page
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
                string qry = "Update Inbound set CustomerCode='" + ddlNewCustomer.SelectedValue + "',CustomerName='" + ddlNewCustomer.SelectedItem.Text + "' where ContainerId='" + txtHU.Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
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
                string qry = "Select * from Inbound where ContainerId='" + txtHU.Text + "' and CustomerCode='" + ddlCustomer.SelectedValue + "'";
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
        protected void FillInbound()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "SELECT  distinct HU  FROM Inbound where  ContainerId='" + txtHU.Text + "'";
                qry += " and HU like '%" + txtStock.Text + "%'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
            }
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

        protected void txtExistingHU_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtNewHu_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtAddHU_TextChanged(object sender, EventArgs e)
        {

        }

        protected void btnComplete_Click1(object sender, EventArgs e)
        {
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
            popupAdd.Visible = true;
        }

        protected void btnModifyHu_Click(object sender, EventArgs e)
        {
            popupMod.Visible = true;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            FillInbound();
            popupdelete.Visible = true;
        }

        protected void btnAddSave_Click(object sender, EventArgs e)
        {
            save();
        }

        protected void btnAddclose_Click(object sender, EventArgs e)
        {
            popupAdd.Visible = false;
        }
        private bool checkHUForModification()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from Inbound where HU='" + txtExistingHU.Text + "' and ContainerId='" + txtHU.Text + "'";
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
        private bool checkHUWarehoused()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from Warehouseprocess where HU='" + txtExistingHU.Text + "'";
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
                string qry = "Select * from Inbound where HU='" + txtAddHU.Text + "'";
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
        private bool checkModifyHU()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from Inbound where HU='" + txtNewHu.Text + "'";
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
                string script = "alert(\"HU Already Exisits\");";
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
                HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");

                var userName = hdUserName.Value;
                string qry = "Select * from Inbound where ContainerId='" + txtHU.Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    
                    if (dt.Rows.Count > 0)
                    {
                        qry = "Insert into Inbound(ContainerId,BranchId,BranchName,CustomerCode,CustomerName,HU,Qty,UnitInBoundCost,TotalInBoundCost,Loginname,CreatedDate,CreatedBy,DateTimeofScan,GRN)values('" + txtHU.Text + "'," + dt.Rows[0]["BranchId"].ToString() + ", '" + dt.Rows[0]["BranchName"].ToString() + "','" + ddlCustomer.SelectedValue + "','" + ddlCustomer.SelectedItem.Text + "','" + txtAddHU.Text + "','1'," + dt.Rows[0]["UnitInBoundCost"].ToString() + "," + dt.Rows[0]["UnitInBoundCost"].ToString() + ",'" + userid + "',getdate(),'" + userName + "','" + txtDate.Text + "','" + dt.Rows[0]["GRN"].ToString() + "')";
                        cmd1 = new SqlCommand(qry, con);
                        cmd1.ExecuteNonQuery();
                        
                        qry = "Insert into AuditLogs([Table],Field,Custom,ModifiedByUserId,ModifiedByUserName,ModifiedByDate)values('Inbound','HU','Added the HU: " + txtAddHU.Text + " ','" + hdLoginId.Value + "','" + userName + "',getdate())";
                        cmd1 = new SqlCommand(qry, con);
                        cmd1.ExecuteNonQuery();
                        string script1 = "alert(\"HU Added Successfully\");";
                        ScriptManager.RegisterStartupScript(this, GetType(),
                                              "ServerControlScript", script1, true);
                        // ddlBranch.da
                    }

                    
                }
            }
            txtAddHU.Text = "";
           
        }

        protected void btnModifyHuSave_Click(object sender, EventArgs e)
        {
            if (!checkHUForModification())
            {
                string script = "alert(\"Exisiting HU not available\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                txtExistingHU.Text = "";
                txtExistingHU.Focus();
                return;
            }
            if (checkHUWarehoused())
            {
                string script = "alert(\"Exisiting HU already Binned we cannot edit\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                txtExistingHU.Text = "";
                txtExistingHU.Focus();
                return;
            }
            if (checkModifyHU())
            {
                string script = "alert(\"New HU already available\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                txtNewHu.Text = "";
                txtNewHu.Focus();
                return;
            }

            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");

                var userName = hdUserName.Value;
                string qry = "Update Inbound set HU='" + txtNewHu.Text + "',ModifiedBy='" + userName + "' where HU='" + txtExistingHU.Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                cmd1.ExecuteNonQuery();
                string script = "alert(\"Successfully Updated\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                txtNewHu.Text = "";
                txtExistingHU.Text = "";
                // txtBin.Text = "";
                //txtNewBin.Text = "";
            }
        }

        protected void btnModClose_Click(object sender, EventArgs e)
        {
            popupMod.Visible = false;
        }

        protected void btnDeleteClosw_Click(object sender, EventArgs e)
        {
            popupdelete.Visible = false;
            
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
           // return;
            GridViewRow row = GridView1.SelectedRow;
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                
                string qry = "Select * from Warehouseprocess where HU='" + row.Cells[1].Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        string script1 = "alert(\"HU Already Binned we cannot delete\");";
                        ScriptManager.RegisterStartupScript(this, GetType(),
                                              "ServerControlScript", script1, true);
                    }
                    else
                    {
                        HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");

                        var userName = hdUserName.Value;
                        if (row.Cells[1].Text == "&nbsp;")
                            row.Cells[1].Text = "";
                        qry = "Update Inbound set ModifiedBy='"+userName+"' where HU='" + row.Cells[1].Text + "'";
                        cmd1 = new SqlCommand(qry, con);
                        cmd1.ExecuteNonQuery();
                        qry = "delete from Inbound where HU='" + row.Cells[1].Text + "'";
                        cmd1 = new SqlCommand(qry, con);
                        cmd1.ExecuteNonQuery();
                        string script1 = "alert(\"HU deleted Successfully\");";
                        ScriptManager.RegisterStartupScript(this, GetType(),
                                              "ServerControlScript", script1, true);
                        // ddlBranch.da
                    }


                }
                FillInbound();
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

        protected void imgStock_Click(object sender, ImageClickEventArgs e)
        {
            FillInbound();
        }

        protected void txtStock_TextChanged(object sender, EventArgs e)
        {
            FillInbound();
        }
    }
}