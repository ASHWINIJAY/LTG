using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZXing;
using ZXing.Common;


namespace LTG
{
    public partial class DeliveryNote : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (getLoginType() == null)
                Response.Redirect("Login");
            if ( Request.QueryString["Id"] != null)
            {
                var id = Request.QueryString["Id"].ToString();
                CallExecutable(id);
                FillData(id);
                lblDate.Text = DateTime.Now.ToString("dd/MMM/yyyy hh:mm");
                Thread.Sleep(10000);
                SendEmailWithAttachment();
            }
        }
        public void SendEmailWithAttachment()
        {
            // Specify the sender's and receiver's email addresses
            string fromEmail = "noreply@ltgfreight.co.za";
            string toEmail = "aswini@codex-it.co.za";

            // Specify the subject and body of the email
            string subject = "Subject of the email";
            string body = "This is the body of the email.";

            // Specify the SMTP server details
            string smtpServer = "smtp.office365.com";
            int smtpPort = 587; // or 465 for SSL, or 25 depending on the provider
            string smtpUsername = "noreply@ltgfreight.co.za"; // Usually your email address
            string smtpPassword = "Max853000"; // Your email password

            // Specify the file path of the attachment
            string attachmentFilePath = @"C:\Development2\28-08-2024_02.23 PM.pdf";

            try
            {
                // Create a new MailMessage object
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(fromEmail);
                mail.To.Add(toEmail);
                mail.Subject = subject;
                mail.Body = body;
                var smtp = new SmtpClient();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                // Add the attachment
                Attachment attachment = new Attachment(attachmentFilePath);
                mail.Attachments.Add(attachment);
                smtp.Port = 587;
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

        public void CallExecutable(string id)
        {
            // Specify the path to the executable
            string exePath = @"C:\DEVELOPMENT\AussieProductionOrder - Copy\bin\Debug\ProductionOrders.exe";

            // Specify any arguments if needed
            string arguments = id;

            // Create a new process start info object
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = arguments,
                UseShellExecute = true, // Important: this prevents redirection of streams
                CreateNoWindow = false // Optional: hides the console window
            };

            try
            {
                Process process = Process.Start(startInfo);
               
            }
            catch (Exception ex)
            {
                // Handle exception
                Response.Write("An error occurred: " + ex.Message);
            }
        }
        private Bitmap GenerateBarcode(string text)
        {
            BarcodeWriter barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                Options = new EncodingOptions
                {
                    Width = 300,
                    Height = 80
                }
            };
            return barcodeWriter.Write(text);
        }
        private Int32 totalQty = 0;
        private string getLoginType()
        {
            HttpCookie LoginType = Request.Cookies["LoginId"];
            HttpCookie firstName = Request.Cookies["firstName"];
            HttpCookie loginRole = Request.Cookies["loginRole"];

            // Read the cookie information and display it.
            if (loginRole == null)
                return null;
            
            txtissuesBy.Text = firstName.Value;
           
            return "";
        }

        protected void grdOutBound_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Assuming Qty is a decimal
                int qty = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Qty"));
                totalQty += qty;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[2].Text = "Total Picked Qty";
                // Find the lblTotalQty label in the footer and set its text
                Label lblTotalQty = (Label)e.Row.FindControl("lblTotalQty");
                lblTotalQty.Text = totalQty.ToString(); // Format as needed
            }
        }
        private void GenerateBarCode()
        {
            string barcodeText = lblSlip.Text;
            Bitmap barcodeBitmap = GenerateBarcode(barcodeText);

            // Convert to byte array and display the image in an ASP.NET Image control
            using (MemoryStream ms = new MemoryStream())
            {
                barcodeBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                byte[] byteImage = ms.ToArray();
                string base64Image = Convert.ToBase64String(byteImage);
                BarcodeImage.ImageUrl = "data:image/png;base64," + base64Image;
            }
        }
        private void FillData(string id)
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from DeliveryDetails where OutSlip='" + id + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        lblSlip.Text = dt.Rows[0]["OutSlip"].ToString();
                        lblDelivery.Text = dt.Rows[0]["DelNote"].ToString();
                        lblDelAddr1.Text = dt.Rows[0]["Address"].ToString();
                        lbldelAddr2.Text = dt.Rows[0]["DelAddr2"].ToString();
                        lbldelAddr3.Text = dt.Rows[0]["DelAddr3"].ToString();
                        lbldelAddr4.Text = dt.Rows[0]["DelAddr4"].ToString();
                        lblTransName.Text = dt.Rows[0]["TransportName"].ToString();
                        lblTransMode.Text = dt.Rows[0]["TransportMode"].ToString();
                        lblRegno.Text = dt.Rows[0]["RegNo"].ToString();
                        lblSplIns.Text = dt.Rows[0]["SpecialIns"].ToString();
                        //ClientScript.RegisterStartupScript(this.GetType(), "printPage", "printPage();", true);
                        // ddlBranch.da
                    }
                }
                qry = "select HU,o.BinName,Qty,GDN,c.CustomerName,Address1,c.Address1,c.Address2,c.Address3,c.Address4 from Outbound as o inner join Customers  as c on c.CustomerCode=o.CustomerCode where o.OutSlip=" + lblSlip.Text;
                 cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    grdOutBound.DataSource = dt;
                    grdOutBound.DataBind();
                    if (dt.Rows.Count > 0)
                    {
                        lblSlip.Text = dt.Rows[0]["GDN"].ToString();

                        lblCustomer.Text = dt.Rows[0]["CustomerName"].ToString();
                        lblAddr1.Text = dt.Rows[0]["Address1"].ToString(); 
                        lblAddr2.Text = dt.Rows[0]["Address2"].ToString();
                        lblAddr3.Text = dt.Rows[0]["Address3"].ToString();
                        lblAddr4.Text = dt.Rows[0]["Address4"].ToString();
                        lblTotalQty.Text = dt.Rows.Count.ToString();
                        
                        txtIssueDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                        txtIssueTime.Text = DateTime.Now.ToString("HH:mm:ss");
                        GenerateBarCode();
                       
                        //ClientScript.RegisterStartupScript(this.GetType(), "printPage", "printPage();", true);
                        // ddlBranch.da
                    }
                }
            }
        }
        private string RenderControlToHtml(Control control)
        {
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    control.RenderControl(htw);
                    return sw.ToString();
                }
            }
        }
        private void printPDF()
        {
            img.ImageUrl = "C:/Development2/LTG/LTG/assets/img/ltgpanel_new2.png";
            string htmlContent = RenderControlToHtml(divPrint);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                Document document = new Document();
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                using (StringReader sr = new StringReader(htmlContent))
                {
                    HTMLWorker htmlparser = new HTMLWorker(document);
                    htmlparser.Parse(sr);
                }

                document.Close();
                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();

                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment; filename=WebPage.pdf");
                Response.Buffer = true;
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite(bytes);
                Response.End();
            }
        }
    }
}