using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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
    public partial class TransportProcess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                bindBranch();
                bindCustomer();
                txtDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                GenerateGRN();
                hdninitalNumber.Value = "1";
                if (Request.QueryString["id"] != null)
                {
                    hdninitalNumber.Value = "0";
                    var id = Request.QueryString["id"].ToString();
                    BindIncomplete(id);
                }
                else
                {
                    if (hdninitalNumber.Value == "1")
                    {
                        if (CheckGRN())
                        {
                            GenerateGRN();
                            string script = "alert(\"This GRN Already used by other user,Your new GRN is: " + txtGRN.Text + "\");";
                            ScriptManager.RegisterStartupScript(this, GetType(),
                                                  "ServerControlScript", script, true);
                            FillGrid();
                        }
                    }
                    CheckIncomplete();
                }
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
        private string GenerateSeqn()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "select count(distinct(PartNumber))+1 FROM Transport where PartNumber='" + txtSpare.Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        return dt.Rows[0][0].ToString();

                    }
                    else return "1";
                }
            }
        }
        private bool CheckDuplicate()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "select PartNumber FROM Transport where PartNumber='" + txtSpare.Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        return true;

                    }
                    else return false;
                }
            }
        }
        private int partNumberTotal()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "select count(PartNumber) FROM Transport where PartNumber='" + txtSpare.Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        return Convert.ToInt32(dt.Rows[0][0].ToString());

                    }
                    else return 0;
                }
            }
        }
        private bool CheckFullStockTake()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "select HU FROM StockMaster where Refno='Full Stock Take'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        return true;

                    }
                    else return false;
                }
            }
        }
        private bool CheckFullStockTakeAccuracy()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "select Bin FROM BinAccuracyMaster where Refno='Full Stock Accuracy'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        return true;

                    }
                    else return false;
                }
            }
        }
        private void GenerateGRN()
        {
            string dtMonth = DateTime.Now.ToString("dd-MM-yy").Replace("-", "");
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;
           
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "select count(distinct(GRN))+1 FROM Transport";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        string output = FormatNumberWithLeadingZeros(dt.Rows[0][0].ToString(), 9);
                        txtGRN.Text = "GRN-" + dtMonth + "-" + output;
                        if (CheckGRN())
                        {
                            output = FormatNumberWithLeadingZeros((Convert.ToInt32(dt.Rows[0][0].ToString()) + 1).ToString(), 9);
                            txtGRN.Text = "GRN-" + dtMonth + "-" + output;
                        }
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
            if (CheckFullStockTake())
            {
                string script = "alert(\"Stock take currently in process you cannot proceed please contact LTG warehouse manager/s\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                return;
            }
            if (CheckFullStockTakeAccuracy())
            {
                string script = "alert(\"Stock take accuracy currently in process, you cannot proceed please contact LTG warehouse manager/s\");";
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
            
            getTransportFee();
            if (hdnTransportFee.Value == "")
            {
                string script = "alert(\"Please setup the Transport fee. \");";
                if(hdnFeeDays.Value=="1")
                {
                    script = script.Replace("Transport", "saturday Transport");
                }
                if (hdnFeeDays.Value == "2")
                {
                    script = script.Replace("Transport", "sunday Transport");
                }
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);

                return;
            }
            if (hdnContractDate.Value != "" && Convert.ToDateTime(hdnContractDate.Value) < Convert.ToDateTime(txtDate.Text))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "var myModal = new bootstrap.Modal(document.getElementById('warningModal')); myModal.show();", true);

              //  ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#warningModal').modal('show');", true);
                return;
            }
            divCustomer.Visible = false;
            divScan.Visible = true;
            divHeader.InnerText = "Transport Process for - " + ddlCustomer.SelectedItem.Text;
            txtContainer1.Text = txtContainer.Text;
            txtHU.Focus();
            
            if(ddlCustomer.SelectedValue == "ALP001" || ddlCustomer.SelectedValue == "ALP002" || ddlCustomer.SelectedValue == "AUT003" || ddlCustomer.SelectedValue  == "AUT004")
            {
                divALP.Visible = true;
                divBMW.Visible = false;
                divBar.Visible = false;
                divSeq.Visible = false;
                grdScans.Columns[2].Visible = true;
                grdScans.Columns[1].Visible = false;
                btnPrintBarcode.Visible = true;
                // GenerateSeqn();
            }
            else
            {
                divALP.Visible = false;
                divBMW.Visible = true;
                divBar.Visible = false;
                divSeq.Visible = false;
                grdScans.Columns[1].Visible = true;
                grdScans.Columns[2].Visible = false;
                btnPrintBarcode.Visible = false;
            }
            FillGrid();
        }
        private bool checkRefNo()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from Transport where ContainerId='" + txtContainer.Text + "'";
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
        private void getTransportFee()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select TransportFee,Bin,SatTransportFee,SunTransportFee,ContractDate from Customers where CustomerCode='" + ddlCustomer.SelectedValue + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][4] != DBNull.Value)
                            hdnContractDate.Value = dt.Rows[0][4].ToString();
                        else
                            hdnContractDate.Value = "";
                        DateTime enterDate;
                        if (DateTime.TryParse(txtDate.Text, out enterDate))
                        {
                            if (enterDate.DayOfWeek == DayOfWeek.Saturday)
                            {
                                if (dt.Rows[0][2] != DBNull.Value)
                                    hdnTransportFee.Value = dt.Rows[0][2].ToString();
                                else
                                    hdnTransportFee.Value = "";
                                hdnFeeDays.Value = "1";
                            }
                            else if(enterDate.DayOfWeek == DayOfWeek.Sunday)
                            {
                                if (dt.Rows[0][3] != DBNull.Value)
                                    hdnTransportFee.Value = dt.Rows[0][3].ToString();
                                else
                                    hdnTransportFee.Value = "";
                                hdnFeeDays.Value = "2";
                            }
                            else
                            {
                                if (dt.Rows[0][0] != DBNull.Value)
                                    hdnTransportFee.Value = dt.Rows[0][0].ToString();
                                else
                                    hdnTransportFee.Value = "";
                                hdnFeeDays.Value = "0";
                            }
                        }
                           
                        hdnBin.Value = dt.Rows[0][1].ToString();
                        // ddlBranch.da
                    }
                    else
                    {
                        hdnTransportFee.Value = "";
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
            if (hdninitalNumber.Value == "1")
            {
                if (CheckGRN())
                {
                    GenerateGRN();
                    string script = "alert(\"This GRN Already used by other user,Your new GRN is: " + txtGRN.Text + "\");";
                    ScriptManager.RegisterStartupScript(this, GetType(),
                                          "ServerControlScript", script, true);
                    FillGrid();
                }
            }

            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                HiddenField hdLoginId = (HiddenField)this.Master.FindControl("hdnLoginId");

                var userid = hdLoginId.Value;
                HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");

                var userName = hdUserName.Value;
                string qry = "";
                //if (ddlCustomer.SelectedValue == "ALP001" || ddlCustomer.SelectedValue == "ALP002")
                //{
                //   string barcode= GetBarcode();
                //    qry = "Insert into Transport(BarcodeBase64,BarCode,SeqnNumber,ContainerId,BranchId,BranchName,CustomerCode,CustomerName,HU,Qty,UnitTransportCost,TotalTransportCost,Loginname,DateTimeofScan,CreatedBy,CreatedDate,GRN,DefaultBin)values('" + barcode + "','" + txtBarcode.Text + "','" + txtSeqn.Text + "','" + txtContainer.Text + "'," + ddlBranch.SelectedValue + ",'" + ddlBranch.SelectedItem.Text + "','" + ddlCustomer.SelectedValue + "','" + ddlCustomer.SelectedItem.Text + "','" + txtHU.Text + "','" + txtQty.Text + "'," + hdnTransportFee.Value + "," + hdnTransportFee.Value + ",'" + userid + "','" + txtDate.Text + "','" + userName + "',getdate(),'" + txtGRN.Text + "','" + txtDefaultBin.Text + "')";
                //}
                //else
                //{
                if (Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") == DateTime.Now.ToString("dd/MMM/yyyy"))
                    txtDate.Text = DateTime.Now.ToString("dd/MMM/yyyy HH:mm");

                    qry = "Insert into Transport(ContainerId,BranchId,BranchName,CustomerCode,CustomerName,HU,Qty,UnitTransportCost,TotalTransportCost,Loginname,DateTimeofScan,CreatedBy,CreatedDate,GRN,DefaultBin)values('" + txtContainer.Text + "'," + ddlBranch.SelectedValue + ",'" + ddlBranch.SelectedItem.Text + "','" + ddlCustomer.SelectedValue + "','" + ddlCustomer.SelectedItem.Text + "','" + txtHU.Text + "','" + txtQty.Text + "'," + hdnTransportFee.Value + "," + hdnTransportFee.Value + ",'" + userid + "','" + txtDate.Text + "','" + userName + "',getdate(),'" + txtGRN.Text + "','" + txtDefaultBin.Text + "')";

               // }
                SqlCommand cmd1 = new SqlCommand(qry, con);
                cmd1.ExecuteNonQuery();
                hdninitalNumber.Value = "0";
                //if (ddlCustomer.SelectedValue == "ALP001" || ddlCustomer.SelectedValue == "ALP002")
                //{
                //    GenerateSeqn();
                //}
            }
            txtHU.Text = "";
            txtHU.Focus();
           // txtBarcode.Text = "";
        }
        private void saveAlP()
        {
            
            
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                HiddenField hdLoginId = (HiddenField)this.Master.FindControl("hdnLoginId");

                var userid = hdLoginId.Value;
                HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");

                var userName = hdUserName.Value;
                string qry = "";
                //if (ddlCustomer.SelectedValue == "ALP001" || ddlCustomer.SelectedValue == "ALP002")
                //{
                //   string barcode= GetBarcode();
                //    qry = "Insert into Transport(BarcodeBase64,BarCode,SeqnNumber,ContainerId,BranchId,BranchName,CustomerCode,CustomerName,HU,Qty,UnitTransportCost,TotalTransportCost,Loginname,DateTimeofScan,CreatedBy,CreatedDate,GRN,DefaultBin)values('" + barcode + "','" + txtBarcode.Text + "','" + txtSeqn.Text + "','" + txtContainer.Text + "'," + ddlBranch.SelectedValue + ",'" + ddlBranch.SelectedItem.Text + "','" + ddlCustomer.SelectedValue + "','" + ddlCustomer.SelectedItem.Text + "','" + txtHU.Text + "','" + txtQty.Text + "'," + hdnTransportFee.Value + "," + hdnTransportFee.Value + ",'" + userid + "','" + txtDate.Text + "','" + userName + "',getdate(),'" + txtGRN.Text + "','" + txtDefaultBin.Text + "')";
                //}
                //else
                //{
                string barcode = GetBarcode();
                if (Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") == DateTime.Now.ToString("dd/MMM/yyyy"))
                    txtDate.Text = DateTime.Now.ToString("dd/MMM/yyyy HH:mm");
                qry = "Insert into Transport(BarcodeBase64,PartNumber,PartQty,ContainerId,BranchId,BranchName,CustomerCode,CustomerName,HU,Qty,UnitTransportCost,TotalTransportCost,Loginname,DateTimeofScan,CreatedBy,CreatedDate,GRN,DefaultBin)values('" +barcode +"', '" + txtSpare.Text + "','" + txtALPQty.Text + "','" + txtContainer.Text + "'," + ddlBranch.SelectedValue + ",'" + ddlBranch.SelectedItem.Text + "','" + ddlCustomer.SelectedValue + "','" + ddlCustomer.SelectedItem.Text + "','" + txtHU.Text + "','" + txtQty.Text + "'," + hdnTransportFee.Value + "," + hdnTransportFee.Value + ",'" + userid + "','" + txtDate.Text + "','" + userName + "',getdate(),'" + txtGRN.Text + "','" + txtDefaultBin.Text + "')";

                // }
                SqlCommand cmd1 = new SqlCommand(qry, con);
                cmd1.ExecuteNonQuery();
                hdninitalNumber.Value = "0";
                //if (ddlCustomer.SelectedValue == "ALP001" || ddlCustomer.SelectedValue == "ALP002")
                //{
                //    GenerateSeqn();
                //}
            }
            txtHU.Text = "";
            txtHU.Focus();
            // txtBarcode.Text = "";
        }
        protected void btnComplete_Click(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                
                string qry = "Update Transport set Completed=1 where GRN='" + txtGRN.Text + "'";
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
        private string GenerateBarcodeBase64(string text)
        {
            BarcodeWriter writer = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 250,
                    Height = 70
                },
                Renderer = new ZXing.Rendering.BitmapRenderer
                {
                    TextFont = new Font("Arial", 16, FontStyle.Bold) // Bigger and Bold Text
                }
            };

            using (Bitmap bitmap = writer.Write(text))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }
        private string GetBarcode()
        {
            string barcodeText = txtBarcode.Text.Trim();
           
                return  GenerateBarcodeBase64(barcodeText);
               
        }
        protected void GetContainer()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "SELECT  isnull(max([ContainerId])+1,1)  FROM Transport";
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
                string qry = "SELECT  *  FROM Transport where GRN='" + txtGRN.Text + "' order by CreatedDate";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    grdScans.DataSource = dt;
                    grdScans.DataBind();
                    divHeader.InnerText = "Transport Process - Total Scans: " + dt.Rows.Count;
                }
            }
        }

        protected List<string> FillHU()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;
            List<string> redirectUrls = new List<string>();
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "SELECT  HU  FROM Transport where GRN='" + txtGRN.Text + "' order by CreatedDate";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        redirectUrls.Add(dt.Rows[i][0].ToString());
                    }
                   
                }
            }
            return redirectUrls;
        }
        private bool CheckGRN()
        {
            string dtMonth = DateTime.Now.ToString("dd-MM-yy").Replace("-", "");
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;
           // txtDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "select GRN FROM Transport where GRN ='" + txtGRN.Text + "'";
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
                string qry = "SELECT  *  FROM Transport where Completed=0 and Loginname='" + Session["LoginId"].ToString() + "' order by DateTimeofScan desc";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                   if (dt.Rows.Count>0)
                    {
                        string redirectUrl = "TransportProcess.aspx?id=" + dt.Rows[0]["GRN"].ToString(); // The URL to open in a new tab

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
                string qry = "SELECT  *  FROM Transport where Completed=0 and GRN='" + GRN + "' order by DateTimeofScan";
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
                        getTransportFee();
                        divCustomer.Visible = false;
                        divScan.Visible = true;
                        divHeader.InnerText = "Transport Process for - " + ddlCustomer.SelectedItem.Text;
                        txtContainer1.Text = txtContainer.Text;
                        txtHU.Focus();
                        FillGrid();
                        if (ddlCustomer.SelectedValue == "ALP001" || ddlCustomer.SelectedValue == "ALP002" || ddlCustomer.SelectedValue == "AUT003" || ddlCustomer.SelectedValue == "AUT004")
                        {
                            divALP.Visible = true;
                            divBMW.Visible = false;
                            grdScans.Columns[2].Visible = true;
                            grdScans.Columns[1].Visible = false;
                            btnPrintBarcode.Visible = true;
                            // GenerateSeqn();
                        }
                        else
                        {
                            btnPrintBarcode.Visible = false;
                            divALP.Visible = false;
                            divBMW.Visible = true;
                            divBar.Visible = false;
                            divSeq.Visible = false;
                            grdScans.Columns[1].Visible = true;
                            grdScans.Columns[2].Visible = false;
                        }
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
                string qry = "Select * from Transport where HU='" + txtHU.Text + "' and Isnull(Returned,0)=0";
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
            //if (ddlCustomer.SelectedValue == "ALP001" || ddlCustomer.SelectedValue == "ALP002")
            //{
            //    txtBarcode.Focus();
            //}
            //else
            //{
                save();
                FillGrid();
           // }
            
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

                string qry = "Delete  from Transport where GRN='" + txtGRN.Text + "' and HU not in(select HU from WarehouseProcess)";
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

                string qry = "Delete  from Transport where GRN='" + txtGRN.Text + "' and HU not in(select HU from WarehouseProcess)";
                SqlCommand cmd1 = new SqlCommand(qry, con);

                cmd1.ExecuteNonQuery();
                Response.Redirect("TransportProcess.aspx");
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
                string checkCountQry = "";
                checkCountQry = "SELECT MonthEndDate FROM MonthEnd";
                SqlCommand checkCmd = new SqlCommand(checkCountQry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(checkCmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
                    {
                       if(Convert.ToDateTime(txtNewdate.Text).Date<= Convert.ToDateTime(dt.Rows[0][0].ToString()).Date)
                        {
                            string script = "alert(\"Cannot back post before monthend date please contact your system administrator\");";
                            ScriptManager.RegisterStartupScript(this, GetType(),
                                                  "ServerControlScript", script, true);

                            return;
                        }
                        // ddlBranch.da
                    }
                }
                // Check if the table has any records
                checkCountQry = "SELECT COUNT(*) FROM Supervisor where Password='" + txtSupPassword.Text +"'";
                 checkCmd = new SqlCommand(checkCountQry, con);
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

        protected void txtBarcode_TextChanged(object sender, EventArgs e)
        {
            save();
            FillGrid();
        }

        protected void txtALPQty_TextChanged(object sender, EventArgs e)
        {
            txtSpare.Text = txtSpare.Text.Trim();
            txtALPQty.Text = txtALPQty.Text.Trim();
            if (txtSpare.Text != "")
            {
                saveALPInfo();
            }
            else
                txtSpare.Focus();
        }

        protected void txtSpare_TextChanged(object sender, EventArgs e)
        {

            saveALPInfo();


        }
        private void saveALPInfo()
        {
            txtSpare.Text = txtSpare.Text.Trim();
            txtALPQty.Text = txtALPQty.Text.Trim();
            if (txtSpare.Text == "")
            {
                string script = "alert(\"Please enter the part number\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                return;
            }
            if (txtALPQty.Text == "")
            {
                string script = "alert(\"Please enter the total quantity\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                return;
            }
            if (CheckDuplicate())
            {
                var total = partNumberTotal();
                for (int i = 0; i < Convert.ToInt32(txtALPQty.Text); i++)
                {
                    string output = FormatNumberWithLeadingZeros((i + 1 + total).ToString(), 6);
                    txtHU.Text = txtSpare.Text + "-" + output;
                    txtBarcode.Text = txtHU.Text;
                    saveAlP();
                }
            }
            else
            {
                for (int i = 0; i < Convert.ToInt32(txtALPQty.Text); i++)
                {
                    string output = FormatNumberWithLeadingZeros((i + 1).ToString(), 6);
                    txtHU.Text = txtSpare.Text + "-" + output;
                    txtBarcode.Text = txtHU.Text;
                    saveAlP();
                }
            }
            FillGrid();
            txtALPQty.Text = "";
            txtSpare.Text = "";
            txtALPQty.Focus();
        }
        public void CallExecutable(string id)
        {
            // Specify the path to the executable
            // string exePath = @"C:\DEVELOPMENT\LTGBarcode\bin\Debug\LTGBarcode.exe";
            string exePath = @"C:\LTG\LTGBarcode\LTGBarcode.exe";

            // Specify any arguments if needed
            string arguments = id;

            // Create a new process start info object
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = arguments,
                RedirectStandardOutput = true,
                UseShellExecute = false, // Important: this prevents redirection of streams
                CreateNoWindow = true // Optional: hides the console window
            };

            try
            {
                using (Process process = Process.Start(startInfo))
                {
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string result = reader.ReadToEnd();
                        // return result;
                    }
                }

            }
            catch (Exception ex)
            {
                // Handle exception
                Response.Write("An error occurred: " + ex.Message);
            }
        }
        protected void btnPrintBarcode_Click(object sender, EventArgs e)
        {
            var id = txtGRN.Text;
            CallExecutable(id.ToString());

            string relativeUrl = "Delivery.aspx?source=2&id=" + id;

            // Register the JavaScript to open a new window with the relative URL
            string script = $"openNewWindow('{ResolveUrl(relativeUrl)}');";
            ClientScript.RegisterStartupScript(this.GetType(), "openNewWindowScript", script, true);

            //var HUs = FillHU();
            //if (HUs.Count > 0)
            //{
            //    var script2 = "<script>var userConfirmed2 = confirm('Do you want to print barcode?');" +
            //            "if (userConfirmed2) {";

            //    foreach (string url in HUs)
            //    {
            //        var redirectUrl1 = "Barcode.aspx?id=" + url;
            //        script2 += "window.open('" + redirectUrl1 + "', '_blank');";
            //    }

            //    script2 += "}</script>";

            //    // Register the script to execute
            //    ClientScript.RegisterStartupScript(this.GetType(), "OpenPrintPages", script2);
            //}
        }

        protected void btnYes_Click(object sender, EventArgs e)
        {
            divCustomer.Visible = false;
            divScan.Visible = true;
            divHeader.InnerText = "Transport Process for - " + ddlCustomer.SelectedItem.Text;
            txtContainer1.Text = txtContainer.Text;
            txtHU.Focus();
            if (ddlCustomer.SelectedValue == "ALP001" || ddlCustomer.SelectedValue == "ALP002" || ddlCustomer.SelectedValue == "AUT003" || ddlCustomer.SelectedValue == "AUT004")
            {
                divALP.Visible = true;
                divBMW.Visible = false;
                divBar.Visible = false;
                divSeq.Visible = false;
                grdScans.Columns[2].Visible = true;
                grdScans.Columns[1].Visible = false;
                btnPrintBarcode.Visible = true;
                // GenerateSeqn();
            }
            else
            {
                divALP.Visible = false;
                divBMW.Visible = true;
                divBar.Visible = false;
                divSeq.Visible = false;
                grdScans.Columns[1].Visible = true;
                grdScans.Columns[2].Visible = false;
                btnPrintBarcode.Visible = false;
            }
        }
    }
}