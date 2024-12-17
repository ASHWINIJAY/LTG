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
using System.Web.UI.WebControls;

namespace LTG
{
    public partial class PickedExceptionReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    lblHeader.Text = "List of HUs Picked But Not Outbounded To Create Delivery Note - For Date Range Selected " + Session["frmDt"].ToString() + " To " + Session["toDt"].ToString() + " - " + Session["CustomerName"].ToString()+"(" + Session["CustomerCode"].ToString() + ")";
                    bindInvoice();
                }
            }
            catch { }


        }
        private void bindInvoice()
        {

            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Sp_PickedExceptionReport", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StartDate", Session["frmDt"].ToString());
                    cmd.Parameters.AddWithValue("@EndDate", Session["toDt"].ToString());
                    cmd.Parameters.AddWithValue("@CustomerCode", Session["CustomerCode"].ToString());

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();

                    adapter.Fill(ds);

                    grdDetails.DataSource = ds.Tables[0];
                    grdDetails.DataBind();
                }
            }

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
            string divContent = RenderControlToHtml(grdDetails);

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
                logo.SetPosition(2, 0, 3, 0); // Adjust the position as needed
                logo.SetSize(350, 120); // Adjust the size as needed

                // Add welcome line
                worksheet.Cells[1, 2].Value = lblHeader.Text;
                worksheet.Cells[1, 2].Style.Font.Bold = true;
                worksheet.Cells[1, 2].Style.Font.Size = 14;
                worksheet.Cells[1, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                // Merge cells for the welcome line
                worksheet.Cells[1, 2, 1, 15].Merge = true; // Adjust the range as needed

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
                            excelCell.Value = cellTrim;
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
                // Set the content type to Excel
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=PickedExceptionReport.xlsx");

                // Write the file to the response
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }

        protected void btnExcel_Click(object sender, ImageClickEventArgs e)
        {
            ExportDivContentToExcel1();
        }

        protected void grdDetails_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                // Create the new GridViewRow
                GridViewRow sumRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                // Create the cells and add them to the row
                sumRow.Cells.Add(CreateSumCell("Totals:"));
                sumRow.Cells.Add(CreateEmptyCell()); // For ContainerId
                sumRow.Cells.Add(CreateEmptyCell()); // For Kolli
                sumRow.Cells.Add(CreateEmptyCell());
                sumRow.Cells.Add(CreateEmptyCell()); // For DocDate
                sumRow.Cells.Add(CreateEmptyCell()); // For FromDate
                sumRow.Cells.Add(CreateSumCell("0")); // Placeholder for StoreQty
                sumRow.Cells.Add(CreateEmptyCell());
                sumRow.Cells.Add(CreateEmptyCell());
                sumRow.Cells.Add(CreateEmptyCell());
                sumRow.Cells.Add(CreateSumCell("0"));

                // Insert the sum row at the top of the GridView
                grdDetails.Controls[0].Controls.AddAt(0, sumRow);
            }
        }
        private TableCell CreateSumCell(string text)
        {
            TableCell cell = new TableCell();
            cell.Text = text;
            cell.HorizontalAlign = HorizontalAlign.Left;
            cell.Font.Bold = true;
            cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#3399ff");
            return cell;
        }

        private TableCell CreateEmptyCell()
        {
            TableCell cell = new TableCell();
            cell.Text = string.Empty;
            cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#3399ff");
            return cell;
        }
        protected void grdDetails_DataBound(object sender, EventArgs e)
        {
            if (grdDetails.Rows.Count > 0)
            {
                double storeQty = 0;
                double totalStorageCost = 0;
                double qtyIn = 0;
                double totQtyInRate = 0;
                double qtyOut = 0;
                double totOutRate = 0;
                double totDays = 0;

                foreach (GridViewRow row in grdDetails.Rows)
                {
                    if (row.Cells[6].Text != "&nbsp;")
                        storeQty += Convert.ToDouble(row.Cells[6].Text);
                }

                GridViewRow sumRow = grdDetails.Controls[0].Controls[0] as GridViewRow;
                sumRow.Cells[6].Text = storeQty.ToString();
            }
        }
    }
}