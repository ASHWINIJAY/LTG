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
    public partial class MonthEnd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillData();
                txtMonthEnd.Attributes["max"] = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }
        private void FillData()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select MonthEndDate,[Password] from MonthEnd";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0 && dt.Rows[0][0] != null)
                    {
                        txtPrevMonthEnd.Text = Convert.ToDateTime(dt.Rows[0][0]).ToString("dd/MMM/yyyy");
                   hdnMonthendPwd.Value = dt.Rows[0][1].ToString();
                        // ddlBranch.da
                    }
                }
            }
        }
        protected void btnCreate_Click(object sender, EventArgs e)
        {
           if(hdnMonthendPwd.Value != txtMonthEndPwd.Text)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Month-End Password Incorrect,Please try again.');", true);
                return;
            }
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();

                // Check if the table has any records
                string checkCountQry = "SELECT COUNT(*) FROM MonthEnd";
                SqlCommand checkCmd = new SqlCommand(checkCountQry, con);
                int recordCount = (int)checkCmd.ExecuteScalar();

                string qry = "";
                if (recordCount == 0)
                {
                    // No records exist, so insert a new record
                    qry = "INSERT INTO MonthEnd(MonthEndDate,ModifiedDate,ModifiedBy) VALUES (@MonthEndDate,getdate(),@ModifiedBy)";
                }
                else
                {
                    // Records exist, so update the existing user
                    qry = "UPDATE MonthEnd SET MonthEndDate = @MonthEndDate,ModifiedDate=getdate(),ModifiedBy=@ModifiedBy";
                }

                HiddenField hdLoginId = (HiddenField)this.Master.FindControl("hdnLoginId");

                var userid = hdLoginId.Value;
                HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");

                var userName = hdUserName.Value;

                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.AddWithValue("@MonthEndDate", txtMonthEnd.Text);
                    cmd.Parameters.AddWithValue("@ModifiedBy", userName);

                    cmd.ExecuteNonQuery();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Month-End Date successfully updated.');", true);
                }

                txtMonthEnd.Text = "";
                FillData();
            }

        }
    }
}