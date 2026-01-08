using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LTG
{
    public partial class CustomerSetup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                bindBranch();
                bindBin();
                bindUOP();
                if (Request.QueryString["Code"] != null)
                {
                    var id = Request.QueryString["Code"].ToString();
                    bindCustomer(id);
                    btnCreate.Text = "Update Customer";
                    divHeader.InnerText = "Editing Customer";
                }
            }
        }
        private void bindCustomer(string id)
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from Customers where CustomerCode='" + id + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {

                        txtSurName.Text = dt.Rows[0]["CustomerName"].ToString();
                        txtCode.Text = dt.Rows[0]["CustomerCode"].ToString();
                        txtAddr1.Text = dt.Rows[0]["Address1"].ToString();
                        txtAddr2.Text = dt.Rows[0]["Address2"].ToString();
                        txtAddr3.Text = dt.Rows[0]["Address3"].ToString();
                        txtAddr4.Text = dt.Rows[0]["Address4"].ToString();
                        txtDelAddr1.Text = dt.Rows[0]["DelAddr1"].ToString();
                        txtDelAddr2.Text = dt.Rows[0]["DelAddr2"].ToString();
                        txtDelAddr3.Text = dt.Rows[0]["DelAddr3"].ToString();
                        txtDelAddr4.Text = dt.Rows[0]["DelAddr4"].ToString();
                        if (Convert.ToBoolean(dt.Rows[0]["Active"]))
                            chkActive.Checked = true;
                        else
                            chkActive.Checked = false;
                        if(dt.Rows[0]["Bin"] !=null)
                        ddlBin.SelectedValue= dt.Rows[0]["Bin"].ToString();
                        if (dt.Rows[0]["UOP"] != null)
                            ddlUOP.SelectedValue = dt.Rows[0]["UOP"].ToString();
                        if (dt.Rows[0]["ContractFiles"] != null)
                            hdnPdfLocation.Value = dt.Rows[0]["ContractFiles"].ToString();
                        ddlBranch.SelectedValue = dt.Rows[0]["BranchId"].ToString();
                        if (dt.Rows[0]["ContractDate"] != null && dt.Rows[0]["ContractDate"].ToString() != "")
                            txtExtContract.Text = Convert.ToDateTime(dt.Rows[0]["ContractDate"]).ToString("dd/MMM/yyyy");
                    }
                }
            }
        }
        private void bindBin()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from Bin";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        ddlBin.DataSource = dt;
                        ddlBin.DataBind();
                        ddlBin.DataTextField = "BinName";
                        ddlBin.DataValueField = "BinId";
                        ddlBin.DataBind();
                        // ddlBranch.da
                    }
                }
            }
        }
        private void bindUOP()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from UOPMaster";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        ddlUOP.DataSource = dt;
                        ddlUOP.DataBind();
                        ddlUOP.DataTextField = "UOP";
                        ddlUOP.DataValueField = "UOP";
                        ddlUOP.DataBind();
                        ddlUOP.Items.Insert(0, new ListItem("Select Default UOP", ""));
                        // ddlBranch.da
                    }
                }
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

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            if(txtNewContract.Text !="" && !FileUpload1.HasFile)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Please upload a contract document.');", true);
                return;
            }
            if (Request.QueryString["Code"] != null)
            {
                update();
            }
            else
                save();
        }
        private void save()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from Customers where CustomerCode='" + txtCode.Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('CustomerCode already exisits.');", true);
                        return;
                    }
                }
                string savePath = "";
                if (FileUpload1.HasFile)
                {
                    string ext = Path.GetExtension(FileUpload1.FileName).ToLower();
                    if (ext != ".pdf")
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Only PDF Files are allowed.');", true);
                        return;
                    }
                    savePath = "Contract_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".pdf";

                    // string filename = Path.GetFileName(FileUpload1.FileName);
                    string savePath1 = Server.MapPath("~/Contract/") + savePath;
                    FileUpload1.SaveAs(savePath1);


                }
                if(ddlUOP.SelectedIndex==0)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Please select Default UOP.');", true);
                    return;
                }
                if(savePath=="")
                qry = "Insert into Customers(UOP,CustomerCode,CustomerName,BranchId,Active,Address1,Address2,Address3,Address4,DelAddr1,DelAddr2,DelAddr3,DelAddr4)values('" + ddlUOP.SelectedValue + "','" + txtCode.Text + "','" + txtSurName.Text + "','" + ddlBranch.SelectedValue + "',1,'" + txtAddr1.Text + "','" + txtAddr2.Text + "','" + txtAddr3.Text + "','" + txtAddr4.Text +"','" + txtDelAddr1.Text + "','" + txtDelAddr2.Text + "','" + txtDelAddr3.Text + "','" + txtDelAddr4.Text + "')";
                else
                    qry = "Insert into Customers(UOP,CustomerCode,CustomerName,BranchId,Active,Address1,Address2,Address3,Address4,DelAddr1,DelAddr2,DelAddr3,DelAddr4,ContractDate,ContractFiles)values('" + ddlUOP.SelectedValue + "','" + txtCode.Text + "','" + txtSurName.Text + "','" + ddlBranch.SelectedValue + "',1,'" + txtAddr1.Text + "','" + txtAddr2.Text + "','" + txtAddr3.Text + "','" + txtAddr4.Text + "','" + txtDelAddr1.Text + "','" + txtDelAddr2.Text + "','" + txtDelAddr3.Text + "','" + txtDelAddr4.Text + "','" + txtNewContract.Text + "','"+ savePath +"')";

                using (SqlCommand cmd = new SqlCommand(qry, con))
                {



                    cmd.ExecuteNonQuery();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Customer Successfully Created.');", true);
                    txtCode.Text = "";
                    txtSurName.Text = "";
                    ddlBranch.SelectedIndex = 0;
                    txtAddr1.Text = "";
                    txtAddr2.Text = "";
                    txtAddr3.Text = "";
                    txtAddr4.Text = "";
                    txtDelAddr1.Text = "";
                    txtDelAddr2.Text = "";
                    txtDelAddr3.Text = "";
                    txtDelAddr4.Text = "";
                }
            }

        }
        private void update()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();

                // Check if the customer exists
                string qryCheck = "SELECT * FROM Customers WHERE CustomerCode=@CustomerCode";
                SqlCommand cmdCheck = new SqlCommand(qryCheck, con);
                cmdCheck.Parameters.AddWithValue("@CustomerCode", txtCode.Text);
                HiddenField hdLoginId = (HiddenField)this.Master.FindControl("hdnLoginId");

                var userid = hdLoginId.Value;
                HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");
                if (ddlUOP.SelectedIndex == 0)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Please select Default UOP.');", true);
                    return;
                }
                var userName = hdUserName.Value;
                string savePath = "";
                if (FileUpload1.HasFile)
                {
                    string ext = Path.GetExtension(FileUpload1.FileName).ToLower();
                    if (ext != ".pdf")
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Only PDF Files are allowed.');", true);
                        return;
                    }
                    savePath  = "Contract_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".pdf";

                   // string filename = Path.GetFileName(FileUpload1.FileName);
                    string savePath1 = Server.MapPath("~/Contract/") + savePath;
                    FileUpload1.SaveAs(savePath1);

                    
                }
                    using (SqlDataAdapter da = new SqlDataAdapter(cmdCheck))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // If the customer exists, update their details
                    if (dt.Rows.Count > 0)
                    {
                        string qryUpdate = "UPDATE Customers SET " +
                                           "CustomerName=@CustomerName, " +
                                           "BranchId=@BranchId, " +
                                           "Address1=@Address1, " +
                                           "Address2=@Address2, " +
                                           "Address3=@Address3, " +
                                           "Address4=@Address4, " +
                                           "DelAddr1=@DelAddr1, " +
                                           "DelAddr2=@DelAddr2, " +
                                           "DelAddr3=@DelAddr3, " +
                                           "DelAddr4=@DelAddr4, " +
                                           "Active=@Active," +
                                           "UOP=@UOP," +
                                           "ModifiedUserId=@ModifiedUserId," +
                                           "ModifiedUserName=@ModifiedUserName";
                                           if(savePath !="")
                        {
                            qryUpdate += " ,ContractDate='" + txtNewContract.Text + "',ContractFiles='" + savePath + "'";
                        }
                        qryUpdate += " WHERE CustomerCode=@CustomerCode";

                        SqlCommand cmdUpdate = new SqlCommand(qryUpdate, con);
                        cmdUpdate.Parameters.AddWithValue("@CustomerCode", txtCode.Text);
                        cmdUpdate.Parameters.AddWithValue("@CustomerName", txtSurName.Text);
                        cmdUpdate.Parameters.AddWithValue("@BranchId", ddlBranch.SelectedValue);
                        cmdUpdate.Parameters.AddWithValue("@UOP", ddlUOP.SelectedValue);
                        cmdUpdate.Parameters.AddWithValue("@Address1", txtAddr1.Text);
                        cmdUpdate.Parameters.AddWithValue("@Address2", txtAddr2.Text);
                        cmdUpdate.Parameters.AddWithValue("@Address3", txtAddr3.Text);
                        cmdUpdate.Parameters.AddWithValue("@Address4", txtAddr4.Text);
                        cmdUpdate.Parameters.AddWithValue("@DelAddr1", txtDelAddr1.Text);
                        cmdUpdate.Parameters.AddWithValue("@DelAddr2", txtDelAddr2.Text);
                        cmdUpdate.Parameters.AddWithValue("@DelAddr3", txtDelAddr3.Text);
                        cmdUpdate.Parameters.AddWithValue("@DelAddr4", txtDelAddr4.Text);
                        cmdUpdate.Parameters.AddWithValue("@ModifiedUserId", userid);
                        cmdUpdate.Parameters.AddWithValue("@ModifiedUserName", userName);
                        if (chkActive.Checked)
                            cmdUpdate.Parameters.AddWithValue("@Active", 1);
                        else
                            cmdUpdate.Parameters.AddWithValue("@Active", 0);
                        cmdUpdate.ExecuteNonQuery();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Customer Successfully Updated.');", true);
                    }
                    else
                    {
                        // If the customer does not exist, show an alert or handle as needed
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('CustomerCode does not exist.');", true);
                    }

                    // Clear form fields
                    txtCode.Text = "";
                    txtSurName.Text = "";
                    ddlBranch.SelectedIndex = 0;
                    txtAddr1.Text = "";
                    txtAddr2.Text = "";
                    txtAddr3.Text = "";
                    txtAddr4.Text = "";
                    txtDelAddr1.Text = "";
                    txtDelAddr2.Text = "";
                    txtDelAddr3.Text = "";
                    txtDelAddr4.Text = "";
                }
            }
        }
        protected void btnViewPdf_Click(object sender, EventArgs e)
        {
            if(hdnPdfLocation.Value=="")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Contract does not exist.');", true);

            }
            // Replace this with your actual PDF path (must be accessible via browser)
            string pdfUrl = ResolveUrl("~/Contract/" + hdnPdfLocation.Value); // ← THIS is browser-friendly
            hdnPdfUrl.Value = pdfUrl;

           
            // Call the JS function to show modal
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowPdfModal", "showPdf();", true);
        }

        protected void chkDelivery_CheckedChanged(object sender, EventArgs e)
        {
            txtDelAddr1.Focus();
            if (chkDelivery.Checked)
            {
                txtDelAddr1.Text = txtAddr1.Text;
                txtDelAddr2.Text = txtAddr2.Text;
                txtDelAddr3.Text = txtAddr3.Text;
                txtDelAddr4.Text = txtAddr4.Text;
            }
            else
            {
                txtDelAddr1.Text = "";
                txtDelAddr2.Text = "";
                txtDelAddr3.Text ="";
                txtDelAddr4.Text = "";
            }
        }
    }
}