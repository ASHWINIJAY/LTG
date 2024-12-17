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
    public partial class InBoundProcess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                bindBranch();
                bindCustomer();
                GenerateGRN();
                if (Request.QueryString["id"] != null)
                {
                    var id = Request.QueryString["id"].ToString();
                    BindIncomplete(id);
                }
                else
                    CheckIncomplete();
                    //GetContainer();
                }
        }

        private void bindBranch()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from Branch";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        ddlBranch.DataSource = dt;
                        ddlBranch.DataBind();
                        ddlBranch.DataTextField = "BranchName";
                        ddlBranch.DataValueField = "BranchId";
                        ddlBranch.DataBind();
                        // ddlBranch.da
                    }
                }
            }
        }
        private void GenerateGRN()
        {
            string dtMonth = DateTime.Now.ToString("dd-MM-yy").Replace("-", "");
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;
            txtDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "select count(distinct(GRN))+1 FROM Inbound";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        string output = FormatNumberWithLeadingZeros(dt.Rows[0][0].ToString(), 9);
                        txtGRN.Text = "GRN-" + dtMonth + "-" + output;
                        // ddlBranch.da
                    }
                }
            }

        }
        static string FormatNumberWithLeadingZeros(string number, int totalLength)
        {
            return number.PadLeft(totalLength, '0');
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
        protected void btnCusNext_Click(object sender, ImageClickEventArgs e)
        {
            if(ddlCustomer.SelectedIndex==0)
            {
                string script = "alert(\"Please select customer\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                return;
            }
            if (checkRefNo())
            {
                string script = "alert(\"Container Number Already Exisits\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                return;
            }
            
            getInboundFee();
            if (hdnInboundFee.Value == "")
            {
                string script = "alert(\"Please setup the inbound fee. \");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);

                return;
            }
            divCustomer.Visible = false;
            divScan.Visible = true;
            divHeader.InnerText = "Inbound Process for - " + ddlCustomer.SelectedItem.Text;
            txtContainer1.Text = txtContainer.Text;
            txtHU.Focus();
        }
        private bool checkRefNo()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from Inbound where ContainerId='" + txtContainer.Text + "'";
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
        private void getInboundFee()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select InboundFee,Bin from Customers where CustomerCode='" + ddlCustomer.SelectedValue + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0] != DBNull.Value)
                            hdnInboundFee.Value = dt.Rows[0][0].ToString();
                        else
                            hdnInboundFee.Value = "";
                        hdnBin.Value = dt.Rows[0][1].ToString();
                        // ddlBranch.da
                    }
                    else
                    {
                        hdnInboundFee.Value = "";
                    }
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            save();
            FillGrid();

        }
        private void save()
        {
            txtHU.Text = txtHU.Text.Trim();
            if (txtHU.Text == "")
            {
                string script = "alert(\"HU Cannot be empty\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                txtHU.Text = "";
                txtHU.Focus();
                return;
            }
           
            if (checkHU())
            {
                string script = "alert(\"HU Already Exisits\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                txtHU.Text = "";
                txtHU.Focus();
                return;
            }
            
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                HiddenField hdLoginId = (HiddenField)this.Master.FindControl("hdnLoginId");

                var userid = hdLoginId.Value;
                HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");

                var userName = hdUserName.Value;
                string qry = "Insert into Inbound(ContainerId,BranchId,BranchName,CustomerCode,CustomerName,HU,Qty,UnitInBoundCost,TotalInBoundCost,Loginname,DateTimeofScan,CreatedBy,CreatedDate,GRN,DefaultBin)values('" + txtContainer.Text + "'," + ddlBranch.SelectedValue + ",'" + ddlBranch.SelectedItem.Text + "','" + ddlCustomer.SelectedValue + "','" + ddlCustomer.SelectedItem.Text + "','" + txtHU.Text + "','" + txtQty.Text + "'," + hdnInboundFee.Value + "," + hdnInboundFee.Value + ",'" + userid + "','" +txtDate.Text+"','" + userName + "',getdate(),'" + txtGRN.Text + "','" + txtDefaultBin.Text + "')";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                cmd1.ExecuteNonQuery();                
            }
            txtHU.Text = "";
            txtHU.Focus();
        }
        protected void btnComplete_Click(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                
                string qry = "Update Inbound set Completed=1 where GRN='" + txtGRN.Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                cmd1.ExecuteNonQuery();
            }
            string redirectUrl = "GRN.aspx?id=" + txtGRN.Text; // The URL to open in a new tab

            // Inject the JavaScript confirmation dialog with a redirect in a new tab on confirmation
            string script = "var userConfirmed = confirm('Do you want to print this data?');" +
                            "if (userConfirmed) {" +
                            "    window.open('" + redirectUrl + "', '_blank');" +
                            "}";

            // Register the script to run on the client-side
            ClientScript.RegisterStartupScript(this.GetType(), "confirmRedirectNewTab", script, true);

            txtHU.Text = "";
            divCustomer.Visible = true;
            divScan.Visible = false;
            txtContainer.Text = "";
            GenerateGRN();
        }
        protected void GetContainer()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "SELECT  isnull(max([ContainerId])+1,1)  FROM Inbound";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        hdnContainer.Value = dt.Rows[0][0].ToString();
                    }
                }
            }
        }

        protected void FillGrid()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "SELECT  *  FROM Inbound where ContainerId='" + txtContainer.Text + "' order by CreatedDate";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    grdScans.DataSource = dt;
                    grdScans.DataBind();
                    divHeader.InnerText = "InBound Process - Total Scans: " + dt.Rows.Count;
                }
            }
        }
        protected void CheckIncomplete()
        {
            if(Session["LoginId"] == null)
            {
                Response.Redirect("Dashboard.aspx");
            }
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;
          
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "SELECT  *  FROM Inbound where Completed=0 and Loginname='" + Session["LoginId"].ToString() + "' order by DateTimeofScan desc";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                   if (dt.Rows.Count>0)
                    {
                        string redirectUrl = "InboundProcess.aspx?id=" + dt.Rows[0]["GRN"].ToString(); // The URL to open in a new tab

                        // Inject the JavaScript confirmation dialog with a redirect in a new tab on confirmation
                        string script = "var userConfirmed = confirm('You have existing GRN is incomplete, Do you want to continue?');" +
                                        "if (userConfirmed) {" +
                                        "    window.open('" + redirectUrl + "', '_blank');" +
                                        "}";
                        ClientScript.RegisterStartupScript(this.GetType(), "confirmRedirectNewTab", script, true);
                    }
                }
            }
        }

        protected void BindIncomplete(string GRN)
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;
            HiddenField hdLoginId = (HiddenField)this.Master.FindControl("hdnLoginId");
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "SELECT  *  FROM Inbound where Completed=0 and GRN='" + GRN + "' order by DateTimeofScan";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        ddlBranch.SelectedValue = dt.Rows[0]["BranchId"].ToString();
                        ddlCustomer.SelectedValue = dt.Rows[0]["CustomerCode"].ToString();
                        txtDate.Text = Convert.ToDateTime(dt.Rows[0]["DateTimeOfScan"]).ToString("dd/MMM/yyyy");
                        txtContainer.Text = dt.Rows[0]["ContainerId"].ToString();
                        txtGRN.Text = GRN;
                        getInboundFee();
                        divCustomer.Visible = false;
                        divScan.Visible = true;
                        divHeader.InnerText = "Inbound Process for - " + ddlCustomer.SelectedItem.Text;
                        txtContainer1.Text = txtContainer.Text;
                        txtHU.Focus();
                        FillGrid();
                    }
                }
            }
        }
        private bool checkHU()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from Inbound where HU='" + txtHU.Text + "'";
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtHU.Text = "";
        }

        protected void txtHU_TextChanged(object sender, EventArgs e)
        {
            save();
            FillGrid();
        }

        protected void txtHU_TextChanged1(object sender, EventArgs e)
        {

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();

                string qry = "Delete  from Inbound where GRN='" + txtGRN.Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                
                        cmd1.ExecuteNonQuery();
                divScan.Visible = false;
                divCustomer.Visible = true;
                FillGrid();
                        // ddlBranch.da

            }
        }

        protected void btnCancel_Click1(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();

                string qry = "Delete  from Inbound where GRN='" + txtGRN.Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);

                cmd1.ExecuteNonQuery();
                Response.Redirect("InboundProcess.aspx");
                // ddlBranch.da

            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            popup.Visible = false;
        }

        protected void btnSaveDate_Click(object sender, EventArgs e)
        {
            if(txtNewdate.Text =="")
            {
                string script = "alert(\"Please select date\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                
                return;
            }
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();

                // Check if the table has any records
                string checkCountQry = "SELECT COUNT(*) FROM Supervisor where Password='" + txtSupPassword.Text +"'";
                SqlCommand checkCmd = new SqlCommand(checkCountQry, con);
                int recordCount = (int)checkCmd.ExecuteScalar();

               
                if (recordCount == 0)
                {
                    string script = "alert(\"Password is wrong, Please reenter\");";
                    ScriptManager.RegisterStartupScript(this, GetType(),
                                          "ServerControlScript", script, true);
                    txtSupPassword.Text = "";
                    return;
                }
            }
                    txtDate.Text = txtNewdate.Text;
            txtSupPassword.Text = "";
            txtNewdate.Text = "";
            popup.Visible = false;
        }

        protected void btnCancelDate_Click(object sender, EventArgs e)
        {
            popup.Visible = false;
        }

        protected void btnDateChange_Click(object sender, ImageClickEventArgs e)
        {
           
            txtNewdate.Attributes["max"] = DateTime.Now.ToString("yyyy-MM-dd");
           // return;
            popup.Visible = true;
        }
    }
}