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
    public partial class UOPCreate : System.Web.UI.Page
    {

            protected void Page_Load(object sender, EventArgs e)
            {

                if (!IsPostBack)
                {
                    //bindBranch();
                if (Request.QueryString["Code"] != null)
                {
                    var id = Request.QueryString["Code"].ToString();
                    FillData(id);
                    divTest.InnerText = "Editing Unit Of Package(UOP)";
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
                string qry = "Select * from UOPMaster where UOP='" + txtCode.Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        txtSurName.Text = dt.Rows[0]["UOP"].ToString();
                        chkActive.Checked = true;
                        if (dt.Rows[0]["IsActive"] != DBNull.Value)
                            chkActive.Checked = Convert.ToBoolean(dt.Rows[0]["IsActive"]);
                        
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
                string qry = "Select * from UOPMaster where UOP='" + txtCode.Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('UOP already exisits.');", true);
                        return;
                    }
                }
                HiddenField hdLoginId = (HiddenField)this.Master.FindControl("hdnLoginId");

                var userid = hdLoginId.Value;
                HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");

                var userName = hdUserName.Value;
                var Active = 0;
                if (chkActive.Checked)
                    Active = 1;
                qry = "Insert into UOPMaster(IsActive,UOP,CreatedBy,CreatedDate)values("+Active+",'" + txtSurName.Text + "','" + userName + "',getdate())";

                using (SqlCommand cmd = new SqlCommand(qry, con))
                {



                    cmd.ExecuteNonQuery();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Unit Of Package Successfully Created.');", true);
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
                var Active = 0;
                if (chkActive.Checked)
                    Active = 1;
                qry = "UPDATE UOPMaster SET IsActive=" +Active+", UOP = '" + txtSurName.Text + "', ModifiedBy = '" + userName + "', ModifiedDate = getdate() WHERE UOP = '" + id + "'";

                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.ExecuteNonQuery();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Unit Of Package Successfully Updated.');", true);
                    txtCode.Text = "";
                    txtSurName.Text = "";
                }
            }
        }
    }
}