using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
namespace LTG
{
    /// <summary>
    /// Summary description for PdfHandler
    /// </summary>
    public class PdfHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            // Get the document ID from the query string
            int documentId;
            if (!int.TryParse(context.Request.QueryString["id"], out documentId))
            {
                context.Response.StatusCode = 400; // Bad Request
                return;
            }

            // Retrieve PDF data from the database
            byte[] pdfData = GetPdfDataFromDatabase(documentId); // Implement this method

            if (pdfData != null)
            {
                context.Response.ContentType = "application/pdf";
                context.Response.AddHeader("Content-Disposition", "inline;filename=document.pdf");
                context.Response.OutputStream.Write(pdfData, 0, pdfData.Length);
                context.Response.End();
            }
            else
            {
                context.Response.StatusCode = 404; // Not Found
            }
        }

        private byte[] GetPdfDataFromDatabase(int documentId)
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from DeliveryDetails where OutSlip='" + documentId + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0 && dt.Rows[0]["DeliveryNote"] != DBNull.Value)
                    {
                        // Convert the "DeliveryNote" column value to byte[]
                        return  (byte[])dt.Rows[0]["DeliveryNote"];
                    }
                    else
                    {
                        // Handle the case where there's no data
                        return  null;
                        // Or handle accordingly, e.g., throwing an exception or returning an empty array
                    }
                }
            }
            // Fetch PDF from your database using Entity Framework or ADO.NET
           
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}