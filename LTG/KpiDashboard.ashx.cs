using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using System.Web.Script.Serialization;

namespace LTG
{
    public class KpiDashboard : IHttpHandler
    {
        public bool IsReusable { get { return false; } }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;

            int userId;
            if (!int.TryParse(GetCookie(context, "LoginId"), out userId))
            {
                Write(context, new { error = "Unauthorized" }, 401);
                return;
            }

            DateTime fromDate;
            DateTime toDate;
            if (!DateTime.TryParseExact(context.Request.QueryString["fromDate"], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out fromDate))
                fromDate = DateTime.Today;
            if (!DateTime.TryParseExact(context.Request.QueryString["toDate"], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out toDate))
                toDate = fromDate;
            fromDate = fromDate.Date;
            toDate = toDate.Date.AddDays(1);
            if (toDate <= fromDate)
                toDate = fromDate.AddDays(1);

            string activity = NormalizeFilter(context.Request.QueryString["activity"], "All Activities");
            string shift = NormalizeFilter(context.Request.QueryString["shift"], "All Shifts");
            int employeeId;
            int? employeeFilter = int.TryParse(context.Request.QueryString["employee"], out employeeId) ? (int?)employeeId : null;

            DataTable rows = LoadRows(fromDate.Date, toDate, activity, shift, employeeFilter);
            List<EmployeeOption> employeeOptions = LoadEmployees();
            KpiTarget target = LoadKpiTarget();
            Write(context, BuildDashboard(rows, employeeOptions, target, fromDate, toDate), 200);
        }

        private static KpiTarget LoadKpiTarget()
        {
            const string sql = @"
SELECT TOP (1) OnTrackMinimum, AboveTargetMinimum
FROM dbo.KpiTargets
WHERE IsActive = 1
ORDER BY TargetId DESC;";

            using (SqlConnection connection = new SqlConnection(KpiData.ConnectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                KpiData.EnsureKpiTargetsTable(connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new KpiTarget
                        {
                            OnTrackMinimum = Convert.ToDouble(reader["OnTrackMinimum"]),
                            AboveTargetMinimum = Convert.ToDouble(reader["AboveTargetMinimum"])
                        };
                    }
                }
            }

            return new KpiTarget { OnTrackMinimum = 80, AboveTargetMinimum = 100 };
        }

