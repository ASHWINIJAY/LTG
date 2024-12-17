using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LTG
{
    public partial class Delivery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)

                if (Request.QueryString["Id"] != null)
                {
                   // ScriptManager.RegisterStartupScript(this, this.GetType(), "showLoader", "showLoader();", true);
                    var id = Request.QueryString["Id"].ToString();
                   // CallExecutable(id.ToString());
                    // Assuming you have the document ID
                    // int documentId = 8; // You can get this value dynamically as needed
                   // Thread.Sleep(8000);
                    // Set the src attribute of the iframe
                    pdfIframe.Attributes["src"] = $"PdfHandler.ashx?id={id}";
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "showLoader", "closeLoader();", true);
                    SendEmailWithAttachment(id);
                }
            }
        public void SendEmailWithAttachment(string id)
        {
            // Specify the sender's and receiver's email addresses
            string fromEmail = "noreply@ltgfreight.co.za";
           // string toEmail = "aswini@codex-it.co.za";
            string fileName = GetPdfFileName(id);
            // Specify the subject and body of the email
            string subject = "LTG Delivery Note Number-" + fileName;
            //string body = "This is the body of the email.";
            string body = "<b>Hello!  </b>, <br> <br>";
            body += "Kindly find attached a copy of the Delivery Note Generated From The LTG Warehousing Application.";
            body += "<br><br><b> Thanks & Regards,</b> <br>LTG Team.";
            string smtpUsername = "noreply@ltgfreight.co.za"; // Usually your email address
           string smtpPassword = "Max853000"; // Your email passwords


            try
            {
                // Create a new MailMessage object
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(fromEmail);
                //mail.To.Add(toEmail);
                // mail.To.Add("greg@codex-it.co.za");
                mail.To.Add("tbusang@ltgfreight.co.za");
                mail.To.Add("vz3dispatch@ltgfreight.co.za");
                mail.To.Add("tmabena@ltgfreight.co.za");
                mail.To.Add("vz3receiving@ltgfreight.co.za");
                mail.To.Add("tngamthewe@ltgfreight.co.za");
                mail.To.Add("tsetshedi@ltgfreight.co.za");


                mail.Subject = subject;
                mail.Body = body;
                var smtp = new SmtpClient();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                MemoryStream memoryStream = new MemoryStream(GetPdfDataFromDatabase(id));
                 fileName = fileName + ".pdf";
                Attachment attach1 = new Attachment(memoryStream, fileName);
                mail.Attachments.Add(attach1);

                mail.IsBodyHtml = true;


                smtp.Port = 587;
                //smtp.Host = "smtp.gmail.com";
                smtp.Host = "smtp.office365.com";
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtp.Send(mail);


                Console.WriteLine("Email sent successfully with attachment.");
            }
            catch (Exception ex)
            {
                // Handle any errors that occur
                Console.WriteLine("An error occurred while sending the email: " + ex.Message);
            }
        }
        public string filename = "";
        private string GetPdfFileName(string documentId)
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select GDN from Outbound where OutSlip='" + documentId + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0 && dt.Rows[0]["GDN"] != DBNull.Value)
                    {
                       
                        return dt.Rows[0]["GDN"].ToString();
                    }
                    else
                    {
                        // Handle the case where there's no data
                        return "GDN";
                        // Or handle accordingly, e.g., throwing an exception or returning an empty array
                    }
                }
            }
            // Fetch PDF from your database using Entity Framework or ADO.NET

        }

        private byte[] GetPdfDataFromDatabase(string documentId)
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from DeliveryDetails where OutSlip='" + documentId + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0 && dt.Rows[0]["DeliveryNote"] != DBNull.Value)
                    {
                        filename = "";
                        // Convert the "DeliveryNote" column value to byte[]
                        return (byte[])dt.Rows[0]["DeliveryNote"];
                    }
                    else
                    {
                        // Handle the case where there's no data
                        return null;
                        // Or handle accordingly, e.g., throwing an exception or returning an empty array
                    }
                }
            }
            // Fetch PDF from your database using Entity Framework or ADO.NET

        }


        public void CallExecutable(string id)
        {
            // Specify the path to the executable
            string exePath = @"C:\DEVELOPMENT\LTGReports\bin\Debug\LTGReports.exe";
           //string exePath = @"C:\LTG\LTGLiveReports\LTGReports.exe";

            // Specify any arguments if needed
            string arguments = id;

            // Create a new process start info object
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = arguments,
                RedirectStandardOutput = true,
                UseShellExecute = false, // Important: this prevents redirection of streams
                CreateNoWindow = true // Optional: hides the console window
            };

            try
            {
                using (Process process = Process.Start(startInfo))
                {
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string result = reader.ReadToEnd();
                       // return result;
                    }
                }

            }
            catch (Exception ex)
            {
                // Handle exception
                Response.Write("An error occurred: " + ex.Message);
            }
        }
    }

    
}