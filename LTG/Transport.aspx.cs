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
    public partial class Transport : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                bindBranch();
                if (Request.QueryString["Code"] != null)
                {
                    var id = Request.QueryString["Code"].ToString();
                    loadUserData(id);
                    divTest.InnerText = "Editing Transporter";
                }
            }
        }
        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindCustomer();
        }

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
           // FillData();
        }
        private void loadUserData(string username)
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from Transport where TransportId=@Username";
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@Username", username);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) // If the user exists
                    {
                        
                        txtSurName.Text = reader["TransportName"].ToString();
                        ddlBranch.SelectedValue = reader["BranchId"].ToString();
                        bindCustomer();
                        if(reader["CustomerCode"] != null)
                        ddlCustomer.SelectedValue = reader["CustomerCode"].ToString();
                    }
                    else
                    {
                        // User does not exist, handle appropriately
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Transporter not found.');", true);
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
        protected void btnCreate_Click(object sender, EventArgs e)
        {
            if(ddlCustomer.SelectedIndex==0)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Please select customer.');", true);
                return;
            }
            if (Request.QueryString["Code"] != null)
            {
                var id = Request.QueryString["Code"].ToString();
                save(id);

            }
            else
            { 
                save();
            }
            }
        private void save(string id)
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "";
                
                HiddenField hdLoginId = (HiddenField)this.Master.FindControl("hdnLoginId");

                var userid = hdLoginId.Value;
                HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");

                var userName = hdUserName.Value;

                // Update query to modify the existing Transport record
                qry = "UPDATE Transport SET TransportName=@TransportName1,CustomerCode=@CustomerCode, BranchId = @BranchId, BranchName = @BranchName, ModifiedBy = @CreatedBy, ModifiedDate = getdate() WHERE TransportId = @TransportName";

                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.AddWithValue("@BranchId", ddlBranch.SelectedValue);
                    cmd.Parameters.AddWithValue("@CustomerCode", ddlCustomer.SelectedValue);
                    cmd.Parameters.AddWithValue("@BranchName", ddlBranch.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@CreatedBy", userName);
                    cmd.Parameters.AddWithValue("@TransportName", id);
                    cmd.Parameters.AddWithValue("@TransportName1", txtSurName.Text);

                    cmd.ExecuteNonQuery();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Transport Successfully Updated.');", true);
                    txtSurName.Text = "";
                }
            }
        }

        private void save()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from Transport where TransportName='" + txtSurName.Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Transport Name already exisits.');", true);
                        return;
                    }
                }
                HiddenField hdLoginId = (HiddenField)this.Master.FindControl("hdnLoginId");

                var userid = hdLoginId.Value;
                HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");

                var userName = hdUserName.Value;
                qry = "Insert into Transport(BranchId,BranchName,TransportName,CreatedBy,CreatedDate,CustomerCode)values('" + ddlBranch.SelectedValue + "','" + ddlBranch.SelectedItem.Text + "','" + txtSurName.Text + "','" + userName + "',getdate(),'" + ddlCustomer.SelectedValue +"')";

                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.ExecuteNonQuery();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Transport Successfully Created.');", true);
                    txtSurName.Text = "";
                }
            }
        }
    }
}