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
            string documentId= context.Request.QueryString["id"].ToString();
            
           var source = "1";
            if (context.Request.QueryString["source"] != null)
            {
                source = context.Request.QueryString["source"].ToString();
            }
            // Retrieve PDF data from the database
            if (source == "1")
            {
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
            else if (source == "3")
            {
                byte[] pdfData = GetStockConfirmDetails(documentId); // Implement this method

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
            else
            {
                byte[] pdfData = GetGRNDetails(documentId); // Implement this method

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
        }

        private byte[] GetPdfDataFromDatabase(string documentId)
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
        private byte[] GetStockConfirmDetails(string documentId)
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select * from StockConfirmReport where Id='" + documentId + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0 && dt.Rows[0]["Documents"] != DBNull.Value)
                    {
                        string base64String = dt.Rows[0]["Documents"].ToString();
                        byte[] bytes = Convert.FromBase64String(base64String);
                        // Convert the "DeliveryNote" column value to byte[]
                        return bytes;
                    }
                    else
                    {
                        // Handle the case where there's no data
                        return null;
                        // Or handle accordingly, e.g., throwing an exception or returning an empty array
                    }
                }
            }
            // Fetch PDF from your database using Entity Framework or ADO.NET

        }
        private byte[] GetGRNDetails(string GRN)
        {
            string constr = ConfigurationManager.ConnectionStrings["LTGConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string qry = "Select Barcode from GRNBarcode where GRN='" + GRN + "'";
                SqlCommand cmd1 = new SqlCommand(qry, con);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0 && dt.Rows[0]["Barcode"] != DBNull.Value)
                    {
                        // Convert the "DeliveryNote" column value to byte[]
                        return (byte[])dt.Rows[0]["Barcode"];
                    }
                    else
                    {
                        // Handle the case where there's no data
                        return null;
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