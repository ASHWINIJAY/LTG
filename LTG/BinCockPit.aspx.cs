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
    public partial class BinCockPit : System.Web.UI.Page
    {
        protected void grdBin_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");
                string hu = grdBin.DataKeys[e.Row.RowIndex].Value.ToString(); // Use HU as the key

                if (ViewState["CheckedBoxes"] is List<string> checkedBoxes && checkedBoxes.Contains(hu))
                {
                    chkSelect.Checked = true;
                }
                
                    //GridViewRow row = grdBin.Rows[e.Row.RowIndex];
                    //Label lblBin = (Label)e.Row.FindControl("lblBin"); ; // Use HU as the key

                    if (ViewState["CheckedBoxesBin"] is List<string> checkedBoxes1 && checkedBoxes1.Contains(hu))
                    {
                        chkSelect.Checked = true;
                    }
                
            }
        }
        private void loadUsers()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd;
            con.Open();
            string qry = "select * from Users as U inner join UserRoles as R on R.Username=U.Username Where R.StockRoles=0";
            if (txtStock.Text != "")
                qry += " and U.Username like'%" + txtStock.Text + "%'";

            cmd = new SqlCommand(qry, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            con.Close();
            GridView1.DataSource = ds.Tables[0];
            GridView1.DataBind();

        }
        private List<string> GetSelectedHUs()
        {
            List<string> selectedHUs = new List<string>();

            foreach (GridViewRow row in grdBin.Rows)
            {
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect != null && chkSelect.Checked)
                {
                    string hu = grdBin.DataKeys[row.RowIndex].Value.ToString();
                    selectedHUs.Add(hu);
                }
            }

            return selectedHUs;
        }
        private DataTable loadData()
        {
            SaveCheckboxStates();
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;
           
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd;
            con.Open();
            string qry = "select Distinct Bin,CustomerCode from BinAccuracyMaster where Allocated is null";
           
            cmd = new SqlCommand(qry, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            con.Close();
           return ds.Tables[0];
            

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
            grdBin.Columns[0].Visible = false;
            // Render the div content to a string
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
                worksheet.Cells[1, 2].Value = "List of Bins";
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
                Response.AddHeader("content-disposition", "attachment; filename=CockBit.xlsx");
                grdBin.Columns[0].Visible = true;
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

        private void loadGrid()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;
            SaveCheckboxStates();
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd;
            con.Open();
            string qry = "select Distinct Bin,CustomerCode from BinAccuracyMaster where Allocated is null ";
            if (txtSearchContainer.Text != "")
                qry += " and (Bin like'" + txtSearchContainer.Text + "%')";
            txtSearchContainer.Text = "";
            cmd = new SqlCommand(qry, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            con.Close();
            grdBin.DataSource = ds.Tables[0];
            grdBin.DataBind();
            
        }
        private void loadBinGrid()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;
          //  SaveCheckboxStates();
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd;
            con.Open();
            string qry = "select Distinct Bin,CustomerCode from BinAccuracyMaster where Allocated is null ";
            if (txtBins.Text != "")
                qry += " and (Bin like'" + txtBins.Text + "%')";
            txtBins.Text = "";
            cmd = new SqlCommand(qry, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            con.Close();
            grdBins.DataSource = ds.Tables[0];
            grdBins.DataBind();

        }
        protected void SaveCheckboxStates()
        {
            List<string> checkedBoxes = ViewState["CheckedBoxes"] as List<string> ?? new List<string>();

            foreach (GridViewRow row in grdBin.Rows)
            {
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect != null)
                {
                    string hu = grdBin.DataKeys[row.RowIndex].Value.ToString();

                    if (chkSelect.Checked && !checkedBoxes.Contains(hu))
                    {
                        checkedBoxes.Add(hu); // Add checked items
                    }
                    else if (!chkSelect.Checked && checkedBoxes.Contains(hu))
                    {
                        checkedBoxes.Remove(hu); // Remove unselected rows
                    }

                }
            }

            ViewState["CheckedBoxes"] = checkedBoxes;
        }
        protected void SaveCheckboxStatesBin()
        {
            List<string> checkedBoxes = ViewState["CheckedBoxesBin"] as List<string> ?? new List<string>();

            foreach (GridViewRow row in grdBins.Rows)
            {
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect != null)
                {
                    string hu = grdBins.DataKeys[row.RowIndex].Value.ToString();

                    if (chkSelect.Checked && !checkedBoxes.Contains(hu))
                    {
                        checkedBoxes.Add(hu); // Add checked items
                    }
                    else if (!chkSelect.Checked && checkedBoxes.Contains(hu))
                    {
                        checkedBoxes.Remove(hu); // Remove unselected rows
                    }

                }
            }

            ViewState["CheckedBoxesBin"] = checkedBoxes;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadGrid();
                loadUsers();
            }
            
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            popupAdd.Visible = true;
        }
        protected void grdBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void imgExcel_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void txtSearchContainer_TextChanged(object sender, EventArgs e)
        {
            loadGrid();
        }

        protected void imgSearch_Click(object sender, ImageClickEventArgs e)
        {
            loadGrid();
        }

        protected void btnAddclose_Click(object sender, EventArgs e)
        {
            popupAdd.Visible = false;
        }

        protected void btnAddSave_Click(object sender, EventArgs e)
        {

        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            AllocateStock();
        }
        protected void AllocateStock()
        {
            loadGrid();
            GridViewRow selectedRow = GridView1.SelectedRow;
            if (selectedRow == null)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('No row selected in GridView1.');", true);
                return;
            }

            string userName = selectedRow.Cells[1].Text;
            HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");
            if (hdUserName == null || string.IsNullOrEmpty(hdUserName.Value))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Hidden username not found or invalid.');", true);
                return;
            }

            string userName1 = hdUserName.Value;
            string connectionString = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            // Parameterized queries to prevent SQL injection
            string query = "UPDATE BinAccuracyMaster SET AllocatedTo = @UserName,Allocated=1  WHERE Bin = @Bin";
            string query1 = "insert into BinCounterInstruction(Bin,CurrentDate,CustomerCode,Allocated , AllocatedTo, AllocatedBy, AllocatedDate )Values(@Bin,Getdate(),@customer,1,@Username,@AllocatedBy,Getdate())";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                foreach (GridViewRow row in grdBin.Rows)
                {
                    CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                    if (chkSelect != null && chkSelect.Checked)
                    {
                        string bin = row.Cells[2].Text;
                        string customer = row.Cells[3].Text;
                        ExecuteUpdate(conn, query, userName, userName1, bin);
                        ExecuteUpdate(conn, query1, userName, userName1, bin,customer);
                    }
                }
            }

            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Allocation Completed Successfully!');", true);
            ViewState["CheckedBoxes"] = null;
            popupAdd.Visible = false;
            loadGrid();
        }

        private void ExecuteUpdate(SqlConnection conn, string query, string userName, string allocatedBy, string bin, string customer = "")
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@AllocatedBy", allocatedBy);
                cmd.Parameters.AddWithValue("@Bin", bin);
                cmd.Parameters.AddWithValue("@Customer", customer);
                cmd.ExecuteNonQuery();
            }
        }

        protected void txtStock_TextChanged(object sender, EventArgs e)
        {
            loadUsers();
        }

        protected void imgStock_Click(object sender, ImageClickEventArgs e)
        {
            loadUsers();
        }
        private void bindselectedBin()
        {
            DataTable dt = loadData();
            List<string> selectedHUs = ViewState["CheckedBoxesBin"] as List<string> ?? new List<string>();
            //  List<string> selectedHUs = GetSelectedHUs();
            // Filter the DataTable
            DataTable filteredDt = dt.Clone(); // Clone the structure of the DataTable
            foreach (DataRow row in dt.Rows)
            {
                if (selectedHUs.Contains(row["Bin"].ToString()))
                {
                    filteredDt.ImportRow(row); // Add matching rows to the filtered DataTable
                }
            }

            // Bind the filtered data to the GridView
            grdBin.DataSource = filteredDt;
            grdBin.DataBind();
        }
        protected void btnShowAllocate_Click(object sender, EventArgs e)
        {
           

            // Retrieve the full data source (replace this with your actual data source logic)
            DataTable dt = loadData();
            List<string> selectedHUs = ViewState["CheckedBoxes"] as List<string> ?? new List<string>();
          //  List<string> selectedHUs = GetSelectedHUs();
            // Filter the DataTable
            DataTable filteredDt = dt.Clone(); // Clone the structure of the DataTable
            foreach (DataRow row in dt.Rows)
            {
                if (selectedHUs.Contains(row["Bin"].ToString()))
                {
                    filteredDt.ImportRow(row); // Add matching rows to the filtered DataTable
                }
            }

            // Bind the filtered data to the GridView
            grdBin.DataSource = filteredDt;
            grdBin.DataBind();
        }

        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            loadGrid();
        }

        protected void btnBrowseBin_Click(object sender, EventArgs e)
        {
            loadBinGrid();
            binPopup.Visible = true;
        }

        protected void btncloseBin_Click(object sender, EventArgs e)
        {
            binPopup.Visible = false;
        }

        protected void txtBins_TextChanged(object sender, EventArgs e)
        {
            loadBinGrid();
        }

        protected void imgsearchBin_Click(object sender, ImageClickEventArgs e)
        {
            loadBinGrid();
        }

        protected void grdBins_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");
                //string hu = grdBin.DataKeys[e.Row.RowIndex].Value.ToString(); // Use HU as the key
                ////string hu = grdBin.Rows[e.Row.RowIndex].Cells[3].Text.ToString(); // Use HU as the key

                //if (ViewState["CheckedBoxesBin"] is List<string> checkedBoxes && checkedBoxes.Contains(hu))
                //{
                //    chkSelect.Checked = true;
                //}
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            SaveCheckboxStatesBin();
            bindselectedBin();
            binPopup.Visible = false;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            binPopup.Visible = false;
        }
    }
}