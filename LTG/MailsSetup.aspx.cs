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
    public partial class MailsSetup : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
              
                    FillData();
                   
            }
        }

       
        private void FillData()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from MailSetup Where Type=" +ddlReturnType.SelectedValue;
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        txtMails.Text = dt.Rows[0][0].ToString();

                    }
                    else
                        txtMails.Text = "";
                }
            }
        }
        protected void btnCreate_Click(object sender, EventArgs e)
        {

            save();
        }
        private void save()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from MailSetup Where Type=" + ddlReturnType.SelectedValue;
                SqlCommand cmd1 = new SqlCommand(qry, con);
                HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");

                var userName = hdUserName.Value;
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        qry = "Update MailSetup set  MailIds='" + txtMails.Text + "',UpdatedDate=Getdate(),UpdatedBy='" + hdUserName.Value + "'  Where Type=" + ddlReturnType.SelectedValue;

                    }
                    else
                        qry = "Insert into MailSetup(MailIds,UpdatedDate,UpdatedBy,Type)Values('" + txtMails.Text + "',GetDate(),'" + hdUserName.Value + "','" + ddlReturnType.SelectedValue + "')";
                    cmd1 = new SqlCommand(qry, con);
                    cmd1.ExecuteNonQuery();
                }
            }


                   
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", "alert('Successfully Updated.');", true);
                    
               
        }

        protected void ddlReturnType_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillData();
        }
    }
}