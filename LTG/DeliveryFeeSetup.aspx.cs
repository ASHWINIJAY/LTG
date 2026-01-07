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
    public partial class DeliveryFeeSetup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(! IsPostBack)
            {
                bindBranch();
               
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
                string qry = "Select DeliveryFee,SatDeliveryFee,PrevSatDeliveryFee,SunDeliveryFee,PrevSunDeliveryFee from Customers Where CustomerCode='" + ddlCustomer.SelectedValue + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        txtExistingFee.Text = dt.Rows[0]["DeliveryFee"].ToString();
                        txtExtSaturday.Text = dt.Rows[0]["SatDeliveryFee"].ToString();
                        txtExtSunday.Text = dt.Rows[0]["SunDeliveryFee"].ToString();

                        // ddlBranch.da
                    }
                    else
                    {
                        txtExistingFee.Text = "0";
                        txtExtSaturday.Text = "0";
                        txtExtSunday.Text = "0";
                    }
                }
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;
            if (ddlCustomer.Items.Count <= 0 || ddlCustomer.SelectedIndex == 0)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Please select customer');", true);
                return;
            }
            if (txtNewFee.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Please enter the Delivery fee');", true);
                return;
            }
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "";
                HiddenField hdLoginId = (HiddenField)this.Master.FindControl("hdnLoginId");

                var userid = hdLoginId.Value;
                HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");

                var userName = hdUserName.Value;

                qry = "Insert into FeeMaster(BranchId,BranchName,FeeType,Fee,Active,ValidFrom,CreatedBy,CreatedDate,CustomerCode)values('" + ddlBranch.SelectedValue + "','" + ddlBranch.SelectedItem.Text + "','Delivery','" + txtNewFee.Text + "',1,getdate(),'" + userName + "',getdate(),'" + ddlCustomer.SelectedValue + "')";

                using (SqlCommand cmd = new SqlCommand(qry, con))
                {

                    if (txtExtSaturday.Text == "")
                        txtExtSaturday.Text = "0";
                    if (txtExtSunday.Text == "")
                        txtExtSunday.Text = "0";
                    if (txtExistingFee.Text == "")
                        txtExistingFee.Text = "0";

                    cmd.ExecuteNonQuery();
                    qry = "Update Customers set DeliveryFee='" + txtNewFee.Text + "',PreviousDeliveryFee='" + txtExistingFee.Text + "',SatDeliveryFee='" + txtSaturday.Text + "',PrevSatDeliveryFee='" + txtExtSaturday.Text + "',SunDeliveryFee='" + txtSunday.Text + "',PrevSunDeliveryFee='" + txtExtSunday.Text + "' where CustomerCode='" + ddlCustomer.SelectedValue + "' and BranchId=" + ddlBranch.SelectedValue;
                    cmd.CommandText=qry;
                        cmd.ExecuteNonQuery();
                    if (txtNewFee.Text != txtExistingFee.Text)
                    {
                        //qry = "Insert into AuditLogs([Table],Field,ExistingValue,NewValue,ModifiedByUserId,ModifiedByUserName,ModifiedByDate,BranchName,BranchId,Custom)Values('FeeMaster','Fee','" + txtExistingFee.Text + "','" + txtNewFee.Text + "'," + userid + ",'" + userName + "',getdate(),'" + ddlBranch.SelectedItem.Text + "','" + ddlBranch.SelectedValue + "','Update the Delivery Fee " + ddlCustomer.SelectedValue + "')";
                        //cmd.CommandText = qry;
                        //cmd.ExecuteNonQuery();
                        qry = "Insert into AuditLogs([Table],Field,ExistingValue,NewValue,ModifiedByUserId,ModifiedByUserName,ModifiedByDate,BranchName,BranchId,Custom)Values('Customers','DeliveryFee','" + txtExistingFee.Text + "','" + txtNewFee.Text + "'," + userid + ",'" + userName + "',getdate(),'" + ddlBranch.SelectedItem.Text + "','" + ddlBranch.SelectedValue + "','Update the WeekDays Delivery Fee for Customer " + ddlCustomer.SelectedValue + "')";
                        cmd.CommandText = qry;
                        cmd.ExecuteNonQuery();
                    }
                     if (txtSaturday.Text != txtExtSaturday.Text)
                    {
                        //qry = "Insert into AuditLogs([Table],Field,ExistingValue,NewValue,ModifiedByUserId,ModifiedByUserName,ModifiedByDate,BranchName,BranchId,Custom)Values('FeeMaster','Fee','" + txtExtSaturday.Text + "','" + txtSaturday.Text + "'," + userid + ",'" + userName + "',getdate(),'" + ddlBranch.SelectedItem.Text + "','" + ddlBranch.SelectedValue + "','Update the Saturday Fee " + ddlCustomer.SelectedValue + "')";
                        //cmd.CommandText = qry;
                        //cmd.ExecuteNonQuery();
                        qry = "Insert into AuditLogs([Table],Field,ExistingValue,NewValue,ModifiedByUserId,ModifiedByUserName,ModifiedByDate,BranchName,BranchId,Custom)Values('Customers','Delivery Satuday Fee','" + txtExtSaturday.Text + "','" + txtSaturday.Text + "'," + userid + ",'" + userName + "',getdate(),'" + ddlBranch.SelectedItem.Text + "','" + ddlBranch.SelectedValue + "','Update the Saturday Delivery Fee for Customer " + ddlCustomer.SelectedValue + "')";
                        cmd.CommandText = qry;
                        cmd.ExecuteNonQuery();
                    }
                     if (txtSunday.Text != txtExtSunday.Text)
                    {
                        //qry = "Insert into AuditLogs([Table],Field,ExistingValue,NewValue,ModifiedByUserId,ModifiedByUserName,ModifiedByDate,BranchName,BranchId,Custom)Values('FeeMaster','Fee','" + txtExtSaturday.Text + "','" + txtSaturday.Text + "'," + userid + ",'" + userName + "',getdate(),'" + ddlBranch.SelectedItem.Text + "','" + ddlBranch.SelectedValue + "','Update the Saturday Fee " + ddlCustomer.SelectedValue + "')";
                        //cmd.CommandText = qry;
                        //cmd.ExecuteNonQuery();
                        qry = "Insert into AuditLogs([Table],Field,ExistingValue,NewValue,ModifiedByUserId,ModifiedByUserName,ModifiedByDate,BranchName,BranchId,Custom)Values('Customers','Delivery Sunday Fee','" + txtExtSunday.Text + "','" + txtSunday.Text + "'," + userid + ",'" + userName + "',getdate(),'" + ddlBranch.SelectedItem.Text + "','" + ddlBranch.SelectedValue + "','Update the Sunday Delivery Fee for Customer " + ddlCustomer.SelectedValue + "')";
                        cmd.CommandText = qry;
                        cmd.ExecuteNonQuery();
                    }
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Delivery Fee Successfully Updated.');", true);
                    txtNewFee.Text = "";
                    txtSaturday.Text = "";
                    txtSunday.Text = "";
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