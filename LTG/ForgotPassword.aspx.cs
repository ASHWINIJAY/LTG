using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Web.UI;

namespace LTG
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        private string ConnectionString
        {
            get
            {
                return ConfigurationManager
                    .ConnectionStrings["LTGConn"]
                    .ConnectionString;
            }
        }
        protected void btnSubmit_Click(
            object sender,
            EventArgs e)
        {
            string username =
                txtUsername.Text.Trim();

            if (!UserExists(username))
            {
                ScriptManager.RegisterStartupScript(
    this,
    GetType(),
    "alert",
    "alert('Username does not exist.');",
    true);

                return;
            }

            Guid token = Guid.NewGuid();

            SaveResetToken(
                username,
                token);

            SendResetEmail(
                username,
                token);

            lblMessage.ForeColor =
                System.Drawing.Color.Green;

            ScriptManager.RegisterStartupScript(
    this,
    GetType(),
    "alert",
    "alert('Password reset request submitted successfully.Please Contact Admin Team');",
    true);
        }
        private void SaveResetToken(
    string username,
    Guid token)
        {
            string sql =
            @"INSERT INTO PasswordResetRequest
      (
        UserName,
        ResetToken,
        RequestedDate,
        ExpiryDate,
        IsUsed
      )
      VALUES
      (
        @UserName,
        @Token,
        GETDATE(),
        DATEADD(HOUR,24,GETDATE()),
        0
      )";

            using (SqlConnection con =
                new SqlConnection(ConnectionString))
            {
                SqlCommand cmd =
                    new SqlCommand(sql, con);

                cmd.Parameters.AddWithValue(
                    "@UserName",
                    username);

                cmd.Parameters.AddWithValue(
                    "@Token",
                    token);

                con.Open();

                cmd.ExecuteNonQuery();
            }
        }
        private void SendResetEmail(string username, Guid token)
        {
            string smtpServer = ConfigurationManager.AppSettings["SMTPServer"];
            int smtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
            string smtpUsername = ConfigurationManager.AppSettings["SMTPUserName"];
            string smtpPassword = ConfigurationManager.AppSettings["SMTPPassword"];

            
            string resetLink =
    ResolveUrl("~/ResetPassword.aspx?token=" + token);

            string fullUrl =
                Request.Url.GetLeftPart(UriPartial.Authority)
                + resetLink;

            MailMessage mail = new MailMessage();

            var emails = GetMailSetup();

            foreach (DataRow row in emails.Rows)
            {
                string email = row["MailIds"].ToString();

                if (!string.IsNullOrWhiteSpace(email))
                {
                    mail.To.Add(email);
                }
            }

            mail.From = new MailAddress(smtpUsername);

            mail.Subject = "Password Reset Request";

            mail.Body =
                $"Dear Administrator,<br/><br/>" +
                $"A password reset has been requested for user <b>{username}</b>.<br/><br/>" +
                $"Please forward the link below to the user:<br/><br/>" +
                $"<a href='{fullUrl}'>Click Here to Reset the Password</a><br/><br/>" +
                $"This link expires in 24 hours.";

            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient(smtpServer, smtpPort);

            smtp.EnableSsl = true;

            smtp.Credentials = new NetworkCredential(
                smtpUsername,
                smtpPassword);

            smtp.Send(mail);
        }
        private DataTable GetMailSetup()
        {
            DataTable dt = new DataTable();

            string sql = @"
        SELECT MailIds
        FROM MailSetup
        WHERE Type = 5";

            using (SqlConnection con =
                new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd =
                    new SqlCommand(sql, con))
                {
                    using (SqlDataAdapter da =
                        new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }
        private bool UserExists(string username)
        {
            string sql =
                @"SELECT COUNT(*)
                  FROM Users
                  WHERE Username=@UserName";

            using (SqlConnection con =
                new SqlConnection(
                ConnectionString))
            {
                SqlCommand cmd =
                    new SqlCommand(sql, con);

                cmd.Parameters.AddWithValue(
                    "@UserName",
                    username);

                con.Open();

                return (int)cmd.ExecuteScalar() > 0;
            }
        }
    }
}