        private static List<EmployeeOption> LoadEmployees()
        {
            const string sql = @"
SELECT UserId, Username, FirstName, LastName
FROM Users
ORDER BY FirstName, LastName, Username;";

            var employees = new List<EmployeeOption>();
            using (SqlConnection connection = new SqlConnection(KpiData.ConnectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new EmployeeOption
                        {
                            EmployeeId = Convert.ToInt32(reader["UserId"]),
                            Name = BuildEmployeeName(reader["FirstName"], reader["LastName"], reader["Username"])
                        });
                    }
                }
            }
            return employees;
        }

        private static DataTable LoadRows(DateTime fromDate, DateTime toDate, string activity, string shift, int? employeeId)
        {
            const string sql = @"
WITH Scans AS
(
    SELECT TRY_CONVERT(int, Loginname) AS UserId, DateTimeofScan AS ScanTime, 'Inbound' AS Activity
    FROM Inbound
    WHERE DateTimeofScan >= @FromDate AND DateTimeofScan < @ToDate
    UNION ALL
    SELECT TRY_CONVERT(int, UserName), ScannedInTime, 'Warehouse'
    FROM WarehouseProcess
    WHERE ScannedInTime >= @FromDate AND ScannedInTime < @ToDate
    UNION ALL
    SELECT TRY_CONVERT(int, Loginname), DateTimeofScan, 'Outbound'
    FROM Outbound
    WHERE DateTimeofScan >= @FromDate AND DateTimeofScan < @ToDate
), ScanSummary AS
(
    SELECT UserId,
        SUM(CASE WHEN Activity = 'Inbound' THEN 1 ELSE 0 END) AS InboundScans,
        SUM(CASE WHEN Activity = 'Warehouse' THEN 1 ELSE 0 END) AS WarehouseScans,
        SUM(CASE WHEN Activity = 'Outbound' THEN 1 ELSE 0 END) AS OutboundScans,
        MIN(ScanTime) AS FirstScan
    FROM Scans
    WHERE UserId IS NOT NULL
      AND (@Activity IS NULL OR Activity = @Activity)
      AND (@Shift IS NULL OR CASE WHEN DATEPART(HOUR, ScanTime) BETWEEN 6 AND 17 THEN 'Day' ELSE 'Night' END = @Shift)
    GROUP BY UserId
), SessionSummary AS
(
    SELECT UserId,
        SUM(DATEDIFF(SECOND,
            CASE WHEN LoginTime < @FromDate THEN @FromDate ELSE LoginTime END,
            CASE
                WHEN ISNULL(LogoutTime, GETDATE()) > @ToDate THEN @ToDate
                ELSE ISNULL(LogoutTime, GETDATE())
            END)) / 3600.0 AS ActiveHours,
        MIN(CASE WHEN LoginTime < @FromDate THEN @FromDate ELSE LoginTime END) AS FirstLogin,
        MAX(LogoutTime) AS LastLogout
    FROM dbo.UserLoginSessions
    WHERE LoginTime < @ToDate
      AND ISNULL(LogoutTime, GETDATE()) > @FromDate
      AND (@EmployeeId IS NULL OR UserId = @EmployeeId)
    GROUP BY UserId
)
SELECT u.UserId, u.Username, u.FirstName, u.LastName,
       CASE WHEN DATEPART(HOUR, COALESCE(ss.FirstLogin, sc.FirstScan)) BETWEEN 6 AND 17 THEN 'Day' ELSE 'Night' END AS Shift,
       ISNULL(sc.InboundScans, 0) AS InboundScans,
       ISNULL(sc.WarehouseScans, 0) AS WarehouseScans,
       ISNULL(sc.OutboundScans, 0) AS OutboundScans,
       ISNULL(ss.ActiveHours, 0) AS ActiveHours,
       ss.FirstLogin, ss.LastLogout
FROM Users u
LEFT JOIN ScanSummary sc ON sc.UserId = u.UserId
LEFT JOIN SessionSummary ss ON ss.UserId = u.UserId
WHERE (sc.UserId IS NOT NULL OR ss.UserId IS NOT NULL)
  AND (@EmployeeId IS NULL OR u.UserId = @EmployeeId)
ORDER BY (ISNULL(sc.InboundScans, 0) + ISNULL(sc.WarehouseScans, 0) + ISNULL(sc.OutboundScans, 0)) DESC;";

            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(KpiData.ConnectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                connection.Open();
                KpiData.EnsureLoginSessionTable(connection);
                command.Parameters.AddWithValue("@FromDate", fromDate);
                command.Parameters.AddWithValue("@ToDate", toDate);
                command.Parameters.AddWithValue("@Activity", string.IsNullOrEmpty(activity) ? (object)DBNull.Value : activity);
                command.Parameters.AddWithValue("@Shift", string.IsNullOrEmpty(shift) ? (object)DBNull.Value : shift);
                command.Parameters.AddWithValue("@EmployeeId", employeeId.HasValue ? (object)employeeId.Value : DBNull.Value);
                adapter.Fill(table);
            }
            return table;
        }

        private static object BuildDashboard(DataTable rows, List<EmployeeOption> employeeOptions, KpiTarget target, DateTime fromDate, DateTime toDate)
        {
            var employees = new List<object>();
            var topEmployees = new List<object>();
            int totalScans = 0;
            int inbound = 0;
            int warehouse = 0;
            int outbound = 0;
            double totalHours = 0;
            int activeEmployees = 0;
            int dayScans = 0;
            int nightScans = 0;

            foreach (DataRow row in rows.Rows)
            {
                int inScans = Convert.ToInt32(row["InboundScans"]);
                int warehouseScans = Convert.ToInt32(row["WarehouseScans"]);
                int outScans = Convert.ToInt32(row["OutboundScans"]);
                double hours = Convert.ToDouble(row["ActiveHours"]);
                int scans = inScans + warehouseScans + outScans;
                string employeeName = BuildEmployeeName(row["FirstName"], row["LastName"], row["Username"]);
                string employeeShift = Convert.ToString(row["Shift"]);
                double scansPerHour = hours > 0 ? scans / hours : 0;
                string status = scansPerHour >= target.AboveTargetMinimum ? "Above Target" : scansPerHour >= target.OnTrackMinimum ? "On Track" : "Below Target";

                if (hours > 0) activeEmployees++;
                totalScans += scans;
                inbound += inScans;
                warehouse += warehouseScans;
                outbound += outScans;
                totalHours += hours;
                if (employeeShift == "Day") dayScans += scans; else nightScans += scans;

                employees.Add(new
                {
                    employee = employeeName,
                    employeeId = Convert.ToInt32(row["UserId"]),
                    shift = employeeShift,
                    login = FormatTime(row["FirstLogin"]),
                    logout = FormatTime(row["LastLogout"]),
                    activeHours = Math.Round(hours, 1),
                    inbound = inScans,
                    warehouse = warehouseScans,
                    outbound = outScans,
                    totalScans = scans,
                    scansPerHour = Math.Round(scansPerHour, 0),
                    status
                });
                topEmployees.Add(new { employee = employeeName, scansPerHour = Math.Round(scansPerHour, 0) });
            }

            topEmployees.Sort((left, right) => Convert.ToDouble(((dynamic)right).scansPerHour).CompareTo(Convert.ToDouble(((dynamic)left).scansPerHour)));
            return new
            {
                fromDate = fromDate.ToString("yyyy-MM-dd"),
                toDate = toDate.AddDays(-1).ToString("yyyy-MM-dd"),
                target = new { onTrackMinimum = target.OnTrackMinimum, aboveTargetMinimum = target.AboveTargetMinimum },
                employeeOptions,
                activeEmployees,
                totalScans,
                averageActiveHours = activeEmployees == 0 ? 0 : Math.Round(totalHours / activeEmployees, 1),
                averageScansPerHour = totalHours == 0 ? 0 : Math.Round(totalScans / totalHours, 0),
                topEmployees,
                shiftComparison = new { day = dayScans, night = nightScans },
                activityBreakdown = new { inbound, warehouse, outbound },
                employees
            };
        }

        private static string BuildEmployeeName(object firstName, object lastName, object username)
        {
            string name = (Convert.ToString(firstName) + " " + Convert.ToString(lastName)).Trim();
            return string.IsNullOrWhiteSpace(name) ? Convert.ToString(username) : name;
        }

        private sealed class EmployeeOption
        {
            public int EmployeeId { get; set; }
            public string Name { get; set; }
        }

        private sealed class KpiTarget
        {
            public double OnTrackMinimum { get; set; }
            public double AboveTargetMinimum { get; set; }
        }

        private static string FormatTime(object value)
        {
            return value == null || value == DBNull.Value ? "-" : Convert.ToDateTime(value).ToString("HH:mm");
        }

        private static string GetCookie(HttpContext context, string name)
        {
            HttpCookie cookie = context.Request.Cookies[name];
            return cookie == null ? null : cookie.Value;
        }

        private static string NormalizeFilter(string value, string allValue)
        {
            if (string.IsNullOrWhiteSpace(value) || value == allValue) return null;
            if (value == "Day Shift") return "Day";
            if (value == "Night Shift") return "Night";
            return value;
        }

        private static void Write(HttpContext context, object value, int statusCode)
        {
            context.Response.StatusCode = statusCode;
            context.Response.Write(new JavaScriptSerializer().Serialize(value));
        }
    }
}