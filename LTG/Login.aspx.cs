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
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;
            

            using (SqlConnection con = new SqlConnection(constr))
            {

                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE Username = @User AND Password = @Id", con))
                {
                    cmd.Parameters.AddWithValue("@Id", txtPassword.Text);
                    cmd.Parameters.AddWithValue("@User", txtUsername.Text);
                    con.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            if(dt.Rows[0]["Active"] != DBNull.Value)
                            {
                                if(dt.Rows[0]["Active"].ToString()=="N")
                                {
                                    Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('User deactivated,Please contact administrator.');", true);
                                    return;
                                }
                            }
                            HttpCookie myCookie = new HttpCookie("LoginId");

                            myCookie.Value = dt.Rows[0]["UserId"].ToString();
                            // Set the cookie expiration date.
                            myCookie.Expires = DateTime.Now.AddHours(50); // For a cookie to effectively never expire

                            // Add the cookie.
                            Response.Cookies.Add(myCookie);
                            HttpCookie loginValue = new HttpCookie("Username");

                            loginValue.Value = dt.Rows[0]["Username"].ToString();
                            // Set the cookie expiration date.
                            loginValue.Expires = DateTime.Now.AddHours(50); // For a cookie to effectively never expire

                            // Add the cookie.
                            Response.Cookies.Add(loginValue);

                            HttpCookie loginRole = new HttpCookie("loginRole");

                            loginRole.Value = dt.Rows[0]["Roles"].ToString();
                            // Set the cookie expiration date.
                            loginRole.Expires = DateTime.Now.AddHours(50); // For a cookie to effectively never expire
                            Response.Cookies.Add(loginRole);
                            HttpCookie firstName = new HttpCookie("firstName");

                            firstName.Value = dt.Rows[0]["FirstName"].ToString();
                            // Set the cookie expiration date.
                            firstName.Expires = DateTime.Now.AddHours(50); // For a cookie to effectively never expire

                            // Add the cookie.
                            Response.Cookies.Add(firstName);
                            Response.Redirect("Dashboard.aspx");
                        }
                        else
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Invalid username or password.');", true);

                    }
                }
            }
        }
    }
}