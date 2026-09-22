using System;
using System.Configuration;
using System.Data.SqlClient;

namespace LTG
{
    public static class KpiData
    {
        public static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString; }
        }

        public static void EnsureLoginSessionTable()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                EnsureLoginSessionTable(connection);
            }
        }

        public static void EnsureLoginSessionTable(SqlConnection connection)
        {
            const string sql = @"
IF OBJECT_ID(N'dbo.UserLoginSessions', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.UserLoginSessions
    (
        SessionId uniqueidentifier NOT NULL CONSTRAINT PK_UserLoginSessions PRIMARY KEY,
        UserId int NOT NULL,
        Username nvarchar(150) NOT NULL,
        RoleName nvarchar(100) NULL,
        LoginTime datetime2(0) NOT NULL,
        LogoutTime datetime2(0) NULL,
        IpAddress nvarchar(64) NULL,
        UserAgent nvarchar(500) NULL
    );
    CREATE INDEX IX_UserLoginSessions_UserDate ON dbo.UserLoginSessions(UserId, LoginTime, LogoutTime);
END";

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        public static void EnsureKpiRoleColumn(SqlConnection connection)
        {
            const string sql = @"
IF COL_LENGTH(N'dbo.UserRoles', N'KPIDashboard') IS NULL
BEGIN
    ALTER TABLE dbo.UserRoles
    ADD KPIDashboard bit NOT NULL CONSTRAINT DF_UserRoles_KPIDashboard DEFAULT (0);
END";

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        public static void EnsureKpiTargetsTable(SqlConnection connection)
        {
            const string sql = @"
IF OBJECT_ID(N'dbo.KpiTargets', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.KpiTargets
    (
        TargetId int IDENTITY(1,1) NOT NULL CONSTRAINT PK_KpiTargets PRIMARY KEY,
        TargetName nvarchar(100) NOT NULL,
        OnTrackMinimum decimal(10,2) NOT NULL,
        AboveTargetMinimum decimal(10,2) NOT NULL,
        IsActive bit NOT NULL CONSTRAINT DF_KpiTargets_IsActive DEFAULT (1),
        UpdatedBy nvarchar(150) NULL,
        UpdatedDate datetime2(0) NOT NULL CONSTRAINT DF_KpiTargets_UpdatedDate DEFAULT (GETDATE())
    );
    INSERT INTO dbo.KpiTargets (TargetName, OnTrackMinimum, AboveTargetMinimum, IsActive, UpdatedBy)
    VALUES ('Scans per Hour', 80, 100, 1, 'System');
END";

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        public static void RecordLogin(int userId, string username, string roleName, string ipAddress, string userAgent)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand command = new SqlCommand(@"
INSERT INTO dbo.UserLoginSessions
    (SessionId, UserId, Username, RoleName, LoginTime, IpAddress, UserAgent)
VALUES
    (@SessionId, @UserId, @Username, @RoleName, GETDATE(), @IpAddress, @UserAgent);", connection))
            {
                connection.Open();
                EnsureLoginSessionTable(connection);
                command.Parameters.AddWithValue("@SessionId", Guid.NewGuid());
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@Username", username ?? string.Empty);
                command.Parameters.AddWithValue("@RoleName", (object)roleName ?? DBNull.Value);
                command.Parameters.AddWithValue("@IpAddress", (object)ipAddress ?? DBNull.Value);
                command.Parameters.AddWithValue("@UserAgent", (object)userAgent ?? DBNull.Value);
                command.ExecuteNonQuery();
            }
        }

        public static void RecordLogout(int userId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                EnsureLoginSessionTable(connection);

                using (SqlCommand command = new SqlCommand(@"
;WITH OpenSession AS
(
    SELECT TOP (1) SessionId
    FROM dbo.UserLoginSessions
    WHERE UserId = @UserId AND LogoutTime IS NULL
    ORDER BY LoginTime DESC
)
UPDATE dbo.UserLoginSessions
SET LogoutTime = GETDATE()
WHERE SessionId IN (SELECT SessionId FROM OpenSession);", connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}