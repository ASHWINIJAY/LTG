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
    public partial class StorageFee : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindBranch();
                bindBin();
               
            }
        }
        private void bindBin()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from Bin";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        ddlBin.DataSource = dt;
                        ddlBin.DataBind();
                        ddlBin.DataTextField = "BinName";
                        ddlBin.DataValueField = "BinId";
                        ddlBin.DataBind();
                        // ddlBranch.da
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
                string qry = "Select * from Customers where BranchId=" + ddlBranch.SelectedValue;
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
                        ddlBranch.Items.Insert(0, new ListItem("Select Branch", ""));
                        // ddlBranch.da
                    }
                }
            }
        }
        private void FillData()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select Fee from FeeMaster Where FeeType='StorageFee' and CustomerCode='" + ddlCustomer.SelectedValue + "' and BranchId=" + ddlBranch.SelectedValue +" order by FeeId desc";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        txtExistingFee.Text = dt.Rows[0]["Fee"].ToString();
                        // ddlBranch.da
                    }
                    else
                    {
                        txtExistingFee.Text = "0";
                    }
                }
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            if(ddlCustomer.Items.Count<=0 || ddlCustomer.SelectedIndex==0)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Please select customer');", true);
                return;
            }
            if (txtNewFee.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Please enter the storage fee');", true);
                return;
            }
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "";
                HiddenField hdLoginId = (HiddenField)this.Master.FindControl("hdnLoginId");

                var userid = hdLoginId.Value;
                HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");

                var userName = hdUserName.Value;

                qry = "Insert into FeeMaster(BranchId,BranchName,FeeType,Fee,Active,ValidFrom,CreatedBy,CreatedDate,Bin,CustomerCode)values('" + ddlBranch.SelectedValue + "','" + ddlBranch.SelectedItem.Text + "','StorageFee','" + txtNewFee.Text + "',1,getdate(),'" + userName + "',getdate(),'" + ddlBin.SelectedValue + "','" + ddlCustomer.SelectedValue + "')";

                using (SqlCommand cmd = new SqlCommand(qry, con))
                {


                    if (txtExistingFee.Text == "")
                        txtExistingFee.Text = "0";
                    cmd.ExecuteNonQuery();
                    qry = "Update Customers set StorageFee='" + txtNewFee.Text + "',PreviousStorageFee='" + txtExistingFee.Text + "' where BranchId=" + ddlBranch.SelectedValue + " and CustomerCode='" + ddlCustomer.SelectedValue + "'";
                    cmd.CommandText = qry;
                    cmd.ExecuteNonQuery();
                    qry = "Update WarehouseProcess set UnitStorageCost='" + txtNewFee.Text + "',TotalStorageCost='" + txtNewFee.Text + "' where BranchId=" + ddlBranch.SelectedValue + " and QtyOnHand>0 and CustomerCode='" + ddlCustomer.SelectedValue + "'";
                    cmd.CommandText = qry;
                    cmd.ExecuteNonQuery();
                    qry = "Insert into AuditLogs([Table],Field,ExistingValue,NewValue,ModifiedByUserId,ModifiedByUserName,ModifiedByDate,BranchName,BranchId,Custom)Values('FeeMaster','Fee','" + txtExistingFee.Text + "','" + txtNewFee.Text + "'," + userid + ",'" + userName + "',getdate(),'" + ddlBranch.SelectedItem.Text + "','" + ddlBranch.SelectedValue + "','Update the StorageFee Fee " + ddlCustomer.SelectedValue + "')";
                    cmd.CommandText = qry;
                    cmd.ExecuteNonQuery();
                    qry = "Insert into AuditLogs([Table],Field,ExistingValue,NewValue,ModifiedByUserId,ModifiedByUserName,ModifiedByDate,BranchName,BranchId,Custom)Values('Customers','StorageFee','" + txtExistingFee.Text + "','" + txtNewFee.Text + "'," + userid + ",'" + userName + "',getdate(),'" + ddlBranch.SelectedItem.Text + "','" + ddlBranch.SelectedValue + "','Update the StorageFee Fee for Customer " + ddlCustomer.SelectedValue + "')";
                    cmd.CommandText = qry;
                    cmd.ExecuteNonQuery();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Storage Fee Successfully Updated.');", true);
                    txtNewFee.Text = "";
                    FillData();
                }
            }
        }
        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindCustomer();
        }

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillData();
        }
    }
}