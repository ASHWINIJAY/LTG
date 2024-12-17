using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LTG
{
    public partial class DetailedReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindCustomer();
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            if (ddlCustomer.SelectedIndex == 0)
            {
                string script1 = "alert(\"Please select customer\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script1, true);
                return;
            }
            string frmDt = Convert.ToDateTime(txtFrmDate.Text).ToString("dd/MMM/yyyy");
            string toDt = Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy");
            Session["frmDt"] = frmDt;
            Session["toDt"] = toDt;
            Session["CustomerCode"] = ddlCustomer.SelectedValue;
            Session["CustomerName"] = ddlCustomer.SelectedItem.Text;
            //string relativeUrl = "Invoice.aspx";
            string relativeUrl = "InvoiceDetailedReport.aspx";

            // Register the JavaScript to open a new window with the relative URL
            string script = $"openNewWindow('{ResolveUrl(relativeUrl)}');";
            ClientScript.RegisterStartupScript(this.GetType(), "openNewWindowScript", script, true);

        }
        private void bindCustomer()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from Customers";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        ddlCustomer.DataSource = dt;
                        ddlCustomer.DataBind();
                        ddlCustomer.DataTextField = "CustomerName";
                        ddlCustomer.DataValueField = "CustomerCode";
                        ddlCustomer.DataBind();
                        // ddlBranch.da
                    }
                    ddlCustomer.Items.Insert(0, new ListItem("Select customer", ""));
                }
            }
        }

    }
}