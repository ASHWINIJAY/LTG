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
    public partial class StockPwd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Password and Confirm Password should be match.');", true);
                return;
            }
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();

                // Check if the table has any records
                string checkCountQry = "SELECT COUNT(*) FROM StockTakeSetup";
                SqlCommand checkCmd = new SqlCommand(checkCountQry, con);
                int recordCount = (int)checkCmd.ExecuteScalar();

                string qry = "";
                if (recordCount == 0)
                {
                    // No records exist, so insert a new record
                    qry = "INSERT INTO StockTakeSetup (Password,ModifiedDate,ModifiedBy,ModifiedId) VALUES (@Password,getdate(),@ModifiedBy,@ModifiedId)";
                }
                else
                {
                    // Records exist, so update the existing user
                    qry = "UPDATE StockTakeSetup SET Password = @Password,ModifiedDate=getdate(),ModifiedBy=@ModifiedBy,ModifiedId=@ModifiedId";
                }

                HiddenField hdLoginId = (HiddenField)this.Master.FindControl("hdnLoginId");

                var userid = hdLoginId.Value;
                HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");

                var userName = hdUserName.Value;

                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                    cmd.Parameters.AddWithValue("@ModifiedBy", userName);
                    cmd.Parameters.AddWithValue("@ModifiedId", userid);

                    cmd.ExecuteNonQuery();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Password successfully updated.');", true);
                }

                txtConfirmPassword.Text = "";
                txtPassword.Text = "";
            }

        }
    }
}