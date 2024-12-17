
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LTG
{
    public partial class Invoice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                bindInvoice();
            }
            catch { }
           

        }
        private void bindInvoice()
        {
            lblDate.Text = Session["frmDt"].ToString() + " To " + Session["toDt"].ToString() + " - " + Session["CustomerName"].ToString()+"(" + Session["CustomerCode"].ToString() + ")";
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("InvoiceSummary", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StartDate", Session["frmDt"].ToString());
                    cmd.Parameters.AddWithValue("@EndDate", Session["toDt"].ToString());

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();

                    adapter.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lblStorageVAT.Text = Convert.ToDecimal(ds.Tables[0].Rows[0]["TotalStorageCostVAT"]).ToString("0.00");
                        lblStorageSub.Text = Convert.ToDecimal(ds.Tables[0].Rows[0]["TotalStorageCost"]).ToString("0.00");
                        lblStorageTot.Text = Convert.ToDecimal(ds.Tables[0].Rows[0]["TotalStorage"]).ToString("0.00");
                        lblinSub.Text = Convert.ToDecimal(ds.Tables[0].Rows[0]["TotalInBoundCost"]).ToString("0.00");
                        lblInVat.Text = Convert.ToDecimal(ds.Tables[0].Rows[0]["TotalInBoundVAT"]).ToString("0.00");
                        lblInTot.Text = Convert.ToDecimal(ds.Tables[0].Rows[0]["TotInbound"]).ToString("0.00");
                        lblOutSub.Text = Convert.ToDecimal(ds.Tables[0].Rows[0]["TotalOutBoundCost"]).ToString("0.00");
                        lblOutVat.Text = Convert.ToDecimal(ds.Tables[0].Rows[0]["TotalOutBoundVAT"]).ToString("0.00");
                        lblOutTot.Text = Convert.ToDecimal(ds.Tables[0].Rows[0]["TotOutbound"]).ToString("0.00");
                        lblvat.Text = Convert.ToDecimal(ds.Tables[0].Rows[0]["TotalVat"]).ToString("0.00");
                        lblSub.Text = Convert.ToDecimal(ds.Tables[0].Rows[0]["SubTotal"]).ToString("0.00");
                        lblTot.Text = Convert.ToDecimal(ds.Tables[0].Rows[0]["TotalCost"]).ToString("0.00");
                        lblInvoice.Text = lblTot.Text;
                        lblDueAmount.Text = lblTot.Text;
                        lblDueDate.Text = DateTime.Now.AddDays(7).ToString("dd/MMM/yyyy");
                        lblInvDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                        lblDueDatee.Text = lblDueDate.Text;
                    }
                    grdInbound.DataSource = ds.Tables[1];
                    grdInbound.DataBind();
                    grdOutBound.DataSource = ds.Tables[2];
                    grdOutBound.DataBind();
                    grdStorage.DataSource = ds.Tables[3];
                    grdStorage.DataBind();
                }
            }
           
        }
        private decimal totalPrice = 0m;
        private decimal totaloutPrice = 0m;
        private decimal totalstorPrice = 0m;
        protected void grdInbound_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Accumulate the total price
                decimal price = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TotalInBoundCost"));
                totalPrice += price;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                // Set the total price in the footer
                e.Row.Cells[4].Text = "Total For Inbounds";
                e.Row.Cells[5].Text = totalPrice.ToString("0.00");
            }
        }

        protected void grdOutBound_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Accumulate the total price
                decimal price = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TotalOutBoundCost"));
                totaloutPrice += price;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                // Set the total price in the footer
                e.Row.Cells[4].Text = "Total For Outbounds";
                e.Row.Cells[5].Text = totaloutPrice.ToString("0.00");
            }
        }

        protected void grdStorage_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Accumulate the total price
                decimal price = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TotalStorageCost"));
                totalstorPrice += price;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                // Set the total price in the footer
                e.Row.Cells[3].Text = "Total For Storage";
                e.Row.Cells[4].Text = totalstorPrice.ToString("0.00");
            }
        }
        private void ExportToExcel()
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Invoice.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                // Render the content of the div to the HtmlTextWriter
                divPrint.RenderControl(hw);

                // Write the rendered content to the response
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void btnPrint_Click(object sender, ImageClickEventArgs e)
        {
            divHeader.Visible = false;
            ClientScript.RegisterStartupScript(this.GetType(), "printPage", "printPage();", true);
            divHeader.Visible = true;
            
        }
        private string RenderControlToHtml(System.Web.UI.Control control)
        {
            StringWriter stringWriter = new StringWriter();
            HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);
            control.RenderControl(htmlWriter);
            return stringWriter.ToString();
        }

        private string StripHtml(string input)
        {
            

            input = input.Replace("&nbsp;", "");
            return Regex.Replace(input, "<.*?>", String.Empty).Trim();
        }

        public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
        {
            // Required for rendering
        }
        private void ExportDivContentToExcel()
        {
            divHeader.Visible = false;
            string tableContent = RenderControlToHtml(grdInbound);
           // tableContent = tableContent.Replace("<h3>InBounds:</h3>", "");

            // Create a memory stream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Create a new Excel document
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
                {
                    // Add a WorkbookPart to the document
                    WorkbookPart workbookPart = document.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();

                    // Add a WorksheetPart to the WorkbookPart
                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    worksheetPart.Worksheet = new Worksheet(new SheetData());

                    // Add Sheets to the Workbook
                    Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

                    // Append a new worksheet and associate it with the workbook
                    Sheet sheet = new Sheet()
                    {
                        Id = workbookPart.GetIdOfPart(worksheetPart),
                        SheetId = 1,
                        Name = "Sheet1"
                    };
                    sheets.Append(sheet);

                    // Get the SheetData cell table
                    SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                    // Parse the HTML table content
                    string[] rows = tableContent.Split(new string[] { "<tr>", "</tr>" }, StringSplitOptions.RemoveEmptyEntries);
                    int i = 0;
                    foreach (string row in rows)
                    {
                        i += 1;
                        if (i > 2 )
                        {
                            if (i > 24 && i<26)
                                continue;
                            Row newRow = new Row();

                            string[] cells = row.Split(new string[] { "<td>", "</td>", "<th>", "</th>" }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string cell in cells)
                            {
                                var strValue = StripHtml(cell.Trim());
                                Cell newCell = new Cell()
                                {
                                    CellValue = new CellValue(strValue),
                                    DataType = CellValues.String
                                };
                                if (strValue.Contains("All business") || strValue.Contains("Transfer Funds To:"))
                                {

                                }
                                else
                                newRow.Append(newCell);
                            }

                            sheetData.Append(newRow);
                        }
                    }

                    // Save the document
                    workbookPart.Workbook.Save();
                }

                // Set the response content type to Excel
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=Invoice.xlsx");

                // Write the file to the response
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            divHeader.Visible = true;
        }
        private void ExportDivContentToExcel1()
        {
            // Render the div content to a string
            string divContent = RenderControlToHtml(divPrint);

            // Use a regular expression to extract the table rows and cells
            string[] rows = Regex.Split(divContent, "<tr>|</tr>", RegexOptions.IgnoreCase);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // Create a new Excel package
            using (ExcelPackage excel = new ExcelPackage())
            {
                // Add a new worksheet
                var worksheet = excel.Workbook.Worksheets.Add("Sheet1");

                int rowNumber = 1;
                int i = 0;
               
                foreach (string row in rows)
                {
                    i += 1;
                    if (i > 14)
                    {
                        if (string.IsNullOrWhiteSpace(row)) continue;

                        string[] cells = Regex.Split(row, "<td>|</td>|<th>|</th>", RegexOptions.IgnoreCase);
                        int colNumber = 1;
                        bool isHeaderRow = false;
                        int movecolumn = 0;
                        foreach (string cell in cells)
                        {
                            if (string.IsNullOrWhiteSpace(cell)) continue;
                            if (cell.Contains("All business") || cell.Contains("Balance Due") || cell.Contains("Transfer Funds To:") || cell.Contains("Transfer Funds To:") || cell.Contains("Address") || cell.Contains("Invoiced ZAR"))
                            {
                                continue;
                            }
                            var cellTrim = StripHtml(cell);
                            if(cellTrim.Contains("Total For"))
                            {
                                colNumber--;
                            }
                            if(i>14 && i<26 && colNumber>1 && movecolumn ==0)
                            {
                                movecolumn = 1;
                                colNumber--;
                            }
                            var excelCell = worksheet.Cells[rowNumber, colNumber];
                            excelCell.Value = cellTrim;
                            if (i == 14)
                                excelCell.Value = "Invoice Summary";
                            // Apply border to the cell
                            var border = excelCell.Style.Border;
                            border.Top.Style = ExcelBorderStyle.Thin;
                            border.Left.Style = ExcelBorderStyle.Thin;
                            border.Right.Style = ExcelBorderStyle.Thin;
                            border.Bottom.Style = ExcelBorderStyle.Thin;
                            if (excelCell.Value.ToString()=="Description" || excelCell.Value.ToString() == "SNo")
                            {
                                isHeaderRow = true;
                            }
                            colNumber++;
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
                Response.AddHeader("content-disposition", "attachment; filename=Invoice.xlsx");

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
            divHeader.Visible = false;
            ExportDivContentToExcel1();
            divHeader.Visible = true;
            //ExportDivContentToExcel();
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {

        }
    }
}