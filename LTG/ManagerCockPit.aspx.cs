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

namespace LTG
{
    public partial class ManagerCockPit : System.Web.UI.Page
    {
        protected void grdBin_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");
                string hu = grdBin.DataKeys[e.Row.RowIndex].Value.ToString(); // Use HU as the key

                if (ViewState["CheckedBoxes"] is List<string> checkedBoxes && checkedBoxes.Contains(hu))
                {
                    chkSelect.Checked = true;
                }
            }
        }
        private void loadUsers()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd;
            con.Open();
            string qry = "select * from Users as U inner join UserRoles as R on R.Username=U.Username Where R.StockRoles=1";
            if (txtStock.Text != "")
                qry += " and U.Username like'%" + txtStock.Text + "%'";

            cmd = new SqlCommand(qry, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            con.Close();
            GridView1.DataSource = ds.Tables[0];
            GridView1.DataBind();

        }
        private List<string> GetSelectedHUs()
        {
            List<string> selectedHUs = new List<string>();

            foreach (GridViewRow row in grdBin.Rows)
            {
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect != null && chkSelect.Checked)
                {
                    string hu = grdBin.DataKeys[row.RowIndex].Value.ToString();
                    selectedHUs.Add(hu);
                }
            }

            return selectedHUs;
        }
        private DataTable loadData()
        {
            SaveCheckboxStates();
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd;
            con.Open();
            string qry = "select QtyOnHand-Qty as variance,ContainerId,Bin,HU,QtyOnHand,Qty,AllocatedTo,STN,CustomerCode from StocktakeCounterInstruction Where MAllocatedTo is null and  QtyOnHand-Qty<>0";

            cmd = new SqlCommand(qry, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            con.Close();
            return ds.Tables[0];


        }

        private void loadGrid()
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;
            SaveCheckboxStates();
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd;
            con.Open();
            string qry = "select QtyOnHand-Qty as variance,ContainerId,Bin,HU,QtyOnHand,Qty,AllocatedTo,STN,CustomerCode from StocktakeCounterInstruction Where MAllocatedTo is null and QtyOnHand-Qty<>0";
            if (txtSearchContainer.Text != "")
                qry += " and (Bin like'" + txtSearchContainer.Text + "%' or HU like'" + txtSearchContainer.Text + "%')";
            txtSearchContainer.Text = "";
            cmd = new SqlCommand(qry, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            con.Close();
            grdBin.DataSource = ds.Tables[0];
            grdBin.DataBind();

        }
        protected void SaveCheckboxStates()
        {
            List<string> checkedBoxes = ViewState["CheckedBoxes"] as List<string> ?? new List<string>();

            foreach (GridViewRow row in grdBin.Rows)
            {
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect != null)
                {
                    string hu = grdBin.DataKeys[row.RowIndex].Value.ToString();

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

            ViewState["CheckedBoxes"] = checkedBoxes;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadGrid();
                loadUsers();
            }

        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            popupAdd.Visible = true;
        }
        protected void grdBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void imgExcel_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void txtSearchContainer_TextChanged(object sender, EventArgs e)
        {
            loadGrid();
        }

        protected void imgSearch_Click(object sender, ImageClickEventArgs e)
        {
            loadGrid();
        }

        protected void btnAddclose_Click(object sender, EventArgs e)
        {
            popupAdd.Visible = false;
        }

        protected void btnAddSave_Click(object sender, EventArgs e)
        {

        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            AllocateStock();
        }
        protected void AllocateStock()
        {
            loadGrid();
            GridViewRow selectedRow = GridView1.SelectedRow;
            if (selectedRow == null)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('No row selected in GridView1.');", true);
                return;
            }

            string userName = selectedRow.Cells[1].Text;
            HiddenField hdUserName = (HiddenField)this.Master.FindControl("hdnUserName");
            if (hdUserName == null || string.IsNullOrEmpty(hdUserName.Value))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Hidden username not found or invalid.');", true);
                return;
            }

            string userName1 = hdUserName.Value;
            string connectionString = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            // Parameterized queries to prevent SQL injection
            //string query = "UPDATE StockMaster SET Counter1Allocate = @UserName, AllocatedBy = @AllocatedBy, AllocatedDate = GETDATE() WHERE HU = @Bin";
            string query1 = "UPDATE StocktakeCounterInstruction SET MAllocatedTo = @UserName, MAllocatedBy = @AllocatedBy, MAllocatedDate = GETDATE() WHERE HU = @Bin";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                foreach (GridViewRow row in grdBin.Rows)
                {
                    CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                    if (chkSelect != null && chkSelect.Checked)
                    {
                        string bin = row.Cells[3].Text;

                       // ExecuteUpdate(conn, query, userName, userName1, bin);
                        ExecuteUpdate(conn, query1, userName, userName1, bin);
                    }
                }
            }

            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Stock Take Allocation Completed Successfully!');", true);
            popupAdd.Visible = false;
            loadGrid();
        }

        private void ExecuteUpdate(SqlConnection conn, string query, string userName, string allocatedBy, string bin)
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@AllocatedBy", allocatedBy);
                cmd.Parameters.AddWithValue("@Bin", bin);

                cmd.ExecuteNonQuery();
            }
        }

        protected void txtStock_TextChanged(object sender, EventArgs e)
        {
            loadUsers();
        }

        protected void imgStock_Click(object sender, ImageClickEventArgs e)
        {
            loadUsers();
        }

        protected void btnShowAllocate_Click(object sender, EventArgs e)
        {


            // Retrieve the full data source (replace this with your actual data source logic)
            DataTable dt = loadData();
            List<string> selectedHUs = ViewState["CheckedBoxes"] as List<string> ?? new List<string>();
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

        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            loadGrid();
        }
    }
}