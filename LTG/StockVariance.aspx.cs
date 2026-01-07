
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

namespace LTG
{
    public partial class StockVariance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadGrid();
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
            if (!ValidateStockMaster())
            {
                // Show a confirmation dialog to the user
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Save operation not allowed. Stock Take already in progress.');", true);
                return;
            }

            // If validation passes, proceed with save
            save();
        }
        protected void txtStock_TextChanged(object sender, EventArgs e)
        {
            loadGrid();
        }

        protected void imgStock_Click(object sender, ImageClickEventArgs e)
        {
            loadGrid();
        }
        private bool ValidateStockMaster()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            string validationQuery = "SELECT COUNT(*) FROM StockMaster WHERE Completed = 0";

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
        private void loadGrid()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd;
            con.Open();
            string qry = "select QtyOnHand-Isnull(ManagerQty,Qty) as variance,ContainerId,Bin,HU,QtyOnHand,Qty,ManagerQty,AllocatedTo,STN from StocktakeCounterInstruction Where Completed=1 and StockTakeCompleted is null";
            if (txtStock.Text != "")
                qry += " and HU like '%" + txtStock.Text + "%'";
            cmd = new SqlCommand(qry, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            con.Close();
            grdBin.DataSource = ds.Tables[0];
            grdBin.DataBind();

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
            string connectionString = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;
            HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");

            var userName = hdUserName.Value;
            // SQL Insert Query
            string query = "INSERT INTO StockMaster (CustomerCode, ContainerId, Bin, HU, Qty,Currentdate,NoofCount,Completed,CreatedBy) VALUES (@CustomerCode, @ContainerId, @Bin, @HU, @Qty,getdate(),1,0,'" + userName + "')";
            string query1 = "INSERT INTO StocktakeCounterInstruction (CustomerCode, ContainerId, Bin, HU, Qty,Currentdate,NoofCount,Completed,CreatedBy,QtyOnHand) VALUES (@CustomerCode, @ContainerId, @Bin, @HU, @Qty,getdate(),1,0,'" + userName + "',@Qty)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Iterate through GridView rows
                foreach (GridViewRow row in grdBin.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        // Extract data from GridView row
                        string customerCode = row.Cells[1].Text;
                        string containerId = row.Cells[2].Text;
                        string bin = row.Cells[3].Text;
                        string hu = row.Cells[4].Text;
                        string qty = row.Cells[5].Text;

                        // Execute Insert Command
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@CustomerCode", customerCode);
                            cmd.Parameters.AddWithValue("@ContainerId", containerId);
                            cmd.Parameters.AddWithValue("@Bin", bin);
                            cmd.Parameters.AddWithValue("@HU", hu);
                            cmd.Parameters.AddWithValue("@Qty", qty);

                            cmd.ExecuteNonQuery();
                        }
                        using (SqlCommand cmd = new SqlCommand(query1, conn))
                        {
                            cmd.Parameters.AddWithValue("@CustomerCode", customerCode);
                            cmd.Parameters.AddWithValue("@ContainerId", containerId);
                            cmd.Parameters.AddWithValue("@Bin", bin);
                            cmd.Parameters.AddWithValue("@HU", hu);
                            cmd.Parameters.AddWithValue("@Qty", qty);

                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }

            // Notify user of success
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Stock Take Process Initiated Successfully!');", true);

        }
    }
}