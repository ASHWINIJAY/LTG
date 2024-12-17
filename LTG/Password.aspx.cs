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
    public partial class Password : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Password and Confirm Password should be match.');", true);
                return;
            }
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
              
                        // Update existing user
                      string  qry = "Update users set";
                        if (txtPassword.Text != "")
                            qry += " Password=@Password ";
                        qry += " where Username=@Username";
                HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");

                var userName = hdUserName.Value;
                using (SqlCommand cmd = new SqlCommand(qry, con))
                        {
                            cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                           
                            cmd.Parameters.AddWithValue("@Username",userName );

                            cmd.ExecuteNonQuery();
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Password successfully changed.');", true);
                        }
                txtConfirmPassword.Text = "";
                txtPassword.Text = "";

            }
        }
    }
}