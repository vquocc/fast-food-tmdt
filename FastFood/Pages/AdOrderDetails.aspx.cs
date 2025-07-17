using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FastFood.Pages
{
    public partial class AdOrderDetails : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Request.QueryString["id"] != null)
            {
                if (Session["UserID"] == null)
                {
                    Response.Redirect("HomePage.aspx");
                }
                int orderId;
                if (int.TryParse(Request.QueryString["id"], out orderId))
                {
                    LoadOrderDetails(orderId);
                }
            }
        }
        private void LoadOrderDetails(int orderId)
        {
            string query = @"
                SELECT 
                    s.ProductName, 
                    d.Quantity, 
                    s.UnitPrice, 
                    (d.Quantity * s.UnitPrice) AS Subtotal,
                    p.Product_Image
                FROM OrderDetails d
                INNER JOIN OrderProductSnapshots s ON d.SnapshotID = s.SnapshotID
                INNER JOIN Products p ON s.ProductID = p.ProductID
                WHERE d.OrderID = @OrderID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@OrderID", orderId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                rptOrderDetails.DataSource = reader;
                rptOrderDetails.DataBind();
            }
        }
    }
}