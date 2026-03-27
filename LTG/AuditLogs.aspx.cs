using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace LTG
{
    public partial class AuditLogs : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindActionDropdown();
            }
        }
        private void BindActionDropdown()
        {
            ddlActionType.Items.Clear();

            ddlActionType.Items.Add(new ListItem("-- All --", ""));

            ddlActionType.Items.Add("Insert Return Reason");
            ddlActionType.Items.Add("Update Return Reason");
            ddlActionType.Items.Add("Update Delivery Fee");
            ddlActionType.Items.Add("Update Transport Fee");
            ddlActionType.Items.Add("Bin To Bin");
            ddlActionType.Items.Add("GRN Return");
            ddlActionType.Items.Add("GDN Return");
            ddlActionType.Items.Add("Pick Return");
            ddlActionType.Items.Add("User Creation");
            ddlActionType.Items.Add("User Update");
            ddlActionType.Items.Add("User Roles Updation");
            ddlActionType.Items.Add("MonthEnd Updation");
            ddlActionType.Items.Add("Regenerate DN");
            ddlActionType.Items.Add("RePrint DN");
            ddlActionType.Items.Add("Fee Changes");
            ddlActionType.Items.Add("HU Actions");
            ddlActionType.Items.Add("Customer Updates");
            ddlActionType.Items.Add("System Settings");
            ddlActionType.Items.Add("Other");
        }
        private void bindCustomer()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();

                string qry = @"
        SELECT *
        FROM AuditLogs
        WHERE CAST(ModifiedByDate AS DATE) BETWEEN @FromDate AND @ToDate
        ";

                // ✅ Add dropdown filter
                if (!string.IsNullOrEmpty(ddlActionType.SelectedValue))
                {
                    qry += @"
            AND (
                CASE 
                    WHEN LOWER(Custom) LIKE '%insert%return reason%' THEN 'Insert Return Reason'
                    WHEN LOWER(Custom) LIKE '%update%return reason%' THEN 'Update Return Reason'
                    WHEN LOWER(Custom) LIKE '%delivery fee%' THEN 'Update Delivery Fee'
                    WHEN LOWER(Custom) LIKE '%transport fee%' THEN 'Update Transport Fee'
                    WHEN LOWER(Custom) LIKE '%bin to bin%' THEN 'Bin To Bin'
                    WHEN LOWER(Custom) LIKE '%grn%' AND LOWER(Custom) LIKE '%return%' THEN 'GRN Return'
                    WHEN LOWER(Custom) LIKE '%gdn%' AND LOWER(Custom) LIKE '%return%' THEN 'GDN Return'
                    WHEN LOWER(Custom) LIKE '%pick%' AND LOWER(Custom) LIKE '%return%' THEN 'Pick Return'
                    WHEN [Table] = 'Users' AND LOWER(Custom) LIKE '%create%' THEN 'User Creation'
                    WHEN [Table] = 'Users' AND LOWER(Custom) LIKE '%update%' THEN 'User Update'
                    WHEN LOWER(Custom) LIKE '%role%' THEN 'User Roles Updation'
                    WHEN [Table] = 'MonthEnd' THEN 'MonthEnd Updation'
                    WHEN LOWER(Custom) LIKE '%regenerate%' THEN 'Regenerate DN'
                    WHEN LOWER(Custom) LIKE '%reprint%' THEN 'RePrint DN'
                    WHEN LOWER(Custom) LIKE '%fee%' THEN 'Fee Changes'
                    WHEN LOWER(Custom) LIKE '%hu%' THEN 'HU Actions'
                    WHEN [Table] = 'Customers' THEN 'Customer Updates'
                    WHEN [Table] IN ('ReminderMails', 'UOPMaster') THEN 'System Settings'
                    ELSE 'Other'
                END = @Filter
            )";
                }

                SqlCommand cmd = new SqlCommand(qry, con);

                // ✅ Safe parameters
                cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(txtFrmDate.Text));
                cmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(txtToDate.Text));

                if (!string.IsNullOrEmpty(ddlActionType.SelectedValue))
                {
                    cmd.Parameters.AddWithValue("@Filter", ddlActionType.SelectedValue);
                }

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    grdCustomer.DataSource = dt;
                    grdCustomer.DataBind();
                }
            }
        }
        protected void txtSearchContainer_TextChanged(object sender, EventArgs e)
        {
            bindCustomer();
        }

        protected void imgSearch_Click(object sender, ImageClickEventArgs e)
        {
            bindCustomer();
        }

        protected void grdCustomer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string customerCode = e.CommandArgument.ToString();
            if (e.CommandName == "SelectImage")
                Response.Redirect("BinCreate.aspx?Code=" + customerCode);
            else
            {
                if (checkCustomer(customerCode))
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('You cannot delete as this Bin has already been used and has history');", true);

                }
                else
                {
                    DeleteCustomer(customerCode);
                }
            }

        }
        private bool checkCustomer(string cusCode)
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from WarehouseProcess where Bin='" + cusCode + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        return true;

                        // ddlBranch.da
                    }
                }
            }
            return false;
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


        public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
        {
            // Required for rendering
        }
        private void ExportDivContentToExcel1()
        {
          
            // Render the div content to a string
            string divContent = RenderControlToHtml(grdCustomer);

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
                worksheet.Cells[1, 2].Value = "Audit Logs";
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
                Response.AddHeader("content-disposition", "attachment; filename=AuditLogs.xlsx");
              
                // Write the file to the response
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

        private void DeleteCustomer(string cusCode)
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "delete  from Bin where BinCode='" + cusCode + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                cmd1.ExecuteNonQuery();
                HiddenField hdLoginId = (HiddenField)this.Master.FindControl("hdnLoginId");

                var userid = hdLoginId.Value;
                HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");

                var userName = hdUserName.Value;
                qry = "Insert into AuditLogs([Table],Field,Custom,ModifiedByUserId,ModifiedByUserName,ModifiedByDate)values('Bin','Delete','Delete the Bin: " + cusCode + " ','" + hdLoginId.Value + "','" + userName + "',getdate())";
                cmd1 = new SqlCommand(qry, con);
                cmd1.ExecuteNonQuery();
                Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Bin deleted successfully.');", true);

                bindCustomer();
            }

        }
        protected void grdCustomer_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        protected void imgExcel_Click(object sender, ImageClickEventArgs e)
        {
            ExportDivContentToExcel1();
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            bindCustomer();
        }
    }
}