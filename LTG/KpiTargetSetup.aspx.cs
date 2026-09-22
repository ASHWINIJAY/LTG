using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI;

namespace LTG
{
    public partial class KpiTargetSetup : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadTarget();
        }

        private void LoadTarget()
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString))
            using (SqlCommand command = new SqlCommand(@"
SELECT TOP (1) OnTrackMinimum, AboveTargetMinimum
FROM dbo.KpiTargets
WHERE IsActive = 1
ORDER BY TargetId DESC;", connection))
            {
                connection.Open();
                KpiData.EnsureKpiTargetsTable(connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtOnTrackMinimum.Text = Convert.ToDecimal(reader["OnTrackMinimum"]).ToString(CultureInfo.InvariantCulture);
                        txtAboveTargetMinimum.Text = Convert.ToDecimal(reader["AboveTargetMinimum"]).ToString(CultureInfo.InvariantCulture);
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            decimal onTrackMinimum;
            decimal aboveTargetMinimum;
            if (!decimal.TryParse(txtOnTrackMinimum.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out onTrackMinimum) ||
                !decimal.TryParse(txtAboveTargetMinimum.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out aboveTargetMinimum) ||
                onTrackMinimum < 0 || aboveTargetMinimum <= onTrackMinimum)
            {
                lblMessage.Text = "Enter valid values. Above Target must be greater than On Track.";
                lblMessage.ForeColor = System.Drawing.Color.Firebrick;
                return;
            }

            string username = Request.Cookies["Username"] == null ? "Unknown" : Request.Cookies["Username"].Value;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString))
            using (SqlCommand command = new SqlCommand(@"
UPDATE dbo.KpiTargets SET IsActive = 0;
INSERT INTO dbo.KpiTargets (TargetName, OnTrackMinimum, AboveTargetMinimum, IsActive, UpdatedBy, UpdatedDate)
VALUES ('Scans per Hour', @OnTrackMinimum, @AboveTargetMinimum, 1, @UpdatedBy, GETDATE());", connection))
            {
                connection.Open();
                KpiData.EnsureKpiTargetsTable(connection);
                command.Parameters.AddWithValue("@OnTrackMinimum", onTrackMinimum);
                command.Parameters.AddWithValue("@AboveTargetMinimum", aboveTargetMinimum);
                command.Parameters.AddWithValue("@UpdatedBy", username);
                command.ExecuteNonQuery();
            }

            lblMessage.Text = "KPI targets saved successfully.";
            lblMessage.ForeColor = System.Drawing.Color.ForestGreen;
        }
    }
}