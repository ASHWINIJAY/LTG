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
    public partial class UserCreation : System.Web.UI.Page
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
                    btnCreate.Text = "Update";
                    chkReset.Visible = true;
                    divPassword.Visible = false;
                    divConPassword.Visible = false;
                    txtUserName.ReadOnly = true;
                }
                else
                    chkReset.Visible = false;
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
                        // ddlBranch.da
                    }
                }
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            if(txtPassword.Text!=txtConfirmPassword.Text)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Password and Confirm Password should be match.');", true);
                return;
            }
            if (Request.QueryString["Code"] != null)
            {
                Update();
            }
            else
                save();
        }
        private void clear()
        {
            txtUserName.Text = "";
            txtName.Text = "";
            txtSurName.Text = "";
            txtEmail.Text = "";
            txtMobile.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            ddlRoles.SelectedIndex = 0;
        }
        private void loadUserData(string username)
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select FirstName, LastName, Email, Roles, Mobile, Active,Password,Username from users where Username=@Username";
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@Username", username);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) // If the user exists
                    {
                        // Assign values to controls
                        txtName.Text = reader["FirstName"].ToString();
                        txtSurName.Text = reader["LastName"].ToString();
                        txtEmail.Text = reader["Email"].ToString();
                        ddlRoles.SelectedItem.Text = reader["Roles"].ToString();
                        txtMobile.Text = reader["Mobile"].ToString();
                        txtPassword.TextMode = TextBoxMode.SingleLine;
                            txtPassword.Text = reader["Password"].ToString();
                        txtConfirmPassword.Text = reader["Password"].ToString();
txtUserName.Text= reader["Username"].ToString();
                        txtPassword.TextMode = TextBoxMode.Password;
                        // Optionally, check the Active status
                        bool isActive = reader["Active"].ToString() == "Y";
                        chkActive.Checked = isActive;
                    }
                    else
                    {
                        // User does not exist, handle appropriately
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('User not found.');", true);
                    }
                }
            }
        }

        private void Update()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from users where Username=@Username";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                cmd1.Parameters.AddWithValue("@Username", txtUserName.Text);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0) // User already exists
                    {
                        // Update existing user
                        qry = "Update users set";
                        if( txtPassword.Text !="")
                        qry += " Password=@Password, " ;
                            qry += " FirstName =@FirstName, LastName=@LastName, Email=@Email, Roles=@Roles, Mobile=@Mobile, Active=@Active where Username=@Username";
                        string Active = "N";

                        if(chkActive.Checked)
                            Active="Y";
                        using (SqlCommand cmd = new SqlCommand(qry, con))
                        {
                            cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                            cmd.Parameters.AddWithValue("@FirstName", txtName.Text);
                            cmd.Parameters.AddWithValue("@LastName", txtSurName.Text);
                            cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                            cmd.Parameters.AddWithValue("@Roles", ddlRoles.SelectedItem.Text);
                            cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text);
                            cmd.Parameters.AddWithValue("@Active", Active);
                            cmd.Parameters.AddWithValue("@Username", txtUserName.Text);

                            cmd.ExecuteNonQuery();
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('User Successfully Updated.');", true);
                        }
                    }
                    else // New user, insert
                    {
                       
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Username not available.');", true);
                        }
                    }
                }
           

            clear();
        }
        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("UserMaintenance.aspx");
        }

        private void save()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from users where Username='" + txtUserName.Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Username already exisits.');", true);
                        return;
                    }
                }

                qry = "Insert into users(Username,Password,FirstName,LastName,Email,Roles,Mobile,Active)values('" + txtUserName.Text + "','" + txtPassword.Text + "','" + txtName.Text + "','" + txtSurName.Text + "','" + txtEmail.Text + "','" + ddlRoles.SelectedItem.Text + "','" + txtMobile.Text + "','Y')";

                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                   
                   

                    cmd.ExecuteNonQuery();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('User Successfully Created.');", true);
                    clear();
                }
            }

        }

        protected void chkReset_CheckedChanged(object sender, EventArgs e)
        {
            if (chkReset.Checked)
            {
                divPassword.Visible = true;
                divConPassword.Visible = true;
            }
            else
            {
                divPassword.Visible = false;
                divConPassword.Visible = false;

            }
            chkReset.Focus();
        }

    }
}