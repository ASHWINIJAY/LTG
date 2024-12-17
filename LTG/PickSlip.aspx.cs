using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZXing;
using ZXing.Common;

namespace LTG
{
    public partial class PickSlip : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (getLoginType() == null)
                Response.Redirect("Login");
            if (Request.QueryString["Id"] != null)
            {
                var id = Request.QueryString["Id"].ToString();
                FillData(id);
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
                e.Row.Cells[3].Text = "Total Qty Of HU Picked";
                // Find the lblTotalQty label in the footer and set its text
                Label lblTotalQty = (Label)e.Row.FindControl("lblTotalQty");
                lblTotalQty.Text = totalQty.ToString(); // Format as needed
                lblTotal.Text = totalQty.ToString();
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
                string qry = "";
                lblSlip.Text = id;
                qry = "select * from PickingProcess where PickNumber='" + lblSlip.Text + "' order by CreatedDate ";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    grdOutBound.DataSource = dt;
                    grdOutBound.DataBind();
                    if (dt.Rows.Count > 0)
                    {

                        lblCustomer.Text = dt.Rows[0]["CustomerName"].ToString();


                        txtIssueDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                        txtIssueTime.Text = DateTime.Now.ToString("HH:mm:ss");
                        GenerateBarCode();
                        ClientScript.RegisterStartupScript(this.GetType(), "printPage", "printPage();", true);
                        // ddlBranch.da
                    }
                }
            }
        }
    }
}