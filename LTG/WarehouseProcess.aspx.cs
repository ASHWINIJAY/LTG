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
    public partial class WarehouseProcess : System.Web.UI.Page
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
                            string script = "alert(\"This Binning Number Already used by other user,Your new Binning Number is: " + txtGRN.Text + "\");";
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
        protected void btnCusNext_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlCustomer.SelectedIndex == 0)
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
            getIStorageFee();
            if (hdnInboundFee.Value == "")
            {
                string script = "alert(\"Please setup the storage fee. \");";
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
            GetContainer();
            divHeader.InnerText = "Binning Process for - " + ddlCustomer.SelectedItem.Text;
            txtBin.Focus();
        }
        protected void btnYes_Click(object sender, EventArgs e)
        {
            divCustomer.Visible = false;
            divScan.Visible = true;
            GetContainer();
            divHeader.InnerText = "Binning Process for - " + ddlCustomer.SelectedItem.Text;
            txtBin.Focus();
        }
        private void GenerateGRN()
        {
            string dtMonth = DateTime.Now.ToString("dd-MM-yy").Replace("-", "");
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;
            
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "select count(distinct(BinningNumber))+1 FROM WarehouseProcess";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        string output = FormatNumberWithLeadingZeros(dt.Rows[0][0].ToString(), 9);
                        txtGRN.Text = "BINNING-" + dtMonth + "-" + output;
                        if (CheckGRN())
                        {
                            output = FormatNumberWithLeadingZeros((Convert.ToInt32(dt.Rows[0][0].ToString()) + 1).ToString(), 9);
                            txtGRN.Text = "BINNING-" + dtMonth + "-" + output;
                        }
                        // ddlBranch.da
                    }
                }
            }

        }
        private bool CheckGRN()
        {
            string dtMonth = DateTime.Now.ToString("dd-MM-yy").Replace("-", "");
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;
            
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "select BinningNumber FROM WarehouseProcess where BinningNumber ='" + txtGRN.Text + "'";
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
        static string FormatNumberWithLeadingZeros(string number, int totalLength)
        {
            return number.PadLeft(totalLength, '0');
        }
        private bool checkBin()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from Bin where BinName='" + txtBin.Text + "' and Isnull(IsActive,1)<>0";
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
        protected void btnSave_Click(object sender, EventArgs e)
        {
            save();
            FillGrid();
        }
        private void save()
        {
            txtHU.Text = txtHU.Text.Trim();
            txtBin.Text = txtBin.Text.Trim();
            if (txtHU.Text == "")
            {
                string script = "alert(\"HU Cannot be empty\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                txtHU.Text = "";
                txtHU.Focus();
                return;
            }
            if (txtBin.Text == "")
            {
                string script = "alert(\"Bin Cannot be empty\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                txtBin.Text = "";
                txtBin.Focus();
                return;
            }
            if (CheckCycleCount())
            {
                string script = "alert(\"Stock take initiated for this Bin,You cannot proceed until complete it.\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                txtHU.Text = "";
                txtHU.Focus();
                return;
            }
            if (CheckStockTake())
            {
                string script = "alert(\"Stock take initiated for this HU,You cannot proceed until complete it.\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                txtHU.Text = "";
                txtHU.Focus();
                return;
            }
            if (checkBin())
            {

            }
            else
            {
                string script = "alert(\"Bin is not available\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                txtBin.Text = "";
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
            if (!checkValidHU())
            {
                string script = "alert(\"HU not avaliable in Inbound\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                txtHU.Text = "";
                txtHU.Focus();
                return;
            }
            if (!checkValidCustomerHU())
            {
                string script = "alert(\"This HU is not linked to this customer and cannot be scanned\");";
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
                    string script = "alert(\"This BinningNumber Already used by other user,Your new BinningNumber is: " + txtGRN.Text + "\");";
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
                string qry = "Insert into WarehouseProcess(Bin,BranchId,BranchName,CustomerCode,CustomerName,HU,QtyIn,QtyOnHand,UnitStorageCost,TotalStorageCost,UserName,ScannedInTime,CreatedBy,CreatedDate,UniqueId,BinningNumber)values('" + txtBin.Text + "'," + ddlBranch.SelectedValue + ",'" + ddlBranch.SelectedItem.Text + "','" + ddlCustomer.SelectedValue + "','" + ddlCustomer.SelectedItem.Text + "','" + txtHU.Text + "','" + txtQty.Text + "','" + txtQty.Text + "'," + hdnInboundFee.Value + "," + hdnInboundFee.Value + ",'" + userid + "','"+txtDate.Text+ "','" + userName + "',getdate()," + hdnContainer.Value + ",'" + txtGRN.Text + "')";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                cmd1.ExecuteNonQuery();
                hdninitalNumber.Value = "0";
                // qry = "Insert into WarehouseProcess(Bin,BranchId,BranchName,CustomerCode,CustomerName,HU,QtyIn,QtyOnHand,UnitStorageCost,TotalStorageCost,UserName,ScannedInTime,CreatedBy,CreatedDate)values('" + hdnBin.Value + "'," + ddlBranch.SelectedValue + ",'" + ddlBranch.SelectedItem.Text + "','" + ddlCustomer.SelectedValue + "','" + ddlCustomer.SelectedItem.Text + "','" + txtHU.Text + "','" + txtQty.Text + "','" + txtQty.Text + "'," + hdnInboundFee.Value + "," + hdnInboundFee.Value + ",'" + userid + "',getdate(),'" + userName + "',getdate())";
                // cmd1 = new SqlCommand(qry, con);
                //cmd1.ExecuteNonQuery();
            }
            txtHU.Text = "";
            txtBin.Text = "";
            txtBin.Focus();
            
        }
        private bool checkHU()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from WarehouseProcess where HU='" + txtHU.Text + "'";
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

        private bool checkValidHU()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from Inbound where HU='" + txtHU.Text + "' and isnull(Returned,0)=0";
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
        private bool checkValidCustomerHU()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from Inbound where HU='" + txtHU.Text + "' and isnull(Returned,0)=0";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["CustomerCode"].ToString() == ddlCustomer.SelectedValue)
                            return true;
                        else
                            return false;
                        // ddlBranch.da
                    }
                }
            }
            return false;
        }
        protected void FillGrid()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "SELECT  *  FROM WarehouseProcess where BinningNumber='" + txtGRN.Text + "' order by CreatedDate";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    grdScans.DataSource = dt;
                    grdScans.DataBind();
                    divHeader.InnerText = "Binning Process - Total Scans: " + dt.Rows.Count;
                }
            }
        }
        protected void CheckIncomplete()
        {
            if (Session["LoginId"] == null)
            {
                Response.Redirect("Dashboard.aspx");
            }
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "SELECT  *  FROM WarehouseProcess where Completed=0 and UserName='" + Session["LoginId"].ToString() + "' order by ScannedInTime desc";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        string redirectUrl = "WarehouseProcess.aspx?id=" + dt.Rows[0]["BinningNumber"].ToString(); // The URL to open in a new tab

                        // Inject the JavaScript confirmation dialog with a redirect in a new tab on confirmation
                        string script = "var userConfirmed = confirm('You have existing Binning Process is incomplete, Do you want to continue?');" +
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
                string qry = "SELECT  *  FROM WarehouseProcess where Completed=0 and BinningNumber='" + GRN + "' order by ScannedInTime";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        ddlBranch.SelectedValue = dt.Rows[0]["BranchId"].ToString();
                        ddlCustomer.SelectedValue = dt.Rows[0]["CustomerCode"].ToString();
                        txtDate.Text = Convert.ToDateTime(dt.Rows[0]["ScannedInTime"]).ToString("dd/MMM/yyyy");
                       // txtContainer.Text = dt.Rows[0]["ContainerId"].ToString();
                        txtGRN.Text = GRN;
                        getIStorageFee();
                        divCustomer.Visible = false;
                        divScan.Visible = true;
                        divHeader.InnerText = "Binning Process for - " + ddlCustomer.SelectedItem.Text;
                       // txtContainer1.Text = txtContainer.Text;
                        txtHU.Focus();
                        FillGrid();
                    }
                }
            }
        }
        protected void btnComplete_Click(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();

                string qry = "Update WarehouseProcess set Completed=1 where BinningNumber='" + txtGRN.Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                cmd1.ExecuteNonQuery();
            }
            string redirectUrl = "Binning.aspx?id=" + txtGRN.Text; // The URL to open in a new tab

            // Inject the JavaScript confirmation dialog with a redirect in a new tab on confirmation
            string script = "var userConfirmed = confirm('Do you want to print this data?');" +
                            "if (userConfirmed) {" +
                            "    window.open('" + redirectUrl + "', '_blank');" +
                            "}";

            // Register the script to run on the client-side
            ClientScript.RegisterStartupScript(this.GetType(), "confirmRedirectNewTab", script, true);
            txtHU.Text = "";
            txtBin.Text = "";
            divCustomer.Visible = true;
            divScan.Visible = false;
            GenerateGRN();
        }
        private bool CheckStockTake()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "select HU FROM StockMaster where HU='" + txtHU.Text + "'";
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
        private bool CheckCycleCount()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "select Bin FROM BinAccuracyMaster where Bin='" + txtBin.Text + "'";
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
        private void getIStorageFee()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select StorageFee,ContractDate from Customers where CustomerCode='" + ddlCustomer.SelectedValue + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][1] != DBNull.Value)
                            hdnContractDate.Value = dt.Rows[0][1].ToString();
                        else
                            hdnContractDate.Value = "";
                        if (dt.Rows[0][0] != DBNull.Value)
                            hdnInboundFee.Value = dt.Rows[0][0].ToString();
                        else
                            hdnInboundFee.Value = "";
                        // ddlBranch.da
                    }
                }
            }
        }
        protected void GetContainer()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "SELECT  isnull(max([UniqueId])+1,1)  FROM WarehouseProcess";
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
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtHU.Text = "";
        }

        protected void txtBin_TextChanged(object sender, EventArgs e)
        {
            save();
           
        }

        protected void txtBin_TextChanged1(object sender, EventArgs e)
        {
            if (checkBin())
            {

            }
            else
            {
                string script = "alert(\"Bin Does Not Exist\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                txtBin.Text = "";
                txtBin.Focus();
                return;
            }
            txtHU.Focus();
        }

        protected void txtHU_TextChanged(object sender, EventArgs e)
        {
            save();
            FillGrid();
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();

                string qry = "Delete  from WarehouseProcess where UniqueId='" + hdnContainer.Value + "'  and HU not in(select HU from PickingProcess) and isnull(Completed,0)=0";
                SqlCommand cmd1 = new SqlCommand(qry, con);

                //cmd1.ExecuteNonQuery();
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

                string qry = "Delete  from WarehouseProcess where UniqueId='" + hdnContainer.Value + "' and HU not in(select HU from PickingProcess) and isnull(Completed,0)=0 ";
                SqlCommand cmd1 = new SqlCommand(qry, con);

                cmd1.ExecuteNonQuery();
                Response.Redirect("WarehouseProcess.aspx");
                // ddlBranch.da

            }
        }
        protected void btnClose_Click(object sender, EventArgs e)
        {
            popup.Visible = false;
        }

        protected void btnSaveDate_Click(object sender, EventArgs e)
        {
            if (txtNewdate.Text == "")
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
                        if (Convert.ToDateTime(txtNewdate.Text).Date <= Convert.ToDateTime(dt.Rows[0][0].ToString()).Date)
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
                 checkCountQry = "SELECT COUNT(*) FROM Supervisor where Password='" + txtSupPassword.Text + "'";
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
    }
}