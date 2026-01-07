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
    public partial class ReversePick : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                 bindCustomer();
                // FillGrid();
                GenerateGRN();
            }
        }
        private void bindCustomer()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from ReturnReason where ReturnType='Pickup Return'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        ddlReason.DataSource = dt;
                        ddlReason.DataBind();
                        ddlReason.DataTextField = "ReturnReason";
                        ddlReason.DataValueField = "ReturnCode";
                        ddlReason.DataBind();
                        // ddlBranch.da
                    }
                    ddlReason.Items.Insert(0, new ListItem("Select ReturnReason", ""));
                }
            }
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
            divChangeDate.Visible = true;
        }
        static string FormatNumberWithLeadingZeros(string number, int totalLength)
        {
            return number.PadLeft(totalLength, '0');
        }
        private void GenerateGRN()
        {
            string dtMonth = DateTime.Now.ToString("dd-MM-yy").Replace("-", "");
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;
            txtDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "select count(distinct(RPICKN))+1 FROM ReversePick";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        string output = FormatNumberWithLeadingZeros(dt.Rows[0][0].ToString(), 9);
                        txtGRN.Text = "RPICKN-" + dtMonth + "-" + output;
                        
                        // ddlBranch.da
                    }
                }
            }

        }
        protected void btnBrowse_Click(object sender, EventArgs e)
        {
            FillGrid();
            popup.Visible = true;
        }

        protected void btnComplete_Click(object sender, EventArgs e)
        {

            save();
        }
        private bool checkHU()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from PickingProcess where HU='" + txtRetHU.Text + "' and Status<>'Outbounded' and isnull(Returned,0)=0 ";
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
        private bool checkGDN()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from PickingProcess where PickNumber='" + txtGDN.Text+ "' and Status<>'Outbounded' and isnull(Returned,0)=0 ";
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

        private bool checkBin()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from Inbound";
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

        }
        protected void FillGrid()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "SELECT  distinct PickNumber,Cast(DateTimeofScan as Date) as OutboundDate  FROM PickingProcess where Status<>'Outbounded' and isnull(Returned,0)=0 ";
                if (txtSearchContainer.Text != "")
                    qry += " and PickNumber like '%" + txtSearchContainer.Text + "%'";
                qry += " order by Cast(DateTimeofScan as Date) desc";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    grdScans.DataSource = dt;
                    grdScans.DataBind();
                }
            }
        }
        protected void FillHUGrid(string GDN)
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "SELECT  distinct HU  FROM PickingProcess Where isnull(Returned,0)<>1 and Status<>'Outbounded' and PickNumber='" + GDN + "'";
                if (txtSearchHU.Text != "")
                    qry += " and HU like '%" + txtSearchHU.Text + "%'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
            }
        }
        protected void txtSearchContainer_TextChanged(object sender, EventArgs e)
        {
            FillGrid();
        }

        protected void imgSearch_Click(object sender, ImageClickEventArgs e)
        {
            FillGrid();
        }
        protected void grdScans_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = grdScans.SelectedRow;
            hdnContainer.Value = row.Cells[1].Text;
             txtGDN.Text = row.Cells[1].Text;
            // txtBin.Text = row.Cells[1].Text;
            popup.Visible = false;
            FillHUGrid(hdnContainer.Value);
            divHUPopup.Visible = true;
        }

        protected void txtHU_TextChanged(object sender, EventArgs e)
        {
            if (checkBin())
            {

            }
            else
            {
                string script = "alert(\"HU is not available\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
               // txtHU.Text = "";
                return;
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            popup.Visible = false;
        }

        protected void txtGDN_TextChanged(object sender, EventArgs e)
        {
            if (checkGDN())
            {
                FillHUGrid(txtGDN.Text);
                divHUPopup.Visible = true;
            }
            else
            {
                string script = "alert(\"PickNumber is not available\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                 txtGDN.Text = "";
                return;
            }
            
        }

       

        protected void btnHuPopup_Click(object sender, EventArgs e)
        {
            divHUPopup.Visible = false;
        }

        protected void txtHU_TextChanged1(object sender, EventArgs e)
        {

        }

        protected void imgHU_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void txtSearchHU_TextChanged(object sender, EventArgs e)
        {

        }
        List<string> selectedHu = new List<string>();
        protected void btnSaveHU_Click(object sender, EventArgs e)
        {
            divHUPopup.Visible = false;
            List<string> selectedValues = new List<string>();

            foreach (GridViewRow row in GridView1.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkSelect");
                if (chk != null && chk.Checked)
                {
                    string cellValue = row.Cells[2].Text; // Assuming ID is in column index 1
                    selectedValues.Add(cellValue);
                }
            }

            if (selectedValues.Count > 0)
            {
               // string idList = string.Join(",", selectedValues); // "1,2,3"
                string result = string.Join(", ", selectedValues.ConvertAll(s => $"'{s}'"));
                string query = "SELECT distinct HU,PickNumber FROM PickingProcess WHERE  HU IN (" + result + ")";
                string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();
                    
                    SqlCommand cmd1 = new SqlCommand(query, con);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        GridView2.DataSource = dt;
                        GridView2.DataBind();
                    }
                }

            }
        }
        protected void ReloadGrid()
        {
            List<string> selectedValues = ViewState["CheckedBoxes"] as List<string> ?? new List<string>();
            //List<string> selectedValues = new List<string>();

            
            if (selectedValues.Count > 0)
            {
                // string idList = string.Join(",", selectedValues); // "1,2,3"
                string result = string.Join(", ", selectedValues.ConvertAll(s => $"'{s}'"));
                string query = "SELECT distinct HU,PickNumber FROM PickingProcess WHERE  HU IN (" + result + ")";
                string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();

                    SqlCommand cmd1 = new SqlCommand(query, con);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        GridView2.DataSource = dt;
                        GridView2.DataBind();
                    }
                }

            }
        }

        protected void btnChangeDate_Click(object sender, EventArgs e)
        {
            divChangeDate.Visible = false;
        }
        private void save()
        {
            if (ddlReason.SelectedIndex==0)
            {
                string script = "alert(\"Please select return reason\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);

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
                foreach (GridViewRow row in GridView2.Rows)
                {
                    string Hu= row.Cells[1].Text;

                    string qry = "Select PickNumber,CustomerName,CustomerCode,HU,Qty,'' as t,'' as t1,DateTimeofScan,FromBinLocation from PickingProcess Where HU='" + Hu + "' and isnull(Returned,0)=0";
                    SqlCommand cmd1 = new SqlCommand(qry, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);

                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        var unitCost = "0";
                        var days = (Convert.ToDateTime(txtDate.Text) - Convert.ToDateTime(dt.Rows[0][7].ToString())).Days;
                        if(days<0)
                        {
                            string script = "alert(\"Returned Date Should not be less then Pickup Date\");";
                            ScriptManager.RegisterStartupScript(this, GetType(),
                                                  "ServerControlScript", script, true);
                            break;
                        }
                        if ( days> 0)
                        {
                            unitCost = dt.Rows[0][6].ToString();
                        }
                       
                        qry = "Update PickingProcess set Returned=1 where HU='" + Hu + "'";
                        cmd1 = new SqlCommand(qry, con);
cmd1.ExecuteNonQuery();
                        qry = "Insert into ReversePick(ReasonCode,Reason,RPICKN,PickNumber,CustomerName,CustomerCode,HU,Qty,Loginname,DateTimeofScan,ReturnDateTimeofScan,CreatedBy,CreatedDate,Bin)values('" + ddlReason.SelectedValue + "','" + ddlReason.SelectedItem.Text +"','" + txtGRN.Text + "','" + dt.Rows[0][0].ToString() + "','" + dt.Rows[0][1].ToString() + "','" + dt.Rows[0][2].ToString() + "','" + dt.Rows[0][3].ToString() + "',1,'" + userid + "','" + dt.Rows[0][7].ToString() + "','" + txtDate.Text + "','" + userName + "',getdate(),'" + dt.Rows[0][7].ToString() + "')";
                        cmd1 = new SqlCommand(qry, con);
                        cmd1.ExecuteNonQuery();
                        
                    }
                }
                bindCustomer();
                // FillGrid();
                GenerateGRN();
                DataTable dt1 = new DataTable();
                txtGDN.Text = "";
                // Add columns to the DataTable
                dt1.Columns.Add("HU");
                dt1.Columns.Add("PickNumber");

                // Bind the empty DataTable to the GridView
                GridView2.DataSource = dt1;
                GridView2.DataBind();
                string script1 = "alert(\"Saved Successfully\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script1, true);
                ViewState["CheckedBoxes"] = null; 
            }
        }

        protected void txtRetHU_TextChanged(object sender, EventArgs e)
        {   if (!checkHU())
            {
                string script1 = "alert(\"HU is not available for reverse process\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script1, true);
            }
            else
            {
                List<string> checkedBoxes = ViewState["CheckedBoxes"] as List<string> ?? new List<string>();
                checkedBoxes.Add(txtRetHU.Text);
                ViewState["CheckedBoxes"] = checkedBoxes;
                ReloadGrid();
                txtRetHU.Text = "";
            }
        }
    }
}