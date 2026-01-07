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
    public partial class ReturnReason : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
               // bindBranch();
                if (Request.QueryString["Code"] != null)
                {
                    var id = Request.QueryString["Code"].ToString();
                    FillData(id);
                    divTest.InnerText = "Editing Reason";
                }
            }
        }

        
        private void FillData(string id)
        {
            txtCode.Text = id;

            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from ReturnReason where ReturnCode='" + txtCode.Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        txtSurName.Text = dt.Rows[0]["ReturnReason"].ToString();
                       ddlReturnType.SelectedItem.Text = dt.Rows[0]["ReturnType"].ToString();
                    }
                }
            }
        }
        protected void btnCreate_Click(object sender, EventArgs e)
        {

            if (Request.QueryString["Code"] != null)
            {
                var id = Request.QueryString["Code"].ToString();
                Update(id);
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
                string qry = "Select * from ReturnReason where ReturnCode='" + txtCode.Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('ReasonCode already exisits.');", true);
                        return;
                    }
                }
                HiddenField hdLoginId = (HiddenField)this.Master.FindControl("hdnLoginId");

                var userid = hdLoginId.Value;
                HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");

                var userName = hdUserName.Value;
               
                qry = "Insert into ReturnReason(ReturnType,ReturnCode,ReturnReason,CreatedBy,CreatedTime)values('" + ddlReturnType.SelectedItem.Text + "','" + txtCode.Text + "','" + txtSurName.Text + "','" + userName + "',getdate())";

                using (SqlCommand cmd = new SqlCommand(qry, con))
                {



                    cmd.ExecuteNonQuery();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Return Reason Successfully Created.');", true);
                    txtCode.Text = "";
                    txtSurName.Text = "";
                }
            }
        }
        private void Update(string id)
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "";


                HiddenField hdLoginId = (HiddenField)this.Master.FindControl("hdnLoginId");

                var userid = hdLoginId.Value;
                HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");

                var userName = hdUserName.Value;
               
                qry = "UPDATE ReturnReason SET ReturnType='" + ddlReturnType.SelectedItem.Text + "', ReturnCode = '" + txtCode.Text + "', ReturnReason = '" + txtSurName.Text + "', ModifiedBy = '" + userName + "', ModifiedTime = getdate() WHERE ReturnCode = '" + id + "'";

                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.ExecuteNonQuery();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Return Reason Successfully Updated.');", true);
                    txtCode.Text = "";
                    txtSurName.Text = "";
                }
            }
        }
    }
}