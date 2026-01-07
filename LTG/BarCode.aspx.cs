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
    public partial class BarCode : System.Web.UI.Page
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
        private string getLoginType()
        {
            HttpCookie LoginType = Request.Cookies["LoginId"];
            HttpCookie firstName = Request.Cookies["firstName"];
            HttpCookie loginRole = Request.Cookies["loginRole"];

            // Read the cookie information and display it.
            if (loginRole == null)
                return null;

            return "";
        }
        private void FillData(string id)
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "";
                qry = "select * from Inbound where HU='" + id + "' order by CreatedDate";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        lblQty.Text = "1";
                        lblClient.Text = dt.Rows[0]["CustomerCode"].ToString();
                        lblWH.Text = dt.Rows[0]["DefaultBin"].ToString();
                        lblDate.Text = Convert.ToDateTime( dt.Rows[0]["DateTimeofScan"].ToString()).ToString("dd/MMM/yyyy HH:mm");
                        lbluser.Text = dt.Rows[0]["CreatedBy"].ToString();
                        lblPart.Text = dt.Rows[0]["PartNumber"].ToString();
                        lblCustomerName.Text = dt.Rows[0]["CustomerName"].ToString();
                        imgBarcode.ImageUrl= "data:image/png;base64,"  + dt.Rows[0]["BarcodeBase64"].ToString();

                        ClientScript.RegisterStartupScript(this.GetType(), "printPage", "printPage();", true);
                    }
                }
            }
        }
    }
}