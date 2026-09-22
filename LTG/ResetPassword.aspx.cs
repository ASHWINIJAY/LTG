using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

namespace LTG
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private string ConnectionString
        {
            get
            {
                return ConfigurationManager
                    .ConnectionStrings["LTGConn"]
                    .ConnectionString;
            }
        }
        protected void btnReset_Click(
    object sender,
    EventArgs e)
        {
            if (txtPassword.Text !=
                txtConfirmPassword.Text)
            {
                lblMessage.Text =
                    "Passwords do not match.";

                return;
            }

            string token =
                Request.QueryString["token"];

            string username =
                GetUserName(token);

            UpdatePassword(
                username,
                txtPassword.Text);

            MarkTokenUsed(token);

          

            Response.Redirect(
                "Login.aspx?reset=1");
        }
        private void UpdatePassword(
    string userName,
    string password)
        {
            string encryptedPassword =
                (password);

            string sql = @"
        UPDATE Users
        SET Password = @Password
        WHERE UserName = @UserName";

            using (SqlConnection con =
                new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd =
                    new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue(
                        "@Password",
                        encryptedPassword);

                    cmd.Parameters.AddWithValue(
                        "@UserName",
                        userName);

                    con.Open();

                    cmd.ExecuteNonQuery();
                }
            }
        }
        private string GetUserName(string token)
        {
            string userName = string.Empty;

            string sql = @"
        SELECT UserName
        FROM PasswordResetRequest
        WHERE ResetToken = @Token
        AND IsUsed = 0
        AND ExpiryDate > GETDATE()";

            using (SqlConnection con =
                new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd =
                    new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue(
                        "@Token",
                        Guid.Parse(token));

                    con.Open();

                    object result =
                        cmd.ExecuteScalar();

                    if (result != null)
                    {
                        userName = result.ToString();
                    }
                }
            }

            return userName;
        }
        private void MarkTokenUsed(
    string token)
        {
            string sql =
            @"UPDATE PasswordResetRequest
      SET IsUsed = 1
      WHERE ResetToken = @Token";

            using (SqlConnection con =
                new SqlConnection(ConnectionString))
            {
                SqlCommand cmd =
                    new SqlCommand(sql, con);

                cmd.Parameters.AddWithValue(
                    "@Token",
                    token);

                con.Open();

                cmd.ExecuteNonQuery();
            }
        }
    }
}