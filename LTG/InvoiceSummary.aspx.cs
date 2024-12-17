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
    public partial class InvoiceSummary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    lblHeader.Text = "Reconciliation Report - HU- For Date Range Selected " + Session["frmDt"].ToString() + " To " + Session["toDt"].ToString() + " - " + Session["CustomerName"].ToString()+"(" + Session["CustomerCode"].ToString() + ")";
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
                using (SqlCommand cmd = new SqlCommand("Sp_SummaryReport", con))
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
                int rowNumber = 11;
                int i = 0;
                worksheet.Cells[10, 9].Value = "Storage";
                worksheet.Cells[10, 9].Style.Font.Bold = true;
                worksheet.Cells[10, 9].Style.Font.Size = 13;
                worksheet.Cells[10, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells[10, 9, 10, 12].Merge = true;

                var border1 = worksheet.Cells[10, 9, 10, 12].Style.Border;
                border1.Top.Style = ExcelBorderStyle.Thin;
                border1.Left.Style = ExcelBorderStyle.Thin;
                border1.Right.Style = ExcelBorderStyle.Thin;
                border1.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[10, 9, 10, 12].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[10, 9, 10, 12].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFFF00"));

                worksheet.Cells[10, 13].Value = "Inbound";
                worksheet.Cells[10, 13].Style.Font.Bold = true;
                worksheet.Cells[10, 13].Style.Font.Size = 13;
                worksheet.Cells[10, 13].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells[10, 13, 10, 15].Merge = true;

                border1 = worksheet.Cells[10, 13, 10, 15].Style.Border;
                border1.Top.Style = ExcelBorderStyle.Thin;
                border1.Left.Style = ExcelBorderStyle.Thin;
                border1.Right.Style = ExcelBorderStyle.Thin;
                border1.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[10, 13, 10, 15].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[10, 13, 10, 15].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#00B050"));

                worksheet.Cells[10, 16].Value = "Outbound";
                worksheet.Cells[10, 16].Style.Font.Bold = true;
                worksheet.Cells[10, 16].Style.Font.Size = 13;
                worksheet.Cells[10, 16].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells[10, 16, 10, 18].Merge = true;
                border1 = worksheet.Cells[10, 16, 10, 18].Style.Border;
                border1.Top.Style = ExcelBorderStyle.Thin;
                border1.Left.Style = ExcelBorderStyle.Thin;
                border1.Right.Style = ExcelBorderStyle.Thin;
                border1.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[10, 16, 10, 18].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[10, 16, 10, 18].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#92D050"));

                foreach (string row in rows)
                {
                    i += 1;
                    if (i > 1)
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
                            excelCell.Style.WrapText = false;
                            if (colNumber != 4 && double.TryParse(cellTrim, out double numericValue))
                            {
                                excelCell.Value = numericValue;  // Set cell value as a number
                                //excelCell.Style.Numberformat.Format = "#,##0.00"; // Optional: format for numbers
                            }
                            else
                            {
                                excelCell.Value = cellTrim; // Fallback if parsing fails
                            }
                            if (colNumber==11 || colNumber == 12 || colNumber == 9 || colNumber == 10)
                            {
                                excelCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                excelCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFFF00"));
                            }
                            if (colNumber == 14 || colNumber == 15 || colNumber == 13)
                            {
                                excelCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                excelCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#00B050"));
                            }
                            if (colNumber == 17 || colNumber == 18 || colNumber == 16)
                            {
                                excelCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                excelCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#92D050"));
                            }
                            //excelCell.bac
                            // Apply border to the cell
                            var border = excelCell.Style.Border;
                            border.Top.Style = ExcelBorderStyle.Thin;
                            border.Left.Style = ExcelBorderStyle.Thin;
                            border.Right.Style = ExcelBorderStyle.Thin;
                            border.Bottom.Style = ExcelBorderStyle.Thin;
                            if (i == 2)
                            {
                                isHeaderRow = true;
                            }
                            if (i == 3)
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
                Response.AddHeader("content-disposition", "attachment; filename=SummaryReport.xlsx");

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
        private void ExportGridToExcel()
        {
            DataTable dt = new DataTable();
            foreach (TableCell cell in grdDetails.HeaderRow.Cells)
            {
                dt.Columns.Add(cell.Text);
            }

            foreach (GridViewRow row in grdDetails.Rows)
            {
                dt.Rows.Add();
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    dt.Rows[dt.Rows.Count - 1][i] = row.Cells[i].Text;
                }
            }
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet1");

                ws.Cells["A1"].LoadFromDataTable(dt, true);

                // Format the header for the table
                using (ExcelRange rng = ws.Cells["A1:Z1"])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(79, 129, 189));
                    rng.Style.Font.Color.SetColor(System.Drawing.Color.White);
                }

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Charset = "";

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    pck.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }
        protected void btnExcel_Click(object sender, ImageClickEventArgs e)
        {
           // ExportGridToExcel();
            ExportDivContentToExcel1();
        }

        protected void grdDetails_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                // Create the new GridViewRow
                GridViewRow sumRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                // Create the cells and add them to the row
                TableCell mergedCell = CreateSumCell("");
                mergedCell.ColumnSpan = 8;
                mergedCell.HorizontalAlign = HorizontalAlign.Center;
                sumRow.Cells.Add(mergedCell);

                mergedCell = CreateSumCell("Storage");
                mergedCell.ColumnSpan = 4;
                mergedCell.HorizontalAlign = HorizontalAlign.Center;// Merging the first three cells
                sumRow.Cells.Add(mergedCell);

                mergedCell = CreateSumCell("Inbound");
                mergedCell.ColumnSpan = 4;  // Merging the first three cells
                mergedCell.HorizontalAlign = HorizontalAlign.Center;
                sumRow.Cells.Add(mergedCell);
                mergedCell = CreateSumCell("Outbound");
                mergedCell.ColumnSpan = 4;  // Merging the first three cells
                mergedCell.HorizontalAlign = HorizontalAlign.Center;
                sumRow.Cells.Add(mergedCell);

                // Insert the sum row at the top of the GridView
                grdDetails.Controls[0].Controls.AddAt(0, sumRow);

                // Create the new GridViewRow
                sumRow = new GridViewRow(1, 1, DataControlRowType.Header, DataControlRowState.Insert);

                // Create the cells and add them to the row
                sumRow.Cells.Add(CreateSumCell("Totals:"));
                sumRow.Cells.Add(CreateEmptyCell()); // For ContainerId
                sumRow.Cells.Add(CreateEmptyCell()); // For Kolli
                sumRow.Cells.Add(CreateEmptyCell()); // For DocDate
                sumRow.Cells.Add(CreateEmptyCell());
                sumRow.Cells.Add(CreateEmptyCell());
                sumRow.Cells.Add(CreateEmptyCell()); // For FromDate
                sumRow.Cells.Add(CreateEmptyCell()); // For ToDate
                sumRow.Cells.Add(CreateSumCell("0")); // Placeholder for StoreQty
                sumRow.Cells.Add(CreateEmptyCell()); // For NoOfDays
                sumRow.Cells.Add(CreateEmptyCell());  // Placeholder for UnitStorageCost
                sumRow.Cells.Add(CreateSumCell("0.00")); // Placeholder for TotalStorageCost
                sumRow.Cells.Add(CreateSumCell("0")); // Placeholder for QtyIn
                sumRow.Cells.Add(CreateEmptyCell());// Placeholder for QtyInRate
                sumRow.Cells.Add(CreateSumCell("0.00")); // Placeholder for TotQtyInRate
                sumRow.Cells.Add(CreateSumCell("0")); // Placeholder for QtyOut
                sumRow.Cells.Add(CreateEmptyCell());// Placeholder for QtyOutRate
                sumRow.Cells.Add(CreateSumCell("0.00")); // Placeholder for TotOutRate

                // Insert the sum row at the top of the GridView
                grdDetails.Controls[0].Controls.AddAt(1, sumRow);
            }
        }
        decimal totalPrice =0;

        protected void grdDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Accumulate the total price
                decimal price = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "StoreQty"));
                totalPrice += price;
            }
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

                // Loop through each row in the GridView
                foreach (GridViewRow row in grdDetails.Rows)
                {
                    if (row.Cells[8].Text != "&nbsp;") // Cell index 7 + 1
                        storeQty += Convert.ToDouble(row.Cells[8].Text);

                    if (row.Cells[9].Text != "&nbsp;") // Cell index 8 + 1
                        totDays += Convert.ToDouble(row.Cells[9].Text);

                    if (row.Cells[11].Text != "&nbsp;") // Cell index 10 + 1
                        totalStorageCost += Convert.ToDouble(row.Cells[11].Text);

                    if (row.Cells[12].Text != "&nbsp;") // Cell index 11 + 1
                        qtyIn += Convert.ToDouble(row.Cells[12].Text);

                    if (row.Cells[14].Text != "&nbsp;") // Cell index 13 + 1
                        totQtyInRate += Convert.ToDouble(row.Cells[14].Text);

                    if (row.Cells[15].Text != "&nbsp;") // Cell index 14 + 1
                        qtyOut += Convert.ToDouble(row.Cells[15].Text);

                    if (row.Cells[17].Text != "&nbsp;") // Cell index 16 + 1
                        totOutRate += Convert.ToDouble(row.Cells[17].Text);
                }

                // Set the calculated totals in the specified summary row
                GridViewRow sumRow = grdDetails.Controls[0].Controls[1] as GridViewRow;
                sumRow.Cells[8].Text = storeQty.ToString();           // Cell index 7 + 1
                sumRow.Cells[9].Text = totDays.ToString();            // Cell index 8 + 1
                sumRow.Cells[11].Text = totalStorageCost.ToString("F2"); // Cell index 10 + 1
                sumRow.Cells[12].Text = qtyIn.ToString();             // Cell index 11 + 1
                sumRow.Cells[14].Text = totQtyInRate.ToString("F2");  // Cell index 13 + 1
                sumRow.Cells[15].Text = qtyOut.ToString();            // Cell index 14 + 1
                sumRow.Cells[17].Text = totOutRate.ToString("F2");    // Cell index 16 + 1

            }
        }
        private TableCell CreateSumCell(string text)
        {
            TableCell cell = new TableCell();
            cell.Text = text;
            cell.HorizontalAlign = HorizontalAlign.Right;
            cell.Font.Bold = true;
            cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#3399ff");
            return cell;
        }

        private TableCell CreateEmptyCell()
        {
            TableCell cell = new TableCell();
            cell.Text = string.Empty;
            cell.Font.Bold = true;
            cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#3399ff");
            return cell;
        }
        protected void btnAddclose_Click(object sender, EventArgs e)
        {
            popupAdd.Visible = false;
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            popupAdd.Visible = true;
        }
    }
}