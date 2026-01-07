
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;

namespace LTG
{
    public partial class StockAccConfirm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                loadGrid(false);
            }
        }
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (hfUserConfirmed.Value == "true")
            {
                save();
            }
        }
        public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
        {
            // Required for rendering
        }
        private string StripHtml(string input)
        {


            input = input.Replace("&nbsp;", "");
            return Regex.Replace(input, "<.*?>", String.Empty).Trim();
        }
        private string RenderControlToHtml(System.Web.UI.Control control)
        {
            StringWriter stringWriter = new StringWriter();
            HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);
            control.RenderControl(htmlWriter);
            return stringWriter.ToString();
        }
        private void ExportDivContentToExcel1()
        {

            string divContent = RenderControlToHtml(grdBin);

            // Use a regular expression to extract the table rows and cells
            string[] rows = Regex.Split(divContent, "<tr>|</tr>", RegexOptions.IgnoreCase);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // Create a new Excel package
            using (ExcelPackage excel = new ExcelPackage())
            {
                // Add a new worksheet
                var worksheet = excel.Workbook.Worksheets.Add("Sheet1");
                string logoPath = Server.MapPath("~/assets/img/ltgpanel_new2.png"); // Replace with your logo path
                var logo = worksheet.Drawings.AddPicture("Logo", new FileInfo(logoPath));
                logo.SetPosition(1, 0, 0, 0); // Adjust the position as needed
                logo.SetSize(350, 120); // Adjust the size as needed

                // Add welcome line
                worksheet.Cells[1, 2].Value = "Stock variance Report";
                worksheet.Cells[1, 2].Style.Font.Bold = true;
                worksheet.Cells[1, 2].Style.Font.Size = 14;
                worksheet.Cells[1, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                // Merge cells for the welcome line
                worksheet.Cells[1, 2, 1, 8].Merge = true; // Adjust the range as needed

                int rowNumber = 10;
                int i = 0;

                foreach (string row in rows)
                {
                    i += 1;
                    if (i > 0)
                    {
                        if (string.IsNullOrWhiteSpace(row)) continue;

                        string[] cells = Regex.Split(row, "<td>|</td>|<th>|</th>", RegexOptions.IgnoreCase);
                        int colNumber = 1;
                        bool isHeaderRow = false;
                        bool isAltHeaderRow = false;
                        int movecolumn = 0;
                        int j = 0;
                        foreach (string cell in cells)
                        {
                            j++;

                            if (string.IsNullOrWhiteSpace(cell)) continue;


                            //else
                            //{
                            //    if(i ==1)
                            //    {

                            //        if (j == 13 || j == 15 || j == 19 || j == 23 || j == 26)
                            //            continue;
                            //        if (j > 1 && j < 7)
                            //            continue;                                  

                            //    }

                            //}


                            var cellTrim = StripHtml(cell);
                            if (cellTrim.Contains("Total For"))
                            {
                                colNumber--;
                            }

                            var excelCell = worksheet.Cells[rowNumber, colNumber];

                            if (colNumber != 4 && double.TryParse(cellTrim, out double numericValue))
                            {
                                excelCell.Value = numericValue;  // Set cell value as a number
                                //excelCell.Style.Numberformat.Format = "#,##0.00"; // Optional: format for numbers
                            }
                            else
                            {
                                excelCell.Value = cellTrim; // Fallback if parsing fails
                            }
                            //if (colNumber == 11 || colNumber == 8 || colNumber == 9 || colNumber == 10)
                            //{
                            //    excelCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            //    excelCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFFF00"));
                            //}
                            //if (colNumber == 14 || colNumber == 12 || colNumber == 15 || colNumber == 13)
                            //{
                            //    excelCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            //    excelCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#00B050"));
                            //}
                            //if (colNumber == 17 || colNumber == 18 || colNumber == 16 || colNumber == 19)
                            //{
                            //    excelCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            //    excelCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#92D050"));
                            //}
                            //excelCell.bac
                            // Apply border to the cell
                            var border = excelCell.Style.Border;
                            border.Top.Style = ExcelBorderStyle.Thin;
                            border.Left.Style = ExcelBorderStyle.Thin;
                            border.Right.Style = ExcelBorderStyle.Thin;
                            border.Bottom.Style = ExcelBorderStyle.Thin;
                            if (i == 1)
                            {
                                isHeaderRow = true;
                            }
                            if (i == 2)
                            {
                                isAltHeaderRow = true;
                            }
                            colNumber++;
                        }
                        if (isAltHeaderRow)
                        {
                            var headerRow = worksheet.Cells[rowNumber, 1, rowNumber, colNumber - 1];
                            headerRow.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            headerRow.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#ABABAB"));
                        }
                        if (isHeaderRow)
                        {
                            var headerRow = worksheet.Cells[rowNumber, 1, rowNumber, colNumber - 1];
                            headerRow.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            headerRow.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#3091c9"));
                        }

                        rowNumber++;
                    }
                }
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                // Set the content type to Excel
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=VarianceReport.xlsx");

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();

                    Response.End();
                }

                //Response.Redirect("UserMaintenance.aspx");
            }
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            //if (!ValidateStockMaster())
            //{
            //    // Show a confirmation dialog to the user
            //    //ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Save operation not allowed. Stock Take already in progress.');", true);
            //   // return;
            //}
            popup.Visible = true;
            // If validation passes, proceed with save
           // save();
        }
        protected void btnClose_Click(object sender, EventArgs e)
        {
            popup.Visible = false;
        }

        protected void btnSaveDate_Click(object sender, EventArgs e)
        {

            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string checkCountQry = "";

                // Check if the table has any records
                checkCountQry = "SELECT COUNT(*) FROM StockTakeSetup where Password='" + txtSupPassword.Text + "'";
                SqlCommand checkCmd = new SqlCommand(checkCountQry, con);
                int recordCount = (int)checkCmd.ExecuteScalar();


                if (recordCount == 0)
                {
                    string script = "alert(\"Password is wrong, Please setup the new password\");";
                    ScriptManager.RegisterStartupScript(this, GetType(),
                                          "ServerControlScript", script, true);
                    txtSupPassword.Text = "";
                    return;
                }
            }
            txtSupPassword.Text = "";
            popup.Visible = false;
            save();
        }

        protected void btnCancelDate_Click(object sender, EventArgs e)
        {
            popup.Visible = false;
        }
        protected void txtStock_TextChanged(object sender, EventArgs e)
        {
            loadGrid(false);
        }

        protected void imgStock_Click(object sender, ImageClickEventArgs e)
        {
            loadGrid(false);
        }
        private bool ValidateStockMaster()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            string validationQuery = "SELECT COUNT(*) FROM BinCounterInstruction WHERE isnull(Completed,0) = 0";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(validationQuery, conn))
                {
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count == 0; // Return true if no rows with Completed = 0
                }
            }
        }
        protected void grdBin_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        private void loadGrid(bool variance)
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd;
            con.Open();
            string qry = "select CASE WHEN ActualBin <> Isnull(ManagerCountedBin,CountedBin) THEN 'Yes'  ELSE 'No'   END AS Variance ,ActualBin,CountedBin,ManagerCountedBin,HU,CountedBy,MCountedBy from BinAccuracyCount Where StockTakeCompleted is null";
            if (txtStock.Text != "")
                qry += " and HU like '%" + txtStock.Text + "%'";
            if(variance)
            {
                qry += " and ActualBin <> Isnull(ManagerCountedBin,CountedBin)";
            }
            cmd = new SqlCommand(qry, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            con.Close();
            grdBin.DataSource = ds.Tables[0];
            grdBin.DataBind();

        }
        private string GenerateGRN()
        {
            string dtMonth = DateTime.Now.ToString("dd-MM-yy").Replace("-", "");
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "select count(distinct(RefNo))+1 FROM BinAccuracyHistory";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        string output = FormatNumberWithLeadingZeros(dt.Rows[0][0].ToString(), 9);
                        return "STAH-" + dtMonth + "-" + output;
                        
                    }
                    
                }
            }
            string output1 = FormatNumberWithLeadingZeros("0", 9);
            return "STAH-" + dtMonth + "-" + output1;
        }
        static string FormatNumberWithLeadingZeros(string number, int totalLength)
        {
            return number.PadLeft(totalLength, '0');
        }
        protected void grdBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void imgExcel_Click(object sender, ImageClickEventArgs e)
        {
            ExportDivContentToExcel1();
        }
        private void save()
        {
            loadGrid(true);
            string connectionString = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;
            HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");

            var userName = hdUserName.Value;
            string RefNo = GenerateGRN();
            // SQL Insert Query
            //string query = "INSERT INTO StocktakeAdjustment(CustomerCode, ContainerId, Bin, HU, QtyOnSystem,Currentdate,STN,Counter1Qty,ManagerQty,FinalQty,Variance,ConfirmedBy,RefNo) VALUES (@CustomerCode, @ContainerId, @Bin, @HU, @Qty,getdate(),@STN,@CQty,@MQty,@FQty,@Variance,'" + userName +"','"+ RefNo + "')";
            //// string query1 = "INSERT INTO StocktakeCounterInstruction (CustomerCode, ContainerId, Bin, HU, Qty,Currentdate,NoofCount,Completed,CreatedBy,QtyOnHand) VALUES (@CustomerCode, @ContainerId, @Bin, @HU, @Qty,getdate(),1,0,'" + userName + "',@Qty)";
            ////string query1 = "INSERT INTO Inbound(CustomerCode, ContainerId, DefaultBin, HU, Qty,DateTimeofScan,Completed,IsStockAdj) VALUES (@CustomerCode, @ContainerId, @Bin, @HU, @Qty,getdate(),1,1)";
            ////string query2 = "INSERT INTO WarehouseProcess(BranchId,CustomerCode, Bin, HU, QtyIn,ScannedInTime,Completed,IsStockAdj) VALUES (1,@CustomerCode, @Bin, @HU, @Qty,getdate(),1,1)";
            
            string query4 = "Update Warehouseprocess set IsStockAcc=1,Bin=@Bin where HU=@HU";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Iterate through GridView rows
                foreach (GridViewRow row in grdBin.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        // Extract data from GridView row
                        
                        string bin = row.Cells[2].Text;
                        string mbin = row.Cells[3].Text;
                        string hu = row.Cells[4].Text;
                        string abin = row.Cells[1].Text;

                        // Execute Insert Command
                        if (mbin== "&nbsp;" || mbin =="")
                        {
                            mbin = bin;
                        }
                        if(mbin == "&nbsp;" || mbin == "")
                        {
                            mbin = abin;
                        }
                        
                       
                        using (SqlCommand cmd = new SqlCommand(query4, conn))
                        {
                            cmd.Parameters.AddWithValue("@HU", hu);
                            cmd.Parameters.AddWithValue("@Bin", mbin);

                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                
                }
            loadGrid(false);
            string query3 = "Update BinAccuracyHistory set RefNo='" + RefNo + "',StockTakeCompleted=1 where RefNo is null;Delete From BinAccuracyMaster;Update BinCounterInstruction set StockTakeCompleted=1;";
            string query5 = "Update Inbound set StockAccuracyCompleted=1 where HU=@HU;Update BinAccuracyCount set RefNo='" + RefNo + "', StockTakeCompleted=1,ConfirmedBy='" + userName + "',ConfirmedDate=getdate() where Hu=@hu;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                foreach (GridViewRow row in grdBin.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        // Extract data from GridView row

                        string bin = row.Cells[2].Text;
                        string mbin = row.Cells[3].Text;
                        string hu = row.Cells[4].Text;
                        string abin = row.Cells[1].Text;
                        // Iterate through GridView rows

                        using (SqlCommand cmd = new SqlCommand(query5, conn))
                        {
                            cmd.Parameters.AddWithValue("@HU", hu);
                            cmd.ExecuteNonQuery();

                        }
                    }
                }
                using (SqlCommand cmd = new SqlCommand(query3, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }

            // Notify user of success
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Stock Take Accuracy Process Completed Successfully!');", true);
            // var id = hdnBin.Value;
            CallExecutable(RefNo.ToString());
            SendEmailWithAttachment(RefNo.ToString());
            string relativeUrl = "Delivery.aspx?source=3&id=" + RefNo;

            // Register the JavaScript to open a new window with the relative URL
            string script1 = $"openNewWindow('{ResolveUrl(relativeUrl)}');";
            ClientScript.RegisterStartupScript(this.GetType(), "openNewWindowScript", script1, true);
            loadGrid(false);
        }
        public void CallExecutable(string id)
        {
            // Specify the path to the executable
            //string exePath = @"C:\DEVELOPMENT\LTGStockConfirmReport\bin\Debug\LTGStkConfirm.exe";
            string exePath = @"C:\LTG\LTGStockAccConfirmReport\LTGStkAccConfirm.exe";
           // string exePath = @"C:\DEVELOPMENT\LTGStockAccConfirmReport\bin\Release\LTGStkAccConfirm.exe";

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
        public void SendEmailWithAttachment(string id)
        {
            // Specify the sender's and receiver's email addresses
            string fromEmail = "noreply@ltgfreight.co.za";
            // string toEmail = "aswini@codex-it.co.za";
            //string fileName = GetPdfFileName(id);
            // Specify the subject and body of the email
            string subject = "LTG Stock Accuracy Confirm -" + id;
            //string body = "This is the body of the email.";
            string body = "<b>Hello!  </b>, <br> <br>";
            body += "Kindly find attached a copy of the stock take accuracy report.";
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
                //mail.To.Add("tbusang@ltgfreight.co.za");
                //mail.To.Add("vz3dispatch@ltgfreight.co.za");
                //mail.To.Add("tmabena@ltgfreight.co.za");
                //mail.To.Add("vz3receiving@ltgfreight.co.za");
                //mail.To.Add("tngamthewe@ltgfreight.co.za");
                //mail.To.Add("tsetshedi@ltgfreight.co.za");
                string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();
                    string qry = "Select * from MailSetup Where Type=2";
                    SqlCommand cmd1 = new SqlCommand(qry, con);
                    // HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");

                    //var userName = hdUserName.Value;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            var maillist = dt.Rows[0]["MailIds"].ToString().Split(',');
                            foreach (var item in maillist)
                            {
                                mail.To.Add(item);
                            }
                            // qry = "Update MailSetup set  MailIds='" + txtMails.Text + "',UpdatedDate=Getdate(),UpdatedBy='" + hdUserName.Value + "'  Where Type=" + ddlReturnType.SelectedValue;

                        }
                        else
                        {
                            mail.To.Add("tbusang@ltgfreight.co.za");
                            mail.To.Add("vz3dispatch@ltgfreight.co.za");
                            mail.To.Add("tmabena@ltgfreight.co.za");
                            mail.To.Add("vz3receiving@ltgfreight.co.za");
                            mail.To.Add("tngamthewe@ltgfreight.co.za");
                            mail.To.Add("tsetshedi@ltgfreight.co.za");
                        }

                    }
                }

                mail.Subject = subject;
                mail.Body = body;
                var smtp = new SmtpClient();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                MemoryStream memoryStream = new MemoryStream(GetPdfDataFromDatabase(id));
                id = id + ".pdf";
                Attachment attach1 = new Attachment(memoryStream, id);
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
                string qry = "Select Id from Outbound where OutSlip='" + documentId + "'";
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
                string qry = "Select * from StockConfirmReport where Id='" + documentId + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0 && dt.Rows[0]["Documents"] != DBNull.Value)
                    {
                        filename = "";
                        string base64String = dt.Rows[0]["Documents"].ToString();
                        byte[] fileBytes = Convert.FromBase64String(base64String);
                        return fileBytes;

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
    }
}