
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
using System.Net;
using System.Net.Mail;

namespace LTG
{
    public partial class InitStockTake : System.Web.UI.Page
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
            string qry = "select * from vw_InitStockTake1";

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
            string qry = "select Distinct Bin,CustomerCode,HU from vw_InitStockTake1 ";
            if (txtBins.Text != "")
                qry += " where Bin like'" + txtBins.Text + "%' or HU like'" + txtBins.Text + "%'";
            txtBins.Text = "";
            cmd = new SqlCommand(qry, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            con.Close();
            grdBins.DataSource = ds.Tables[0];
            grdBins.DataBind();

        }
        private DataTable loadData()
        {
           // SaveCheckboxStates();
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd;
            con.Open();
            string qry = "select * from vw_InitStockTake1";

            cmd = new SqlCommand(qry, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            con.Close();
            return ds.Tables[0];


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
        private void bindselectedBin()
        {
            DataTable dt = loadData();
            List<string> selectedHUs = ViewState["CheckedBoxesBin"] as List<string> ?? new List<string>();
            //  List<string> selectedHUs = GetSelectedHUs();
            // Filter the DataTable
            DataTable filteredDt = dt.Clone(); // Clone the structure of the DataTable
            foreach (DataRow row in dt.Rows)
            {
                if (selectedHUs.Contains(row["HU"].ToString()))
                {
                    filteredDt.ImportRow(row); // Add matching rows to the filtered DataTable
                }
            }

            // Bind the filtered data to the GridView
            grdBin.DataSource = filteredDt;
            grdBin.DataBind();
        }
        protected void btnOk_Click(object sender, EventArgs e)
        {
            SaveCheckboxStatesBin();
            bindselectedBin();
            binPopup.Visible = false;
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
        protected void Button2_Click(object sender, EventArgs e)
        {
            binPopup.Visible = false;
        }
        protected void grdBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void imgExcel_Click(object sender, ImageClickEventArgs e)
        {

        }
        protected void btnCancelPopup_Click(object sender, EventArgs e)
        {
            popupcancel.Visible = false;
        }

        protected void btnConfirmCancel_Click(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string checkCountQry = "";

                // Check if the table has any records
                checkCountQry = "SELECT COUNT(*) FROM StockTakeSetup where Password='" + txtstockcancelpwd.Text + "'";
                SqlCommand checkCmd = new SqlCommand(checkCountQry, con);
                int recordCount = (int)checkCmd.ExecuteScalar();


                if (recordCount == 0)
                {
                    string script = "alert(\"Password is wrong, Please setup the new password\");";
                    ScriptManager.RegisterStartupScript(this, GetType(),
                                          "ServerControlScript", script, true);
                    txtstockcancelpwd.Text = "";
                    return;
                }
                string query3 = "Delete from StockTakeHistory where RefNo is null;Delete From StockMaster;Update StocktakeCounterInstruction set StockTakeCompleted=1;";
                checkCmd = new SqlCommand(query3, con);
                checkCmd.ExecuteNonQuery();
            }
            txtstockcancelpwd.Text = "";
            
            popupcancel.Visible = false;
            SendEmailWithAttachment();
            txtCancelReason.Text = "";
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Current stock take progress is cancelled successfully');", true);

        }
        public void SendEmailWithAttachment()
        {
            HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");
            // Specify the sender's and receiver's email addresses
            string fromEmail = "noreply@ltgfreight.co.za";
            // string toEmail = "aswini@codex-it.co.za";
            //string fileName = '';
            // Specify the subject and body of the email
            string subject = "LTG - Stock Take Cancelled";
            //string body = "This is the body of the email.";
            string body = "<b>Hello!  </b>, <br> <br>";
            body += "The ongoing stock take  process successfully cancelled. ";
            body += "<br>Cancelled by: " + hdUserName.Value;
            body += "<br>Reason for cancellation: " + txtCancelReason.Text;

            body += "<br><br><b> Thanks & Regards,</b> <br>LTG Team.";
            string smtpUsername = "noreply@ltgfreight.co.za"; // Usually your email address
            string smtpPassword = "Max853000"; // Your email passwords


            try
            {
                // Create a new MailMessage object
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(fromEmail);
                //mail.To.Add(toEmail);
                // mail.To.Add("greg@codex-it.co.za");
                //mail.To.Add("tbusang@ltgfreight.co.za");
                //mail.To.Add("vz3dispatch@ltgfreight.co.za");
                //mail.To.Add("tmabena@ltgfreight.co.za");
                //mail.To.Add("vz3receiving@ltgfreight.co.za");
                //mail.To.Add("tngamthewe@ltgfreight.co.za");
                //mail.To.Add("tsetshedi@ltgfreight.co.za");
                string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();
                    string qry = "Select * from MailSetup Where Type=1";
                    SqlCommand cmd1 = new SqlCommand(qry, con);
                   // HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");

                    var userName = hdUserName.Value;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            var maillist = dt.Rows[0]["MailIds"].ToString().Split(',');
                            foreach(var item in maillist)
                            {
                                mail.To.Add(item);
                            }
                            // qry = "Update MailSetup set  MailIds='" + txtMails.Text + "',UpdatedDate=Getdate(),UpdatedBy='" + hdUserName.Value + "'  Where Type=" + ddlReturnType.SelectedValue;

                        }
                        else
                            return;
                           
                    }
                }

                mail.Subject = subject;
                mail.Body = body;
                var smtp = new SmtpClient();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;


                mail.IsBodyHtml = true;


                smtp.Port = 587;
                //smtp.Host = "smtp.gmail.com";
                smtp.Host = "smtp.office365.com";
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtp.Send(mail);


                Console.WriteLine("Email sent successfully with attachment.");
            }
            catch (Exception ex)
            {
                // Handle any errors that occur
                Console.WriteLine("An error occurred while sending the email: " + ex.Message);
            }
        }

        protected void btnCloseCancel_Click(object sender, EventArgs e)
        {
            popupcancel.Visible = false;
        }

        protected void btnCancelOngoingStock_Click(object sender, EventArgs e)
        {
            if (ValidateStockMaster())
            {
                // Show a confirmation dialog to the user
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('There is no current stock take in progress');", true);
                return;
            }
            popupcancel.Visible = true;
        }
        private void save()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;
            HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");

            var userName = hdUserName.Value;
            string refno = "CycleCount";
            if (radFullStockTake.Checked)
                refno = "Full Stock Take";
            // SQL Insert Query
            string query = "INSERT INTO StockMaster (CustomerCode, ContainerId, Bin, HU, Qty,Currentdate,NoofCount,Completed,CreatedBy,Refno) VALUES (@CustomerCode, @ContainerId, @Bin, @HU, @Qty,getdate(),1,0,'" + userName + "','"+ refno +"')";
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

        protected void radCycle_CheckedChanged(object sender, EventArgs e)
        {
            btnBrowseBin.Visible = true;
            btnReset.Text = "Initiate Cycle Count";
        }

        protected void radFullStockTake_CheckedChanged(object sender, EventArgs e)
        {
            btnBrowseBin.Visible = false;
            btnReset.Text = "Initiate Full Stock Take";
            loadGrid();
        }
    }
}